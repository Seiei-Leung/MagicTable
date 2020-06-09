using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Common
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class ConvertHandler
    {
        /// <summary>

        /// 日期：2012-10-05
        /// 通过配置参数和字典组装List
        /// </summary>
        /// <param name="paramStr">配置参数</param>
        /// <param name="dic">字典</param>
        /// <returns>List</returns>
        public static List<DataRow> ToDataRowList(string paramStr, Dictionary<string, BindingSource> dic)
        {
            string[] vars = paramStr.Split(new char[] { ';' });
            List<DataRow> listRow = new List<DataRow>();
            for (int i = 0; i < vars.Length; i++)
            {
                listRow.Add(TableHandler.GetCurrentDataRow(dic[StringHandler.GetStrAfter(vars[i].Trim(), "-")]));
            }
            return listRow;
        }

        /// <summary>

        /// 日期：2012-10-05
        /// 通过配置参数和字典组装List
        /// </summary>
        /// <param name="paramStr">配置参数</param>
        /// <param name="dic">字典</param>
        /// <returns>List</returns>
        public static List<DataRow> ToDataRowList_new(string paramStr, Dictionary<string, BindingSource> dic)
        {
            paramStr = paramStr.Replace("|", ";");
            string[] vars = paramStr.Split(new char[] { ';' });
            List<DataRow> listRow = new List<DataRow>();
            for (int i = 0; i < vars.Length; i++)
            {
                listRow.Add(TableHandler.GetCurrentDataRow(dic[StringHandler.GetStrAfter(vars[i].Trim(), "-")]));
            }
            return listRow;
        }

        /// <summary>

        /// 日期：2012-10-05
        /// 通过配置参数和字典组装List
        /// </summary>
        /// <param name="paramStr">配置参数</param>
        /// <param name="dic">字典</param>
        /// <returns>List</returns>
        public static List<DataRow> ToDataRowList_new(string paramStr, Dictionary<string, BindingSource> dic, DataRow MasterRow)
        {
            paramStr = paramStr.Replace("|", ";");
            string[] vars = paramStr.Split(new char[] { ';' });
            List<DataRow> listRow = new List<DataRow>();
            for (int i = 0; i < vars.Length; i++)
            {
                if (vars[i].EndsWith("-M"))
                    listRow.Add(MasterRow);
                else
                    listRow.Add(TableHandler.GetCurrentDataRow(dic[StringHandler.GetStrAfter(vars[i].Trim(), "-")]));
            }
            return listRow;
        }

        /// <summary>

        /// 日期：2012-10-05
        /// 通过配置参数和字典组装List
        /// </summary>
        /// <param name="paramStr">配置参数</param>
        /// <param name="dic">字典</param>
        /// <returns>List</returns>
        public static List<DataRow> ToDataRowList_Row(DataRow[] drB, Dictionary<string, BindingSource> dic)
        {
            List<DataRow> listRow = new List<DataRow>();
            for (int i = 0; i < drB.Length; i++)
            {
                string[] vars = drB[i]["DetailSqlVals"].ToString().Trim().Split(new char[] { ';' });
                for (int j = 0; j < vars.Length; j++)
                    listRow.Add(TableHandler.GetCurrentDataRow(dic[StringHandler.GetStrAfter(vars[j].Trim(), "-")]));
            }
            return listRow;
        }

        /// <summary>

        /// 日期：2012-10-05
        /// 通过配置参数和字典组装List
        /// </summary>
        /// <param name="paramStr">配置参数</param>
        /// <param name="dic">字典</param>
        /// <returns>List</returns>
        public static bool ToDataRowList_Row_Var(List<DataRow> rowResult, DataRow[] drB, Dictionary<string, BindingSource> dic, Dictionary<string, object> dicVars)
        {
            bool changed = false;
            //List<DataRow> listRow = new List<DataRow>();
            for (int i = 0; i < drB.Length; i++)
            {
                string pageCaption = Convert.ToString(drB[i]["PageCaption"]);
                string[] vars = drB[i]["DetailSqlVals"].ToString().Trim().Split(new char[] { ';' });
                for (int j = 0; j < vars.Length; j++)
                {
                    DataRow thisRow = TableHandler.GetCurrentDataRow(dic[StringHandler.GetStrAfter(vars[j].Trim(), "-")]);
                    if (thisRow == null)
                        thisRow = (dic[StringHandler.GetStrAfter(vars[j].Trim(), "-")].DataSource as DataTable).NewRow();
                    string varName = StringHandler.GetStrBefore(vars[j].Trim(), "-");
                    rowResult.Add(thisRow);
                    string captionVar = pageCaption + "-" + vars[j].Trim();
                    if (!dicVars.Keys.Contains(captionVar))
                    {
                        dicVars.Add(captionVar, thisRow[varName]);
                        changed = true;
                    }
                    else
                    {
                        if (dicVars[captionVar].ToString() != thisRow[varName].ToString())
                        {
                            dicVars[captionVar] = thisRow[varName];
                            changed = true;
                        }
                    }
                }
            }
            return changed;
        }


        /// <summary>

        /// 转换日期格式
        /// 日期：2012-10-18
        /// </summary>
        /// <param name="dtDate">日期</param>
        /// <returns>格式化后的日期字符串（yyyy-MM-dd）</returns>
        public static string ConvetDateFormate(DateTime dtDate)
        {

            if (dtDate != null)
                return dtDate.ToString("yyyy-MM-dd");
            else
                return "";
        }




        /// <summary>

        /// 将Color转换为HtmlColor并填入制定行、字段
        /// 日期：2012-10-18
        /// </summary>
        /// <param name="row">要填入的行</param>
        /// <param name="colorfieldname">要填入的字段</param>
        public static void ConvetToHtmlColor(DataRow row, string colorfieldname)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Color c = dlg.Color;
                string colorName = System.Drawing.ColorTranslator.ToHtml(c);
                row[colorfieldname] = colorName;
            }
        }

        /// <summary>

        /// <para>byte[]转换为Image</para>
        /// <para>日期：2012-12-17</para>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] bytes)
        {
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                Image img = Image.FromStream(ms);
                ms.Close();
                return img;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>

        /// <para>Image转换为byte[]</para>
        /// <para>日期：2012-12-17</para>
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, (object)image);
            byte[] b = ms.ToArray();
            ms.Close();
            return b;
        }

        /// <summary>

        /// <para>Image转换为byte[]</para>
        /// <para>日期：2012-12-17</para>
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image, ImageFormat imageFormat)
        {
            MemoryStream stream = new System.IO.MemoryStream();
            Bitmap img = new Bitmap(image);
            img.Save(stream, imageFormat);
            byte[] bytes = stream.GetBuffer();
            img.Dispose();
            stream.Close();
            return bytes;
        }
        /// <summary>

        /// 转换日期格式
        /// 日期：2012-10-18
        /// </summary>
        /// <param name="dtDate">日期</param>
        /// <returns>格式化后的日期字符串（yyyy-MM-dd HH:mm:ss）</returns>
        public static string ConvetDateFormate2(DateTime dtDate)
        {

            if (dtDate != null)
                return dtDate.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return "";
        }

        public static bool ToBoolean(object value)
        {
            if (value is DBNull)
                return false;
            else
                return Convert.ToBoolean(value);
        }

        public static int ToInt32(object value)
        {
            if (value is DBNull || value.ToString() == "")
                return 0;
            else
                return Convert.ToInt32(value);
        }

        public static Double ToDouble(object value)
        {
            if (value is DBNull || value.ToString() == "")
                return 0;
            else
                return Convert.ToDouble(value);
        }

        public static decimal ToDecimal(object value)
        {
            try
            {
                if (value is DBNull || value.ToString() == "")
                    return 0;
                else
                    return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }

        }

        ///   <summary> 
        ///   得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 
        ///   </summary> 
        ///   <param   name="CnChar">单个汉字</param> 
        ///   <returns>单个大写字母</returns> 
        public static string GetCharSpellCode(string CnChar)
        {
            string CnCharNew = CnChar.Replace("/", "");
            CnCharNew = CnCharNew.Replace("~", "");
            CnCharNew = CnCharNew.Replace("!", "");
            CnCharNew = CnCharNew.Replace("@", "");
            CnCharNew = CnCharNew.Replace("#", "");
            CnCharNew = CnCharNew.Replace("$", "");
            CnCharNew = CnCharNew.Replace("%", "");
            CnCharNew = CnCharNew.Replace("^", "");
            CnCharNew = CnCharNew.Replace("&", "");
            CnCharNew = CnCharNew.Replace("*", "");
            CnCharNew = CnCharNew.Replace("(", "");
            CnCharNew = CnCharNew.Replace(")", "");
            string returnStr = "";
            long iCnChar;
            string[] cnChar = new string[CnCharNew.Length];
            for (var i = 0; i < CnCharNew.Length; i++)
            {
                cnChar[i] = CnCharNew.Substring(i, 1);
                byte[] ZW = System.Text.Encoding.Default.GetBytes(cnChar[i]);

                //如果是字母，则直接返回 
                if (ZW.Length == 1)
                {
                    return cnChar[i].ToUpper();
                }
                else
                {
                    //   get   the     array   of   byte   from   the   single   char    
                    int i1 = (short)(ZW[0]);
                    int i2 = (short)(ZW[1]);
                    iCnChar = i1 * 256 + i2;
                }

                if ((iCnChar >= 45217) && (iCnChar <= 45252))
                {
                    returnStr = returnStr + "A";
                }
                else if ((iCnChar >= 45253) && (iCnChar <= 45760))
                {
                    returnStr = returnStr + "B";
                }
                else if ((iCnChar >= 45761) && (iCnChar <= 46317))
                {
                    returnStr = returnStr + "C";
                }
                else if ((iCnChar >= 46318) && (iCnChar <= 46825))
                {
                    returnStr = returnStr + "D";
                }
                else if ((iCnChar >= 46826) && (iCnChar <= 47009))
                {
                    returnStr = returnStr + "E";
                }
                else if ((iCnChar >= 47010) && (iCnChar <= 47296))
                {
                    returnStr = returnStr + "F";
                }
                else if ((iCnChar >= 47297) && (iCnChar <= 47613))
                {
                    returnStr = returnStr + "G";
                }
                else if ((iCnChar >= 47614) && (iCnChar <= 48118))
                {
                    returnStr = returnStr + "H";
                }
                else if ((iCnChar >= 48119) && (iCnChar <= 49061))
                {
                    returnStr = returnStr + "J";
                }
                else if ((iCnChar >= 49062) && (iCnChar <= 49323))
                {
                    returnStr = returnStr + "K";
                }
                else if ((iCnChar >= 49324) && (iCnChar <= 49895))
                {
                    returnStr = returnStr + "L";
                }
                else if ((iCnChar >= 49896) && (iCnChar <= 50370))
                {
                    returnStr = returnStr + "M";
                }

                else if ((iCnChar >= 50371) && (iCnChar <= 50613))
                {
                    returnStr = returnStr + "N";
                }
                else if ((iCnChar >= 50614) && (iCnChar <= 50621))
                {
                    returnStr = returnStr + "O";
                }
                else if ((iCnChar >= 50622) && (iCnChar <= 50905))
                {
                    returnStr = returnStr + "P";
                }
                else if ((iCnChar >= 50906) && (iCnChar <= 51386))
                {
                    returnStr = returnStr + "Q";
                }
                else if ((iCnChar >= 51387) && (iCnChar <= 51445))
                {
                    returnStr = returnStr + "R";
                }
                else if ((iCnChar >= 51446) && (iCnChar <= 52217))
                {
                    returnStr = returnStr + "S";
                }
                else if ((iCnChar >= 52218) && (iCnChar <= 52697))
                {
                    returnStr = returnStr + "T";
                }
                else if ((iCnChar >= 52698) && (iCnChar <= 52979))
                {
                    returnStr = returnStr + "W";
                }
                else if ((iCnChar >= 52980) && (iCnChar <= 53640))
                {
                    returnStr = returnStr + "X";
                }
                else if ((iCnChar >= 53689) && (iCnChar <= 54480))
                {
                    returnStr = returnStr + "Y";
                }
                else if ((iCnChar >= 54481) && (iCnChar <= 55289))
                {
                    returnStr = returnStr + "Z";
                }
                else returnStr = returnStr + "?";
            }
            return returnStr;
        }

        /// <summary>
        /// 转换为简体中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSimplifiedChinese(string str)
        {
            //return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese);
            return ToSimplified(str);
        }

        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTraditionalChinese(string str)
        {
            //return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.TraditionalChinese);
            return ToTraditional(str);
        }


        /// <summary>
        /// 中文字符工具类
        /// </summary>
        private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 将字符转换成简体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        private static string ToSimplified(string source)
        {
            String target = new String(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, source, source.Length, target, source.Length);
            return target;
        }

        /// <summary>
        /// 讲字符转换为繁体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        private static string ToTraditional(string source)
        {
            String target = new String(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, source, source.Length, target, source.Length);
            return target;
        }

    }
}
