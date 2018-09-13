using System.Collections.Generic;
using Models.ViewModels.RpsModel;

namespace Models.ViewModels.Common
{
    /*
     * 定义基础返回类型
     * a.无数据实体返回 只返回状态
     * b.单个实体返回
     * c.列表实体返回
     * d.分页实体返回
     */

    public class ResultBase
    {
        public int Status { get; set; }
        public string Msg { get; set; }
    }
    public class ResultData<T> : ResultBase
    {
        public T Data { get; set; }
    }



    public class ResultPageData<T> : ResultData<T> where T:RpsData<IEnumerable<T>>
    {
        public int Total { get; set; }
        public int Pagesize { get; set; }
        public int Pageindex { get; set; }
    }
}
