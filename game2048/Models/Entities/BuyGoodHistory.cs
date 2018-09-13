using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Models.Enum;

namespace Models.Entities
{
    [Table("Game_BuyGoodHistory")]
   public class BuyGoodHistory:Entity
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public BuyTypeEnum Type { get; set; }

        /// <summary>
        /// MallId为0则表示代表金币
        /// </summary>
        public int  MallId { get; set; }
        public int Num { get; set; }

        /// <summary>
        /// 有没有收到
        /// </summary>
        public bool HaveGet { get; set; } = true;

        public DateTime CreateDate { get; set; }
    }
}
