using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Bll
{
    public class SystemLogBll : BaseBll
    {
        internal ISystemLog systemlog;
        internal EventLog eventlog;
                
        public SystemLogBll()
        {
            systemlog = base.GetDal.SystemLogDal;
            eventlog = new EventLog();
            eventlog.Source = "My EAP";
        }

        public SystemLogBll(ISystemLog SystemLog)
        {
            this.systemlog = SystemLog;
            eventlog = new EventLog();
            eventlog.Source = "My EAP";
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-11-03</para>
        /// <para>得到当前的方法名称</para>
        /// </summary>
        public string CurrMethod
        {
            get
            {
                return (new StackTrace(1, true)).GetFrame(0).GetMethod().ToString();
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-11-03</para>
        /// <para>保存系统错误的详细详细</para>
        /// </summary>
        /// <param name="currMethod">当前方法</param>
        /// <param name="objExample">调用方法的对象</param>
        /// <param name="e">异常对象</param>
        /// <param name="userCode">用户ID</param>
        /// <param name="ip">IP地址</param>
        public void SysErrorSave(string currMethod, object objExample, Exception e, string userCode, string ip)
        {
            try
            {
                string classType = objExample.GetType().ToString(); //实体对象的类型
                string errorType = e.GetType().ToString();  //异常类型
                string msg_log = e.ToString().Replace("'", "''");
                string msg_short = e.Message.Replace("'", "''");

                string sql = "insert into t_SYLog(SContent,SContent_Short,ClassType,CurrMethod,ErrorType,TypeId,BUser,IP)" +
                    "values(@msg_log,@msg_short,@classType,@currMethod ,@errorType,1,@userCode,@ip)";
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("msg_log", msg_log);
                param[1] = new SqlParameter("msg_short", msg_short);
                param[2] = new SqlParameter("classType", classType);
                param[3] = new SqlParameter("currMethod", currMethod);
                param[4] = new SqlParameter("errorType", errorType);
                param[5] = new SqlParameter("userCode", userCode);
                param[6] = new SqlParameter("ip", ip);
                systemlog.ExcuteSQL(sql, param);

                //systemlog.ExcuteSQL(sql);
            }
            catch (Exception ee)
            {
                try
                {
                    eventlog.WriteEntry(e.Message + "\r\n" + ee.Message);
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-24
        /// 系统普通日志保存
        /// </summary>
        /// <param name="strMsg">日志内容</param>
        public void SysLogSave(string strMsg)
        {
            string strSQL = string.Empty;
            string IP = Comm.ClientIP;
            string UserCode = "";// Comm._user.UserCode;

            strSQL = " insert t_sylog (SContent,IP,BUser) values ";
            strSQL += " ('" + strMsg + "','" + IP + "'," + UserCode + ")";

            systemlog.ExcuteSQL(strSQL);
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-24
        /// 系统错误日志保存
        /// </summary>
        /// <param name="strMsg">日志内容</param>
        public void SysErrorSave(string strMsg)
        {
            string strSQL = string.Empty;
            string IP = string.Empty;
            string UserCode = string.Empty;
            IP = Comm.ClientIP;
            //if (Common.Comm._user != null)
            //{
            //    UserCode = Common.Comm._user.UserCode;
            //}

            strMsg = strMsg.Replace("'", "''");
            strSQL = " insert t_sylog (SContent,IP,typeId,BUser) values ";
            strSQL += " ('" + strMsg + "','" + IP + "',1,'" + UserCode + "')";

            systemlog.ExcuteSQL(strSQL);        
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-30</para>
        /// <para>系统错误日志保存</para>
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="ip"></param>
        /// <param name="userCode"></param>
        public void SysErrorSave(string strMsg, string ip, string userCode)
        {
            string strSQL = string.Empty;

            strMsg = strMsg.Replace("'", "''");
            strSQL = " insert t_sylog (SContent,IP,typeId,BUser) values ";
            strSQL += " ('" + strMsg + "','" + ip + "',1,'" + userCode + "')";

            systemlog.ExcuteSQL(strSQL);
        }
    }
}
