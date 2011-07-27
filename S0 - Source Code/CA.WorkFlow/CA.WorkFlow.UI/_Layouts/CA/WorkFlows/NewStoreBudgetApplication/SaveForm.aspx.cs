using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CA.SharePoint;

using QuickFlow.Core;
using Microsoft.SharePoint;
using QuickFlow;


namespace CA.WorkFlow.UI.NewStoreBudgetApplication
{
    public partial class SaveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value); ";
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
        }
        void actions_ActionExecuted(object sender, EventArgs e)
        {
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }
            string msg = DataForm1.validateGeneralInfo();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }
            WorkflowContext.Current.DataFields["FileName"] = DataForm1.Submit();
            WorkflowContext.Current.DataFields["Status"] = "In Progress";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            item["FileName"] = DataForm1.Submit();
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        item.Web.AllowUnsafeUpdates = true;
                        item.Update();
                        item.Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occured while updating the items");
            }

            //item.Web.AllowUnsafeUpdates = true;
            //item.Update(); 
            base.Back();
        }
    }
}
