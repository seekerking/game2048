using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Models.Common;
using Models.Enum;
using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
    public class ParamUserInfo : ParamUserBase, IValidatableObject
    {
        public string NickName { get; set; }
        public string Icon { get; set; } = string.Empty;

        public string Code { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamUserInfo result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamUserInfo>(this.Sign, SignKey);
            }
            catch
            {
                result = new ParamUserInfo();
            }

            var baseResult = this.Validate(result);
            if (baseResult != null)
            {
                yield return baseResult;
            }
            if (result.NickName != this.NickName || result.Icon != this.Icon || result.Code != this.Code)
            {
                yield return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }

        }
    }
}
