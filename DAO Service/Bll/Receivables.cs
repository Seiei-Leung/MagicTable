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
using Common;

namespace Bll
{
    public class Receivables : BaseBll
    {
        internal IReceivables AutRecBill;
        SystemLogBll log = null;

        public Receivables()
        {
            AutRecBill = base.GetDal.RecAuthorDal;
            log = new SystemLogBll();
        }
        #region 应收管理
        //银行帐户科目设置
        public DataTable GetGy_BankAccount()
        {
            try
            {
                string Sql = "select a.*,b.Cname from t_Gy_BankAccount a left join dbo.t_ma_AccCode b on a.AccCode=b.Ccode";
                DataTable dt = AutRecBill.Query(Sql).Tables[0];
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
        //会计科目基础
        public DataTable Get_t_maAccCout()
        {
            try
            {
                string Sql = "select CClass,Ccode,AssCode,Cname from t_ma_AccCode";
                DataTable dt = AutRecBill.Query(Sql).Tables[0];
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

        //用于会计科目设置
        public DataTable GetRP_InputCode()
        {
            try
            {
                string Sql = "select * from t_RP_InputCode ";
                DataTable dt = AutRecBill.Query(Sql).Tables[0];
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
    }
}
