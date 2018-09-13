using Models.Enum;

namespace Models.ViewModels.RpsModel
{
  public  class RpsMallDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MallGoodsTypeEnum Type { get; set; }

        public MallTypeEnum MallType { get; set; }

        public int LimitedTime { get; set; }

        public int LimitedPeriod { get; set; }

        public bool CanBuy { get; set; } = true;

        public int Num { get; set; }

        public int RewardNum { get; set; }

        public int Price { get; set; }
        public string Descroption { get; set; }

        public string Icon { get; set; }
    }
}
