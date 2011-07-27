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
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint.Utilities.Common;


namespace CA.WorkFlow.UI.ITHardwareOrSoftwareApplication
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
                string msg = DataForm1.Validate;
                if (!string.IsNullOrEmpty(msg))
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Flag"] = "Submit";
                WorkflowContext.Current.DataFields["Status"] = "In Progress";

                //string strDepartment = DataForm1.Applicant.Department;
                //string strManager = @"ca\ztao";
                //string strDirector = @"ca\ztao";
                //string strHeader = @"ca\ztao";
                //string strBBSTeam = @"ca\ztao";

                //组开始
                List<string> lst = WorkFlowUtil.UserListInGroup("wf_IT");
                if (lst.Count == 0)
                {
                    //Don
                    DisplayMessage("Unable to submit the application. There is no user in wf_IT group. Please contact IT for further help.");
                    e.Cancel = true;
                    return;
                }
                NameCollection ItMemberUser = new NameCollection();
                ItMemberUser.AddRange(lst.ToArray());
                //ItMemberUser.Add(strManager);

                //string department = string.Empty;
                //if (DataForm1.Department.Contains(';'))
                //    department = DataForm1.Department.Substring(0, DataForm1.Department.IndexOf(';') + 1);
                //else
                //    department = DataForm1.Department;

                Employee approver = WorkFlowUtil.GetEmployeeApprover(DataForm1.Applicant);
                if (approver == null)
                {
                    //Don
                    DisplayMessage("Unable to find an approver for the applicant. Please contact IT for further help.");
                    e.Cancel = true;
                    return;
                }

                NameCollection DepartHeadUser = new NameCollection();
                //lst = WorkFlowUtil.UserListInGroup("DepartmentHeadGroup");
                //DepartHeadUser.AddRange(lst.ToArray());
                DepartHeadUser.Add(approver.UserAccount);
                NameCollection ITHeadUser = new NameCollection();
                string strHRHeadAccount = UserProfileUtil.GetDepartmentManager("it");
                //lst = WorkFlowUtil.UserListInGroup("ITHead");
                //ITHeadUser.AddRange(lst.ToArray());
                ITHeadUser.Add(strHRHeadAccount);
                NameCollection CFOUser = new NameCollection();
                lst = WorkFlowUtil.UserListInGroup("wf_CFO");
                CFOUser.AddRange(lst.ToArray());
                //FOCOUser.Add(strBBSTeam);
                //组结束
                WorkflowContext.Current.UpdateWorkflowVariable("ITMemberAccounts", ItMemberUser);
                WorkflowContext.Current.UpdateWorkflowVariable("DepartHeaderAccounts", DepartHeadUser);
                WorkflowContext.Current.UpdateWorkflowVariable("ITHeaderAccounts", ITHeadUser);
                WorkflowContext.Current.UpdateWorkflowVariable("FOCOAccounts", CFOUser);
                WorkflowContext.Current.UpdateWorkflowVariable("IsFOCO", false);
            }

            //修改TaskTitle
            WorkflowContext.Current.UpdateWorkflowVariable("ITMemberTitle", DataForm1.Applicant.DisplayName + "'s IT request needs confirm");
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeaderTitle", DataForm1.Applicant.DisplayName + "'s IT request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("ITHeaderTitle", DataForm1.Applicant.DisplayName + "'s IT request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("FOCOTitle", DataForm1.Applicant.DisplayName + "'s IT request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("UpdateTaskTitle", "Please complete IT request for " + DataForm1.Applicant.DisplayName);
            //修改结束

            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["Name"] = DataForm1.Name;
            WorkflowContext.Current.DataFields["Department"] = DataForm1.Department;
            WorkflowContext.Current.DataFields["FOCOApprove"] = DataForm1.IsFOCO;
            WorkflowContext.Current.DataFields["ApplyDate"] = DataForm1.ApplyDate; 
            WorkflowContext.Current.DataFields["AppliedUser"] = this.DataForm1.Applicant.UserAccount;
            WorkflowContext.Current.DataFields["ReasonforRequest"] = this.DataForm1.ReasonforRequest;
        }

        private string CreateWorkFlowNumber()
        {
            return "ITR_" + WorkFlowUtil.CreateWorkFlowNumber("ITRequestWorkFlow").ToString("000000");
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            DataTable dtRecords = this.DataForm1.DataTableRecord;
            Repeater rept = (Repeater)this.DataForm1.FindControl("rptItRequest");
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ITRequestItems.ToString());
            SPListItem item = null;
            string strApplicantName = this.DataForm1.ApplicantName;
            string strDepartment = this.DataForm1.Department;


            foreach (DataRow dr in dtRecords.Rows)
            {
                item = list.Items.Add();
                item["HardwareOrSoftwareName"] = dr["HardwareOrSoftwareName"];
                item["Cost"] = dr["Cost"];
                item["WorkFlowNumber"] = this.WorkFlowNumber;
                item.Update();
            }
        }
    }
}
