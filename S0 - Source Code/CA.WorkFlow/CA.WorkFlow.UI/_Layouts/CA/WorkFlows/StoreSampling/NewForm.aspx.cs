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
using CA.WorkFlow.UI.Code;
using CodeArt.SharePoint.CamlQuery;

using QuickFlow;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.StoreSampling
{
    public partial class NewForm : CAWorkFlowPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton2_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton2_Executed);
        }

        //提交
        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg = DataForm1.Validate();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            SaveFormToWf();
            WorkflowContext.Current.DataFields["Status"] = "In Progress";
            WorkflowContext.Current.UpdateWorkflowVariable("IsSubmit", "Yes");
            //string strNextTaskUrl = @"_Layouts/CA/WorkFlows/StoreSampling/EditForm.aspx";
            //string strNextTaskTitle = string.Format("{0}'s leave application need approve", this.CurrentEmployee.DisplayName);
            //WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
            //WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            //base.Back();
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        //保存
        void StartWorkflowButton2_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            SaveFormToWf();
            WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            WorkflowContext.Current.UpdateWorkflowVariable("IsSubmit", "No");
            
        }

        void StartWorkflowButton2_Executed(object sender, EventArgs e)
        {
            //base.Back();
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }


        void SaveFormToWf()
        {
            WorkflowContext curContext = WorkflowContext.Current;
            string passTo = DataForm1.PickedBy;
            //DateTime now = DateTime.Now;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string taskTitle = SPContext.Current.Web.CurrentUser.Name + "'s Store Sampling";

            fields["Store Number"] = ((DropDownList)DataForm1.FindControl("ddlStoreNumber")).SelectedValue; //((TextBox)DataForm1.FindControl("txtStoreNumber")).Text;
            fields["Cost Center"] = string.Empty; 
 
            fields["Issued to"] = ((DropDownList)DataForm1.FindControl("ddlIssuedTo")).SelectedValue;
            fields["Actual Quantity"] = ((TextBox)DataForm1.FindControl("txtActualQuantity")).Text;
            if (!string.IsNullOrEmpty(passTo))
            {
                fields["Picked by"] = EnsureUser(passTo);
            }
            fields["Picked Time"] = ((CADateTimeControl)DataForm1.FindControl("CADateTime1")).SelectedDate.ToShortDateString();
            
            DataForm1.WorkflowNumber = CreateWorkflowNumber();
            fields["WorkflowNumber"] = DataForm1.WorkflowNumber;
            fields["FileName"] = DataForm1.Submit();

            curContext.UpdateWorkflowVariable("StoreAdminSubmitTitle", "Please complete store sampling");
            curContext.UpdateWorkflowVariable("BuyerApproveTitle", taskTitle + " needs confirm");
            curContext.UpdateWorkflowVariable("StoreManagerApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("AreaManagerApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("BSSHeadGroupApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("BSSTeamTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("FinanceGroupConfirmTitle", taskTitle + " needs confirm");

            ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);
            SPList stores = sps.GetList("Stores");
            QueryField field = new QueryField("Store Number", false);
            SPListItemCollection coll = sps.Query(stores, field.Equal(fields["Store Number"] + ""), 1);
            if (coll.Count > 0)
            {
                curContext.UpdateWorkflowVariable("StoreManager", new SPFieldLookupValue(coll[0]["Manager"] + "").LookupValue);
                curContext.UpdateWorkflowVariable("AreaManagerApproveUser", new SPFieldLookupValue(coll[0]["AreaManager"]+"").LookupValue);
            }
            else
            {
                return;
            }
            curContext.UpdateWorkflowVariable("Buyer", passTo);
            curContext.UpdateWorkflowVariable("BSSHeadGroup", "wf_BSSHead");

            NameCollection BBSTeamUser = new NameCollection();
            List<string> lst = WorkFlowUtil.UserListInGroup("wf_BSSTeam");
            BBSTeamUser.AddRange(lst.ToArray());
            curContext.UpdateWorkflowVariable("BSSTeamAccount", BBSTeamUser);
            curContext.UpdateWorkflowVariable("FinanceGroup", "wf_Finance_SS");
        }

        private string CreateWorkflowNumber()
        {
            return "SS_" + ((DropDownList)DataForm1.FindControl("ddlStoreNumber")).SelectedValue + "_" + WorkFlowUtil.CreateWorkFlowNumber("StoreSampling").ToString("000000");
        }

        public static SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            }
             );

            return user;
        }

      
    }
}
