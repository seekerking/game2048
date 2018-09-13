using Models.ViewModels.Common;

namespace Models.ViewModels.ParamModel
{
  public  class ParamMallDetails:ParamUserBase
    {
        /// <summary>
        /// 0全部，1道具商城,2金币商城
        /// </summary>
        public int Mode { get; set; }
    }
}
