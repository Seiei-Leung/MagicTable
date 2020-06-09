using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class ConvertHandler
    {
        public static bool ToBoolean(object value)
        {
            if (value is DBNull)
                return false;
            else
                return Convert.ToBoolean(value);
        }


    }    
}
