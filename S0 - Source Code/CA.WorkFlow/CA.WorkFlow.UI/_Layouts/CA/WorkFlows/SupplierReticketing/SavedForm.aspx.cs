using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CA.SharePoint;

using QuickFlow.Core;
using Microsoft.SharePoint;
using QuickFlow;

namespace CA.WorkFlow.UI.SupplierReticketing
{
    public partial class SavedForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            UpdateRecords();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }
            string msg = DataForm1.Validate();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }
            NameCollection buyingUsers = new NameCollection();
            buyingUsers.Add(DataForm1.BuyingUser);
            WorkflowContext.Current.UpdateWorkflowVariable("BuyingApproveUsers", buyingUsers);
            WorkflowContext.Current.DataFields["BuyingUser"] = DataForm1.BuyingUser;
            WorkflowContext.Current.DataFields["FileName"] = DataForm1.Submit();
            WorkflowContext.Current.DataFields["Status"] = "In Progress";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            item["BuyingUser"] = DataForm1.BuyingUser;
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
            UpdateRecords();
            base.Back();
        }

        private void UpdateRecords()
        {
            DataForm1.Update();

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.PODetails.ToString());
            SPListItem item = null;

            // remove old records for the workflow
            WorkFlowUtil.RemoveExistingRecord(list, "RequestID", DataForm1.WorkflowNumber);

            DataTable dtPODetails = this.DataForm1.dtPODetails;
            foreach (DataRow dr in dtPODetails.Rows)
            {
                item = list.Items.Add();
                item["RequestID"] = DataForm1.WorkflowNumber;
                item["PONumber"] = dr["PONumber"];
                item["Amount"] = dr["Amount"];
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
            }
        }
    }
}
