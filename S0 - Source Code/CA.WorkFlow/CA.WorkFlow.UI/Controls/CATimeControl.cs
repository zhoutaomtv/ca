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
using System.ComponentModel;
using System.Security.Permissions;

namespace CA.WorkFlow.UI
{  
    //""+Date js
    public class CADateTimeControl : DateTimeControl //,IValidator
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            
        }   
    }
}
