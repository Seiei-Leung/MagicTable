using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Globalization;
using System.Management;
using System.Management.Instrumentation; 


namespace Common
{
    /// <summary>
    /// 字符串操作
    /// </summary>
    public static class StringHandler
    {
        public static string DesEncrypt(string strtoencrypt, string crykey)
        {

            if (crykey.Trim().Length != 8)
            {
                throw new ArgumentException("the length of the key must equal 8");
            }
            //string key = ConfigurationManager.AppSettings[crykey].ToString();
            string key = crykey;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            byte[] strbyte = Encoding.Default.GetBytes(strtoencrypt);
            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(strbyte, 0, strbyte.Length);
            cs.FlushFinalBlock();

            StringBuilder strret = new StringBuilder();
            foreach (byte item in ms.ToArray())
            {
                strret.AppendFormat("{0:X2}", item);
            }

            return strret.ToString();
        }

        public static string MD5Encoding(string s)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider  
            MD5 md5 = MD5.Create();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(s);//Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化  
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static string DesDecrypt(string strtoDecrypt, string crykey)
        {
            if (crykey.Trim().Length != 8)
            {
                throw new ArgumentException("the length of the key must equal 8");
            }
            //string key = ConfigurationManager.AppSettings[crykey].ToString();
            string key = crykey;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] strbyte = new byte[strtoDecrypt.Length / 2];

            for (int x = 0; x < strtoDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(strtoDecrypt.Substring(x * 2, 2), 16));
                strbyte[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(strbyte, 0, strbyte.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }


        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-09-25
        /// 获取字符串中某字符前的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="separate">指定分隔字符</param>
        /// <returns>字符串</returns>
        public static string GetStrBefore(string str, string separate)
        {
            string myStr = str;
            int i = myStr.IndexOf(separate);
            return myStr.Substring(0, i);
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-09-25
        /// 获取字符串中某字符的后字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="separate">指定分隔字符</param>
        /// <returns>字符串</returns>
        public static string GetStrAfter(string str, string separate)
        {
            string myStr = str;
            int i = myStr.IndexOf(separate);
            return myStr.Substring(i + 1, myStr.Length - i - 1);
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-09-25
        /// 替换SQL
        /// </summary>
        /// <param name="str">要替换的SQL</param>
        /// <param name="drs">参数值DataRow</param>
        /// <param name="fields">字段名</param>
        /// <returns>SQL</returns>
        public static string MosaicSQL(string sql, DataRow[] drs, string[] fields)
        {
            string mySql = sql;
            if ((drs.Length == 0) || (fields.Length == 0))
            {
                return mySql;
            }
            else
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    mySql = mySql.Replace("^" + fields[i], Convert.ToString(drs[i][GetStrBefore(fields[i], "-")]));
                }
                return mySql;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-11-22
        /// 创建文件夹
        /// </summary>
        /// <param name="strPath">文件夹路径</param>
        public static void CreateDirectory(string strPath)
        {
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
        }

        public static string[] TurnParamAndSql(string sql, string[] paramName, out string outSql)
        {
            outSql = sql;
            string[] outparamName = new string[paramName.Length];
            for (int i = 0; i < paramName.Length; i++)
            {
                outSql = outSql.Replace("@" + paramName[i], "@TWParam" + i.ToString());
                outparamName[i] = "TWParam" + i.ToString();
            }
            return outparamName;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-03-02
        /// 获取日历控件当前月份的第一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetSchedulerFirstDay()
        {
            string weekName = string.Empty, date = string.Empty;
            int weekIndex = 0;

            date = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-01";

            weekName = Convert.ToDateTime(date).DayOfWeek.ToString().ToLower();

            switch (weekName)
            {
                case "sunday":
                    weekIndex = 0;
                    break;
                case "monday":
                    weekIndex = 1;
                    break;
                case "tuesday":
                    weekIndex = 2;
                    break;
                case "wednesday":
                    weekIndex = 3;
                    break;
                case "thursday":
                    weekIndex = 4;
                    break;
                case "friday":
                    weekIndex = 5;
                    break;
                case "saturday":
                    weekIndex = 6;
                    break;
            }

            return Convert.ToDateTime(date).AddDays(-weekIndex).ToString("yyyy-MM-dd");
        }


        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-03-02
        /// 获取日历控件当前月份的最后一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetSchedulerLastDay()
        {
            string weekName = string.Empty, date = string.Empty;
            int weekIndex = 0;
            date = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-01";

            date = Convert.ToDateTime(date).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            weekName = Convert.ToDateTime(date).DayOfWeek.ToString().ToLower();

            switch (weekName)
            {
                case "sunday":
                    weekIndex = 6;
                    break;
                case "monday":
                    weekIndex = 1;
                    break;
                case "tuesday":
                    weekIndex = 2;
                    break;
                case "wednesday":
                    weekIndex = 3;
                    break;
                case "thursday":
                    weekIndex = 4;
                    break;
                case "friday":
                    weekIndex = 5;
                    break;
                case "saturday":
                    weekIndex = 6;
                    break;
            }

            return Convert.ToDateTime(date).AddDays(weekIndex).ToString("yyyy-MM-dd");
        }

        public static string MosaicString(string sql, string var)
        {
            if (var.Trim() != "")
            {
                string[] vars = var.Trim().Split(new char[] { ';' });
                for (int i = 0; i < vars.Length; i++)
                {
                    if (Comm.DicStrVar.Keys.Contains(vars[i]))
                    {
                        sql = sql.Replace("~" + vars[i], Comm.DicStrVar[vars[i]]);
                    }
                }
                return sql;
            }
            else
                return sql;
        }

        public static string GetSystemMosaicString(string str, string var)
        {
            string sysVar = ";GroupTreeCodes;UserId;Brands;";
            if (var.Trim() != "")
            {
                string[] vars = var.Trim().Split(new char[] { ';' });
                for (int i = 0; i < vars.Length; i++)
                {
                    if (sysVar.Contains(";" + vars[i] + ";"))
                    {
                        if (vars[i] == "GroupTreeCodes")
                            str = str.Replace("~GroupTreeCodes", Comm._user.GroupTreeCodes);
                        if (vars[i] == "UserId")
                            str = str.Replace("~UserId", Comm._user.UserId);
                        if (vars[i] == "Brands")
                            str = str.Replace("~Brands", Comm._user.Brands);
                    }
                }
                return str;
            }
            else
                return str;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2014-07-22
        /// 根据日期获取第几周
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>第几周</returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            //CultureInfo ci = new CultureInfo("zh-CN");
            
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt,CalendarWeekRule.FirstDay,DayOfWeek.Sunday);
        }

        public static string[] GetFormuleField(string formuleStr)
        {
            string str = "";
            while (formuleStr.IndexOf("@fval(") > -1)
            {
                formuleStr = formuleStr.Substring(formuleStr.IndexOf("@fval(") + 6, formuleStr.Length - (formuleStr.IndexOf("@fval(") + 6));
                int index2 = formuleStr.IndexOf(")");
                if (index2 > 0)
                {
                    str = str + formuleStr.Substring(0, index2) + ";";
                }
            }
            if (str == "")
                return null;
            else
                return str.Substring(0, str.Length - 1).Split(new char[] { ';' });

        }

        public static string GetBoardID()
        {
            string strbNumber = string.Empty; 
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_baseboard");
            foreach (ManagementObject mo in mos.Get())
            {
                strbNumber = mo["SerialNumber"].ToString();
                return strbNumber;
            }
            return strbNumber;
        }
    }
}
