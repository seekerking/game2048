using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2048EF.Extensions;
using Game2048EF.Repository.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Models.Common;
using Models.Dtos;
using Models.Entities;
using Models.Enum;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;
using Newtonsoft.Json;

namespace Game2048EF.Repository
{
    public class UserRepository : IUserReppository
    {
        private readonly GameDbContext _context;
        private readonly ConfigHelper _configHelper;

        public UserRepository(GameDbContext context, ConfigHelper configHelper)
        {
            _context = context;
            _configHelper = configHelper;
        }

        public async Task<RpsData<RpsUser>> AddOrUpdate(ParamUserInfo param)
        {

            var existEntity = param.Id > 0 ? await _context.Users.FirstOrDefaultAsync(x => x.Id == param.Id)
                : await _context.Users.FirstOrDefaultAsync(x =>
                x.OpenId == param.OpenId
                && x.GameType == param.GameType
                && x.OpenType == param.OpenType);
            var entity = existEntity != null
                ? _context.Users.Attach(existEntity).Entity
                : _context.Users.Attach(new User()).Entity;


            //App段不提示用户登录直接默认一个用户，用户可以更改重新刷新信息
            if (param.OpenType == OpenTypeEnum.系统默认 && existEntity == null)
            {
                entity.OpenId = Guid.NewGuid().ToString("N");
                entity.NickName = "游戏玩家";
                entity.Icon = _configHelper.Config.DefaultUserIcon;
            }
            else
            {
                entity.OpenId = param.OpenId;
                entity.NickName = param.NickName;
                entity.Icon = param.Icon;
            }

            entity.GameType = param.GameType;
            entity.OpenType = param.OpenType;
            entity.UnionId = param.UnionId;

            entity.UpdateDateTime = DateTime.Now;
            if (existEntity == null)
            {
                entity.CreateDateTime = DateTime.Now;
                entity.UserLoginInfo = new UserLoginInfo() { LatetestSignInDate = DateTime.MinValue, SignInContinuousDays = 0 };

                if (!string.IsNullOrEmpty(param.Code) && param.GameType == GameTypeEnum.小程序)
                {

                    var haveUsed = await _context.InviteHistories.AsNoTracking().AnyAsync(x => x.Code == param.Code);
                    if (!haveUsed)
                    {
                        var inviteDto = SecurityHelper.GetAES<InviteUserDto>(param.Code, _configHelper.Config.WXCEncryptionKey);
                        var haveInvitedCount = await _context.InviteHistories.AsNoTracking().CountAsync(x => x.UserId == inviteDto.UserId);

                        var inviteHis = new InviteHistory()
                        {
                            Code = param.Code,
                            UserId = inviteDto.UserId,
                            CreateDate = DateTime.Now
                        };

                        if (haveInvitedCount < _configHelper.Config.LimitInviteReward)
                        {
                            var inviteReward = await _context.InviteRewards.AsNoTracking()
                                .WhereIf(haveInvitedCount != _configHelper.Config.LimitInviteReward - 1, x => x.AchieveInvite == false && x.Count == haveInvitedCount + 1)
                                .WhereIf(haveInvitedCount == _configHelper.Config.LimitInviteReward - 1, x => x.AchieveInvite && x.Count == _configHelper.Config.LimitInviteReward)
                                .ToListAsync();

                            if (inviteReward.Any(x => x.AchieveInvite))
                            {
                                var items = inviteReward.Where(x => x.AchieveInvite).Select(x => new RewardDataDto()
                                {
                                    GoodsType = x.GoodsType,
                                    Num = x.Num,
                                    Type = x.Type
                                }).ToList();
                                foreach (var reward in items)
                                {
                                    _context.BuyGoodHistories.Add(new BuyGoodHistory()
                                    {
                                        CreateDate = DateTime.Now,
                                        HaveGet = false,
                                        MallId = (int)reward.GoodsType,
                                        Num = reward.Num,
                                        UserId = inviteDto.UserId,
                                        Type = reward.GoodsType > 0 ? BuyTypeEnum.购买 : BuyTypeEnum.邀请好友达标赠送
                                    });

                                }


                            }
                            if (inviteReward.Count != 0)
                            {
                                var item = inviteReward.Where(x => x.AchieveInvite == false).Select(x => new RewardDataDto()
                                {
                                    GoodsType = x.GoodsType,
                                    Num = x.Num,
                                    Type = x.Type
                                }).FirstOrDefault();
                                if (item != null)
                                {
                                    inviteHis.HaveReward = true;
                                    inviteHis.Data = JsonConvert.SerializeObject(item);
                                }

                            }

                        }

                        entity.InviteHistory = inviteHis;
                    }

                }
            }

            bool result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                return RpsData<RpsUser>.Ok(entity.MapTo<RpsUser>());
            }
            else
            {
                return RpsData<RpsUser>.Error("新增或修改失败");
            }
        }

