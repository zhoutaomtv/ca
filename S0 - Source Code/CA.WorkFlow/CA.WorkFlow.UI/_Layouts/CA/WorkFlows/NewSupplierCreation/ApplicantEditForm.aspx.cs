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
using CA.WorkFlow.UI.Code;
using CA.WorkFlow.UI;

namespace CA.WorkFlows.NewSupplierCreation
{
    public partial class ApplicantEditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          //  this.actions.Attributes.Add("onclick", "return CheckValue()");
            this.actions.OnClientClick += "return CheckValue(this.value);";

            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            Common.IsShowComments(trComments, tblCommentsList);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }
            WorkflowContext.Current.DataFields["UserName"] = DataForm1.Applicant.UserAccount;
            WorkflowContext.Current.DataFields["Supplier"] = DataForm1.Supplier;
            WorkflowContext.Current.DataFields["SubDivision"] = DataForm1.SubDivision;
            WorkflowContext.Current.DataFields["IsMondial"] = DataForm1.IsMondia;
            WorkflowContext.Current.DataFields["Status"] = DataForm1.Status;

            WorkflowContext.Current.DataFields["Flag"] = "Submit";

            WorkflowContext.Current.DataFields["RequestStatus"] = "In Progress";
        }
        void actions_ActionExecuted(object sender, EventArgs e)
        {
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //SPContext.Current.ListItem.Update();
            SPListItem item = SPContext.Current.ListItem;
            item["UserName"] = DataForm1.Applicant.UserAccount;
            item["Supplier"] = DataForm1.Supplier;
            item["SubDivision"] = DataForm1.SubDivision;
            item["IsMondial"] = DataForm1.IsMondia;
            item["Status"] = DataForm1.Status;

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
