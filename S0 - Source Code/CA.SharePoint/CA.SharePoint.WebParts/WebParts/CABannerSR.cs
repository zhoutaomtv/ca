using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint.WebParts;

namespace CA.SharePoint
{
    public class CABannerSR : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "CABannerSR.ascx";
            }
        }
    }
}
