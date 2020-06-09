using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace Model
{ 
    public class SystemParams
    {
        private string numericFormat;
        private string moneyFormat;
        private string footerFormat;
        //private string ftpServerIP; 
        private string ftpRemotePath;
        private string ftpUserID;
        private string ftpPassword;
        private string controlReadOnlyColor;
        private string controlNoNullColor;
        private string colReadOnlyColor;
        private string colNoNullColor;
        private string versions;
        private int queryMons;
        private int queryScope;
        private string systemCaption;
        private bool showNavigationPage;
        private bool showDesktop;
        private bool hideWarning;
        private bool hideTask;
        private bool hideSchedule;
        private bool hideNotice;
        private bool showChart;
        private string unCheckTable;
        private string unCheckUserCode;
        private string unCheckUserName;
        private bool useIM;
        private bool encrypt;
        private DateTime encryptSDate;
        private DateTime encryptEDate;
        private int lockTime;
        private bool autoLock;
        private bool unCheckDefault;
        private string navType;
        private string posBarcodePriority = "";//分销：POS条码扫描规则
        private bool useIconRemote;
        private bool enableOpenForBusiness;//分销：启用开始营业
        private bool holidayLimit;
        private bool loginShowName;
        private bool useSeason;
        private bool isT;
        private bool showApproveOpinion;

        private bool _PW_Encryption;
        private string _PW_Default;
        private int _PW_Len1;
        private int _PW_Len2;
        private bool _PW_ContainNum;
        private bool _PW_Letters;
        private bool _PW_SpecialChar;
        private bool _PW_WithoutLimitingAdmin;

        private bool _ShowImgByDockPanel;

        #region 密码配置
        public bool PW_Encryption
        {
            get { return _PW_Encryption; }
        }
        public string PW_Default
        {
            get { return _PW_Default; }
        }
        public int PW_Len1
        {
            get { return _PW_Len1; }
        }
        public int PW_Len2
        {
            get { return _PW_Len2; }
        }
        public bool PW_ContainNum
        {
            get { return _PW_ContainNum; }
        }
        public bool PW_Letters
        {
            get { return _PW_Letters; }
        }
        public bool PW_SpecialChar
        {
            get { return _PW_SpecialChar; }
        }
        public bool PW_WithoutLimitingAdmin
        {
            get { return _PW_WithoutLimitingAdmin; }
        }
        #endregion

        /// <summary> 以浮动框显示图片 </summary>
        public bool ShowImgByDockPanel
        {
            get { return _ShowImgByDockPanel; }
        }

        /// <summary>分销：启用开始营业 </summary>
        public bool EnableOpenForBusiness
        {
            get { return enableOpenForBusiness; }
        }

        public bool Encrypt { get; set; }
        public DateTime EncryptSDate { get; set; }
        public DateTime EncryptEDate { get; set; }

        public bool IsT
        {
            get { return isT; }
        }

        public bool ShowApproveOpinion
        {
            get { return showApproveOpinion; }
        }

        public bool UseSeason
        {
            get { return useSeason; }
        }

        public bool LoginShowName
        {
            get { return loginShowName; }
        }

        public bool HolidayLimit
        {
            get { return holidayLimit; }
        }

        public bool UseIconRemote
        {
            get { return useIconRemote; }
        }
        /// <summary>分销：POS条码扫描规则</summary>
        public string PosBarcodePriority
        {
            get { return posBarcodePriority; }
        }
        public string NavType
        {
            get { return navType; }
        }
        public bool UnCheckDefault
        {
            get { return unCheckDefault; }
        }
        public bool AutoLock
        {
            get { return autoLock; }
        }
        public int LockTime
        {
            get { return lockTime; }
        }
        public string UnCheckTable
        {
            get { return unCheckTable; }
        }
        public string UnCheckUserCode
        {
            get { return unCheckUserCode; }
        }
        public string UnCheckUserName
        {
            get { return unCheckUserName; }
        }
        public bool ShowNavigationPage
        {
            get { return showNavigationPage; }
        }
        public bool ShowDesktop
        {
            get { return showDesktop; }
        }
        public bool HideSchedule
        {
            get { return hideSchedule; }
            set { hideSchedule = value; }
        }
        public bool HideTask
        {
            get { return hideTask; }
            set { hideTask = value; }
        }
        public bool HideNotice
        {
            get { return hideNotice; }
        }
        public bool HideWarning
        {
            get { return hideWarning; }
        }
        public bool ShowChart
        {
            get { return showChart; }
        }
        public bool UseIM
        {
            get { return useIM; }
        }
        public string SystemCaption
        {
            get { return systemCaption; }
        }

        public int QueryMons
        {
            get { return queryMons; }
        }

        public int QueryScope
        {
            get { return queryScope; }
        }

        public string ControlReadOnlyColor
        {
            get { return controlReadOnlyColor; }
        }


        public string ControlNoNullColor
        {
            get { return controlNoNullColor; }
        }

        public string ColReadOnlyColor
        {
            get { return colReadOnlyColor; }
        }


        public string ColNoNullColor
        {
            get { return colNoNullColor; }
        }
        public string Versions
        {
            get { return versions; }
        }
        public string SqlConnectString { get; set; }
        public string TimeOut { get; set; }
        public string IMServerIP { get; set; }
        public string IMServerPort { get; set; }
        public string CompanyName { get; set; }

        public string FooterFormat
        {
            get { return footerFormat; }
        }
        public string NumericFormat { get { return numericFormat; } }
        public string MoneyFormat { get { return moneyFormat; } }
        //public string FtpServerIP { get { return ftpServerIP; } }
        public string FtpRemotePath { get { return ftpRemotePath; } }
        public string FtpUserID { get { return ftpUserID; } }
        public string FtpPassword { get { return ftpPassword; } }

        /// <summary>配置弃审原因</summary>
        private bool unCheckReason = false;
        /// <summary>配置弃审原因</summary>
        public bool UnCheckReason { get { return unCheckReason; } }

        private bool billInforAdmin = false;
        public bool BillInforAdmin { get { return billInforAdmin; } }

        public SystemParams(DataTable dtSystemParams)
        {
            if (dtSystemParams != null && dtSystemParams.Rows.Count > 0)
            {
                this.footerFormat = dtSystemParams.Rows[0]["FooterFormat"].ToString();
                this.numericFormat = dtSystemParams.Rows[0]["NumericFormat"].ToString();
                this.moneyFormat = dtSystemParams.Rows[0]["MoneyFormat"].ToString();
                //this.ftpServerIP = dtSystemParams.Rows[0]["FtpServerIP"].ToString();
                this.ftpRemotePath = dtSystemParams.Rows[0]["FtpRemotePath"].ToString();
                this.ftpUserID = dtSystemParams.Rows[0]["FtpUserID"].ToString();
                this.ftpPassword = dtSystemParams.Rows[0]["FtpPassword"].ToString();
                this.controlReadOnlyColor = dtSystemParams.Rows[0]["controlreadOnlyColor"].ToString();
                this.controlNoNullColor = dtSystemParams.Rows[0]["controlNoNullColor"].ToString();
                this.colReadOnlyColor = dtSystemParams.Rows[0]["colReadOnlyColor"].ToString();
                this.colNoNullColor = dtSystemParams.Rows[0]["colNoNullColor"].ToString();
                this.queryMons = ToInt32(dtSystemParams.Rows[0]["queryMons"]);
                this.queryScope = ToInt32(dtSystemParams.Rows[0]["queryScope"]);
                this.systemCaption = dtSystemParams.Rows[0]["systemCaption"].ToString();
                this.showNavigationPage = ToBoolean(dtSystemParams.Rows[0]["showNavigationPage"]);
                this.showDesktop = ToBoolean(dtSystemParams.Rows[0]["showDesktop"]);
                this.showChart = ToBoolean(dtSystemParams.Rows[0]["showChart"]);
                this.hideWarning = ToBoolean(dtSystemParams.Rows[0]["hideWarning"]);
                this.hideTask = ToBoolean(dtSystemParams.Rows[0]["hideTask"]);
                this.hideSchedule = ToBoolean(dtSystemParams.Rows[0]["hideSchedule"]);
                this.hideNotice = ToBoolean(dtSystemParams.Rows[0]["HideNotice"]);
                this.useIM = ToBoolean(dtSystemParams.Rows[0]["useIM"]);
                this.unCheckDefault = ToBoolean(dtSystemParams.Rows[0]["UnCheckDefault"]);
                this.navType = dtSystemParams.Rows[0]["NavType"].ToString();
                this.useIconRemote = ToBoolean(dtSystemParams.Rows[0]["useIconRemote"]);
                //this.versions = dtSystemParams.Rows[0]["Versions"].ToString();
                if (dtSystemParams.Rows[0]["UnCheckTable"].ToString() != "")
                {
                    this.unCheckTable = dtSystemParams.Rows[0]["UnCheckTable"].ToString();
                    this.unCheckUserCode = dtSystemParams.Rows[0]["UnCheckUserCode"].ToString();
                    this.unCheckUserName = dtSystemParams.Rows[0]["UnCheckUserName"].ToString();
                }
                else
                {
                    this.unCheckTable = "t_SYEmployee";
                    this.unCheckUserCode = "Code";
                    this.unCheckUserName = "Name";
                }


                //this.SqlConnectString = dtSystemParams.Rows[0]["SqlConnectString"].ToString();
                this.TimeOut = dtSystemParams.Rows[0]["TimeOut"].ToString();
                //this.SqlConnectString = "Connection Timeout=" + TimeOut + ";" + ConfigurationManager.AppSettings["Constring"];
                
                this.IMServerIP = dtSystemParams.Rows[0]["IMServerIP"].ToString();
                //this.IMServerIP = "192.168.0.115";
                this.IMServerPort = dtSystemParams.Rows[0]["IMServerPort"].ToString();

                this.CompanyName = dtSystemParams.Rows[0]["CompanyName"].ToString();

                //this.encrypt = true;
                this.encrypt = ToBoolean(dtSystemParams.Rows[0]["Encrypt"]);
                if (dtSystemParams.Rows[0]["EncryptSDate"] == DBNull.Value)
                    this.encryptSDate = DateTime.Now.Date;
                else
                    this.encryptSDate = Convert.ToDateTime(dtSystemParams.Rows[0]["EncryptSDate"]);
                if (dtSystemParams.Rows[0]["EncryptEDate"] == DBNull.Value)
                    this.encryptEDate = DateTime.Now.Date;
                else
                    this.encryptEDate = Convert.ToDateTime(dtSystemParams.Rows[0]["EncryptEDate"]);
                
                this.lockTime = ToInt32(dtSystemParams.Rows[0]["LockTime"]); 
                this.autoLock = ToBoolean(dtSystemParams.Rows[0]["AutoLock"]);
                if (dtSystemParams.Columns.Contains("PosBarcodePriority"))
                    this.posBarcodePriority = Convert.ToString(dtSystemParams.Rows[0]["PosBarcodePriority"]);
                if (dtSystemParams.Columns.Contains("EnableOpenForBusiness"))
                    this.enableOpenForBusiness = ToBoolean(dtSystemParams.Rows[0]["EnableOpenForBusiness"]);
                this.holidayLimit = ToBoolean(dtSystemParams.Rows[0]["holidayLimit"]);
                this.loginShowName = ToBoolean(dtSystemParams.Rows[0]["LoginShowName"]);
                //安踏-登陆选择季节
                if (dtSystemParams.Columns.Contains("UseSeason"))
                    this.useSeason = ToBoolean(dtSystemParams.Rows[0]["UseSeason"]);
                if (dtSystemParams.Columns.Contains("PW_Encryption"))
                {
                    this._PW_Encryption = ToBoolean(dtSystemParams.Rows[0]["PW_Encryption"]);
                    this._PW_Default = Convert.ToString((dtSystemParams.Rows[0]["PW_Default"]));
                    this._PW_Len1 = dtSystemParams.Rows[0]["PW_Len1"] == DBNull.Value ? 0 : Convert.ToInt32(dtSystemParams.Rows[0]["PW_Len1"]);
                    this._PW_Len2 = dtSystemParams.Rows[0]["PW_Len2"] == DBNull.Value ? 0 : Convert.ToInt32(dtSystemParams.Rows[0]["PW_Len2"]);
                    this._PW_ContainNum = ToBoolean(dtSystemParams.Rows[0]["PW_ContainNum"]);
                    this._PW_Letters = ToBoolean(dtSystemParams.Rows[0]["PW_Letters"]);
                    this._PW_SpecialChar = ToBoolean(dtSystemParams.Rows[0]["PW_SpecialChar"]);
                    this._PW_WithoutLimitingAdmin = ToBoolean(dtSystemParams.Rows[0]["PW_WithoutLimitingAdmin"]);
                }
                
                if (dtSystemParams.Columns.Contains("ShowImgByDockPanel"))
                    this._ShowImgByDockPanel = ToBoolean(dtSystemParams.Rows[0]["ShowImgByDockPanel"]);

                if (dtSystemParams.Columns.Contains("UnCheckReason"))
                    this.unCheckReason = ToBoolean(dtSystemParams.Rows[0]["UnCheckReason"]);

                this.billInforAdmin = ToBoolean(dtSystemParams.Rows[0]["BillInforAdmin"]);
                this.isT = ToBoolean(dtSystemParams.Rows[0]["IsT"]);
                this.showApproveOpinion = ToBoolean(dtSystemParams.Rows[0]["ShowApproveOpinion"]); 
            }
        }

        private int ToInt32(object value)
        {
            if (value is DBNull)
                return 0;
            else
                return Convert.ToInt32(value);
        }

        private bool ToBoolean(object value)
        {
            if (value is DBNull)
                return false;
            else
                return Convert.ToBoolean(value);
        }
    }
}
