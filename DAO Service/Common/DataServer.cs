using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Windows.Forms;
using SQLServerDal;
using IDAL;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Common
{
    public class DataServer
    {
        //private static DataService.ServiceMethod.AuthorityServiceMgrClient proxy_ = null;
        //private static DataService.ServiceCommon.CommonServiceMgrClient commonProxy_ = null;
        //private static ServiceSystemLog.SystemLogServiceMgrClient systemLogProxy_ = null;
        //private static DataService.ServiceFinance.FinanceServiceMgrClient finxy_ = null;
        //private static DataService.ServiceReceivables.ServiceReceivablesClient recProx_ = null;
        private static ServiceECommerce.ECommerceServiceMgrClient ecommerceProxy_ = null;
        private static ServiceMgr.ServiceMgrClient serviceProxy_ = null;

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-11-21</para>
        /// <para>关闭服务对象</para>
        /// </summary>
        public static void Close()
        {
            try
            {
                if(serviceProxy_ != null && serviceProxy_.State == CommunicationState.Opened)
                    serviceProxy_.Close();

                if (DataService.DataServer.IsWCF &&
                    DataService.DataServer.Proxy_WCF != null && 
                    DataService.DataServer.Proxy_WCF.State == CommunicationState.Opened)
                    DataService.DataServer.Proxy_WCF.Close();

                if (DataService.DataServer.IsWCF &&
                    DataService.DataServer.CommonProxy_WCF != null && 
                    DataService.DataServer.CommonProxy_WCF.State == CommunicationState.Opened)
                    DataService.DataServer.CommonProxy_WCF.Close();

                //if (systemLogProxy_ != null && systemLogProxy_.State == CommunicationState.Opened)
                //    systemLogProxy_.Close();

                if (DataService.DataServer.IsWCF &&
                    DataService.DataServer.FinxyProxy_WCF != null && 
                    DataService.DataServer.FinxyProxy_WCF.State == CommunicationState.Opened)
                    DataService.DataServer.FinxyProxy_WCF.Close();

                if (DataService.DataServer.IsWCF &&
                    DataService.DataServer.RecProx_WCF != null && 
                    DataService.DataServer.RecProx_WCF.State == CommunicationState.Opened)
                    DataService.DataServer.RecProx_WCF.Close();

                if (ecommerceProxy_ != null && ecommerceProxy_.State == CommunicationState.Opened)
                    ecommerceProxy_.Close();
            }
            catch (Exception e)
            {
                Comm._frmMain.MsgAlert("会话超时退出");  //测试
            }
        }

        static void ChannelFactory_Faulted(object sender, EventArgs e)
        {
            Comm._frmMain.MsgAlert("程序会话超时，请重新登录！");
            if (Comm._frmMain != null)
            {
                try
                {
                    Comm._frmMain.IsSystemExit = true;
                    Comm._frmMain.Close();
                }
                catch (InvalidOperationException ioe)
                {
                    Application.Exit();
                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-26</para>
        /// <para>得到公共服务类</para>
        /// </summary>
        //public static Common.ServiceCommon.CommonServiceMgrClient commonProxy
        //{
        //    get
        //    {
        //        if (commonProxy_ == null)
        //        {
        //            commonProxy_ = new ServiceCommon.CommonServiceMgrClient();
        //            commonProxy_.StartSession(Comm._user.UserCode, Comm.ClientIP);
        //            commonProxy_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
        //            commonProxy_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);
        //        }
        //        return commonProxy_;
        //    }
        //}

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-26</para>
        /// <para>得到权限服务类</para>
        /// </summary>
        //public static ServiceMethod.AuthorityServiceMgrClient proxy
        //{
        //    get
        //    {
        //        if (proxy_ == null)
        //        {
        //            proxy_ = new ServiceMethod.AuthorityServiceMgrClient();
        //            proxy_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
        //            proxy_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);
        //        }
        //        return proxy_;
        //    }
        //}

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-09-26</para>
        /// <para>得到系统日志类</para>
        /// </summary>
        //public static ServiceSystemLog.SystemLogServiceMgrClient systemLogProxy
        //{
        //    get
        //    {
        //        if (systemLogProxy_ == null)
        //        {
        //            systemLogProxy_ = new ServiceSystemLog.SystemLogServiceMgrClient();
        //            systemLogProxy_.StartSession(Comm._user.UserCode, Comm.ClientIP);
        //            systemLogProxy_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
        //            systemLogProxy_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);
        //        }
        //        return systemLogProxy_;
        //    }
        //}

        /// <summary>
        /// 创建人：王剑
        /// <para>日期：2012-11-06</para>
        /// <para>财务操作模块</para>
        /// </summary>
        //public static ServiceCwZz.FinanceServiceMgrClient finxy
        //{
        //    get
        //    {
        //        if (finxy_ == null)
        //        {
        //            finxy_ = new ServiceCwZz.FinanceServiceMgrClient();
        //            finxy_.StartSession(Comm._user.UserCode, Comm.ClientIP);
        //            finxy_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
        //            finxy_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);
        //        }
        //        return finxy_;
        //    }
        //}

        /// <summary>
        /// 创建人：粟泽洪
        /// <para>日期：2012-11-08</para>
        /// <para>财务应收应付模块</para>
        /// </summary>
        //public static ServiceReceivables.ServiceReceivablesClient recProx
        //{
        //    get
        //    {
        //        if (recProx_ == null)
        //        {
        //            recProx_ = new ServiceReceivables.ServiceReceivablesClient();
        //            recProx_.StartSession(Comm._user.UserCode, Comm.ClientIP);
        //            recProx_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
        //            recProx_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);
        //        }
        //        return recProx_;
        //    }
        //}

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-12-21</para>
        /// <para>得到电商服务类</para>
        /// </summary>
        public static ServiceECommerce.ECommerceServiceMgrClient ecommerceProxy
        {
            get 
            {
                if (ecommerceProxy_ == null)
                {
                    ecommerceProxy_ = new ServiceECommerce.ECommerceServiceMgrClient();
                    ecommerceProxy_.StartSession(Comm._user.UserCode, Comm.ClientIP);
                    ecommerceProxy_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
                    ecommerceProxy_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);
                }
                return DataServer.ecommerceProxy_; 
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-02-04</para>
        /// <para>得到回调服务类</para>
        /// </summary>
        public static ServiceMgr.ServiceMgrClient ServiceProxy
        {
            get 
            {
                if (serviceProxy_ == null)
                {
                    ServiceMgrCallback callback = new ServiceMgrCallback();
                    InstanceContext instanceContext = new InstanceContext(callback);
                    serviceProxy_ = new ServiceMgr.ServiceMgrClient(instanceContext);

                    string port = ConfigurationManager.AppSettings["ServicePort"];
                    if (string.IsNullOrEmpty(port))
                        port = Comm.LocalConfig.ServicePort;
                    FirewallHandler.AddPort(port, int.Parse(port), "TCP");
                    string uri = "http://" + FirewallHandler.GetNetCardIP() + ":" + port;
                    //System.Windows.Forms.MessageBox.Show(FirewallHandler.GetNetCardIP());
                    //System.Windows.Forms.MessageBox.Show(Comm.ClientIP);
                    //WSDualHttpBinding binding = serviceProxy_.Endpoint.Binding as WSDualHttpBinding;
                    (serviceProxy_.Endpoint.Binding as WSDualHttpBinding).ClientBaseAddress = new Uri(uri);

                    serviceProxy_.ChannelFactory.Faulted += new EventHandler(ChannelFactory_Faulted);
                    serviceProxy_.InnerChannel.Faulted += new EventHandler(ChannelFactory_Faulted);

                }
                return DataServer.serviceProxy_;
            }
        }

        #region 本地数据库访问
/*
        private static ICommonDal localDataCommon = null;
        /// <summary>
        /// 本地数据库访问类
        /// </summary>
        public static ICommonDal LocalDataCommon
        {
            get
            {
                if (localDataCommon == null)
                {
                    //if (string.IsNullOrEmpty(Microsoft.ApplicationBlocks.Data.SqlHelper.ConString))
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ConString = Comm._systemParams.SqlConnectString;
                    localDataCommon = new CommonService();
                }
                return localDataCommon;
            }
        }
        /// <summary>
        /// 执行sql语句(传参)，返回DataTable
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ExecSqlByParam(string sqlstr, string[] paramName, object[] value, string tableName)
        {
            try
            {
                DataTable dt;
                if (paramName != null)
                {
                    SqlParameter[] param = new SqlParameter[paramName.Length];
                    for (int i = 0; i < paramName.Length; i++)
                        param[i] = new SqlParameter(paramName[i], value[i]);
                    dt = LocalDataCommon.Query(sqlstr, param);
                }
                else
                    dt = LocalDataCommon.Query(sqlstr).Tables[0];
                if (dt != null)
                {
                    dt.TableName = tableName;
                }
                return dt;
            }
            catch (Exception ex)
            {
                SysErrorSave(CurrMethod, ex);

                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("msg", Type.GetType("System.String"));
                dtErr.Rows.Add(new object[] { ex.Message });
                return dtErr;
            }
        }

        /// <summary>
        /// 执行sql语句，返回DataTable
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable OpenDataSingle(string sqlstr, string table)
        {
            try
            {
                return ExecSqlByParam(sqlstr, null, null, table);
            }
            catch (Exception ex)
            {
                SysErrorSave(CurrMethod, ex);
                return null;
            }
        }

        /// <summary>
        /// 保存数据表
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static bool SaveData(DataTable dataTable)
        {
            try
            {
                LocalDataCommon.SaveData(dataTable);
                return true;
            }
            catch (Exception ex)
            {
                SysErrorSave(CurrMethod, ex);
                return false;
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
        public static void SysErrorSave(string currMethod, Exception e)
        {
            try
            {
                string classType = "Common.DataServer"; //实体对象的类型
                string errorType = e.GetType().ToString();  //异常类型
                string msg_log = e.ToString().Replace("'", "''");
                string msg_short = e.Message.Replace("'", "''");

                string sql = "insert into t_SYLog(SContent,SContent_Short,ClassType,CurrMethod,ErrorType,TypeId,BUser,IP)" +
                    "values('" + msg_log + "','" + msg_short + "','" + classType + "','" + currMethod + "','" + errorType + "',1,'" + Comm._user.UserCode + "','" + Comm.ClientIP + "')";

                LocalDataCommon.ExcuteSQL(sql);
            }
            catch (Exception ee)
            {
                EventLog eventlog = new EventLog();
                eventlog.Source = "My EAP";
                eventlog.WriteEntry(e.Message + "\r\n" + ee.Message);
            }
        }
*/
        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-11-03</para>
        /// <para>得到当前的方法名称</para>
        /// </summary>
        public static string CurrMethod
        {
            get
            {
                return (new StackTrace(1, true)).GetFrame(0).GetMethod().ToString();
            }
        }

        #endregion
    }
}
