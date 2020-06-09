using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Common.CommForm;
namespace Common
{
    public static class Message
    {

        public static string sMsgTitle = "提示";

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 普通的消息提醒
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">提醒内容</param>
        public static void MsgAlert(this Form f, string sContent)
        {            
            //MessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            sContent = Comm.StrConv(sContent);
            //DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 普通的消息提醒(不是扩展方法)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">提醒内容</param>
        public static void MsgAlert(string sContent)
        {
            //MessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            sContent = Comm.StrConv(sContent);
            //DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        } 
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 警告的消息提示
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">消息内容</param>
        public static void MsgWarn(this Form f, string sContent)
        {
            //MessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            sContent = Comm.StrConv(sContent);
            //DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 警告的消息提示(不是扩展方法)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">消息内容</param>
        public static void MsgWarn(string sContent)
        {
            //MessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            sContent = Comm.StrConv(sContent);
            //DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 错误的消息提示
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">消息内容</param>
        public static void MsgError(this Form f, string sContent)
        {
            //MessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            sContent = Comm.StrConv(sContent);
            //DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2013-05-30
        /// 错误的消息提示(用户定制表单)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent"></param>
        public static void MsgErrorAtUserForm(this Form f, string sContent)
        {
            sContent = Comm.StrConv(sContent);
            //FrmErrMsgBox form = new FrmErrMsgBox(sMsgTitle, sContent);
            //form.ShowDialog();
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2013-05-30
        /// 错误的消息提示(用户定制表单)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent"></param>
        public static void MsgNoticeAtUserForm(this Form f, string sContent)
        {
            sContent = Comm.StrConv(sContent);
            //FrmErrMsgBox form = new FrmErrMsgBox(sMsgTitle, sContent, true);
            //form.ShowDialog();
        }


        /// <summary>
        /// 创建人：黎金来
        /// 日期：2015-01-17
        /// 错误的消息提示(用户定制表单)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent"></param>
        public static void MsgNoticeAtUserForm(string sContent)
        {
            sContent = Comm.StrConv(sContent);
            //FrmErrMsgBox form = new FrmErrMsgBox(sMsgTitle, sContent, true);
            //form.ShowDialog();
        }
         
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-09-17
        /// 错误的消息提示(不是扩展方法)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">消息内容</param>
        public static void MsgError(string sContent)
        {
            //MessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            sContent = Comm.StrConv(sContent);
            //DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        /// <summary>
        /// 创建人：黎金来、
        /// 日期：2012-09-17
        /// 询问的消息提示
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent">消息内容</param>
        /// <returns>OK:返回True; NO:返回False</returns>
        //public static bool MsgConfirm(this Form f, string sContent)
        //{
        //    sContent = Comm.StrConv(sContent);
        //    return DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.YesNo, 
        //        MessageBoxIcon.Question) == DialogResult.Yes;
        //}

        //public static bool MsgConfirm(string sContent)
        //{
        //    sContent = Comm.StrConv(sContent);
        //    return DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.YesNo, 
        //        MessageBoxIcon.Question) == DialogResult.Yes;       
        //}

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-04-15
        /// 提提示框有三个选项（终止、重试和忽略）
        /// </summary>
        /// <param name="sContent"></param>
        /// <returns></returns>
        //public static DialogResult MsgAbortRetryIgnore(string sContent)
        //{
        //    sContent = Comm.StrConv(sContent);
        //    return DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
        //}


        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-04-15
        /// 扩展的提示框有三个选项（终止、重试和忽略）
        /// </summary>
        /// <param name="sContent"></param>
        /// <returns></returns>
        //public static DialogResult MsgAbortRetryIgnore(this Form f, string sContent)
        //{
        //    sContent = Comm.StrConv(sContent);
        //    return DevExpress.XtraEditors.XtraMessageBox.Show(sContent, sMsgTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
        //}

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2015-03-04
        /// 错误的消息提示(自动关闭窗体)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent"></param>
        public static void MsgNoticeAtUserForm(string sContent,bool IsClose)
        {
            sContent = Comm.StrConv(sContent);
            //FrmErrMsgBox form = new FrmErrMsgBox(sContent, IsClose);
            //form.ShowDialog();
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2015-03-04
        /// 错误的消息提示(自动关闭窗体)
        /// 自定义图标
        /// </summary>
        /// <param name="f"></param>
        /// <param name="sContent"></param>
        public static void MsgNoticeAtUserForm(string sContent, bool IsClose, string ImgType, System.Drawing.Color color)
        {
            sContent = Comm.StrConv(sContent);
            //FrmErrMsgBox form = new FrmErrMsgBox(sContent, IsClose, ImgType,color);
            //form.ShowDialog();
        } 

       
    }
}
