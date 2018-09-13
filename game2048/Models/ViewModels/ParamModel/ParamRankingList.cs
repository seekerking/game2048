using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
   public class ParamRankingList:ParamUserBase
    {
        public int Gate { get; set; }

        public int Num { get; set; } = 15;
    }
}
