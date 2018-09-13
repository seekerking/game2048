using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Game2048Api.Extensions;
using Game2048EF.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Common;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;
using Newtonsoft.Json;

namespace Game2048Api.Controllers
{

    [Route("api/user/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ConfigHelper _config;
    

        public UserController(IUserService userService, ConfigHelper config, ILogger<UserController> logger) : base(logger)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> UploadUserInfo([FromBody]ParamUserInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsUser>> result = new ResultData<RpsData<RpsUser>>();
                var seeionId = HttpContext.User.GetId();
                if (param.Id != 0&&param.Id!=seeionId&&seeionId!=0)
                {
                    throw new Exception("参数错误");
                }
                if (seeionId != 0 && param.Id == 0)
                {
                    param.Id = seeionId;
                }
                result.Data = await _userService.AddOrUpdate(param);

                if (result.Data.Result.Id != 0)
                {
                   result.Data.Result.Token=RequestToken(result.Data.Result);
                }
                return result;

            });
        }


        [HttpPost]
        public async Task<IActionResult> GetUserInfo([FromBody]ParamUserInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsUser>> result = new ResultData<RpsData<RpsUser>>();
                var seeionId = HttpContext.User.GetId();
                if (param.Id != 0 && param.Id != seeionId&&seeionId!=0)
                {
                    throw new Exception("参数错误");
                }
                if (seeionId != 0 && param.Id == 0)
                {
                    param.Id = seeionId;
                }
                result.Data = await _userService.GetUserInfo(param);
                if (result.Data.Result.Id != 0)
                {
                    result.Data.Result.Token = RequestToken(result.Data.Result);
                }
                return result;

            });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserSign()
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsSignIn>> result = new ResultData<RpsData<RpsSignIn>>();
                var paramUserBase = HttpContext.User.GetUserBase();
                result.Data = await _userService.UserSignIn(paramUserBase);
                return result;

            });

        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserSignList()
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<IEnumerable<RpsSignList>>> result = new ResultData<RpsData<IEnumerable<RpsSignList>>>();
                var paramUserBase = HttpContext.User.GetUserBase();
                result.Data = await _userService.GetSignList(paramUserBase);
                return result;

            });

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MyGoldCoinInfo([FromBody]ParamUserGoldCoin param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<RpsData<RpsMyGoldCoinInfo>> result = new ResultData<RpsData<RpsMyGoldCoinInfo>>();
                param = HttpContext.User.GetUserBase<ParamUserGoldCoin>(param);
                result.Data = await _userService.MyGoldCoinInfo(param);
                return result;

            });

        }

        private string RequestToken(RpsUser user)
        {
            if (user.Id != 0)
            {
                // push the user’s name into a claim, so we can identify the user later on.
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.NickName),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("OpenType", user.OpenType.ToString()),
                    new Claim("GameType", user.GameType.ToString()),
                    new Claim("OpenId", user.OpenId)
                };
                //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Config.JwtSecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);


                return new JwtSecurityTokenHandler().WriteToken(token);


            }

            return string.Empty;


        }
    }
}
