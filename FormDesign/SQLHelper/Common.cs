using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace SqlHandle
{
    public class Common
    {
        public static string connString = "data source=192.168.0.153;initial catalog=TWERPPLF;user id=sa;pwd=";
        static SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString);
        public static DataTable sqlToDataTable(string sql)
        {
            //conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            //conn.Close();//连接需要关闭
            return table;
        }

        public static object[,] DataTableToArray(DataTable dt)
        {
            int i = 0;
            int rowsCount = dt.Rows.Count;
            int colsCount = dt.Columns.Count;
            string[,] arrReturn = new string[rowsCount, colsCount];
            foreach (System.Data.DataRow row in dt.Rows)
            {
                int j = 0;
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    arrReturn[i, j] = row[column.ColumnName].ToString();
                    j = j + 1;
                }
                i = i + 1;
            }
            return arrReturn;
        }

        public static DataTable sqlToDataTable1(string sql)
        {
            SqlConnection conn1 = new System.Data.SqlClient.SqlConnection(connString);
            conn1.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn1;
            cmd.CommandText = sql;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            conn1.Close();//连接需要关闭
            return table;
        }

        public static System.Data.DataSet Query(string sql)
        {

            return SqlHelper.ExecuteDataset(connString, CommandType.Text, sql);

        }

        public static bool SaveDataBase(DataTable dt)
        {
            return SqlHelper.SaveToDatabase(dt);
        }

        public static DataTable Query(string sql, SqlParameter[] param)
        {
            
            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset(connString, CommandType.Text, sql, param);

            DataTable dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds.Dispose();
            return dt;
        }

        public static DataTable ExecSqlByParam(string sqlstr, string[] paramName, object[] value, string tableName)
        {
            try
            {
                //conn.Open();
                DataTable dt = null;
                if (paramName != null)
                {
                    SqlParameter[] param = new SqlParameter[paramName.Length];
                    for (int i = 0; i < paramName.Length; i++)
                        param[i] = new SqlParameter(paramName[i], value[i]);
                    dt = Query(sqlstr, param);
                }
                else
                {
                    dt = Query(sqlstr).Tables[0];
                }
                if (dt != null)
                {
                    dt.TableName = tableName;
                }
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                //conn.Close();
                string paramNameStr = string.Empty;
                string valueStr = string.Empty;
                if (paramName != null)
                {
                    for (int i = 0; i < paramName.Length; i++)
                    {
                        paramNameStr += paramName[i] + ";";
                        valueStr += Convert.ToString(value[i]) + ";";
                    }
                }

                //string str = "     查询SQL:" + sqlstr + "\r\n参数名:" + paramNameStr + "\r\n参数值:" + valueStr;
                //log.SysErrorSave(str, this, ex, UserCode, IP);

                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("msg", Type.GetType("System.String"));
                dtErr.Rows.Add(new object[] { ex.Message });
                dtErr.TableName = "异常";
                return dtErr;
            }
        }
    }
}
