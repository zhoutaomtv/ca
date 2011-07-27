using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CA.SharePoint.WebParts
{
    public class RedirectToUserDetails : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "RedirectToUserDetails.ascx";
            }
        }
    }
}
