using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint;
using System.IO;
using CA.SharePoint.Utilities.Common;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Text;
using System.DirectoryServices;

namespace CA.SharePoint.Web
{
    public partial class PublishNote : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string listName = "MarketingCommunicationNotes";
                if (SPContext.Current.List != null)
                {
                    listName = SPContext.Current.List.Title;
                }
                this.Title = listName;
                this.PagePath.Add(listName, SPContext.Current.List.DefaultViewUrl);
                this.PagePath.Add("New Item", "");
            }

            this.btnSave.Click += new ImageClickEventHandler(btnSave_Click);
            this.btnCancle.Click += new ImageClickEventHandler(btnCancle_Click);

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            SPList list = SPContext.Current.List;
            SPListItem item = list.Items.Add();
            item["Title"] = this.txtTitle.Text.Trim();
            item["Content"] = this.formFieldBody.Value;

            item.Web.AllowUnsafeUpdates = true;
            item.Update();

            
            string sendmail = ConfigurationManager.AppSettings["mcnoticesendmail"] + "";
            if (sendmail.ToLower() == "on")
            {
                //send mails
                string rootweburl = ConfigurationManager.AppSettings["rootweburl"] + "";
                if (string.IsNullOrEmpty(rootweburl))
                {
                    rootweburl = "http://cnshsps.cnaidc.cn:91";
                }
                SPSite site = new SPSite(rootweburl);
                SPWeb web = site.OpenWeb("documentcenter");
                //SPList mclist = SPContext.Current.Web.Lists["MarketingCommunication"];
                SPList mclist = web.Lists["Marketing Communication"];


                string libraryFolder = ConfigurationManager.AppSettings["marketingcommunicationlibrary"] + "";
                if (string.IsNullOrEmpty(libraryFolder))
                {
                    libraryFolder = "MarketingCommunication/MarketingCommunication";
                }
                SPFolder folder = GetFolder(web, mclist, libraryFolder);

                if (folder != null)
                {
                    List<string> mailList = new List<string>();

                    try
                    {
                        foreach (SPRoleAssignment role in folder.Item.RoleAssignments)
                        {
                            string membertype = role.Member.GetType().ToString();
                            if (membertype == "Microsoft.SharePoint.SPGroup")
                            {
                                string groupName = ((SPGroup)role.Member).Name;
                                List<string> names = UserProfileUtil.UserListInGroup(groupName);

                                foreach (string name in names)
                                {
                                    if (name.Equals("CNAIDC\\" + groupName, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        GetEmailsByGroup(mailList, groupName);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Employee mcuser = UserProfileUtil.GetEmployee(name);
                                            mailList.Add(mcuser.WorkEmail);
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    Employee mcuser = UserProfileUtil.GetEmployee(role.Member.ToString());
                                    mailList.Add(mcuser.WorkEmail);
                                }
                                catch
                                { }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }

                    string mfrom = ConfigurationManager.AppSettings["mcnoticemailfrom"] + "";
                    if (string.IsNullOrEmpty(mfrom))
                    {
                        mfrom = "spsadmin@C-AND-A.CN";
                    }

                    StringDictionary dict = new StringDictionary();
                    mailList.Sort();
                    dict.Add("from", mfrom);
                    dict.Add("to", string.Join(";", mailList.Distinct().ToArray()));
                    dict.Add("bcc", mfrom);
                    dict.Add("subject", "Marketing Communication Note Notification");
                    StringBuilder mcontent = new StringBuilder();
                    mcontent.Append("<html><head></head><body>");

                    string mbody = ConfigurationManager.AppSettings["mcnoticecontent"] + "";
                    if (string.IsNullOrEmpty(mbody))
                    {
                        mbody = "A new Marketing Communication Note has been added, please go to Intranet to check.<br/> 今天有新的市场部通讯上传，请查看。<br/>";
                    }
                    else
                    {
                        mbody = mbody.Replace("\n", "<br/>");
                    }
                    string link =rootweburl + "/documentcenter/_layouts/ca/marketingcommunication.aspx";

                    mcontent.Append(mbody + "<br /><a href='{0}' target='_blank'>Marketing Communication</a></body></html>");

                    SPUtility.SendEmail(web, dict, string.Format(mcontent.ToString(), link));
                }
            }
            GoRedirect();

        }

        public static SPFolder GetFolder(SPWeb web, SPList list, string folderURL)
        {
            if (String.IsNullOrEmpty(folderURL))
                return list.RootFolder;

            string folderFullURL = folderURL.TrimStart('/');

            SPFolder f = web.GetFolder(folderFullURL);
            if (f.Exists)
                return f;
            else
                return null;
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

        private void GetEmailsByGroup(List<string> mailList, string groupName)
        {
            DirectoryEntry objADAM = new DirectoryEntry("LDAP://OU=dlgroup,dc=cnaidc,dc=cn", "cnaidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
            DirectorySearcher objSearchADAM = new DirectorySearcher(objADAM);
            objSearchADAM.Filter = "(&(objectClass=group)(cn=" + groupName + "))";
            //objSearchADAM.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            SearchResult results = objSearchADAM.FindOne();
            objADAM.Close();

            DirectoryEntry deGroup = new DirectoryEntry(results.Path, "cnaidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
            //assign a property collection
            System.DirectoryServices.PropertyCollection pcoll = deGroup.Properties;
            int n = pcoll["member"].Count;

            if (n > 0)
            {
                foreach (string userpath in pcoll["member"])
                {

                    DirectoryEntry objUserEntry = new DirectoryEntry("LDAP://" + userpath, "cnidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
                    //objUserEntry.RefreshCache();

                    PropertyCollection userProps = objUserEntry.Properties;
                    objUserEntry.Close();

                    if (!string.IsNullOrEmpty(userProps["mail"].Value + ""))
                    {
                        mailList.Add(userProps["mail"].Value.ToString());
                    }
                }
            }
            else
            {
                throw new Exception("No groups found");
            }
        }
    }
}
