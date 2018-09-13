using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_UserTB")]
    public partial class User:Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OpenId { get; set; }

        public string UnionId { get; set; }
        public string NickName { get; set; }
        public string Icon { get; set; }
        public CommonStatusEnum Status { get; set; }

        public OpenTypeEnum OpenType { get; set; }

        public GameTypeEnum GameType { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }


        public virtual UserLoginInfo UserLoginInfo { get; set; }

        public virtual InviteHistory InviteHistory { get; set; }
    }
}
