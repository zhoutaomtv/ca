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
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.TimeOff
{
    public partial class CancelApproveForm : CAWorkFlowPage
    {
        public bool IsCompleted
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            if (this.IsCompleted)
            {
                DataTable dtRecords = this.DataForm1.DataTableRecord;

                ISharePointService sps = ServiceFactory.GetSharePointService(true);

                SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());
                QueryField field = new QueryField("Employee");
                QueryField field2 = new QueryField("Year");

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
                    catch 
                    {
                        Response.Write("An error occured while updating items.");
                    }

                }


                //SPList listRecord = sps.GetList(CAWorkFlowConstants.ListName.LeaveRecord.ToString());

                

                string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
                //WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", strTimeOffNumber);

                SPListItemCollection records = ListQuery.Select()
                             .From(SPContext.Current.Web.Lists[CAWorkFlowConstants.ListName.LeaveRecord.ToString()])
                             .Where(new QueryField("WorkflowID", false).Equal(strTimeOffNumber))
                             .GetItems();

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
                //timeWageTypes.Add("Sick Leave 病假", "6HS0");
                timeWageTypes.Add("No Pay Leave 不带薪假", "6HS1");
                timeWageTypes.Add("Maternity Leave 产假", "6HS6");

                foreach (SPListItem record in records)
                {
                    //if ((item1["WorkflowID"] + "").ToLower() == strTimeOffNumber.ToLower())
                    //{

                        record["Status"] = WorkflowContext.Current.DataFields["Status"];
                        try
                        {
                            record.Web.AllowUnsafeUpdates = true;
                            record.Update();
                            record.Web.AllowUnsafeUpdates = false;
                        }
                        catch 
                        {
                            Response.Write("An error occured while updating the items");
                        }

                        //break;

                    //}


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
                                    //请假日期大于当前统计月的，统计到请假的将来月份中去
                                    exp = WorkFlowUtil.LinkAnd(exp, qStatMon.Equal(mon > statMon ? mon : statMon));
                                    exp = WorkFlowUtil.LinkAnd(exp, qEmployeeAccount.Equal(record["EmployeeLoginName"]));
                                    exp = WorkFlowUtil.LinkAnd(exp, qTimeWageType.Equal(timeWageTypes[record["LeaveType"] + ""]));
                                    exp = WorkFlowUtil.LinkAnd(exp, qDate.Equal(mon));
                                    exp = WorkFlowUtil.LinkAnd(exp, qDataType.Equal("Cancelled"));

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
                                        leaveData["EmployeeID"] = record["EmployeeID"];
                                        leaveData["EmployeeName"] = record["Title"];
                                        leaveData["TimeWageType"] = timeWageTypes[record["LeaveType"] + ""];
                                        leaveData["Date"] = mon;
                                        leaveData["Days"] = days * -1;
                                        leaveData["Number"] = number * -1;
                                        leaveData["DataType"] = "Cancelled";
                                    }
                                    else
                                    {
                                        leaveData["Days"] = Convert.ToDecimal(leaveData["Days"]) - days;
                                        leaveData["Number"] = Convert.ToDecimal(leaveData["Number"]) - number;

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

            }
            base.Back();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (IsNeedUpdateBalanceStep
                && string.Equals(e.Action, "Approve", StringComparison.CurrentCultureIgnoreCase))
            {
                this.IsCompleted = true;
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
            }

            if (string.Equals(e.Action, "Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
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
    }
}
