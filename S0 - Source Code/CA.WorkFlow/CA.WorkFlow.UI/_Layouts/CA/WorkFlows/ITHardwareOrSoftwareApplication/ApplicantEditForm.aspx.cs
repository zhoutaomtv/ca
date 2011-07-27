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
using CA.WorkFlow.UI;
using CA.WorkFlow.UI.Code;
using QuickFlow;
using CA.SharePoint.Utilities.Common;
using System.Collections.Generic;

namespace CA.WorkFlows.ITHardwareOrSoftwareApplication
{
    public partial class ApplicantEditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.actions.Attributes.Add("onclick", "return CheckValue()");
            this.actions.OnClientClick += "return CheckValue(this.value);";
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted +=new EventHandler<EventArgs>(actions_ActionExecuted);
            Common.IsShowComments(trComments, tblCommentsList);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }
            string msg = DataForm1.Validate;
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }
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

            Employee approver = WorkFlowUtil.GetEmployeeApprover(DataForm1.Applicant);
            if (approver == null)
            {
                //Don
                DisplayMessage("Unable to find an approver for the applicant. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }

            NameCollection DepartHeadUser = new NameCollection();
            DepartHeadUser.Add(approver.UserAccount);
            NameCollection ITHeadUser = new NameCollection();
            string strHRHeadAccount = UserProfileUtil.GetDepartmentManager("it");
            ITHeadUser.Add(strHRHeadAccount);
            NameCollection CFOUser = new NameCollection();
            lst = WorkFlowUtil.UserListInGroup("wf_CFO");
            CFOUser.AddRange(lst.ToArray());
            //组结束

            try
            {
                WorkflowContext.Current.UpdateWorkflowVariable("ITMemberAccounts", ItMemberUser);
                WorkflowContext.Current.UpdateWorkflowVariable("DepartHeaderAccounts", DepartHeadUser);
                WorkflowContext.Current.UpdateWorkflowVariable("ITHeaderAccounts", ITHeadUser);
                WorkflowContext.Current.UpdateWorkflowVariable("FOCOAccounts", CFOUser);
                WorkflowContext.Current.UpdateWorkflowVariable("IsFOCO", false);
                WorkflowContext.Current.UpdateWorkflowVariable("ITMemberTitle", DataForm1.Applicant.DisplayName + "'s IT request needs confirm");
                WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeaderTitle", DataForm1.Applicant.DisplayName + "'s IT request needs approval");
                WorkflowContext.Current.UpdateWorkflowVariable("ITHeaderTitle", DataForm1.Applicant.DisplayName + "'s IT request needs approval");
                WorkflowContext.Current.UpdateWorkflowVariable("FOCOTitle", DataForm1.Applicant.DisplayName + "s 'IT request needs approval");
                WorkflowContext.Current.UpdateWorkflowVariable("UpdateTaskTitle", "Please complete IT request for " + DataForm1.Applicant.DisplayName);

                WorkflowContext.Current.DataFields["Name"] = DataForm1.Name;
                WorkflowContext.Current.DataFields["Department"] = DataForm1.Department;
                WorkflowContext.Current.DataFields["FOCOApprove"] = DataForm1.IsFOCO;
                WorkflowContext.Current.DataFields["ApplyDate"] = DataForm1.ApplyDate;
                WorkflowContext.Current.DataFields["AppliedUser"] = this.DataForm1.Applicant.UserAccount;
                WorkflowContext.Current.DataFields["ReasonforRequest"] = this.DataForm1.ReasonforRequest;
                WorkflowContext.Current.DataFields["Flag"] = "Submit";

                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void actions_ActionExecuted(object sender, EventArgs e)
        {
            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ITRequestItems.ToString());

            //frist delete the items
            string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", strTimeOffNumber);

            SPListItem item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                item = listRecord.Items.Add();
                item["HardwareOrSoftwareName"] = dr["HardwareOrSoftwareName"];
                item["Cost"] = dr["Cost"];
                item["WorkFlowNumber"] = this.DataForm1.WorkflowNumber;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string msg = DataForm1.ValidSavedate;
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    DisplayMessage(msg);
            //    return;
            //}
            SPListItem item = SPContext.Current.ListItem;
            item["Name"] = DataForm1.Name;
            item["Department"] = DataForm1.Department;
            item["FOCOApprove"] = DataForm1.IsFOCO;
            item["ApplyDate"] = DataForm1.ApplyDate;
            item["AppliedUser"] = this.DataForm1.Applicant.UserAccount;
            item["ReasonforRequest"] = DataForm1.ReasonforRequest;

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

            DataTable dtRecords = this.DataForm1.DataTableRecord;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ITRequestItems.ToString());

            //frist delete the items
            string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            WorkFlowUtil.RemoveExistingRecord(listRecord, "WorkFlowNumber", strTimeOffNumber);

            item = null;
            foreach (DataRow dr in dtRecords.Rows)
            {
                item = listRecord.Items.Add();
                item["HardwareOrSoftwareName"] = dr["HardwareOrSoftwareName"];
                item["Cost"] = dr["Cost"];
                item["WorkFlowNumber"] = this.DataForm1.WorkflowNumber;
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
            base.Back();
        }
    }
}
