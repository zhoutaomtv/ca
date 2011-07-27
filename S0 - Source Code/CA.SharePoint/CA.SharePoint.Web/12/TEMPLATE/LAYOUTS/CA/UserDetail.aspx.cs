using System;
using System.Collections;
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
using CA.SharePoint.Utilities.Common;

namespace CA.SharePoint.Web
{
    public partial class UserDetail : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request["ID"]))
                {
                    Employee emp = null;
                    int id = Convert.ToInt32(Page.Request["ID"]);
                    SPUser user = SPContext.Current.Site.RootWeb.SiteUsers.GetByID(id);
                    try
                    {
                        emp = UserProfileUtil.GetEmployee(user.LoginName);
                    }
                    catch
                    {
                        lblmessage.Visible = true;
                        ucUser.Visible = false;
                        return;
                    }
                    ucUser.User = emp;
                    ucUser.BindData();
                }
                else if (!string.IsNullOrEmpty(Page.Request["GUID"]))
                {
                    Employee emp = UserProfileUtil.GetEmployee(new Guid(Page.Request["GUID"]));
                    ucUser.User = emp;
                    ucUser.BindData();
                }
            }
        }
    }
}
