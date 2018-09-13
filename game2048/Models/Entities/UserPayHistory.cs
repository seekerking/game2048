using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("Game_UserPayHistory")]
   public class UserPayHistory:Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int MallId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
