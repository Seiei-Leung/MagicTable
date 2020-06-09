using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mail;
using System.Net.Mail;
using System.ComponentModel;

namespace Common
{
    public  class Email
    {

        private string senderEmail = string.Empty, GMailPort = "";
        public string SmtpServer = string.Empty, mailPassword = string.Empty;
        private string[] ArrAddress;
        public string Error;
        public Email()
        {
            //senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
            //mailPassword = System.Configuration.ConfigurationManager.AppSettings["MailPassword"].ToString();
            //SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString();
            //GMailPort = System.Configuration.ConfigurationManager.AppSettings["GmailPort"].ToString();
            //senderEmail = Common.Comm._user.EmailName;
            //mailPassword = Common.Comm._user.EmailPwd;
            //SmtpServer = Common.Comm._user.SMTP;
            //senderEmail = "king0530@163.com";
            //mailPassword = "king939552";
            //SmtpServer = "smtp.163.com";

            senderEmail = "system@itx.com.cn";
            //mailPassword = "123654";
            mailPassword = "tw159357";
            SmtpServer = "mail.itx.com.cn";
            //SmtpServer = "59.33.36.124";
          
        }

        /// <summary>
        /// 创建人：黎金来
        /// 发邮件
        /// 日期：2013-1-23
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="to">收件人，多个收人，以分号隔开</param>
        /// <returns></returns>
        public bool MailSend(string title, string content, string to)
        {
            return this.Send(title, content, to, "");
        }

        /// <summary>
        /// 创建人：黎金来
        /// 发邮件
        /// 日期：2013-1-23
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="to">收件人，多个收人，以分号隔开</param>
        /// <param name="cc">抄送人，多个收人，以分号隔开</param>
        /// <returns></returns>
        public bool MailSend(string title, string content, string to, string cc)
        {
            return this.Send(title, content, to, cc);
        }

        /// <summary>
        /// 创建人：黎金来
        /// 发邮件
        /// 日期：2013-1-23
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="to">收件人，多个收件人，以分号隔开</param>
        /// <param name="cc">抄送人，多个抄送人，以分号隔开</param>
        /// <param name="bcc">密送人，多个密送人，以分号隔开</param>
        /// <returns></returns>
        public bool MailSend(string title, string content, string to, string cc, string bcc)
        {
            return this.Send(title, content, to, cc, bcc, "");
        }

