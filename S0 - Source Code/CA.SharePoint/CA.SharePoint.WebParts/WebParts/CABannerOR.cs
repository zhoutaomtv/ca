using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint.WebParts;

namespace CA.SharePoint
{

    public class CABannerOR : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "CABannerOR.ascx";
            }
        }
    }
}
