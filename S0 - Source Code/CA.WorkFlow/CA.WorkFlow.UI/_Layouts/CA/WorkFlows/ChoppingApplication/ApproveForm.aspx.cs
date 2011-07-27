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
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI.ChoppingApplication
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataForm1.ControlMode = SPControlMode.Display;
            this.actions.ActionExecuting += new EventHandler<ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        void actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            string strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication/ApproveForm.aspx";
            string strNextTaskTitle = string.Format("{0}'s chopping application needs approval", new SPFieldLookupValue(SPContext.Current.ListItem["Created By"] + "").LookupValue);
         
            if (string.Equals(e.Action, "Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                strNextTaskUrl = @"_Layouts/CA/WorkFlows/ChoppingApplication/ApplicantEditForm.aspx";
                strNextTaskTitle = "Please modify your chopping application";
            }

            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

            if ((WorkflowContext.Current.Task.Step == DataForm.Constants.CEOApprove || 
                (WorkflowContext.Current.Task.Step==DataForm.Constants.LegalHeadApprove
                    && string.IsNullOrEmpty(this.DataForm1.CEOAccount) ))
                &&e.Action=="Approve")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }

            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;
        }   

    }
}
