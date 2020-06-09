using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using IMEnterpriseObject;
using IMEnterpriseObject.Class;
using IMEnterpriseObject.Components;
using IMLibrary3;
using IMLibrary3.Protocol;
using IMLibrary3.Net;

namespace Common
{
    public class IMUdpClass
    {
        UDPClass _udpLiten = new UDPClass();
        public UDPClass UdpLiten
        {
            get { return _udpLiten; }
            set { _udpLiten = value; }
        }
        public static IMUdpClass netMain = new IMUdpClass();
        /// <summary>
        /// 构造函数
        /// </summary>
        public IMUdpClass()
        {
            if (netMain == null || netMain.MyAuth == null)
                netMain = this;
        }


        #region 公共方法
        public void SendMsg(byte msgInfo, byte[] data, IPEndPoint ep)
        {
            byte[] buf = new byte[data.Length + 2];
            buf[0] = 255;
            buf[1] = msgInfo;
            Buffer.BlockCopy(data, 0, buf, 2, data.Length);
            this.UdpLiten.Send(buf, ep);
        }

        public void SendMsg(byte msgInfo, byte[] data, IPAddress ip, int port)
        {
            byte[] buf = new byte[data.Length + 2];
            buf[0] = 255;
            buf[1] = msgInfo;
            Buffer.BlockCopy(data, 0, buf, 2, data.Length);
            this.UdpLiten.Send(buf, ip, port);
        }

        public void SendMsg(byte msgInfo, byte[] data, string ip, int port)
        {
            byte[] buf = new byte[data.Length + 2];
            buf[0] = 255;
            buf[1] = msgInfo;
            Buffer.BlockCopy(data, 0, buf, 2, data.Length);
            this.UdpLiten.Send(buf, ip, port);
        }
        //public void Login(string userName, string password)
        //{
        //    this.LoginUser.Loginos = 0;
        //    this.LoginUser.Userid = userName;
        //    this.LoginUser.Pwd = password;
        //    string msg = userName + "|" + password + "|" + "0";
        //    byte[] data = Encoding.Default.GetBytes(msg);
        //    this.SendMsg(0, data, this.ServerIP, this.ServerPort);
        //    this.timer1.Enabled = true;
        //}
        #endregion

        /// <summary>
        /// 登录认证
        /// </summary>
        public Auth MyAuth = null;
        /// <summary>
        /// tcp客户端
        /// </summary>
        public TCPClient tcpClient = null;
        /// <summary>
        /// 发送消息至服务器(如：向服务器登录)
        /// </summary>
        /// <param name="e"></param>
        public void SendMessageToServer(Element e)
        {
            e.from = netMain.MyAuth.UserID;//此处告之服务器，消息由自己发送
            if (netMain.tcpClient != null && !netMain.tcpClient.IsDisposed && netMain.tcpClient.IsConnected)
            {
                netMain.tcpClient.Write(e);
            }
        }

        /// <summary>
        /// 发送消息至服务器
        /// </summary>
        /// <param name="e">消息对像</param>
        public void SendMessageToServer(object e)
        {
            if (netMain.tcpClient != null && !netMain.tcpClient.IsDisposed && netMain.tcpClient.IsConnected)
                netMain.tcpClient.Write(IMLibrary3.Protocol.Factory.CreateXMLMsg(e));
        }

