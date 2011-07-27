using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CA.SharePoint;
using System.Data;
using QuickFlow.Core;
using Microsoft.SharePoint;
using System.Configuration;


namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.TravelRequset
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);

        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;

            WorkflowContext curContext = WorkflowContext.Current;
            if (curContext.Task.Step == "ReceptionistTask")
            {
                //两次confirm为同一个人
                if (SPContext.Current.ListItem["IsTheSame"]+""=="yes")
                {
                    //流程走完形成报表
                    GenerateReport();
                }
                //两次confirm为不同的人
                else if (string.IsNullOrEmpty(SPContext.Current.ListItem["TimesConfirm"] + ""))
                {
                    WorkflowContext.Current.DataFields["TimesConfirm"] = "SecondComfirm";
                }
                else if (SPContext.Current.ListItem["TimesConfirm"] + "" == "SecondComfirm")
                {
                    GenerateReport();
                }
            }
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        private void GenerateReport()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
                ISharePointService sps = ServiceFactory.GetSharePointService(true);
                SPListItem item = null;

                DataTable TravelDetails = this.DataForm1.dtTravelDetails;
                SPList list = sps.GetList(CAWorkFlowConstants.ListName.TravelApplication.ToString());

                string rootweburl = ConfigurationManager.AppSettings["rootweburl"] + "";
                if (string.IsNullOrEmpty(rootweburl))
                {
                    rootweburl = "https://portal.c-and-a.cn";
                }

                foreach (DataRow dr in TravelDetails.Rows)
                {
                    SPFieldUrlValue uv = new SPFieldUrlValue();

                    uv.Url = rootweburl + "/WorkFlowCenter/_layouts/CA/WorkFlows/TravelRequest/DisplayForm.aspx?List="
                        + SPContext.Current.ListId.ToString()
                        + "&ID="
                        + SPContext.Current.ListItem.ID;
                    uv.Description = DataForm1.WorkflowNumber;

                    item = list.Items.Add();
                    item["Name"] = DataForm1.Name;
                    item["Department"] = DataForm1.Department;
                    item["WorkflowNumber"] = uv;
                    item["TotalNumber"] = DataForm1.EstimateDays;
                    item["Purpose"] = DataForm1.Purpose;
                    item["Date"] = dr["FromDate"];
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
            });
        }
    }
}
