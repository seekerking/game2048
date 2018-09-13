using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Infrastructure;
using Models.Common;
using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
    public class ParamAdInfo : ParamUserBase, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamAdInfo result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamAdInfo>(this.Sign, SignKey);
            }
            catch
            {
                result = new ParamAdInfo();
            }

            var baseResult = this.Validate(result);
            if (baseResult != null)
            {
                yield return baseResult;
            }
        }
    }
}
