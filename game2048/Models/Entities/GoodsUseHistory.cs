using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_GoodsUseHistory")]
   public class GoodsUseHistory:Entity
    {
        public int Id { get; set; }
        public MallGoodsTypeEnum GoodsType { get; set; }
        public int Num { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
