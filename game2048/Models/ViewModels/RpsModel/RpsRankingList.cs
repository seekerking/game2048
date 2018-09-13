using System.Collections.Generic;

namespace Models.ViewModels.RpsModel
{
   public class RpsRankingList
    {
        public RpsRankingList()
        {
            OtherList=new List<BaseRanking>();
        }
        public IEnumerable<BaseRanking> OtherList { get; set; }

        public BaseRanking MyRanking { get; set; }
    }

    public class BaseRanking
    {
        public int Score { get; set; }

        public long Number { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }
    }
}
