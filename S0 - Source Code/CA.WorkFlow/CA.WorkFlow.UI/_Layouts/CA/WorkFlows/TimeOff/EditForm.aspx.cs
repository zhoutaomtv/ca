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
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using System.Collections.Generic;
using CA.SharePoint.Utilities.Common;
using System.Text;

namespace CA.WorkFlow.UI.TimeOff
{
    public partial class EditForm :CAWorkFlowPage
    {
        bool needEmail = false;
        protected void Page_Load(object sender, EventArgs e)
        { 
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
           
        }

        protected override void OnPreRender(EventArgs e)
        {
            //if (WorkflowContext.Current.Task.Step == DataForm.Constants.HREeviewStep)
            //{
            //    this.DataForm1.AttachmentVisible = true;
            //}
            //else if (WorkflowContext.Current.Task.Step== DataForm.Constants.HRHeadApproveStep)
            //{
            //    this.DataForm1.AttachmentVisible = this.DataForm1.IsSick;
            //}
            base.OnPreRender(e);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            // edit by caixiang 
            if (e.Action == "Reject")
            {
                //得到sharepointservice 用来进行sharepoint API操作
                ISharePointService sps = ServiceFactory.GetSharePointService(true);

                //获取列表
                SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());

                //根据field来查询
                QueryField field = new QueryField("Employee");
                QueryField field2 = new QueryField("Year");

                DataTable dtRecords = this.DataForm1.DataTableRecord;

                foreach (DataRow dr in dtRecords.Rows)
                {
                    int year = DateTime.Parse(dr["DateFrom"] + "").Year;

                    //根据field来查询
                    SPListItemCollection items = sps.Query(listBalance, field.Equal(this.DataForm1.ApplicantName) && field2.Equal(year), 1);

                    //审批submit后 在balance表中扣除所请的天数
                    SPListItem itemBalance = items[0];

                    if ((dr["LeaveType"] + "").Equals("Annual Leave 年假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        itemBalance["AnnualBalance"] = Convert.ToDouble(itemBalance["AnnualBalance"]) + double.Parse(dr["LeaveDays"] + "");
                    }
                    else if ((dr["LeaveType"] + "").Equals("Sick Leave 病假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        itemBalance["SickBalance"] = Convert.ToDouble(itemBalance["SickBalance"]) + double.Parse(dr["LeaveDays"] + "");
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
                    catch (Exception ex)
                    {
                        Response.Write("An error occured while updating items.");
                    }
                }
            }
            //if (this.IsNeedUpdateBalanceStep && e.Action == "Approve")
            //{
            //    //得到sharepointservice 用来进行sharepoint API操作
            //    ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //    //获取列表
            //    SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());

            //    //根据field来查询
            //    QueryField field = new QueryField("Employee");
            //    QueryField field2 = new QueryField("Year");
            //    SPListItemCollection items = sps.Query(listBalance, field.Equal(this.DataForm1.ApplicantName) && field2.Equal(DateTime.Now.Year), 1);

            //    //审批通过后 在balance表中扣除所请的天数
            //    SPListItem item = items[0];

            //    double annualLeaveDays = Convert.ToDouble(item["AnnualLeave"]) - this.DataForm1.LeaveBalance;
            //    if (annualLeaveDays < 0)
            //    {
            //        e.Cancel = true;

            //        this.labMessage.Text = "The annual leave balance will be less than 0, can not approve.";
            //        return;
            //    }
            //    item["AnnualLeave"] = Convert.ToDouble(item["AnnualLeave"]) - this.DataForm1.LeaveBalance;
            //    item["SickLeave"] = Convert.ToDouble(item["SickLeave"]) - this.DataForm1.SickLeaveBalance;
            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //}

            string strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/EditForm.aspx";
            string strUserName = SPContext.Current.ListItem["Applicant"] + "";
            string strNextTaskTitle = string.Format("{0}'s leave application needs approval", strUserName);

            if (WorkflowContext.Current.Task.Step == DataForm.Constants.HeadApproveStep && this.DataForm1.IsSickLeave)
            {
                strNextTaskTitle = string.Format("Please review {0}'s leave application", strUserName);
                strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/EditForm.aspx";
            }
            if (string.Equals(e.Action, "Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/ApplicantEditForm.aspx";
                strNextTaskTitle = "Please modify your leave application";
            }

            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;

            if (this.IsNeedUpdateBalanceStep && e.Action == "Approve")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
            if (WorkflowContext.Current.Task.Step == DataForm.Constants.HeadApproveStep
                && string.Equals(e.Action, "Approve", StringComparison.CurrentCultureIgnoreCase))
            {
                needEmail = true;
            }
            TaskOutcome = e.Action;
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            if (TaskOutcome.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
            {

                //frist delete the items
                //wsq updated: no!
                //DataTable dtRecords = this.DataForm1.DataTableRecord;
                //WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkflowID", strTimeOffNumber);

                string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
                
                string strDepartment = this.DataForm1.Applicant.Department;


                var now = DateTime.Now;

                //当前统计月
                var statMon = new DateTime(now.Year, now.Month, 1);
                int downloadsCount = SPContext.Current.Web.Lists["LeaveDownloads"].Items.Count;
                if (SPContext.Current.Web.Lists["LeaveDownloads"].Items.Count > 0)
                {
                    SPListItem lastDownload = SPContext.Current.Web.Lists["LeaveDownloads"].Items[downloadsCount - 1];
                    statMon = DateTime.Parse(lastDownload["Title"] + "").AddMonths(1);
                }

                //扣薪请假类型
                var timeWageTypes = new Dictionary<string, string>();
                timeWageTypes.Add("Sick Leave 病假", "6HS0");
                timeWageTypes.Add("No Pay Leave 不带薪假", "6HS1");
                timeWageTypes.Add("Maternity Leave 产假", "6HS6");


                SPListItemCollection records = ListQuery.Select()
                             .From(SPContext.Current.Web.Lists[CAWorkFlowConstants.ListName.LeaveRecord.ToString()])
                             .Where(new QueryField("WorkflowID", false).Equal(SPContext.Current.ListItem["WorkFlowNumber"] + ""))
                             .GetItems();


                foreach (SPListItem record in records)
                {
                    record["Department"] = WorkflowContext.Current.DataFields["Department"];
                    record["Status"] = WorkflowContext.Current.DataFields["Status"];




                    if (!(record["LeaveType"] + "").Equals("Annual Leave 年假", StringComparison.CurrentCultureIgnoreCase) && !(record["LeaveType"] + "").Equals("No Pay Leave 不带薪假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        needEmail &= true;
                    }
                    else
                    {
                        needEmail = false;
                    }


                    if (WorkflowContext.Current.DataFields["Status"].ToString().ToLower() == "completed")
                    {
                        record["CompletedAt"] = now;

                        #region update LeaveData 扣薪统计表

                        if (timeWageTypes.Keys.Contains(record["LeaveType"] + ""))
                        {
                            var payDays = string.IsNullOrEmpty(record["PayDays"] + "") ? decimal.Parse(record["LeaveDays"] + "") : decimal.Parse(record["PayDays"] + "");


                            //根据DateFrom、DateTo，分段拿月份
                            var mons = new List<DateTime>();

                            var dateFrom = DateTime.Parse(record["DateFrom"] + "");
                            var dateTo = DateTime.Parse(record["DateTo"] + "");
                            var dateFromMon1stDay = new DateTime(dateFrom.Year, dateFrom.Month, 1);
                            var dateToMon1stDay = new DateTime(dateTo.Year, dateTo.Month, 1);

                            while (dateFromMon1stDay <= dateToMon1stDay)
                            {
                                if (!mons.Contains(dateFromMon1stDay))
                                {
                                    mons.Add(dateFromMon1stDay);
                                }
                                dateFromMon1stDay = dateFromMon1stDay.AddMonths(1);
                            }

                            mons.Reverse();

                            foreach (var mon in mons)
                            {
                                SPList leaveDataList = SPContext.Current.Web.Lists["LeaveData"];

                                SPListItem leaveData = null;
                                if (leaveDataList.Items.Count > 0)
                                {

                                    var qStatMon = new QueryField("StatMon", false);
                                    var qEmployeeAccount = new QueryField("EmployeeAccount", false);
                                    var qTimeWageType = new QueryField("TimeWageType", false);
                                    var qDate = new QueryField("Date", false);
                                    var qDataType = new QueryField("DataType", false);
   
                                    CamlExpression exp = null;
                                    //exp = WorkFlowUtil.LinkAnd(exp, qStatMon.Equal(statMon));
                                    //请假日期大于当前统计月的，统计到请假的将来月份中去
                                    exp = WorkFlowUtil.LinkAnd(exp, qStatMon.Equal(mon > statMon ? mon : statMon));
                                    exp = WorkFlowUtil.LinkAnd(exp, qEmployeeAccount.Equal(record["EmployeeLoginName"]));
                                    exp = WorkFlowUtil.LinkAnd(exp, qTimeWageType.Equal(timeWageTypes[record["LeaveType"] + ""]));
                                    exp = WorkFlowUtil.LinkAnd(exp, qDate.Equal(mon));

                                    CamlExpression exp2 = null;
                                    exp2 = WorkFlowUtil.LinkOr(exp2, qDataType.Equal("Normal"));
                                    exp2 = WorkFlowUtil.LinkOr(exp2, qDataType.Equal("Supplement"));

                                    exp = WorkFlowUtil.LinkAnd(exp, exp2);

                                    var leaveDataColl = ListQuery.Select()
                                        .From(leaveDataList)
                                        .Where(exp).GetItems();

                                    if (leaveDataColl.Count == 1)
                                    {
                                        leaveData = leaveDataColl[0];
                                    }
                                }

                                var monLastSecond = mon.AddMonths(1).AddSeconds(-1);

                                

                                var days = WorkFlowUtil.GetMixedDays(
                                        dateFrom,
                                        dateTo,
                                        record["DateFromTime"] + "",
                                        record["DateToTime"] + "",
                                        mon,
                                        monLastSecond);


                                if (days <= payDays)
                                {
                                    payDays -= days;
                                }
                                else
                                {
                                    days = payDays;
                                    payDays = 0;
                                }
                                //ok
                                //扣薪的天数
                                if (days > 0)
                                {
                                    var number = days;

                                    switch (timeWageTypes[record["LeaveType"] + ""])
                                    {
                                        case "6HS0":
                                            number = days * 8;
                                            break;
                                        case "6HS1":
                                            number = days * 8;
                                            break;
                                        case "6HS6":
                                            number = days;
                                            break;
                                        default:
                                            number = days;
                                            break;
                                    }

                                    //判断是更新是插入
                                    if (leaveData == null)
                                    {
                                        leaveData = leaveDataList.Items.Add();
                                        leaveData["StatMon"] = mon > statMon ? mon : statMon;
                                        leaveData["EmployeeAccount"] = record["EmployeeLoginName"];

                                        //if (employee != null)
                                        //{
                                        //    leaveData["EmployeeID"] = employee.EmployeeID;
                                        //    leaveData["EmployeeName"] = employee.DisplayName;
                                        //}

                                        leaveData["EmployeeID"] = record["EmployeeID"];
                                        leaveData["EmployeeName"] = record["EmployeeDisplayName"];

                                        leaveData["TimeWageType"] = timeWageTypes[record["LeaveType"] + ""];
                                        leaveData["Date"] = mon;
                                        leaveData["Days"] = days;
                                        leaveData["Number"] = number;
                                        leaveData["DataType"] = mon >= statMon ? "Normal" : "Supplement";
                                    }
                                    else
                                    {
                                        leaveData["Days"] = Convert.ToDecimal(leaveData["Days"]) + days;
                                        leaveData["Number"] = Convert.ToDecimal(leaveData["Number"]) + number;
                                    }



                                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                    {
                                        using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                                        {
                                            leaveData.Web.AllowUnsafeUpdates = true;
                                            leaveData.Update();
                                            leaveData.Web.AllowUnsafeUpdates = false;
                                        }
                                    }

                                }
                            }
                        }



                        #endregion
                    }

                    try
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                                {
                                    record.Web.AllowUnsafeUpdates = true;
                                    record.Update();
                                    record.Web.AllowUnsafeUpdates = false;
                                }
                            }
                        });
                    }
                    catch
                    {
                        Response.Write("An error occured while updating items.");
                    }
                }

                //sendMail();
            }
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx" + "?" + sb.ToString());
        }

        public bool IsNeedUpdateBalanceStep
        {
            get
            {
                if (WorkflowContext.Current.Task.Step == DataForm.Constants.HRHeadApproveStep)
                    return true;
                if (WorkflowContext.Current.Task.Step == DataForm.Constants.HeadApproveStep && !this.DataForm1.IsSickLeave)
                    return true;
                return false;
            }
        }

        private void sendMail()
        {
             if (needEmail)
             {
                 List<string> mailList = new List<string>();
                 SPUser user=EnsureUser(DataForm1.ApplicantName);
                 mailList.Add(user.Email);
                 StringDictionary dict = new StringDictionary();
                 dict.Add("to", string.Join(";", mailList.ToArray()));
                 dict.Add("subject",  "Your Leave Application");

                 string mcontent = " You need to submit some relative documents to HR for your leave application "
                     + SPContext.Current.ListItem["WorkFlowNumber"]+".<br/><br/>" + @"Please view the detail by clicking <a href='"
                        + SPContext.Current.Web.Url + "/_layouts/CA/WorkFlows/TimeOff/DisplayForm.aspx?List="
                        + SPContext.Current.ListId.ToString()
                        + "&ID="
                        + SPContext.Current.ListItem.ID
                        + "'>here</a>.";

                 SPUtility.SendEmail(SPContext.Current.Web, dict, mcontent);
             }
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
