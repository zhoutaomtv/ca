using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using System.Collections.Generic;

namespace CA.SharePoint.WebControls
{
    public partial class CompanyNavigator : System.Web.UI.UserControl
    {
        public string Link
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillData();
        }

        private void FillData()
        {
            SPList list;
            SPListItemCollection items;
            if (string.IsNullOrEmpty(this.Page.Request["dept"]))
            {
                this.PermissionControl1.PermissionGroups = CAConstants.GroupName.CAOwners;

                list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.CompanyLink);
                items = list.Items;
                this.Link = list.DefaultViewUrl;
            }
            else
            {
                string dept = this.Page.Request["dept"];

                list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.DepartmentLinks);
                string userDept = UserProfileUtil.GetEmployee(SPContext.Current.Web.CurrentUser.LoginName).Department;
                if ((userDept.ToLower() == dept.ToLower())
                    || (UserProfileUtil.GetDepartmentDisplayName(userDept.ToLower()) == dept.ToLower()))
                {
                    this.PermissionControl1.PermissionGroups = CAConstants.GroupName.DepartmentPageMangager;
                }
                SPFolder folder;
                try
                {
                    folder = SharePointUtil.EnsureFolder(SPContext.Current.Web, list, dept);

                    SPQuery query = new SPQuery();
                    query.Folder = folder;
                    items = list.GetItems(query);
                    this.Link = list.RootFolder.SubFolders[dept].ServerRelativeUrl;
                    this.Link += ("?Dept=" + dept);
                }
                catch
                {
                    items = null;
                }
            }

            if (items == null)
            {
                return;
            }

            List<CALink> lstLinks = new List<CALink>();
            CALink link = null;
            foreach (SPListItem item in items)
            {
                link = new CALink(item["URL"] + "");
                lstLinks.Add(link);
            }

            this.repLinks.DataSource = lstLinks;
            this.repLinks.DataBind();

        }
    }
}