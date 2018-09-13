using System;
using System.Collections.Generic;
using System.Text;
using Models.Enum;

namespace Models.ViewModels.RpsModel
{
  public  class RpsBuyGood
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Num { get; set; }

        public MallGoodsTypeEnum Type { get; set; }
    }
}