        /// <summary>
        /// 发送消息给多位用户
        /// </summary>
        /// <param name="msgType">消息类型(工作流;任务;通知)</param>
        /// <param name="Content">消息类容</param>
        /// <param name="useridList">用户Id列表</param>
        /// <param name="usernameList">用户名列表</param>
        /// <param name="ClassName">模块实例名</param>
        /// <param name="Serialno">模块单据主键(serialno)</param>
        public void SendMessageToUser(string msgType,string Content, string useridList, string usernameList, string ClassName, string Serialno)
        {
            string SendName = Common.Comm._user.UserName;

            if (Content == "kill")
            {
                #region kill
                string dstr = "select top 1 * from t_SYIMGeneralMessage order by id desc";
                DataTable dt = DataService.DataServer.proxy.OpenDataSingle(dstr, "t_SYIMGeneralMessage");
                DataRow newRow = dt.NewRow();
                newRow["Content"] = Content;
                newRow["UserIdList"] = useridList;
                newRow["UserNameList"] = usernameList;
                newRow["SendTime"] = DateTime.Now;
                newRow["ModuleName"] = ClassName;
                newRow["ModuleGuid"] = Serialno;
                dt.Rows.Add(newRow);

                DataTable aadt = dt.Clone();
                aadt.Rows.Add(newRow.ItemArray);
                aadt.Rows[0]["Content"] = Content;

                string xml = IMLibrary3.Protocol.Factory.DataTable2XML(aadt);
                IMLibrary3.Protocol.GeneralMessage s = new IMLibrary3.Protocol.GeneralMessage();
                s.Content = xml;
                SendMessageToServer(s);//发送消息
                #endregion
            }
            else
            {
                #region 把数据插入表:t_SYMessageList(旨在做消息列表数据源)
                List<string> grps = new List<string>();
                if (useridList.Contains(";") && usernameList.Contains(";"))
                {
                    string gr = "select PCode,TreeCode,TypeName from t_SYMessageGroup where TreeCode<>1";
                    DataTable grb = DataService.DataServer.proxy.OpenDataSingle(gr, "t_SYMessageGroup");
                    int treecode = 0;
                    for (int j = 0; j < grb.Rows.Count; j++)
                    {
                        string groupname = grb.Rows[j]["TypeName"].ToString();
                        treecode = Convert.ToInt32(grb.Rows[j]["TreeCode"].ToString());
                        grps.Add(groupname);
                    }
                    if (!grps.Contains(msgType))
                    {
                        treecode = treecode + 1;
                        string cr = "insert into t_SYMessageGroup(Id,PCode,TreeCode,TypeName) values ('" + System.Guid.NewGuid().ToString() + "','1','" + treecode.ToString() + "','" + msgType + "')";
                        DataService.DataServer.commonProxy.ExecuteNonQuery(cr); //执行update与insert语句
                    }

                    string treeNum = "select TreeCode from t_SYMessageGroup where TypeName='" + msgType + "'";
                    DataTable grc = DataService.DataServer.proxy.OpenDataSingle(treeNum, "t_SYMessageGroup1");

                    string[] New_useridList = useridList.Split(';');
                    string[] New_usernameList = usernameList.Split(';');
                    for (int i = 0; i < New_useridList.Length; i++)
                    {
                        //Common.Message.MsgAlert(New_useridList[i].ToString());

                        string sql = "insert into t_SYMessageList(Id,TreeCode,TypeName,Content,UserId,UserName,SendTime,ModuleName,ModuleSerialno,SendName) values (";
                        sql += "'" + System.Guid.NewGuid().ToString() + "','" + grc.Rows[0][0].ToString()+ "','" + msgType + "','" + Content + "','" + New_useridList[i].ToString() + "','" + New_usernameList[i].ToString() + "','" + System.DateTime.Now.ToString() + "','" + ClassName + "','" + Serialno + "','" + SendName + "')";
                        DataService.DataServer.commonProxy.ExecuteNonQuery(sql); //执行update与insert语句
                    }
                }
                else
                {
                    string gr = "select PCode,TreeCode,TypeName from t_SYMessageGroup where TreeCode<>1";
                    DataTable grb = DataService.DataServer.proxy.OpenDataSingle(gr, "t_SYMessageGroup");
                    int treecode = 0;
                    for (int j = 0; j < grb.Rows.Count; j++)
                    {
                        string groupname = grb.Rows[j]["TypeName"].ToString();
                        treecode = Convert.ToInt32(grb.Rows[j]["TreeCode"].ToString());
                        grps.Add(groupname);
                    }
                    if (!grps.Contains(msgType))
                    {
                        treecode = treecode + 1;
                        string cr = "insert into t_SYMessageGroup(Id,PCode,TreeCode,TypeName) values ('" + System.Guid.NewGuid().ToString() + "','1','" + treecode.ToString() + "','" + msgType + "')";
                        DataService.DataServer.commonProxy.ExecuteNonQuery(cr); //执行update与insert语句
                    }

                    string treeNum = "select TreeCode from t_SYMessageGroup where TypeName='" + msgType + "'";
                    DataTable grc = DataService.DataServer.proxy.OpenDataSingle(treeNum, "t_SYMessageGroup1");

                    string sql = "insert into t_SYMessageList(Id,TreeCode,TypeName,Content,UserId,UserName,SendTime,ModuleName,ModuleSerialno,SendName) values (";
                    sql += "'" + System.Guid.NewGuid().ToString() + "','" + grc.Rows[0][0].ToString() + "','" + msgType + "','" + Content + "','" + useridList + "','" + usernameList + "','" + System.DateTime.Now.ToString() + "','" + ClassName + "','" + Serialno + "','" + SendName + "')";
                    DataService.DataServer.commonProxy.ExecuteNonQuery(sql); //执行update与insert语句
                }
                #endregion

                #region 构造消息并发送
                string dstr = "select top 1 * from t_SYIMGeneralMessage order by id desc";
                DataTable dt = DataService.DataServer.proxy.OpenDataSingle(dstr, "t_SYIMGeneralMessage");
                DataRow newRow = dt.NewRow();
                newRow["Content"] = Content;
                newRow["UserIdList"] = useridList;
                newRow["UserNameList"] = usernameList;
                newRow["SendTime"] = DateTime.Now;
                newRow["ModuleName"] = ClassName;
                newRow["ModuleGuid"] = Serialno;
                dt.Rows.Add(newRow);
                Comm.SaveTable(dt);
                dt.AcceptChanges();

                DataTable aadt = dt.Clone();
                aadt.Rows.Add(newRow.ItemArray);
                aadt.Rows[0]["Content"] = Content;

                string xml = IMLibrary3.Protocol.Factory.DataTable2XML(aadt);
                IMLibrary3.Protocol.GeneralMessage s = new IMLibrary3.Protocol.GeneralMessage();
                s.Content = xml;
                SendMessageToServer(s);//发送消息
                #endregion

            }
        }

