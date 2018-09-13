using Models.Dtos;

namespace Models.ViewModels.RpsModel
{
   public class RpsUserGetPoster
    {
        public int Gate { get; set; }
        public int PosterId { get; set; }
        public string Icon { get; set; }
        public RewardDataDto Reward { get; set; }
    }
}
