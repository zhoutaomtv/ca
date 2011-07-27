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
using CA.SharePoint.Utilities.Common;
using System.Collections.Generic;
using QuickFlow;

namespace CA.WorkFlow.UI.TravelRequset
{
    public partial class SavedForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.actions.OnClientClick +=DataForm1.CheckIsCancel("End");
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);

            //DataForm1.CanEditUser = true;
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            UpdateRecords();
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }
            string msg = DataForm1.Validate();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            UpdateWorkflow(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            item["UserName"] = DataForm1.Applicant.UserAccount;
            item["EnglishName"] = DataForm1.Applicant.DisplayName;
            item["Department"] = DataForm1.Applicant.Department;
            item["JobTitle"] = DataForm1.Applicant.Title;
            item["AirNotes"] = DataForm1.AirNotes;
            item["HotelNotes"] = DataForm1.HotelNotes;

            if (DataForm1.radiobuttonNO)
            {
                item["btUser"] = DataForm1.btUser1;
            }
            else
            {
                item["btUser"] = string.Empty;
            }

            if (DataForm1.RadioNo)
            {
                item["btApplicant"] = DataForm1.btApplicant1;
            }
            else
            {
                item["btApplicant"] = string.Empty;
            }
            if (DataForm1.RadioYes)
            {
                item["RadioYes"] = DataForm1.RadioYes.ToString();
            }
            else
            {
                item["RadioYes"] = "False";
            }
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
            UpdateRecords();
            base.Back();
        }

        private void UpdateWorkflow(QuickFlow.UI.Controls.ActionEventArgs e)
        {
            Employee approver = WorkFlowUtil.GetEmployeeApprover(DataForm1.Applicant);
            if (approver == null)
            {
                DisplayMessage("Unable to find an approver for the applicant. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            List<string> list = WorkFlowUtil.UserListInGroup("wf_Reception");
            if (list.Count > 1)
            {
                DisplayMessage("Unable to submit the application. More than one receptionist is defined. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }

            string deptHead = approver.UserAccount;
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHead", deptHead);

            NameCollection names = new NameCollection();
            if (DataForm1.radiobuttonYes && DataForm1.RadioYes)
            {
                names.AddRange(list.ToArray());
                WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                WorkflowContext.Current.DataFields["IsTheSame"] = "yes";
            }
            else if (DataForm1.radiobuttonYes && DataForm1.RadioNo)
            {
                if (IsReceptionist(DataForm1.btApplicant1))
                {
                    names.AddRange(list.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "yes";
                }
                else
                {
                    names.AddRange(list.ToArray());
                    names.Add(DataForm1.btApplicant1);
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "no";
                }
            }
            else if (DataForm1.radiobuttonYes && DataForm1.RadioDisplay)
            {
                
                    names.AddRange(list.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "yes";
                
            }
            else if (DataForm1.radiobuttonNO && DataForm1.RadioYes)
            {
                if (IsReceptionist(DataForm1.btUser1))
                {
                    names.AddRange(list.ToArray());
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "yes";
                }
                else
                {
                    names.AddRange(list.ToArray());
                    names.Add(DataForm1.btUser1);
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "no";
                }
            }
            else if (DataForm1.radiobuttonNO && DataForm1.RadioNo)
            {
                if (DataForm1.btUser1.Equals(DataForm1.btApplicant1, StringComparison.CurrentCultureIgnoreCase))
                {
                    names.Add(DataForm1.btUser1);
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "yes";
                }
                else
                {
                    names.Add(DataForm1.btUser1);
                    names.Add(DataForm1.btApplicant1);
                    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                    WorkflowContext.Current.DataFields["IsTheSame"] = "no";
                }
            }
            else if (DataForm1.radiobuttonNO && DataForm1.RadioDisplay)
            {
                names.Add(DataForm1.btUser1);
                WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
                WorkflowContext.Current.DataFields["IsTheSame"] = "yes";
            }

            //List<string> strGroupUsers = WorkFlowUtil.UserListInGroup("wf_Reception");
            //NameCollection names = new NameCollection();
            //names.AddRange(strGroupUsers.ToArray());
            //WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistUsers", names);
            WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadTaskTitle", DataForm1.Applicant.DisplayName + "'s travel request needs approval");
            WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistTaskTitle", "Please complete travel request for " + DataForm1.Applicant.DisplayName);
            WorkflowContext.Current.DataFields["UserName"] = DataForm1.Applicant.UserAccount;
            WorkflowContext.Current.DataFields["EnglishName"] = DataForm1.Applicant.DisplayName;
            WorkflowContext.Current.DataFields["Department"] = DataForm1.Applicant.Department;
            WorkflowContext.Current.DataFields["JobTitle"] = DataForm1.Applicant.Title;
            WorkflowContext.Current.DataFields["AirNotes"] = DataForm1.AirNotes;
            WorkflowContext.Current.DataFields["HotelNotes"] = DataForm1.HotelNotes;

            bool bol = DataForm1.RadioYes;
            WorkflowContext.Current.DataFields["RadioYes"] = bol.ToString();

            if (DataForm1.radiobuttonNO)
            {
                WorkflowContext.Current.DataFields["btUser"] = DataForm1.btUser1;
            }
            else
            {
                WorkflowContext.Current.DataFields["btUser"] = string.Empty; 
            }

            if (DataForm1.RadioNo)
            {
                WorkflowContext.Current.DataFields["btApplicant"] = DataForm1.btApplicant1;
            }
            else
            {
                WorkflowContext.Current.DataFields["btApplicant"] = string.Empty; 
            }

            WorkflowContext.Current.DataFields["Status"] = "InProgress";
        }
        private bool IsReceptionist(string user)
        {
            QuickFlow.NameCollection receptionList = WorkFlowUtil.GetUsersInGroup("wf_Reception");

            foreach (string name in receptionList)
            {
                if (name.Equals(user, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }
        private void UpdateRecords()
        {
            DataForm1.Update();

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.TravelDetails.ToString());
            SPListItem item = null;

            // remove old records for the workflow
            WorkFlowUtil.RemoveExistingRecord(list, "RequestID", DataForm1.WorkflowNumber);

            DataTable dtTravelDetails = this.DataForm1.dtTravelDetails;
            foreach (DataRow dr in dtTravelDetails.Rows)
            {
                item = list.Items.Add();
                item["RequestID"] = DataForm1.WorkflowNumber;
                item["FromDate"] = dr["FromDate"];
                item["ToDate"] = dr["ToDate"];
                item["Departure"] = dr["Departure"];
                item["Destination"] = dr["Destination"];
                item["Vehicle"] = dr["Vehicle"];
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

            DataTable dtHotelInfo = this.DataForm1.dtHotelInfo;
            list = sps.GetList(CAWorkFlowConstants.ListName.TravelHotelInfo.ToString());
            // remove old records for the workflow
            WorkFlowUtil.RemoveExistingRecord(list, "RequestID", DataForm1.WorkflowNumber);
            foreach (DataRow dr in dtHotelInfo.Rows)
            {
                item = list.Items.Add();
                item["RequestID"] = DataForm1.WorkflowNumber;
                item["CheckInDate"] = dr["CheckInDate"];
                item["CheckOutDate"] = dr["CheckOutDate"];
                item["Nights"] = dr["Nights"];
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

            DataTable dtVehicle = this.DataForm1.dtVehicle;
            list = sps.GetList(CAWorkFlowConstants.ListName.TravelVehicleInfo.ToString());
            // remove old records for the workflow
            WorkFlowUtil.RemoveExistingRecord(list, "RequestID", DataForm1.WorkflowNumber);
            foreach (DataRow dr in dtVehicle.Rows)
            {
                item = list.Items.Add();
                item["RequestID"] = DataForm1.WorkflowNumber;
                item["Date"] = dr["Date"];
                item["Time"] = dr["Time"];
                item["VehicleNumber"] = dr["VehicleNumber"];
                item["VehicleFrom"] = dr["VehicleFrom"];
                item["VehicleTo"] = dr["VehicleTo"];
                item["Class"] = dr["Class"];
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
        //private string CheckIsCancel( string str )
        //{   if(str=="End")
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "Confirm('Are you sure you want to end this application? .');", true);
        //    return str;
        //}
    }
}
