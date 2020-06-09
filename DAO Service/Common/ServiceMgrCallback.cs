using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public class ServiceMgrCallback : ServiceMgr.IServiceMgrCallback
    {
        public void OnSessionKilled(ServiceMgr.SessionInfo sessionInfo, string msg)
        {
            Comm._frmMain.MsgAlert(msg);
            Comm._frmMain.IsSystemExit = true;
            Comm._frmMain.Close();
        }

        public void OnSessionTimeout(ServiceMgr.SessionInfo sessionInfo)
        {
            Comm._frmMain.MsgAlert("程序运行超时！");
            Comm._frmMain.IsSystemExit = true;
            Comm._frmMain.Close();
        }

        public void OnRepeatLogin(ServiceMgr.SessionInfo sessionInfo)
        {
            if(Comm._frmMain.MsgConfirm("该用户存在其他会话，登录将关闭该用户的其他会话，是否继续？"))
            {
                DataServer.ServiceProxy.RepeatSessionKilled(sessionInfo);
            }
            else
            {
                Comm._frmMain.IsSystemExit = true;
                Comm._frmMain.Close();
            }
        }
    }
}
