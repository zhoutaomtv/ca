using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web
{
    public class CloseButton : System.Web.UI.WebControls.WebControl
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<input type='button' onclick='window.close()' value='Close' class='formButton'>");
        }
    }
}
