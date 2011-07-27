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
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;

namespace CA.WorkFlow.UI.ITHardwareOrSoftwareApplication
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.actions.Attributes.Add("onclick", "return CheckComments();");
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            hdBody.Value = this.body.ClientID;
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            if ((WorkflowContext.Current.Step.ToString() == "ITMember")&&TaskOutcome.Equals("Confirm",StringComparison.CurrentCultureIgnoreCase))
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
            else if ((WorkflowContext.Current.Step.ToString() == "ITHeader" && !DataForm1.IsFOCO) || WorkflowContext.Current.Step.ToString() == "FOCO")
            {
                //added by wsq 0906
                if (SPContext.Current.ListItem["Status"].ToString().Equals("Completed", StringComparison.CurrentCultureIgnoreCase))
                {
                    List<string> mailList = new List<string>();
                    List<SPUser> users = WorkFlowUtil.GetSPUsersInGroup("wf_IT");
                    foreach (SPUser user in users)
                    {
                        mailList.Add(user.Email);
                    }
                    string name = this.DataForm1.Name;
                    StringDictionary dict = new StringDictionary();
                    dict.Add("to", string.Join(";", mailList.ToArray()));
                    dict.Add("subject", name + "'s IT request");

                    string mcontent = name + "'s IT hardware software request has been approved. Workflow number is "
                        + SPContext.Current.ListItem["WorkFlowNumber"] + ".<br/><br/>" + @"Please view the detail by clicking <a href='"
                        + SPContext.Current.Web.Url + "/_layouts/CA/WorkFlows/ITHardwareOrSoftwareApplication/DisplayForm.aspx?List="
                        + SPContext.Current.ListId.ToString()
                        + "&ID="
                        + SPContext.Current.ListItem.ID
                        + "'>here</a>.";

                    SPUtility.SendEmail(SPContext.Current.Web, dict, mcontent);
                }
            }
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (WorkflowContext.Current.Step.ToString() == "ITMember")
            {
                string msg = DataForm1.ValidSavedate;
                if (!string.IsNullOrEmpty(msg))
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
            }
            WorkflowContext.Current.DataFields["Flag"] = "Approve";
            if (WorkflowContext.Current.Step.ToString() == "ITHeader")
            {
                bool strFOCO = this.DataForm1.IsFOCO;
                WorkflowContext.Current.UpdateWorkflowVariable("IsFOCO", strFOCO);
                WorkflowContext.Current.DataFields["FOCOApprove"] = DataForm1.IsFOCO;
            }
            if (WorkflowContext.Current.Step == "FOCO" && e.Action == "Approve")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
            else if (WorkflowContext.Current.Step == "ITHeader" && !this.DataForm1.IsFOCO && e.Action == "Approve")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
            else
            {
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }
            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;

            TaskOutcome = e.Action;
        }
    }
}
