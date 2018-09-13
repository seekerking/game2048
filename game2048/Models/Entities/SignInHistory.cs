using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("Game_SignInHistory")]
  public  class SignInHistory:Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Data { get; set; }

        public DateTime CreateDate { get; set; }


        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
