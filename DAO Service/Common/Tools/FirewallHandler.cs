using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFwTypeLib;
using System.Management;

namespace Common
{
    /// <summary>
    /// 防火墙例外TCP、UDP端口操作
    /// </summary>
    public static class FirewallHandler
    {
        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-02-22</para>
        /// <para>添加防火墙例外端口</para>
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="port">端口</param>
        /// <param name="protocol">协议(TCP、UDP)</param>
        public static bool AddPort(string name, int port, string protocol)
        {
            try
            {
                //创建firewall管理类的实例
                INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));

                INetFwOpenPort objPort = (INetFwOpenPort)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));

                objPort.Name = name;
                objPort.Port = port;
                if (protocol.ToUpper().Equals("TCP"))
                {
                    objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                }
                else
                {
                    objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
                }
                objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
                objPort.Enabled = true;

                //加入到防火墙的管理策略
                foreach (INetFwOpenPort mPort in netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts)
                {
                    if (objPort == mPort)
                    {
                        return true;
                    }
                }
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(objPort);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-02-22</para>
        /// <para>将应用程序添加到防火墙例外</para>
        /// </summary>
        /// <param name="name">应用程序名称</param>
        /// <param name="executablePath">应用程序可执行文件全路径</param>
        public static bool AddApp(string name, string executablePath)
        {
            try
            {
                //创建firewall管理类的实例
                INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));

                INetFwAuthorizedApplication app = (INetFwAuthorizedApplication)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication"));

                //在例外列表里，程序显示的名称
                app.Name = name;

                //程序的路径及文件名
                app.ProcessImageFileName = executablePath;

                //是否启用该规则
                app.Enabled = true;

                //加入到防火墙的管理策略
                netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);

                //加入到防火墙的管理策略
                foreach (INetFwAuthorizedApplication mApp in netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications)
                {
                    if (app == mApp)
                    {
                        return true;
                    }
                }
                netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-02-22</para>
        /// <para>删除防火墙例外端口</para>
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="protocol">协议（TCP、UDP）</param>
        public static bool DelPort(int port, string protocol)
        {
            try
            {
                INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
                if (protocol.ToUpper().Equals("TCP"))
                {
                    netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
                }
                else
                {
                    netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-02-22</para>
        /// <para>删除防火墙例外中应用程序</para>
        /// </summary>
        /// <param name="executablePath">程序的绝对路径</param>
        public static bool DelApp(string executablePath)
        {
            try
            {
                INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
                netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(executablePath);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-03-01</para>
        /// <para>获取当前网卡IP地址</para>
        /// </summary>         
        /// <returns></returns>         
        public static string GetNetCardIP()
        {
            try
            {
                ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MO in MOC)
                {
                    if ((bool)MO["IPEnabled"])
                    {
                        string[] IPAddresses = (string[])MO["IPAddress"];
                        if (IPAddresses.Length > 0)
                        {
                            return IPAddresses[0].ToString();
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static string GetCPUId()
        {
            try
            {
                ManagementClass MC = new ManagementClass("Win32_Processor");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MO in MOC)
                {
                    string cpuInfo = MO.Properties["ProcessorId"].Value.ToString();
                    return cpuInfo;
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 获取物理内存大小
        /// </summary>
        /// <returns></returns>
        public static double GetPhisicalMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher();
            searcher.Query = new SelectQuery("Win32_PhysicalMemory", "", new string[] { "Capacity" });
            ManagementObjectCollection collection = searcher.Get();
            ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();
            
            long capacity = 0;
            while(em.MoveNext())
            {
                ManagementBaseObject baseObj = em.Current;
                if(baseObj.Properties["Capacity"].Value != null)
                {
                    try
                    {
                        capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                    }
                    catch(Exception e)
                    {                        
                        return 0;
                    }
                }
            }
            return capacity / 1024.0 / 1024.0 / 1024.0;
        }

        /// <summary>
        /// 获取CPU型号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj2 in searcher.Get())
            {
                try
                {
                    return (obj2.GetPropertyValue("Name").ToString() + "," + obj2.GetPropertyValue("CurrentClockSpeed").ToString() + " Mhz," + Environment.ProcessorCount.ToString() + " 个处理器");
                }
                catch
                {
                    continue;
                }
            }
            return "未知";
        }
    }
}
