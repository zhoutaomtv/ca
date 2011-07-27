using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint.WebParts;

namespace CA.SharePoint
{
    public class CALeaveReport : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "CALeaveReport.ascx";
            }
        }
    }
}
