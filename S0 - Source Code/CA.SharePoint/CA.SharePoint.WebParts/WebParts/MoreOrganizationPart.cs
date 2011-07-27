using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint.WebParts;

namespace CA.SharePoint.WebParts
{
    public class MoreOrganizationPart : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "MoreOrganization.ascx";
            }
        }
    }
}
