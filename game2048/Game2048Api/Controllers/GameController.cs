using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game2048Api.Extensions;
using Game2048EF.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048Api.Controllers
{
    [Route("api/game/[action]")]
    public class GameController:BaseController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService,ILogger<UserController> logger):base(logger)
        {
            _gameService = gameService;

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserGoods()
        {
          
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<IEnumerable<RpsUserGood>>> result = new ResultData<RpsData<IEnumerable<RpsUserGood>>>();
                var param = HttpContext.User.GetUserBase();
                result.Data = await _gameService.GetUserGoods(param);
                return result;

            });

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetMallDetails([FromBody] ParamMallDetails param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<IEnumerable<RpsMallDetail>>> result = new ResultData<RpsData<IEnumerable<RpsMallDetail>>>();
                param = HttpContext.User.GetUserBase<ParamMallDetails>(param);
                result.Data = await _gameService.GetMallDetails(param);
                return result;

            });

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UseGoods([FromBody] ParamUseGoods useGoods)
        {

            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<bool>> result = new ResultData<RpsData<bool>>();
                var param = HttpContext.User.GetUserBase(useGoods);
                result.Data = await _gameService.UseGoods(param);
                return result;

            });

        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyGoods([FromBody] ParamBuyGoods buyGoods)
        {

            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsBuyGood>> result = new ResultData<RpsData<RpsBuyGood>>();
                var param = HttpContext.User.GetUserBase(buyGoods);
                result.Data = await _gameService.BuyGoods(param);
                return result;

            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetRankingList([FromBody] ParamRankingList paramRankingList)
        {

            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsRankingList>> result = new ResultData<RpsData<RpsRankingList>>();
                var param = HttpContext.User.GetUserBase(paramRankingList);
                result.Data = await _gameService.GetRankingList(param);
                return result;

            });

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadGameScore([FromBody] ParamUserScore paramUserScore)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<bool>> result = new ResultData<RpsData<bool>>();
                var param = HttpContext.User.GetUserBase(paramUserScore);
                result.Data = await _gameService.UploadGameScore(param);
                return result;

            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserPassGates()
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<int[]>> result = new ResultData<RpsData<int[]>>();
                var param = HttpContext.User.GetUserBase();
                result.Data = await _gameService.GetUserPassGates(param);
                return result;

            });

        }
    }
}
