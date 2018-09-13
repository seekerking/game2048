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
    public class OtherRepository : IOtherRepository
    {
        private readonly GameDbContext _context;
        private readonly ConfigHelper _configHelper;

        public OtherRepository(GameDbContext context, ConfigHelper configHelper)
        {
            _context = context;
            _configHelper = configHelper;
        }

        public RpsData<string> GetInviteCode(ParamUserBase param)
        {
            if (param.GameType == GameTypeEnum.小程序)
            {
                InviteUserDto inviteUserDto = new InviteUserDto();
                inviteUserDto.UserId = param.Id;
                inviteUserDto.DateTimeTick = DateTime.Now.Ticks;
                var str = JsonConvert.SerializeObject(inviteUserDto);
                var result = SecurityHelper.EncryptAES(str, _configHelper.Config.WXCEncryptionKey);
                return RpsData<string>.Ok(result);
            }
            return RpsData<string>.Error("无权获取");


        }

        public async Task<RpsData<RpsInvited>> GetInvitedUsers(ParamUserBase param)
        {
            if (param.GameType == GameTypeEnum.小程序)
            {
                var rpsInvited = new RpsInvited();

                var invitedUsers = await _context.InviteHistories.AsNoTracking().Where(x => x.UserId == param.Id).Join(
                    _context.Users.AsNoTracking(),
                    x => x.InvitedUserId, y => y.Id, (x, y) => new InvitedUser()
                    {
                        Id = y.Id,
                        GameType = (int) y.GameType,
                        Icon = y.Icon,
                        NickName = y.NickName,
                        OpenId = y.OpenId,
                        HaveGet = x.IsGet,
                        OpenType = (int) y.OpenType,
                        UnionId = y.UnionId

                    }).ToListAsync();

                rpsInvited.InvitedUsers = invitedUsers;

                rpsInvited.HaveGetInviteReward = await _context.BuyGoodHistories.AnyAsync(x =>
                    x.UserId == param.Id && x.HaveGet && x.Type == BuyTypeEnum.邀请好友达标赠送);
                rpsInvited.HaveAchiedved = invitedUsers.Count >= _configHelper.Config.LimitInviteReward;
                return RpsData<RpsInvited>.Ok(rpsInvited);
            }
            return RpsData<RpsInvited>.Error("无权操作");
        }

        public async Task<RpsData<IEnumerable<RpsPoster>>> GetUserPoster(ParamUserBase param)
        {
            var parameters = new List<object>();
            parameters.Add(new SqlParameter("@UserId", param.Id));
            var result = await _context.SqlQueryAsync<RpsPoster>(@"SELECT  g.[Icon] ,p.[Gate], Count(p.[Gate]) as Count,
	                                                  (select count(Id) from [dbo].[Game_Posters] gp where gp.GateId=p.Gate and gp.UserId=@UserId) as GetCount
                                                  FROM [Game2048].[dbo].[Game_PosterGallery]  p
                                                  inner join [dbo].[Game_Gate] g on g.Gate=p.[Gate] group by g.[Icon],p.[Gate]", parameters);

            return RpsData<IEnumerable<RpsPoster>>.Ok(result);
        }


        public async Task<RpsData<IEnumerable<RpsPosterDetail>>> GetUserPosterDetail(ParamUserPosterDetail param)
        {
            var result = await _context.PosterGalleries.AsNoTracking().Where(x => x.Gate == param.Gate).GroupJoin(
                   _context.Posters.AsNoTracking().Where(x => x.UserId == param.Id), x => x.Id, y => y.PosterId, (x, y) =>
                   new RpsPosterDetail()
                   {
                       Icon = x.Icon,
                       HaveIt = y.Any(),
                       Name = x.Name,
                       PosterId = x.Id

                   }).ToListAsync();
            return RpsData<IEnumerable<RpsPosterDetail>>.Ok(result);
        }


        public async Task<RpsData<RpsUserGetPoster>> GetPoster(ParamGetPoster param)
        {
            var posterLevel = await _context.PosterRewards.AsNoTracking().SingleAsync(x => x.Gate == param.Gate && x.Score == param.Score);


            var existPoster = await _context.Posters.AsNoTracking().Where(x => x.UserId == param.Id)
                .AnyAsync(x => _context.PosterGalleries.AsNoTracking().Any(y =>
                    y.Gate == param.Gate && y.Id == x.PosterId && y.PosterLevel == posterLevel.PosterLevel));
            if (!existPoster)
            {
                var poseterGallery = await _context.PosterGalleries.Where(x =>
                    x.Gate == param.Gate && x.PosterLevel == posterLevel.PosterLevel).Select(x => new PosterGallery(){Id = x.Id,Icon = x.Icon}).SingleAsync();


                _context.Posters.Add(new Poster()
                {
                    CreateDateTime = DateTime.Now,
                    GateId = posterLevel.Gate,
                    PosterId = poseterGallery.Id,
                    UserId = param.Id
                });
                _context.BuyGoodHistories.Add(new BuyGoodHistory()
                {
                    CreateDate = DateTime.Now,
                    HaveGet = true,
                    MallId = 0,
                    Num = posterLevel.Num,
                    UserId = param.Id,
                    Type = BuyTypeEnum.解锁图鉴赠送
                });
                var userGoods = await _context.UserGoods.SingleAsync(x => x.UserId == param.Id && x.GoodsType == MallGoodsTypeEnum.金币);
                userGoods.Num += posterLevel.Num;

                bool flag = await _context.SaveChangesAsync() > 0;
                if (flag)
                {
                    return RpsData<RpsUserGetPoster>.Ok(new RpsUserGetPoster()
                    {
                        Gate = param.Gate,
                        PosterId = poseterGallery.Id,
                        Icon = poseterGallery.Icon,
                        Reward = new RewardDataDto()
                        {
                            GoodsType = MallGoodsTypeEnum.金币,
                            Num = posterLevel.Num,
                            Type = MallTypeEnum.金币商城
                        }
                    });
                }
                else
                {
                    return RpsData<RpsUserGetPoster>.Error("解锁图鉴失败");
                }
            }
            return RpsData<RpsUserGetPoster>.Error("已解锁，无需再解锁",2);

        }

        public async Task<RpsData<IEnumerable<RewardDataDto>>> GetInvitedReward(ParamGetInviteReward param)
        {
            var result = new List<RewardDataDto>();
            if (param.GetAchieveReaward)
            {
                var haveGetAchieveReward =
                    await _context.BuyGoodHistories.AnyAsync(x => !x.HaveGet && x.UserId == param.Id && x.Type == BuyTypeEnum.看广告赠送);
                if (haveGetAchieveReward)
                {
                    var acheveInviteReward = await _context.BuyGoodHistories.Where(x => !x.HaveGet && x.UserId == param.Id && x.Type == BuyTypeEnum.看广告赠送).ToListAsync();

                    var userGoods = await _context.UserGoods.Where(x => x.UserId == param.Id).ToListAsync();
                    foreach (var item in acheveInviteReward)
                    {
                        userGoods.Single(x => x.GoodsType == (MallGoodsTypeEnum)item.MallId).Num += item.Num;
                        item.HaveGet = true;
                        result.Add(new RewardDataDto()
                        {
                            GoodsType = (MallGoodsTypeEnum)item.MallId,
                            Num = item.Num,
                            Type = item.MallId > 0 ? MallTypeEnum.道具商城 : MallTypeEnum.金币商城
                        });
                    }

                }



            }
            else
            {
                var inviteHis = await _context.InviteHistories.SingleAsync(x =>
                    x.HaveReward && !x.IsGet && x.UserId == param.Id && x.InvitedUserId == param.InvitedUserId);
                inviteHis.IsGet = true;
                if (!string.IsNullOrEmpty(inviteHis.Data))
                {
                    var rewardDataDto = JsonConvert.DeserializeObject<RewardDataDto>(inviteHis.Data);

                    if (rewardDataDto != null)
                    {
                        var userGoods = await _context.UserGoods.SingleAsync(x => x.UserId == param.Id && x.GoodsType == rewardDataDto.GoodsType);
                        userGoods.Num += rewardDataDto.Num;

                        result.Add(rewardDataDto);
                    }
                }

            }

            var flag = await _context.SaveChangesAsync() > 0;
            if (flag)
            {
                return RpsData<IEnumerable<RewardDataDto>>.Ok(result);
            }
            else
            {
                return RpsData<IEnumerable<RewardDataDto>>.Error("获取邀请奖励失败");
            }

        }


        public async Task<RpsData<RpsAddWatch>> UploadAdInfo(ParamUserBase param)
        {
            _context.BuyGoodHistories.Add(new BuyGoodHistory()
            {
                CreateDate = DateTime.Now,
                MallId = 0,
                Num = _configHelper.Config.WatchAddReward,
                Type = BuyTypeEnum.看广告赠送,
                UserId = param.Id
            });

            var userGoods = await _context.UserGoods.SingleAsync(x => x.UserId == param.Id && x.GoodsType == MallGoodsTypeEnum.金币);
            userGoods.Num += _configHelper.Config.WatchAddReward;
            var result = await _context.SaveChangesAsync() > 0;
            if (result)
            {
                return RpsData<RpsAddWatch>.Ok(new RpsAddWatch() { Num = _configHelper.Config.WatchAddReward, Type = MallGoodsTypeEnum.金币 });
            }
            else
            {
                return RpsData<RpsAddWatch>.Error("看广告赠送金币失败");
            }

        }
    }
}

