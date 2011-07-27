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
     
    public class IframeWebPart : WebPart
    {
        private string _PageUrl;

        [Bindable(true)]
       // [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("Page Path")]
        public string PageUrl
        {
            get
            {
                return _PageUrl;
            }
            set
            {
                _PageUrl = value;
            }
        }
               

        protected override void RenderContents(HtmlTextWriter output)
        {

            if (_PageUrl == null || _PageUrl == "")
            {
                output.Write("IframeWebPart:No PageUrl");
                return;
            }

            output.Write("<iframe src='");
            output.Write( ResolveUrl(_PageUrl) );

            output.Write("' frameborder='0' style='margin:0px;width:100%;height:100%;'");

            output.Write("></iframe>");

            //this.Width
        }



        //protected override void Render(HtmlTextWriter writer)
        //{           

        //    if (this.Width.IsEmpty)
        //        this.Width = new Unit("100%");

        //    if (this.Height.IsEmpty)
        //        this.Height = new Unit("100%");

        //    writer.Write("<iframe id='" + this.ClientID + "' src='" + base.ResolveUrl(_PageUrl) + "' frameborder='0' scrolling='no'
        //style='margin:0px; width:" + this.Width.ToString() + "; height:" + this.Height.ToString() + ";'></iframe>");
        //}




    }
}
