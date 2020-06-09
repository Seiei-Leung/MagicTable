using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbFactory;
using IDAL;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Configuration;
using Model;
using System.Collections;
using System.ServiceModel.Security;

namespace Bll
{
    public class ReceivablesBll : BaseBll
    {
        internal IReceivables AutRecBill;
        SystemLogBll log = null;

        public ReceivablesBll()
        {
            AutRecBill = base.GetDal.ReceivablesDAL;
            log = new SystemLogBll();
        }
        #region 应收管理
        /// <summary>
        #region 银行帐户科目设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetGy_BankAccount()
        {
            try
            {
                string Sql = "select a.*,b.Cname from t_Gy_BankAccount a left join dbo.t_ma_AccCode b on a.AccCode=b.Ccode";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_Gy_BankAccount";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        #region 会计科目基础
        /// </summary>
        /// <returns></returns>
        public DataTable Get_t_maAccCout()
        {
            try
            {
                string Sql = "select  CClass,Ccode,AssCode,Cname from t_ma_AccCode";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_ma_AccCode";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        #region 用于会计科目设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetRP_InputCode()
        {
            try
            {
                string Sql = "select * from t_RP_InputCode ";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_RP_InputCode";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
            }
        }
        #endregion
        /// <summary>
        #region 查询其初列表
        /// </summary>
        /// <param name="Date_First">开始时间</param>
        /// <param name="Date_end">结束时间</param>
        /// <param name="Custer">客户</param>
        /// <param name="Dept">部门</param>
        /// <param name="Cur">币别</param>
        /// <returns></returns>
        public DataTable GetV_QcArNote(string Date_First, string Date_end, string Custer, string Dept, string Cur)
        {
            try
            {
                StringBuilder sp = new StringBuilder();
                sp.Append("SELECT * FROM Ar_v_QcArNote  where 1=1 and RPFlag = 'AR' and StartFlag = 1  ");
                if (Date_First.ToString().Trim() != "")
                {
                    sp.Append("and BillDate>='"+Date_First+"'");
                }
                if (Date_end.ToString().Trim()!=string.Empty)
                {
                    sp.Append("and BillDate<='"+Date_end+"'");
                }
                if (Custer.ToString().Trim() !=string.Empty)
                {
                    sp.Append("and PSCode='" + Custer + "'");
                }
                if (Dept.ToString().Trim() != string.Empty)
                {
                    sp.Append("and DeptCode='" + Dept + "'");
                }
                if (Cur.ToString().Trim() != string.Empty)
                {
                    sp.Append("and ForeignCurrCode='"+Cur+"'");
                }
                DataTable dt = AutRecBill.GetSingle(sp.ToString());
                if (dt != null)
                {
                    dt.TableName = "QcArNote";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
            }

        }
        #endregion
        /// <summary>
        #region 基础资料列表币别、部门、客户
        /// </summary>
        /// <returns></returns>
        public DataSet GetMasterCur_Dept_cust()
        {
            try
            {
                string Sql = "select ForeignCurrCode,ForeignCurrName from t_FI_ForeignCurrency select DeptCode,DeptProp from t_Gy_Department  select CusCode,CusName,Address from t_FI_Customer ";
                DataSet ds = AutRecBill.Query(Sql);
                if (ds != null)
                {
                    ds.Tables[0].TableName = "t_Gy_ForeignCurrency";
                    ds.Tables[1].TableName = "t_Gy_Department";
                    ds.Tables[2].TableName = "t_Gy_Customer";
                }
                return ds;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
            }
        }
        #endregion
        /// <summary>
        #region 发票资料
        #region 发票类型数据实体
        /// </summary>
        /// <returns></returns>
        public DataTable select_FIinv()
        {
            try
            {
                string Sql = "select  * from t_FIinv_type";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIinv_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        #region 发票树对表格查询
        /// </summary>
        /// <returns></returns>
        public DataTable select2_FIinv(string inv)
        {
            try
            {
                string Sql = "select  * from t_FIinv_type where Invno ='" + inv + "'";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIinv_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        #region 发票树结构得到实体
      
        public DataTable select_FIinv_tree()
        {
            try
            {
                string Sql = "select distinct Invno,Invname from t_FIinv_type";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIinv_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 创建人：王琳
        /// <para>日期：2012-12-10</para>
        #region 用户查询
        /// </summary>
        public DataTable select_SYUsers()
        {
            try
            {
                string Sql = "select  Code,Name from t_SYUsers";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYUsers";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 创建人：王琳
        /// <para>日期：2012-12-10</para>
        #region <para>发票新增</para>
        /// </summary>
        public int add_FIinv(string inv_no, string inv_name, string inv_frate, string inv_type, string act_sw, string org_no, string update_date, string user_no, string inv_name1)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FIinv_type(Invno,Invname,Invfrate,Invtype,Actsw,Orgno,Updatedate,Userno,Invname1) values ";
                strSQL += "('" + inv_no + "','" + inv_name + "','" + inv_frate + "','" + inv_type + "','" + act_sw + "','" + org_no + "','" + update_date + "','" + user_no + "','" + inv_name1 + "')";

                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
          
        }
        #endregion
        /// <summary>
        /// 创建人：王琳
        /// <para>日期：2012-12-11</para>
        #region <para>发票删除</para>
        /// </summary>
        public int Delete_FIinv(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FIinv_type where Invno=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #endregion 
        #region 应收单据类型设置
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-13
        #region 得到数据实体
        public DataTable select_FIsheet()
        {
            try
            {
              
                string Sql = "select * from t_FIsheet_type";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIsheet_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-13
        #region 单据类型新增
       
        /// </summary>
        public int add_FIsheet_type2(string sheet_type, string sheet_name, string serial_code_type, string serial_head, string serial_month, string serial_length, string act_sw, string org_no, DateTime update_date, string user_no, string Sheet_kind, string Invoice)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FIsheet_type(sheet_type, sheet_name, serial_code_type, serial_head, serial_month, serial_length, act_sw, org_no, update_date, user_no, Sheet_kind, Invoice) values ";
                stSQL += "('" + sheet_type + "','" + sheet_name + "','" + serial_code_type + "','" + serial_head + "','" + serial_month + "','" + serial_length + "','" + act_sw + "','" + org_no + "','" + update_date + "','" + user_no + "','" + Sheet_kind + "','" + Invoice + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
       
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-13
        #region 单据树结构得到实体
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-13
        public DataTable select_FIsheet_tree()
        {
            try
            {
                string Sql = "select sheet_type,sheet_name from t_FIsheet_type";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIsheet_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-13
        #region 单据类型删除
        /// </summary>
        /// 王琳
        /// 创建日期：2012-12-13
        public int Delete_FIsheet(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FIsheet_type where ID=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 单据更新
        /// 更新数据
        /// </summary>
        public int update_FIsheet(string sheet_type, string sheet_name, string serial_code_type, string serial_head, string serial_month, string serial_length, string act_sw, string org_no, DateTime update_date, string user_no, string Sheet_kind, string Invoice, int ID)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FIsheet_type set sheet_type='" + sheet_type + "',sheet_name='" + sheet_name + "',serial_code_type='" + serial_code_type + "',serial_head='" + serial_head + "',";
                strSQL += "serial_month='" + serial_month + "',serial_length=" + serial_length + ",act_sw='" + act_sw + "',org_no='" + org_no + "',update_date='" + update_date + "',";
                strSQL += "user_no='" + user_no + "',Sheet_kind='" + Sheet_kind + "',Invoice='" + Invoice + "' where  ID=" + ID + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        #region 发票查询
        /// </summary>
        public DataTable select_FIinv2()
        {
            try
            {
                string Sql = "select  Invno,Invname from t_FIinv_type";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIinv_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 业务类型
        /// </summary>
        public DataTable select_FIdus()
        {
            try
            {
                string Sql = "select Code,Name from t_FIdus_type";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIdus_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #endregion
        #endregion
        #region 帐龄区间设置
        #region 得到数据实体
        public DataTable select_FIorpm()
        {
            try
            {
                string Sql = "select * from t_FIorpm_age";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIorpm_age";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 帐龄新增
        /// </summary>
        public int add_FIorpm(string iage_no, string age_name, string days1, string days2, string act_sw, string org_no, DateTime update_date, string user_no)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FIorpm_age(iage_no, age_name, days1, days2, act_sw, org_no, update_date, user_no) values ";
                strSQL += "('" + iage_no + "','" + age_name + "','" + days1 + "','" + days2 + "','" + act_sw + "','" + org_no + "','" + update_date + "','" + user_no + "')";

                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }

        }
        #endregion

        #region 更新数据
        public int update_FIorpm(string iage_no, string age_name, string days1, string days2, string act_sw, string org_no, DateTime update_date, string user_no)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FIorpm_age set iage_no='" + iage_no + "',age_name='" + age_name + "',days1='" + days1 + "',days2 ='" + days2 + "',";
                strSQL += "act_sw='" + act_sw + "',org_no=" + org_no + ",update_date ='" + update_date + "',user_no ='" + user_no + "'";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 帐龄删除
        /// </summary>
        public int Delete_FIorpm(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FIorpm_age where inv_no=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion

        #endregion
        #region 应收票据管理
        #region 应收票据数据实体
        /// </summary>
        /// <returns></returns>
        public DataTable select_bill()
        {
            try
            {
                string Sql = "select  * from t_FI_orpm_bill";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_orpm_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 应收票据树结构得到实体 

        public DataTable select_bill_tree()
        {
            try
            {
                string Sql = "select distinct Itemno,Custname1 from t_FI_orpm_bill";//查询不重复的数据
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_orpm_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 应收票据树对表格查询
        /// </summary>
        /// <returns></returns>
        public DataTable select2_bill(string inv)
        {
            try
            {
                string Sql = "select  * from t_FI_orpm_bill where Itemno ='" + inv + "'";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_orpm_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 查单号
        /// </summary>
        /// <returns></returns>
        public DataTable select_billd()
        {
            try
            {
                string Sql = "select top 1  max(ID),Sheetno from t_FI_orpm_bill group by Sheetno";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_orpm_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 币别查询
        /// </summary>
        /// <returns></returns>
        public DataTable select_SYCur()
        {
            try
            {
                string Sql = "select Code,[Name] from t_SYCurrency";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYCurrency";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion 
        #region 客户
        /// </summary>
        /// <returns></returns>
        public DataTable select_SYCust()
        {
            try
            {
                string Sql = "select Code,ShortName from t_SYCustomer";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYCustomer";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 客户名称查询
        /// </summary>
        /// <returns></returns>
        public DataTable OneDepNameR(string dc)
        {
            try
            {
                string sql = "select ShortName from t_SYCustomer where Code='" + dc + "'";
                DataTable dt = AutRecBill.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYCustomer";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        #endregion
        #region 币别汇率查询
        /// </summary>
        /// <returns></returns>
        public DataTable SYCurName(string dc)
        {
            try
            {
                string sql = "select Rate from t_SYCurrency where Code='" + dc + "'";
                DataTable dt = AutRecBill.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYCurrency";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        #endregion
        #region 应收票据新增

        /// </summary>
        public int add_FIorpm_bill(string a1, string a2, DateTime a3, string a4, string a5, DateTime a6, DateTime a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_orpm_bill(Sheettype,Sheetno,Sheetdate,Billtype,Billno,Fromdate,Todate,Curno,Currate,Billamt,Interestrate,Itemno,Custname1,Bankno,Accountno,Empno,Payno) values ";
                stSQL += "('" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 + "','" + a11 + "','" + a12 + "','" + a13 + "','" + a14 + "','" + a15 + "','" + a16 + "','" + a17 + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        #endregion
        #region 票据更新
       
        /// </summary>
        public int update_FIorpm_bill(string a1, DateTime a3, string a4, string a5, DateTime a6, DateTime a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17,int ID)
        {
            try
            {
               
                string strSQL = string.Empty;
                strSQL = "update t_FI_orpm_bill set Sheettype='" + a1 + "',Sheetdate='" + a3 + "',Billtype='" + a4 + "',Billno='" + a5 + "',";
                strSQL += "Fromdate='" + a6 + "',Todate='" + a7 + "',Curno='" + a8 + "',Currate='" + a9 + "',Billamt='" + a10 + "',";
                strSQL += "Interestrate='" + a11 + "',Itemno='" + a12 + "',Custname1='" + a13 + "',Bankno='" + a14 + "',Accountno='" + a15 + "',Empno='" + a16 + "',Payno='" + a17 + "' where ID=" + ID + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        public int update_FIorpm_bill2(string a1, DateTime a3, string a4, string a5, DateTime a6, DateTime a7, int ID)
        {
            try
            {
        
                string strSQL = string.Empty;
                strSQL = "update t_FI_orpm_bill set Sheettype='" + a1 + "',Sheetdate='" + a3 + "', Billtype='" + a4 + "',Billno='" + a5 + "','" + a6 + "',Todate='" + a7 + "'  where ID=" + ID + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 票据删除
        /// </summary>
        /// 王琳
        /// 创建日期：2013-1-7
        public int Delete_FIorpm_bill(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_orpm_bill where ID=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode  , IP);
                return 0;
            }
        }
        #endregion
        #region 单据类型查询
        /// </summary>
        /// <returns></returns>
        public DataTable select_Sheetb()
        {
            try
            {
                string Sql = "SELECT sheet_type,sheet_name,serial_code_type,serial_head,serial_month,serial_length,Sheet_kind,Name,Invoice,Invname FROM v_FIsheet_Type where Sheet_kind='ys02'";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "v_FIsheet_Type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #endregion
        #region 汇兑损溢
        #region 汇兑损溢数据实体
        /// </summary>王军繁 2013-1-11
        /// <returns></returns>
        public DataTable select_los()
        {
            try
            {
                string Sql = "select * from t_FI_Bill_los";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Bill_los";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 汇兑损溢新增

        /// </summary>
        //public int add_Bill_los(string a1, string a2, DateTime a3, string a4, string a5, string a6, string a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17)
        //{
        //    try
        //    {
        //        string stSQL = string.Empty;
        //        stSQL = "insert into t_FI_Bill_los(Sheettype,Sheetno,Sheetdate,Itemno,Custname1,Bankno,Empno,Paytype,Curno,Itemno1,Custname,Currate,Rectype,Sheetamt,Billno,Disamt,Rem) values ";
        //        stSQL += "('" + a1 + "','" + a2 + "'," + a3 + ",'" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 + "','" + a11 + "','" + a12 + "')";
        //        stSQL += "(,'" + a13 + "','" + a14 + "','" + a15 + "','" + a16 + "','" + a17 + "')";
        //        return AutRecBill.ExcuteSQL(stSQL);
        //    }
        //    catch (SecurityAccessDeniedException e)
        //    {
        //        log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
        //        return 0;
        //    }
        //}
        public int add_Bill_los(string a1, string a2, DateTime a3, string a4, string a5, string a6, string a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_Bill_los(Sheettype,Sheetno,Sheetdate,Itemno,Custname1,Bankno,Empno,Paytype,Curno,Itemno1,Custname,Currate,Rectype,Sheetamt,Billno,Disamt,Rem) values ";
                stSQL += "('" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 + "','" + a11 + "','" + a12 + "','" + a13 + "','" + a14 + "','" + a15 + "','" + a16 + "','" + a17 + "')";
                //stSQL += "(,'" + a13 + "','" + a14 + "','" + a15 + "','" + a16 + "','" + a17 + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 汇兑损溢更新

        /// </summary>
        public int update_Bill_los(string a1, string a2, DateTime a3, string a4, string a5, string a6, string a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_Bill_los set Sheettype='" + a1 + "',Sheetdate='" + a3 + "',Itemno='" + a4 + "',Custname1='" + a5 + "',";
                strSQL += "Bankno='" + a6 + "',Empno=" + a7 + ",Paytype='" + a8 + "',Curno='" + a9 + "',Itemno1='" + a10 + "',";
                strSQL += "Custname='" + a11 + "',Currate='" + a12 + "',Rectype='" + a13 + "',Sheetamt='" + a14 + "',";
                strSQL += "Billno='" + a15 + "',Disamt='" + a16 + "',Rem='" + a17 + "' where Sheetno=" + a2 + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 删除
        /// </summary>
        /// 王琳
        /// 创建日期：2013-1-7
        public int Delete_Bill_los(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from Bill_los where Sheetno=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #endregion
        #region 应收月结
        #region 得到当前年月
        public DataTable select_Carry()
        {
            try
            {

                string Sql = "select max(Yearm)as Yearm from t_FI_Carry";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Carry";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-16
        #region 结帐新增

        /// </summary>
        public int add_Carry(string Yearm)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_Carry(Yearm) values ";
                stSQL += "('" + Yearm + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐删除
        /// </summary>
        /// 王琳
        /// 创建日期：2012-12-16
        public int Delete_Carry(int Yearm)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_Carry where Yearm=" + Yearm + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐表发票余额新增

        /// </summary>
        public int add_Carry_iov(string Yearm)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_Carry(Yearm,[Year],[Month],CusID,Sheet,Source) ";
                stSQL += "select BillDate,YEAR,MONTH,CustCode,Totamt,Totamt1 from v_FI_Carry_Orpm where Totamt>'0' and BillDate='" + Yearm + "'";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐表发票余额更新状态

        /// </summary>
        public int update_Carry_iov()
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_Carry set Sheetsta='1' where CusID>'0'";
               
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐表发票余额删除状态
        /// </summary>
        /// 王琳
        /// 创建日期：2013-1-22
        public int Delete_Carry_iov(int Yearm)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_Carry where Yearm=" + Yearm + " and Sheetsta='1'";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 查询删除月份是否有数据
        /// </summary>
        /// 王琳
        /// 创建日期：2013-1-22
       
        public DataTable select_Carry_iov(string Yearm)
        {
            try
            {
                string Sql = "select top 1 CusID from t_FI_Carry where Yearm=" + Yearm + " and CusID>'0'";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Carry";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 查询发票是否审核
        public DataTable select_BillNo(string dc)
        {
            try
            {
                string sql = "select top 1 BillNo from t_FI_orpm_inv1 where BillStatus='0' and left(convert(varchar(12),BillDate,112),6)='" + dc + "'";
                DataTable dt = AutRecBill.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_orpm_inv1";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        #endregion
        #endregion
        #region 未审核发票
        #region 查询未审核发票
        public DataTable select_BillNo2(string dc)
        {
            try
            {
                string sql = "select BillDate, BillNo from t_FI_orpm_inv1 where BillStatus='0' and left(convert(varchar(12),BillDate,112),6)='" + dc + "'";
                DataTable dt = AutRecBill.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_orpm_inv1";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        #endregion
        #endregion
       
        #endregion
        #region 应付管理
        #region 应付单据类型设置
        /// <summary>
        /// 王琳
        /// 创建日期：2013-2-18
        #region 得到数据实体
        public DataTable select_FIsheet_Pay()
        {
            try
            {

                string Sql = "select * from t_FIsheet_type2";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIsheet_type2";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2012-12-13
        #region 单据类型新增

        /// </summary>
        public int add_FIsheet_type2_Pay(string sheet_type, string sheet_name, string serial_code_type, string serial_head, string serial_month, string serial_length, string act_sw, string org_no, DateTime update_date, string user_no, string Sheet_kind, string Invoice)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FIsheet_type2(sheet_type, sheet_name, serial_code_type, serial_head, serial_month, serial_length, act_sw, org_no, update_date, user_no, Sheet_kind, Invoice) values ";
                stSQL += "('" + sheet_type + "','" + sheet_name + "','" + serial_code_type + "','" + serial_head + "','" + serial_month + "','" + serial_length + "','" + act_sw + "','" + org_no + "','" + update_date + "','" + user_no + "','" + Sheet_kind + "','" + Invoice + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        #endregion
        #region 单据树结构得到实体
        /// <summary>
        /// 王琳
        /// 创建日期：2013-2-18
        public DataTable select_FIsheet_tree_Pay()
        {
            try
            {
                string Sql = "select sheet_type,sheet_name from t_FIsheet_type2";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FIsheet_type";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2013-2-18
        #region 单据类型删除
        /// </summary>
        /// 王琳
        /// 创建日期：2013-2-18
        public int Delete_FIsheet_Pay(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FIsheet_type2 where ID=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 单据更新
        /// 更新数据
        /// </summary>
        public int update_FIsheet_Pay(string sheet_type, string sheet_name, string serial_code_type, string serial_head, string serial_month, string serial_length, string act_sw, string org_no, DateTime update_date, string user_no, string Sheet_kind, string Invoice, int ID)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FIsheet_type2 set sheet_type='" + sheet_type + "',sheet_name='" + sheet_name + "',serial_code_type='" + serial_code_type + "',serial_head='" + serial_head + "',";
                strSQL += "serial_month='" + serial_month + "',serial_length=" + serial_length + ",act_sw='" + act_sw + "',org_no='" + org_no + "',update_date='" + update_date + "',";
                strSQL += "user_no='" + user_no + "',Sheet_kind='" + Sheet_kind + "',Invoice='" + Invoice + "' where  ID=" + ID + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #endregion
        #region 应付票据管理
        /// <summary>
        /// 王琳
        /// 创建日期：2013-2-21
        #region 应付票据数据实体
        /// </summary>
        /// <returns></returns>
        public DataTable select_opbill()
        {
            try
            {
                string Sql = "select  * from t_FI_opur_bill";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_opur_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 应付票据树结构得到实体

        public DataTable select_opbill_tree()
        {
            try
            {
                string Sql = "select distinct Itemno,Custname1 from t_FI_opur_bill";//查询不重复的数据
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_opur_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 应付票据树对表格查询
        /// </summary>
        /// <returns></returns>
        public DataTable select2_opbill(string inv)
        {
            try
            {
                string Sql = "select  * from t_FI_opur_bill where Itemno ='" + inv + "'";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_opur_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 查单号
        /// </summary>
        /// <returns></returns>
        public DataTable select_opbilld()
        {
            try
            {
                string Sql = "select top 1  max(ID),Sheetno from t_FI_opur_bill group by Sheetno";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_opur_bill";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 应付票据新增

        /// </summary>
        public int add_FIorpm_opbill(string a1, string a2, DateTime a3, string a4, string a5, DateTime a6, DateTime a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_opur_bill(Sheettype,Sheetno,Sheetdate,Billtype,Billno,Fromdate,Todate,Curno,Currate,Billamt,Interestrate,Itemno,Custname1,Bankno,Accountno,Empno,Payno) values ";
                stSQL += "('" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 + "','" + a11 + "','" + a12 + "','" + a13 + "','" + a14 + "','" + a15 + "','" + a16 + "','" + a17 + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 票据更新

        /// </summary>
        public int update_FIorpm_opbill(string a1, DateTime a3, string a4, string a5, DateTime a6, DateTime a7, string a8, string a9, string a10, string a11, string a12, string a13, string a14, string a15, string a16, string a17, int ID)
        {
            try
            {

                string strSQL = string.Empty;
                strSQL = "update t_FI_opur_bill set Sheettype='" + a1 + "',Sheetdate='" + a3 + "',Billtype='" + a4 + "',Billno='" + a5 + "',";
                strSQL += "Fromdate='" + a6 + "',Todate='" + a7 + "',Curno='" + a8 + "',Currate='" + a9 + "',Billamt='" + a10 + "',";
                strSQL += "Interestrate='" + a11 + "',Itemno='" + a12 + "',Custname1='" + a13 + "',Bankno='" + a14 + "',Accountno='" + a15 + "',Empno='" + a16 + "',Payno='" + a17 + "' where ID=" + ID + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 票据删除
        /// </summary>
        /// 王琳
        /// 创建日期：2013-2-21
        public int Delete_FIorpm_opbill(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_opur_bill where ID=" + id + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        /// <summary>
        /// 王琳
        /// 创建日期：2013-2-22
        #region 应付月结
        #region 得到当前年月
        public DataTable select_Parry()
        {
            try
            {

                string Sql = "select max(Yearm)as Yearm from t_FI_Parry";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Parry";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 结帐新增

        /// </summary>
        public int add_Parry(string Yearm)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_Parry(Yearm) values ";
                stSQL += "('" + Yearm + "')";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐删除
        /// </summary>
        /// 王琳
        /// 创建日期：2013-2-22
        public int Delete_Parry(int Yearm)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_Parry where Yearm=" + Yearm + "";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐表发票余额新增

        /// </summary>
        public int add_Parry_iov(string Yearm)
        {
            try
            {
                string stSQL = string.Empty;
                stSQL = "insert into t_FI_Parry(Yearm,[Year],[Month],CusID,Sheet,Source) ";
                stSQL += "select BillDate,YEAR,MONTH,CustCode,Totamt,Totamt1 from v_FI_Parry_Orpm where Totamt>'0' and BillDate='" + Yearm + "'";

                return AutRecBill.ExcuteSQL(stSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐表发票余额更新状态

        /// </summary>
        public int update_Parry_iov()
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_Parry set Sheetsta='1' where CusID>'0'";

                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 结帐表发票余额删除状态
        /// </summary>
        /// 王琳
        /// 创建日期：2013-1-22
        public int Delete_Parry_iov(int Yearm)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_Parry where Yearm=" + Yearm + " and Sheetsta='1'";
                return AutRecBill.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        #endregion
        #region 查询删除月份是否有数据
        /// </summary>
        /// 王琳
        /// 创建日期：2013-2-22

        public DataTable select_Parry_iov(string Yearm)
        {
            try
            {
                string Sql = "select top 1 CusID from t_FI_Parry where Yearm=" + Yearm + " and CusID>'0'";
                DataTable dt = AutRecBill.GetSingle(Sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Parry";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, "", "");
                return null;
                //throw;
            }
        }
        #endregion
        #region 查询发票是否审核
        public DataTable select_PBillNo(string dc)
        {
            try
            {
                string sql = "select top 1 BillNo from t_FI_Opur_Inv1 where BillStatus='0' and left(convert(varchar(12),BillDate,112),6)='" + dc + "'";
                DataTable dt = AutRecBill.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Opur_Inv1";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        #endregion
        #endregion
        #region 未审核发票
        #region 查询未审核发票
        public DataTable select_PBillNo2(string dc)
        {
            try
            {
                string sql = "select BillDate, BillNo from t_FI_Opur_Inv1 where BillStatus='0' and left(convert(varchar(12),BillDate,112),6)='" + dc + "'";
                DataTable dt = AutRecBill.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Opur_Inv1";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        #endregion
        #endregion
        #endregion
        #endregion
    }
}
