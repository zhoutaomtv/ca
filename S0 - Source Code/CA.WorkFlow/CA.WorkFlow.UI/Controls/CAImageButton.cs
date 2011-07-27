using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint.WebControls;
using QuickFlow.UI.Controls;

namespace CA.WorkFlow.UI
{
    public class CAImageButton : ImageButton
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.OnClientClick +=  ";__SubmitConfirmMessage =''; ";
        }
    }
}
