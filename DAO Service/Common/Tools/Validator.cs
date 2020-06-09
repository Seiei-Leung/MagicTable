using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public static class Validator
    {

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 判断输入的字符串是否日期格式
        /// </summary>
        /// <param name="strDate">日期字符串</param>
        /// <returns>true:日期格式；false:不是日期格式</returns>
        public static bool IsDate(string strDate)
        {
            try
            {
                if (strDate.Length < 8) return false;

                Convert.ToDateTime(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-29
        ///  判断字符串是否数字格式(金额)
        /// </summary>
        /// <param name="sInput">输入字符串</param>
        /// <returns>布尔值（true:是金额格式；false:不是金额格式）</returns>
        public static bool IsNumber(string sInput)
        {
            string reg = @"^([1-9]\d+|[0-9])(\.\d\d?)?$";

            if (sInput.Trim() == "") return false;

            try
            {
                if (Regex.IsMatch(sInput, reg))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-23</para>
        /// <para>判断对象是否为布尔值</para>
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>是返回真,否返回假</returns>
        public static bool IsBoolean(object value)
        {
            bool b;
            return bool.TryParse(value.ToString(), out b);
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-10-26
        /// 判断输入的字符串能否转换为Color
        /// </summary>
        /// <param name="htmlColor">html颜色字符串</param>
        public static bool IsHtmlColor(string htmlColor)
        {
            try
            {
                System.Drawing.ColorTranslator.FromHtml(htmlColor);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// (?=.*[0-9])                     #必须包含数字
        /// ?=.*[a-zA-Z])                  #必须包含小写或大写字母
        /// (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
        /// .{8,30}                         #至少8个字符，最多30个字符
        /// </summary>
        /// <param name="pw">密码</param>
        /// <param name="ContainsNum">包含数字</param>
        /// <param name="ContainsLetters">包含字母</param>
        /// <param name="ContainsSpeciaChar">包含特殊字符</param>
        /// <param name="MinLen">最小长度</param>
        /// <param name="MaxLen">最大长度</param>
        /// <returns></returns>
        public static bool IsValidPassWord(string pw, bool ContainsNum, bool ContainsLetters, bool ContainsSpeciaChar, int MinLen, int MaxLen, out string msg)
        {
            msg = "";
            string regexStr = "";
            if (ContainsNum)
            {
                msg += "数字 ";
                regexStr += @"(?=.*[0-9])";
            }
            if (ContainsLetters)
            {
                msg += "字母 ";
                regexStr += @"(?=.*[a-zA-Z])";
            }
            if (ContainsSpeciaChar)
            {
                msg += "特殊字符 ";
                regexStr += @"(?=([\x21-\x7e]+)[^a-zA-Z0-9])";
            }
            regexStr += @".{"+MinLen+","+MaxLen+"}";
            
            if (msg != "")
                msg = "密码必须由：" + msg + "组成,\r\n且长度为："+MinLen+"~"+MaxLen+"之间!";
            Regex regex = new Regex(regexStr, RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return regex.IsMatch(pw);

//            var regex = new Regex(@"
//            (?=.*[0-9])
//            (?=.*[a-zA-Z])
//            (?=([\x21-\x7e]+)[^a-zA-Z0-9])
//            .{8,30}
//            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
//            return regex.IsMatch(pw);
        }

            
    }
}
