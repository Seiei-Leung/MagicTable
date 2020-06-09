using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 郑志冲
    /// <para>2013-03-08</para>
    /// <para>客户端本地用户配置文件对象</para>
    /// </summary>
    public class LocalConfig 
    {
        /// <summary>
        /// 界面窗体样式名称
        /// </summary>
        public string Skin { get; set; }

        /// <summary>
        /// 回调服务引用的端口
        /// </summary>
        public string ServicePort { get; set; }

        /// <summary>
        /// 是否直接读取SQL
        /// </summary>
        public string IsReadSQLConfig { get; set; }

        public bool IsReadSQLConfigBoolean
        {
            get
            {
                if (string.IsNullOrEmpty(IsReadSQLConfig) || IsReadSQLConfig == "0")
                    return false;
                else
                    return true;
            }
        }

        public string UpdateServerUrl { get; set; }

        /// <summary>
        /// 登录的默认用户名
        /// </summary>
        public string LoginDefaultUserName { get; set; }

        /// <summary>
        /// 连接方式
        /// </summary>
        //public string IsWCF { get; set; }

        public string LinkType { get; set; }

        /// <summary>
        /// FTP地址
        /// </summary>
        public string FtpServerIP { get; set; }
        ///// <summary>
        ///// IM连接方式
        ///// </summary>
        //public string IMLinkType { get; set; }
        /// <summary>
        /// IM地址
        /// </summary>
        public string IMServerIP { get; set; }
    }
}
