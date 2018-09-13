using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Game_PosterGallery")]
  public  class PosterGallery:Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 关卡
        /// </summary>
        public int Gate { get; set; }

        /// <summary>
        /// 图鉴等级
        /// </summary>
        public int PosterLevel { get; set; }
        /// <summary>
        /// 图鉴不同皮肤
        /// </summary>
        public int Grade { get; set; }

        public DateTime CreateDateTime { get; set; }
        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
