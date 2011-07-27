namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint.WebControls;

    public partial class DisplayForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataForm1.CurrentStep = string.Empty;
            this.DataForm1.RecordType = WorkflowContext.Current.DataFields["Record Type"].AsString();
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
        }
    }
}