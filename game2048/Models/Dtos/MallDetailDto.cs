using Models.Enum;

namespace Models.Dtos
{
  public  class MallDetailDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public MallGoodsTypeEnum Type { get; set; }
    }
}
