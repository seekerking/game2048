using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DateExt
    {
        /// <summary>
        /// 时间转化为 yyyy-MM-dd 格式字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToYMDStr(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
    }
}
