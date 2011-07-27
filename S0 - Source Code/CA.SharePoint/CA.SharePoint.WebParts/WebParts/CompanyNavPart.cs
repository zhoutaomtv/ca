using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint.WebParts;

namespace CA.SharePoint.WebParts
{
    public class CompanyNavPart : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "CompanyNavigator.ascx";
            }
        }
    }
}
