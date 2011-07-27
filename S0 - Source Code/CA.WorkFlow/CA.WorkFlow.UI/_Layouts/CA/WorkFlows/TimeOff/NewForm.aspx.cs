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
using System.Configuration;
using QuickFlow.UI.Controls;
using System.Collections.Specialized;
using QuickFlow;
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint.Utilities;
using CA.SharePoint.Utilities.Common;
using System.Web.Services;

namespace CA.WorkFlow.UI.TimeOff
{
    public partial class NewForm : CAWorkFlowPage
    {
        private string _WorkFlowNumber;

        public string WorkFlowNumber
        {
            get { return _WorkFlowNumber; }
            set { _WorkFlowNumber = value; }
        }

        protected override void AddPermisson()
        {
            base.AddPermisson();
          //  this.PermissionSet.Add(@"cx\caix", PermissionType.Edit);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.WorkflowName = this.StartWorkflowButton2.WorkflowName = DataForm.Constants.WorkflowName;
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = SPContext.Current.Web.Lists[CAWorkFlowConstants.ListName.LeaveRecord.ToString()];
            SPListItem item = null;
            string strApplicantName=this.DataForm1.ApplicantName;
            string strDepartment=this.DataForm1.Applicant.Department;

            StartWorkflowButton btnStart = sender as StartWorkflowButton;
            SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());
            QueryField field = new QueryField("Employee");
            QueryField field2 = new QueryField("Year");

            

