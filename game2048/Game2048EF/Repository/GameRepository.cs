using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Game2048EF.Extensions;
using Game2048EF.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entities;
using Models.Enum;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 预计特价商品不会太多，所以采用的子查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<RpsData<IEnumerable<RpsMallDetail>>> GetMallDetails(ParamMallDetails param)
        {
            var result = await _context.Malls.AsNoTracking()
                .WhereIf(param.Mode==1,x=>x.MallType==MallTypeEnum.道具商城)
                .WhereIf(param.Mode ==2, x => x.MallType == MallTypeEnum.金币商城)
                .Select(x=>

                    new RpsMallDetail()
                    {
                        Id = x.Id,
                        Type = x.Type,
                        MallType = x.MallType,
                        Num = x.Num,
                        Name = x.Name,
                        RewardNum = x.RewardNum,
                        LimitedTime = x.LimitedNum,
                        LimitedPeriod = x.LimitedPeriod,
                        Price = x.Price,
                        Descroption = x.Description,
                        Icon = x.Icon

                    }
                ).ToListAsync();

             var specialResult = result.Where(x => x.LimitedTime != 0);
            foreach (var item in specialResult)
            {
                var countHis = await _context.BuyGoodHistories
                    .WhereIf(item.LimitedPeriod > 1, x => x.CreateDate.Date >= DateTime.Today.AddDays(1 - item.LimitedPeriod))
                    .WhereIf(item.LimitedPeriod == 1, x => x.CreateDate.Date >= DateTime.Today)
                    .CountAsync(x => x.UserId == param.Id);
                if (countHis >=item.LimitedTime)
                {
                    item.CanBuy = false;
                }
            }


               
                
              

            return RpsData<IEnumerable<RpsMallDetail>>.Ok(result);
        }

        public async Task<RpsData<IEnumerable<RpsUserGood>>> GetUserGoods(ParamUserBase param)
        {
            var result = await _context.UserGoods.AsNoTracking().Where(x => x.UserId == param.Id)
                   .GroupJoin(_context.Malls.AsNoTracking().Where(x => x.Type != 0), x => x.GoodsType, y => y.Type, (x, y) => new RpsUserGood()
                   {
                       Id = x.Id,
                       GoodsType = x.GoodsType,
                       MallDetail = y.Select(z => new MallDetailDto() { Name = z.Name, Price = z.Price, Type = z.Type }).FirstOrDefault(),
                       Num = x.Num

                   }).ToListAsync();

            return RpsData<IEnumerable<RpsUserGood>>.Ok(result);
        }

        public async Task<RpsData<bool>> UseGoods(ParamUseGoods param)
        {

            var userGood =
                await _context.UserGoods.SingleAsync(x => x.UserId == param.Id && x.GoodsType == param.Type && x.GoodsType != MallGoodsTypeEnum.金币);

            if (userGood.Num > 0)
            {
                userGood.Num -= 1;
                _context.GoodsUseHistories.Add(new GoodsUseHistory()
                {
                    CreateDateTime = DateTime.Now,
                    GoodsType = userGood.GoodsType,
                    Num = 1,
                    UserId = userGood.UserId
                });
                var result = await _context.SaveChangesAsync() > 0;
                if (result)
                {
                    return RpsData<bool>.Ok(true);
                }
                else
                {
                    return RpsData<bool>.Error("保存出错");
                }
            }
            return RpsData<bool>.Error("数量不足，请先购买");
        }

        public async Task<RpsData<RpsBuyGood>> BuyGoods(ParamBuyGoods param)
        {
            if (param.Type == MallGoodsTypeEnum.金币)
            {
                return await BuyJbGoods(param);
            }
            else
            {
                return await BuyDjGoods(param);
            }
        }


        public async Task<RpsData<RpsRankingList>> GetRankingList(ParamRankingList param)
        {
            var parameters = new List<object>();
            parameters.Add(new SqlParameter("@UserId", param.Id));
            parameters.Add(new SqlParameter("@Gate", param.Gate));
            parameters.Add(new SqlParameter("@GameType", param.GameType));
            parameters.Add(new SqlParameter("@Take", param.Num));
            var rankinglist = await _context.SqlQueryAsync<BaseRanking>(@"select *from 
                                            (
                                            select u.Icon, u.NickName as Name,s.Score,
                                            ROW_NUMBER() OVER(order by s.Score desc) as Number 
                                            from  [dbo].[Game_UserTB] u
                                            inner join [dbo].[Game_UserScore] s on u.Id=s.UserId
                                            where u.GameType=@GameType and u.Status=0 and s.Gate=@Gate
                                             )
                                            temp where temp.Number<=@Take", parameters);

            var myrankinglist = await _context.SqlQueryAsync<BaseRanking>(@"select *from 
                                            (
                                            select u.Icon, u.NickName as Name,s.Score,u.Id,
                                            ROW_NUMBER() OVER(order by s.Score desc) as Number 
                                            from  [dbo].[Game_UserTB] u
                                            inner join [dbo].[Game_UserScore] s on u.Id=s.UserId
                                            where u.GameType=@GameType and u.Status=0 and s.Gate=@Gate
                                             )
                                            temp where temp.Id<=@UserId", parameters);


            var result = new RpsRankingList()
            {
                MyRanking = myrankinglist.FirstOrDefault(),
                OtherList = rankinglist
            };
            return RpsData<RpsRankingList>.Ok(result);




        }


        public async Task<RpsData<bool>> UploadGameScore(ParamUserScore param)
        {

            var entity = await _context.UserScores.AnyAsync(x => x.UserId == param.Id && x.Gate == param.Gate)
                ? await _context.UserScores.SingleAsync(x => x.UserId == param.Id && x.Gate == param.Gate)
                : _context.UserScores.Attach(new UserScore()
                {
                    CreateDate = DateTime.Now,
                    UserId = param.Id,
                    Gate = param.Gate,
                    Status = ScoreStatusEnum.未通关,
                    Score = param.Score
                }).Entity;
            var gameGate = await _context.GameGates.SingleAsync(x => x.Gate == param.Gate);
            if (param.Score > entity.Score)
            {
                entity.Score = param.Score;
            }
            if (entity.Score >= gameGate.PassScore)
            {
                entity.Status = ScoreStatusEnum.通关;
            }


            await _context.SaveChangesAsync();
            return RpsData<bool>.Ok(true);

        }

        public async Task<RpsData<int[]>> GetUserPassGates(ParamUserBase param)
        {

           var result=await _context.UserScores.AsNoTracking().Where(x => x.UserId == param.Id && x.Status == ScoreStatusEnum.通关)
               .Select(x=>x.Gate)
                .ToArrayAsync();

            return RpsData<int[]>.Ok(result);
        }


        /// <summary>
        /// 购买金币
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<RpsData<RpsBuyGood>> BuyJbGoods(ParamBuyGoods param)
        {

          var userPay=  _context.UserPayHistories.Add(new UserPayHistory()
            {
                CreateTime = DateTime.Now,
                MallId = param.MallId,
                Name = param.Name,
                Price = param.Price,
                Status = (int) PayStatusEnum.正在处理,
                UserId = param.Id
            });
            await _context.SaveChangesAsync();
            userPay.Entity.Status = (int) PayStatusEnum.已处理;
            userPay.Property(x => x.Status).IsModified = true;
            var mall = await _context.Malls.AsNoTracking().SingleAsync(x => x.Id == param.MallId);
            var userGoods = await _context.UserGoods
                .SingleAsync(x => x.UserId == param.Id && x.GoodsType == MallGoodsTypeEnum.金币);
            if (mall.LimitedNum != 0)
            {
                
                var countHis = await _context.BuyGoodHistories
                    .WhereIf(mall.LimitedPeriod>1,x=>x.CreateDate.Date>=DateTime.Today.AddDays(1-mall.LimitedPeriod))
                    .WhereIf(mall.LimitedPeriod==1,x=>x.CreateDate.Date>=DateTime.Today)
                    .CountAsync(x => x.UserId==param.Id);
                if (mall.LimitedNum <= countHis)
                {
                    return RpsData<RpsBuyGood>.Error("超过每日限购次数",2);
                }
            }

            userGoods.Num += mall.Num + mall.RewardNum;

            var buyEntity = _context.BuyGoodHistories.Add(new BuyGoodHistory()
            {
                MallId = param.MallId,
                CreateDate = DateTime.Now,
                Num = userGoods.Num,
                Type = BuyTypeEnum.购买,
                UserId = param.Id
            }).Entity;
            bool result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                return RpsData<RpsBuyGood>.Ok(new RpsBuyGood()
                {
                    Id = buyEntity.Id,
                    Name = mall.Name,
                    Num = userGoods.Num,
                    Type = mall.Type

                });
            }
            else
            {
                return RpsData<RpsBuyGood>.Error("购买失败");
            }
        }


        /// <summary>
        /// 购买道具
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<RpsData<RpsBuyGood>> BuyDjGoods(ParamBuyGoods param)
        {
            var mall = await _context.Malls.AsNoTracking().SingleAsync(x => x.Id == param.MallId);
            var userGoods = await _context.UserGoods.AnyAsync(x => x.UserId == param.Id && x.GoodsType == mall.Type) ?
                await _context.UserGoods
                .SingleAsync(x => x.UserId == param.Id && x.GoodsType == mall.Type) :
                _context.UserGoods.Add(new UserGood() { GoodsType = mall.Type, Num = 0, UserId = param.Id }).Entity;
            var userJB = await _context.UserGoods.AnyAsync(x => x.UserId == param.Id && x.GoodsType == MallGoodsTypeEnum.金币) ?
                await _context.UserGoods.SingleAsync(x => x.UserId == param.Id && x.GoodsType == MallGoodsTypeEnum.金币)
                : _context.UserGoods.Add(new UserGood() { GoodsType = MallGoodsTypeEnum.金币, Num = 0, UserId = param.Id }).Entity;

            if (userJB.Num < mall.Price)
            {
                return RpsData<RpsBuyGood>.Error("金币不够，请充值");
            }

            userGoods.Num += mall.Num + mall.RewardNum;
            userJB.Num -= mall.Price;
            var buyEntity = _context.BuyGoodHistories.Add(new BuyGoodHistory()
            {
                MallId = param.MallId,
                Type = BuyTypeEnum.购买,
                CreateDate = DateTime.Now,
                Num = userGoods.Num,
                UserId = param.Id
            }).Entity;
            bool result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                return RpsData<RpsBuyGood>.Ok(new RpsBuyGood()
                {
                    Id = buyEntity.Id,
                    Name = mall.Name,
                    Num = userGoods.Num,
                    Type = mall.Type

                });
            }
            else
            {
                return RpsData<RpsBuyGood>.Error("购买失败");
            }


        }

    }
}
