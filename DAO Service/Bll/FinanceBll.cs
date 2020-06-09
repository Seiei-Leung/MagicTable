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
    public class FinanceBll : BaseBll
    {
        internal IFinance finces;
        SystemLogBll log;

        public FinanceBll()
        {
            finces = base.GetDal.Finance;
            log = new SystemLogBll();
        }

        /// <summary>
        /// 获得会计科目表总数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFinance()
        {
            try
            {
                string sql = "select * from t_FI_AccountingSubject";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 查分类数据
        /// </summary>
        /// <param name="zc"></param>
        /// <returns></returns>
        public DataTable GetAllFinanceZC(string zc)
        {
            try
            {
                string sql = "select * from t_FI_AccountingSubject where CClass='" + zc + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 增加会计科目
        /// </summary>
        /// <param name="parentcode"></param>
        /// <param name="ccode"></param>
        /// <param name="cname"></param>
        /// <param name="cclass"></param>
        /// <param name="asscode"></param>
        /// <param name="cpr"></param>
        /// <param name="acc"></param>
        /// <param name="daybookflag"></param>
        /// <param name="accdeadline"></param>
        /// <param name="balanceOri"></param>
        /// <param name="rmbtype"></param>
        /// <param name="measure"></param>
        /// <param name="personflag"></param>
        /// <param name="cusflag"></param>
        /// <param name="supplierflag"></param>
        /// <param name="deptflag"></param>
        /// <param name="itemclass"></param>
        /// <param name="cashflowflag"></param>
        /// <param name="csumcode"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public int add_AccountingSubject(int parentcode, string ccode, string cname, string cclass, string asscode, string cpr, string acc, int daybookflag, DateTime accdeadline, string balanceOri, string rmbtype, string measure, int personflag, int cusflag, int supplierflag, int deptflag, string itemclass, int cashflowflag, string csumcode, int stop)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_AccountingSubject(ParentCode,CCode,Cname,CClass,AssCode,Cproperty,AccFormat,DayBookFlag,AccDeadLine,BalanceOri,RmbType,measure,PersonFlag,CusFlag,SupplierFlag,DeptFlag,ItemClass,CashFlowFlag,cSumCode,sTop) values ";
                strSQL += "(" + parentcode + ",'" + ccode + "','" + cname + "','" + cclass + "','" + asscode + "','" + cpr + "','" + acc + "'," + daybookflag + ",'" + accdeadline + "','" + balanceOri + "','" + rmbtype + "','" + measure + "'," + personflag + "," + cusflag + "," + supplierflag + "," + deptflag + ",'" + itemclass + "'," + cashflowflag + ",'" + csumcode + "'," + stop + ")";

                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 删除带子级科目的会计科目
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int delete_AccountingSubject(string code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_AccountingSubject where CCode='" + code + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch(SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 子级科目与父级科目一同删除
        /// </summary>
        /// <param name="codeAll"></param>
        /// <returns></returns>
        public int delete_AccountingSubject_All(string codeAll)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_AccountingSubject where CCode like '" + codeAll + "%'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 查询子级科目，不包含根科目
        /// </summary>
        /// <param name="ccode"></param>
        /// <returns></returns>
        public DataTable SelectAccountingSubject_Subtree(string ccode)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select * from t_FI_AccountingSubject where CCode like '" + ccode + "%' and CCode != '" + ccode + "'";
                DataTable dt = finces.GetSingle(strSQL);
                //return finces.ExcuteSQL(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 查ID：select Id from t_FI_AccountingSubject where CCode='102'
        /// </summary>
        /// <param name="ccode"></param>
        /// <returns></returns>
        public DataTable SelectAccountingSubject_RootId(string ccode)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select Id from t_FI_AccountingSubject where CCode='" + ccode + "'";
                DataTable dt = finces.GetSingle(strSQL);
                //return finces.ExcuteSQL(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int update_AccountingSubject(string name, string class1, string ass, string cpr, string acc, int dayb, DateTime banktime, string bal, string rmbtype, string dw, int grhs, int khhs, int gyshs, int bmhs, string xmhs, int cash, string csum, int tops, string kmcode)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_AccountingSubject set Cname='" + name + "',CClass='" + class1 + "',AssCode='" + ass + "',Cproperty='" + cpr + "',";
                strSQL += "AccFormat='" + acc + "',DayBookFlag=" + dayb + ",AccDeadLine='" + banktime + "',BalanceOri='" + bal + "',RmbType='" + rmbtype + "',";
                strSQL += "measure='" + dw + "',PersonFlag=" + grhs + ",CusFlag=" + khhs + ",SupplierFlag=" + gyshs + ",DeptFlag=" + bmhs + ",";
                strSQL += "ItemClass='" + xmhs + "',CashFlowFlag=" + cash + ",cSumCode='" + csum + "',sTop=" + tops + "  where  CCode='" + kmcode + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch(Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 期初余额数据GetFrmBeginningBalanceData
        /// </summary>
        /// <returns>dt</returns>
        public DataTable GetFrmBeginningBalanceData()
        {
            try
            {
                string sql = "select * from t_FI_BBalance";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_BBalance";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 期初表(会计科目业务加)
        /// </summary>
        /// <param name="khmc"></param>
        /// <param name="kmfx"></param>
        /// <param name="rmbandjn"></param>
        /// <returns></returns>
        public int Add_BBalance(string kmcode,string khmc, string kmfx,string rmbandjn)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_BBalance(KMCode,KMName,KMfx,RmbtypeAndJn) values ";
                strSQL += "('" + kmcode + "','" + khmc + "','" + kmfx + "','" + rmbandjn + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 期初表(会计科目业务删)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete_BBalance(string kmcode)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_BBalance where KMCode='" + kmcode + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 期初表(会计科目业务改)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kmname"></param>
        /// <param name="kmfx"></param>
        /// <param name="rmbtypeandjn"></param>
        /// <returns></returns>
        public int Update_BBalance(string kmcode, string kmname, string kmfx, string rmbtypeandjn)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_BBalance set KMName='" + kmname + "',KMfx='" + kmfx + "',RmbtypeAndJn='" + rmbtypeandjn + "' where KMCode=" + kmcode + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 凭证类别数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllPZCategory()
        {
            try
            {
                string sql = "select * from t_FI_PZCategory";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_PZCategory";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// from to PZ seachlookupedit 
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAcc()
        {
            try
            {
                string sql = "select CCode,Cname,CClass from t_FI_AccountingSubject";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// t_FI_PZCategory ++++
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public int Add_PZCategory(string a, string b, string c, string d)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_PZCategory(CategoryZ,CategoryName,ConstraintType,ConstraintKM) values ";
                strSQL += "('" + a + "','" + b + "','" + c + "','" + d + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        public int Update_PZCategory(string a, string b, string c, string d,int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_PZCategory set CategoryZ='" + a + "',CategoryName='" + b + "',ConstraintType='" + c + "',ConstraintKM='"+d+"' where Id=" + id + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        public int Delete_PZCategory(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_PZCategory where Id=" + id + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 凭证类别与凭证名称
        /// </summary>
        /// <returns></returns>
        public DataTable SelectLookup()
        {
            try
            {
                string sql = "select CategoryZ,CategoryName from t_FI_PZCategory";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_PZCategory";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 查凭证子表t_FI_PZSonTable
        /// </summary>
        /// <returns></returns>
        public DataTable SelectPZSonTable()
        {
            try
            {
                string sql = "select PZCode,Zi,Hao,ZaiYao,KMName,Money_Je,Money_Dai from t_FI_PZSonTable";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_PZSonTable";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 查摘要表t_FI_ZaiYaoTable
        /// </summary>
        /// <returns></returns>
        public DataTable SelectZaiYaoTable()
        {
            try
            {
                string sql = "select * from t_FI_ZaiYaoTable";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ZaiYaoTable";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 摘要增加
        /// </summary>
        public int Add_ZaiYaoTable(string zy1, string zy2, string zy3)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_ZaiYaoTable(ZYCode,ZYTxt,ZYKM) values ";
                strSQL += "('" + zy1 + "','" + zy2 + "','" + zy3 + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 摘要更新
        /// </summary>
        public int Update_ZaiYaoTable(string zy1, string zy2, string zy3,int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_ZaiYaoTable set ZYCode='"+zy1+"',ZYTxt='"+zy2+"',ZYKM='"+zy3+"' where Id="+id+"";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 摘要删除
        /// </summary>
        public int Delete_ZaiYaoTable(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_ZaiYaoTable where Id=" + id + "";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 查摘要表t_FI_ZaiYaoTable1
        /// </summary>
        /// <returns></returns>
        public DataTable SelectZaiYaoTable1()
        {
            try
            {
                string sql = "select ZYCode,ZYTxt,ZYKM from t_FI_ZaiYaoTable";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ZaiYaoTable";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 查凭证主表t_FI_PZFatherTable
        /// </summary>
        /// <returns></returns>
        public DataTable SelectPZFatherTable(string zi)
        {
            try
            {
                string sql = "select Hao from t_FI_PZFatherTable where Zi='" + zi + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_PZFatherTable";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 让凭证子表返回空
        /// </summary>
        /// <returns></returns>
        public DataTable selectTopPZSonTb()
        {
            try
            {
                string sql = "select top 0 Id,PZCode,Zi,Hao,ZaiYao,KMName,Money_Je,Money_Dai from t_FI_PZSonTable";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_PZSonTable";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 凭证主表增加
        /// </summary>
        public int Add_PZFatherTable(string r,string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k, string l, string m, string n, string o, string p, string q)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_PZFatherTable(PZCode,Zi,Hao,Zd_Date,DNum,PHao,PDate,PNum,P_Jia,P_Project,P_Cust,P_Dep,P_Ywy,P_Person,JZ,SH,CN,ZD) values ";
                strSQL += "('"+r+"','" + a + "','" + b + "','" + c + "','" + d + "','" + e + "','" + f + "','" + g + "','" + h + "','" + i + "','" + j + "','" + k + "','" + l + "','" + m + "','" + n + "','" + o + "','" +p + "','" + q + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 查所有凭证主表数据
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllPZFatherTb()
        {
            try
            {
                string sql = "select * from t_FI_PZFatherTable";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_PZFatherTable";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable SelectNewColToKMTable()
        {
            try
            {
                string sql = "select CCode,CCode+'    '+Cname as NewCol from t_FI_AccountingSubject";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 据科目编码查名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable SelectKMName(string code)
        {
            try
            {
                string sql = "select Cname from t_FI_AccountingSubject where CCode='" + code + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccountingSubject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 会计年度方法
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int ExecYearData(string year)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_Year(Years) values ('" + year + "');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',1,'" + year + "'+'-01-01','" + year + "'+'-01-31');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',2,'" + year + "'+'-02-01','" + year + "'+'-02-29');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',3,'" + year + "'+'-03-01','" + year + "'+'-03-31');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',4,'" + year + "'+'-04-01','" + year + "'+'-04-30');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',5,'" + year + "'+'-05-01','" + year + "'+'-05-31');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',6,'" + year + "'+'-06-01','" + year + "'+'-06-30');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',7,'" + year + "'+'-07-01','" + year + "'+'-07-31');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',8,'" + year + "'+'-08-01','" + year + "'+'-08-31');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',9,'" + year + "'+'-09-01','" + year + "'+'-09-30');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',10,'" + year + "'+'-10-01','" + year + "'+'-10-31');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',11,'" + year + "'+'-11-01','" + year + "'+'-11-30');";
                strSQL += "insert into t_FI_AccPeriod(Years,QJ,StartDate,EndDate) values ('" + year + "',12,'" + year + "'+'-12-01','" + year + "'+'-12-31')";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 查年表
        /// </summary>
        /// <returns></returns>
        public DataTable SelectYearTb()
        {
            try
            {
                string sql = "select * from t_FI_Year";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Year";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 查年期间表
        /// </summary>
        /// <returns></returns>
        public DataTable SelectYearQJTb()
        {
            try
            {
                string sql = "select Years,QJ,StartDate,EndDate from t_FI_AccPeriod";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AccPeriod";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 最多年度
        /// </summary>
        /// <returns></returns>
        public DataTable TopYear()
        {
            try
            {
                string sql = "select top 1 Years from t_FI_Year order by Years desc";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_Year";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 删除年度数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int ExecDeleteYearData(string year)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_Year where Years='" + year + "';";
                strSQL += "delete from t_FI_AccPeriod where Years='" + year + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 成本中心数据
        /// </summary>
        /// <returns></returns>
        public DataTable CostCenterData()
        {
            try
            {
                string sql = "select * from t_FI_CostCenter";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心加
        /// </summary>
        public int Add_CostCenter(string a_Code,string a_Name,string a_sc,string a_ft,string bz)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_CostCenter(CostCenterCode,CostCenterName,ProductAttributes,ShareAttributes,BZ) values ";
                strSQL += "('" + a_Code + "','" + a_Name + "','" + a_sc + "','" + a_ft + "','" + bz + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 更新成本中心
        /// </summary>
        public int Update_CostCenter(string a_Code, string a_Name, string a_sc, string a_ft, string bz,int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_FI_CostCenter set CostCenterCode='" + a_Code + "',CostCenterName='" + a_Name + "',ProductAttributes='" + a_sc + "',ShareAttributes='" + a_ft + "',BZ='" + bz + "' where Id=" + id + "";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 删除成本中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete_CostCenter(int id)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_CostCenter where Id=" + id + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 辅助成本中心服务lookupedit数据源
        /// </summary>
        /// <returns></returns>
        public DataTable CostCenterServerLookupEdeitData()
        {
            try
            {
                string sql = "select CostCenterName+'('+CostCenterCode+')' as newColumn,CostCenterName from t_FI_CostCenter where ProductAttributes='辅助生产'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 辅助成本中心
        /// </summary>
        /// <returns></returns>
        public DataTable AuxiliaryCostCenter()
        {
            try
            {
                string sql = "select * from t_FI_AuxiliaryCostCenter";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AuxiliaryCostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// t_FI_AuxiliaryCostCenter加
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="c1"></param>
        /// <param name="d1"></param>
        /// <returns></returns>
        public int Add_AuxiliaryCostCenter(string a1, string b1, string c1, string d1)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_AuxiliaryCostCenter(AuxiliaryCostCenter,AuxiliaryCostCenterCode,AuxiliaryCostCenterName,AuxiliaryCostCenterDW) values ";
                strSQL += "('" + a1 + "','" + b1 + "','" + c1 + "','" + d1 + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 据辅助成本中心查服务列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable SelectAuxiliaryCostCenter(string str)
        {
            try
            {
                string sql = "select * from t_FI_AuxiliaryCostCenter where AuxiliaryCostCenter='" + str + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_AuxiliaryCostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 删除辅助成本中心
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public int Delete_AuxiliaryCostCenter(string sr)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_AuxiliaryCostCenter where AuxiliaryCostCenterCode='" + sr + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// (视图)v_FICostCenter=SELECT t_FI_CostCenter.CostCenterName,t_FI_AuxiliaryCostCenter.AuxiliaryCostCenter
        ///FROM t_FI_CostCenter LEFT OUTER JOIN
        ///t_FI_AuxiliaryCostCenter ON t_FI_CostCenter.CostCenterName = t_FI_AuxiliaryCostCenter.AuxiliaryCostCenter
        ///WHERE(t_FI_CostCenter.ProductAttributes = '辅助生产')
        /// </summary>
        /// <returns></returns>
        public DataTable SelectView1()
        {
            try
            {
                //string sql = "select * from v_FICostCenter where AuxiliaryCostCenter is null or AuxiliaryCostCenter=''";查所有为空数据
                string sql = "select top 1 CostCenterName from v_FICostCenter where AuxiliaryCostCenter is null or AuxiliaryCostCenter=''";//查第一条为空数据
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "v_FICostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 树视图（改用主从）主表
        /// </summary>
        /// <returns></returns>
        public DataTable TreeListViewData()
        {
            try
            {
                string sql = "select distinct productattributes from t_FI_CostCenter";//主table
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 树视图（改用主从）从表
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public DataTable TreeListViewData1(string f)
        {
            try
            {
                string sql = "select * from t_FI_CostCenter where productattributes='" + f + "'";//从table
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenter1";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心对照表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCostCenterContrast()
        {
            try
            {
                string sql = "select * from t_FI_CostCenterContrast";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenterContrast";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心searchlookupedit数据
        /// </summary>
        /// <returns></returns>
        public DataTable CostCenterData1()
        {
            try
            {
                string sql = "select CostCenterCode,CostCenterName,ProductAttributes,ShareAttributes from t_FI_CostCenter";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 得到部门数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepData()
        {
            try
            {
                string sql = "select Code,Name from t_SYDepartment";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYDepartment";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 参数查询成本中心数据
        /// </summary>
        /// <param name="oc"></param>
        /// <returns></returns>
        public DataTable OneCostCenterName(string oc)
        {
            try
            {
                string sql = "select CostCenterName from t_FI_CostCenter where CostCenterCode='" + oc + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenter";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 参数查询部门数据
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public DataTable OneDepName(string dc)
        {
            try
            {
                string sql = "select Name from t_SYDepartment where Code='" + dc + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYDepartment";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心对照加
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="c1"></param>
        /// <param name="d1"></param>
        /// <param name="e1"></param>
        /// <param name="f1"></param>
        /// <param name="g1"></param>
        /// <returns></returns>
        public int Add_CostCenterContrast(string a1, string b1, string c1, string d1,string e1,string f1,string g1)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_CostCenterContrast(CostCenterCode,CostCenterName,DepCode,DepName,WorkCenterCode,WorlCenterName,AttachedDep) values ";
                strSQL += "('" + a1 + "','" + b1 + "','" + c1 + "','" + d1 + "','" + e1 + "','" + f1 + "','" + g1 + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 条件查询成本中心对照
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public DataTable Select_CostCenterContrast(string df)
        {
            try
            {
                string sql = "select * from t_FI_CostCenterContrast where CostCenterCode='" + df + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_CostCenterContrast";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 删除成本中心对照
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public int Delete_CostCenterContrast(string sr)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_CostCenterContrast where CostCenterCode='" + sr + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 成本中心组主表数据
        /// </summary>
        /// <returns></returns>
        public DataTable Select_t_FI_FatherCostCenterGroup()
        {
            try
            {
                string sql = "select * from t_FI_FatherCostCenterGroup";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_FatherCostCenterGroup";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心组从表数据1
        /// </summary>
        /// <returns></returns>
        public DataTable Select_t_FI_ChildrenCostCenterGroup()
        {
            try
            {
                string sql = "select top 0 Id,CostCenterGroupCode,CostCenterCode,CostCenterName,ProductAttributes,BZ from t_FI_ChildrenCostCenterGroup";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ChildrenCostCenterGroup";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心组从表数据2
        /// </summary>
        /// <returns></returns>
        public DataTable Select_t_FI_ChildrenCostCenterGroup2()
        {
            try
            {
                string sql = "select * from t_FI_ChildrenCostCenterGroup";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ChildrenCostCenterGroup";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 成本中心主表加
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="c1"></param>
        /// <returns></returns>
        public int Add_t_FI_FatherCostCenterGroup(string a1, string b1, string c1)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_FatherCostCenterGroup(CostCenterGroupCode,CostCenterGroupName,MS) values ";
                strSQL += "('" + a1 + "','" + b1 + "','" + c1 + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 项目大类表加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public int Add_t_FI_ProjectLargeClassTb(int d,string a, string b, string c)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_ProjectLargeClassTb(ParentCode,ProjectLargeClassCode,ProjectLargeClassName,ProjectLargeClassBZ) values ";
                strSQL += "("+d+",'" + a + "','" + b + "','" + c + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 判断重复数据查询(t_FI_ProjectLargeClassTb表)
        /// </summary>
        /// <param name="naes"></param>
        /// <returns></returns>
        public DataTable Select_t_FI_ProjectLargeClassTb(string naes)
        {
            try
            {
                string sql = "select ProjectLargeClassName from t_FI_ProjectLargeClassTb where ProjectLargeClassName='" + naes + "'";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 查t_FI_ProjectLargeClassTb表项目大类名称数据
        /// </summary>
        /// <returns></returns>
        public DataTable SelectProjectLargeClassTb_ForName()
        {
            try
            {
                string sql = "select ProjectLargeClassName from t_FI_ProjectLargeClassTb where ParentCode=" + 0 + "";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// t_FI_StockProject部分数据
        /// </summary>
        /// <returns></returns>
        public DataTable PartOfTheDataFort_FI_StockProject()
        {
            try
            {
                string sql = "select Id,ProjectLargeClassName,BT,LX,Length from t_FI_StockProject";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_StockProject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// t_FI_StockProject表数据
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAll_t_FI_StockProjectTb()
        {
            try
            {
                string sql = "select * from t_FI_StockProject";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_StockProject";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// t_FI_ProjectLargeClassTb表(treelist_datasource)
        /// </summary>
        /// <returns></returns>
        public DataTable Get_All_t_FI_ProjectLargeClassData()
        {
            try
            {
                string sql = "select Id,ParentCode,ProjectLargeClassCode,ProjectLargeClassName from t_FI_ProjectLargeClassTb";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 查t_FI_ProjectLargeClassTb表子级数据,不包括根数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable Select_treeChildren_For_ProjectLargeClassTb(string code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select Id,ParentCode,ProjectLargeClassCode,ProjectLargeClassName from t_FI_ProjectLargeClassTb where ProjectLargeClassCode like '" + code + "%' and ProjectLargeClassCode!='" + code + "'";
                //strSQL = "select * from t_FI_AccountingSubject where CCode like '" + ccode + "%' and CCode != '" + ccode + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 从t_FI_ProjectLargeClassTb表返回一个分类代码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable Table1(string name)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select ProjectLargeClassCode from t_FI_ProjectLargeClassTb where ProjectLargeClassName='" + name + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch(Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 返回一个参数Table，参数是上面方法返回值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable Table2(string code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select Id,ParentCode,ProjectLargeClassCode,ProjectLargeClassName from t_FI_ProjectLargeClassTb where ProjectLargeClassCode like '" + code + "%'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 删除t_FI_ProjectLargeClassTb表
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public int Delete_t_FI_ProjectLargeClassTb(int sr)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_ProjectLargeClassTb where Id=" + sr + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 添加表的列
        /// </summary>
        /// <param name="TbName">表名</param>
        /// <param name="ColName">列名</param>
        /// <param name="ColType">字段类型</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public int AddColumn(string TbName, string ColName, string ColType, int length)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "alter table " + TbName + " add " + ColName + " " + ColType + "(" + length + ") null";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 添加表的列
        /// </summary>
        /// <param name="TbName">表名</param>
        /// <param name="ColName">列名</param>
        /// <param name="ColType">字段类型</param>
        /// <returns></returns>
        public int AddColumn1(string TbName, string ColName, string ColType)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "alter table " + TbName + " add " + ColName + " " + ColType + " null";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public int DeleteColumn(string TbName, string ColumnName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "alter table " + TbName + " drop column " + ColumnName + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="TbName">表名</param>
        /// <returns></returns>
        public int CreateTb(string TbName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "create table " + TbName + "(ID int identity(1,1) primary key,)";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 删除t_FI_StockProject表数据
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public int Delete_t_FI_StockProject(int sr)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_FI_StockProject where Id=" + sr + "";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 增加行
        /// </summary>
        /// <param name="ProName">项目大类名</param>
        /// <param name="btName">BT</param>
        /// <param name="lxName">LX</param>
        /// <param name="length">Length</param>
        /// <returns></returns>
        public int ExecProjectStructureData(string ProName,string btName,string lxName,int length)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_FI_StockProject(ProjectLargeClassName,BT,LX,Length) values ";
                strSQL += "('" + ProName + "','" + btName + "','" + lxName + "'," + length + ")";
                
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 查项目结构表，根据项目大类
        /// </summary>
        /// <param name="ProjectLargeClassName"></param>
        /// <returns></returns>
        public DataTable Select_Project_JG(string ProjectLargeClassName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select * from t_FI_StockProject where ProjectLargeClassName='" + ProjectLargeClassName + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_FI_ProjectLargeClassTb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// 查询不同的表名数据
        /// </summary>
        /// <param name="TbName"></param>
        /// <returns></returns>
        public DataTable DifferentTableData(string TbName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select * from " + TbName + "";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = TbName;
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        #region IM
        /// <summary>
        /// IM系统里找用户名
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable FindUserName(string code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select Name from t_SYUsers where Code='"+code+"'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_SYUsers";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        public int SaveMsgRecord(string id, string content, int messagetype, string froms, string tos, string datetime, string vcard)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into MsgRecord(ID,Content,MessageType,froms,tos,DateTime,Vcard) ";
                strSQL += "values ('" + id + "','" + content + "'," + messagetype + ",'" + froms + "','" + tos + "','" + datetime + "','" + vcard + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 保存离线消息记录
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public int SaveOutLineIMMessage(string userID, string from, string to, string xml)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into RecordMsg(userID,froms,tos,datetime,MessageXML) values ";
                strSQL += "('" + userID + "','" + from + "','" + to + "','" + DateTime.Now.ToString() + "','" + xml + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 保存最近会话列表
        /// </summary>
        /// <param name="serialno"></param>
        /// <param name="fromUser"></param>
        /// <param name="toUserName"></param>
        /// <param name="toUserId"></param>
        /// <param name="LastMsg"></param>
        /// <returns></returns>
        public int SaveRecentContact(string serialno, string fromUser, string toUserName, string toUserId, string LastMsg)
        {
            //serialno = System.Guid.NewGuid().ToString();
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into iM_RecentContact(serialno,current_Person,name,userId,lastMsg) values ";
                strSQL += "('" + serialno + "','" + fromUser + "','" + toUserName + "','" + toUserId + "','" + LastMsg + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 保存用户所创建的组的主表
        /// </summary>
        /// <returns></returns>
        public int SaveImGroup(string serialno, string creatercode,string creatername, string groupname, DateTime CreateDate)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into T_SY_IM_DiscussionGroup(serialno,CreaterCode,CreaterName,GroupName,CreateDate) values ";
                strSQL += "('" + serialno + "','" + creatercode + "','" + creatername + "','" + groupname + "','" + CreateDate + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 保存用户所创建的组的从表
        /// </summary>
        /// <returns></returns>
        public int SaveImGroupdl(string guid, string serialno, string code, string name)  //先暂时保存这几项数据吧
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into T_SY_IM_DiscussionGroupdl(guid,serialno,Code,Name) values ";
                strSQL += "('" + guid + "','" + serialno + "','" + code + "','" + name + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 用户建讨论组时的临时表
        /// </summary>
        /// <param name="TmpName"></param>
        /// <returns></returns>
        public int ImGroupTmp(string TmpName, string Modifiyer)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into T_SY_IM_DiscussionGroupTmp(TmpName,Modifiyer) values ";
                strSQL += "('" + TmpName + "','" + Modifiyer + "')";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 删除讨论组临时表数据(关闭窗口或点取消按钮时)
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public int ImGroupTmpDel(string Code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from T_SY_IM_DiscussionGroupTmp where Modifiyer='" + Code + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 删除讨论组临时表数据(双击右侧所选列表项数据时)
        /// </summary>
        /// <param name="TmpName"></param>
        /// <param name="Modifiyer"></param>
        /// <returns></returns>
        public int ImGroupTmpDel1(string TmpName,string Modifiyer)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from T_SY_IM_DiscussionGroupTmp where TmpName='" + TmpName + "' and Modifiyer='" + Modifiyer + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }

        /// <summary>
        /// 获取离线消息记录
        /// </summary>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public DataTable GetIMOutLineMessage(string ToUser)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select * from t_SYOutLineMessge where ToUser='" + ToUser + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_SYOutLineMessge";
                }
                string delSql = "delete from t_SYOutLineMessge where ToUser='" + ToUser + "'";
                finces.ExcuteSQL(delSql);
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        
        /// <summary>
        /// 当天的聊天记录
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public DataTable SelectMessageLogToday(string fromUser,string toUser)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select SendTime,MessageTxt,Images from IMMessageDb where SendTime between '" + System.DateTime.Now.Date + "' and dateadd(day,1,getdate()) and FromUser='" + fromUser + "' and ToUser='" + toUser + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "IMMessageDb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 最近两天的聊天记录
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public DataTable SelectMessageLog2Today(string fromUser, string toUser)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select SendTime,MessageTxt,Images from IMMessageDb where SendTime between getdate()-2 and dateadd(day,1,getdate()) and FromUser='" + fromUser + "' and ToUser='" + toUser + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "IMMessageDb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 最近一周的聊天记录
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public DataTable SelectMessageLog7Today(string fromUser, string toUser)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select SendTime,MessageTxt,Images from IMMessageDb where SendTime between getdate()-7 and dateadd(day,1,getdate()) and FromUser='" + fromUser + "' and ToUser='" + toUser + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "IMMessageDb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 最近一月的聊天记录
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public DataTable SelectMessageLog30Today(string fromUser, string toUser)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select SendTime,MessageTxt,Images from IMMessageDb where SendTime between getdate()-30 and dateadd(day,1,getdate()) and FromUser='" + fromUser + "' and ToUser='" + toUser + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "IMMessageDb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 所有聊天记录
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <returns></returns>
        public DataTable SelectMessageLogAllToday(string fromUser, string toUser)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select SendTime,MessageTxt,Images from IMMessageDb where FromUser='" + fromUser + "' and ToUser='" + toUser + "'";
                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "IMMessageDb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        #endregion

        #region desktopData
        public int Update_DesktopLayout(string ControlName,int w,int h,int x,int y)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_SY_DesktopLayout set SizeW=" + w + ",SizeH=" + h + ",LocationX=" + x + ",LocationY=" + y + " where ControlName='" + ControlName + "'";

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }

        public DataTable GetDesktopLayout()
        {
            try
            {
                string sql = "select * from t_SY_DesktopLayout";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SY_DesktopLayout";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        public DataTable GetWarningType()
        {
            try
            {
                string sql = "select WarningType from WarningType";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "WarningType";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        public DataTable GetWarningContent()
        {
            try
            {
                string sql = "select WarningType,WarningContent,WarningLink from WarningContent";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "WarningContent";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        public DataTable GetNotice()
        {
            try
            {
                string sql = "select Contents,Hours from Notice";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "Notice";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        public DataTable GetProcess()
        {
            try
            {
                string sql = "select DHao,Title,Person,Times,State,Options from Process";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "Process";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        public DataTable GetConversation()
        {
            try
            {
                string sql = "select ConversationContent from Conversation";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "Conversation";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        public DataTable GetCGDAN()
        {
            try
            {
                string sql = "select ddhao,wlname,demandquantity,gys from CGDAN";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "CGDAN";
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

        #region WorkFlowDesigner
        public DataTable WorkFlowTree()
        {
            try
            {
                string sql = "select * from t_WorkFlow";
                DataTable dt = finces.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_WorkFlow";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 查工作流设计器结点,不包括所选结点
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable SelectWFCode(string code)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select * from t_WorkFlow where FlowCode like '" + code + "%' and FlowCode!='" + code + "'";

                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_WorkFlow";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 新建流程图的对应的树的数据
        /// </summary>
        /// <param name="PCode"></param>
        /// <param name="FlowCode"></param>
        /// <param name="FlowName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public int AddWorkFlow(int PCode, string FlowCode, string FlowName, string file)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "insert into t_WorkFlow(ParentCode,FlowCode,FlowName,Pic) values ";
                strSQL += "(" + PCode + ",'" + FlowCode + "','" + FlowName + "','" + file + "')";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 删除流程图结树
        /// </summary>
        /// <param name="WFName"></param>
        /// <returns></returns>
        public int DeleteWorkFlow(string WFName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "delete from t_WorkFlow where FlowName='" + WFName + "'";
                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 更新工作流设计器的piadata
        /// </summary>
        /// <param name="bytr"></param>
        /// <param name="flowName"></param>
        /// <returns></returns>
        public int UpdateWorkFlowImage(string bytr, string flowName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update t_WorkFlow set PicData='" + bytr + "' where FlowName='" + flowName + "'";

                //SqlParameter[] parm = new SqlParameter[1];
                //parm[0] = new SqlParameter("@photo", SqlDbType.Image);
                //parm[0].Value = bytr;

                return finces.ExcuteSQL(strSQL);
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return 0;
            }
        }
        /// <summary>
        /// 根据xml文件名取流程名
        /// </summary>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        public DataTable SelcWorkFlowName(string XmlFileName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select FlowName from t_WorkFlow where Pic='" + XmlFileName + "'";

                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_WorkFlow";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        /// <summary>
        /// 根据名称查图片二进制数据
        /// </summary>
        /// <param name="flowName"></param>
        /// <returns></returns>
        public DataTable SelcWorkFlowPicData(string flowName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select PicData from t_WorkFlow where FlowName='" + flowName + "'";

                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "t_WorkFlow";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        #endregion

        #region ACC->应收管理->应收月结
        public DataTable AMRDataMothod()
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select '中山市沙溪镇通伟电脑车花制衣厂' company,simplename,recieve_y,recieve_m from customer where hide=0 union select '中山市通伟服装有限公司' company,simplename,nrecieve_y,nrecieve_m from customer where hide=0";

                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "AMRTable";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 应付月结
        /// </summary>
        /// <returns></returns>
        public DataTable Pay_m_DataMothod()
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "select '中山市沙溪镇通伟电脑车花制衣厂' company,f03,pay_y,pay_m from t1020 where hide=0 union select '中山市通伟服装有限公司' company,f03,npay_y,npay_m from t1020 where hide=0";

                DataTable dt = finces.GetSingle(strSQL);
                if (dt != null)
                {
                    dt.TableName = "Pay_mTb";
                }
                return dt;
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }
    }
}
