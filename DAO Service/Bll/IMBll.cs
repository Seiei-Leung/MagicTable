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
    public class IMBll : BaseBll
    {
        //IM系统数据访问逻辑
        internal IFinance im;
        SystemLogBll log;

        public IMBll()
        {
            im = base.GetDal.Finance;
            log = new SystemLogBll();
        }
        /// <summary>
        /// 获得全部部门数据，还没用在什么地方，测试的
        /// </summary>
        /// <returns></returns>
        public DataTable GetDep()
        {
            try
            {
                string sql = "select * from t_SYDepartment";
                DataTable dt = im.GetSingle(sql);
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
        /// 用实体类方法(还没具体用在什么地方)测试的
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public int UpdateDep(DepartmentEntity dep)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = "update...........";
                SqlParameter[] pa ={
                                   new SqlParameter("@ee",dep.Id),
                              };
                int result = im.ExcuteSQL(strSQL, pa);
                return result;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return 0;
            }
        }
    }
}
