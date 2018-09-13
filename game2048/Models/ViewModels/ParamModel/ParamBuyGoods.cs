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
   public class ParamBuyGoods:ParamUserBase,IValidatableObject
    {
        public MallTypeEnum MallType { get; set; }

        public MallGoodsTypeEnum Type { get; set; }

        public int MallId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamBuyGoods result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamBuyGoods>(this.Sign, SignKey);
            }
            catch
            {
                result = new ParamBuyGoods();
            }
            if (result.TimeOffset != this.TimeOffset || result.Type != this.Type || result.MallId != this.MallId||result.MallType!=this.MallType)
            {
                yield return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }
        }
    }
}
