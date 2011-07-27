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
    public partial class ApplicantEditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";
            this.actions.ActionExecuting += new EventHandler<ActionEventArgs>(actions_ActionExecuting);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            base.Back();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            SPContext.Current.ListItem["Projects"] = this.DataForm1.Projects;
            SPContext.Current.ListItem.Update();
            base.Back();
        }

        void actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }
            string strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication/ApproveForm.aspx";
            string strNextTaskTitle = string.Format("{0}'s chopping application needs approve", new SPFieldLookupValue(SPContext.Current.ListItem["Created By"] + "").LookupValue);
        
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

            string LegalHead = UserProfileUtil.GetDepartmentManager("legal");
            string LegalCounsel = WorkFlowUtil.GetUserInGroup("wf_LegalCounsel");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartHeadAccount", this.DataForm1.DeptManagerAccount);
            WorkflowContext.Current.UpdateWorkflowVariable("LegalCounselAccount", LegalCounsel);
            WorkflowContext.Current.UpdateWorkflowVariable("LegalHeadAccount", LegalHead);
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
            WorkflowContext.Current.UpdateWorkflowVariable("ManagerAccount", this.DataForm1.CFCO2Account);

            WorkflowContext.Current.DataFields["Projects"] = this.DataForm1.Projects;
            WorkflowContext.Current.DataFields["Status"] = "In Progress";
        }     

    }
}
