using System.Collections.Generic;

namespace Models.ViewModels.RpsModel
{
   public class RpsMyGoldCoinInfo
    {

        public RpsMyGoldCoinInfo()
        {
            BuyCoinList=new List<RpsBuyGood>();
        }
        public int GoldCoins { get; set; }

        public IEnumerable<RpsBuyGood> BuyCoinList { get; set; }
    }
}
