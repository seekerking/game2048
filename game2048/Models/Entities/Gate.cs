using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    [Table("Game_Gate")]
    public  class GameGate:Entity
    {
        public int Id { get; set; }

        public int Gate { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int PassScore { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
