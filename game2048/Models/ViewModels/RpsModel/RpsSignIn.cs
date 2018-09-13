using Models.Dtos;
using Models.Enum;

namespace Models.ViewModels.RpsModel
{
  public  class RpsSignIn
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public MallGoodsTypeEnum GoodsType{ get; set; }

        public MallDetailDto MallDetail { get; set; }
        public int Num { get; set; }
    }
}
