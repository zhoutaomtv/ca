namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using System.ComponentModel;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;

            this.dfSelectVendor.Department = CurrentEmployee.Department;
            this.dfSelectVendor.ApplicantAccount = CurrentEmployee.UserAccount;
            this.DataForm1.DepartmentVal = CurrentEmployee.Department;
            this.DataForm1.ApplicantAccount = CurrentEmployee.UserAccount;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WorkflowContext.Current.SaveTask();
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            if (!this.DataForm1.Validate())
            {
                DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : "Please fill in all the necessary fields.");
                e.Cancel = true;
                return;
            }

            this.DataForm1.UpdateValues();

            WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;

            //Update varivable.(Payment term less than 30 days required CFO's special approval)
            WorkflowContext.Current.UpdateWorkflowVariable("PaymentTerm", this.DataForm1.PaymentTerm);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}