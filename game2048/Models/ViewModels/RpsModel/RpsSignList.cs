using System;

namespace Models.ViewModels.RpsModel
{
  public  class RpsSignList
    {
        public int SignHisId { get; set; }
        public long Day { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsGet { get; set; }
    }
}
