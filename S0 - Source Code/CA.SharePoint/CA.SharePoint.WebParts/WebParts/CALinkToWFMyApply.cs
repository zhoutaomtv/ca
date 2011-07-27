using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using System.Data;
using System.IO;
using System.Xml;
using Microsoft.VisualBasic;
using CodeArt.SharePoint.CamlQuery;
using System.Web.UI.WebControls;

namespace CA.SharePoint
{
    public class CALinkToWFMyApply : BaseSPWebPart
    {
        private HyperLink hyperLink = null;

        protected override void CreateChildControls()
        {
            if (base.ChildControlsCreated) return;

            hyperLink = new HyperLink();
            hyperLink.CssClass = "CA_additem";

            SPList list = SPContext.Current.List;
            //string urlNew = list.ContentTypes[0].NewFormUrl;
            string text = "Switch to My Apply";

            hyperLink.Text = text;
            hyperLink.NavigateUrl = SPContext.Current.Web.Url + "/" + SPContext.Current.List.Views["MyApply"].Url;

            this.Controls.Add(hyperLink);

            base.CreateChildControls();

        }
    }
}
