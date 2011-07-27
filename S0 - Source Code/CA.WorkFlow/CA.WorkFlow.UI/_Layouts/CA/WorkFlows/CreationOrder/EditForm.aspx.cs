namespace CA.WorkFlow.UI.CreationOrder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.SharePoint;
    using QuickFlow;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using CA.SharePoint;

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
        }

        void btnSave_Click(object sender, EventArgs e)
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

            #region Set users for workflow
            //Modify task users
            var departmentManagerTaskUsers = new NameCollection();
            var manager = UserProfileUtil.GetDepartmentManager(CurrentEmployee.Department);
            if (manager.IsNullOrWhitespace())
            {
                DisplayMessage("The department manager is not set in the system.");
                e.Cancel = true;
                return;
            }
            departmentManagerTaskUsers.Add(manager);

            var deleman = WorkFlowUtil.GetDeleman(manager, "116");
            if (deleman != null)
            {
                departmentManagerTaskUsers.Add(deleman);
            }
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskUsers", departmentManagerTaskUsers);
            #endregion

            WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}