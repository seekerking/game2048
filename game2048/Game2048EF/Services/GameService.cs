using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Game2048EF.Repository.Interfaces;
using Game2048EF.Services.Interfaces;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Services
{
  public  class GameService:IGameService
  {
      private readonly IGameRepository _gameRepository;

      public GameService(IGameRepository gameRepository)
      {
          _gameRepository = gameRepository;
      }

        public async Task<RpsData<IEnumerable<RpsUserGood>>> GetUserGoods(ParamUserBase param)
        {
            return await _gameRepository.GetUserGoods(param);
        }

      public async Task<RpsData<IEnumerable<RpsMallDetail>>> GetMallDetails(ParamMallDetails param)
      {
          return await _gameRepository.GetMallDetails(param);
      }

        public async Task<RpsData<bool>> UseGoods(ParamUseGoods param)
        {
            return await _gameRepository.UseGoods(param);
        }

        public async Task<RpsData<RpsBuyGood>> BuyGoods(ParamBuyGoods param)
        {
            return await _gameRepository.BuyGoods(param);
        }

        public async Task<RpsData<RpsRankingList>> GetRankingList(ParamRankingList param)
        {
            return await _gameRepository.GetRankingList(param);
        }

        public async Task<RpsData<bool>> UploadGameScore(ParamUserScore param)
        {
            return await _gameRepository.UploadGameScore(param);
        }

        public async Task<RpsData<int[]>> GetUserPassGates(ParamUserBase param)
        {
            return await _gameRepository.GetUserPassGates(param);
        }
    }
}
