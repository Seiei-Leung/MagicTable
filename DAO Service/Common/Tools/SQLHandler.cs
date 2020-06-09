using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// SQL语句处理 
    /// </summary>
    public static class SQLHandler
    {

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-10-06
        /// 替换SQL语句敏感字符
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>替换后的SQL语句</returns>
        public static string SecuritySQL(string strSQL)
        {          
              strSQL = Regex.Replace(strSQL, @"[']+", "");
              //strSQL = Regex.Replace(strSQL, @"[--]+", "");
              strSQL = Regex.Replace(strSQL, "--", "");
            
              return strSQL;
        }
    }
}
