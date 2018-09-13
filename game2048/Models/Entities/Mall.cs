using System;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_Mall")]
  public  class Mall:Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public MallGoodsTypeEnum Type { get; set; }
        public MallTypeEnum MallType { get; set; }
        public int Num { get; set; }
        public int RewardNum { get; set; }

        public int LimitedNum { get; set; }

        public int LimitedPeriod { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
