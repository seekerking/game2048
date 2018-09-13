using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Game_InviteHistory")]
   public class InviteHistory:Entity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Data { get; set; }
        public int UserId { get; set; }

        public int InvitedUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsGet { get; set; }
        public bool HaveReward { get; set; }


        [Timestamp]
        public byte[] TimeStamp { get; set; }

        [ForeignKey("InvitedUserId")]
        public virtual User User { get; set; }

    }
}
