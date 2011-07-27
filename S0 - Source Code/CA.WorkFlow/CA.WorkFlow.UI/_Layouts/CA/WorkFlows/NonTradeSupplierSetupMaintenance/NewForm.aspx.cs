namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.SharePoint;
    using QuickFlow;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using CA.SharePoint;
    using SharePoint.Utilities.Common;

    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;

            this.dfSelectVendor.Department = CurrentEmployee.Department;
            this.dfSelectVendor.ApplicantAccount = CurrentEmployee.UserAccount;
            this.DataForm1.DepartmentVal = CurrentEmployee.Department;
            this.DataForm1.ApplicantAccount = CurrentEmployee.UserAccount;

            this.StartWorkflowButton2.OnClientClick = "return dispatchAction(this);";
        }

        //Save or Submit
        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            string taskTitle = CurrentEmployee.DisplayName + "'s Non-Trade Supplier Setup & Maintenance ";

            //Check which button has been clicked
            var btn = sender as StartWorkflowButton;
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

                this.DataForm1.UpdateValues();
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.InProgress;

                //Update varivable.(Payment term less than 30 days required CFO's special approval)
                WorkflowContext.Current.UpdateWorkflowVariable("PaymentTerm", this.DataForm1.PaymentTerm);
            }
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = CreateWorkflowNumber();
            WorkflowContext.Current.DataFields["Applicant"] = CurrentEmployee.DisplayName + "(" + CurrentEmployee.UserAccount + ")";
            WorkflowContext.Current.DataFields["DepartmentVal"] = CurrentEmployee.Department;

            #region Set title for workflow
            //Modify task title
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskTitle", "please complete Supplier Setup & Maintenance");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskTitle", taskTitle + "needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("MDMTaskTitle", taskTitle + "needs confirm");
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskTitle", taskTitle + "needs approval");
            #endregion

            #region Set users for workflow
            //Modify task users
            var DepartmentHeadTaskUsers = new NameCollection();
            var MDMTaskUsers = new NameCollection();
            var CFOTaskUsers = new NameCollection();

            string department = CurrentEmployee.Department;
            DepartmentHeadTaskUsers.Add(UserProfileUtil.GetDepartmentManager(department));

            List<string> lst = WorkFlowUtil.UserListInGroup("wf_CFO");
            CFOTaskUsers.AddRange(lst.ToArray());

            lst = WorkFlowUtil.UserListInGroup("wf_Finance_MDM");
            MDMTaskUsers.AddRange(lst.ToArray());

            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskUsers", DepartmentHeadTaskUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("MDMTaskUsers", MDMTaskUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskUsers", CFOTaskUsers);
            #endregion

            #region Set page URL for workflow
            //Set page url
            var editURL = "/_Layouts/CA/WorkFlows/NonTradeSupplierSetupMaintenance/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/NonTradeSupplierSetupMaintenance/ApproveForm.aspx";
            WorkflowContext.Current.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskFormURL", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("MDMTaskFormURL", approveURL);
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskFormURL", approveURL);
            #endregion
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private static string CreateWorkflowNumber()
        {
            return "NTV_" + WorkFlowUtil.CreateWorkFlowNumber("NonTradeSupplierSetupMaintenance").ToString("000000");
        }

    }
}