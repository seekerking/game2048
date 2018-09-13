using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.Common
{
    public class ParamPage<T> : ParamBase where T : class
    {
        public int Size { get; set; }
        public int Index { get; set; }
        public T Filter { get; set; }
    }
}
