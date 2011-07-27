using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Web.UI.HtmlControls;
using QuickFlow.Core;
using System.Data;
using QuickFlow;
using QuickFlow.UI.Controls;
using CA.WorkFlow.UI.Code;
using CodeArt.SharePoint.CamlQuery;

namespace CA.WorkFlow.UI.ConstructionPurchasing
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
           // this.StartWorkflowButton1.Attributes.Add("onclick", "return CheckValue()");
            this.StartWorkflowButton1.OnClientClick = "return CheckValue('')";
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartWorkflowButton btn = sender as StartWorkflowButton;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
                WorkflowContext.Current.DataFields["Flag"] = "Save";
                WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            }
            else
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Flag"] = "Submit";
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }
            //string strDepartment = DataForm1.Applicant.Department;
            //string strConstructionHeader = @"ca\ztao";
            //string strConstruction = @"ca\ztao";
            //string strDepartmentHeader = @"ca\ztao";
            //string strStoreOperationTeam = @"ca\ztao";
            //string strCFO = @"ca\ztao";

            //组开始
            string strConstructionHeadAccount = UserProfileUtil.GetDepartmentManager("Construction");
            //List<string> lst = WorkFlowUtil.UserListInGroup("ConstructionHeadGroup");
            NameCollection ConstructionHead = new NameCollection();
            //ConstructionHead.AddRange(lst.ToArray());
            ConstructionHead.Add(strConstructionHeadAccount);
            //ConstructionHead.Add(@"ca\caix");

            NameCollection ConstructionUser = new NameCollection();
            List<string> lst = WorkFlowUtil.UserListInGroup("wf_Construction");
            ConstructionUser.AddRange(lst.ToArray());
            //ConstructionUser.Add(strConstruction);

            string department = string.Empty;
            if (DataForm1.Applicant.Department.Contains(';'))
                department = DataForm1.Applicant.Department.Substring(0, DataForm1.Applicant.Department.IndexOf(';')+1);
            else
                department = DataForm1.Applicant.Department;

            string departmentManager = UserProfileUtil.GetDepartmentManager(department);
            if (string.IsNullOrEmpty(departmentManager))
            {
                //Don
                DisplayMessage("Unable to submit the application. There is no department manager defined. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            NameCollection DepartmentHead = new NameCollection();
            //lst = WorkFlowUtil.UserListInGroup("DepartmentHeadGroup");
            //DepartmentHead.AddRange(lst.ToArray());
            //DepartmentHead.Add(strDepartmentHeader);
            DepartmentHead.Add(departmentManager);

            NameCollection StoreOperationTeamUser = new NameCollection();
            try
            {
                //lst = WorkFlowUtil.UserListInGroup("wf_StoreOperationTeam");
                //StoreOperationTeamUser.AddRange(lst.ToArray());
                //SPListItemCollection coll = GetSPColl("Stores", "Cost Center", DataForm1.CostCenter, 1);
                //StoreOperationTeamUser.Add(new SPFieldLookupValue(coll[0]["Owner"] + "").LookupValue);
                string strStoreOperations = UserProfileUtil.GetDepartmentManager("Store Operations");
                StoreOperationTeamUser.Add(strStoreOperations);
            }
            catch { }

            
            //StoreOperationTeamUser.Add(strStoreOperationTeam);

            NameCollection CFOUser = new NameCollection();
            lst = WorkFlowUtil.UserListInGroup("wf_CFO");
            CFOUser.AddRange(lst.ToArray());
            //CFOUser.Add(strCFO);
            //组结束
            WorkflowContext.Current.UpdateWorkflowVariable("ConstructionHeadAccount", ConstructionHead);
            WorkflowContext.Current.UpdateWorkflowVariable("ConstructionAccount", ConstructionUser);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadAccount", DepartmentHead);
            WorkflowContext.Current.UpdateWorkflowVariable("StoreOperationTeamAccount", StoreOperationTeamUser);
            WorkflowContext.Current.UpdateWorkflowVariable("CFOAccount", CFOUser);
            WorkflowContext.Current.UpdateWorkflowVariable("isSameUser", Common.IsSameUser(DepartmentHead, StoreOperationTeamUser, CFOUser));
            WorkflowContext.Current.UpdateWorkflowVariable("IsFixedAsset", DataForm1.RequestType == "Capex" ? "FixedAsset" : DataForm1.RequestType);

            //修改TaskTitle
            WorkflowContext.Current.UpdateWorkflowVariable("ConstructionHeadTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("ConstructionTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs supplies other information");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("StoreOperationTeamTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("CFOTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("UpdateTaskTitle", "Please complete construction purchasing request for " + DataForm1.Applicant.DisplayName);
            WorkflowContext.Current.UpdateWorkflowVariable("PlacesTheOrderTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs places the order");
            WorkflowContext.Current.UpdateWorkflowVariable("OrderHandoverTitle", DataForm1.Applicant.DisplayName + "'s construction purchasing request needs order handover");
            //修改结束

            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.WorkFlowNumber;
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
        }

        private string CreateWorkFlowNumber()
        {
            return "CPR_" + WorkFlowUtil.CreateWorkFlowNumber("ConstructionPurchasingWorkflow").ToString("000000");
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            DataTable dtRecords = this.DataForm1.DataTableRecord;
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionItems.ToString());
            SPListItem item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                item = list.Items.Add();
                item["Discription"] = dr["Discription"];
                item["ItemCode"] = dr["ItemCode"];
                item["Quantity"] = dr["Quantity"];
                item["Unit"] = dr["Unit"];
                item["UnitPrice"] = dr["UnitPrice"];
                item["TotalPrice"] = dr["TotalPrice"];
                item["Remark"] = dr["Remark"];
                item["WorkFlowNumber"] = this.WorkFlowNumber;
                item.Update();
            }
        }
    }
}
