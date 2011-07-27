using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using System.Data;
using System.Configuration;
using QuickFlow.UI.Controls;
using System.Collections.Specialized;
using QuickFlow;

namespace CA.WorkFlow.UI.ChoppingApplication
{
    public partial class NewForm : CAWorkFlowPage
    {
       

        //protected override void AddPermisson()
        //{
        //    base.AddPermisson();
        //    this.PermissionSet.Add(@"cx\caix", PermissionType.Edit);
        //}

        protected void Page_Load(object sender, EventArgs e)
        { 
            this.StartWorkflowButton1.WorkflowName = this.StartWorkflowButton2.WorkflowName = DataForm.Constants.WorkflowName;
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            //base.UpdatePermissions();
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string strManagerAccount = this.DataForm1.DeptManagerAccount;

            StartWorkflowButton btnStart = sender as StartWorkflowButton;

            WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.DataForm1.WorkFlowNumber;

            if (string.Equals(btnStart.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
                string strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication/ApplicantEditForm.aspx";
                string strNextTaskTitle = "Please complete your chopping application";
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

                WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            }
            else
            {
                if (string.IsNullOrEmpty(strManagerAccount))
                {
                    e.Cancel = true;
                    this.labMessage.Text = "Responsible Functional Department and Manager is requied.";                   
                    return;
                }
                string strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication/ApproveForm.aspx";
                string strNextTaskTitle = string.Format("{0}'s chopping application needs approval", this.CurrentEmployee.DisplayName);
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }

            string LegalHead = UserProfileUtil.GetDepartmentManager("legal");
            string LegalCounsel = WorkFlowUtil.GetUserInGroup("wf_LegalCounsel");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartHeadAccount", this.DataForm1.DeptManagerAccount);
            WorkflowContext.Current.UpdateWorkflowVariable("LegalCounselAccount", LegalCounsel);
            WorkflowContext.Current.UpdateWorkflowVariable("LegalHeadAccount", LegalHead);
            WorkflowContext.Current.UpdateWorkflowVariable("CEOAccount", this.DataForm1.CEOAccount);
            if (string.IsNullOrEmpty(DataForm1.CFCOAccount) && !string.IsNullOrEmpty(DataForm1.CFCO2Account))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("CFCOAccount", this.DataForm1.CFCO2Account);
                WorkflowContext.Current.UpdateWorkflowVariable("ManagerAccount", this.DataForm1.CFCOAccount);
            }
            else
            {
                WorkflowContext.Current.UpdateWorkflowVariable("CFCOAccount", this.DataForm1.CFCOAccount);
                WorkflowContext.Current.UpdateWorkflowVariable("ManagerAccount", this.DataForm1.CFCO2Account);
            }
            WorkflowContext.Current.DataFields["Department"] = this.DataForm1.CurrentEmployee.Department;

            WorkflowContext.Current.DataFields["Projects"] = this.DataForm1.Projects;
            WorkflowContext.Current.DataFields["LegalCounsel"] = GetUserIDByAccount(LegalHead);
        }

        private int GetUserIDByAccount(string account)
        {
            SPUser user = SPContext.Current.Site.RootWeb.SiteUsers[account];
            return user.ID;
        }
    }
}
