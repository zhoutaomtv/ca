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

namespace CA.SharePoint.Web
{
    public partial class CustomActionsHello : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string clientQuery = Page.ClientQueryString;
            if (clientQuery == "NewMenu")
            {
                Response.Write("You came from the new document menu.");
            }
            else if (clientQuery == "UploadMenu")
            {
                Response.Write("You came from the upload menu.");
            }
            else if (clientQuery == "ActionsMenu")
            {
                Response.Write("You came from the actions menu.");
            }
            else if (clientQuery == "SettingsMenu")
            {
                Response.Write("You came from the settings menu.");
            }
            else if (clientQuery == "SiteActions")
            {
                Response.Write("You came from the Site Actions menu.");
            }
            else if (clientQuery == "ECBItem")
            {
                Response.Write("You came from the document's context menu.");
            }
            else if (clientQuery == "DisplayFormToolbar")
            {
                Response.Write("You came from the display item properties form.");
            }
            else if (clientQuery == "EditFormToolbar")
            {
                Response.Write("You came from the edit item properties form.");
            }
            else if (clientQuery == "Customization")
            {
                Response.Write("You came from the Site Settings menu.");
            }
            else if (clientQuery.StartsWith("General"))
            {
                Response.Write("You came from the Content Type Settings menu.");
            }

        }
    }
}
