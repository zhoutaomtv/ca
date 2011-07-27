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
using CA.SharePoint.Utilities.Common;
using System.Collections.Generic;

namespace CA.WorkFlow.UI.TimeOff
{
    public partial class ApplicantEditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";

            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.btnSave.Click += new EventHandler(btnSave_Click);

            if (Convert.ToInt32(SPContext.Current.ListItem["IsSave"]) == 1)
            {
                this.trComments.Visible = false;
            }
            if (!IsPostBack)
            {
                if (WorkflowContext.Current.Step == "ApplicantModify")
                {
                    this.body.Value = SPContext.Current.ListItem["SaveComment"];
                }
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            UpdateRecords(false);

            SPListItem item = SPContext.Current.ListItem;
            item["EmployeeID"] = this.DataForm1.EmployeeNo;
            item["SaveComment"] = this.body.Value;
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
            catch
            {
                Response.Write("An error occured while updating the items");
            }

            //item.Web.AllowUnsafeUpdates = true;
            //item.Update();         

            base.Back();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";

                SPListItem item = SPContext.Current.ListItem;

                ISharePointService sps = ServiceFactory.GetSharePointService(true);
                SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());
                QueryField field = new QueryField("Employee");
                QueryField field2 = new QueryField("Year");

                

                

                int year = DateTime.Parse(item["DateFrom"] + "").Year;

                //根据field来查询
                SPListItemCollection items = sps.Query(listBalance, field.Equal(this.DataForm1.ApplicantName) && field2.Equal(year), 1);

                //审批submit后 在balance表中扣除所请的天数
                SPListItem itemBalance = items[0];

                

                return;
            }
            bool IsSick = this.DataForm1.IsSickLeave;

            WorkflowContext.Current.UpdateWorkflowVariable("IsSickLeave", IsSick);

            string strNextTaskUrl = @"_Layouts/CA/WorkFlows/TimeOff/EditForm.aspx";
            string strNextTaskTitle = string.Format("{0}'s leave application needs approval", SPContext.Current.Web.CurrentUser.Name);
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskUrl", strNextTaskUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("NextTaskTitle", strNextTaskTitle);

            WorkflowContext.Current.DataFields["IsSave"] = 0;
            WorkflowContext.Current.DataFields["EmployeeID"] = this.DataForm1.EmployeeNo;
            WorkflowContext.Current.DataFields["Status"] = "In Progress";
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            UpdateRecords(true);
        }

        private void UpdateRecords(bool NeedUpdateBalance)
        {
            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = SPContext.Current.Web.Lists[CAWorkFlowConstants.ListName.LeaveRecord.ToString()];
            SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());
            QueryField field = new QueryField("Employee");
            QueryField field2 = new QueryField("Year");

            //frist delete the items
            string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkflowID", strTimeOffNumber);
            string strApplicantName = this.DataForm1.ApplicantName;
            string strDepartment = this.DataForm1.Applicant.Department;
            SPListItem item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                SPFieldUrlValue uv = new SPFieldUrlValue();
                uv.Url = SPContext.Current.Web.Url
                  + "/_layouts/CA/WorkFlows/TimeOff/DisplayForm.aspx?List="
                  + SPContext.Current.ListId.ToString()
                  + "&ID="
                  + SPContext.Current.ListItem.ID;
                uv.Description = strTimeOffNumber;
               

                item = listRecord.Items.Add();
                item["Applicant"] = strApplicantName;
                item["EmployeeLoginName"] = this.DataForm1.Applicant.UserAccount;
                item["EmployeeID"] = this.DataForm1.Applicant.EmployeeID;
                item["EmployeeDisplayName"] = this.DataForm1.Applicant.DisplayName;
                item["Department"] = strDepartment;
                item["LeaveType"] = dr["LeaveType"];
                item["LeaveDays"] = dr["LeaveDays"];
                item["DateFrom"] = dr["DateFrom"];
                item["DateTo"] = dr["DateTo"];
                item["DateFromTime"] = dr["DateFromTime"];
                item["DateToTime"] = dr["DateToTime"];             
                item["WorkFlowNumber"] = uv;
                item["WorkflowID"] = strTimeOffNumber;

                item["SubmittedAt"] = DateTime.Now;

                //item["Status"] = WorkflowContext.Current.DataFields["Status"];
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
                catch 
                {
                    Response.Write("An error occured while updating the items");
                }

                if (NeedUpdateBalance)
                {
                    int year = DateTime.Parse(dr["DateFrom"] + "").Year;

                    SPListItemCollection items = sps.Query(listBalance, field.Equal(this.DataForm1.ApplicantName) && field2.Equal(year), 1);

                    //审批submit后 在balance表中扣除所请的天数
                    SPListItem itemBalance = items[0];

                    if ((dr["LeaveType"] + "").Equals("Annual Leave 年假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        itemBalance["AnnualBalance"] = Convert.ToDouble(itemBalance["AnnualBalance"]) - double.Parse(dr["LeaveDays"] + "");
                    }
                    else if ((dr["LeaveType"] + "").Equals("Sick Leave 病假", StringComparison.CurrentCultureIgnoreCase))
                    {
                        itemBalance["SickBalance"] = Convert.ToDouble(itemBalance["SickBalance"]) - double.Parse(dr["LeaveDays"] + "");
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
                        Response.Write("An error occured while updating the items");
                    }
                }
            }

        }
    }
}
