using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_UserScore")]
  public  class UserScore:Entity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Gate { get; set; }

        public int Score { get; set; }

        public ScoreStatusEnum Status { get; set; }

        public DateTime CreateDate { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
