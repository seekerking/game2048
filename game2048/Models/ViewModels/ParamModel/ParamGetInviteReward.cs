using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Models.Common;
using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
   public class ParamGetInviteReward:ParamUserBase,IValidatableObject
    {
        public bool GetAchieveReaward { get; set; }
        public int InvitedUserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParamGetInviteReward result = null;
            try
            {
                result = SecurityHelper.GetAES<ParamGetInviteReward>(this.Sign, SignKey);
            }
            catch
            {
                result = new ParamGetInviteReward();
            }
            if (result.GetAchieveReaward != this.GetAchieveReaward || result.InvitedUserId != this.InvitedUserId)
            {
                yield return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }
        }
    }
}
