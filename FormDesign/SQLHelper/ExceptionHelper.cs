using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Microsoft.ApplicationBlocks.Data
{
    public class ExceptionHelper
    {
        /// <summary>
        /// 返回因“将截断字符串或二进制数据”错误，引发异常的具体异常信息
        /// </summary>
        /// <param name="rowbk"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Exception GetException(DataRow rowbk, Exception exp)
        {
            string tableName = rowbk.Table.TableName;
            string sql = "select column_name,character_octet_length from INFORMATION_SCHEMA.COLUMNS"
                            + " where table_name='" + tableName + "'"
                            + " and (data_type='varchar' or data_type='nvarchar' or data_type='char')"
                            + " and character_octet_length>0";

            using (DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConString, CommandType.Text, sql))
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtLen = ds.Tables[0];
                    dtLen.PrimaryKey = new DataColumn[] { dtLen.Columns["column_name"] };
                    foreach (DataColumn col in rowbk.Table.Columns)
                    {
                        if (col.DataType == typeof(string))
                        {
                            string str = Convert.ToString(rowbk[col.ColumnName]);
                            if (str == "")
                                continue;

                            if (!IsValidFieldContentLen(dtLen, col.ColumnName, str))
                            {
                                //return new Exception(exp.Message + "\r\n具体错误信息：\r\n[" + tableName + "] 字段：“" + col.ColumnName + "”的限定长度不能容纳值：\r\n" + str,
                                //                    exp.InnerException);
                                return new Exception("具体错误信息：\r\n[" + tableName + "] 字段：“" + col.ColumnName + "”的限定长度不能容纳值：\r\n" + str,
                                                    exp.InnerException);
                            }
                        }
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 验证字符串的长度是否在字段限定长度内
        /// </summary>
        /// <param name="dtLen"></param>
        /// <param name="fieldName"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidFieldContentLen(DataTable dtLen, string fieldName, string str)
        {
            DataRow row = dtLen.Rows.Find(fieldName);
            if (row == null)
                return true;

            return (Convert.ToInt32(row["character_octet_length"]) > GetRealLength(str));
        }

        /// <summary>
        /// 获取字符串的真实长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetRealLength(string str)
        {
            if (str == null || str.Length == 0) { return 0; }

            int l = str.Length;
            int realLen = l;

            #region 计算长度
            int clen = 0;//当前长度
            while (clen < l)
            {
                //每遇到一个中文，则将实际长度加一。
                if ((int)str[clen] > 128) { realLen++; }
                clen++;
            }
            #endregion

            return realLen;
        }
    }
}
