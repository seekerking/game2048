using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.RpsModel
{
  public  class RpsData<T>:RpsBase
    {
        public T Result { get; set; }

        public static RpsData<T> Ok(T data)
        {

            return new RpsData<T>()
            {
                Code = 0,
                Success = true,
                ErrorMessage = string.Empty,
                Result = data
            };
        }

        public static RpsData<IEnumerable<T>> Ok(IEnumerable<T> data)
        {

            return new RpsData<IEnumerable<T>>()
            {
                Code = 0,
                Success = true,
                ErrorMessage = string.Empty,
                Result = data
            };
        }

        public  static RpsData<T> Error(string errorMessage="操作异常",int code = 1)
        {
            return new RpsData<T>()
            {
                Code = code,
                Success = false,
                Result =default(T),
                ErrorMessage = errorMessage
            };
        }
    }
}