            foreach (DataRow dr in dtRecords.Rows)
            {
                #region add leave record
                item = list.Items.Add();             
                item["Applicant"] = strApplicantName;
                item["EmployeeLoginName"] = this.DataForm1.Applicant.UserAccount;
                item["EmployeeID"] = this.DataForm1.Applicant.EmployeeID;
                item["EmployeeDisplayName"] = this.DataForm1.Applicant.DisplayName;
                item["Department"] = strDepartment;
                item["LeaveType"] = dr["LeaveType"];
                item["LeaveDays"] = dr["LeaveDays"];
                if (!string.IsNullOrEmpty(dr["DateFrom"] + ""))
                {
                    item["DateFrom"] = dr["DateFrom"];
                }
                if (!string.IsNullOrEmpty(dr["DateTo"] + ""))
                {
                    item["DateTo"] = dr["DateTo"];
                }
             
                item["DateFromTime"] = dr["DateFromTime"];
                item["DateToTime"] = dr["DateToTime"];

                SPFieldUrlValue uv = new SPFieldUrlValue();

                uv.Url = SPContext.Current.Web.Url
                    + "/_layouts/CA/WorkFlows/TimeOff/DisplayForm.aspx?List="
                    + SPContext.Current.ListId.ToString()
                    + "&ID="
                    + SPContext.Current.ListItem.ID;

                uv.Description = this.WorkFlowNumber;
                item["WorkFlowNumber"] = uv;
                item["WorkflowID"] = this.WorkFlowNumber;

                //item["Status"] = WorkflowContext.Current.DataFields["Status"];
               
                
                if (btnStart.Text != "Save")
                {
                    //如果是提交

                    item["SubmittedAt"] = DateTime.Now;

                    int year = DateTime.Parse(dr["DateFrom"] + "").Year;

                    //根据field来查询
                    SPListItemCollection items = sps.Query(listBalance, field.Equal(this.DataForm1.ApplicantName) && field2.Equal(year), 1);

                    //审批submit后 在balance表中扣除所请的天数
                    SPListItem itemBalance = items[0];

 

                    var leaveDays = double.Parse(dr["LeaveDays"] + "");

                    if ((dr["LeaveType"] + "").Equals("Annual Leave 年假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        itemBalance["AnnualBalance"] = Convert.ToDouble(itemBalance["AnnualBalance"]) - leaveDays;
                        item["PayDays"] = leaveDays;
                    }
                    else if ((dr["LeaveType"] + "").Equals("Sick Leave 病假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var sickBalance = Convert.ToDouble(itemBalance["SickBalance"]);
                        

                        if (sickBalance > 0)
                        {
                            if (sickBalance >= leaveDays)
                            {
                                item["PayDays"] = 0;
                            }
                            else
                            {
                                item["PayDays"] = leaveDays - sickBalance;
                            }
                        }
                        else
                        {
                            item["PayDays"] = leaveDays;
                        }

                        itemBalance["SickBalance"] = sickBalance - leaveDays;
                    }
                    else
                    {
                        item["PayDays"] = leaveDays;
                    }

                    try
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                            {
                                itemBalance.Web.AllowUnsafeUpdates = true;
                                itemBalance.Update();
                                itemBalance.Web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                    catch
                    {
                        Response.Write("An error occured while updating items.");
                    }

                    //itemBalance.Web.AllowUnsafeUpdates = true;
                    //itemBalance.Update();

                    
                }

                item.Update();
                #endregion
            }


        //    base.UpdatePermissions();
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataForm1.DataTableRecord.Rows.Count == 0)
            {
                e.Cancel = true;
                
                this.labMessage.Text = "Unable to submit a leave application without any leave request.";
                return;
            }

            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());
            double annualLeaveDays = -1;

            foreach (DataRow dr in DataForm1.DataTableRecord.Rows)
            {
                int year = DateTime.Parse(dr["DateFrom"] + "").Year;

                if (year != DateTime.Parse(dr["DateTo"] + "").Year)
                {
                    this.labMessage.Text = "You can not apply leave for different year in the same request.";
                    e.Cancel = true;
                    return;
                }

                //根据field来查询
                QueryField field = new QueryField("Employee");
                QueryField field2 = new QueryField("Year");
                SPListItemCollection items = sps.Query(listBalance, field.Equal(this.DataForm1.ApplicantName) && field2.Equal(year), 1);

                //if can not find the item, means no datas in list
                if (items == null || items.Count == 0)
                {
                    e.Cancel = true;

                    this.labMessage.Text = "You don't have any remaining leave balance for year " + year + ",please contact IT.";
                    return;
                }

                //check the data
                SPListItem item = items[0];

                if (annualLeaveDays == -1)
                    annualLeaveDays = Convert.ToDouble(item["AnnualBalance"]);

                if ((dr["LeaveType"] + "").Equals("Annual Leave 年假", StringComparison.CurrentCultureIgnoreCase))
                {
                    annualLeaveDays -= Convert.ToDouble(dr["LeaveDays"] + "");
                    if (annualLeaveDays < 0)
                    {
                        e.Cancel = true;

                        this.labMessage.Text = "The annual leave balance will be less than 0,can't submit.";
                        return;
                    }
                }
            }

            StartWorkflowButton btnStart = sender as StartWorkflowButton;

            if (string.Equals(btnStart.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
               
                WorkflowContext.Current.UpdateWorkflowVariable("IsSaved", true);
                string strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/ApplicantEditForm.aspx";
                string strNextTaskTitle = "Please complete your leave application";
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

                WorkflowContext.Current.DataFields["IsSave"] = 1;
                WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            }
            else
            {
                //验证组里用户是否为空
                List<string> list = WorkFlowUtil.UserListInGroup("wf_HR");
                if (list.Count == 0)
                {
                    DisplayMessage("Unable to submit the application. There is no user in wf_HR group. Please contact IT for further help.");
                    e.Cancel = true;
                    return;
                }
                string strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/EditForm.aspx";
                string strNextTaskTitle = string.Format("{0}'s leave application needs approval", this.CurrentEmployee.DisplayName);
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
                WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);
             
            }
             
            string strDepartment = DataForm1.Applicant.Department;           
            string deptHead = WorkFlowUtil.GetEmployeeApprover(this.CurrentEmployee).UserAccount;             
            string strHRHeadAccount = UserProfileUtil.GetDepartmentManager("hr");              
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadAccount", deptHead);
            NameCollection hrAccounts = WorkFlowUtil.GetUsersInGroup("wf_HR");          
            WorkflowContext.Current.UpdateWorkflowVariable("HRAccounts", hrAccounts);
            WorkflowContext.Current.UpdateWorkflowVariable("HRHeadAccount", strHRHeadAccount);

            bool IsSick = this.DataForm1.IsSickLeave; 
            WorkflowContext.Current.UpdateWorkflowVariable("IsSickLeave", IsSick);
         
            this.WorkFlowNumber=CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["EmployeeID"] = this.DataForm1.EmployeeNo;
        }

        private string CreateWorkFlowNumber()
        {
            return "TO_" + WorkFlowUtil.CreateWorkFlowNumber("TimeOff").ToString("000000");
        }


        [WebMethod]
        public static decimal GetMixedDays(string formBeginAt, string formEndAt, string formBeginAt_am_or_pm, string formEndAt_am_or_pm)
        {
            DateTime formBeginAtDT = DateTime.Parse(formBeginAt);
            DateTime formEndAtDT = DateTime.Parse(formEndAt);
            DateTime queryBeginAtDT = DateTime.Parse("2011-06-05");
            DateTime queryEndAtDT = DateTime.Parse("2011-06-28");

            return WorkFlowUtil.GetMixedDays(formBeginAtDT, formEndAtDT, formBeginAt_am_or_pm, formEndAt_am_or_pm, queryBeginAtDT, queryEndAtDT);
        }
    }
}