        /// <summary>
        /// 创建人：黎金来
        /// 发邮件
        /// 日期：2013-1-23
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="to">收件人，多个收件人，以分号隔开</param>
        /// <param name="cc">抄送人，多个抄送人，以分号隔开</param>
        /// <param name="bcc">密送人，多个密送人，以分号隔开</param>
        /// <param name="AttachPath">附件</param>
        /// <returns></returns>
        public bool MailSend(string title, string content, string to, string cc, string bcc, string AttachPath)
        {
            return this.Send(title, content, to, cc, bcc, AttachPath);
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2010-12-26
        /// 发邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="emailList">收件人EMAIL地址，多个EMAIL地址，有分号隔开</param>
        private bool Send(string title, string content, string To, string Cc)
        {
            string emailAddr = string.Empty;
            try
            {
                System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                System.Web.Mail.MailMessage mailMessage = new System.Web.Mail.MailMessage();
                mailMessage.To = To;//收件人
                mailMessage.From = senderEmail;//发件人                  
                if (Cc != "") mailMessage.Cc = Cc;
                mailMessage.Subject = title;
                mailMessage.Body = content;//邮件内容  
                mailMessage.BodyFormat = MailFormat.Html;
                mailMessage.BodyEncoding = System.Text.Encoding.Default;

                mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");           //认证类型  
                mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", senderEmail);       //要认证的用户名  
                mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", mailPassword);          //要认证的密码               
                SmtpMail.SmtpServer = SmtpServer;
                SmtpMail.Send(mailMessage);
            }
            catch (Exception E)
            {
                Error = E.ToString();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2010-12-26
        /// 发邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="emailList">收件人EMAIL地址，多个EMAIL地址，有分号隔开</param>
        private bool Send(string title, string content, string To, string Cc, string Bcc, string AttachPath)
        {
            string emailAddr = string.Empty;
            try
            {
                System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                System.Web.Mail.MailMessage mailMessage = new System.Web.Mail.MailMessage();
                MailAttachment objMailAttachment = null;

                //附件
                if (AttachPath != "")
                    objMailAttachment = new MailAttachment(AttachPath);

                mailMessage.To = To;//收件人
                mailMessage.From = senderEmail;//发件人                  
                if (Cc != "") mailMessage.Cc = Cc;
                if (Bcc != "") mailMessage.Bcc = Bcc;
                mailMessage.Subject = title;
                mailMessage.Body = content;//邮件内容  
                mailMessage.BodyFormat = MailFormat.Html;
                mailMessage.BodyEncoding = System.Text.Encoding.Default;
                mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");           //认证类型  
                mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", senderEmail);       //要认证的用户名  
                mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", mailPassword);          //要认证的密码               

                if (objMailAttachment != null)
                    mailMessage.Attachments.Add(objMailAttachment);

                SmtpMail.SmtpServer = SmtpServer;
                SmtpMail.Send(mailMessage);
            }
            catch (Exception E)
            {
                //point.FileTxtLogs.WriteLog(E.Message.Trim());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 发GMAIL邮件
        /// 创建人：黎金来
        /// 日期:2011-10-31
        ///  
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">邮件邮件</param>
        /// <param name="To">收件人，多个有收件人，用分号隔开；</param>
        /// <param name="Cc">抄送人，多个有抄送人，用分号隔开；</param>
        /// <param name="Bcc">暗送人，多个有暗送人，用分号隔开；</param>
        /// <returns></returns>
        private void SendByGmail(string title, string content, string To, string Cc, string Bcc)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            #region 收件人
            if (To != "")
            {
                ArrAddress = To.Split(';');
                foreach (string s in ArrAddress)
                {
                    if (s.Trim() != "")
                    {
                        mail.To.Add(s.Trim());
                    }

                }
            }
            else
            {
                return;
            }

            #endregion

            mail.From = new MailAddress(senderEmail, " 1Point Commerce ", System.Text.Encoding.UTF8);


            #region  抄送人
            if (Cc != "")
            {
                ArrAddress = Cc.Split(';');
                foreach (string s in ArrAddress)
                {
                    if (s.Trim() != "")
                    {
                        mail.CC.Add(s.Trim());
                    }

                }
            }
            #endregion

            #region 暗送人
            if (Bcc != "")
            {
                ArrAddress = Bcc.Split(';');
                foreach (string s in ArrAddress)
                {
                    if (s.Trim() != "")
                    {
                        mail.Bcc.Add(s.Trim());
                    }
                }
            }

            #endregion

            mail.Subject = title.Trim();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = content.Trim();
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            //mail.Priority = System.Net.Mail.MailPriority.Normal;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(senderEmail, mailPassword);

            client.Port = int.Parse(GMailPort); // Gmail works on this port
            client.Host = SmtpServer;
            client.EnableSsl = true; //Gmail works on Server Secured Layer
            //client.SendCompleted += new SendCompletedEventHandler(SendMailCompleted);

            try
            {
                client.Send(mail);
                // 在页面设置这个属性 Async="true",则可以使用异步
                //client.SendAsync(mail, mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                //point.FileTxtLogs.WriteLog(errorMessage);
            }
            finally
            {
                client.Dispose();
                mail = null;
            }

        }

        void SendMailCompleted(object sender, AsyncCompletedEventArgs e)
        {
            System.Net.Mail.MailMessage mailMsg = (System.Net.Mail.MailMessage)e.UserState;
            string subject = mailMsg.Subject;
            //if (e.Cancelled) // 邮件被取消 
            //{
            //    point.FileTxtLogs.WriteLog(e.Error + "<br> cancel");
            //}
            //if (e.Error != null)
            //{
            //    point.FileTxtLogs.WriteLog(e.Error + "<br> error");
            //}
            //else {
            //    point.FileTxtLogs.WriteLog("发送完成");
            //}

        }

    }
}
