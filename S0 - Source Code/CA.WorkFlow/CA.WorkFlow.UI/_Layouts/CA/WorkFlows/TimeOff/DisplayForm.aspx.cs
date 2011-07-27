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
using Microsoft.SharePoint;
using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint;
using CodeArt.SharePoint.CamlQuery;

namespace CA.WorkFlow.UI.TimeOff
{
    public partial class DisplayForm : CAWorkFlowPage
    {
        public bool WorkflowCompleted
        {
            get
            {
                return SPContext.Current.ListItem[DataForm.Constants.WorkflowName] + "" == "5";
            }
        }

        public bool HaveCancelWorkflow
        {
            get
            {
                try
                {
                    return SPContext.Current.ListItem[DataForm.Constants.CancelWorkflowName] != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool IsCurrentUser
        {
            get
            {
                return base.CurrentEmployee.PreferredName.Equals(this.CurrentListItem["Applicant"] + "", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public bool IsCancelCompleted
        {
            get
            {
                try
                {
                    return SPContext.Current.ListItem[DataForm.Constants.CancelWorkflowName] + "" == "5";
                }
                catch
                {
                    return false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                SPListItemCollection records = ListQuery.Select()
                             .From(SPContext.Current.Web.Lists[CAWorkFlowConstants.ListName.LeaveRecord.ToString()])
                             .Where(new QueryField("WorkflowID", false).Equal(SPContext.Current.ListItem["WorkFlowNumber"] + "") &&
                             new QueryField("LeaveType", false).Equal("Sick Leave 病假")
                             )
                             .GetItems();

                if (this.WorkflowCompleted && this.IsCurrentUser && !this.HaveCancelWorkflow && !this.IsCancelCompleted && records.Count == 0)
                {  
                    this.btnStartWorkflow.Visible = true;
                }
            }
            this.btnStartWorkflow.Click += new EventHandler(btnStartWorkflow_Click);
          
        }

        void btnStartWorkflow_Click(object sender, EventArgs e)
        {
            string strDepartment = DataForm1.Applicant.Department;   

            string strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/CancelApproveForm.aspx";
            string strNextTaskTitle = string.Format("{0}'s leave cancellation needs approval", SPContext.Current.Web.CurrentUser.Name);
         
            string deptHead = WorkFlowUtil.GetEmployeeApprover(this.CurrentEmployee).UserAccount;
            string strHRHeadAccount = UserProfileUtil.GetDepartmentManager("hr");           

            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);          
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadAccount", deptHead);
            WorkflowContext.Current.UpdateWorkflowVariable("HRHeadAccount", strHRHeadAccount);
            WorkflowContext.Current.DataFields["Status"] = "In Progress";

            NameCollection hrAccounts = WorkFlowUtil.GetUsersInGroup("wf_HR");
            WorkflowContext.Current.UpdateWorkflowVariable("hrAccounts", hrAccounts);
            bool IsSick = this.DataForm1.IsSickLeave;
            WorkflowContext.Current.UpdateWorkflowVariable("IsSick", IsSick);

            WorkflowContext.Current.StartWorkflow(DataForm.Constants.CancelWorkflowName);



            //ISharePointService sps = ServiceFactory.GetSharePointService(true);
            //SPList listRecord = sps.GetList(CAWorkFlowConstants.ListName.LeaveRecord.ToString());

            //foreach (SPListItem item in LeaveRecord.Items)
            //{
            //    item["Status"] = WorkflowContext.Current.DataFields["Status"];
            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //    break;
            //}
            //string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            //WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", strTimeOffNumber);
            //DataTable dtRecords = this.DataForm1.DataTableRecord;
            //string strApplicantName = this.DataForm1.ApplicantName;
            
            //SPListItem item1 = null;
            //foreach (DataRow dr in dtRecords.Rows)
            //{
            //    SPFieldUrlValue uv = new SPFieldUrlValue();
            //    uv.Url = SPContext.Current.Web.Url
            //      + "/_layouts/CA/WorkFlows/TimeOff/DisplayForm.aspx?List="
            //      + SPContext.Current.ListId.ToString()
            //      + "&ID="
            //      + SPContext.Current.ListItem.ID;
            //    uv.Description = strTimeOffNumber;


            //    item1 = listRecord.Items.Add();
            //    item1["Applicant"] = strApplicantName;
            //    item1["Department"] = strDepartment;
            //    item1["LeaveType"] = dr["LeaveType"];
            //    item1["LeaveDays"] = dr["LeaveDays"];
            //    item1["DateFrom"] = dr["DateFrom"];
            //    item1["DateTo"] = dr["DateTo"];
            //    item1["DateFromTime"] = dr["DateFromTime"];
            //    item1["DateToTime"] = dr["DateToTime"];
            //    item1["WorkFlowNumber"] = uv;
            //    item1["WorkflowID"] = strTimeOffNumber;
            //    item1["Status"] = WorkflowContext.Current.DataFields["Status"];
            //    item1.Web.AllowUnsafeUpdates = true;
            //    item1.Update();
            //}

            base.Back();
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.DataForm1.AttachmentVisible = this.DataForm1.IsSickLeave;
            base.OnPreRender(e);
        }
     
    }
}
