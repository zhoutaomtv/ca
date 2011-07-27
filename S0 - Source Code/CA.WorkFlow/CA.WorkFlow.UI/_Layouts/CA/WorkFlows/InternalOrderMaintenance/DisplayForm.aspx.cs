namespace CA.WorkFlow.UI.InternalOrderMaintenance
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    public partial class DisplayForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataForm1.OrderNumber = WorkflowContext.Current.DataFields["Order Number"].AsString();
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            this.DataForm1.Department = WorkflowContext.Current.DataFields["Department"].AsString();
        }
    }
}