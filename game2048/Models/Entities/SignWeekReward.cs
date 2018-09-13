using System.ComponentModel.DataAnnotations.Schema;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_SignWeekReward")]
   public class SignWeekReward:Entity
    {
        public int Id { get; set; }
        public MallTypeEnum  Type { get; set; }

        public MallGoodsTypeEnum GoodsType { get; set; }

        public int Num { get; set; }
        public int Day { get; set; }
        public virtual  Mall Mall { get; set; }
    }
}
