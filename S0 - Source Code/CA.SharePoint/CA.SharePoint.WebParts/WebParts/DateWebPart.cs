
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Specialized;
using System.Xml;
using System.Collections;
using CA.SharePoint.EditParts;

namespace CA.SharePoint
{
    /// <summary>
    /// »’∆⁄œ‘ æwebPart
    /// </summary>
    public class DateWebPart : BaseSPWebPart
    {



        protected override void RenderContents(HtmlTextWriter writer)
        {
            //base.RenderContents(writer);

          // writer.Write( DateTime.Now

            string[] week = new string[] { "Sunday", "Monday", "Tuesday", "Wedensday", "Thursday", "Friday", "Saturday" };

            writer.Write(DateTime.Now.ToString("yyyy-MM-dd") + week[(int)DateTime.Now.DayOfWeek]);
        }

    }
}
