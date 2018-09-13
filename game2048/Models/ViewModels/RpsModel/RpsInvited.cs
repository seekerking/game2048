using System.Collections.Generic;

namespace Models.ViewModels.RpsModel
{
   public class RpsInvited
    {
        public RpsInvited()
        {
            InvitedUsers = new List<InvitedUser>();
        }
        public bool HaveAchiedved { get; set; }
        public bool HaveGetInviteReward { get; set; }
        public IEnumerable<InvitedUser> InvitedUsers { get; set; }
    }

    public class InvitedUser
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public string UnionId { get; set; }

        public int OpenType { get; set; }

        public int GameType { get; set; }

        public string NickName { get; set; }

        public string Icon { get; set; }
        public bool HaveGet { get; set; }
    }
}
