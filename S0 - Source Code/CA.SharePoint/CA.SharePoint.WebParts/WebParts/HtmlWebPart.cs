using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CA.SharePoint
{
    public class HtmlWebPart : BaseSPWebPart
    {

        private string _Html = "";

        [Category("Customize Control Settings"),
         Description("Set Display html"),
         Browsable(true),
         DisplayName("Displayed html"),
        WebBrowsable,
        Personalizable( PersonalizationScope.Shared)
         ]
        public string Html
        {
            get { return _Html; }
            set { _Html = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //base.RenderContents(writer);
            writer.Write(Html);
        }
    }
}
