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
    using CA.SharePoint;

    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
            this.DataForm1.Department = CurrentEmployee.Department;
        }

        //Save or Submit
        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {            
            string taskTitle = CurrentEmployee.DisplayName + "'s Internal Order Maintenance ";

            //Check which button has been clicked
            var btn = sender as StartWorkflowButton;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!Validate(e))
                {
                    return;
                }

                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
                WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.Pending;
            }
            else
            {
                if (!Validate(e))
                {
                    return;
                }

                double lastValue = this.DataForm1.GetLastValue(WorkflowContext.Current.DataFields["Order Number"] as string);
                WorkflowContext.Current.DataFields["Last Value"] = lastValue;
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;
            }
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = CreateWorkflowNumber();
            WorkflowContext.Current.DataFields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
            WorkflowContext.Current.DataFields["Department"] = CurrentEmployee.Department;

            #region Set title for workflow
            //Modify task title
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskTitle", "please complete internal order maintenance");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskTitle", taskTitle + "needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskTitle", taskTitle + "needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskTitle", taskTitle + "needs confirm");
            #endregion

            #region Set users for workflow
            //Modify task users
            var DepartmentManagerTaskUsers = new NameCollection();
            var FinanceAnalystTaskUsers = new NameCollection();
            var CFOTaskUsers = new NameCollection();

            DepartmentManagerTaskUsers.Add(UserProfileUtil.GetDepartmentManager(CurrentEmployee.Department));

            List<string> lst = WorkFlowUtil.UserListInGroup("wf_CFO");            
            CFOTaskUsers.AddRange(lst.ToArray());

            lst = WorkFlowUtil.UserListInGroup("wf_FinanceAnalyst_IO");
            FinanceAnalystTaskUsers.AddRange(lst.ToArray());
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskUsers", FinanceAnalystTaskUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskUsers", CFOTaskUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskUsers", DepartmentManagerTaskUsers);
            #endregion

            #region Set page URL for workflow
            //Set page url
            var editURL = "/_Layouts/CA/WorkFlows/InternalOrderMaintenance/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/InternalOrderMaintenance/ApproveForm.aspx";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentManagerTaskFormURL", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskFormURL", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskFormURL", approveURL);
            #endregion
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

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private static string CreateWorkflowNumber()
        {
            return "IOM_" + WorkFlowUtil.CreateWorkFlowNumber("InternalOrderMaintenance").ToString("000000");
        }

    }
}