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
using CodeArt.SharePoint.CamlQuery;
using CA.WorkFlow.UI;
using CA.WorkFlow.UI.Code;

namespace CA.WorkFlows.ConstructionPurchasing
{
    public partial class ApplicantEditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // this.actions.Attributes.Add("onclick", "return CheckValue()");
            this.actions.OnClientClick += "return CheckValue(this.value);";
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted +=new EventHandler<EventArgs>(actions_ActionExecuted);
            Common.IsShowComments(trComments, tblCommentsList);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
   
            if(e.Action.Equals("End",StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"]="Cancelled";
                return;

            }
            WorkflowContext.Current.DataFields["ApplyDept"] = DataForm1.Applicant.Department;
            WorkflowContext.Current.DataFields["ApplyDate"] = DataForm1.ApplyDate;
            WorkflowContext.Current.DataFields["ApplyUser"] = this.DataForm1.Applicant.UserAccount;
            WorkflowContext.Current.DataFields["RequestType"] = this.DataForm1.RequestType;
            WorkflowContext.Current.DataFields["BudgetApproved"] = this.DataForm1.BudgetApproved;
            WorkflowContext.Current.DataFields["CurrencyType"] = this.DataForm1.CurrencyType;
            WorkflowContext.Current.DataFields["CostCenter"] = this.DataForm1.CostCenter;
            WorkflowContext.Current.DataFields["BudgetValue"] = this.DataForm1.BudgetValue;
            WorkflowContext.Current.DataFields["ProduceandDeliveryDate"] = this.DataForm1.ProduceandDeliveryDate;

            WorkflowContext.Current.DataFields["Installation"] = this.DataForm1.Installation;
            WorkflowContext.Current.DataFields["Freight"] = this.DataForm1.Freight;
            WorkflowContext.Current.DataFields["Packaging"] = this.DataForm1.Packaging;

            WorkflowContext.Current.DataFields["Status"] = "In Progress";
        }
        void actions_ActionExecuted(object sender, EventArgs e)
        {
            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionItems.ToString());

            //frist delete the items
            string WorkflowNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", WorkflowNumber);

            SPListItem item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                item = listRecord.Items.Add();
                item["Discription"] = dr["Discription"];
                item["ItemCode"] = dr["ItemCode"];
                item["Quantity"] = dr["Quantity"];
                item["Unit"] = dr["Unit"];
                item["UnitPrice"] = dr["UnitPrice"];
                item["TotalPrice"] = dr["TotalPrice"];
                item["Remark"] = dr["Remark"];
                item["WorkFlowNumber"] = WorkflowNumber;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            SPListItem item = SPContext.Current.ListItem;
            item["ApplyDept"] = DataForm1.Applicant.Department;
            item["ApplyDate"] = DataForm1.ApplyDate;
            item["ApplyUser"] = this.DataForm1.Applicant.UserAccount;
            item["RequestType"] = this.DataForm1.RequestType;
            item["BudgetApproved"] = this.DataForm1.BudgetApproved;
            item["CurrencyType"] = this.DataForm1.CurrencyType;
            item["CostCenter"] = this.DataForm1.CostCenter;
            item["BudgetValue"] = this.DataForm1.BudgetValue;
            item["ProduceandDeliveryDate"] = this.DataForm1.ProduceandDeliveryDate;
            item["Installation"] = this.DataForm1.Installation;
            item["Freight"] = this.DataForm1.Freight;
            item["Packaging"] = this.DataForm1.Packaging;
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

            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionItems.ToString());

            //frist delete the items
            string WorkFlowNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", WorkFlowNumber);

            item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                item = listRecord.Items.Add();
                item["Discription"] = dr["Discription"];
                item["ItemCode"] = dr["ItemCode"];
                item["Quantity"] = dr["Quantity"];
                item["Unit"] = dr["Unit"];
                item["UnitPrice"] = dr["UnitPrice"];
                item["TotalPrice"] = dr["TotalPrice"];
                item["Remark"] = dr["Remark"];
                item["WorkFlowNumber"] = WorkFlowNumber;
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
            base.Back();
        }
    }
}
