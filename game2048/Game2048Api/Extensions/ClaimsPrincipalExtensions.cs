using System;
using System.Linq;
using System.Security.Claims;
using Models.Enum;
using Models.ViewModels.Common;

namespace Game2048Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal source)
        {
            int userId = 0;
            try
            {
                int.TryParse(source.Claims.Single(x => x.Type == "Id").Value, out userId);
                return userId;
            }
            catch
            {
                return 0;
            }
           
        }

        public static ParamUserBase GetUserBase(this ClaimsPrincipal source)
        {
            try
            {
                var userBase = new ParamUserBase();
                int userId = 0;
                int openType = 0;
                int gameType = 0;
                userId=int.Parse(source.Claims.Single(x => x.Type == "Id").Value);
                int.TryParse(source.Claims.Single(x => x.Type == "OpenType").Value, out openType);
                int.TryParse(source.Claims.Single(x => x.Type == "GameType").Value, out gameType);
                userBase.OpenId = source.Claims.Single(x => x.Type == "OpenId").Value;
                userBase.Id = userId;
                userBase.OpenType = (OpenTypeEnum)openType;
                userBase.GameType = (GameTypeEnum)gameType;
                return userBase;
            }
            catch
            {
                throw;
            }


        }

        public static T GetUserBase<T>(this ClaimsPrincipal source,T target) where  T :ParamUserBase
        {
            try
            {
               
                int userId = 0;
                int openType = 0;
                int gameType = 0;
                string openId = string.Empty;
                userId=int.Parse(source.Claims.Single(x => x.Type == "Id").Value);
                int.TryParse(source.Claims.Single(x => x.Type == "OpenType").Value, out openType);
                int.TryParse(source.Claims.Single(x => x.Type == "GameType").Value, out gameType);
                openId = source.Claims.Single(x => x.Type == "OpenId").Value;
                var props = target.GetType().GetProperties();

                foreach (var prop in props)
                {
                    if (prop.Name.Equals("Id"))
                    {
                        prop.SetValue(target,userId);
                    }
                    if (prop.Name.Equals("OpenId"))
                    {
                        prop.SetValue(target, openId);
                    }
                    if (prop.Name.Equals("OpenType"))
                    {
                        prop.SetValue(target, (OpenTypeEnum)openType);
                    }
                    if (prop.Name.Equals("GameType"))
                    {
                        prop.SetValue(target, (GameTypeEnum)gameType);
                    }

                }
                return target;

            }
            catch
            {
                throw;
            }


        }
    }
}
