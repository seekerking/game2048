using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Repository.Interfaces
{
   public interface IGameRepository
   {


       Task<RpsData<IEnumerable<RpsUserGood>>> GetUserGoods(ParamUserBase param);

       Task<RpsData<IEnumerable<RpsMallDetail>>> GetMallDetails(ParamMallDetails param);

       Task<RpsData<bool>> UseGoods(ParamUseGoods param);

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
