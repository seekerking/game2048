using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Infrastructure;
using Models.Common;
using Models.Enum;
using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
   public class ParamUseGoods:ParamUserBase,IValidatableObject
    {
        public MallGoodsTypeEnum Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamUseGoods result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamUseGoods>(this.Sign, SignKey);
            }
            catch
            {
                result = new ParamUseGoods();
            }
            if (result.TimeOffset != this.TimeOffset || result.Type != this.Type)
            {
                yield return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }
        }
    }
}
