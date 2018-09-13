using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_UserGoods")]
  public  class UserGood:Entity
    {
        public int Id { get; set; }
        public MallGoodsTypeEnum GoodsType { get; set; }
        public int Num { get; set; }
        public int UserId { get; set; }
        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
