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

    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;

            this.DataForm1.CurrentStatus = string.Empty;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            var btn = sender as StartWorkflowButton;
            var taskTitle = CurrentEmployee.DisplayName + "'s Internal Order Creation ";
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
                WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.Pending;
            }
            else
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

                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;
                
            }
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
            WorkflowContext.Current.DataFields["Department"] = CurrentEmployee.Department;

            #region Set title for workflow
            //Modify task title
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskTitle", "please complete internal order creation");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskTitle", taskTitle + "needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskTitle", taskTitle + "needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskTitle", taskTitle + "needs confirm");
            #endregion

            #region Set page URL for workflow
            //Set page url
            var editURL = "/_Layouts/CA/WorkFlows/CreationOrder/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/CreationOrder/ApproveForm.aspx";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskFormURL", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskFormURL", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskFormURL", approveURL);
            #endregion
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private string CreateWorkFlowNumber()
        {
            return "CO_" + WorkFlowUtil.CreateWorkFlowNumber("CreationOrder").ToString("000000");
        }
    }
}