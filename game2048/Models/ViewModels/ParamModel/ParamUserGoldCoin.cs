using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Models.Common;
using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
   public class ParamUserGoldCoin:ParamUserBase,IValidatableObject
    {
       public bool ShowBuyCoinLogs { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamUserGoldCoin result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamUserGoldCoin>(this.Sign, SignKey);
            }
            catch (Exception ex)
            {
                result = new ParamUserGoldCoin();
            }
            if (result.ShowBuyCoinLogs != this.ShowBuyCoinLogs || result.TimeOffset != this.TimeOffset)
            {
                yield return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }
        }
    }
}
