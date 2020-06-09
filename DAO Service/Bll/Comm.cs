using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data;

namespace Bll
{
    public static class Comm
    {

        private static string clientIP = null;  //客户端ip地址
        private static string clientHostName = null;  //客户端计算机名称

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
        /// 日期：2014-05-23
        /// 列印日期，如遇到节假日期，时间自动往后移
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWorkDate(DateTime dt, DataTable dtCalendar)
        {
            HolidayHandler hh = new HolidayHandler(dtCalendar);
            DateTime dtdate = hh.GetWorkDate(dt);
            return dtdate;
        }
    }
}
