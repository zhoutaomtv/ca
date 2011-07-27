using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Microsoft.SharePoint;
using CA.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint.Utilities;
using System.Text;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint.Administration;
using System.Net.Mail;
using System.Net.Mime;

namespace CA.WorkFlow
{
    public partial class NewsApprove : SPLayoutsPageBase
    {
        private string _FileUrl = string.Empty;

        public string FileUrl
        {
            get { return _FileUrl; }
            set { _FileUrl = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SPListItem item = SPContext.Current.ListItem;

                this.PagePath.Add(SPContext.Current.List.Title, SPContext.Current.List.DefaultViewUrl);
                this.PagePath.Add(item.Title, "");

                this.Title = SPContext.Current.List.Title;

                if (item.Attachments.Count > 0)
                {
                    this.divFile.Visible = true;

                    SPFolder folder = SPContext.Current.List.RootFolder.SubFolders["Attachments"].SubFolders[item.ID.ToString()];

                    SPFile file = folder.Files[0];

                    //this.FileUrl = file.ServerRelativeUrl;
                    this.FileUrl = this.Page.Request.RawUrl.Replace("NewsApprove", "NewsFormDoc");
                    //this.Page.Response.Redirect(this.Page.Request.RawUrl.Replace("NewsView", "NewsFormDoc"));
                }
                else
                {
                    this.divFile.Visible = false;
                }

                this.litTitle.Text = item.Title;
                this.litCreated.Text = new SPFieldLookupValue(item["Created By"] + "").LookupValue;
                this.litTime.Text = DateTime.Parse(item["Created"] + "").ToString("yyyy-MM-dd");
                this.litBody.Text = item["Body"] + "";
                if (SPContext.Current.List.Title.Equals("Internal Announcement"))
                {
                    divEmail.Visible = true;
                    ckbEmail.Checked = item["Email"].ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    divEmail.Visible = false;
                }
            }

            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action == "Approve")
            {
                WorkflowContext.Current.DataFields["Approved"] = 1;

                if (SPContext.Current.List.Title.Equals("Internal Announcement"))
                {
                    SendEmails();
                }
            }
        }

        private void SendEmails()
        {
            string receivers = ConfigurationManager.AppSettings["commemailreceivers"];

            if (string.IsNullOrEmpty(receivers)||ckbEmail.Checked==false)
                return;

            List<string> mailList = new List<string>();

            //if (receivers.Equals("all", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    List<Employee> employees = UserProfileUtil.GetEmployeeFromSSPByDept(string.Empty);

            //    foreach (Employee employee in employees)
            //    {
            //        mailList.Add(employee.WorkEmail);
            //    }
            //}
            //else
            //{
            string[] users = receivers.Split(new char[] { ';', ',' });

            foreach (string user in users)
            {
                mailList.Add(user);
                //Employee emp = UserProfileUtil.GetEmployeeByDisplayName(user);
                //mailList.Add(emp.WorkEmail);
            }
            //}

            string mfrom = ConfigurationManager.AppSettings["commemailsender"] + "";
            if (string.IsNullOrEmpty(mfrom))
            {
                mfrom = "communication@C-AND-A.CN";
            }

            StringBuilder mcontent = new StringBuilder();
            mcontent.Append("<html><head><meta http-equiv='content-type' content='text/html; charset=utf-8'></head><body>");
            mcontent.Append("<img src='cid:headerimg' />");
            mcontent.Append(SPContext.Current.ListItem["Body"]);
            mcontent.Append("<img src='cid:footerimg' />");
            mcontent.Append("</body></html>");

            //Added by Don 2010-12-21
            string mailTitle = SPContext.Current.ListItem["Title"].ToString();
            if (mailTitle == "") mailTitle = "HR COMMUNICATION";

            foreach (string address in mailList)
            {
                SendEmail(mailTitle, mfrom, address, mcontent.ToString());
            }
        }

        private void SendEmail(string pSubject, string pFrom, string pTo, string pBody)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            message.IsBodyHtml = true;
            message.Body = pBody;
            message.From = new System.Net.Mail.MailAddress(pFrom, "Communication");
            message.To.Add(pTo);
            message.Bcc.Add("spsadmin@C-AND-A.CN");
            message.Subject = pSubject;

            AlternateView av = AlternateView.CreateAlternateViewFromString(pBody, null, MediaTypeNames.Text.Html);
            LinkedResource lr = new LinkedResource(Server.MapPath("/_layouts/CAResources/themeCA/images/hrcommheader.jpg"), MediaTypeNames.Image.Jpeg);
            lr.ContentId = "headerimg";
            av.LinkedResources.Add(lr);
            lr = new LinkedResource(Server.MapPath("/_layouts/CAResources/themeCA/images/hrcommfooter.jpg"), MediaTypeNames.Image.Jpeg);
            lr.ContentId = "footerimg";
            av.LinkedResources.Add(lr);
            message.AlternateViews.Add(av);  
          
            SPOutboundMailServiceInstance smtpServer = this.Web.Site.WebApplication.OutboundMailServiceInstance;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer.Server.Address);

            smtp.Send(message);
            message.Dispose();
    
        }

        protected string GetFrameSrc()
        {
            return this.Page.Request.RawUrl.Replace("NewsApprove", "NewsFormDoc");
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            GoRedirect();
        }

        private void GoRedirect()
        {
            if (Request.QueryString["Source"] != null && !string.IsNullOrEmpty(Request.QueryString["Source"].ToString()))
                this.Page.Response.Redirect(Request.QueryString["Source"].ToString());
            else
                this.Page.Response.Redirect(SPContext.Current.List.DefaultViewUrl);
        }

        //public string dd()
        //{
        //    SPSite oSiteCollection = SPContext.Current.Site;
        //    SPWebCollection collWebsites = oSiteCollection.AllWebs;

        //    SPWeb oWebsite = collWebsites["Site_Name"];
        //    SPFolder oFolder = oWebsite.Folders["Shared Documents"];

        //    foreach (SPWeb oWebsiteNext in collWebsites)
        //    {
        //        SPList oList = oWebsiteNext.Lists["List_Name"];
        //        SPListItem oListItem = oList.Items[0];
        //        SPAttachmentCollection collAttachments = oListItem.Attachments;

        //        collAttachments[

        //        SPFileCollection collFiles = oFolder.Files;

        //        foreach (SPFile oFile in collFiles)
        //        {
        //            string strFileName = oFile.Name;

        //            byte[] binFile = oFile.OpenBinary();

        //            collAttachments.Add(strFileName, binFile);
        //        }

        //        oListItem.Update();
        //        oWebsiteNext.Dispose();
        //    }
        //    oWebsite.Dispose();
        //}
    }
}
