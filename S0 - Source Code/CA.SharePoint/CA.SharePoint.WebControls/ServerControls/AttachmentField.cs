using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.Design;

namespace CA.SharePoint.WebControls
{
    class AttachmentFieldDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            return "[Attachment]";
        }
    }

    [Designer(typeof(AttachmentFieldDesigner))]
    public class AttachmentField : FormComponent
    {
        protected override string DefaultTemplateName
        {
            get
            {
                if (this.ControlMode == SPControlMode.Display)
                    return "QuickFlow_DisplayFormAttachments";
                else
                    return "QuickFlow_FormAttachments";
            }
        }       
    }
}
