using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; 
using IMLibrary4.Controls;
using IMLibrary4.Protocol;

namespace Model
{
    /// <summary>
    /// 登录用户信息类
    /// </summary>
    public class User
    {
        private string groupTreeCode;
        public string GroupTreeCode
        {
            get { return groupTreeCode; }
        }

        private string groupCode;
        public string GroupCode
        {
            get { return groupCode; }
        }

        private string groupName;
        public string GroupName
        {
            get { return groupName; }
        }

        private string brand;
        public string Brand
        {
            get { return brand; }
        }


        private string groupTreeCodes;

        public string GroupTreeCodes
        {
            get { return groupTreeCodes; }
        }

        private string groupCodes;

        public string GroupCodes
        {
            get { return groupCodes; }
        }

        private string groupIds;
        public string GroupIds
        {
            get { return groupIds; }
        }

        private string brands;
        public string Brands
        {
            get { return brands; }
        }

        private DataTable dtUserRights;

        public DataTable DtUserRights
        {
            get { return dtUserRights; }
        }
        private DataTable dtUserMod;

        public DataTable DtUserMod
        {
            get { return dtUserMod; }
            set { dtUserMod = value; }
        }

        private String userId;

        public String UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string userCode;

        public string UserCode
        {
            get { return userCode; }
        }

        private string season;

        public string Season
        {
            get { return season; }
            set { season = value; }
        }

        private bool imONOff;

        public bool ImONOff
        {
            get { return imONOff; }
            set { imONOff = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
        }
        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        private int grade;

        public int Grade
        {
            get { return grade; }
        }
        private int queryMonths;

        public int QueryMonths
        {
            get { return queryMonths; }
        }

        private int queryScope;
        /// <summary>
        /// 可查范围(按月)
        /// </summary>
        public int QueryScope { get { return queryScope; } }

        private bool isAdmin;

        public bool IsAdmin
        {
            get { return isAdmin; }
        }

        private DataTable dtUserRole;

        public DataTable DtUserRole
        {
            get { return dtUserRole; }
        }


        private string emailName;
        /// <summary>
        /// 邮箱账户
        /// </summary>
        public string EmailName
        {
            get { return this.emailName; }
        }

        private string emailPwd;
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string EmailPwd
        {
            get { return this.emailPwd; }
        }


        private string smtp;
        /// <summary>
        /// 发邮件服务器
        /// </summary>
        public string SMTP
        {
            get { return this.smtp; }
        }

        private byte[] pic;
        /// <summary>
        /// 用户图片
        /// </summary>
        public byte[] Pic
        {
            get { return pic; }
            set { pic = value; }
        }

        private bool unCheckUser;

        public bool UnCheckUser
        {
            get { return unCheckUser; }
        }

        private bool hideNotice;
        public bool HideNotice
        {
            get { return hideNotice; }
        }
                
        private bool useTraditional;
        public bool UseTraditional
        {
            get { return useTraditional; }
        }

        private bool hasUseTraditional;
        public bool HasUseTraditional
        {
            get { return hasUseTraditional; }
        }

        private bool warnNotice;
        public bool WarnNotice
        {
            get { return warnNotice; }
        }

        public User() { }

        #region 变量
        /// <summary>
        /// 是否登录
        /// </summary>
        private bool isLogin = false;
        /// <summary>
        /// 是否退出程序
        /// </summary>
        public bool isExit = false;

        /// <summary>
        /// 是否因为密码错误或服务超时产生的重复登录
        /// </summary>
        public static bool IsRepeat = false;

        /// <summary>
        /// 登录参数
        /// </summary>
        public static IMLibrary4.Protocol.Auth _auth = new IMLibrary4.Protocol.Auth();

        #endregion

        public User(DataTable dtUserRights, DataTable dtUserMod, DataTable dtUser, DataTable userRole, string groupTreeCodes, string groupCodes, string groupIds, DataTable userGroup)
        {
            Init(dtUserRights, dtUserMod, dtUser, userRole, groupTreeCodes, groupCodes, groupIds, userGroup);
        }

        public User(DataTable dtUserRights, DataTable dtUserMod, DataTable dtUser, DataTable userRole, string groupTreeCodes, string groupCodes, string groupIds, string brands, DataTable userGroup)
        {
            Init(dtUserRights, dtUserMod, dtUser, userRole, groupTreeCodes, groupCodes, groupIds, userGroup);
            this.brands = brands;
            this.brand = userGroup.Rows[0]["Brand"].ToString();
        }

        private void Init(DataTable dtUserRights, DataTable dtUserMod, DataTable dtUser, DataTable userRole, string groupTreeCodes, string groupCodes, string groupIds, DataTable userGroup)
        {

            this.dtUserRights = dtUserRights;
            this.dtUserMod = dtUserMod;
            this.userId = Convert.ToString(dtUser.Rows[0]["Id"]);
            this.userCode = Convert.ToString(dtUser.Rows[0]["Code"]);
            this.userName = Convert.ToString(dtUser.Rows[0]["Name"]);
            this.passWord = Convert.ToString(dtUser.Rows[0]["PW"]);
            this.imONOff = ToBoolean(dtUser.Rows[0]["On_IM_Off"]);
            this.grade = ToInt32(dtUser.Rows[0]["Grade"]);
            this.queryMonths = ToInt32(dtUser.Rows[0]["QueryMons"]);
            this.queryScope = ToInt32(dtUser.Rows[0]["QueryScope"]);
            this.isAdmin = ToBoolean(dtUser.Rows[0]["IsAdmin"]);
            this.emailName = Convert.ToString(dtUser.Rows[0]["EmailName"].ToString());
            this.emailPwd = Convert.ToString(dtUser.Rows[0]["EmailPWD"].ToString());
            this.smtp = Convert.ToString(dtUser.Rows[0]["SMTP"].ToString());
            this.dtUserRole = userRole;
            this.groupCodes = groupCodes;
            this.groupTreeCodes = groupTreeCodes;
            this.groupIds = groupIds;
            this.groupTreeCode = userGroup.Rows[0]["TreeCode"].ToString();
            this.groupCode = userGroup.Rows[0]["Code"].ToString();
            this.groupName = userGroup.Rows[0]["Name"].ToString();
            this.unCheckUser = ToBoolean(dtUser.Rows[0]["UnCheckUser"]);
            this.hideNotice = ToBoolean(dtUser.Rows[0]["hideNotice"]);

            hasUseTraditional = dtUser.Columns.Contains("UseTraditional");
            useTraditional = hasUseTraditional ? ToBoolean(dtUser.Rows[0]["UseTraditional"]) : false;

            if (dtUser.Columns.Contains("WarnNotice"))
                warnNotice = ToBoolean(dtUser.Rows[0]["WarnNotice"]);
            else
                warnNotice = false;

            _auth.UserID = this.userCode;
            _auth.UserName = this.userName;
            _auth.Password = this.passWord;

            if (_auth.ShowType == ShowType.Offline || _auth.ShowType == ShowType.away || _auth.ShowType == ShowType.chat || _auth.ShowType == ShowType.dnd || _auth.ShowType == ShowType.Else || _auth.ShowType == ShowType.Invisible)
                _auth.ShowType = ShowType.NONE;

            isLogin = true;//正常登录
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

        public string get_ProSupplier()
        {
            return "";
        }
    }
}
