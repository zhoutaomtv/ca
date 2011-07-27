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

namespace CA.WorkFlow.UI.SupplierReinspection
{
    public partial class UpdateForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(WorkflowContext.Current.DataFields["Comments"] + ""))
                    ctfComments.Value = WorkflowContext.Current.DataFields["Comments"] + "";
            }
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);

            DataForm1.NeedUpdateMode = false;
            DataForm1.ShowUpdateTable();
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            UpdateRecords();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            string msg = DataForm1.Validate();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }
            if (!string.IsNullOrEmpty(ctfComments.Value.ToString()))
            {
                WorkflowContext.Current.DataFields["Comments"] = string.Empty;
            }
            WorkflowContext.Current.DataFields["Status"] = "Completed";

            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            if (!string.IsNullOrEmpty(ctfComments.Value.ToString()))
            {
                item["Comments"] = ctfComments.Value.ToString();
            }
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


            //string msg = DataForm1.Validate();
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    DisplayMessage(msg);
            //    return;
            //}

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
                item["ProcessDate"] = dr["ProcessDate"];
                item["InvoiceNumber"] = dr["InvoiceNumber"];
                item["DocumentNumber"] = dr["DocumentNumber"];
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
