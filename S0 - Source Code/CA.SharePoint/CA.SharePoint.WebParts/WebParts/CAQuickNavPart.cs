using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint.WebParts;

namespace CA.SharePoint.WebParts
{
    public class CAQuickNavPart : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "QuickNavigator.ascx";
            }
        }
    }
}
