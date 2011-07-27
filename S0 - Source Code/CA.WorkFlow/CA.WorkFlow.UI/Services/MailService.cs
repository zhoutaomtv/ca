using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CA.WorkFlow
{   

    internal class MailService : IMailService
    {

        public void SendMail(String subject, string body, StringCollection to )
        {
            SendMail(subject, body, to, null, null);
        }

        public void SendMail(String subject, string body, StringCollection to, StringCollection cc , IDictionary<string,Stream> attachments )
        {
            string mailServerAddress = System.Configuration.ConfigurationManager.AppSettings["SmtpServerAddress"];
            string from = System.Configuration.ConfigurationManager.AppSettings["SenderMailAddress"];

			if (String.IsNullOrEmpty(mailServerAddress))
				throw new Exception("配置文件中缺少SmtpServerAddress配置");

			if (String.IsNullOrEmpty(from))
				throw new Exception("配置文件中缺少SenderMailAddress配置");

            MailMessage mailtoSent = new MailMessage();

            mailtoSent.From = new MailAddress(from);

            mailtoSent.Subject = subject;
            mailtoSent.Body = body;
            mailtoSent.IsBodyHtml = true ;

            foreach (String mail in to)
            {
                mailtoSent.To.Add(mail);                
            }

            if (cc != null && cc.Count != 0)
            {
                foreach (String mail in cc)
                {
                    mailtoSent.CC.Add(mail);
                }
            }

            if (attachments != null && attachments.Count > 0)
            {
                foreach( KeyValuePair<string,Stream> kv in attachments )
                {
                    Attachment att = new Attachment( kv.Value , kv.Key);
                    mailtoSent.Attachments.Add(att);
                }               
            }

            SmtpClient mailServer = new SmtpClient(mailServerAddress);

            mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
 
            mailServer.Send(mailtoSent);      

        }
    }

   

}