        /// <summary>
        /// 发多用户
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="usercode"></param>
        /// <param name="ClassName"></param>
        /// <param name="Serialno"></param>
        public void SendMessageToUser_1(string msgType, string Content, string usercodeList, string ClassName, string Serialno)
        {
            #region 数据准备
            string SendName = Common.Comm._user.UserName;
            string dstr = "select top 1 * from t_SYIMGeneralMessage order by id desc";
            DataTable dt = DataService.DataServer.proxy.OpenDataSingle(dstr, "t_SYIMGeneralMessage");
            List<string> grps = new List<string>();
            string gr = "select PCode,TreeCode,TypeName from t_SYMessageGroup where TreeCode<>1";
            DataTable grb = DataService.DataServer.proxy.OpenDataSingle(gr, "t_SYMessageGroup");
            int treecode = 0;
            for (int j = 0; j < grb.Rows.Count; j++)
            {
                string groupname = grb.Rows[j]["TypeName"].ToString();
                treecode = Convert.ToInt32(grb.Rows[j]["TreeCode"].ToString());
                grps.Add(groupname);
            }
            #endregion

            if (usercodeList.Contains(";")) //发多人
            {
                string[] code = usercodeList.Split(';');
                for (int i = 0; i < code.Length; i++)
                {
                    string usercode = code[i].ToString();  //一个用户code
                    string tr1 = "select id,name from t_syusers where code='" + usercode + "'";
                    DataTable dt1 = DataService.DataServer.proxy.OpenDataSingle(tr1, "t_syusers1");

                    #region 把数据插入表:t_SYMessageList(旨在做消息列表数据源)
                    if (!grps.Contains(msgType))
                    {
                        treecode = treecode + 1;
                        string cr = "insert into t_SYMessageGroup(Id,PCode,TreeCode,TypeName) values ('" + System.Guid.NewGuid().ToString() + "','1','" + treecode.ToString() + "','" + msgType + "')";
                        DataService.DataServer.commonProxy.ExecuteNonQuery(cr); //执行update与insert语句
                    }

                    string treeNum = "select TreeCode from t_SYMessageGroup where TypeName='" + msgType + "'";
                    DataTable grc = DataService.DataServer.proxy.OpenDataSingle(treeNum, "t_SYMessageGroup1");

                    string sql = "insert into t_SYMessageList(Id,TreeCode,TypeName,Content,UserId,UserName,SendTime,ModuleName,ModuleSerialno,SendName) values (";
                    sql += "'" + System.Guid.NewGuid().ToString() + "','" + grc.Rows[0][0].ToString() + "','" + msgType + "','" + Content + "','" + dt1.Rows[0][0].ToString() + "','" + dt1.Rows[0][1].ToString() + "','" + System.DateTime.Now.ToString() + "','" + ClassName + "','" + Serialno + "','" + SendName + "')";
                    DataService.DataServer.commonProxy.ExecuteNonQuery(sql); //执行update与insert语句
                    #endregion

                    #region 构造消息并发送

                    DataRow newRow = dt.NewRow();
                    newRow["Content"] = Content;
                    newRow["UserIdList"] = dt1.Rows[0][0].ToString();
                    newRow["UserNameList"] = dt1.Rows[0][1].ToString();
                    newRow["SendTime"] = DateTime.Now;
                    newRow["ModuleName"] = ClassName;
                    newRow["ModuleGuid"] = Serialno;
                    dt.Rows.Add(newRow);
                    Comm.SaveTable(dt);
                    dt.AcceptChanges();

                    DataTable aadt = dt.Clone();
                    aadt.Rows.Add(newRow.ItemArray);
                    aadt.Rows[0]["Content"] = Content;

                    string xml = IMLibrary3.Protocol.Factory.DataTable2XML(aadt);
                    IMLibrary3.Protocol.GeneralMessage s = new IMLibrary3.Protocol.GeneralMessage();
                    s.Content = xml;
                    SendMessageToServer(s);//发送消息
                    #endregion
                }
            }
            else  //发一人
            {
                string tr1 = "select id,name from t_syusers where code='" + usercodeList + "'";
                DataTable dt1 = DataService.DataServer.proxy.OpenDataSingle(tr1, "t_syusers1");
                
                #region 把数据插入表:t_SYMessageList(旨在做消息列表数据源)
                if (!grps.Contains(msgType))
                {
                    treecode = treecode + 1;
                    string cr = "insert into t_SYMessageGroup(Id,PCode,TreeCode,TypeName) values ('" + System.Guid.NewGuid().ToString() + "','1','" + treecode.ToString() + "','" + msgType + "')";
                    DataService.DataServer.commonProxy.ExecuteNonQuery(cr); //执行update与insert语句
                }

                string treeNum = "select TreeCode from t_SYMessageGroup where TypeName='" + msgType + "'";
                DataTable grc = DataService.DataServer.proxy.OpenDataSingle(treeNum, "t_SYMessageGroup1");

                string sql = "insert into t_SYMessageList(Id,TreeCode,TypeName,Content,UserId,UserName,SendTime,ModuleName,ModuleSerialno,SendName) values (";
                sql += "'" + System.Guid.NewGuid().ToString() + "','" + grc.Rows[0][0].ToString() + "','" + msgType + "','" + Content + "','" + dt1.Rows[0][0].ToString() + "','" + dt1.Rows[0][1].ToString() + "','" + System.DateTime.Now.ToString() + "','" + ClassName + "','" + Serialno + "','" + SendName + "')";
                DataService.DataServer.commonProxy.ExecuteNonQuery(sql); //执行update与insert语句
                #endregion

                #region 构造消息并发送
                DataRow newRow = dt.NewRow();
                newRow["Content"] = Content;
                newRow["UserIdList"] = dt1.Rows[0][0].ToString();
                newRow["UserNameList"] = dt1.Rows[0][1].ToString();
                newRow["SendTime"] = DateTime.Now;
                newRow["ModuleName"] = ClassName;
                newRow["ModuleGuid"] = Serialno;
                dt.Rows.Add(newRow);
                Comm.SaveTable(dt);
                dt.AcceptChanges();

                DataTable aadt = dt.Clone();
                aadt.Rows.Add(newRow.ItemArray);
                aadt.Rows[0]["Content"] = Content;

                string xml = IMLibrary3.Protocol.Factory.DataTable2XML(aadt);
                IMLibrary3.Protocol.GeneralMessage s = new IMLibrary3.Protocol.GeneralMessage();
                s.Content = xml;
                SendMessageToServer(s);//发送消息
                #endregion
            }
        }
    }
}
