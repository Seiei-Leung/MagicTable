using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Common
{
    public static class TableHandler
    {
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 替换datatable 敏感字符
        /// </summary>
        /// <param name="strQuery">需要检查的字符</param>
        /// <returns>替换敏感字符后的字符串</returns>
        public static string ReplaceByQuery(string strQuery)
        {
            string s = string.Empty;
            s = strQuery.Replace("=", "");
            s.Replace(">", "");
            s.Replace(">=", "");
            s.Replace("<", "");
            s.Replace("<=", "");
            s.Replace("like", "");
            s.Replace("not like", "");
            s.Replace("'", "");
            s.Replace("\\", "");

            return s;
        }

        /// <summary>
        /// 获取当前DataRow
        /// </summary>
        public static DataRow GetCurrentDataRow(BindingSource bindSource)
        {
            if (!typeof(DataRowView).IsInstanceOfType(bindSource.Current))
                return null;
            DataRowView drv = (DataRowView)bindSource.Current;

            if (drv == null)
                return null;
            else
                return drv.Row;
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-10-05
        /// 判断DataTable是否有修改
        /// </summary>
        /// <param name="dt">DataTable集合</param>
        /// <returns>修改状态</returns>
        public static bool GetTablesState(List<DataTable> dts)
        {
            //bool result = false;
            foreach (DataTable dt in dts)
            {
                if (dt.GetChanges() != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-10-11
        /// 检查空值
        /// </summary>
        /// <param name="col">要检查的列</param>
        /// <param name="str">列名称</param>
        /// <returns>存在空值返回真</returns>
        public static bool NullWarning(object col, string str, Form form)
        {
            if (col.ToString() == "")
            {
                form.MsgError(str + "不能为空！");
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-10-18
        /// 逐行复制表
        /// </summary>
        /// <param name="dtSource">源数据表</param>
        /// <param name="dtTarget">目标数据表</param>
        public static void CopyTable(DataTable dtSource, DataTable dtTarget)
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                DataRow thisRow = dtTarget.NewRow();
                thisRow.ItemArray = dr.ItemArray;
                dtTarget.Rows.Add(thisRow);
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-10-18
        /// 修改DataRow的Buser,Euser,Btime,ETime
        /// </summary>
        /// <param name="thisRow">要修改的DataRow</param>
        public static void InforSave(DataRow thisRow)
        {
            if (thisRow != null)
            {
                string now = DataService.DataServer.commonProxy.GetSystemDatetime_NoH();
                if ((thisRow.RowState == DataRowState.Added || thisRow.RowState == DataRowState.Detached) && (thisRow["BUser"].ToString() == ""))
                {
                    thisRow["BUser"] = Comm._user.UserCode;
                    thisRow["BTime"] = now;
                    thisRow["EUser"] = Comm._user.UserCode;
                    thisRow["ETime"] = now;
                    if (thisRow.Table.Columns.Contains("BUserName"))
                        thisRow["BUserName"] = Comm._user.UserName;
                    if (thisRow.Table.Columns.Contains("EUserName"))
                        thisRow["EUserName"] = Comm._user.UserName;
                    if (thisRow.Table.Columns.Contains("GroupCode"))
                        thisRow["GroupCode"] = Comm._user.GroupCode;
                }
                else
                {
                    //System.TimeSpan ts = Convert.ToDateTime(DataServer.commonProxy.GetSystemDatetime()).Subtract(Convert.ToDateTime(thisRow["ETime"]));
                    //if (ts.Seconds > 1)
                    //{
                        if (thisRow.RowState == DataRowState.Modified)
                        {
                            thisRow["EUser"] = Comm._user.UserCode;
                            thisRow["ETime"] = now;
                            if (thisRow.Table.Columns.Contains("EUserName"))
                                thisRow["EUserName"] = Comm._user.UserName;
                        }                        
                    //}
                }
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2012-10-18
        /// 修改DataRow的Buser,Euser,Btime,ETime
        /// </summary>
        /// <param name="thisRow">要修改的DataRow</param>
        public static void InforSave2(DataRow thisRow, string userCode, DateTime editTime)
        {
            if (thisRow != null)
            {
                if (thisRow.RowState == DataRowState.Added || thisRow.RowState == DataRowState.Detached)
                {
                    thisRow["BUser"] = userCode;
                    thisRow["EUser"] = userCode;
                    thisRow["BTime"] = editTime;
                    thisRow["ETime"] = editTime;
                    if (thisRow.Table.Columns.Contains("BUserName"))
                        thisRow["BUserName"] = Comm._user.UserName;
                    if (thisRow.Table.Columns.Contains("EUserName"))
                        thisRow["EUserName"] = Comm._user.UserName;
                    if (thisRow.Table.Columns.Contains("GroupCode"))
                        thisRow["GroupCode"] = Comm._user.GroupCode;
                }
                else
                    if (thisRow.RowState == DataRowState.Modified)
                    {
                        thisRow["EUser"] = userCode;
                        thisRow["ETime"] = editTime;
                        if (thisRow.Table.Columns.Contains("EUserName"))
                            thisRow["EUserName"] = Comm._user.UserName;
                    }
            }
        }

        public static void UpRecord(BindingSource bs,string sno)
        {
            bool isPage = ((DataTable)bs.DataSource).Columns.Contains("PageName");
            if (bs.Count > 0)
            {
                DataRow thisRow = GetCurrentDataRow(bs);
                int currentSno = Convert.ToInt32(thisRow[sno]);
                if (currentSno == 0)
                    return;
                int thisPosition = bs.Position;
                int lastPosition = bs.Find(sno, currentSno - 1);

                thisRow[sno] = currentSno - 1;
                if (isPage)
                    thisRow["PageName"] = "tPag" + thisRow[sno].ToString();

                if (lastPosition >= 0)
                {
                    bs.Position = lastPosition;
                    DataRow afterRow = GetCurrentDataRow(bs);
                    afterRow[sno] = currentSno;
                    if (isPage)
                        afterRow["PageName"] = "tPag" + afterRow[sno].ToString();
                }
                bs.Position = thisPosition;
            }
        }

        public static void DownRecord(BindingSource bs, string sno)
        {
            bool isPage = ((DataTable)bs.DataSource).Columns.Contains("PageName");
            if (bs.Count > 0)
            {
                DataRow thisRow = GetCurrentDataRow(bs);
                int currentSno = Convert.ToInt32(thisRow[sno]);
                DataTable copyTable = GetNewDataTable((DataTable)bs.DataSource, "");
                copyTable.AcceptChanges();
                int maxSNo = copyTable.AsEnumerable().Select(t => t.Field<int>(sno)).Max();
                if (currentSno == maxSNo)
                    return;
                int thisPosition = bs.Position;
                int nextPosition = bs.Find(sno, currentSno + 1);
                thisRow[sno] = currentSno + 1;
                if (isPage)
                    thisRow["PageName"] = "tPag" + thisRow[sno].ToString();

                if (nextPosition >= 0)
                {
                    bs.Position = nextPosition;
                    DataRow afterRow = GetCurrentDataRow(bs);
                    afterRow[sno] = currentSno;
                    if (isPage)
                        afterRow["PageName"] = "tPag" + afterRow[sno].ToString();
                }
                bs.Position = thisPosition;
            }
        }

        public static void DeleteRecord(BindingSource bs, string sno, string condition, string searchKey2)
        {
            bool isPage = ((DataTable)bs.DataSource).Columns.Contains("PageName");
            DataRow dr = GetCurrentDataRow(bs);
            int delNo = Convert.ToInt32(dr[sno]);
            bs.RemoveCurrent();
            DataRow[] drs = ((DataTable)bs.DataSource).Select(condition);
            for (int i = 0; i < drs.Length; i++)
            {
                if (drs[i].RowState != DataRowState.Deleted)
                {
                    if (Convert.ToInt32(drs[i][sno]) > delNo)
                    {
                        drs[i][sno] = Convert.ToInt32(drs[i][sno]) - 1;
                        if (searchKey2 != "" && ConvertHandler.ToBoolean(drs[i]["FormulaNoLock"]) == false)
                            drs[i][searchKey2] = drs[i][sno];
                        if (isPage)
                            drs[i]["PageName"] = "tPag" + drs[i][sno].ToString();
                    }
                }
            }
        }

        public static DataRow InsertRecord(BindingSource bs, string sno, string condition, string searchKey2)
        {
            bool isPage = ((DataTable)bs.DataSource).Columns.Contains("PageName");
            DataRow dr = GetCurrentDataRow(bs);
            int insNo;
            if (bs.Count > 0)
                insNo = Convert.ToInt32(dr[sno]);
            else
                insNo = 1;
            DataRow[] drs = ((DataTable)bs.DataSource).Select(condition);
            for (int i = 0; i < drs.Length; i++)
            {
                if (drs[i].RowState != DataRowState.Deleted)
                {
                    if (Convert.ToInt32(drs[i][sno]) >= insNo)
                    {
                        drs[i][sno] = Convert.ToInt32(drs[i][sno]) + 1;
                        if (searchKey2 != "" && ConvertHandler.ToBoolean(drs[i]["FormulaNoLock"]) == false)
                            drs[i][searchKey2] = drs[i][sno];
                        if (isPage)
                            drs[i]["PageName"] = "tPag" + drs[i][sno].ToString();
                    }
                }
            }
            bs.AddNew();
            DataRow insRow = GetCurrentDataRow(bs);
            insRow[sno] = insNo;
            if (searchKey2 != "" && ConvertHandler.ToBoolean(insRow["FormulaNoLock"]) == false)
                insRow[searchKey2] = insRow[sno];
            if (isPage)
                insRow["PageName"] = "tPag" + insRow[sno].ToString();
            return insRow;
        }

        public static DataRow AppendRecord(BindingSource bs, string sno, string condition)
        {
            bool isPage = ((DataTable)bs.DataSource).Columns.Contains("PageName");
            DataTable copyTable = GetNewDataTable((DataTable)bs.DataSource, condition);
            copyTable.AcceptChanges();
            int maxSno;
            if (copyTable.Rows.Count == 0)
                maxSno = 0;
            else
                maxSno = copyTable.AsEnumerable().Select(t => t.Field<int>(sno)).Max();
            bs.AddNew();
            DataRow dr = GetCurrentDataRow(bs);
            dr[sno] = maxSno + 1;
            if (isPage)
                dr["PageName"] = "tPag" + dr[sno].ToString();
            return dr;
        }


        
        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            try
            {
                newdt = dt.Clone();
                DataRow[] dr = dt.Select(condition);
                for (int i = 0; i < dr.Length; i++)
                {
                    newdt.ImportRow((DataRow)dr[i]);
                }
                return newdt;//返回的查询结果
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return newdt;
            }
        }

        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, string condition,string sort)
        {
            DataTable newdt = new DataTable();
            try
            {
                newdt = dt.Clone();
                DataRow[] dr = dt.Select(condition, sort);
                for (int i = 0; i < dr.Length; i++)
                {
                    newdt.ImportRow((DataRow)dr[i]);
                }
                return newdt;//返回的查询结果
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return newdt;
            }
        }

        public static bool RowEqual(DataRow rowA, DataRow rowB, DataColumnCollection columns)
        {
            bool result = true;
            for (int i = 0; i < columns.Count; i++)
            {
                result &= ColumnEqual(rowA[columns[i].ColumnName], rowB[columns[i].ColumnName]);
            }
            return result;
        }

        public static bool ColumnEqual(object A, object B)
        {
            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
         

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-11-09</para>
        /// <para>按照fieldName从sourceTable中选择出不重复的行，</para> 
        /// <para>相当于select distinct fieldName1,fieldName2,,fieldNamen from sourceTable </para>
        /// </summary>
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldNames">列名数组</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldNames中指明的列</returns> 
        public static DataTable SelectDistinct(string tableName, DataTable sourceTable, string[] fieldNames)
        {
            DataTable dt = new DataTable(tableName);
            object[] values = new object[fieldNames.Length];
            string fields = "";
            for (int i = 0; i < fieldNames.Length; i++)
            {
                dt.Columns.Add(fieldNames[i], sourceTable.Columns[fieldNames[i]].DataType);
                fields += fieldNames[i] + ",";
            }
            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;
            for (int j = 0; j < sourceTable.Rows.Count; j++)
            {
                if (lastRow == null || !(RowEqual(lastRow, sourceTable.Rows[j], dt.Columns)))
                {
                    lastRow = sourceTable.Rows[j];
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        values[i] = sourceTable.Rows[j][fieldNames[i]];
                    }
                    dt.Rows.Add(values);
                }
            }
            return dt;
        }

        

        
        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2013-04-21
        /// 检查表列唯一
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName">多列需用分号隔开</param>
        /// <returns></returns>
        public static bool IsUnique(DataTable table, string fieldName, out string msg)
        {
            string[] fields = fieldName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            msg = "";
            IDictionary<string, int> fs = new Dictionary<string, int>();
            //foreach (DataRow row in table.Rows)
            for (int k = 0; k < table.Rows.Count; k++)
            {
                if (table.Rows[k].RowState == DataRowState.Deleted)
                    continue;
                string str = "";
                for (int i = 0; i < fields.Length; i++)
                {
                    str += Convert.ToString(table.Rows[k][fields[i]]);
                }
                if (fs.ContainsKey(str))
                {
                    fs.Clear();
                    fs = null;
                    msg = str;
                    return false;
                }
                else
                {
                    fs.Add(str, 0);
                }
            }
            return true;
        }

    }
}
