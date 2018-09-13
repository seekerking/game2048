using System.ComponentModel.DataAnnotations.Schema;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_InviteReward")]
  public  class InviteReward
    {
        public int Id { get; set; }
        public MallTypeEnum Type { get; set; }
        public MallGoodsTypeEnum GoodsType { get; set; }
        public int Num { get; set; }
        public int Count { get; set; }

        public bool AchieveInvite { get; set; }
    }
}
