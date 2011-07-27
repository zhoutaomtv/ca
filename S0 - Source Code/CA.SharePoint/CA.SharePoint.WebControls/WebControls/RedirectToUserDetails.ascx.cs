using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.SharePoint.WebControls.WebControls
{
    public partial class RedirectToUserDetails : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Page.Request["ID"]))
            {
                Response.Redirect("/layouts/ca/UserDetail.aspx?ID=" + Page.Request["ID"]);
            }
            else if (!string.IsNullOrEmpty(Page.Request["GUID"]))
            {
                Response.Redirect("/layouts/ca/UserDetail.aspx?GUID=" + Page.Request["GUID"]);
            }
        }
    }
}