        public async Task<RpsData<IEnumerable<RpsSignList>>> GetSignList(ParamUserBase param)
        {
            var parampters = new List<object>();
            parampters.Add(new SqlParameter("@UserId", param.Id));
            IEnumerable<RpsSignList> result = await _context.SqlQueryAsync<RpsSignList>(@"
                 select b.Id SignHisId,b.IsGet IsGet,b.CreateDate as CreateDate,ROW_NUMBER() over(order by b.CreateDate) as Day  from [dbo].[Game_UserLoginInfo] a  
                               inner join 
                               [dbo].[Game_SignInHistory] b 
                               on a.UserId=b.UserId
                         where a.UserId=@UserId 
                        and a.SignInContinuousDays!=0 
                        and DATEDIFF(day,a.LatetestSignInDate,GETDATE())<=1
                        and b.CreateDate>DATEADD(day,-a.SignInContinuousDays,a.LatetestSignInDate)", parampters);

            return RpsData<IEnumerable<RpsSignList>>.Ok(result);



        }

        public async Task<RpsData<RpsUser>> GetUserInfo(ParamUserInfo param)
        {
            var entity = param.Id > 0 ? await _context.Users.SingleAsync(x => x.Id == param.Id) :
                await _context.Users.SingleAsync(x =>
                x.OpenId == param.OpenId
                && x.GameType == param.GameType
                && x.OpenType == param.OpenType);

            return RpsData<RpsUser>.Ok(entity.MapTo<RpsUser>());
        }

        public async Task<RpsData<RpsSignIn>> UserSignIn(ParamUserBase param)
        {
            using (var tran = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("@UserId", param.Id));

                    var rows = await _context.Database.ExecuteSqlCommandAsync(@"UPDATE
                                    [dbo].[Game_UserLoginInfo]
                                SET
                                    [SignInContinuousDays] = 
                                    CASE WHEN CONVERT(varchar(100), [LatetestSignInDate],23) =CONVERT(varchar(100), DATEADD(day,-1,GETDATE()),23)  THEN ([SignInContinuousDays] + 1)%8
                                    ELSE 1 END,
									LatetestSignInDate=GETDATE()
                                WHERE
                                    [UserId] =@UserId and  CONVERT(varchar(100), [LatetestSignInDate],23)<CONVERT(varchar(100), GETDATE(),23)", parameters.ToArray());

                    var loginEntity = await _context.UserLoginInfos.SingleAsync(x => x.UserId == param.Id);
                    var reward = await _context.SignWeekRewards.AsNoTracking()
                        .Where(x => x.Day == loginEntity.SignInContinuousDays - 1).Select(x => new SignWeekReward()
                        {
                            Day = x.Day,
                            GoodsType = x.GoodsType,
                            Id = x.Id,
                            Num = x.Num,
                            Type = x.Type
                        }).FirstAsync();

                    if (rows != 0)
                    {
                        if (reward.Type == MallTypeEnum.道具商城)
                        {
                            reward.Mall = await _context.Malls.AsNoTracking().SingleAsync(x => x.MallType == MallTypeEnum.道具商城 && x.Type == reward.GoodsType);
                        }


                        var jsonReward = new RewardDataDto()
                        {
                            GoodsType = reward.GoodsType,
                            Num = reward.Num,
                            Type = reward.Type
                        };

                        _context.SignInHistories.Add(new SignInHistory()
                        {
                            Data = JsonConvert.SerializeObject(jsonReward),
                            UserId = loginEntity.UserId,
                            CreateDate = DateTime.Now
                        });

                        var goodexist =
                            await _context.UserGoods.AnyAsync(x =>
                                x.UserId == param.Id && x.GoodsType == reward.GoodsType);

                        UserGood userGood = null;
                        if (!goodexist)
                        {
                            userGood = new UserGood() { Num = 0, GoodsType = reward.GoodsType, UserId = param.Id };
                            userGood = _context.UserGoods.Attach(userGood).Entity;
                        }
                        else
                        {
                            userGood = await _context.UserGoods.SingleAsync(x => x.UserId == param.Id && x.GoodsType == reward.GoodsType);
                        }
                        userGood.Num += reward.Num;

                    }
                    bool result = await _context.SaveChangesAsync() > 0;
                    tran.Commit();
                    if (result)
                    {

                        return RpsData<RpsSignIn>.Ok(new RpsSignIn()
                        {
                            Id = loginEntity.Id,
                            Type = (int)reward.Type,
                            GoodsType = reward.GoodsType,
                            Num = reward.Num,
                            MallDetail = reward.Mall != null ? new MallDetailDto()
                            {
                                Name = reward.Mall.Name,
                                Price = reward.Mall.Price,
                                Type = (MallGoodsTypeEnum)reward.Type
                            } : null


                        });
                    }
                    else
                    {
                        return RpsData<RpsSignIn>.Error("签到异常");
                    }


                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public async Task<RpsData<RpsMyGoldCoinInfo>> MyGoldCoinInfo(ParamUserGoldCoin param)
        {
            var userCoins = await _context.UserGoods.AsNoTracking()
                .Where(x => x.UserId == param.Id && x.GoodsType == MallGoodsTypeEnum.金币).Select(x => x.Num)
                .SingleAsync();

            RpsMyGoldCoinInfo myGoldInfo = new RpsMyGoldCoinInfo() { GoldCoins = userCoins };
            if (param.ShowBuyCoinLogs)
            {
                myGoldInfo.BuyCoinList = await _context.BuyGoodHistories.AsNoTracking()
                    .Where(x => x.UserId == param.Id && x.Type == BuyTypeEnum.购买).Join(
                        _context.Malls.AsNoTracking().Where(y => y.MallType == MallTypeEnum.金币商城), x => x.MallId,
                        y => y.Id, (x, y) => new RpsBuyGood()
                        {
                            Id = x.Id,
                            Name = y.Name,
                            Num = x.Num,
                            Type = y.Type

                        }).ToListAsync();
            }

            return RpsData<RpsMyGoldCoinInfo>.Ok(myGoldInfo);
        }
    }
}
