namespace CA.WorkFlow.UI.InternalOrderMaintenance
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.SharePoint;
    using QuickFlow;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
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
            this.Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);
            this.btnSave.Click += new EventHandler(btnSave_Click);

            this.DataForm1.Department = WorkflowContext.Current.DataFields["Department"].AsString();
            this.DataForm1.OrderNumber = WorkflowContext.Current.DataFields["Order Number"].AsString();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate(new CancelEventArgs()))
            {
                return;
            }
            else
            {
                WorkflowContext.Current.SaveTask();
            }
            
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            if (!Validate(e))
            {
                return;
            }

            double lastValue = this.DataForm1.GetLastValue(WorkflowContext.Current.DataFields["Order Number"] as string);
            WorkflowContext.Current.DataFields["Last Value"] = lastValue;
            this.DataForm1.UpdateValues();

            WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private bool Validate(CancelEventArgs e)
        {
            bool flag = false;
            if (!this.DataForm1.Validate())
            {
                this.lblError.Text = this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : "Please fill in all the necessary fields.";
                e.Cancel = true;
                flag = false;
            }
            else
            {
                flag = true;
            }
            return flag;
        }

    }
}