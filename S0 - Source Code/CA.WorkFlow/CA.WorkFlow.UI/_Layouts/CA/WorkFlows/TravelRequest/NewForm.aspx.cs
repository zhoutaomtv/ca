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
using QuickFlow.UI.Controls;
using QuickFlow;
using CA.SharePoint.Utilities.Common;


namespace CA.WorkFlow.UI.TravelRequset
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
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartWorkflowButton btnStart = sender as StartWorkflowButton;


            if (string.Equals(btnStart.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);

                DataForm1.Update();
            }
            else
            {
                string msg = DataForm1.Validate();
                if (!string.IsNullOrEmpty(msg))
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }

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
                //验证组里用户是否为空
                if(list.Count==0)
                {
                    DisplayMessage("Unable to submit the application. There is no user in wf_Reception group. Please contact IT for further help.");
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
                WorkflowContext.Current.DataFields["Status"] = "InProgress";
            }

            this.WorkFlowNumber = CreateWorkFlowNumber();
            WorkflowContext.Current.DataFields["WorkflowNumber"] = this.WorkFlowNumber;
            WorkflowContext.Current.DataFields["UserName"] = DataForm1.Applicant.UserAccount;
            WorkflowContext.Current.DataFields["EnglishName"] = DataForm1.Applicant.DisplayName;
            WorkflowContext.Current.DataFields["Department"] = DataForm1.Applicant.Department;
            WorkflowContext.Current.DataFields["JobTitle"] = DataForm1.Applicant.Title;
            WorkflowContext.Current.DataFields["Status"] = "InProgress";
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
                WorkflowContext.Current.DataFields["btUser"] = string.Empty; ;
            }
            if (DataForm1.RadioNo)
            {
                WorkflowContext.Current.DataFields["btApplicant"] = DataForm1.btApplicant1;
            }
            else
            {
                WorkflowContext.Current.DataFields["btApplicant"] = string.Empty; ;
            }
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
        private string CreateWorkFlowNumber()
        {
            return "TR_" + WorkFlowUtil.CreateWorkFlowNumber("TravelRequest").ToString("000000");
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            DataForm1.Update();
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.TravelDetails.ToString());
            SPListItem item = null;

            DataTable dtTravelDetails = this.DataForm1.dtTravelDetails;
            foreach (DataRow dr in dtTravelDetails.Rows)
            {
                if ((string.IsNullOrEmpty(dr["FromDate"].ToString()))
                   && (string.IsNullOrEmpty(dr["ToDate"].ToString()))
                   && (string.IsNullOrEmpty(dr["Departure"].ToString()))
                   && (string.IsNullOrEmpty(dr["Destination"].ToString())))
                {
                    continue;
                }

                item = list.Items.Add();
                item["RequestID"] = this.WorkFlowNumber;
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
            foreach (DataRow dr in dtHotelInfo.Rows)
            {
                if ((string.IsNullOrEmpty(dr["CheckInDate"].ToString()))
                   && (string.IsNullOrEmpty(dr["CheckOutDate"].ToString()))
                   && (string.IsNullOrEmpty(dr["Nights"].ToString())))
                {
                    continue;
                }

                item = list.Items.Add();
                item["RequestID"] = this.WorkFlowNumber;
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
            foreach (DataRow dr in dtVehicle.Rows)
            {
                if ((string.IsNullOrEmpty(dr["Date"].ToString()))
                   && (string.IsNullOrEmpty(dr["Time"].ToString()))
                   && (string.IsNullOrEmpty(dr["VehicleNumber"].ToString()))
                   && (string.IsNullOrEmpty(dr["VehicleFrom"].ToString()))
                   && (string.IsNullOrEmpty(dr["VehicleTo"].ToString()))
                   && (string.IsNullOrEmpty(dr["Class"].ToString())))
                {
                    continue;
                }

                item = list.Items.Add();
                item["RequestID"] = this.WorkFlowNumber;
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
    }
}
