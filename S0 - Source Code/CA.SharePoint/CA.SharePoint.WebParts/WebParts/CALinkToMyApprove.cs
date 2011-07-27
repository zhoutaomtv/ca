using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web;

namespace CA.SharePoint.WebParts
{
    public class CALinkToMyApprove : BaseSPWebPart
    {
        private HyperLink hyperLink = null;

        protected override void CreateChildControls()
        {
            if (base.ChildControlsCreated) return;

            hyperLink = new HyperLink();
            hyperLink.CssClass = "CA_additem";

            SPList list = SPContext.Current.List;
            //string urlNew = list.ContentTypes[0].NewFormUrl;
            string text = "Switch to My Approved";

            hyperLink.Text = text;
            hyperLink.NavigateUrl = SPContext.Current.Web.Url + "/" + SPContext.Current.List.Views["All Items"].Url
                + "?FilterField1=Approvers&FilterValue1=" + HttpUtility.UrlDecode(SPContext.Current.Web.CurrentUser.Name);

            this.Controls.Add(hyperLink);
            base.CreateChildControls();

        }


      
    }
}