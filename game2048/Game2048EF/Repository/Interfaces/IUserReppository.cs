using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Repository.Interfaces
{
  public  interface IUserReppository
  {
      /// <summary>
      /// 新增或修改用户信息
      /// </summary>
      /// <param name="param"></param>
      /// <returns></returns>
      Task<RpsData<RpsUser>> AddOrUpdate(ParamUserInfo param);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
      Task<RpsData<RpsUser>> GetUserInfo(ParamUserInfo param);

        /// <summary>
        /// 获取用户签到情况
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<RpsData<IEnumerable<RpsSignList>>> GetSignList(ParamUserBase param);

        /// <summary>
        /// 用户签到
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
      Task<RpsData<RpsSignIn>> UserSignIn(ParamUserBase param);
       /// <summary>
       /// 获取金币和购买金币记录
       /// </summary>
       /// <param name="param"></param>
       /// <returns></returns>
      Task<RpsData<RpsMyGoldCoinInfo>> MyGoldCoinInfo(ParamUserGoldCoin param);


  }
}
