using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Game_Posters")]
  public  class Poster:Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PosterId { get; set; }
        public int GateId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
