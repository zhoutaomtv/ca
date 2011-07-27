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
using QuickFlow;
using QuickFlow.UI.Controls;
using CodeArt.SharePoint.CamlQuery;
using QuickFlow.WorkflowAdapters;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Utilities;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.NewSupplierCreation
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
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool flag = IsCanSubmit();
            if (!flag)
            {
                ScriptManager.RegisterStartupScript(this,this.GetType(),"msg", "alert('You do not belong to Buying .');",true);
                e.Cancel = true;
                return;
            }
            StartWorkflowButton btn = sender as StartWorkflowButton;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
                WorkflowContext.Current.DataFields["Flag"] = "Save";
                WorkflowContext.Current.DataFields["RequestStatus"] = "NonSubmit";
            }
            else
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Flag"] = "Submit";
                WorkflowContext.Current.DataFields["RequestStatus"] = "In Progress";
            }
            string strDepartment = DataForm1.Applicant.Department;
            //string strManager = @"ca\ztao";
            //string strDirector = @"ca\ztao";
            //string strHead = @"ca\ztao";
            //string strBBSTeam = @"ca\ztao";
            string strMondial = this.DataForm1.IsMondia;
            //组开始
            //List<string> lst = WorkFlowUtil.UserListInGroup("DivisionManagerGroup");
            string Head = WorkFlowUtil.GetEmployeeApproverInDept(DataForm1.Applicant, true, true).UserAccount;
            NameCollection ManagerUser = new NameCollection();
            NameCollection DirectorUser = new NameCollection();
            bool isHeadOne = false;
            bool isHeadTwo = false;
            if (!string.IsNullOrEmpty(Head))
            {
                isHeadOne = true;
                ManagerUser.Add(Head);
                Employee employee = WorkFlowUtil.GetEmployeeApproverInDept(WorkFlowUtil.GetEmployeeApproverInDept(DataForm1.Applicant, true, true), true, false);
                if (employee != null)
                {
                    string strhead = employee.UserAccount;
                    if (!string.IsNullOrEmpty(strhead))
                    {
                        isHeadTwo = true;
                        DirectorUser.Add(strhead);
                    }
                }
            }
            
            //lst = WorkFlowUtil.UserListInGroup("BuyingDirectorGroup");
            //DirectorUser.AddRange(lst.ToArray());
            //DirectorUser.Add(strDirector);
            NameCollection HeadUser = new NameCollection();
            List<string> lst = WorkFlowUtil.UserListInGroup("wf_CommercialHead");
            HeadUser.AddRange(lst.ToArray());
            //HeadUser.Add(strHeader);
            NameCollection BBSTeamUser = new NameCollection();
            lst = WorkFlowUtil.UserListInGroup("wf_BSSTeam");
            BBSTeamUser.AddRange(lst.ToArray());
            //BBSTeamUser.Add(strBBSTeam);
            //组结束
            WorkflowContext.Current.UpdateWorkflowVariable("Manager", ManagerUser);
            WorkflowContext.Current.UpdateWorkflowVariable("BuyDirector", DirectorUser);
            WorkflowContext.Current.UpdateWorkflowVariable("Header", HeadUser);
            WorkflowContext.Current.UpdateWorkflowVariable("BBSTeamAccount", BBSTeamUser);
            WorkflowContext.Current.UpdateWorkflowVariable("Mondial", strMondial);
            WorkflowContext.Current.UpdateWorkflowVariable("isHeadOne", isHeadOne);
            WorkflowContext.Current.UpdateWorkflowVariable("isHeadTwo", isHeadTwo);

            //修改TaskTitle
            WorkflowContext.Current.UpdateWorkflowVariable("ManagerTitle", DataForm1.Applicant.DisplayName + "'s New Trade Supplier Creation request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("DirectorTitle", DataForm1.Applicant.DisplayName + "'s New Trade Supplier Creation request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("HeaderTitle", DataForm1.Applicant.DisplayName + "'s New Trade Supplier Creation request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("BBSTeamTitle", DataForm1.Applicant.DisplayName + "'s New Trade Supplier Creation request needs confirm");
            WorkflowContext.Current.UpdateWorkflowVariable("TaskUpdateTitle", "Please complete New Trade Supplier Creation request for " + DataForm1.Applicant.DisplayName);
            //修改结束



            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["UserName"] = DataForm1.Applicant.UserAccount;
            WorkflowContext.Current.DataFields["Supplier"] = DataForm1.Supplier;
            WorkflowContext.Current.DataFields["SubDivision"] = DataForm1.SubDivision;
            WorkflowContext.Current.DataFields["IsMondial"] = DataForm1.IsMondia;
            //WorkflowContext.Current.DataFields["Upload"] = DataForm1.UploadFile;
            WorkflowContext.Current.DataFields["Status"] = DataForm1.Status;
        }

        private string CreateWorkFlowNumber()
        {
            return "NSC_" + WorkFlowUtil.CreateWorkFlowNumber("NewSupplierCreationWorkFlow").ToString("000000");
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            
        }

        private bool IsCanSubmit()
        {
            bool flag = false;
            if (DataForm1.Applicant.Department + "" == "")
            {
                return false;
            }
            CA.SharePoint.ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);

            foreach (var department in DataForm1.Applicant.Department.Split(';'))
            {
                //string department = string.Empty;
                //if (DataForm1.Applicant.Department.Contains(';'))
                //    department = DataForm1.Applicant.Department.Substring(0, DataForm1.Applicant.Department.IndexOf(';') + 1);
                //else
                //    department = DataForm1.Applicant.Department;

                if (!string.IsNullOrEmpty(department))
                {
                    SPListItemCollection SPColl = sps.Query(sps.GetList("Department"), new QueryField("Name", false).Equal(department), 1);
                    if (SPColl.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(SPColl[0]["DisplayName"].ToString()) && SPColl[0]["DisplayName"].ToString().ToLower() == "buying")
                        {
                            flag = true;
                            break;
                        }
                        if (string.IsNullOrEmpty(SPColl[0]["DisplayName"].ToString()) && SPColl[0]["Name"].ToString().ToLower() == "buying")
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }
    }
}
