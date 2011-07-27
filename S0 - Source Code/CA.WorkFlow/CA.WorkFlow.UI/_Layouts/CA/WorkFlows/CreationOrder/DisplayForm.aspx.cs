namespace CA.WorkFlow.UI.CreationOrder
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    public partial class Display : CAWorkFlowPage
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
        }
    }
}