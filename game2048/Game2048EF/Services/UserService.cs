using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game2048EF.Repository.Interfaces;
using Game2048EF.Services.Interfaces;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Services
{
  public  class UserService:IUserService
  {
      private readonly IUserReppository _userReppository;

      public UserService(IUserReppository userReppository)
      {
          _userReppository = userReppository;
      }

        public async Task<RpsData<RpsUser>> AddOrUpdate(ParamUserInfo param)
        {
            return await _userReppository.AddOrUpdate(param);
        }

        public async Task<RpsData<IEnumerable<RpsSignList>>> GetSignList(ParamUserBase param)
        {
            return await _userReppository.GetSignList(param);
        }

        public async Task<RpsData<RpsUser>> GetUserInfo(ParamUserInfo param)
        {
            return await _userReppository.GetUserInfo(param);
        }

        public async Task<RpsData<RpsMyGoldCoinInfo>> MyGoldCoinInfo(ParamUserGoldCoin param)
        {
            return await _userReppository.MyGoldCoinInfo(param);
        }

        public async Task<RpsData<RpsSignIn>> UserSignIn(ParamUserBase param)
        {
            return await _userReppository.UserSignIn(param);
        }

    }
}
