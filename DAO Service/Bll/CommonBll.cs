using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Bll
{
    public class CommonBll : BaseBll
    {
        SystemLogBll log = null;
        public static Dictionary<string, DataTable> dicWarningTable_New = new Dictionary<string, DataTable>();

        internal ICommonDal commonDal;
        /// <summary>
        /// 正确
        /// </summary>
        private const string SUCCESS = "SUCCESS";

        /// <summary>
        /// 失败
        /// </summary>
        private const string FAIL = "FAIL";

        /// <summary>
        /// 保存标识
        /// </summary>
        private const string flagSave = "Save";

        /// <summary>
        /// 保存后标识
        /// </summary>
        private const string flagSaveAfter = "SaveAfter";

        /// <summary>
        /// 草稿
        /// </summary>
        private const string flagUncheck = "UnCheck";

        /// <summary>
        /// 流程中
        /// </summary>
        private const string flagCheck = "Check";
        /// <summary>
        /// 完成
        /// </summary>
        private const string flagApprove = "Approve";

        /// <summary>
        /// 作废
        /// </summary>
        private const string flagCancellation = "Cancellation";

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>Bll公共类</para>
        /// </summary>
        public CommonBll()
        {
            commonDal = base.GetDal.CommonDal;
            log = new SystemLogBll();
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>通过日期查询主档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="beginDate">搜索开始日期，查询字段:t_SYModManage_Config.SearchDate</param>
        /// <param name="endDate">搜索结束日期:t_SYModManage_Config.SearchDate</param>
        /// <returns>主档记录</returns>
        public DataTable SearchMaster(string formName, string beginDate, string endDate)
        {
            //查询主档sql 
            DataRow row = SearchMasterConfig(formName);
            if (row == null)
                return null;

            try
            {
                //查询主档内容
                string sql = Convert.ToString(row["MasterSql"]);
                string dateField = Convert.ToString(row["DateField"]);
                sql += " where datediff(day,a." + dateField + ",'" + beginDate + "')<=0" +
                    " and datediff(day,a." + dateField + ",'" + endDate + "')>=0";

                DataTable rtnTable = commonDal.GetSingle(sql);
                if (rtnTable != null)
                    rtnTable.TableName = Convert.ToString(row["MasterTableName"]);

                row.Table.Dispose();

                return rtnTable;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2012-12-11</para>
        /// <para>通过sql查询主档</para>
        /// </summary>
        /// <param name="sql">主档Sql</param>
        /// <param name="masterTableName">主档表名</param>
        /// <returns>主档记录</returns>
        public DataTable SearchMaster_Sql(string sql, string masterTableName)
        {
            try
            {
                //查询主档内容
                DataTable rtnTable = commonDal.GetSingle(sql);

                if (rtnTable == null)
                    rtnTable.TableName = Convert.ToString(masterTableName);
                return rtnTable;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2012-12-10</para>
        /// <para>通过主键查询主档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="serialno">主键</param>
        /// <returns>主档记录</returns>
        public DataTable SearchMasterBySerialno(string formName, string serialno)
        {
            try
            {
                //查询主档sql 
                DataRow row = SearchMasterConfig(formName);
                if (row == null)
                    return null;

                //查询主档内容
                string sql = "";
                sql = Convert.ToString(row["MasterSqlClick"]).Trim();
                if (sql == "")
                    sql = Convert.ToString(row["MasterSql"]).Trim();
                string masterPKField = Convert.ToString(row["MasterPKField"]);
                sql += " where a." + masterPKField + " = '" + serialno + "'";

                DataTable rtnTable = commonDal.GetSingle(sql);

                if (rtnTable != null)
                    rtnTable.TableName = Convert.ToString(row["MasterTableName"]);

                row.Table.Dispose();

                return rtnTable;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2012-12-10</para>
        /// <para>通过主键查询主档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="serialno">主键</param>
        /// <returns>主档记录</returns>
        public DataTable SearchMasterByField(string formName, string field, string fieldValue)
        {
            try
            {
                //查询主档sql 
                DataRow row = SearchMasterConfig(formName);
                if (row == null)
                    return null;

                //查询主档内容
                string sql = "";
                sql = Convert.ToString(row["MasterSqlClick"]).Trim();
                if (sql == "")
                    sql = Convert.ToString(row["MasterSql"]).Trim();
                //string masterPKField = Convert.ToString(row["MasterPKField"]);
                sql += " where a." + field + " = '" + fieldValue + "'";

                DataTable rtnTable = commonDal.GetSingle(sql);

                if (rtnTable != null)
                    rtnTable.TableName = Convert.ToString(row["MasterTableName"]);

                row.Table.Dispose();

                return rtnTable;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        public BindingSource SearchMasterBySerialnoBs(string formName, string serialno)
        {
            DataTable dtM = SearchMasterBySerialno(formName, serialno);
            BindingSource bsM = new BindingSource();
            bsM.DataSource = dtM;
            return bsM;          
        }

        public DataRow SearchMasterRow(string formName, string serialno)
        {
            DataTable dtMaster = SearchMasterBySerialno(formName, serialno);
            if (dtMaster != null && dtMaster.Rows.Count > 0)
                return dtMaster.Rows[0];
            else
                return null;
        }

        public string Getnxserialno(string serialno,DataRow drM)
        {
            string nxserialno = "";
            if (drM.Table.Columns.Contains("nxserialno"))
                nxserialno = drM["nxserialno"].ToString();
            else
                nxserialno = serialno;
            return nxserialno;
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2012-12-10</para>
        /// <para>通过主键查询单据数据表集合</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="serialno">主键</param>
        /// <returns>主档记录</returns>
        public Dictionary<string, DataTable> SearchMasterDetailTables(string formName, string serialno)
        {
            DataTable dtM = SearchMasterBySerialno(formName, serialno);
            Dictionary<string, DataTable> dicTables = new Dictionary<string, DataTable>();
            dicTables.Add("M", dtM);
            return dicTables;
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>查询从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="pKeyValue">主键值</param>
        /// <returns></returns>
        public DataTable[] SearchData(string formName, string pKeyValue)
        {
            //查询主档sql
            DataRow row = SearchMasterConfig(formName);
            if (row == null)
                return null;

            try
            {
                //配置记录里面的从档sql语句与表名
                string[] sqls = Convert.ToString(row["DetailSql"]).Split(new char[] { ';' });
                string[] DetailTableNames = Convert.ToString(row["DetailTableName"]).Split(new char[] { ';' });
                //判断是否存在sql语句
                if (sqls.Length == 0 || sqls[0].Equals(""))
                    return null;

                string[] fKeys = Convert.ToString(row["DetailFKField"]).Split(new char[] { ';' });

                row.Table.Dispose();

                DataTable[] dts = new DataTable[sqls.Length];

                string sql = "";
                DataTable dttmp = null;
                //循环加载
                for (int i = 0; i < sqls.Length; i++)
                {
                    sql = sqls[i];
                    sql += " where a." + fKeys[i] + "='" + pKeyValue + "'";
                    dttmp = commonDal.GetSingle(sql);
                    if (dttmp != null)
                    {
                        dttmp.TableName = DetailTableNames[i];
                        dts[i] = dttmp;
                    }
                }
                return dts;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }

        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>查询从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="pKeyValue">主键值</param>
        /// <returns></returns>
        public DataTable[] SearchData_New(string formName, string pKeyValue)
        {
            //查询主档sql
            DataTable dt = commonDal.GetSingle("select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and isnull(b.SearchPage,0)=0  and b.DetailSql<>''");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            try
            {
                //配置记录里面的从档sql语句与表名
                //string[] sqls = Convert.ToString(row["DetailSql"]).Split(new char[] { ';' });
                //string[] DetailTableNames = Convert.ToString(row["DetailTableName"]).Split(new char[] { ';' });
                //判断是否存在sql语句
                //if (sqls.Length == 0 || sqls[0].Equals(""))
                //return null;

                //string[] fKeys = Convert.ToString(row["DetailFKField"]).Split(new char[] { ';' });

                DataTable[] dts = new DataTable[dt.Rows.Count];

                string sql = "";
                DataTable dttmp = null;
                //循环加载
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql = dt.Rows[i]["DetailSql"].ToString();
                    sql += " where a." + dt.Rows[i]["DetailFKField"].ToString() + "='" + pKeyValue + "'";
                    dttmp = commonDal.GetSingle(sql);
                    if (dttmp != null)
                    {
                        dttmp.TableName = dt.Rows[i]["PageCaption"].ToString();
                        dts[i] = dttmp;
                    }
                }
                dt.Clear();
                dt.Dispose();
                return dts;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }

        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2013-05-22</para>
        /// <para>查询从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="MasterTable">主档table</param>
        /// <param name="defaultPKField">默认主档主键</param>
        /// <returns></returns>
        public DataTable[] SearchData_New2(string formName, DataTable MasterTable, string defaultMPKField)
        {
            //查询主档sql
            DataTable dt = commonDal.GetSingle("select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and isnull(b.SearchPage,0)=0  and b.DetailSql<>''");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            try
            {
                DataTable[] dts = new DataTable[dt.Rows.Count];

                string sql = "";
                DataTable dttmp = null;
                //循环加载
                string pkValue = "";
                string masterPKField = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    masterPKField = dt.Rows[i]["MasterPKField"].ToString();
                    if (MasterTable != null)
                    {
                        if (masterPKField == "")
                            pkValue = Convert.ToString(MasterTable.Rows[0][defaultMPKField]);
                        else
                            pkValue = Convert.ToString(MasterTable.Rows[0][masterPKField]);
                    }
                    else
                        pkValue = "null";
                    sql = dt.Rows[i]["DetailSql"].ToString();
                    sql += " where a." + dt.Rows[i]["DetailFKField"].ToString() + "='" + pkValue + "'";
                    dttmp = commonDal.GetSingle(sql);
                    if (dttmp != null)
                    {
                        dttmp.TableName = dt.Rows[i]["PageCaption"].ToString();
                        dts[i] = dttmp;
                    }
                }
                dt.Clear();
                dt.Dispose();
                return dts;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }

        }

        public DataTable[] SearchData_New3(string formName, DataTable[] dts)
        {
            //查询主档sql
            DataTable dt = commonDal.GetSingle("select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and b.DetailSql_test<>''");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            try
            {
                DataTable[] tables = new DataTable[dt.Rows.Count];

                int index_ = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //构造存储过程SQL语句
                    string sql = dt.Rows[i]["DetailSql_test"].ToString();
                    string[] paramNames = dt.Rows[i]["DetailSqlVals"].ToString().Split(new char[] { ';' });
                    object[] values = new object[paramNames.Length];
                    if (paramNames[0] != "")
                    {
                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            if (dts == null)
                                values[j] = System.DBNull.Value;
                            else
                                values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                            index_ = index_ + 1;
                        }
                        paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    }
                    //执行存储过程
                    DataTable table = ExecSqlByParam(sql, paramNames, values, dt.Rows[i]["PageCaption"].ToString());
                    tables[i] = table;
                }
                dt.Clear();
                dt.Dispose();
                return tables;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2013-05-07</para>
        /// <para>查询从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="dts">参数</param>
        /// <param name="type">0：所有从档；1：非查询从档；2：查询从档</param> 
        /// <returns></returns>
        public DataTable[] SearchData_New4(string formName, DataTable[] dts, int type)
        {
            //查询主档sql
            string sqlb = "";
            if (type == 0)
                sqlb = "select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and b.DetailSql_test<>'' order by b.PageNo";
            if (type == 1)
                sqlb = "select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and b.DetailSql_test<>'' and isnull(SearchPage,0)=0 order by b.PageNo";
            if (type == 2)
                sqlb = "select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and b.DetailSql_test<>'' and isnull(SearchPage,0)=1 order by b.PageNo";
            DataTable dt = commonDal.GetSingle(sqlb);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            try
            {
                DataTable[] tables = new DataTable[dt.Rows.Count];

                int index_ = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //构造存储过程SQL语句
                    string sql = dt.Rows[i]["DetailSql_test"].ToString();
                    string[] paramNames = dt.Rows[i]["DetailSqlVals"].ToString().Split(new char[] { ';' });
                    object[] values = new object[paramNames.Length];
                    if (paramNames[0] != "")
                    {
                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            if (dts == null)
                                values[j] = System.DBNull.Value;
                            else
                                values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                            index_ = index_ + 1;
                        }
                        paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    }
                    //执行存储过程
                    DataTable table = ExecSqlByParam(sql, paramNames, values, dt.Rows[i]["PageCaption"].ToString());
                    tables[i] = table;
                }
                dt.Clear();
                dt.Dispose();
                return tables;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>查询从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="pKeyValue">主键值</param>
        /// <returns></returns>
        public DataTable[] SearchData_Search_New(string formName, DataTable[] dts)
        {
            //查询主档sql
            DataTable dt = commonDal.GetSingle("select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and isnull(b.SearchPage,0)=1 and b.DetailSql<>''");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            try
            {
                DataTable[] tables = new DataTable[dt.Rows.Count];

                int index_ = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //构造存储过程SQL语句
                    string sql = dt.Rows[i]["DetailSql"].ToString();
                    string[] paramNames = dt.Rows[i]["DetailSqlVals"].ToString().Split(new char[] { ';' });
                    object[] values = new object[paramNames.Length];
                    if (paramNames[0] != "")
                    {
                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            if (dts == null)
                                values[j] = System.DBNull.Value;
                            else
                                values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                            index_ = index_ + 1;
                        }
                        paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    }
                    //执行存储过程
                    DataTable table = ExecSqlByParam(sql, paramNames, values, dt.Rows[i]["PageCaption"].ToString());
                    tables[i] = table;
                }
                dt.Clear();
                dt.Dispose();
                return tables;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }

        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>查询从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="pKeyValue">主键值</param>
        /// <returns></returns>
        public DataTable SearchData_Search_Caption(string formName, string pageCaption, DataTable[] dts)
        {
            //查询主档sql
            DataTable dt = commonDal.GetSingle("select * from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.classname='" + formName + "' and b.pageCaption='" + pageCaption + "' and b.DetailSql_test<>''");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            try
            {
                DataTable table = null;
                int index_ = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //构造存储过程SQL语句
                    string sql = dt.Rows[i]["DetailSql_test"].ToString();
                    string[] paramNames = dt.Rows[i]["DetailSqlVals"].ToString().Split(new char[] { ';' });
                    object[] values = new object[paramNames.Length];
                    if (paramNames[0] != "")
                    {
                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                            index_ = index_ + 1;
                        }
                        paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    }
                    //执行存储过程
                    table = ExecSqlByParam(sql, paramNames, values, dt.Rows[i]["DetailTableName"].ToString());
                }
                dt.Clear();
                dt.Dispose();
                return table;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2013-03-29</para>
        /// <para>查询嵌套从档</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="pKeyValue">参数</param>
        /// <param name="dtSysModBD">配置数据</param>
        /// <param name="isFrame">只获取框架</param>
        /// <returns></returns>
        public DataTable[] SearchData_Detail(string formName, DataTable[] dts, DataTable dtSysModBD, bool isFrame)
        {
            //查询嵌套从档sql
            //dtSysModBD = Common.Data.OpenDataSingle("select * from t_SYModManagedl_B where serialno in (select guid from t_SYModManagedl_B where serialno in (select serialno from t_SYModManage where ClassName='" + formName + "')) order by PageNo", "t_SYModManagedl_B");
            if (dtSysModBD == null || dtSysModBD.Rows.Count == 0)
                return null;

            try
            {
                DataTable[] tables = new DataTable[dtSysModBD.Rows.Count];
                //bool SearchPage = false;
                int index_ = 0;
                for (int i = 0; i < dtSysModBD.Rows.Count; i++)
                {
                    //构造存储过程SQL语
                    DataTable table;
                    //SearchPage = ConvertHandler.ToBoolean(dtSysModBD.Rows[i]["SearchPage"]);
                    //if (isFrame && !SearchPage)
                    //{
                    //    string sql = dtSysModBD.Rows[i]["DetailSql"].ToString().Trim() + " where 1<>1";
                    //    table = commonDal.GetSingle(sql);
                    //    table.TableName = dtSysModBD.Rows[i]["PageCaption"].ToString().Trim();
                    //}
                    //else
                    //{
                    string sql = dtSysModBD.Rows[i]["DetailSql_test"].ToString().Trim();
                    string[] paramNames = dtSysModBD.Rows[i]["DetailSqlVals"].ToString().Trim().Split(new char[] { ';' });
                    object[] values = new object[paramNames.Length];
                    if (paramNames[0] != "")
                    {
                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            if (isFrame)
                                values[j] = "";
                            else
                                values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                            index_ = index_ + 1;
                        }
                        paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    }
                    //执行存储过程
                    table = ExecSqlByParam(sql, paramNames, values, dtSysModBD.Rows[i]["PageCaption"].ToString().Trim());
                    //}
                    tables[i] = table;
                }
                return tables;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        public DataTable[] SearchData_Search(string formName, DataTable[] dts)
        {
            //查询主档sql
            DataRow row = SearchMasterConfig(formName);
            if (row == null)
                return null;

            try
            {
                string[] searchDetailSql = Convert.ToString(row["SearchDetailSql"]).Split(new char[] { ';' });
                string[] searchTableName = Convert.ToString(row["SearchTableName"]).Split(new char[] { ';' });
                string[] searchDetailSqlVals = Convert.ToString(row["SearchDetailSqlVals"]).Split(new char[] { '|' });
                DataTable[] tables = new DataTable[searchDetailSql.Length];

                int index_ = 0;
                for (int i = 0; i < searchDetailSql.Length; i++)
                {
                    //构造存储过程SQL语句
                    string sql = searchDetailSql[i];
                    string[] paramNames = searchDetailSqlVals[i].Split(new char[] { ';' });
                    object[] values = new object[paramNames.Length];
                    if (paramNames[0] != "")
                    {
                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                            index_ = index_ + 1;
                        }
                        paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    }
                    //执行存储过程
                    DataTable table = ExecSqlByParam(sql, paramNames, values, searchTableName[i]);
                    tables[i] = table;
                }
                row.Table.Dispose();
                return tables;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-19</para>
        /// <para>根据sql查询语句集合返回DataTable集合</para>
        /// </summary>
        /// <param name="sqls">sql查询语句</param>
        /// <param name="tableNames">指定对应查询的表名</param>
        /// <returns></returns>
        public DataTable[] SearchData(string[] sqls, string[] tableNames)
        {
            try
            {
                DataTable dt = null;
                DataTable[] list = new DataTable[sqls.Length];
                for (int i = 0; i < sqls.Length; i++)
                {
                    dt = commonDal.GetSingle(sqls[i]);
                    if (dt == null)
                        continue;

                    dt.TableName = tableNames[i];
                    list[i] = dt;
                }
                return list;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }


        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>保存数据集</para>
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SaveData(DataSet ds)
        {
            try
            {
                //return Microsoft.ApplicationBlocks.Data.SqlHelper.SaveData(ds, true);

                return commonDal.SaveData(ds, true);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-24</para>
        /// <para>保存数据集</para>
        /// </summary>
        /// <param name="list">需要保存的表格集合</param>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="dts">执行存储过程需要的参数值对应的DataTable集合,每个DataTable只含一行</param>
        /// <param name="msg">返回操作消息</param>
        /// <param name="IsCreateBillNo">是否生成单号</param>
        /// <returns></returns>
        public bool SaveData(DataTable[] list, string formName, DataTable[] dts, DataTable[] dtsAfter, out string msg, out string _billno, bool IsCreateBillNo, string code_FormulaValue)
        {
            msg = "";
            _billno = "";
            //查询主档sql
            DataRow rowMaster = SearchMasterConfig(formName);
            if (rowMaster == null)
            {
                msg = "自定义表单配置错误！";
                return false;
            }

            try
            {
                //执行存储过程
                //string rtnMsg_sp = "";  
                if (dts != null)   //不传人参数即不执行存储过程
                {
                    msg = ExecStoredProcedure_new(rowMaster, flagSave, dts);   //执行存储过程返回的状态
                }
                //改变状态
                //if (msg.Equals(SUCCESS) || msg.Equals(""))
                if (msg.StartsWith(SUCCESS) || msg.Equals(""))
                {
                    //创建单据编号
                    string billno = "";
                    if (IsCreateBillNo)
                    {
                        string masterTableName = Convert.ToString(rowMaster["MasterTableName"]);  //主档表名
                        string dateMasterField = Convert.ToString(rowMaster["DateMasterField"]);  //主档日期字段(用于生成单号)
                        string billnoField = Convert.ToString(rowMaster["BillNoField"]);
                        foreach (DataTable dt in list)
                        {
                            if (masterTableName.Equals(dt.TableName))
                            {
                                if (Convert.ToString(dt.Rows[0][billnoField]).Equals(""))
                                {
                                    //string errMsg = "";
                                    string dateSet = null;
                                    if (!"".Equals(dateMasterField))
                                        dateSet = Convert.ToString(dt.Rows[0][dateMasterField]);    //得到对应的日期

                                    billno = CreateBillno(formName, dateSet, out msg, code_FormulaValue);
                                    dt.Rows[0][billnoField] = billno;

                                    if (!msg.Equals(""))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    billno = Convert.ToString(dt.Rows[0][billnoField]);
                                }
                                break;
                            }
                        }
                    }

                    if (commonDal.SaveData(list, true))
                    {
                        //保存成功后执行存储过程
                        if (dtsAfter != null)   //不传人参数即不执行存储过程
                        {
                            msg = ExecStoredProcedure_new(rowMaster, flagSaveAfter, dtsAfter);   //执行存储过程返回的状态
                        }
                        //msg = billno;
                        _billno = billno;
                        return true;
                    }
                    else
                    {
                        msg = "保存失败！";
                        return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod + "    模块实例名：" + formName, this, e, UserCode, IP);
                msg = "保存失败！\r\n" + e.Message;
                return false;
            }
            finally
            {
                if (rowMaster != null)
                {
                    rowMaster.Table.Dispose();
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-20</para>
        /// <para>改变主档状态(字段BillStatus)</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="pKeyValue">主键值</param>
        /// <param name="status">设置的状态代码：流程中：1 完成：2，草稿：0 ，作废：-1</param>
        /// <param name="dts">执行存储过程需要的参数值对应的DataTable集合,每个DataTable只含一行</param>
        /// <returns></returns>
        public string ChangeBillStatus(string formName, string pKeyValue, int status, DataTable[] dts, string checker)
        {
            //查询主档sql
            DataRow row = SearchMasterConfig(formName);
            if (row == null)
                return "自定义表单配置错误";

            try
            {
                //主档表名
                string tableName = Convert.ToString(row["MasterTableName"]);
                if (tableName.Equals(""))
                    return "不存在主档名称！";
                //主键名称
                string pKey = Convert.ToString(row["MasterPKField"]);
                //单据状态字段
                string billStatusField = Convert.ToString(row["BillStatusField"]) == "" ? "BillStatus" : Convert.ToString(row["BillStatusField"]);
                string billStatuserField = Convert.ToString(row["BillStatuserField"]) == "" ? "BillStatuser" : Convert.ToString(row["BillStatuserField"]);
                string billStatusTimeField = Convert.ToString(row["BillStatusTimeField"]) == "" ? "BillStatusTime" : Convert.ToString(row["BillStatusTimeField"]);
                string checkField = Convert.ToString(row["CheckField"]);
                string cancellationField = Convert.ToString(row["CancellationField"]);
                //查找单据状态
                string rtn = GetBillStatus(tableName, pKey, pKeyValue, billStatusField, checkField, cancellationField);
                if (rtn == null)
                    return "单据不存在！";
                else if (rtn.Equals(""))
                    return "单据没有状态！";

                int curBillStatus = int.Parse(rtn);
                //判断状态操作
                //if (curBillStatus == 2)
                //return "单据审批完成，不能操作！";
                if (curBillStatus == -1)
                    return "单据已作废！";

                //执行存储过程
                string rtnMsg_sp = "";  //执行存储过程返回的状态
                if (dts != null)   //不传人参数即不执行存储过程
                {
                    string flag = "";
                    switch (status)
                    {
                        case 1:
                            flag = flagCheck;
                            break;
                        case 2:
                            flag = "Check";
                            break;
                        case 0:
                            flag = flagUncheck;
                            break;
                        case -1:
                            flag = flagCancellation;
                            break;
                        default:
                            break;
                    }
                    if (!flag.Equals("") && dts != null)
                        rtnMsg_sp = ExecStoredProcedure_new(row, flag, dts);
                }
                //改变状态
                if (rtnMsg_sp.StartsWith(SUCCESS) || rtnMsg_sp.Equals(""))
                {
                    string sql = "update " + tableName + " set " + billStatusField + "='" + status + "'," + billStatuserField + "='" + checker + "'," + billStatusTimeField + "='" + GetSystemDatetime_NoH() + "'" +
                            " where " + pKey + "='" + pKeyValue + "'";
                    bool DeleteMaster = ConvertHandler.ToBoolean(row["DeleteMaster"]); //作废后是否删除主档记录
                    string resultStr = commonDal.ExcuteSQL(sql) > 0 ? rtnMsg_sp : "操作失败！";
                    if ((resultStr.StartsWith(SUCCESS) || rtnMsg_sp.Equals("")) && DeleteMaster == true && status == -1)
                    {
                        //作废后删除主档记录
                        string sqldelete = "delete from " + tableName + " where " + pKey + "='" + pKeyValue + "'";
                        resultStr = commonDal.ExcuteSQL(sqldelete) > 0 ? rtnMsg_sp : "操作失败！";
                        //作废后删除从档档记录
                        string sqlB = "select DetailTableName,DetailFKField from t_SYModManagedl_B where Serialno='"
                            + row["Serialno"].ToString() + "' and isnull(SearchPage,0)=0 order by PageNo";
                        DataTable dtB = commonDal.GetSingle(sqlB);
                        if (dtB != null && dtB.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtB.Rows.Count; i++)
                            {
                                if (dtB.Rows[i]["DetailTableName"].ToString().Trim() != "" && dtB.Rows[i]["DetailFKField"].ToString().Trim() != "")
                                    sqldelete = "delete from " + dtB.Rows[i]["DetailTableName"].ToString()
                                        + " where " + dtB.Rows[i]["DetailFKField"].ToString() + "='" + pKeyValue + "'";
                                commonDal.ExcuteSQL(sqldelete);
                            }
                        }
                    }

                    if (resultStr == "")
                        return SUCCESS;
                    else
                        return resultStr;
                }
                else
                    return rtnMsg_sp;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return e.Message;
            }
            finally
            {
                if (row != null)
                {
                    row.Table.Dispose();
                }
            }
        }

        public string ExecMenuProcedure(string menuGuid, DataTable[] dts)
        {
            try
            {
                string sql = "Select MenuSproc Sproc,MenuSprocParam [Param],MenuSprocRule SprocRule from t_SYModManagedl_Mu where Guid='" + menuGuid + "' union all "
                +"Select MenuSproc Sproc,MenuSprocParam [Param],MenuSprocRule SprocRule from t_SYModManagedl_M where Guid='" + menuGuid + "'";
                using (DataTable dt = commonDal.GetSingle(sql))
                {
                    //DataRow row = SearchMenuConfig(formName, MenuName);
                    DataRow row = dt.Rows[0];
                    if (row == null)
                        return "自定义右键菜单配置错误";
                    string memuSproc = Convert.ToString(row["Sproc"]);
                    if (memuSproc.Equals(""))
                        return "不存在存储过程！";
                    string memuSprocParam = Convert.ToString(row["Param"]);
                    if (memuSprocParam.Equals(""))
                        return "不存在存储过程参数！";
                    //string memuSprocRule = Convert.ToString(row["SprocRule"]);
                    //if (memuSprocRule.Equals(""))
                    //    return "不存在存储过程规则！";
                    string rtnMsg_sp = "";  //执行存储过程返回的状态
                    rtnMsg_sp = ExecStoredProcedure_new(row, "", dts);
                    return rtnMsg_sp;
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }


        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-24</para>
        /// <para>根据模块配置文档、参数值，执行存储过程</para>
        /// </summary>
        /// <param name="masterConfig">模块配置文档</param>
        /// <param name="opFlag">执行类型标识</param>
        /// <param name="dts">执行存储过程需要的参数值对应的DataTable集合,每个DataTable只含一行</param>
        /// <returns>返回message，"SUCCESS"为执行成功</returns>
        private string ExecStoredProcedure(DataRow masterConfig, string opFlag, DataTable[] dts)
        {
            //如保存的存储过程配置：
            //字段：Sproc_SAVE
            //值：exec sp_SYInsertRoleRight '@RoleId';exec sp_SYInsertUserRight '@UserId','@RightId'

            //字段：Param_SAVE
            //值：RoleId-t-M;UserId-D;RightId-D //字段和表以“-”分割

            //字段：SprocRule_SAVE
            //值：1;2
            try
            {
                string[] spName = Convert.ToString(masterConfig["Sproc" + opFlag]).Split(new char[] { ';' });
                string[] param = Convert.ToString(masterConfig["Param" + opFlag]).Split(new char[] { ';' });
                string[] rule = Convert.ToString(masterConfig["SprocRule" + opFlag]).Split(new char[] { ';' });
                string[,] paramValue = GetParamValue(param, dts);

                if (spName[0].Equals(""))
                    return "不存在存储过程！";

                int index_ = 0;
                for (int i = 0; i < spName.Length; i++)
                {
                    //构造存储过程SQL语句
                    string sql = spName[i];
                    int intRule = Convert.ToInt32(rule[i]);
                    for (int j = 0; j < intRule; j++)
                    {
                        sql = sql.Replace("^" + paramValue[index_, 0], paramValue[index_, 1]);
                        index_++;
                    }
                    //执行存储过程
                    using (DataTable dt = commonDal.GetSingle(sql))
                    {
                        string msg = Convert.ToString(dt.Rows[0][0]);
                        if (!msg.Equals(SUCCESS))
                            return msg;
                    }
                }
                return SUCCESS;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-24</para>
        /// <para>根据模块配置文档、参数值，执行存储过程</para>
        /// </summary>
        /// <param name="masterConfig">模块配置文档</param>
        /// <param name="opFlag">执行类型标识</param>
        /// <param name="dts">执行存储过程需要的参数值对应的DataTable集合,每个DataTable只含一行</param>
        /// <returns>返回message，"SUCCESS"为执行成功</returns>
        private string ExecStoredProcedure_new(DataRow masterConfig, string opFlag, DataTable[] dts)
        {
            //如保存的存储过程配置：
            //字段：Sproc_SAVE
            //值：exec sp_SYInsertRoleRight '@RoleId';exec sp_SYInsertUserRight '@UserId','@RightId'

            //字段：Param_SAVE
            //值：RoleId-t-M;UserId-D;RightId-D //字段和表以“-”分割

            //字段：SprocRule_SAVE
            //值：1;2
            try
            {
                string[] spName = Convert.ToString(masterConfig["Sproc" + opFlag]).Split(new char[] { ';' });
                string[] param = Convert.ToString(masterConfig["Param" + opFlag]).Split(new char[] { '|' });
                //string[] rule = Convert.ToString(masterConfig["SprocRule" + opFlag]).Split(new char[] { ';' });
                //string[,] paramValue = GetParamValue(param, dts);

                if (spName[0].Equals(""))
                    return "不存在存储过程！";

                int index_ = 0;
                for (int i = 0; i < spName.Length; i++)
                {
                    //构造存储过程SQL语句
                    string sql = spName[i];
                    string[] paramNames = param[i].Split(new char[] { ';' }); 
                    object[] values = new object[paramNames.Length];
                    for (int j = 0; j < paramNames.Length; j++)
                    {
                        values[j] = dts[index_].Rows[0][StringHandler.GetStrBefore(paramNames[j], "-")];
                        index_ = index_ + 1;
                    }
                    paramNames = StringHandler.TurnParamAndSql(sql, paramNames, out sql);
                    //执行存储过程
                    using (DataTable dt = ExecSqlByParam(sql, paramNames, values, "table"))
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string msg = Convert.ToString(dt.Rows[0][0]);
                            if (!msg.StartsWith(SUCCESS))
                                return msg;
                        }

                        //string msg = Convert.ToString(dt.Rows[0][0]);
                        //if (!msg.Equals(SUCCESS))
                        //    return msg;
                        //else
                        //{
                        //    if(dt.Columns.Count>=2)
                        //        return Convert.ToString(dt.Rows[0][1]);
                        //}
                    }
                }
                return SUCCESS;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        public string ExecStoredProcedure_More(string formName, string flag, DataTable[] dts)
        {
            //查询主档sql
            DataRow row = SearchMasterConfig(formName);
            if (row == null)
                return "自定义表单配置错误";

            try
            {
                string rtnMsg_sp = "SUCCESS";
                if (dts != null)
                    rtnMsg_sp = ExecStoredProcedure_new(row, flag, dts);

                row.Table.Dispose();

                return rtnMsg_sp;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return e.Message;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-24</para>
        /// <para>根据参数配置字段查找行集合对应的值</para>
        /// </summary>
        /// <param name="param">参数配置数组，格式:Field_TableName</param>
        /// <param name="dts">执行存储过程需要的参数值对应的DataTable集合,每个DataTable只含一行</param>
        /// <returns>按顺序返回二维数组:string[fieldName,value]</returns>
        private string[,] GetParamValue(string[] param, DataTable[] dts)
        {
            try
            {
                string[,] data = new string[param.Length, 2];

                for (int i = 0; i < param.Length; i++)
                {
                    string[] ss = param[i].Split(new char[] { '-' });
                    string field = ss[0];
                    data[i, 0] = param[i];

                    string val = Convert.ToString(dts[i].Rows[0][field]);
                    if ("True".Equals(val))
                        val = "1";
                    else if ("False".Equals(val))
                        val = "0";
                    data[i, 1] = val.Replace("'", "''"); //处理单引号
                }
                return data;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
            finally
            {
                if (dts != null)
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        dts[i].Clear();
                        dts[i].Dispose();
                    }
                    dts = null;
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-21</para>
        /// <para>根据sql语句查询，返回DataTable集合</para>
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public DataTable[] Print(string[] sqls)
        {
            try
            {
                DataTable dt = null;
                DataTable[] list = new DataTable[sqls.Length];
                for (int i = 0; i < sqls.Length; i++)
                {
                    dt = commonDal.GetSingle(sqls[i]);
                    list[i] = dt;
                }
                return list;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-21</para>
        /// <para>查询单据状态</para>
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pKey">主键名称</param>
        /// <param name="pKeyValue">主键值</param>
        /// <returns>状态</returns>
        private string GetBillStatus(string tableName, string pKey, string pKeyValue, string billStatusField, string checkField, string cancellationField)
        {
            try
            {
                string sql = "";
                if (checkField == "")
                    sql = "select " + billStatusField + " from " + tableName + " where " + pKey + "='" + pKeyValue + "'";
                else
                    sql = "select " + checkField + "," + cancellationField + " from " + tableName + " where " + pKey + "='" + pKeyValue + "'";

                using (DataTable dt = commonDal.GetSingle(sql))
                {
                    if (dt == null || dt.Rows.Count == 0)
                        return null;
                    else
                    {
                        if (checkField == "")
                            return Convert.ToString(dt.Rows[0][0]);
                        else
                        {
                            if (ConvertHandler.ToBoolean(dt.Rows[0][0]))
                                return "2";
                            else if (ConvertHandler.ToBoolean(dt.Rows[0][1]))
                                return "-1";
                            else
                                return "0";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        private DataRow SearchMenuConfig(string formName, string MenuName)
        {
            try
            {
                string sql = "select a.ModName,a.ClassName,b.PageName,c.MenuSproc Sproc,c.MenuSprocParam [Param],c.MenuSprocRule SprocRule from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno inner join t_SYModManagedl_Mu c on b.guid=c.pguid where a.ClassName='" + formName + "' and c.MenuName='" + MenuName + "'";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                else
                {
                    dt.TableName = "t_SYModManagedl_Mu";
                    return dt.Rows[0];
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-19</para>
        /// <para>查询主档sql</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        private DataRow SearchMasterConfig(string formName)
        {
            try
            {
                string sql = "select m.Serialno,c.CheckField,c.CancellationField,c.MasterSql,c.MasterSqlClick,c.DetailSql,c.MasterTableName,c.DetailTableName,c.SearchTableName" +
                        " ,c.DateField,c.MasterPKField,c.DetailPKField,c.DetailFKField,c.BillNoField" +
                        " ,c.SprocSave,c.ParamSave,c.SprocRuleSave" + //保存时执行的存储过程
                        " ,c.SprocSaveAfter,c.ParamSaveAfter" + //保存后执行的存储过程
                        " ,c.SprocCheck,c.ParamCheck,c.SprocRuleCheck" + //从草稿到提交时执行的存储过程
                        " ,c.SprocUnCheck,c.ParamUnCheck,c.SprocRuleUnCheck" + //从草稿到提交时执行的存储过程
                        " ,c.SprocApprove,c.ParamApprove" + //审批时执行的存储过程
                        " ,c.SprocUnApprove,c.ParamUnApprove" + //反审批时执行的存储过程
                        " ,c.SprocConfirm,c.ParamConfirm" + //确认时执行的存储过程
                        " ,c.SprocUnConfirm,c.ParamUnConfirm" + //撤销确认时执行的存储过程
                        " ,c.SprocShowBill,c.ParamShowBill" + //撤销确认时执行的存储过程
                        " ,c.SprocCancellation,c.ParamCancellation,c.SprocRuleCancellation" +  //作废时执行的存储过程
                        " ,c.SprocNewBefore,c.ParamNewBefore" +  //新增前执行的存储过程
                        " ,c.SprocAfterCheck,c.ParamAfterCheck" +  //新增前执行的存储过程
                        " ,c.SearchDetailSql,c.SearchDetailSqlVals,c.DeleteMaster,c.BillStatusField,c.BillStatuserField,c.BillStatusTimeField" +
                        " ,r.DateMasterField" +
                        " from t_SYModManage as m" +
                        " left join t_SYModManage_Config as c" +
                        " on m.Serialno=c.Serialno" +
                        " left join t_SYModManage_BillnoRule as r" +
                        " on m.Serialno=r.Serialno" +
                        " where m.Del=0 and m.ClassName='" + formName + "'";
                //DataSet ds = commonDal.Query(sql);

                //if (ds == null && ds.Tables.Count == 0)
                //    return null;

                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                else
                {
                    dt.TableName = "t_SYModManage_Config";
                    return dt.Rows[0];
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-22</para>
        /// <para>执行存储过程，返回正确SUCCESS、错误</para>
        /// </summary>
        /// <param name="sporcSql"></param>
        /// <returns></returns>
        public string ExecSporc(string sporcSql)
        {
            try
            {
                using (DataTable dt = commonDal.GetSingle(sporcSql))
                {
                    if (dt == null)
                        return "";
                    return Convert.ToString(dt.Rows[0][0]);
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod + "    查询SQL:" + sporcSql, this, e, UserCode, IP);
                return "系统抛出异常：" + e.Message;
            }
        }


        # region 基础表操作

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-12</para>
        /// <para>保存数据集(基础表)</para>
        /// </summary>
        /// <param name="list">需要保存的表格集合</param>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="dts">执行存储过程需要的参数值对应的DataTable集合,每个DataTable只含一行</param>
        /// <param name="msg">返回操作消息</param>
        /// <returns></returns>
        public bool SaveSingleData(DataTable[] list, string formName, DataTable[] dts, DataTable[] dtsAfter, out string msg)
        {
            msg = "";
            //查询主档sql
            DataRow rowSingle = SearchSingleConfig(formName);
            if (rowSingle == null)
            {
                msg = "自定义表单配置错误！";
                return false;
            }
            try
            {
                //执行存储过程
                //string rtnMsg_sp = "";  
                if (dts != null)   //不传人参数即不执行存储过程
                {
                    msg = ExecStoredProcedure_new(rowSingle, flagSave, dts);   //执行存储过程返回的状态
                }
                //改变状态
                //if (msg.Equals(SUCCESS) || msg.Equals(""))
                if (msg.StartsWith(SUCCESS) || msg.Equals(""))
                {
                    if (commonDal.SaveData(list, true))
                    {
                        //保存成功后执行存储过程
                        if (dtsAfter != null)   //不传人参数即不执行存储过程
                        {
                            msg = ExecStoredProcedure_new(rowSingle, flagSaveAfter, dtsAfter);   //执行存储过程返回的状态
                            if (msg.StartsWith(SUCCESS) && msg.Length > 7)
                                msg = msg.Substring(7, msg.Length - 7);
                            else
                                msg = "";
                        }
                        //msg = "";
                        return true;
                    }
                    else
                    {
                        msg = "保存失败！";
                        return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                msg = "保存失败！\r\n" + e.Message;
                return false;
            }
            finally
            {
                if (rowSingle != null)
                {
                    rowSingle.Table.Dispose();
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-12</para>
        /// <para>删除记录(基础表)</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <param name="id">记录Id</param>
        /// <param name="msg">返回操作消息</param>
        /// <returns></returns>
        public bool DeleteBySingleId(string formName, string id, out string msg, bool deleteComplete, DataTable[] dts)
        {
            msg = "";
            //查询主档sql
            DataRow rowSingle = SearchSingleConfig(formName);
            if (rowSingle == null)
            {
                msg = "自定义表单配置错误！";
                return false;
            }

            try
            {
                //执行存储过程
                //string rtnMsg_sp = "";  
                if (dts != null)   //不传人参数即不执行存储过程
                {
                    msg = ExecStoredProcedure_new(rowSingle, flagCancellation, dts);   //执行存储过程返回的状态
                }

                if (!(msg.StartsWith(SUCCESS) || msg.Equals("")))
                    return false;

                string tableName = Convert.ToString(rowSingle["ListTable"]);
                string ListTableKey = Convert.ToString(rowSingle["ListTableKey"]);
                string sql;
                if (deleteComplete)
                    sql = "delete from " + tableName + " where " + ListTableKey + "='" + id + "'";
                else
                    sql = "update " + tableName + " set del=1" + " where " + ListTableKey + "='" + id + "'";

                int count = commonDal.ExcuteSQL(sql);
                if (count > 0)
                {
                    msg = "";
                    return true;
                }
                else
                {
                    msg = "删除失败！";
                    return false;
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
            finally
            {
                if (rowSingle != null)
                {
                    rowSingle.Table.Dispose();
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-12</para>
        /// <para>获取基础表列表数据</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        public DataTable SearchSingleList(string formName)
        {
            //查询主档sql
            DataRow rowSingle = SearchSingleConfig(formName);
            if (rowSingle == null)
            {
                return null;
            }

            try
            {
                string sql = Convert.ToString(rowSingle["ListSql"]);
                string tableName = Convert.ToString(rowSingle["ListTable"]);
                rowSingle.Table.Dispose();
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                    return null;
                else
                {
                    dt.TableName = tableName;
                    return dt;
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-12</para>
        /// <para>获取基础表树状数据</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        public DataTable SearchSingleTree(string formName)
        {
            //查询主档sql
            DataRow rowSingle = SearchSingleConfig(formName);
            if (rowSingle == null)
            {
                return null;
            }

            try
            {
                string sql = Convert.ToString(rowSingle["TreeSql"]);
                string tableName = Convert.ToString(rowSingle["TreeTable"]);
                rowSingle.Table.Dispose();
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                    return null;
                else
                {
                    dt.TableName = tableName;
                    return dt;
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-12</para>
        /// <para>得到基础表配置信息</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        private DataRow SearchSingleConfig(string formName)
        {
            string sql = "select m.Serialno, c.ListTable, c.ListTableKey, c.TreeTable, c.ListSql, c.TreeSql" +
                    ", c.SprocSave, c.ParamSave, c.SprocRuleSave, c.SprocSaveAfter, c.ParamSaveAfter, c.SprocCancellation, c.ParamCancellation, c.TreeVisible" +
                    " from t_SYModManage as m" +
                    " left join t_SYModSingle_Config as c" +
                    " on m.Serialno=c.Serialno" +
                    " where m.Del=0 and m.ClassName='" + formName + "'";
            //DataSet ds = commonDal.Query(sql);

            //if (ds == null && ds.Tables.Count == 0)
            //    return null;

            try
            {
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                else
                {
                    return dt.Rows[0];
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        # endregion

        # region 自定义查询

        /// <summary>
        /// 自定义查询配置查询
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public DataTable[] GetSyModData_Search(string className)
        {
            try
            {
                IList<DataTable> dts = new List<DataTable>();
                DataTable dt = null;

                dt = GetSyModSearch_M(className);
                dts.Add(dt);
                dt = GetSYModSearch_B(className);
                dts.Add(dt);
                dt = GetSYModSearch_D(className);
                dts.Add(dt);

                dt = RunSQL("select mu.* from t_SYModManagedl_Mu as mu" +
                                    " left join t_SYModSearch_B as b on mu.PGuid=b.Guid" +
                                    " left join t_SYModManage as m on b.Serialno=m.Serialno" +
                                    " where classname='" + className + "'" +
                                    " order by sno");
                dt.TableName = "t_SYModManagedl_Mu";
                dts.Add(dt);

                dt = RunSQL("select mu.* from t_SYModManagedl_Mu as mu" +
                                    " left join t_SYModSearch_Config c on mu.PGuid=c.Guid" +
                                    " left join t_SYModManage as m on c.Serialno=m.Serialno" +
                                    " where classname='" + className + "'" +
                                    " order by sno");
                dt.TableName = "t_SYModManagedl_Mu_Master";
                dts.Add(dt);

                dt = RunSQL("select b.* from t_SYModManage a" +
                                    " inner join t_SYModSearch_Config b on a.serialno=b.serialno" +
                                    " where a.ClassName='" + className + "'");
                dt.TableName = "t_SYModSearch_Config";
                dts.Add(dt);

                return dts.ToArray<DataTable>();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-16</para>
        /// <para>查询自定义查询从档显示信息</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSYModSearch_D(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchSearchConfig(formName);
            if (rowSearch == null)
            {
                return null;
            }
            try
            {
                string Serialno = Convert.ToString(rowSearch["Serialno"]);
                rowSearch.Table.Dispose();
                string sql = "select d.*,b.SearchPage,'tPag'+cast(b.PageNo as varchar(10)) PageName" +
                            " from v_SYModSearch_D d left join t_SYModSearch_B b" +
                            " on d.PGuid=b.Guid" +
                            " where b.Serialno='" + Serialno + "' order by b.PageNo, d.SNo";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                {
                    return null;
                }
                dt.TableName = "t_SYModSearch_D";
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-15</para>
        /// <para>查询自定义查询从档页面信息</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSYModSearch_B(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchSearchConfig(formName);
            if (rowSearch == null)
            {
                return null;
            }

            try
            {
                string Serialno = Convert.ToString(rowSearch["Serialno"]);
                rowSearch.Table.Dispose();

                string sql = "select *,'tPag'+cast(PageNo as varchar(10)) PageName from t_SYModSearch_B" +
                            " where Serialno='" + Serialno + "' order by PGuid,PageNo";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                {
                    return null;
                }
                dt.TableName = "t_SYModSearch_B";
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-13</para>
        /// 获取Panel的高度
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public int GetSearchPanelHeight(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchSearchConfig(formName);
            if (rowSearch == null)
            {
                return 0;
            }

            try
            {
                int h = Convert.ToInt32(rowSearch["PanelHeight"]);
                rowSearch.Table.Dispose();
                return h;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-13</para>
        /// <para>查询主档配置信息</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSyModSearch_M(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchSearchConfig(formName);
            if (rowSearch == null)
            {
                return null;
            }

            try
            {
                string Serialno = Convert.ToString(rowSearch["Serialno"]);
                rowSearch.Table.Dispose();

                string sql = "select * from t_SYModSearch_M" +
                            " where Serialno='" + Serialno + "' order by SNo";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                {
                    return null;
                }
                dt.TableName = "t_SYModSearch_M";
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-13</para>
        /// <para>查询自定义查询的配置表</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        private DataRow SearchSearchConfig(string formName)
        {
            string sql = "select m.Serialno, c.PanelHeight" +
                //", c.SprocSave, c.ParamSave, c.SprocRuleSave, c.TreeVisible" +
                    " from t_SYModManage as m" +
                    " left join t_SYModSearch_Config as c" +
                    " on m.Serialno=c.Serialno" +
                    " where m.Del=0 and m.ClassName='" + formName + "'";

            try
            {
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                else
                {
                    dt.TableName = "t_SYModSearch_Config";
                    return dt.Rows[0];
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        #endregion

        # region 自定义录入操作

        /// <summary>
        /// 自定义录入配置查询
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public DataTable[] GetSyModData_Input(string className)
        {
            try
            {
                IList<DataTable> dts = new List<DataTable>();
                DataTable dt = null;

                dt = GetSYModInput_Congfig(className);
                dts.Add(dt);

                dt = GetSYModInput_M(className);
                dts.Add(dt);

                dt = GetSYModInput_B(className);
                dts.Add(dt);

                dt = GetSYModInput_D(className);
                dts.Add(dt);

                dt = RunSQL("select mu.* from t_SYModManagedl_Mu as mu" +
                                    " left join t_SYModInput_B as b on mu.PGuid=b.Guid" +
                                    " left join t_SYModManage as m on b.Serialno=m.Serialno" +
                                    " where classname='" + className + "'" +
                                    " order by sno");
                dt.TableName = "t_SYModManagedl_Mu";
                dts.Add(dt);

                dt = RunSQL("select mu.* from t_SYModManagedl_Mu as mu" +
                                    " left join t_SYModInput_Config c on mu.PGuid=c.Guid" +
                                    " left join t_SYModManage as m on c.Serialno=m.Serialno" +
                                    " where classname='" + className + "'" +
                                    " order by sno");
                dt.TableName = "t_SYModManagedl_Mu_Master";
                dts.Add(dt);

                return dts.ToArray<DataTable>();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-16</para>
        /// <para>查询自定义录入从档显示信息</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSYModInput_D(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchInputConfig(formName);
            if (rowSearch == null)
            {
                return null;
            }

            try
            {
                string Serialno = Convert.ToString(rowSearch["Serialno"]);
                rowSearch.Table.Dispose();

                string sql = "select d.*,'tPag'+cast(b.PageNo as varchar(10)) PageName" +
                            " from v_SYModInput_D d left join t_SYModInput_B b" +
                            " on d.PGuid=b.Guid" +
                            " where b.Serialno='" + Serialno + "' order by b.PageNo, d.SNo";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                {
                    return null;
                }
                dt.TableName = "t_SYModInput_D";
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-15</para>
        /// <para>查询自定义录入从档页面信息</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSYModInput_B(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchInputConfig(formName);
            if (rowSearch == null)
            {
                return null;
            }

            try
            {
                string Serialno = Convert.ToString(rowSearch["Serialno"]);
                rowSearch.Table.Dispose();

                string sql = "select *,'tPag'+cast(PageNo as varchar(10)) PageName from t_SYModInput_B" +
                            " where Serialno='" + Serialno + "' order by PageNo";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                {
                    return null;
                }
                dt.TableName = "t_SYModInput_B";
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-13</para>
        /// <para>查询主档配置信息</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSYModInput_M(string formName)
        {
            //查询主档sql
            DataRow rowSearch = SearchInputConfig(formName);
            if (rowSearch == null)
            {
                return null;
            }

            try
            {
                string Serialno = Convert.ToString(rowSearch["Serialno"]);
                rowSearch.Table.Dispose();

                string sql = "select * from t_SYModInput_M" +
                            " where Serialno='" + Serialno + "'";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null)
                {
                    return null;
                }
                dt.TableName = "t_SYModInput_M";
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>查询自定义录入的配置表</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public DataTable GetSYModInput_Congfig(string formName)
        {
            DataRow row = SearchInputConfig(formName);
            return row != null ? row.Table : null;
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>查询自定义录入的配置表</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        private DataRow SearchInputConfig(string formName)
        {
            string sql = "select m.Serialno, c.*" + 
                    " from t_SYModManage as m" +
                    " left join t_SYModInput_Config as c" +
                    " on m.Serialno=c.Serialno" +
                    " where m.Del=0 and m.ClassName='" + formName + "'";

            try
            {
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                else
                {
                    dt.TableName = "t_SYModInput_Config";
                    return dt.Rows[0];
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        # endregion

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-25</para>
        /// <para>根据表单名称查询对应的功能脚本</para>
        /// </summary>
        /// <param name="formName">表单名称，对应字段:t_SYModManage.ClassName</param>
        /// <returns></returns>
        public DataTable SearchScriptConfig(string formName)
        {
            string sql = "select Guid, s.Serialno, s.ScriptName, s.ScriptStr, s.RtnType," +
                        " s.BUser, s.BTime, s.EUser, s.ETime" +
                        " from t_SYModManage_Script as s" +
                        " left join t_SYModManage as m" +
                        " on s.Serialno=m.Serialno" +
                        " where m.ClassName='" + formName + "'";

            try
            {
                DataTable dt = commonDal.GetSingle(sql);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-27</para>
        /// <para>返回数据库所在服务器日期时间，格式：yyyy-MM-dd HH:mm:ss</para>
        /// </summary>
        /// <returns></returns>
        public String GetSystemDatetime()
        {
            try
            {
                string sql = "select getdate()";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                string datetime = Convert.ToDateTime(dt.Rows[0][0]).ToString("yyyy-MM-dd HH:mm:ss");
                dt.Dispose();
                return datetime;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2014-05-21</para>
        /// <para>返回数据库所在服务器日期时间(星期天推到星期一)，格式：yyyy-MM-dd HH:mm:ss</para>
        /// </summary>
        /// <returns></returns>
        public String GetSystemDatetime_NoH()
        {
            try
            {
                bool holidayLimit = false;
                string datetime = "";
                string sql = "select getdate()";
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                DataTable dtHolidayLimit = commonDal.GetSingle("select HolidayLimit from t_SYSystemParams");
                holidayLimit = Convert.ToBoolean(dtHolidayLimit.Rows[0]["HolidayLimit"]);
                if (holidayLimit)
                {
                    string sql2 = "select year,BeginTime,endTime,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday," + 
                               "h.HolidaysName,h.BeginDate,h.EndDate,h.IsHolidays" +
                               " from t_SYFactoryCalendar f, t_SYHolidays h" +
                               " where f.Serialno=h.Serialno";
                    DataTable dt2 = commonDal.GetSingle(sql2);
                    if (dt2 == null)
                        return null;

                    //DateTime dtNow = Convert.ToDateTime(dt.Rows[0][0]);
                    DateTime dtNow = DateTime.Now;
                    DateTime adate = Comm.GetWorkDate(dtNow, dt2);
                    datetime = adate.ToString("yyyy-MM-dd HH:mm:ss");
                    dt2.Dispose();
                }
                else
                    datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dt.Dispose();
                
                return datetime;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-27</para>
        /// <para>生成单据编号，返回字符串数组(包含两个元素)，</para>
        /// <para>创建成功返回{ SUCCESS, number }，失败返回{ FAIL, errMsg }</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="dateStr">设定的日期套用规则</param>
        /// <returns></returns>
        public string[] CreateBillno(string formName, string dateStr, string code_FormulaValue)
        {
            try
            {
                string errMsg = "";
                string number = CreateBillno(formName, dateStr, out errMsg, code_FormulaValue);

                if (number.Equals(""))
                    return new string[] { FAIL, errMsg };
                else
                    return new string[] { SUCCESS, number };
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }


        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-27</para>
        /// <para>生成单据编号，返回编号，</para>
        /// <para>如果编号为空字符串，则有错误提示errMsg</para>
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="dateStr">设定的日期套用规则</param>
        /// <param name="errMsg">错误消息</param>
        /// <param name="code_FormulaValue">公式计算前缀值</param>
        /// <returns></returns>
        private string CreateBillno(string formName, string dateStr, out string errMsg, string code_FormulaValue)
        {
            errMsg = "";
            //查询主档sql
            DataRow row = SearchMasterConfig(formName);
            if (row == null)
            {
                errMsg = "自定义表单配置错误！";
                return "";
            }

            try
            {
                string serialno = Convert.ToString(row["Serialno"]);
                string billnoField = Convert.ToString(row["billnoField"]);
                DataRow rowRule = GetNumberRule(serialno);

                if (rowRule == null)
                {
                    errMsg = "没有配置单据编号规则！";
                    return "";
                }

                string code = Convert.ToString(rowRule["Code"]);    //前缀
                string step_First = Convert.ToString(rowRule["Step_First"]);    //前分隔符
                string dateRule = Convert.ToString(rowRule["DateRule"]);    //日期规则

                if (dateStr == null || "".Equals(dateStr))
                    dateStr = Convert.ToDateTime(rowRule["datetime_"]).ToString(dateRule);   //当前日期套用规则
                else
                    dateStr = Convert.ToDateTime(dateStr).ToString(dateRule);   //设定的日期套用规则

                string step_Center = Convert.ToString(rowRule["Step_Center"]);  //中间分隔符
                string code_Last = Convert.ToString(rowRule["Code_Last"]); //紧跟日期后面的代号

                string step_Last = Convert.ToString(rowRule["Step_Last"]);    //后分隔符
                int len = Convert.ToInt32(rowRule["NoLen"]); //序号长度
                bool isChange = Convert.ToBoolean(rowRule["IsChange"]); //是否可以修改

                string firstCode = code + step_First + dateStr + step_Center + code_Last + step_Last; //单号(缺序号)

                string masterTableName = Convert.ToString(row["MasterTableName"]);
                string lastNumber = GetLastNumber(masterTableName, firstCode, billnoField, len, code_FormulaValue);
                if (lastNumber == null)
                {
                    errMsg = "单据编号查询失败！";
                    return "";
                }

                if (lastNumber.Equals("") || isChange)  //把规则配置设为不可修改
                {
                    if (!ChangeNumberRuleSatus(serialno))
                    {
                        errMsg = "修改单据编号规则失败！";
                        return "";
                    }
                }

                //int strlen = lastNumber.Length - firstCode.Length;
                //string _lastNumber = lastNumber.Substring(strlen);
                //if (!_lastNumber.Contains(firstCode))

                //第一张单、或不在当天、当月的，则从1开始
                if (!lastNumber.Contains(firstCode))
                {
                    return code_FormulaValue + firstCode + "1".PadLeft(len, '0');
                }
                else //累加
                {
                    int n = 0;
                    if (string.IsNullOrWhiteSpace(code_FormulaValue))
                    {
                        //n = Convert.ToInt32(lastNumber.Replace(firstCode, ""));
                        n = Convert.ToInt32(lastNumber.Substring(firstCode.Length, len));
                    }
                    else //存在公式动态前缀
                    {
                        string strIndex = lastNumber.Substring(lastNumber.Length - len);
                        n = Convert.ToInt32(strIndex);
                    }
                    return code_FormulaValue + firstCode + (++n).ToString().PadLeft(len, '0');
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                errMsg = e.Message.Replace("'", "''");
                return null;
            }
            finally
            {
                if (row != null)
                    row.Table.Dispose();
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-27</para>
        /// <para>根据主档名称查找最后的单据编号</para>
        /// </summary>
        /// <param name="masterTableName">主档名称</param>
        /// <returns></returns>
        private String GetLastNumber(string masterTableName, string firstCode, string billnoFiled, int indexlen, string code_FormulaValue)
        {
            string sql = string.Empty;
            if (string.IsNullOrWhiteSpace(code_FormulaValue))
                sql = "select max(" + billnoFiled + ") from " + masterTableName +
                        " where " + billnoFiled + " like '%" + firstCode + "%'";

            else//存在公式动态前缀
            {
                int len = firstCode.Length + indexlen;
                sql = "select max(substring(" + billnoFiled + ",len(" + billnoFiled + ")-" + (len - 1) + "," + len + ")) from " + masterTableName +
                        " where " + billnoFiled + " like '%" + firstCode + "%'";

            }
            try
            {
                using (DataTable dt = commonDal.GetSingle(sql))
                {
                    if (dt == null)
                        return null;
                    else if (dt.Rows.Count == 0)
                        return "";

                    return Convert.ToString(dt.Rows[0][0]);
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-27</para>
        /// <para>根据模块编号查找对应的单据编号规则</para>
        /// </summary>
        /// <param name="serialno">模块编号</param>
        /// <returns></returns>
        private DataRow GetNumberRule(string serialno)
        {
            string sql = "select getdate() as datetime_,* from t_SYModManage_BillnoRule" +
                        " where Serialno='" + serialno + "'";

            try
            {
                DataTable dt = commonDal.GetSingle(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                return dt.Rows[0];
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-27</para>
        /// <para>把规则配置设为不可修改</para>
        /// </summary>
        /// <param name="serialno">模块编号</param>
        /// <returns></returns>
        private bool ChangeNumberRuleSatus(string serialno)
        {
            try
            {
                string sql = "update t_SYModManage_BillnoRule set IsChange=0" +
                            " where Serialno='" + serialno + "'";
                return commonDal.ExcuteSQL(sql) > 0 ? true : false;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// <para>日期：2012-10-06</para>
        /// <para>取EXCEL导入模板方案名称</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetExcelTemplate()
        {
            try
            {
                string strSql = " select CatName,id,code from t_syCategory where TypeId=0 ";
                DataTable dtTemplate = commonDal.GetSingle(strSql);
                return dtTemplate;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// <para>日期：2012-10-06</para>
        /// <para>保存EXCEL导入模板</para>
        /// </summary>
        /// <param name="dic">EXCEL栏目与数据库字段对应字典</param>
        /// <param name="TemplateName">方案名称</param>
        /// <param name="code">类别编号（GUID）</param>
        public void SaveExcelTemplate(Dictionary<string, string> dic,
            string TemplateName, string ClassName, string code)
        {
            try
            {
                string strSql = string.Empty;
                string FieldTo, FieldTodesc;

                DataTable dt = new DataTable();

                string strSQL = string.Empty, tableName = string.Empty;

                strSQL = "  declare @code varchar(100)  ";

                strSQL += " if exists(select null from t_syCategory where Code='" + code + "')";
                strSQL += "  begin ";
                strSQL += "  update  t_syCategory set CatName='" + TemplateName + "' where code='" + code + "'";
                strSQL += "  delete from t_syExcelTemplate where CatCode='" + code + "' ";
                strSQL += "  set @code='" + code + "' ";
                strSQL += "  end  ";
                strSQL += " else ";
                strSQL += " begin  ";
                strSQL += " set @code=newid() ";
                strSQL += "   insert t_syCategory (CatName,TypeId,Code,ClassName)";
                strSQL += "  select '" + TemplateName + "',0,@code,'" + ClassName + "'";
                strSQL += " end  ";

                foreach (string s in dic.Keys)
                {
                    strSQL += " insert  into t_syExcelTemplate (FieldFrom,FieldTo,FieldTodesc,CatCode)";
                    FieldTo = dic[s].Split('|')[0];
                    FieldTodesc = dic[s].Split('|')[1];
                    strSQL += "  select '" + s + "','" + FieldTo + "','" + FieldTodesc + "',@code";
                }

                commonDal.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
            }
        }



        /// <summary>
        /// 创建人：黎金来
        /// <para>日期：2012-10-06</para>
        /// <para>执行SQL语句</para>
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        public DataTable RunSQL(string strSQL)
        {
            try
            {
                DataTable dt = commonDal.GetSingle(strSQL);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod + "     查询SQL:" + strSQL, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// <para>日期：2012-10-06</para>
        /// <para>根据EXCEL模板方案名称，取对应的字段</para>
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetExcelTemplateByCatId(string code)
        {
            try
            {
                DataTable dt = commonDal.GetSingle(" select * from dbo.t_SYExcelTemplate  where catCode='" + code + "' ");
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// <para>日期：2012-10-07</para>
        /// <para>删除EXCEL导入配置模板</para>
        /// </summary>
        /// <param name="code">类别ID</param>
        public void DeleteExcelTemplateByCatId(string code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "  delete from  t_SYExcelTemplate where catCode='" + code + "'";
                strSQL += "  delete from t_syCategory where code='" + code + "'";

                commonDal.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-11-02
        /// 根据模块名称取打印模板
        /// </summary>
        /// <param name="ClassName">模块名称</param>
        /// <param name="PrintType">打印类别(0:普通打印；1：草拟打印)</param>
        /// <returns>打印模板数据集</returns>
        public DataTable GetPrintTemplateByClassName(string ClassName, int PrintType)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "  select * from  t_SYReportTemp where ClassName='" + ClassName + "' and PrintType=" + PrintType;

                DataTable dt = commonDal.GetSingle(strSQL);

                dt.TableName = "t_SYReportTemp";

                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-11-03
        /// 取打印模板表的列名
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrintTemplateColumn()
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "  select * from  t_SYReportTemp where  1=2 ";

                DataTable dt = commonDal.GetSingle(strSQL);
                dt.TableName = "t_SYReportTemp";


                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-11-03
        /// 取根据CODE打印资料
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public DataTable GetPrintTemplateByCode(string Code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "  select * from  t_SYReportTemp where code='" + Code + "'";

                DataTable dt = commonDal.GetSingle(strSQL);
                dt.TableName = "t_SYReportTemp";

                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-12-06
        /// 根据实例名，检查该模块的单据是否需要走流程审批
        /// </summary>
        /// <param name="ClassName">实例名</param>
        /// <returns>布尔值（true:需要走流程审批;false:不需要）</returns>
        public bool CheckModIsWorkFlow(string ClassName)
        {
            DataTable dtData = null;
            bool blResult = true;

            string strSQL = string.Empty;
            try
            {
                strSQL = "  select 1 from T_SYWorkFlow_Mod t1 join T_FF_DF_WORKFLOWDEF t2 ";
                strSQL += "  on t2.id=t1.workflow_id and t2.state=1 and  t1.className='" + ClassName + "'";

                dtData = commonDal.GetSingle(strSQL);
                if (dtData != null && dtData.Rows.Count > 0)
                    blResult = true;
                else
                    blResult = false;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                blResult = false;
            }

            return blResult;
        }

        public DataTable ExecSqlByParam(string sqlstr, string[] paramName, object[] value, string tableName)
        {
            try
            {
                DataTable dt = null;
                if (paramName != null)
                {
                    SqlParameter[] param = new SqlParameter[paramName.Length];
                    for (int i = 0; i < paramName.Length; i++)
                        param[i] = new SqlParameter(paramName[i], value[i]);
                    dt = commonDal.Query(sqlstr, param);
                }
                else
                {
                    dt = commonDal.Query(sqlstr).Tables[0];
                }
                if (dt != null)
                {
                    dt.TableName = tableName;
                }
                return dt;
            }
            catch (Exception ex)
            {
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

                string str = log.CurrMethod + "     查询SQL:" + sqlstr + "\r\n参数名:" + paramNameStr + "\r\n参数值:" + valueStr;
                log.SysErrorSave(str, this, ex, UserCode, IP);

                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("msg", Type.GetType("System.String"));
                dtErr.Rows.Add(new object[] { ex.Message });
                dtErr.TableName = "异常";
                return dtErr;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-02-22
        /// 获取日程资料
        /// </summary>
        /// <param name="condiction">SQL条件查询</param>
        /// <returns></returns>
        public DataTable GetScheduler(string condiction)
        {
            DataTable dtData = null;
            string strSQL = string.Empty;

            try
            {
                strSQL = " select  * from  T_OAScheduler " + condiction;
                dtData = commonDal.GetSingle(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
            }
            return dtData;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-03-05
        /// 在桌面上显示我的日程
        /// </summary>
        /// <returns></returns>
        public DataTable GetDesktopScheduler(string BUser)
        {
            DataTable dtData = null;
            string strSQL = string.Empty;

            try
            {
                strSQL = " select   top 7 * from  T_OAScheduler   ";
                //strSQL += "   convert(nvarchar(10),appStart,20) AppStart,";
                //strSQL += " Applocation  from  T_OAScheduler  ";
                strSQL += " where BUser='" + BUser + "'";
                strSQL += "  and convert(nvarchar(10),appStart,20) >='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                strSQL += " and convert(nvarchar(10),appStart,20) <='" + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd") + "'";
                strSQL += " order by appStart ";

                dtData = commonDal.GetSingle(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
            }
            return dtData;
        }


        /// <summary>
        /// 创建人：方君业
        /// 日期：2013-04-15
        /// 把生成的预警结果填入dicWarningTable_New
        /// </summary>
        /// <returns></returns>
        public void AddWarningTable(DataTable dtWarnTable)
        {
            try
            {
                if (dicWarningTable_New.ContainsKey(dtWarnTable.TableName))
                {
                    dicWarningTable_New[dtWarnTable.TableName] = dtWarnTable;
                }
                else
                {
                    dicWarningTable_New.Add(dtWarnTable.TableName, dtWarnTable);
                }
            }
            catch (Exception e) 
            {
                log.SysErrorSave(log.CurrMethod, this, e, dtWarnTable.TableName, dtWarnTable.Rows.Count.ToString()); 
            }
        }

        /// <summary>
        /// 创建人：方君业
        /// 日期：2013-04-15
        /// 把生成的预警结果填入dicWarningTable_New
        /// </summary>
        /// <returns></returns>
        public DataTable GetWarningTable_New(string tableKey)
        {
            try
            {
                if (dicWarningTable_New.ContainsKey(tableKey))
                {
                    return dicWarningTable_New[tableKey];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "WCF", "");
                return null;
            }
        }


        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-04-10
        /// 
        /// </summary>
        /// <param name="BUser"></param>
        /// <returns></returns>
        public DataTable GetMyUnCheckBill(string BUser)
        {
            DataTable dtData = null;
            string strSQL = string.Empty;

            try
            {
                strSQL = " exec sp_syWorkFlowUnCheck '" + BUser + "' ";
                dtData = commonDal.GetSingle(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
            }
            return dtData;
        }

        public int ExcuteSQL(string sql)
        {
            try
            {
                return commonDal.ExcuteSQL(sql);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod + " " + sql, this, ex, UserCode, IP);
                return -1; 
            }
        }

        public string ExcuteSQLAT(string sql)
        {
            try
            {
                return Convert.ToString(commonDal.ExcuteSQL(sql));
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod + " " + sql, this, ex, UserCode, IP);
                return "系统抛出异常：" + ex;
            }
        }
         
        /// <summary>
        /// 批量执行sql语句
        /// </summary>
        /// <param name="Sqls"></param>
        /// <returns></returns>
        public bool ExecuteSqls(IList<string> Sqls)
        {
            try
            {
                return commonDal.ExecuteSqls(Sqls);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return false;
            }
        }

        /// <summary>
        /// 批量执行sql语句
        /// </summary>
        /// <param name="Sqls"></param>
        /// <returns></returns>
        public string ExecuteSqlsAT(IList<string> Sqls)
        {
            try
            {
                return Convert.ToString(commonDal.ExecuteSqls(Sqls));
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return "系统抛出异常：" + ex;
            }
        }

        public void ChangeTXTBillStatus(string cAction, string masterPKValue, string ClassName)
        {
            DataTable dtConfig = commonDal.GetSingle("select * from t_SYModManage_Config where serialno = (select serialno from t_SYModManage where ClassName='" + ClassName + "')");
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                string sql = "";
                string masterPKField = dtConfig.Rows[0]["masterPKField"].ToString();
                string masterTableName = dtConfig.Rows[0]["masterTableName"].ToString();
                string checkField = dtConfig.Rows[0]["checkField"].ToString();
                string approveField = dtConfig.Rows[0]["approveField"].ToString();
                string confirmField = dtConfig.Rows[0]["confirmField"].ToString();
                if (cAction == "弃审")
                    sql = "update " + masterTableName + " set " + checkField + "=0 where " + masterPKField + "='" + masterPKValue + "'";
                if (cAction == "审批")
                    sql = "update " + masterTableName + " set " + approveField + "=1 where " + masterPKField + "='" + masterPKValue + "'";
                if (cAction == "反审批")
                    sql = "update " + masterTableName + " set " + approveField + "=0," + checkField + "=0 where " + masterPKField + "='" + masterPKValue + "'";
                if (cAction == "确认")
                    sql = "update " + masterTableName + " set " + confirmField + "=1 where " + masterPKField + "='" + masterPKValue + "'";
                if (cAction == "撤销确认")
                    sql = "update " + masterTableName + " set " + confirmField + "=0," + approveField + "=0," + checkField + "=0 where " + masterPKField + "='" + masterPKValue + "'";
                if (sql != "")
                    commonDal.ExcuteSQL(sql);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="rowMu"></param>
        /// <param name="gv"></param>
        /// <param name="e"></param>
        /// <param name="thisRow"></param>
        public bool ExecSprocApp(DataRow rowMu, string formName, string serialno)
        {
            DataTable dtMaster = SearchMasterBySerialno(formName, serialno);
            string strDebug = "";
            string spName = Convert.ToString(rowMu["MenuSproc"]);
            string param = Convert.ToString(rowMu["MenuSprocParam"]);
            string rule = Convert.ToString(rowMu["MenuSprocRule"]);

            if (spName.Equals(""))
            {
                return false;
            }
            if (param.Equals(""))
            {
                return false;
            }

            param = param.Replace('|', ';');
            string[] params_ = param.Split(new char[] { ';' });
            //int[] handles = gv.GetSelectedRows();

            DataTable[] dts = new DataTable[params_.Length];
            strDebug = "Sql:" + spName + ";";
            for (int i = 0; i < params_.Length; i++)
            {
                string field = StringHandler.GetStrBefore(params_[i], "-");
                string flag = StringHandler.GetStrAfter(params_[i], "-");
                //参数对应的值
                //string value = "";
                object value;

                if (flag.ToUpper() == "SY")
                    value = "Admin";
                else
                    value = dtMaster.Rows[0][field];

                DataTable dtemp = new DataTable();
                dtemp.TableName = "table" + i;
                //dtemp.Columns.Add(field, Type.GetType("System.String"));
                dtemp.Columns.Add(field, value.GetType());
                dtemp.Rows.Add(new object[] { value });
                dts[i] = dtemp;
            }

            string guid = rowMu["Guid"].ToString();
            ExecMenuProcedure(guid, dts);

            return true;
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2018-01-17</para>
        /// <para>查询生产制单数据</para>
        /// </summary>
        /// <param name="orderno">制单号</param>
        /// <returns></returns>
        public DataTable[] GetWorkOrderInfor(string orderno)
        {
            if (orderno == "")
                return null;

            try
            {
                DataTable[] dts = new DataTable[8];

                string sql = "select * from workorder where orderno='" + orderno + "'";
                dts[0] = commonDal.GetSingle(sql);
                dts[0].TableName = "workorder";
                string serialno = dts[0].Rows[0]["serialno"].ToString();

                sql = "select * from Ordersize where serialno='" + serialno + "'";
                dts[1] = commonDal.GetSingle(sql);
                dts[1].TableName = "Ordersize"; 

                sql = "select * from ordercolor where serialno='" + serialno + "'";
                dts[2] = commonDal.GetSingle(sql);
                dts[2].TableName = "ordercolor";

                sql = "select * from ordercolor1 where serialno='" + serialno + "'";
                dts[3] = commonDal.GetSingle(sql);
                dts[3].TableName = "ordercolor1";

                sql = "select * from ordermark where serialno='" + serialno + "'";
                dts[4] = commonDal.GetSingle(sql);
                dts[4].TableName = "ordermark";

                sql = "select * from shipplan where fserialno='" + serialno + "'";
                dts[5] = commonDal.GetSingle(sql);
                dts[5].TableName = "shipplan";

                sql = "select * from shipplandl where serialno in (select serialno from shipplan where fserialno='" + serialno + "')";
                dts[6] = commonDal.GetSingle(sql);
                dts[6].TableName = "shipplandl";

                sql = "select * from ordcolorsize where serialno='" + serialno + "'";
                dts[7] = commonDal.GetSingle(sql);
                dts[7].TableName = "ordcolorsize";

                return dts;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }

        }
    }
}
