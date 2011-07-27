using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;



namespace CA.SharePoint.WebControls
{  
    public partial class WorkFlowNav : System.Web.UI.UserControl
    {

        private WorkFlowPage _WFPage = WorkFlowPage.HomePage;
        public WorkFlowPage WFPage
        {
            get
            {
                return this._WFPage;
            }
            set
            {
                this._WFPage = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.WFPage == WorkFlowPage.WorkFlowPage)
            {
                this.divWrokFlowNav.Style.Remove("overflow-x");
                this.divWrokFlowNav.Style.Remove("overflow-y");
                this.divWrokFlowNav.Style.Remove("height");
            }
        }
    }
}