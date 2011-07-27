using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using QuickFlow.Core;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.SupplierReticketing
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }
    }
}
