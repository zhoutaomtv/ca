using System;

namespace CA.WorkFlow
{
    public interface IMailService
    {
        void SendMail(string subject, string body, System.Collections.Specialized.StringCollection to, System.Collections.Specialized.StringCollection cc, System.Collections.Generic.IDictionary<string, System.IO.Stream> attachments);
        void SendMail(string subject, string body, System.Collections.Specialized.StringCollection to);
    }

    //sharepoint 发送邮件 默认有方法
       //SPUtility.SendEmail(web, true, false, user.Email, msg, htmlBody, false);

    public static class MailServiceFactory
    {

        static public IMailService GetMailService()
        {
            return new MailService();
        }

    }
}
