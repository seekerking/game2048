using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Models.Common;
using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
  public  class ParamUserScore:ParamUserBase,IValidatableObject
    {
        public int Gate { get; set; }
        public int Score { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamUserScore result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamUserScore>(this.Sign, SignKey);
            }
            catch
            {
                result = new ParamUserScore();
            }
            if (result.TimeOffset!= this.TimeOffset || result.Gate != this.Gate || result.Score != this.Score)
            {
                yield return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }
        }
    }
}
