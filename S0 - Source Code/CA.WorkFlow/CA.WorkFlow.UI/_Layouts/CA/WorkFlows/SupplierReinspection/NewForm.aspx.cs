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
using QuickFlow.UI.Controls;
using QuickFlow;
using CA.SharePoint.Utilities.Common;


namespace CA.WorkFlow.UI.SupplierReinspection
{
    public partial class NewForm : CAWorkFlowPage
    {

        private string _WorkFlowNumber;

        public string WorkFlowNumber
        {
            get { return _WorkFlowNumber; }
            set { _WorkFlowNumber = value; }
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
                DataForm1.Update();
                WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            }
            else
            {
                string msg = DataForm1.Validate();
                if (!string.IsNullOrEmpty(msg))
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }

            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHead", WorkFlowUtil.GetUserInGroup("wf_BSSHead"));
            List<string> strGroupUser = WorkFlowUtil.UserListInGroup("wf_Finance_SR");
            if (strGroupUser.Count == 0)
            {
                //Don
                DisplayMessage("Unable to submit the application. There is no user in wf_Finance_SR. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            NameCollection GroupUsers = new NameCollection();
            GroupUsers.AddRange(strGroupUser.ToArray());
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceTaskUsers", GroupUsers);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskTitle", DataForm1.Applicant.DisplayName + "'s re-inspection request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FinanceUserTaskTitle", "Please complete re-inspection request for " + DataForm1.Applicant.DisplayName);
            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkflowNumber"] = this.WorkFlowNumber;
            DataForm1.WorkflowNumber = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["FileName"] = DataForm1.Submit();
        }

        private string CreateWorkFlowNumber()
        {
            return "SRI_" + WorkFlowUtil.CreateWorkFlowNumber("SupplierReinspection").ToString("000000");
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            DataForm1.Update();
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.PODetails.ToString());
            SPListItem item = null;

            DataTable dtPODetails = this.DataForm1.dtPODetails;
            foreach (DataRow dr in dtPODetails.Rows)
            {
                item = list.Items.Add();
                item["RequestID"] = this.WorkFlowNumber;
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
