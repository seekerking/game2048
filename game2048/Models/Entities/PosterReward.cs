using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Game_PosterReward")]
    public class PosterReward
    {
        public int Id { get; set; }
        public int Gate { get; set; }
        public int Score { get; set; }
        public int Num { get; set; }
        public int PosterLevel { get; set; }
    }
}
