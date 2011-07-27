using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CA.SharePoint;
using Microsoft.SharePoint;

using QuickFlow.Core;

using QuickFlow.UI.Controls;
using QuickFlow;
using CA.SharePoint.Utilities.Common;



namespace CA.WorkFlow.UI.NewStoreBudgetApplication
{
    public partial class NewForm : CAWorkFlowPage
    {
        private string _WorkFlowNumber;

        public string WorkFlowNumber
        {
            get { return _WorkFlowNumber; }
            set { _WorkFlowNumber = value; }
        }

        private string CreateWorkFlowNumber()
        {
            return "SBA_" + WorkFlowUtil.CreateWorkFlowNumber("NewStoreBudgetApplication").ToString("000000");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }
        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartWorkflowButton btnStart = sender as StartWorkflowButton;

            if (string.Equals(btnStart.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
            }
            else
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);

                string msg = DataForm1.validateGeneralInfo();
                if (!string.IsNullOrEmpty(msg))
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }

            //long Intamount = Convert.ToInt64(this.DataForm1.amount);

            float amount = string.IsNullOrEmpty(DataForm1.amount) ? 0 : Convert.ToSingle(DataForm1.amount);


            //string deptHead = WorkFlowUtil.GetEmployeeApprover(DataForm1.Applicant).UserAccount;
            //WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHead", deptHead);


            string strDeptNamemanger = "Construction";
            string strDeptNamemangerName = UserProfileUtil.GetDepartmentManager(strDeptNamemanger);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHead", strDeptNamemangerName);


            string CFOName = WorkFlowUtil.GetUserInGroup("wf_CFO");

            WorkflowContext.Current.UpdateWorkflowVariable("CFOApprovalUser", CFOName);

            //WorkflowContext.Current.UpdateWorkflowVariable("Amount", Intamount);
            WorkflowContext.Current.UpdateWorkflowVariable("Amount", amount);

            string CEOName = WorkFlowUtil.GetUserInGroup("wf_CEO");

            WorkflowContext.Current.UpdateWorkflowVariable("CEOApprovalUser", CEOName);

            List<string> strGroupUser = WorkFlowUtil.UserListInGroup("wf_Finance_BA");
            if (strGroupUser.Count == 0)
            {
                //Don
                DisplayMessage("Unable to submit the application. There is no user in wf_Finance_BA group. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            NameCollection GroupUsers = new NameCollection();
            GroupUsers.AddRange(strGroupUser.ToArray());
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceTaskUsers", GroupUsers);

            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskTitle", DataForm1.Applicant.DisplayName + "'s New Store Construction Budget request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CFOApprovalTaskTitle", DataForm1.Applicant.DisplayName + "'s New Store Construction Budget request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CEOApprovalTaskTitle", DataForm1.Applicant.DisplayName + "'s New Store Construction Budget request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceUserTaskTitle", "Please complete New Store Construction Budget request for " + DataForm1.Applicant.DisplayName);

            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkflowNumber"] = this.WorkFlowNumber;
            DataForm1.WorkflowNumber = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["FileName"] = DataForm1.Submit();

        }
        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {

        }

    }
}
