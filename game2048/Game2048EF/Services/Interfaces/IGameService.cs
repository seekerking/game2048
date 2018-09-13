using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Services.Interfaces
{
  public  interface IGameService
    {
        /// <summary>
        /// 获取用户道具和金币信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<IEnumerable<RpsUserGood>>> GetUserGoods(ParamUserBase param);

        /// <summary>
        /// 获取商城信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        Task<RpsData<IEnumerable<RpsMallDetail>>> GetMallDetails(ParamMallDetails param);

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<bool>> UseGoods(ParamUseGoods param);

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<RpsBuyGood>> BuyGoods(ParamBuyGoods param);

        /// <summary>
        /// 获取排行榜信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<RpsRankingList>> GetRankingList(ParamRankingList param);

        /// <summary>
        /// 上传分数，难度由前端自己进行控制
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<bool>> UploadGameScore(ParamUserScore param);

        /// <summary>
        /// 获取已通关关卡
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<int[]>> GetUserPassGates(ParamUserBase param);
    }
}
