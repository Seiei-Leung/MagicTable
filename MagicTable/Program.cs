using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MagicTable
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            //Application.Run(new BlackForm());

            //try
            //{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (Comm.LocalConfig.LinkType == "0")
                {
                    string conString = FileHandler.GetConString();
                    SqlConnection mySqlConnection = new SqlConnection(conString);
                    mySqlConnection.Open();
                }     
                Application.Run(new MainForm()); ;
                //FrmLogin myLogin = new FrmLogin();
                //System.Diagnostics.Debug.WriteLine("ʱ��5��" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                //myLogin.ShowDialog();
                //if (myLogin.DialogResult == DialogResult.OK)
                //{
                //    if (Common.Comm.SysType != "SysType")
                //        Application.Run(new FrmMain());
                //    else
                //    {
                //        Application.Run(new ERPMain.Forms.wy.Form1());
                //    }
                //}
            //}
            //catch (Exception ex)
            //{
            //    //Common.Message.MsgError(ex.Message);
            //    MessageBox.Show(String.Format("��ǰ���ݿ�����ʧ�ܣ�����ȷ�������ݿ������!", ex.Message), "���ݿ�����", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    //FrmLinkConfig frmLinkConfig = new FrmLinkConfig();
            //    //if (frmLinkConfig.ShowDialog() == DialogResult.OK)
            //    //{
            //    //    Process.Start(Application.ExecutablePath);
            //    //    Environment.Exit(0);
            //    //}

            //    return;
            //}
            
        }
    }
}