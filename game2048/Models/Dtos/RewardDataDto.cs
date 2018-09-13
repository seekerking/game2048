using Models.Enum;

namespace Models.Dtos
{
   public class RewardDataDto
    {
        public MallTypeEnum Type { get; set; }

        public MallGoodsTypeEnum GoodsType { get; set; }

        public int Num { get; set; }
    }
}
