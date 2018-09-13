using System.Collections.Generic;
using System.Threading.Tasks;
using Game2048Api.Extensions;
using Game2048EF.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Dtos;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048Api.Controllers
{
    [Route("api/other/[action]")]
    public class OtherController : BaseController
    {

        private readonly IOtherService _otherService;

        public OtherController(ILogger<OtherController> logger, IOtherService otherService) : base(logger)
        {
            _otherService = otherService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult GetInviteCode()
        {
            return ActionWrap(() =>
           {
               ResultData<RpsData<string>> result = new ResultData<RpsData<string>>();
               var param = HttpContext.User.GetUserBase();
               result.Data = _otherService.GetInviteCode(param);
               return result;

           });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetInvitedUser()
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsInvited>> result = new ResultData<RpsData<RpsInvited>>();
                var param = HttpContext.User.GetUserBase();
                result.Data = await _otherService.GetInvitedUsers(param);
                return result;

            });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetInvitedReward([FromBody]ParamGetInviteReward param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<IEnumerable<RewardDataDto>>> result = new ResultData<RpsData<IEnumerable<RewardDataDto>>>();
                var parameter = HttpContext.User.GetUserBase<ParamGetInviteReward>(param);
                result.Data = await _otherService.GetInvitedReward(parameter);
                return result;

            });

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadAdInfo([FromBody]ParamAdInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsAddWatch>> result = new ResultData<RpsData<RpsAddWatch>>();
                result.Data = await _otherService.UploadAdInfo(HttpContext.User.GetUserBase());
                return result;

            });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUserPoster()
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<IEnumerable<RpsPoster>>> result = new ResultData<RpsData<IEnumerable<RpsPoster>>>();
                var param = HttpContext.User.GetUserBase();
                result.Data = await _otherService.GetUserPoster(param);
                return result;

            });

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUserPosterDetail([FromBody]ParamUserPosterDetail param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<IEnumerable<RpsPosterDetail>>> result = new ResultData<RpsData<IEnumerable<RpsPosterDetail>>>();
                param = HttpContext.User.GetUserBase<ParamUserPosterDetail>(param);
                result.Data = await _otherService.GetUserPosterDetail(param);
                return result;

            });

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetPoster([FromBody]ParamGetPoster param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsUserGetPoster>> result = new ResultData<RpsData<RpsUserGetPoster>>();
                param = HttpContext.User.GetUserBase<ParamGetPoster>(param);
                result.Data = await _otherService.GetPoster(param);
                return result;

            });

        }
    }
}
