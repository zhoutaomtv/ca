using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Navigation;
using System.ComponentModel;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using CA.SharePoint.WebControls;


namespace CA.SharePoint.WebParts
{
 

    public class CAWorkFlowNavigation : TemplateWebPart
    {
        protected override string DefaultTemplateName
        {
            get
            {
                return "WorkFlowNav.ascx";
            }
        }

        private WorkFlowPage _WorkFlowPage = WorkFlowPage.HomePage;

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("WorkFlowPage")]
        public WorkFlowPage WorkFlowPage
        {
            get { return _WorkFlowPage; }
            set { _WorkFlowPage = value; }
        }
       
        protected override void CreateChildControls()
        {            
            this.Controls.Clear();
            Control _control = GetTemplateControl(this.TemplateName);
            WorkFlowNav ctl = _control as WorkFlowNav;
            if (ctl == null) return;
            ctl.WFPage = this.WorkFlowPage;
            this.Controls.Add(_control);
        }
    }
}
