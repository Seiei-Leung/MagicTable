using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms; 
using Model;
using System.Net;
using System.Reflection;
using System.Drawing;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Common.Tools;
using CSharpWin;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace Common
{
    public sealed class Comm
    {
        private static string ftpUsePassive = null;  //FTP数据传输模式
        //public static List<string> loadedPlugins = new List<string>(); //已经显示出来的窗体信息
        //public static bool _remoteFlag = false;  //远程登录标记
        public static User _user = null;  //登录用户
        public static SystemParams _systemParams = null;  //系统参数
        private static string clientIP = null;  //客户端ip地址
        private static string clientHostName = null;  //客户端计算机名称
        public static bool _isDesign = false;  //控件设计模式状态
        public static DataTable _dtControlAttribute = new DataTable();
        public static BindingSource _bsControlAttribute = new BindingSource();

        public static Control _selectedCtrl = null; //选中的控件
        public static DataTable _dtControlProperties = null;

        //public static FrmBase _frmLogin = null;//登录窗体
        public static Object MainTreeList = null; //导航树
        //public static ControlMove _controlMove = null;

        public static Dictionary<string, Assembly> _dlls = new Dictionary<string, Assembly>();
        private static DataTable _dtWarningList;  //预警列表
        private static FtpWeb ftpWeb;

        private static Dictionary<string, string> _dicStrVar = null;
        private static LocalConfig localConfig = null;  //获取客户端本地用户配置文件对象
        private static BindingSource bsNotice = null;
        /// <summary>
        /// 任务项数据源
        /// </summary>
        private static BindingSource taskSource = null;
        public static string DeskTopClassName = string.Empty;//桌面待办任务时赋值的实例名
        public static string DeskTopModTitle = string.Empty;//桌面待办任务时赋值的实例名
        public static string DeskTopBUser = string.Empty;//桌面待办任务时赋值的审批人
        public static string DeskTopSerialnoList = string.Empty;//单据主健,多个主健值，用逗号隔开

        public static bool IsTxt = false;//是否是TXT系统单据(false:不是TXT系统;true：是txt系统)
        public static bool IsRefuse = false;//是否拒收任务
        public static Control EditCurr { get; set; } //当前设置的编辑框
        public static Control LabelCurr { get; set; }//当前设置的标签

        //public static LayoutView grv_History = null;    //最近使用网格
        public static BindingSource bs_history = new BindingSource();   //最近使用模块
        public static DataTable dt_history = null;     //最近使用表格
        private static DataTable _dtIcon = null;  //系统图标        
        private static object defaultIcon = null;  //默认图标  
        private static object newGif = null; //通知公告新通知图标
        private static object agreeGif = null;

        public static DateTime dtSelectDate;//从界面选中的日期
        public static DataTable dtCompanyInfor = null;  //公司信息

        public static bool _debug = false; //调试状态
        public static bool _customTemplate = false; //自定义状态
        public static bool _lockScreen = false; //锁屏标记
        public static bool _changeUser = false; //切换用户标记


        public static string SysType = "";
        private static Icon icon = null;
        public static Icon Icon
        {
            get
            {
                if (icon == null)
                    icon = new Icon(Application.StartupPath + "\\logo.ico");
                return icon;
            }
        }



        public static string FtpUsePassive
        {
            get
            {
                if (ftpUsePassive == null)
                {
                    DataTable dtParam = DataService.Data.OpenDataSingle("select * from t_SYSystemParams", "t_SYSystemParams");
                    if (ConvertHandler.ToBoolean(dtParam.Rows[0]["ftpUsePassive"]))
                        ftpUsePassive = "true";
                    else
                    {
                        DataTable dt = DataService.Data.OpenDataSingle("select * from t_SYFtpUsers where ClientHostName='"
                            + Comm.ClientHostName + "'", "t_SYFtpUsers");
                        if (dt != null && dt.Rows.Count > 0)
                            ftpUsePassive = "true";
                        else
                            ftpUsePassive = "false";
                    }
                }
                return ftpUsePassive;
            }
        }

        /// <summary>
        /// 系统图标
        /// </summary>
        public static DataTable dtIcon
        {
            get
            {
                if (_dtIcon == null)
                {
                    string sql = "select * from t_SYModIcon where Del=0";
                    if (_systemParams.UseIconRemote == false)
                    {
                        if (Comm.LocalConfig.LinkType != "0")
                            sql += " and IsDefault=1";
                    }
                    _dtIcon = DataService.Data.OpenDataSingle(sql, "t_SYModIcon");
                    if (Comm.dtIcon != null && Comm.dtIcon.Rows.Count > 0)
                    {
                        Comm.dtIcon.PrimaryKey = new DataColumn[] { Comm.dtIcon.Columns["Id"] };
                        DataRow[] rowsIcon = Comm.dtIcon.Select("IsDefault=1");
                        if (rowsIcon != null && rowsIcon.Length > 0)
                            Comm.DefaultIcon = rowsIcon[0]["Icon"];
                    }
                }
                return _dtIcon;
            }
        }
        /// <summary>
        /// 默认图标
        /// </summary>
        public static object DefaultIcon
        {
            get
            {
                if (defaultIcon == null)
                {
                    if (dtIcon != null)
                    {
                        DataRow[] rowsIcon = Comm.dtIcon.Select("IsDefault=1");
                        if (rowsIcon != null && rowsIcon.Length > 0)
                            defaultIcon = rowsIcon[0]["Icon"];
                    }
                    if (defaultIcon == null)
                    {
                        string sql = "select top 1 Icon from t_SYModIcon where Del=0 and IsDefault=1";
                        DataTable dt = DataService.Data.OpenDataSingle(sql, "t_SYModIcon");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            defaultIcon = dt.Rows[0]["Icon"];
                        }
                    }
                }
                return defaultIcon;
            }
            set { defaultIcon = value; }
        }

        /// <summary>
        /// 通知公告新通知图标
        /// </summary>
        public static object NewGif
        {
            get
            {
                if (newGif == null)
                {
                    MemoryStream stream = new System.IO.MemoryStream();
                    Common.Properties.Resources._new.Save(stream, ImageFormat.Gif);
                    newGif = stream.GetBuffer();

                    //newGif = ConvertHandler.ImageToBytes(Common.Properties.Resources._new, ImageFormat.Gif);
                }
                return newGif;
            }
        }

        public static object AgreeGif
        {
            get
            {
                if (agreeGif == null)
                {
                    MemoryStream stream = new System.IO.MemoryStream();
                    Common.Properties.Resources.hot_1.Save(stream, ImageFormat.Gif);
                    agreeGif = stream.GetBuffer();

                    //newGif = ConvertHandler.ImageToBytes(Common.Properties.Resources._new, ImageFormat.Gif);
                }
                return agreeGif;
            }
        }

        public static DataTable DtCompanyInfor
        {
            get
            {
                if (dtCompanyInfor == null)
                    dtCompanyInfor = DataService.Data.OpenDataSingle("select * from v_CompanyInfor", "v_CompanyInfor");
                return dtCompanyInfor;
            }
            set { dtCompanyInfor = value; }
        }

        private static Dictionary<string, string[]> dicExtensionMethods = null;
        /// <summary>
        /// 扩展方法别名配置
        /// </summary>
        public static Dictionary<string, string[]> DicExtensionMethods
        {
            get
            {
                if (dicExtensionMethods == null)
                {
                    string sql = "select DllName,ClassName,MethodName,MethodAliases,IsStatic from t_SYExtensionMethods";
                    using (DataTable dt = DataService.Data.OpenDataSingle(sql, "t"))
                    {
                        if (dt == null)
                            return null;
                        dicExtensionMethods = new Dictionary<string, string[]>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string key = Convert.ToString(dt.Rows[i]["MethodAliases"]);
                            string[] method = new string[4];
                            method[0] = Convert.ToString(dt.Rows[i]["DllName"]);
                            method[1] = Convert.ToString(dt.Rows[i]["ClassName"]);
                            method[2] = Convert.ToString(dt.Rows[i]["MethodName"]);
                            method[3] = Convert.ToString(dt.Rows[i]["IsStatic"]);

                            dicExtensionMethods.Add(key, method);
                        }
                    }
                }
                return dicExtensionMethods;
            }
        }


        /// <summary>
        /// 郑志冲
        /// 2013-07-26
        /// 保存最近使用模块
        /// </summary>
        /// <param name="rowMod">模块数据行</param>
        /// <param name="fieldName">模块主键</param>
        public static void SaveModHistory(DataRow rowMod, string fieldName)
        {
            //DataRow[] rows = Comm.dt_history.Select("FunId='" + rowMod["FunId"] + "'");
            //if (rows != null && rows.Length > 0)
            //{
            //    rows[0]["CreateTime"] = DateTime.Now;
            //    rows[0].EndEdit();
            //}
            //else
            //{
            //    NewRowByNode(rowMod, fieldName);
            //}
            //SaveTable(Comm.dt_history);
            //grv_History.Refresh();
        }

        /// <summary> 
        /// 郑志冲
        /// 2013-07-26
        /// 最近使用表格增加行
        /// </summary>
        /// <param name="rowMod">模块数据行</param>
        /// <param name="fieldName">模块主键</param>
        private static void NewRowByNode(DataRow rowMod, string fieldName)
        {
            DataRow drv = null;
            if (bs_history.Count > 19)
            {
                //drv = grv_History.GetDataRow(grv_History.RowCount - 1);
                DataRow[] rows = Comm.dt_history.Select("1=1", "CreateTime");
                drv = rows[0];
            }
            else
            {
                drv = (bs_history.AddNew() as DataRowView).Row;
            }
            drv["FunId"] = rowMod[fieldName];
            drv["DisplayName"] = rowMod["DisplayName"];
            drv["ShowDialog"] = rowMod["ShowDialog"];
            drv["NameSpaceName"] = rowMod["NameSpaceName"];
            drv["InStanceName"] = rowMod["InStanceName"];
            drv["FormClassName"] = rowMod["FormClassName"];
            drv["CreateTime"] = DateTime.Now;
            drv["UserCode"] = Comm._user.UserCode;
            drv["Type"] = 1;
            drv["SNo"] = bs_history.Count;

            string iconId = Convert.ToString(rowMod["IconId"]);
            if (iconId == "")
                drv["Icon"] = Comm.DefaultIcon;
            else
            {
                DataRow[] rows = Comm.dtIcon.Select("Id=" + iconId);
                if (rows == null || rows.Length == 0)
                    drv["IconId"] = Comm.DefaultIcon;
                else
                    drv["Icon"] = rows[0]["Icon"];
            }
            drv.EndEdit();
            bs_history.EndEdit();
        }

        /// <summary>
        /// 郑志冲
        /// 2013-07-26
        /// 保存数据表，并接受修改，失败则回滚
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool SaveTable(DataTable dt)
        {
            DataTable dtTmp = dt.GetChanges();
            if (dtTmp != null && dtTmp.Rows.Count > 0)
            {
                Bll.Excute_Status es = DataService.DataServer.proxy.SaveData(dtTmp);
                if (es.status == Bll.Status.TRUE)
                {
                    dt.AcceptChanges();
                    return true;
                }
                else
                {
                    Message.MsgError(es.msg);
                    dt.RejectChanges();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 任务项数据源
        /// </summary>
        public static BindingSource TaskSource
        {
            get
            {
                if (taskSource == null)
                {
                    taskSource = new BindingSource();
                    string sql = "select *,case when EndDate<Convert(varchar(50),getdate(),23) and Finished=0 then 1 else 0 end OverTime from T_OATaskBar where Checked<>1";
                    DataTable dt = DataService.Data.OpenDataSingle(sql, "T_OATaskBar");
                    taskSource.DataSource = dt;
                }
                return taskSource;
            }
            set { taskSource = value; }
        }

        /// <summary>
        /// 公告通知
        /// </summary>
        public static BindingSource BsNotice
        {
            get
            {
                if (bsNotice == null)
                {
                    bsNotice = new BindingSource();

                    string roleFilter = "";
                    for (int i = 0; i < Comm._user.DtUserRole.Rows.Count; i++)
                        roleFilter += " or '; '+NoticeRoleIdList+';' like '%; " + Comm._user.DtUserRole.Rows[i]["RoleCode"] + ";%'";

                    string sqlstr = "select top 30 '' num, '' sContent," +
                        "cast((case when datediff(day,etime,getdate())<=1 then '' else null end) as image) pic," +
                        " id,GUID,TypeCode,Subject,NoticeGroupIdList,NoticeGroupNameList,ValidDateBegin,ValidDateEnd," +
                        " NoticeUserIdList,NoticeUserNameList,NoticeRoleIdList,NoticeRoleNameList,IsPublish,BUserName,BUser,BTime,EUserName,EUser,ETime,del" +
                        " from T_OANotice" +
                            " where IsPublish=1 and del<>1 and ValidDateEnd>=CONVERT(varchar(20),GETDATE(),23) and (" +
                            " ';'+NoticeUserIdList+';' like '%;" + _user.UserId + ";%'" +
                            " or" +
                            " '; '+NoticeGroupIdList+';' like '%; " + _user.GroupTreeCode + ";%'" + roleFilter + ") order by ETime desc";
                    DataTable dt = DataService.Data.OpenDataSingle(sqlstr, "T_OANotice");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!Convert.IsDBNull(dt.Rows[i]["pic"]))
                            dt.Rows[i]["pic"] = Comm.NewGif;
                    }
                    bsNotice.DataSource = dt;
                }
                return bsNotice;
            }
            set { bsNotice = value; }
        }

        public static DataTable GetNotice()
        {
            string roleFilter = "";
            for (int i = 0; i < Comm._user.DtUserRole.Rows.Count; i++)
                roleFilter += " or '; '+NoticeRoleIdList+';' like '%; " + Comm._user.DtUserRole.Rows[i]["RoleCode"] + ";%'";

            string sqlstr = "select top 30 '' num, '' sContent," +
                "cast((case when datediff(day,etime,getdate())<=1 then '' else null end) as image) pic," +
                " id,GUID,TypeCode,Subject,NoticeGroupIdList,NoticeGroupNameList,ValidDateBegin,ValidDateEnd," +
                " NoticeUserIdList,NoticeUserNameList,NoticeRoleIdList,NoticeRoleNameList,IsPublish,BUserName,BUser,BTime,EUserName,EUser,ETime,del" +
                " from T_OANotice" +
                    " where IsPublish=1 and del<>1 and ValidDateEnd>=CONVERT(varchar(20),GETDATE(),23) and (" +
                    " ';'+NoticeUserIdList+';' like '%;" + _user.UserId + ";%'" +
                    " or" +
                    " '; '+NoticeGroupIdList+';' like '%; " + _user.GroupTreeCode + ";%'" + roleFilter + ") order by ETime desc";
            DataTable dt = DataService.Data.OpenDataSingle(sqlstr, "T_OANotice");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!Convert.IsDBNull(dt.Rows[i]["pic"]))
                    dt.Rows[i]["pic"] = Comm.NewGif;
            }
            return dt;
        }

        public static Dictionary<string, string> DicStrVar
        {
            get
            {
                if (_dicStrVar == null)
                {
                    _dicStrVar = new Dictionary<string, string>();
                    DataTable dt = DataService.Data.OpenDataSingle("select * from t_SYMosaicString", "t_SYMosaicString");
                    string strValues;
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strValues = dt.Rows[i]["StrValues"].ToString();
                            strValues = StringHandler.MosaicString(strValues, dt.Rows[i]["StrValuesVar"].ToString());
                            strValues = StringHandler.GetSystemMosaicString(strValues, dt.Rows[i]["StrValuesVar"].ToString());
                            _dicStrVar.Add(dt.Rows[i]["Keys"].ToString(), strValues);

                        }
                    }

                    return _dicStrVar;
                }
                return _dicStrVar;
            }
        }

        /// <summary>
        /// 郑志冲
        /// <para>2013-03-08</para>
        /// <para>获取客户端本地用户配置文件对象</para>
        /// </summary>
        public static LocalConfig LocalConfig
        {
            get
            {
                if (localConfig == null)
                {
                    localConfig = new LocalConfig();

                    //取文件：“UserConfig.xml”的配置
                    XElement config = XElement.Load("UserConfig.xml");
                    //localConfig.Skin = FileHandler.GetElementValue(config, "Skin");
                    localConfig.ServicePort = FileHandler.GetElementValue(config, "ServicePort");
                    localConfig.IsReadSQLConfig = FileHandler.GetElementValue(config, "IsReadSQLConfig");
                    localConfig.UpdateServerUrl = FileHandler.GetElementValue(config, "UpdateServerUrl");
                    localConfig.LoginDefaultUserName = FileHandler.GetElementValue(config, "LoginDefaultUserName");
                    localConfig.LinkType = FileHandler.GetElementValue(config, "LinkType");
                    config = null;
                    //取文件：“ERPMain.exe.config”的配置
                    localConfig.Skin = ConfigurationManager.AppSettings["Skin"];
                    //localConfig.ServicePort = ConfigurationManager.AppSettings["ServicePort"];
                    //localConfig.IsReadSQLConfig = ConfigurationManager.AppSettings["IsReadSQLConfig"];
                    //localConfig.UpdateServerUrl = ConfigurationManager.AppSettings["UpdateServerUrl"];
                    //localConfig.LoginDefaultUserName = ConfigurationManager.AppSettings["LoginDefaultUserName"];
                    //localConfig.LinkType = ConfigurationManager.AppSettings["LinkType"];

                    if (localConfig.LinkType == "0")
                        DataService.DataServer.LinkType = DataService.LinkType.Intranet;
                    else if (localConfig.LinkType == "1")
                        DataService.DataServer.LinkType = DataService.LinkType.Public;
                    else
                        DataService.DataServer.LinkType = DataService.LinkType.VPN;

                    localConfig.FtpServerIP = GetFtpServerIP(localConfig.LinkType);

                    #region IM连接配置
                    //localConfig.IMServerIP = GetIMServerIP(localConfig.LinkType);
                    #endregion

                    return localConfig;
                }
                return localConfig;
            }
        }

        /// <summary>
        /// 返回IM服务器IP地址（本地、远程、VPN）
        /// </summary>
        /// <param name="imlinktype"></param>
        /// <returns></returns>
        //private static string GetIMServerIP(string imlinktype)
        //{
        //    if (imlinktype == "0")
        //        imlinktype = "Path_Intranet";
        //    else if (imlinktype == "1")
        //        imlinktype = "Path_Public";
        //    else
        //        imlinktype = "Path_VPN";

        //    string path = System.AppDomain.CurrentDomain.BaseDirectory;
        //    XElement config = XElement.Load(path + "\\LocalConifg.xml");
        //    XElement IMConfig = config.Element("IMServerIP");
        //    return Common.FileHandler.GetAttributeValue(IMConfig, imlinktype);
        //}

        private static string GetFtpServerIP(string linkType)
        {
            if (linkType == "0")
                linkType = "Path_Intranet";
            else if (linkType == "1")
                linkType = "Path_Public";
            else
                linkType = "Path_VPN";
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            XElement config = XElement.Load(path + "\\LocalConifg.xml");
            //XElement config = XElement.Load("LocalConifg.xml");
            XElement FtpConfig = config.Element("FtpServerIP");
            return Common.FileHandler.GetAttributeValue(FtpConfig, linkType);
        }

        public static FtpWeb FtpWeb
        {
            get
            {
                if (Comm.ftpWeb == null)
                {
                    Comm.ftpWeb = new FtpWeb(LocalConfig.FtpServerIP, _systemParams.FtpRemotePath, _systemParams.FtpUserID, _systemParams.FtpPassword);
                    return Comm.ftpWeb;
                }
                return Comm.ftpWeb;
            }
            set { Comm.ftpWeb = value; }
        }

        public static DataTable DtWarningList
        {
            get
            {
                if (Comm._dtWarningList == null)
                {
                    //Comm._dtWarningList = new DataTable();
                    //Comm._dtWarningList.TableName = "dtWarningList";
                    //Comm._dtWarningList.Columns.Add("Id");
                    //Comm._dtWarningList.Columns.Add("WarnType");
                    //Comm._dtWarningList.Columns.Add("WarnTitle");
                    //Comm._dtWarningList.Columns.Add("WarnContent");
                    //Comm._dtWarningList.Columns.Add("WarnCount", typeof(Int32));
                    //Comm._dtWarningList.Columns.Add("Image1", typeof(Byte[]));

                    string sql = "select top 0 '' Id,'' WarnType,'' WarnTitle,'' WarnContent,0 WarnCount,cast(null as Image) GuanZhu";
                    Comm._dtWarningList = DataService.Data.OpenDataSingle(sql, "dtWarningList");
                }
                return Comm._dtWarningList;
            }
            set { Comm._dtWarningList = value; }
        }

        public static BindingSource _bsWarningList = new BindingSource();  //预警列表
        public static Dictionary<string, DataTable> _dicWarningTable = new Dictionary<string, DataTable>(); //预警内容

        public static void RefreshWarnningList()
        {
            string sql = "select id,warntype,warntitle,sql from t_SYWarning where startstatus=1";
            DataTable dt = DataService.Data.OpenDataSingle(sql, "dtWarning");
            foreach (DataRow r in dt.Rows)
            {
                DataTable dtResult;
                string checkSql = "select * from t_SYWarningResult where warnid='" + r["id"].ToString() + "'";
                DataTable dtCheck = DataService.Data.OpenDataSingle(checkSql, "dtCheck");
                if (dtCheck.Rows.Count > 0)
                {
                    dtResult = DataService.Data.OpenDataSingle(r["sql"].ToString(), "dtResult");
                    string updateSql = "update t_SYWarningResult set warncount=" + Convert.ToString(dtResult.Rows.Count) + " where warnid='" + r["id"].ToString() + "'";
                    int i = DataService.Data.ExecuteNonQuery(updateSql);
                }
                else
                {
                    dtResult = DataService.Data.OpenDataSingle(r["sql"].ToString(), "dtResult");
                    string insertSql = "insert t_SYWarningResult values('" + r["id"].ToString() + "','" + r["warntype"].ToString() + "','" + r["warntitle"].ToString() + "'," + Convert.ToString(dtResult.Rows.Count) + ",getdate(),getdate())";
                    int i = DataService.Data.ExecuteNonQuery(insertSql);
                }
                //sql = "exec T2050CH_QReadIn '" + formNo + "','" + orderno + "','" + part + "','" + Comm._user.UserCode + "','" + id + "'";
                //int i = DataService.Data.ExecuteNonQuery(sql);
                //if (i > 0)
                //    (form as FrmMasterDetail).bnsModuleRule.LoadDetailData(true);
            }
        }


        public static DataTable GetWarnning()
        {
            //RefreshWarnningList();
            string sql = "select a.*,d.WarnId isGuanZhu from t_SYWarningResult a inner join (select WarningId from t_SYWarningUser where UserId='" + Comm._user.UserId + "' union ";
            sql = sql + " select a.WarningId from t_SYWarningRole a inner join t_SYUserRole b on a.RoleId=b.RoleId ";
            sql = sql + " inner join t_SYUsers c on b.UserId=c.Id where UserId='" + Comm._user.UserId + "') b on a.WarnId=b.WarningId inner join (select * from t_SYWarning where StartStatus=1) c on a.WarnId=c.Id ";
            sql = sql + " left join (select * from t_SYWarnFocus where UserId='" + Comm._user.UserId + "') d on a.WarnId=d.WarnId";
            DataTable dt = DataService.Data.OpenDataSingle(sql, "t_SYWarningResult");
            return dt;
        }

        /// <summary>
        /// 权限判定
        /// </summary>
        public static bool HasRight(string displayname, string pid, Form form)
        {
            DataRow[] drs = _user.DtUserRights.Select("PId='" + pid + "' and DisplayName='" + displayname + "'");
            if (drs.Length == 0)
            {
                form.MsgAlert("你没有 " + displayname + " 的权限！");
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 动态设定控件属性
        /// </summary>
        public static void setControlAttribute(Control c)
        {
            if (c != null)
            {
                DataRow dr = TableHandler.GetCurrentDataRow(_bsControlAttribute);
                c.Size = new System.Drawing.Size(Convert.ToInt32(dr["width"]), Convert.ToInt32(dr["height"]));
                c.Location = new System.Drawing.Point(Convert.ToInt32(dr["locatex"]), Convert.ToInt32(dr["locatey"]));
                c.Name = dr["controlname"].ToString();
                if (c.GetType().ToString() == "System.Windows.Forms.Label")
                {
                    Label label = (Label)c;
                    label.Text = dr["caption"].ToString();
                }

                if (c.GetType().ToString() == "DevExpress.XtraEditors.ButtonEdit")
                {

                }
                DataService.DataServer.proxy.SaveData(_dtControlAttribute);
            }
        }


        

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-09-25
        /// 把DataRow集合装载到DataTable数组里面
        /// </summary>
        /// <param name="rows">DataRow集合</param>
        /// <returns></returns>
        public static DataTable[] ToDataTableList(List<DataRow> rows)
        {
            DataTable[] dts = new DataTable[rows.Count];
            for (int i = 0; i < rows.Count; i++)
            {
                DataTable dt = rows[i].Table.Clone();
                dt.Rows.Add(rows[i].ItemArray);
                dts[i] = dt;
            }
            return dts;
        }

        




        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-27
        /// 开始的括号
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAreaBeg()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("(", "(");
            dic.Add("((", "((");
            dic.Add("(((", "(((");
            dic.Add("((((", "((((");
            dic.Add("(((((", "(((((");
            dic.Add("((((((", "((((((");

            return dic;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-27
        /// 结尾的括号
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAreaEnd()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add(")", ")");
            dic.Add("))", "))");
            dic.Add(")))", ")))");
            dic.Add("))))", "))))");
            dic.Add(")))))", ")))))");
            dic.Add("))))))", "))))))");

            return dic;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-27
        /// 查询过滤的关系
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetRelation()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("并且", "and");
            dic.Add("或者", "or");

            return dic;
        }

        public static Dictionary<string, string> GetTjrec()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "");
            dic.Add("最大值", "max");
            dic.Add("最小值", "min");
            dic.Add("合计", "sum");
            dic.Add("平均", "avg");
            return dic;
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-12-31
        /// 获取客户端的IP地址
        /// </summary>
        public static string ClientIP
        {
            get
            {
                if (clientIP == null)
                {
                    clientIP = getClientIP();
                }
                return clientIP;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-24
        /// 获取客户端的IP地址
        /// </summary>
        /// <returns>字符串</returns>
        private static string getClientIP()
        {
            IPAddress[] ipAddressList = Dns.GetHostEntry(ClientHostName).AddressList;
            foreach (IPAddress address in ipAddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-02-22</para>
        /// <para>获取客户端的计算机名称</para>
        /// </summary>
        public static string ClientHostName
        {
            get
            {
                if (clientHostName == null)
                {
                    clientHostName = Dns.GetHostName();
                }
                return clientHostName;
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-25
        /// 查询条件比较符
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> getCondiction()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("等于", "=");
            dic.Add("大于", ">");
            dic.Add("大于等于", ">=");
            dic.Add("小于", "<");
            dic.Add("小于等于", "<=");
            dic.Add("包含", "like");
            dic.Add("不包含", "not like");

            return dic;
        }

        public static Dictionary<string, string> getProject()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "");
            dic.Add("制单号", "orderno");
            dic.Add("客户", "custname");
            dic.Add("款号", "styleno");
            dic.Add("件数", "quantity");
            dic.Add("单价", "price");
            dic.Add("金额", "amount");
            return dic;
        }



        ///// <summary>
        ///// 创建人：黎金来
        ///// 日期：2012-10-19
        ///// 显示汇总分析
        ///// </summary>
        ///// <param name="AnalyseName">AnalyseName 名</param>
        //public static void ShowAnalyseSummary(string AnalyseName)
        //{
        //    if (string.IsNullOrEmpty(AnalyseName)) return;
        //    FrmAnalyse frm = new FrmAnalyse(AnalyseName);
        //    if (frm.IsParamRight)
        //        frm.ShowDialog();
        //}


      




        public static void SetBillKeyValue(BindingSource bsMaster, DataRow thisRow)
        {
            DataRow drM = TableHandler.GetCurrentDataRow(bsMaster);
            thisRow["Serialno"] = drM["Serialno"].ToString();
            thisRow["Guid"] = System.Guid.NewGuid().ToString();
        }

        public static void SetBillKeyValue(BindingSource bsMaster, BindingSource bsDetail)
        {
            DataRow drM = TableHandler.GetCurrentDataRow(bsMaster);
            DataRow drD = TableHandler.GetCurrentDataRow(bsDetail);
            drD["Serialno"] = drM["Serialno"].ToString();
            drD["Guid"] = System.Guid.NewGuid().ToString();
        }

        public static void SetBillKeyValue(BindingSource bsMaster, DataRow thisRow, string masterKey, string pk, string fk)
        {
            DataRow drM = TableHandler.GetCurrentDataRow(bsMaster);
            thisRow[fk] = drM[masterKey].ToString();
            thisRow[pk] = System.Guid.NewGuid().ToString();
        }

        public static void SetBillKeyValue(BindingSource bsMaster, BindingSource bsDetail, string masterKey, string pk, string fk)
        {
            DataRow drM = TableHandler.GetCurrentDataRow(bsMaster);
            DataRow drD = TableHandler.GetCurrentDataRow(bsDetail);
            drD[fk] = drM[masterKey].ToString();
            if (pk != "")
                drD[pk] = System.Guid.NewGuid().ToString();
        }

        
        private static log4net.ILog log; 
        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-11-16</para>
        /// <para>日志文件记录类</para>
        /// <para>常用方法：Info、Error、Warn</para>
        /// </summary>
        public static log4net.ILog Log
        {
            get
            {
                if (log == null)
                {
                    log = log4net.LogManager.GetLogger("log");
                    log.Info("记录开始。。。");
                }
                return log;
            }
        }

        

        public static Font GetFont(string familyName, float emSize, bool bold, bool italic, bool strikeOut, bool underline)
        {
            int styleNum = 0;
            List<System.Drawing.FontStyle> listFontStyle = new List<FontStyle>();
            if (bold == true)
            {
                styleNum = styleNum + 1;
                listFontStyle.Add(System.Drawing.FontStyle.Bold);
            }
            if (italic == true)
            {
                styleNum = styleNum + 1;
                listFontStyle.Add(System.Drawing.FontStyle.Italic);
            }
            if (strikeOut == true)
            {
                styleNum = styleNum + 1;
                listFontStyle.Add(System.Drawing.FontStyle.Strikeout);
            }
            if (underline == true)
            {
                styleNum = styleNum + 1;
                listFontStyle.Add(System.Drawing.FontStyle.Underline);
            }
            if (styleNum == 1)
                return new Font(familyName, emSize, listFontStyle[0]);
            else if (styleNum == 2)
                return new Font(familyName, emSize, ((System.Drawing.FontStyle)((listFontStyle[0] | listFontStyle[1]))));
            else if (styleNum == 3)
                return new Font(familyName, emSize, ((System.Drawing.FontStyle)(((listFontStyle[0] | listFontStyle[1]) | listFontStyle[2]))));
            else if (styleNum == 4)
                return new Font(familyName, emSize, ((System.Drawing.FontStyle)((((listFontStyle[0] | listFontStyle[1]) | listFontStyle[2]) | listFontStyle[3]))));
            else
                return new Font(familyName, emSize);

        }

        public static string ff(string guid, DataTable[] dts)
        {
            return DataService.DataServer.commonProxy.ExecMenuProcedure(guid, dts);
        }



        /*
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-02-28
        /// 获取今天、明天需要提醒日程
        /// </summary>
        /// <returns></returns>
        public static void GetRemindScheduler()
        {
            //判断是否登录        
            if (Common.Comm._user == null) return;
            //if (ss != null && ss.Appointments.Count > 0) return;

            _ss = new SchedulerStorage();
            _sc = new SchedulerControl();

            string strCondiction = string.Empty;
            DataTable dtData = new DataTable();
            DataSet ds = new DataSet();

            strCondiction = " where BUser='" + Common.Comm._user.UserCode + "'";
            strCondiction += " and  convert(nvarchar(10),AppStart,20) >='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            strCondiction += " and convert(nvarchar(10),AppStart,20) <='" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'";
            dtData = DataService.DataServer.commonProxy.GetScheduler(strCondiction);

            //SchedulerStorage ss = new SchedulerStorage();

            _ss.Appointments.Mappings.AllDay = "AllDay";
            _ss.Appointments.Mappings.Description = "Description";
            _ss.Appointments.Mappings.End = "AppEnd";
            _ss.Appointments.Mappings.Label = "AppLabel";
            _ss.Appointments.Mappings.Location = "AppLocation";
            _ss.Appointments.Mappings.RecurrenceInfo = "AppRecurrenceInfo";
            _ss.Appointments.Mappings.ReminderInfo = "AppReminderInfo";
            _ss.Appointments.Mappings.AppointmentId = "GUID";
            _ss.Appointments.Mappings.Start = "AppStart";

            _ss.Appointments.Mappings.Status = "AppStatus";
            _ss.Appointments.Mappings.Subject = "AppSubject";
            _ss.Appointments.Mappings.Type = "AppType";

            dtData.TableName = "T_SYScheduler";

            if (dtData.DataSet == null)
            {
                if (!ds.Tables.Contains("T_SYScheduler"))
                    ds.Tables.Add(dtData);
            }
            else
                ds = dtData.DataSet;


            _ss.Appointments.DataSource = ds.Tables[0].DefaultView;
            _sc.Storage = _ss;

            //SchedulerControl sc = new SchedulerControl(ss);
            //dtData = null;
            //return ds;
            //ds = null;
        }
        */
        /// <summary>
        /// 获取控件属性
        /// </summary>
        public static void GetControlProperties()
        {
            #region 属性列表
            List<ControlProperties> l = new List<ControlProperties>();
            l.Add(new ControlProperties("Caption", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;统计角度编辑框;"));
            l.Add(new ControlProperties("Tips", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;统计角度编辑框;"));
            l.Add(new ControlProperties("ControlName", "主档控件;表格控件", ";单据状态标签;按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("CustomSize", "主档控件", ";标签;"));
            l.Add(new ControlProperties("LocateX", "主档控件;表格控件", ";单据状态标签;按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("LocateY", "主档控件;表格控件", ";单据状态标签;按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("Width", "主档控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("Height", "主档控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("FontColor", "主档控件;表格控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;统计角度编辑框;勾选编辑框;"));
            l.Add(new ControlProperties("FontSize", "主档控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;统计角度编辑框;勾选编辑框;"));
            l.Add(new ControlProperties("FontBold", "主档控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;时间编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;统计角度编辑框;勾选编辑框;"));
            l.Add(new ControlProperties("FontItalic", "主档控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;统计角度编辑框;勾选编辑框;"));
            l.Add(new ControlProperties("ControlType", "主档控件;表格控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("FieldName", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("ColWidth", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;钻取框;"));
            l.Add(new ControlProperties("ListShow", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("Format", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;尺码组;钻取框;"));
            l.Add(new ControlProperties("NoNull", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;"));
            l.Add(new ControlProperties("NoSpace", "主档控件;表格控件", ";文本编辑框;按钮编辑框;下拉编辑框;下拉弹出框;备注编辑框;备注选择框;查找编辑框;尺码组;备注框;"));
            l.Add(new ControlProperties("ListItems", "主档控件;表格控件", ";下拉编辑框;单选编辑框;下拉多选编辑框;统计角度编辑框;"));
            //l.Add(new ControlProperties("ListSearchField", "主档控件;表格控件", ";下拉弹出框;"));
            l.Add(new ControlProperties("ListEditStyles", "主档控件;表格控件", ";下拉编辑框;下拉弹出框;单选编辑框;下拉多选编辑框;"));
            l.Add(new ControlProperties("IsValidateItems", "主档控件;表格控件", ";下拉编辑框;"));
            l.Add(new ControlProperties("ButtonShowName", "主档控件;表格控件", ";按钮编辑框;"));
            l.Add(new ControlProperties("ButtonSql", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonSqlVals", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonSqlStrVals", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonSearchField", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonGetField", "主档控件;表格控件", ";按钮编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonBuiltDetail", "主档控件;表格控件", ";按钮编辑框;勾选编辑框;"));
            l.Add(new ControlProperties("ButtonSourceDetails", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonTargetDetails", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonSourceFields", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonTargetFields", "主档控件;表格控件", ";按钮编辑框;查找编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonColWidth", "主档控件;表格控件", ";按钮编辑框;下拉弹出框;"));
            l.Add(new ControlProperties("ButtonInOne", "表格控件", ";按钮编辑框;"));
            l.Add(new ControlProperties("ButtonSelectMore", "主档控件;表格控件", ";按钮编辑框;"));
            l.Add(new ControlProperties("DefaultValue", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("DefaultValueVals", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("ReadOnly", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;"));
            l.Add(new ControlProperties("Hide", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("EncryptHide", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("DnyCreate", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;"));
            l.Add(new ControlProperties("Summary", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;计算器编辑框;步进编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;钻取框;"));
            l.Add(new ControlProperties("SummaryType", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;计算器编辑框;步进编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;钻取框;"));
            l.Add(new ControlProperties("SummaryFormat", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;计算器编辑框;步进编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;钻取框;"));
            l.Add(new ControlProperties("SummaryFormula", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;计算器编辑框;步进编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;钻取框;"));
            l.Add(new ControlProperties("ShieldUser", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("ShieldRole", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("VisibleUser", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("VisibleRole", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("AddDays", "主档控件", ";日期编辑框;"));
            l.Add(new ControlProperties("DefaultValue_Type", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("ButtonDllFileName", "主档控件;表格控件", ";按钮编辑框;按钮;"));
            l.Add(new ControlProperties("ButtonDllClassName", "主档控件;表格控件", ";按钮编辑框;按钮;"));
            l.Add(new ControlProperties("ButtonDllMethodName", "主档控件;表格控件", ";按钮编辑框;按钮;"));
            l.Add(new ControlProperties("MaxValue", "主档控件;表格控件", ";步进编辑框;"));
            l.Add(new ControlProperties("MinValue", "主档控件;表格控件", ";步进编辑框;"));
            l.Add(new ControlProperties("LinkModName", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkModClassName", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkModPageCaption", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkParams", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkVals", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkDllFileName", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkDllClassName", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("LinkDllMethodName", "主档控件;表格控件", ";钻取框;"));
            l.Add(new ControlProperties("ListItemsSql", "主档控件;表格控件", ";下拉编辑框;单选编辑框;下拉多选编辑框;"));
            l.Add(new ControlProperties("ListItemsSqlVals", "主档控件;表格控件", ";下拉编辑框;下拉多选编辑框;"));
            l.Add(new ControlProperties("EditFormShow", "表格控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;"));
            l.Add(new ControlProperties("LabelFontColor", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("LabelBackColor", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("ReadOnlyOnEdit", "主档控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;"));
            l.Add(new ControlProperties("SNo", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;货币编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("BackColor", "主档控件;表格控件", ";按钮;标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("Formula", "主档控件;表格控件", ";文本编辑框;货币编辑框;货币编辑框;数值编辑框;按钮编辑框;勾选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;备注框;钻取框;日期编辑框;下拉编辑框;"));
            l.Add(new ControlProperties("FormuleField", "主档控件;表格控件", ";文本编辑框;货币编辑框;货币编辑框;数值编辑框;按钮编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;备注框;钻取框;日期编辑框;"));
            l.Add(new ControlProperties("FormulaSource", "主档控件;表格控件", ";文本编辑框;货币编辑框;货币编辑框;数值编辑框;按钮编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;备注框;钻取框;日期编辑框;"));
            l.Add(new ControlProperties("IsCalcAgain", "主档控件;表格控件", ";文本编辑框;货币编辑框;货币编辑框;数值编辑框;按钮编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;备注框;钻取框;"));
            l.Add(new ControlProperties("CalcAgainType", "主档控件", ";文本编辑框;货币编辑框;货币编辑框;数值编辑框;按钮编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;备注框;钻取框;"));
            l.Add(new ControlProperties("ButtonFrm_Descr", "主档控件;表格控件", ";按钮编辑框;钻取框;"));
            l.Add(new ControlProperties("FormatSys", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("FormatType", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("FormatString", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("IsFill", "主档控件", ";富文本编辑框;"));
            l.Add(new ControlProperties("CreateLabel", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("LabelCustomSize", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("LabelWidth", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("LabelHeight", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("LabelLocateX", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("LabelLocateY", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;分隔线;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;统计角度编辑框;"));
            l.Add(new ControlProperties("FixedCol", "表格控件", ";文本编辑框;货币编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;"));
            l.Add(new ControlProperties("FixedStyle", "表格控件", ";文本编辑框;货币编辑框;货币编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;"));
            l.Add(new ControlProperties("ReadOnlyFormula", "主档控件;表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;"));
            l.Add(new ControlProperties("LoadPages", "主档控件", ";按钮编辑框;"));
            l.Add(new ControlProperties("CancelHolidayLimit", "主档控件;表格控件", ";日期编辑框;"));
            l.Add(new ControlProperties("MaskType", "主档控件;表格控件", ";文本编辑框;按钮编辑框;"));
            l.Add(new ControlProperties("EditMask", "主档控件;表格控件", ";文本编辑框;按钮编辑框;"));
            l.Add(new ControlProperties("CellMerge", "表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;"));
            l.Add(new ControlProperties("SortOrder", "表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;"));
            l.Add(new ControlProperties("EditOnChecked", "主档控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;"));
            l.Add(new ControlProperties("DataSource", "主档控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("NoBinding", "主档控件", ";标签;文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;图片编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;颜色编辑框;查找编辑框;尺码组;下拉多选编辑框;字体编辑框;备注框;富文本编辑框;钻取框;统计角度编辑框;"));
            l.Add(new ControlProperties("PassWordChar", "主档控件;表格控件", ";文本编辑框;"));
            l.Add(new ControlProperties("VarSql", "主档控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;备注编辑框;时间编辑框;步进编辑框;"));
            l.Add(new ControlProperties("DragDropCopy", "表格控件", ";文本编辑框;货币编辑框;数值编辑框;按钮编辑框;下拉编辑框;下拉弹出框;日期编辑框;勾选编辑框;单选编辑框;计算器编辑框;备注编辑框;备注选择框;时间编辑框;步进编辑框;实际库存尺码组;可用库存尺码组;系统库存尺码组;差异库存尺码组;下拉多选编辑框;字体编辑框;备注框;钻取框;"));
            l.Add(new ControlProperties("LinkShowDialog", "主档控件;表格控件", ";钻取框;"));

            l.Add(new ControlProperties("KeyEnterSql", "主档控件;表格控件", ";文本编辑框;下拉编辑框;下拉弹出框;按钮编辑框;"));
            l.Add(new ControlProperties("KeyEnterSqlVals", "主档控件;表格控件", ";文本编辑框;下拉编辑框;下拉弹出框;按钮编辑框;"));
            l.Add(new ControlProperties("KeyEnterSqlStrVals", "主档控件;表格控件", ";文本编辑框;下拉编辑框;下拉弹出框;按钮编辑框;"));
            l.Add(new ControlProperties("KeyEnterSearchField", "主档控件;表格控件", ";文本编辑框;下拉编辑框;下拉弹出框;按钮编辑框;"));
            l.Add(new ControlProperties("KeyEnterGetField", "主档控件;表格控件", ";文本编辑框;下拉编辑框;下拉弹出框;按钮编辑框;"));
            l.Add(new ControlProperties("ShowTime", "主档控件;表格控件", ";日期编辑框;"));
            l.Add(new ControlProperties("Upper", "主档控件;表格控件", ";文本编辑框;按钮编辑框;备注编辑框;"));

            l.Add(new ControlProperties("MenuSproc", "主档控件", ";按钮;"));
            l.Add(new ControlProperties("MenuSprocParam", "主档控件", ";按钮;"));
            #endregion


            DataTable dtSYControlProperties = new DataTable();
            dtSYControlProperties.TableName = "t_SYControlProperties";
            dtSYControlProperties.Columns.Add("MorD");
            dtSYControlProperties.Columns.Add("ControlType");
            dtSYControlProperties.Columns.Add("Properties");
            foreach (ControlProperties ll in l)
            {
                DataRow dr = dtSYControlProperties.NewRow();
                dr["MorD"] = ll.MorD;
                dr["ControlType"] = ll.ControlType;
                dr["Properties"] = ll.Properties;
                dtSYControlProperties.Rows.Add(dr);
            }
            Comm._dtControlProperties = dtSYControlProperties;
        }

        

       

        /// <summary>
        /// 检查用户是否有该时间段查询权限
        /// </summary>
        /// <param name="beginDatetime">开始日期</param>
        /// <param name="endDatetime">结束日期</param>
        /// <param name="rowConfig">配置信息</param>
        /// <returns></returns>
        public static string CheckSearchDatetimeRight(DateTime beginDatetime, DateTime endDatetime, DataRow rowConfig, DataTable userList)
        {
            if (Comm._user.IsAdmin)
                return "SUCCESS";

            int queryMonths = 0;
            int queryScope = 0;


            if (userList != null && userList.Rows.Count > 0)
            {
                DataRow[] drs = userList.Select("UserId = '" + _user.UserId + "'");
                if (drs.Length > 0)
                {
                    queryMonths = Convert.ToInt32(drs[0]["QueryMons"]);
                    queryScope = Convert.ToInt32(drs[0]["QueryScope"]);
                }
            }
            else if (rowConfig != null)
            {
                if (!Convert.IsDBNull(rowConfig["QueryMons"]))
                {
                    int qm = Convert.ToInt32(rowConfig["QueryMons"]);
                    if (qm != 0)
                        queryMonths = qm;
                }
                if (!Convert.IsDBNull(rowConfig["QueryScope"]))
                {
                    int qs = Convert.ToInt32(rowConfig["QueryScope"]);
                    if (qs != 0)
                        queryScope = qs;
                }
            }

            if (queryMonths == 0)
                queryMonths = Comm._user.QueryMonths;
            if (queryScope == 0)
                queryScope = Comm._user.QueryScope;

            //else
            //{
            //    queryMonths = Comm._user.QueryMonths;
            //    queryScope = Comm._user.QueryScope;
            //}

            //if (ConvertHandler.ToInt32(rowConfig["QueryMons"]) == 0)
            //    queryMonths = Comm._systemParams.QueryMons;
            //else
            //    queryMonths = Convert.ToInt32(rowConfig["QueryMons"]);

            //if (ConvertHandler.ToInt32(rowConfig["QueryScope"]) == 0)
            //    queryScope = Comm._systemParams.QueryScope;
            //else
            //    queryScope = Convert.ToInt32(rowConfig["QueryScope"]);

            //if (Comm._user.QueryMonths > 0 && queryMonths > 0)
            //{
            //    if (Comm._user.QueryMonths < queryMonths)
            //        queryMonths = Comm._user.QueryMonths;
            //}
            //else if (Comm._user.QueryMonths > 0 && queryMonths == 0)
            //    queryMonths = Comm._user.QueryMonths;

            //if (Comm._user.QueryScope > 0 && queryScope > 0)
            //{
            //    if (Comm._user.QueryScope < queryScope)
            //        queryScope = Comm._user.QueryScope;
            //}
            //else if (Comm._user.QueryScope > 0 && queryScope == 0)
            //    queryScope = Comm._user.QueryScope;


            DateTime date = Convert.ToDateTime(DataService.DataServer.commonProxy.GetSystemDatetime());
            if (queryScope > 0)
            {
                date = date.AddMonths(-queryScope);
                if (date.CompareTo(beginDatetime) > 0)
                {
                    //return "你没有权限查询 " + queryScope + " 个月前的数据";
                    return "查询日期失效！";
                }
            }

            if (queryMonths > 0)
            {
                date = beginDatetime.AddMonths(queryMonths);
                if (date.CompareTo(endDatetime) < 0)
                {
                    //return "时间段(结束日期-开始日期)不能超过 " + queryMonths + " 个月";
                    return "查询日期失效！";
                }
            }
            return "SUCCESS";
        }
        /*
        /// <summary>
        /// 郑志冲
        /// <para>2013-04-01</para>
        /// <para>执行公式计算</para>
        /// </summary>
        /// <param name="formula">公式表达式</param>
        /// <param name="dataTables">数据表字段</param>
        /// <param name="row">当前计算行(用于从档列计算)</param>
        /// <param name="fieldName">返回的字段(用于从档列计算)</param>
        /// <param name="IsSetColumnValue">是否于列填写结果值(用于从档列计算)</param>
        /// <param name="tableAlias">表的别名</param>
        /// <returns></returns>
        public static object ExecFormula(string formula, Dictionary<string, DataTable> dataTables, BindingSource bsMaster,
                    DataRow row, string fieldName, bool IsSetColumnValue, string tableAlias, Dictionary<string, GridView> gvs)
        {
            RPN rpn = new RPN(dataTables, row, fieldName, IsSetColumnValue, bsMaster, gvs);
            if (rpn.Parse(formula))
            {
                return rpn.Evaluate();
            }
            else
            {
                string tmp = "控件:";
                if (tableAlias != null)
                    tmp = "列:" + tableAlias + ".";
                Comm._frmMain.MsgWarn(tmp + fieldName + "的公式:" + formula + "存在错误！");
                return null;
            }
        }
        */
        
        
       

    
       

       

        //public static void LoadAssembly(string nameSpaceName)
        //{            
        //    string path = Application.StartupPath + "\\";
        //    Assembly loadedDll = Assembly.LoadFrom(path + nameSpaceName + "." + "dll");
        //    _dlls.Add(nameSpaceName + "." + "dll", loadedDll);
        //}


        


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





        /// <summary>  
        /// 获得距离上一次操作时间间隔  
        /// </summary>  
        /// <returns></returns>  
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!Win32.GetLastInputInfo(ref vLastInputInfo)) return 0;
            return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }



        

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2014-01-02
        /// 写入错误日志
        /// </summary>
        /// <param name="currMethod"></param>
        /// <param name="objExample">实体对象的类型</param>
        /// <param name="e"></param>
        public static void SysErrorSave(string currMethod, object objExample, Exception e)
        {
            try
            {
                DataTable dt = DataService.Data.OpenDataSingle("select top 0 * from t_SYLog", "t_SYLog");
                string classType = objExample.GetType().ToString(); //实体对象的类型
                string errorType = e.GetType().ToString();  //异常类型
                string msg_log = e.ToString();
                string msg_short = e.Message;

                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                row["SContent"] = msg_log;
                row["SContent_short"] = msg_short;
                row["ClassType"] = classType;
                row["CurrMethod"] = currMethod;
                row["ErrorType"] = errorType;
                row["BUser"] = _user.UserCode;
                row["IP"] = getClientIP();
                row.EndEdit();

                DataService.DataServer.proxy.SaveData(dt);
            }
            catch (Exception ee)
            {
                EventLog eventlog = new EventLog();
                eventlog.Source = "My EAP";
                eventlog.WriteEntry(e.Message + "\r\n" + ee.Message);
            }
        }


        /// <summary>
        /// 创建人：黎金来
        /// 日期：2014-01-02
        /// 写入错误日志
        /// </summary>
        /// <param name="currMethod">Common.Comm.CurrMethod</param>
        /// <param name="objExample">实体对象的类型</param>
        /// <param name="e"></param>
        public static void SysErrorSave(string currMethod, Exception e)
        {
            try
            {
                DataTable dt = DataService.Data.OpenDataSingle("select top 0 * from t_SYLog", "t_SYLog");
                string classType = ""; //实体对象的类型
                string errorType = e.GetType().ToString();  //异常类型
                string msg_log = e.ToString();
                string msg_short = e.Message;

                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                row["SContent"] = msg_log;
                row["SContent_short"] = msg_short;
                row["ClassType"] = classType;
                row["CurrMethod"] = currMethod;
                row["ErrorType"] = errorType;
                row["BUser"] = _user.UserCode;
                row["IP"] = getClientIP();
                row.EndEdit();

                DataService.DataServer.proxy.SaveData(dt);
            }
            catch (Exception ee)
            {
                EventLog eventlog = new EventLog();
                eventlog.Source = "My EAP";
                eventlog.WriteEntry(e.Message + "\r\n" + ee.Message);
            }
        }




        /// <summary>
        /// 创建人：方君业
        /// 日期：2014-02-21
        /// 生成公式编辑器所需字段字典表
        /// </summary>
        /// <param name="Sql">查询SQL</param>
        /// <param name="Param">查询参数</param>
        /// <param name="drAs">配置表数据</param>
        public static DataTable TurnToFormulaField(string Sql, string Param, DataRow[] drAs)
        {
            DataTable dtColumns = new DataTable();
            dtColumns.TableName = "table";
            dtColumns.Columns.Add("FieldName");
            dtColumns.Columns.Add("Caption");

            DataTable dt = null;


            if (Param.Trim() != "")
            {
                string[] Params = Param.Split(new char[] { ';' });
                for (int i = 0; i < Params.Length; i++)
                    Sql = Sql.Replace("@" + Params[i], "null");
            }
            if (Sql != null)
                dt = DataService.Data.OpenDataSingle(Sql, "table");

            if (dt != null && dt.Columns.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataRow dr = dtColumns.NewRow();
                    dr["FieldName"] = dt.Columns[i].ColumnName;
                    dr["Caption"] = dt.Columns[i].ColumnName;
                    dtColumns.Rows.Add(dr);
                }
                if (drAs != null)
                {
                    foreach (DataRow dra in drAs)
                    {
                        DataRow[] dr = dtColumns.Select("FieldName='" + dra["FieldName"].ToString() + "'");
                        if (dr.Length > 0)
                            dr[0]["Caption"] = dra["Caption"];
                    }
                }
            }
            else
            {
                if (drAs != null)
                {
                    foreach (DataRow dra in drAs)
                    {
                        DataRow dr = dtColumns.NewRow();
                        dr["FieldName"] = dra["FieldName"];
                        dr["Caption"] = dra["Caption"];
                        dtColumns.Rows.Add(dr);
                    }
                }
            }

            return dtColumns;

        }


        /// <summary>
        /// 创建人：方君业
        /// 日期：2014-05-3
        /// 获取模块配置数据表字典
        /// </summary>
        /// <param name="frmName">模块实例名</param>
        public static Dictionary<string, DataTable> GetModTableDic(string frmName)
        {
            Dictionary<string, DataTable> dicTable = new Dictionary<string, DataTable>();
            string serialno = "";
            DataTable dtModManage = DataService.Data.OpenDataSingle("select * from t_SYModManage where ClassName='" + frmName + "'", "t_SYModManage");
            if (dtModManage != null && dtModManage.Rows.Count > 0)
                serialno = dtModManage.Rows[0]["Serialno"].ToString();
            DataTable dtSysModManage_Config = DataService.Data.OpenDataSingle("select * from t_SYModManage_Config where Serialno='" + serialno + "'", "t_SYModManage_Config");

            //获取嵌套从档数据
            DataTable dtSysModBD = DataService.Data.OpenDataSingle("select * from t_SYModManagedl_B where serialno in (select guid from t_SYModManagedl_B where serialno in (select serialno from t_SYModManage where Serialno='" + dtSysModManage_Config.Rows[0]["Serialno"].ToString() + "')) order by PageNo", "t_SYModManagedl_B");
            DataTable dtSysModDD = DataService.Data.OpenDataSingle("select a.PageName,a.PageCaption,a.SearchPage,a.DetailTableNick,b.* from t_SYModManagedl_B a inner join (select * from t_SYModManagedl_D  where PGuid in (select guid from t_SYModManagedl_B where serialno in (select guid from t_SYModManagedl_B where serialno in (select serialno from t_SYModManage where Serialno='" + dtSysModManage_Config.Rows[0]["Serialno"].ToString() + "')))) b on  a.guid=b.pguid  order by b.SNo", "t_SYModManagedl_D");

            //主档
            DataTable dtSysModManagedl_M = DataService.Data.OpenDataSingle("select * from t_SYModManagedl_M where PGuid in (select Guid from t_SYModManagedl_P where Serialno='" + dtSysModManage_Config.Rows[0]["Serialno"].ToString() + "')", "t_SYModManagedl_M");
            DataTable dtm = Comm.TurnToFormulaField(dtSysModManage_Config.Rows[0]["MasterSql"].ToString() + " where 1<>1", "", dtSysModManagedl_M.Select("1=1"));
            dicTable.Add("M;主档", dtm);

            //从档
            DataTable dtSysModManagedl_B = DataService.Data.OpenDataSingle("select * from t_SYModManagedl_B where Serialno='" + dtSysModManage_Config.Rows[0]["Serialno"].ToString() + "'", "t_SYModManagedl_B");
            DataTable dtSysModManagedl_D = DataService.Data.OpenDataSingle("select * from t_SYModManagedl_D where PGuid in (select Guid from t_SYModManagedl_B where Serialno='" + dtSysModManage_Config.Rows[0]["Serialno"].ToString() + "')", "t_SYModManagedl_D");
            for (int i = 0; i < dtSysModManagedl_B.Rows.Count; i++)
            {
                DataRow[] drDs = dtSysModManagedl_D.Select("PGuid='" + dtSysModManagedl_B.Rows[i]["Guid"].ToString() + "'");
                DataTable dt = Comm.TurnToFormulaField(dtSysModManagedl_B.Rows[i]["DetailSql_Test"].ToString(), dtSysModManagedl_B.Rows[i]["DetailSqlVals"].ToString(), drDs);
                string detailTableNick = dtSysModManagedl_B.Rows[i]["DetailTableNick"].ToString();
                string pageCaption = dtSysModManagedl_B.Rows[i]["PageCaption"].ToString();
                dicTable.Add(detailTableNick + ";" + pageCaption, dt);
            }

            //嵌套从档
            for (int i = 0; i < dtSysModBD.Rows.Count; i++)
            {
                DataRow[] drDs = dtSysModDD.Select("PGuid='" + dtSysModBD.Rows[i]["Guid"].ToString() + "'");
                DataTable dt = Comm.TurnToFormulaField(dtSysModBD.Rows[i]["DetailSql_Test"].ToString(), dtSysModBD.Rows[i]["DetailSqlVals"].ToString(), drDs);
                string detailTableNick = dtSysModBD.Rows[i]["DetailTableNick"].ToString();
                string pageCaption = dtSysModBD.Rows[i]["PageCaption"].ToString();
                dicTable.Add(detailTableNick + ";" + pageCaption, dt);
            }
            return dicTable;
        }

        #region 图片
        private static Image picChecked;
        private static Image picApprove;
        private static Image picConfirm;
        private static Image picCancellation;
        private static Image picNoCheck;
        private static Image picChecking;
        private static Image picDone;

        public static Image PicChecked
        {
            get
            {
                if (picChecked == null)
                {
                    string filepath = Application.StartupPath + "\\image\\Checked.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picChecked = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picChecked;
            }
            set { picChecked = value; }
        }

        public static Image PicApprove
        {
            get
            {
                if (picApprove == null)
                {
                    string filepath = Application.StartupPath + "\\image\\Approved.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picApprove = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picApprove;
            }
            set { picApprove = value; }
        }

        public static Image PicConfirm
        {
            get
            {
                if (picConfirm == null)
                {
                    string filepath = Application.StartupPath + "\\image\\Confirm.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picConfirm = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picConfirm;
            }
            set { picConfirm = value; }
        }

        public static Image PicCancellation
        {
            get
            {
                if (picCancellation == null)
                {
                    string filepath = Application.StartupPath + "\\image\\Cancellation.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picCancellation = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picCancellation;
            }
            set { picCancellation = value; }
        }

        public static Image PicNoCheck
        {
            get
            {
                if (picNoCheck == null)
                {
                    string filepath = Application.StartupPath + "\\image\\NoCheck.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picNoCheck = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picNoCheck;
            }
            set { picNoCheck = value; }
        }

        public static Image PicChecking
        {
            get
            {
                if (picChecking == null)
                {
                    string filepath = Application.StartupPath + "\\image\\Checking.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picChecking = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picChecking;
            }
            set { picChecking = value; }
        }

        public static Image PicDone
        {
            get
            {
                if (picDone == null)
                {
                    string filepath = Application.StartupPath + "\\image\\Done.png";
                    if (File.Exists(filepath))
                    {
                        Stream s = File.Open(filepath, FileMode.Open);
                        picDone = Image.FromStream(s);
                        s.Close();
                    }
                }
                return picDone;
            }
            set { picDone = value; }
        }
        #endregion

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2014-05-23
        /// 列印日期，如遇到节假日期，时间自动往后移
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWorkDate(DateTime dt)
        {
            HolidayHandler hh = new HolidayHandler();
            DateTime dtdate = hh.GetWorkDate(dt);
            return dtdate;
        }



        /// <summary>
        /// 创建人：黎金来
        /// 日期：2015-03-26
        /// 获取模块配置数据表字典(基础配置表)
        /// </summary>
        /// <param name="frmName">模块实例名</param>
        public static Dictionary<string, DataTable> GetModTableDicBySingle(string frmName)
        {
            Dictionary<string, DataTable> dicTable = new Dictionary<string, DataTable>();
            string serialno = "", strSQL = "";
            strSQL = "select * from t_SYModManage where ClassName='" + frmName + "'";
            DataTable dtModManage = DataService.Data.OpenDataSingle(strSQL, "t_SYModManage");

            if (dtModManage != null && dtModManage.Rows.Count > 0)
                serialno = dtModManage.Rows[0]["Serialno"].ToString();

            strSQL = "select * from t_SYModSingle_Config where Serialno='" + serialno + "'";
            DataTable dtSysModManage_Config = DataService.Data.OpenDataSingle(strSQL,"t_SYModSingle_Config");

            strSQL = "select * from t_SYModSingledl_D where PGuid in (select Guid from t_SYModSingledl_B where Serialno='" + serialno + "')";
            DataTable dtSysModManagedl_D = DataService.Data.OpenDataSingle(strSQL, "t_SYModManagedl_D");
         
            for (int i = 0; i < dtSysModManage_Config.Rows.Count; i++)
            {
                DataRow[] drDs = dtSysModManagedl_D.Select(" 1=1");
                DataTable dt = Comm.TurnToFormulaField(dtSysModManage_Config.Rows[i]["ListSql"].ToString(),"",drDs);
                
                dicTable.Add("M;基础配置", dt);
                break;
            }
                        
            return dicTable;
        }


        #region 繁简体转换

        /// <summary>
        /// 繁简体转换,按用户参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StrConv(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || _user == null)
                return str;

            if (_user.HasUseTraditional)
            {
                if (_user.UseTraditional)
                    return ConvertHandler.ToTraditionalChinese(str);
                else
                    return ConvertHandler.ToSimplifiedChinese(str);
            }
            return str;
        }

        /// <summary>
        /// 繁简体转换,按传入参数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IsTraditiona">是否转为繁体</param>
        /// <returns></returns>
        public static string StrConv(string str, bool IsTraditiona)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            if (IsTraditiona)
                return ConvertHandler.ToTraditionalChinese(str);
            else
                return ConvertHandler.ToSimplifiedChinese(str);            
        }

        /// <summary>
        /// 数据行的繁简体转换,按用户参数
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DataRow StrConvDataRow(DataRow row)
        {
            if (row == null || row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
                return row;

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (row.Table.Columns[i].DataType == typeof(string))
                    row[i] = StrConv(Convert.ToString(row[i]));
            }
            row.AcceptChanges();
            return row;
        }

        /// <summary>
        /// 数据行的繁简体转换,按传人参数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="IsTraditiona">是否转为繁体</param>
        /// <returns></returns>
        public static DataRow StrConvDataRow(DataRow row, bool IsTraditiona)
        {
            if (row == null || row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
                return row;

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (row.Table.Columns[i].DataType == typeof(string))
                    row[i] = StrConv(Convert.ToString(row[i]), IsTraditiona);
            }
            row.AcceptChanges();
            return row;
        }

        /// <summary>
        /// 数据表的繁简体转换,按用户参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable StrConvDataTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return dt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                StrConvDataRow(row);
            }

            return dt;
        }

        /// <summary>
        /// 数据表的繁简体转换,按传人参数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="IsTraditiona">是否转为繁体</param>
        /// <returns></returns>
        public static DataTable StrConvDataTable(DataTable dt, bool IsTraditiona)
        {
            if (dt == null || dt.Rows.Count == 0)
                return dt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                StrConvDataRow(row, IsTraditiona);
            }

            return dt;
        }

        /// <summary>
        /// 数据集的繁简体转换,按用户参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataSet StrConvDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return ds;
            
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                Comm.StrConvDataTable(ds.Tables[i]);
            }
            return ds;
        }

        /// <summary>
        /// 数据集的繁简体转换,按传人参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataSet StrConvDataSet(DataSet ds, bool IsTraditiona)
        {
            if (ds == null || ds.Tables.Count == 0)
                return ds;

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                Comm.StrConvDataTable(ds.Tables[i], IsTraditiona);
            }
            return ds;
        }
        #endregion



        public static void SetBillStatus(string Serialno, string className,string action)
        {
            string sqlConfig = "select * from t_SYModManage_Config where Serialno in (select Serialno from t_SYModManage where ClassName='" + className + "')";
            DataTable dtConfig = DataService.Data.OpenDataSingle(sqlConfig, "t_SYModManage_Config");
            if (dtConfig.Rows.Count > 0)
            {
                List<string> sqls = new List<string>();
                string sql="";
                string CheckField = dtConfig.Rows[0]["CheckField"].ToString();
                string CheckerField = dtConfig.Rows[0]["CheckerField"].ToString();
                string CheckerNameField = dtConfig.Rows[0]["CheckerNameField"].ToString();
                string CheckDateField = dtConfig.Rows[0]["CheckDateField"].ToString();
                string ApproveField = dtConfig.Rows[0]["ApproveField"].ToString();
                string ApproverField = dtConfig.Rows[0]["ApproverField"].ToString();
                string ApproverNameField = dtConfig.Rows[0]["ApproverNameField"].ToString();
                string ApproveDateField = dtConfig.Rows[0]["ApproveDateField"].ToString();
                string ConfirmField = dtConfig.Rows[0]["ConfirmField"].ToString();
                string ConfirmerField = dtConfig.Rows[0]["ConfirmerField"].ToString();
                string ConfirmerNameField = dtConfig.Rows[0]["ConfirmerNameField"].ToString();
                string ConfirmDateField = dtConfig.Rows[0]["ConfirmDateField"].ToString();
                bool UnApproveToRoot = ConvertHandler.ToBoolean(dtConfig.Rows[0]["UnApproveToRoot"]);
                bool UnConfirmToRoot = ConvertHandler.ToBoolean(dtConfig.Rows[0]["UnConfirmToRoot"]);
                string MasterPKField = dtConfig.Rows[0]["MasterPKField"].ToString();
                string MasterTableName = dtConfig.Rows[0]["MasterTableName"].ToString();
                if (action == "审批")
                {
                    sql = "update " + MasterTableName + " set " + ApproveField + "=1," + ApproverField + "='"
                        + Comm._user.UserCode + "'," + ApproveDateField + "=getdate() where " + MasterPKField + "='" + Serialno + "'";
                    //sqls.Add(sql);
                }
                if (action == "确认")
                {
                    sql = "update " + MasterTableName + " set " + ConfirmField + "=0," + ConfirmerField + "='"
                        + Comm._user.UserCode + "'," + ConfirmDateField + "=getdate() where " + MasterPKField + "='" + Serialno + "'";
                    //sqls.Add(sql);
                }
                DataService.Data.ExecuteNonQuery(sql);
            }
        }

        public static Form LoadSysMod(Form mainForm, string nameSpaceName, string formClassName, string inStanceName,
                                string displayName, string modFunId, bool showDialog, bool openMore, TabControlEx tabControlEx1)
        {
            if ((nameSpaceName != "") && (formClassName != "") && (inStanceName != ""))
            {
                Assembly loadedDll = null;
                if (_dlls.Keys.Contains(nameSpaceName + "." + "dll"))
                    loadedDll = _dlls[nameSpaceName + "." + "dll"];
                else
                {
                    string path = Application.StartupPath + "\\" + nameSpaceName + ".dll";
                    try
                    {
                        loadedDll = Assembly.LoadFrom(path);
                        _dlls.Add(nameSpaceName + ".dll", loadedDll);
                    }
                    catch (FileNotFoundException fe)
                    {
                        //_frmMain.CloseWaitForm(true);
                        //FrmBase.MsgError("找不到：“" + path + "”，请查看配置是否正确！");
                        return null;
                    }
                }
                TabPage tp = new TabPage();
                tp.ImageIndex = 2;
                tp.Location = new System.Drawing.Point(4, 26);
                tp.Name = inStanceName;
                tp.Padding = new System.Windows.Forms.Padding(3);
                tp.Size = new System.Drawing.Size(876, 89);
                tp.TabIndex = 2;
                tp.Text = displayName;
                tp.UseVisualStyleBackColor = true;
                tabControlEx1.Controls.Add(tp);

                Type[] types = loadedDll.GetTypes();
                Type type;
                try
                {
                    type = types.Single(type1 => type1.Name == formClassName);
                    if (type == null)
                        return null;
                    if (!showDialog)
                    {
                        if (openMore == false)
                        {
                            /*
                            if (loadedPlugins2.ContainsKey(inStanceName))
                            {
                                Form f = loadedPlugins2[inStanceName];
                                f.BringToFront();
                                MessageBox.Show("123");
                                return f;
                            }
                             */
                            FrmBase objForm = (FrmBase)loadedDll.CreateInstance(type.FullName);
                            //objForm.TopLevel = false;
                            //objForm.Parent = tp;
                            //objForm.ControlBox = false;
                            //objForm.Dock = System.Windows.Forms.DockStyle.Fill;
                            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                            objForm.TopLevel = false;
                            objForm.Dock = DockStyle.Fill;
                            objForm.Name = inStanceName;
                            objForm.Text = displayName;
                            tp.Controls.Add(objForm);
                            objForm.Show();
                            //objForm.ShowDlg(inStanceName, displayName, modFunId);
                            //Comm.loadedPlugins.Add(inStanceName);//如果没有打开过，就存入打开集合 
                            //Comm.loadedPlugins2.Add(inStanceName, objForm);//如果没有打开过，就存入打开集合 
                            return objForm;
                        }
                        else
                        {
                            //可重复打开同一模块
                            FrmBase objForm = (FrmBase)loadedDll.CreateInstance(type.FullName);
                            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                            objForm.TopLevel = false;
                            objForm.Dock = DockStyle.Fill;
                            objForm.Name = inStanceName;
                            objForm.Text = displayName;
                            tp.Controls.Add(objForm);
                            objForm.Show();
                            //objForm.ShowDlg(inStanceName, displayName, modFunId);
                            //if (!loadedPlugins2.ContainsKey(inStanceName))
                            //    Comm.loadedPlugins2.Add(inStanceName, objForm);
                            return objForm;
                        }
                    }
                    else
                    {
                        FrmBase objForm = (FrmBase)loadedDll.CreateInstance(type.FullName);
                        objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        objForm.TopLevel = false;
                        objForm.Dock = DockStyle.Fill;
                        objForm.Name = inStanceName;
                        objForm.Text = displayName;
                        tp.Controls.Add(objForm);
                        objForm.Show();
                        //objForm.ShowDlgForm(inStanceName, displayName, modFunId);
                        return objForm;
                    }

                }
                catch (InvalidOperationException ioe)
                {
                    //_frmMain.CloseWaitForm(true);
                    //_frmMain.MsgError(ioe.Message + "\n请检查是否存在类型：“" + formClassName + "”");
                    return null;
                }
            }
            return null;

        }
    }
}
