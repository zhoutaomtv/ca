using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Navigation;
using System.ComponentModel;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;

namespace CA.SharePoint.WebParts
{
 

    public class CADepartmentNewsPart : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "DepartmentNews.ascx";
            }
        }

     
    }
}
