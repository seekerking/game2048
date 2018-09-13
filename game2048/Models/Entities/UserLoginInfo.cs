using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("Game_UserLoginInfo")]
   public class UserLoginInfo:Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }

        public int SignInContinuousDays { get; set; }

        public DateTime LatetestSignInDate { get; set; }


        [Timestamp]
        public byte[] TimeStamp { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
