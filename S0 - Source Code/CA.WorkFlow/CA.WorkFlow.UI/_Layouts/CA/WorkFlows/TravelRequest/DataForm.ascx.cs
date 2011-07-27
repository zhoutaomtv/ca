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
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
//using CA.WorkFlow.UI.Code;

using CA.WorkFlow.UI;
using CodeArt.SharePoint.CamlQuery;
using System.Text;
using QuickFlow.Core;

namespace CA.WorkFlow.UI.TravelRequset
{
    public partial class DataForm : QFUserControl
    {
        private string currentAccount
        {
            get { return ViewState["currentAccount"] == null ? string.Empty : ViewState["currentAccount"].ToString(); }
            set { ViewState["currentAccount"] = value; }
        }
        
        public Employee Applicant
        {
            get { return ViewState["applicant"] == null ? null : (Employee)ViewState["applicant"]; }
            set { ViewState["applicant"] = value; }
        }

        public string WorkflowNumber
        {
            get
            {
                return lblWorkflowNumber.Text;
            }
        }
        public string AirNotes
        {
            get
            {
                return txtAirNotes.Text;
            }
        }
        public string HotelNotes
        {
            get
            {
                return txtHotelNotes.Text;
            }
        }
        public string Name
        {
            get
            {
                return this.lblEnglishName.Text;
            }
        }
        public string Department
        {
            get
            {
                return this.lblDepartment.Text;
            }
        }
        public string EstimateDays
        {
            get
            {
                return ffEstimateDays.Value.ToString();
            }
        }
        
        public string Purpose
        {
            get
            {
                return ffPurpose.Value.ToString();
            }
        }
        public bool radiobuttonYes
        {
            get 
            {
                return this.rbbookticketYes.Checked;
            }
        }
        public bool radiobuttonNO
        {
            get
            {
                return this.rbbookticketNo.Checked;
            }
        }
        public string btUser1
        {
            get
            {
                return btUser.Accounts.Count > 0 ? btUser.Accounts[0] + "" : string.Empty;
            }
        }
        public bool RadioYes
        {
            get
            {
                return this.RadioButtonYes.Checked;
            }
        }
        public bool RadioNo
        {
            get
            {
                return this.RadioButtonNo.Checked;
            }
        }
        public bool RadioDisplay
        {
            get
            {
                return this.RadioButtonDisplay.Checked;
            }
        }
        public string btApplicant1
        {
            get
            {
                return btApplicant.Accounts.Count > 0 ? btApplicant.Accounts[0] + "" : string.Empty;
            }
        }
       
        
        public DataTable dtTravelDetails
        {
            get
            {
                return ViewState["dtTravelDetails"] != null ? (DataTable)ViewState["dtTravelDetails"] : CreateTravelDetails();
            }
            set
            {
                ViewState["dtTravelDetails"] = value;
            }
        }

        public DataTable dtHotelInfo
        {
            get
            {
                return ViewState["dtHotelInfo"] != null ? (DataTable)ViewState["dtHotelInfo"] : CreateHotelInfo();
            }
            set
            {
                ViewState["dtHotelInfo"] = value;
            }
        }

        public DataTable dtVehicle
        {
            get
            {
                return ViewState["dtVehicle"] != null ? (DataTable)ViewState["dtVehicle"] : CreateTravelVehicleInfo();
            }
            set
            {
                ViewState["dtVehicle"] = value;
            }
        }

        //private bool _canEditUser = false;

        //public bool CanEditUser
        //{
        //    get
        //    {
        //        return _canEditUser;
        //    }
        //    set
        //    {
        //        _canEditUser = value;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Applicant = this.CurrentEmployee;
                if (this.ControlMode == SPControlMode.New)
                {
                    //this.formfieldEmployee.Value = SPContext.Current.Web.CurrentUser.LoginName;
                    this.lblWorkflowNumber.Text = "NA";
                    dtTravelDetails.Rows.Add();
                    rptTravelDetails.DataSource = dtTravelDetails;
                    rptTravelDetails.DataBind();
                    dtHotelInfo.Rows.Add();
                    rptHotelInformation.DataSource = dtHotelInfo;
                    rptHotelInformation.DataBind();
                    dtVehicle.Rows.Add();
                    rptVehicleInformation.DataSource = dtVehicle;
                    rptVehicleInformation.DataBind();
                    //CADateTimeStartDate.SelectedDate = DateTime.Now;
                    //CADateTimeEndDate.SelectedDate = DateTime.Now;
                    //CADateTimeCheckInDate.SelectedDate = DateTime.Now;
                    //CADateTimeCheckOutDate.SelectedDate = DateTime.Now;
                    //CADateTimeVehicleDate.SelectedDate = DateTime.Now;
                }
                else
                {
                    FillData();
                }
                FillEmployeeData(this.Applicant);
            }
            else
            {
                if ((this.ControlMode == SPControlMode.New) || (this.ControlMode == SPControlMode.Edit))
                {
                    this.cpfUser.Load += new EventHandler(cpfUser_Load);

                    if (rbbookticketNo.Checked)
                    {
                        btUser.Enabled = true;
                       // btUser.Visible = true;
                    }
                    if (RadioButtonNo.Checked)
                    {
                        btApplicant.Enabled = true;
                       // btApplicant.Visible = true;
                    }
                }
                else if (!Applicant.UserAccount.Equals(this.CurrentEmployee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    cpfUser.CommaSeparatedAccounts = Applicant.UserAccount;
                    currentAccount = cpfUser.Accounts[0] + "";
                    if (rbbookticketNo.Checked)
                    { 
                        btUser .CommaSeparatedAccounts = SPContext.Current.ListItem["btUser"] + "";
                        btUser.Enabled = false;
                        
                      //  btUser.Visible = true;
                    
                    }
                    if(RadioButtonNo.Checked)
                    {
                        btApplicant.CommaSeparatedAccounts = SPContext.Current.ListItem["btApplicant"] + "";
                        btApplicant.Enabled = false;
                       // btApplicant.Visible = true;
                    }
                }
                FillAssistants();
            }
            //if (!radiobuttonNO)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "disablebtuser", "disablebtuser();", true);
            //}
            //if (!RadioNo)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "disableapplicant", "disableapplicant();", true);
            //}
        }

        private void FillData()
        {
            lblEnglishName.Text = SPContext.Current.ListItem["EnglishName"] + "";
            lblDepartment.Text = SPContext.Current.ListItem["Department"] + "";
            lblJobTitle.Text = SPContext.Current.ListItem["JobTitle"] + "";
            lblWorkflowNumber.Text = SPContext.Current.ListItem["WorkflowNumber"] + "";
            if (!string.IsNullOrEmpty(SPContext.Current.ListItem["UserName"] + ""))
                this.Applicant = UserProfileUtil.GetEmployee(SPContext.Current.ListItem["UserName"] + "");

            if (!string.IsNullOrEmpty(SPContext.Current.ListItem["btUser"] + ""))
            {
                btUser.CommaSeparatedAccounts = SPContext.Current.ListItem["btUser"] + "";
                rbbookticketYes.Checked = false;
                rbbookticketNo.Checked = true;
                if (this.ControlMode == SPControlMode.Edit)
                {
                    btUser.Enabled = true;
                   // btUser.Visible = true;
                }
                else
                {
                    btUser.Enabled = false;
                   // btUser.Visible = true;
                }
            }
            txtAirNotes.Text=SPContext.Current.ListItem["AirNotes"]+"";
            txtHotelNotes.Text=SPContext.Current.ListItem["HotelNotes"]+"";

            if (!string.IsNullOrEmpty(SPContext.Current.ListItem["btApplicant"] + ""))
            {
                btApplicant.CommaSeparatedAccounts = SPContext.Current.ListItem["btApplicant"] + "";
                RadioButtonDisplay.Checked = false;
                RadioButtonNo.Checked = true;
                RadioButtonYes.Checked = false;
                if (this.ControlMode == SPControlMode.Edit)
                {
                    btApplicant.Enabled = true;
                  //  btApplicant.Visible = true;

                }
                else
                {
                    btApplicant.Enabled = false;
                    //btApplicant.Visible = true;
                }

            }
      
            if (SPContext.Current.ListItem["RadioYes"] + "" == "True")
            {
                RadioButtonYes.Checked = true;
                RadioButtonDisplay.Checked = false;
            }
           
            GetTravelDetails();
            GetHotelInfo();
            GetVehicle();
        }
        private void FillAssistants()
        {
            if ((btUser.Accounts.Count > 0) || (btApplicant.Accounts.Count > 0))
                return;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList TRUserAssistant = sps.GetList(CAWorkFlowConstants.ListName.UserAssistant.ToString());
            foreach (SPListItem item in TRUserAssistant.Items)
            {
                SPFieldUserValue value = new SPFieldUserValue(item.Web, item["Applicant"] as string);
                if (value.User.LoginName.Equals(this.Applicant.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = new SPFieldUserValue(item.Web, item["Assistant"] as string);
                    this.btUser.CommaSeparatedAccounts = value.User.Name;
                    this.btApplicant.CommaSeparatedAccounts = value.User.Name;
                    return;
                }
            }

            this.btUser.CommaSeparatedAccounts = string.Empty;
            this.btApplicant.CommaSeparatedAccounts = string.Empty;
        }
        /// <summary>
        /// 用来处理选人事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cpfUser_Load(object sender, EventArgs e)
        {
            if (this.cpfUser.Accounts.Count > 0)
            {
                string userAccount = this.cpfUser.Accounts[0] + "";
                if (!userAccount.Equals(currentAccount))
                {
                    currentAccount = userAccount;
                    this.Applicant = UserProfileUtil.GetEmployee(userAccount);
                    this.btUser.CommaSeparatedAccounts = string.Empty;
                    this.btApplicant.CommaSeparatedAccounts = string.Empty;
                }
            }

            FillEmployeeData(this.Applicant);
            //  FillBalanceData();
        }

        private void FillEmployeeData(Employee employee)
        {
            this.lblEnglishName.Text = employee.DisplayName;
            this.lblDepartment.Text = employee.Department;
            this.lblJobTitle.Text = employee.Title;

            if (!Applicant.UserAccount.Equals(this.CurrentEmployee.UserAccount, StringComparison.CurrentCultureIgnoreCase))
            {
                cpfUser.CommaSeparatedAccounts = Applicant.UserAccount;
                currentAccount = cpfUser.Accounts[0] + "";
            }

            FillAssistants();
            //if(rbbookticketNo.Checked)
            //{
            //    btUser.CommaSeparatedAccounts= SPContext.Current.ListItem["btUser"] + "";
            //}
        }

        public override void SetControlMode()
        {
            base.SetControlMode();

            if ((ControlMode == SPControlMode.New) || (ControlMode == SPControlMode.Edit))
            {
                cpfUser.Enabled = true;
                rbbookticketNo.Enabled = true;
                rbbookticketYes.Enabled = true;
                RadioButtonNo.Enabled = true;
                RadioButtonYes.Enabled = true;
                RadioButtonDisplay.Enabled = true;
            }
            else
            {
                cpfUser.Enabled = false;
                rbbookticketNo.Enabled = false;
                rbbookticketYes.Enabled = false;
                RadioButtonYes.Enabled = false;
                RadioButtonNo.Enabled = false;
                RadioButtonDisplay.Enabled = false;
                txtAirNotes.ReadOnly = true;
                txtHotelNotes.ReadOnly = true;
            }

            if (ControlMode == SPControlMode.Display)
            {
                rptTravelDetails.Visible = false;
                rptHotelInformation.Visible = false;
                rptVehicleInformation.Visible = false;
                rptHotelInformationDisplay.Visible = true;
                rptTravelDetailsDisplay.Visible = true;
                rptVehicleInformationDisplay.Visible = true;
                btnAddDetail.Visible = false;
                btnAddHotel.Visible = false;
                btnAddVehicle.Visible = false;
                ShowMsg();

            }
            else
            {
                rptTravelDetails.Visible = true;
                rptHotelInformation.Visible = true;
                rptVehicleInformation.Visible = true;
                rptHotelInformationDisplay.Visible = false;
                rptTravelDetailsDisplay.Visible = false;
                rptVehicleInformationDisplay.Visible = false;
                btnAddDetail.Visible = true;
                btnAddHotel.Visible = true;
                btnAddVehicle.Visible = true;
            }
        }

        private void ShowMsg()
        {
            // no need to show message if not the last step.
            if (WorkflowContext.Current.Task!=null&& WorkflowContext.Current.Task.Step != "ReceptionistTask")
                return;

            string currentUser = SPContext.Current.Web.CurrentUser.LoginName;
            string airUser = (SPContext.Current.ListItem["btUser"]+"").ToString();
            string hotelUser = (SPContext.Current.ListItem["btApplicant"]+"").ToString();
            string hotelReceptionist = SPContext.Current.ListItem["RadioYes"].ToString();

            // no need to show message for users other than receptionist and users book tickets
            if (!IsReceptionist(currentUser) 
                && !currentUser.Equals(airUser, StringComparison.CurrentCultureIgnoreCase) 
                && !currentUser.Equals(hotelUser, StringComparison.CurrentCultureIgnoreCase))
                return;

            // no need to reserve hotel
            if (string.IsNullOrEmpty(hotelUser) && hotelReceptionist.Equals("false", StringComparison.CurrentCultureIgnoreCase))
            {
                lblMessage.Text += "You are only responsible for booking the air ticket.";
                tblMsg.Visible = true;
                return;
            }

            // receptionist does both air ticket and hotel
            if (string.IsNullOrEmpty(hotelUser) && string.IsNullOrEmpty(airUser) && hotelReceptionist.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                lblMessage.Text += "You are responsible for booking the air ticket and making the hotel reservation.";
                tblMsg.Visible = true;
                return;
            }

            // assistant does both air ticket and hotel
            if (!string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser) && airUser.Equals(hotelUser, StringComparison.CurrentCultureIgnoreCase))
            {
                lblMessage.Text += "You are responsible for booking the air ticket and making the hotel reservation.";
                tblMsg.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser) 
                && !airUser.Equals(hotelUser, StringComparison.CurrentCultureIgnoreCase)
                && airUser.Equals(currentUser, StringComparison.CurrentCultureIgnoreCase))
            {
                lblMessage.Text += "You are responsible for booking the air ticket.";
                tblMsg.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser)
                && !airUser.Equals(hotelUser, StringComparison.CurrentCultureIgnoreCase)
                && hotelUser.Equals(currentUser, StringComparison.CurrentCultureIgnoreCase))
            {
                lblMessage.Text += "You are responsible for making the hotel reservation.";
                tblMsg.Visible = true;
                return;
            }

            // receptionist books air ticket and assistant reserves hotel
            if (string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser))
            {
                // assistant is receptionist
                if (IsReceptionist(hotelUser))
                {
                    lblMessage.Text += "You are responsible for booking the air ticket and making the hotel reservation.";
                    tblMsg.Visible = true;
                    return;
                }
                else
                {
                    if (IsReceptionist(currentUser))    // current user is receptionist
                    {
                        lblMessage.Text += "You are only responsible for booking the air ticket.";
                        tblMsg.Visible = true;
                        return;
                    }
                    else                               // current user is assistant
                    {
                        lblMessage.Text += "You are only responsible for making the hotel reservation.";
                        tblMsg.Visible = true;
                        return;
                    }
                }
            }

            // assistant books air ticket and receptionist reserves hotel
            if (!string.IsNullOrEmpty(airUser) && string.IsNullOrEmpty(hotelUser) && hotelReceptionist.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                if (IsReceptionist(airUser))
                {
                    lblMessage.Text += "You are responsible for booking the air ticket and making the hotel reservation.";
                    tblMsg.Visible = true;
                    return;
                }
                else
                {
                    if (IsReceptionist(currentUser))
                    {
                        lblMessage.Text += "You are only responsible for making the hotel reservation.";
                        tblMsg.Visible = true;
                        return;
                    }
                    else
                    {
                        lblMessage.Text += "You are only responsible for booking the air ticket.";
                        tblMsg.Visible = true;
                        return;
                    }
                }
            }

            ////Receptionist air ticket and Receptionist hotel reservation
            //if (string.IsNullOrEmpty(airUser) && hotelReceptionist.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    lblMessage.Text += "You are responsible for booking the air ticket and making hotel reservation.";
            //    tblMsg.Visible = true;
            //    return;

            //}

            ////Receptionist air ticket and supply hotel user
            //if (string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser) && currentUser.Equals(hotelUser,StringComparison.CurrentCultureIgnoreCase))
            //{
            //    lblMessage.Text += "You are responsible for making hotel reservation.";
            //    tblMsg.Visible = true;
            //    return;

            //}
            //else if (string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser)&&IsReceptionist(currentUser) )

            //{
            //    lblMessage.Text += "You are responsible for booking the air ticket.";
            //    tblMsg.Visible = true;
            //    return;

            //}

            ////Receptionist air ticket and applicant 
            //if (string.IsNullOrEmpty(hotelUser)&& hotelReceptionist.Equals("false", StringComparison.CurrentCultureIgnoreCase)&&string.IsNullOrEmpty(airUser)&&currentUser==receptionList[0])
            //{
            //    lblMessage.Text += "You are responsible for booking the air ticket.";
            //    tblMsg.Visible = true;
            //    return;
            //}
            //else
            //   if (string.IsNullOrEmpty(hotelUser) && hotelReceptionist.Equals("false", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrEmpty(airUser))
            //    {
            //      lblMessage.Text += "You are responsible for making hotel reservation.";
            //      tblMsg.Visible = true;
            //      return;
            //    }
            //// suppliy air ticket user and applicant
            //if (string.IsNullOrEmpty(hotelUser) && hotelReceptionist.Equals("false", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(airUser) && currentUser.Equals(airUser,StringComparison.CurrentCultureIgnoreCase))
            //{
            //    lblMessage.Text += "You are responsible for booking the air ticket.";
            //    tblMsg.Visible = true;
            //    return;
            //}
            //else
            //    if (string.IsNullOrEmpty(hotelUser) && hotelReceptionist.Equals("false", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(airUser))
            //    {
            //        lblMessage.Text += "You are responsible for making hotel reservation.";
            //        tblMsg.Visible = true;
            //        return;
            //    }
            ////suppliy air ticket user and Receptionist hotel reservation
            // if (!string.IsNullOrEmpty(airUser) && hotelReceptionist.Equals("true", StringComparison.CurrentCultureIgnoreCase) && currentUser.Equals(airUser, StringComparison.CurrentCultureIgnoreCase))
            // {
            //     lblMessage.Text += "You are responsible for booking the air ticket.";
            //     tblMsg.Visible = true;
            //     return;
            // }
            // else
            //     if (!string.IsNullOrEmpty(airUser) && hotelReceptionist.Equals("true", StringComparison.CurrentCultureIgnoreCase) && currentUser==receptionList[0])
            // {
            //     lblMessage.Text = "You are responsible for making hotel reservation.";
            //     tblMsg.Visible = true;
            //     return;
            // }
            ////suppliy air ticket user and suppliy hotel user
            // if (!string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser) && airUser.Equals(hotelUser, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    lblMessage.Text += "You are responsible for booking the air ticket and making hotel reservation.";
            //    tblMsg.Visible = true;
            //    return;
            // }
            // else
            //     if (!string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser) && currentUser.Equals(airUser, StringComparison.CurrentCultureIgnoreCase))
            //     {
            //         lblMessage.Text += "You are responsible for booking the air ticket";
            //         tblMsg.Visible = true;
            //         return;
            //     }
            //     else
            //         if (!string.IsNullOrEmpty(airUser) && !string.IsNullOrEmpty(hotelUser) && currentUser.Equals(hotelUser, StringComparison.CurrentCultureIgnoreCase))
            //         {
            //         lblMessage.Text += "You are responsible for making hotel reservation.";
            //         tblMsg.Visible = true;
            //         return;
            //     }
            
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

        //  具体行程 Travel Details
        private DataTable CreateTravelDetails()
        {
            dtTravelDetails = new DataTable();
            dtTravelDetails.Columns.Add("RequestID");
            dtTravelDetails.Columns.Add("FromDate");
            dtTravelDetails.Columns.Add("ToDate");
            dtTravelDetails.Columns.Add("Departure");
            dtTravelDetails.Columns.Add("Destination");
            dtTravelDetails.Columns.Add("Vehicle");

            return dtTravelDetails;
        }

        private void GetTravelDetails()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.TravelDetails + "");

            QueryField field = new QueryField("RequestID", false);

            SPListItemCollection items = sps.Query(list, field.Equal(string.IsNullOrEmpty(lblWorkflowNumber.Text) ? string.Empty : lblWorkflowNumber.Text), 0);

            foreach (SPListItem item in items)
            {
                DataRow row = dtTravelDetails.Rows.Add();
                row["RequestID"] = item["RequestID"];
                row["FromDate"] = item["FromDate"];
                row["ToDate"] = item["ToDate"];
                row["Departure"] = item["Departure"];
                row["Destination"] = item["Destination"];
                row["Vehicle"] = item["Vehicle"];
            }

            if (rptTravelDetails.Visible)
            {
                if (dtTravelDetails.Rows.Count == 0)
                {
                    dtTravelDetails.Rows.Add();
                }

                this.rptTravelDetails.DataSource = dtTravelDetails;
                this.rptTravelDetails.DataBind();
            }
            else if (rptTravelDetailsDisplay.Visible)
            {
                this.rptTravelDetailsDisplay.DataSource = dtTravelDetails;
                this.rptTravelDetailsDisplay.DataBind();
            }
        }

        protected void btnAddDetail_Click(object sender, ImageClickEventArgs e)
        {
            if (rptTravelDetails.Items.Count > 0)
            {
                dtTravelDetails.Rows.Clear();

                foreach (RepeaterItem item in rptTravelDetails.Items)
                {

                    CADateTimeControl caStartDate = (CADateTimeControl)item.FindControl("CADateTimeStartDate");
                    CADateTimeControl caEndDate = (CADateTimeControl)item.FindControl("CADateTimeEndDate");
                    TextBox txtDeparture = (TextBox)item.FindControl("txtDeparture");
                    TextBox txtDestination = (TextBox)item.FindControl("txtDestination");
                    DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");

                    DataRow row = dtTravelDetails.Rows.Add();
                    row["FromDate"] = caStartDate.IsDateEmpty ? string.Empty : caStartDate.SelectedDate.ToShortDateString();
                    row["ToDate"] = caEndDate.IsDateEmpty ? string.Empty : caEndDate.SelectedDate.ToShortDateString();
                    row["Departure"] = txtDeparture.Text;
                    row["Destination"] = txtDestination.Text;
                    row["Vehicle"] = ddlVehicle.SelectedValue;
                }
            }
            dtTravelDetails.Rows.Add();
            this.rptTravelDetails.DataSource = dtTravelDetails;
            this.rptTravelDetails.DataBind();
        }

        protected void rptTravelDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                updateTravelDetails();
                dtTravelDetails.Rows.Remove(dtTravelDetails.Rows[e.Item.ItemIndex]);
                this.rptTravelDetails.DataSource = dtTravelDetails;
                this.rptTravelDetails.DataBind();
            }
        }

        protected void rptTravelDetails_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                CADateTimeControl caDateTime = (CADateTimeControl)e.Item.FindControl("CADateTimeStartDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["FromDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["FromDate"].ToString());
                caDateTime = (CADateTimeControl)e.Item.FindControl("CADateTimeEndDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["ToDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["ToDate"].ToString());
                TextBox txtBox = (TextBox)e.Item.FindControl("txtDeparture");
                txtBox.Text = row["Departure"].ToString();
                txtBox = (TextBox)e.Item.FindControl("txtDestination");
                txtBox.Text = row["Destination"].ToString();
                DropDownList ddlVehicle = (DropDownList)e.Item.FindControl("ddlVehicle");
                ddlVehicle.SelectedValue = row["Vehicle"].ToString();
            }
        }

        //  Hotel 酒店信息
        private DataTable CreateHotelInfo()
        {
            dtHotelInfo = new DataTable();
            dtHotelInfo.Columns.Add("RequestID");
            dtHotelInfo.Columns.Add("CheckInDate");
            dtHotelInfo.Columns.Add("CheckOutDate");
            dtHotelInfo.Columns.Add("Nights");
            return dtHotelInfo;
        }
        private void GetHotelInfo()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.TravelHotelInfo + "");

            QueryField field = new QueryField("RequestID", false);

            SPListItemCollection items = sps.Query(list, field.Equal(string.IsNullOrEmpty(lblWorkflowNumber.Text) ? string.Empty : lblWorkflowNumber.Text), 0);

            foreach (SPListItem item in items)
            {
                DataRow row = dtHotelInfo.Rows.Add();
                row["RequestID"] = item["RequestID"];
                row["CheckInDate"] = item["CheckInDate"];
                row["CheckOutDate"] = item["CheckOutDate"];
                row["Nights"] = item["Nights"];

            }

            if (rptHotelInformation.Visible)
            {
                if (dtHotelInfo.Rows.Count == 0)
                {
                    dtHotelInfo.Rows.Add();
                }

                this.rptHotelInformation.DataSource = dtHotelInfo;
                this.rptHotelInformation.DataBind();
            }
            else if (rptHotelInformationDisplay.Visible)
            {
                this.rptHotelInformationDisplay.DataSource = dtHotelInfo;
                this.rptHotelInformationDisplay.DataBind();
            }
        }

        protected void rptHotelInformation_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                updateHotelInfo();
                dtHotelInfo.Rows.Remove(dtHotelInfo.Rows[e.Item.ItemIndex]);
                this.rptHotelInformation.DataSource = dtHotelInfo;
                this.rptHotelInformation.DataBind();
            }
        }

        protected void rptHotelInformation_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                CADateTimeControl caDateTime = (CADateTimeControl)e.Item.FindControl("CADateTimeCheckInDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["CheckInDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["CheckInDate"].ToString());
                caDateTime = (CADateTimeControl)e.Item.FindControl("CADateTimeCheckOutDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["CheckOutDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["CheckOutDate"].ToString());
                TextBox txtNights = (TextBox)e.Item.FindControl("txtNights");
                txtNights.Text = row["Nights"].ToString();
            }
        }

        protected void btnAddHotel_Click(object sender, ImageClickEventArgs e)
        {
            if (rptHotelInformation.Items.Count > 0)
            {
                dtHotelInfo.Rows.Clear();

                foreach (RepeaterItem item in rptHotelInformation.Items)
                {

                    CADateTimeControl caCheckInDate = (CADateTimeControl)item.FindControl("CADateTimeCheckInDate");
                    CADateTimeControl caCheckOutDate = (CADateTimeControl)item.FindControl("CADateTimeCheckOutDate");
                    TextBox txtNights = (TextBox)item.FindControl("txtNights");

                    DataRow row = dtHotelInfo.Rows.Add();
                    row["CheckInDate"] = caCheckInDate.IsDateEmpty ? string.Empty : caCheckInDate.SelectedDate.ToShortDateString();
                    row["CheckOutDate"] = caCheckOutDate.IsDateEmpty ? string.Empty : caCheckOutDate.SelectedDate.ToShortDateString();
                    row["Nights"] = txtNights.Text;
                }
            }
            dtHotelInfo.Rows.Add();
            this.rptHotelInformation.DataSource = dtHotelInfo;
            this.rptHotelInformation.DataBind();
        }

        //  Vehicle 交通信息
        private DataTable CreateTravelVehicleInfo()
        {
            dtVehicle = new DataTable();
            dtVehicle.Columns.Add("RequestID");
            dtVehicle.Columns.Add("Date");
            dtVehicle.Columns.Add("Time");
            dtVehicle.Columns.Add("VehicleNumber");
            dtVehicle.Columns.Add("VehicleFrom");
            dtVehicle.Columns.Add("VehicleTo");
            dtVehicle.Columns.Add("Class");

            return dtVehicle;
        }

        private void GetVehicle()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.TravelVehicleInfo + "");

            QueryField field = new QueryField("RequestID", false);

            SPListItemCollection items = sps.Query(list, field.Equal(string.IsNullOrEmpty(lblWorkflowNumber.Text) ? string.Empty : lblWorkflowNumber.Text), 0);

            foreach (SPListItem item in items)
            {
                DataRow row = dtVehicle.Rows.Add();
                row["RequestID"] = item["RequestID"];
                row["Date"] = item["Date"];
                row["Time"] = item["Time"];
                row["VehicleNumber"] = item["VehicleNumber"];
                row["VehicleFrom"] = item["VehicleFrom"];
                row["VehicleTo"] = item["VehicleTo"];
                row["Class"] = item["Class"];
            }

            if (rptVehicleInformation.Visible)
            {
                if (dtVehicle.Rows.Count == 0)
                {
                    dtVehicle.Rows.Add();
                }

                this.rptVehicleInformation.DataSource = dtVehicle;
                this.rptVehicleInformation.DataBind();
            }
            else if (rptVehicleInformationDisplay.Visible)
            {
                this.rptVehicleInformationDisplay.DataSource = dtVehicle;
                this.rptVehicleInformationDisplay.DataBind();
            }
        }

        protected void rptVehicleInformation_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                updateVehicleInfo();
                dtVehicle.Rows.Remove(dtVehicle.Rows[e.Item.ItemIndex]);
                this.rptVehicleInformation.DataSource = dtVehicle;
                this.rptVehicleInformation.DataBind();
            }
        }

        protected void rptVehicleInformation_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                CADateTimeControl caDateTime = (CADateTimeControl)e.Item.FindControl("CADateTimeVehicleDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["Date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["Date"].ToString());
                TextBox txtBox = (TextBox)e.Item.FindControl("txtTime");
                txtBox.Text = row["Time"].ToString();
                txtBox = (TextBox)e.Item.FindControl("txtVehicleNum");
                txtBox.Text = row["VehicleNumber"].ToString();
                txtBox = (TextBox)e.Item.FindControl("txtFrom");
                txtBox.Text = row["VehicleFrom"].ToString();
                txtBox = (TextBox)e.Item.FindControl("txtTo");
                txtBox.Text = row["VehicleTo"].ToString();
                txtBox = (TextBox)e.Item.FindControl("txtClass");
                txtBox.Text = row["Class"].ToString();
            }
        }

        protected void btnAddVehicle_Click(object sender, ImageClickEventArgs e)
        {
            if (rptVehicleInformation.Items.Count > 0)
            {
                dtVehicle.Rows.Clear();

                foreach (RepeaterItem item in rptVehicleInformation.Items)
                {

                    CADateTimeControl caVehicleDate = (CADateTimeControl)item.FindControl("CADateTimeVehicleDate");
                    TextBox txtTime = (TextBox)item.FindControl("txtTime");
                    TextBox txtVehicleNum = (TextBox)item.FindControl("txtVehicleNum");
                    TextBox txtFrom = (TextBox)item.FindControl("txtFrom");
                    TextBox txtTo = (TextBox)item.FindControl("txtTo");
                    TextBox txtClass = (TextBox)item.FindControl("txtClass");

                    DataRow row = dtVehicle.Rows.Add();
                    row["Date"] = caVehicleDate.IsDateEmpty ? string.Empty : caVehicleDate.SelectedDate.ToShortDateString();
                    row["Time"] = txtTime.Text;
                    row["VehicleNumber"] = txtVehicleNum.Text;
                    row["VehicleFrom"] = txtFrom.Text;
                    row["VehicleTo"] = txtTo.Text;
                    row["Class"] = txtClass.Text;
                }
            }

            dtVehicle.Rows.Add();
            this.rptVehicleInformation.DataSource = dtVehicle;
            this.rptVehicleInformation.DataBind();
        }

        //is cancel workflow
        public string CheckIsCancel(string str)
        {
            if (str == "End")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "Confirm('Are you sure you want to end this application? .');", true);
            return str;
        }

        // Validation  验证
        public string Validate()
        {
            string status = string.Empty;

            status += validateGeneralInfo();
            status += validateTravelDetails();
            status += validateVehicleInfo();
            status += validateHotelInfo();
            status += validateApprover();

            return status;
        }

        private string validateGeneralInfo()
        {
            string status = string.Empty;

            if (string.IsNullOrEmpty(ffChineseName.Value.ToString()))
                status += "Please supply a chinese name.\\n";
            if (string.IsNullOrEmpty(ffPinyin.Value.ToString()))
                status += "Please supply a pinyin name.\\n";
            if (string.IsNullOrEmpty(ffIDNumber.Value.ToString()))
                status += "Please supply a ID number.\\n";
            if (ffEstimateDays.Value == null)
                status += "Please supply an estimate days.\\n";
            if (rbbookticketNo.Checked)
            {
                if (btUser.Accounts.Count <= 0)
                    status += "Please supply the name for booking air ticket.\\n";
            }
            if(RadioButtonNo.Checked)
            {
                if(btApplicant.Accounts.Count <=0)
                    status += "Please supply the name for making hotel reservation.\\n";

            }

            return status;
        }

        private string validateTravelDetails()
        {
            string status = string.Empty;

            if (rptTravelDetails.Items.Count == 0)
            {
                status = "Please supply valid travel details.\\n";
            }

            for (int i = 0; i < rptTravelDetails.Items.Count; i++)
            {
                RepeaterItem item = rptTravelDetails.Items[i];

                CADateTimeControl caStartDate = (CADateTimeControl)item.FindControl("CADateTimeStartDate");
                CADateTimeControl caEndDate = (CADateTimeControl)item.FindControl("CADateTimeEndDate");
                TextBox txtDeparture = (TextBox)item.FindControl("txtDeparture");
                TextBox txtDestination = (TextBox)item.FindControl("txtDestination");
                DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");

                if ((caStartDate.IsDateEmpty)
                      || (caEndDate.IsDateEmpty)
                      || (string.IsNullOrEmpty(txtDeparture.Text))
                      || (string.IsNullOrEmpty(txtDestination.Text)))
                {
                    status = "Please supply valid travel details.\\n";
                    break;
                }

            }
            return status;
        }

        private string validateHotelInfo()
        {
            string status = string.Empty;

            //if (rptHotelInformation.Items.Count == 0)
            //{
            //    status = "Please supply valid hotel info.\\n";
            //}

            for (int i = 0; i < rptHotelInformation.Items.Count; i++)
            {
                RepeaterItem item = rptHotelInformation.Items[i];

                CADateTimeControl caCheckInDate = (CADateTimeControl)item.FindControl("CADateTimeCheckInDate");
                CADateTimeControl caCheckOutDate = (CADateTimeControl)item.FindControl("CADateTimeCheckOutDate");
                TextBox txtNights = (TextBox)item.FindControl("txtNights");

                //if ((caCheckInDate.IsDateEmpty)
                //   || (caCheckOutDate.IsDateEmpty)
                //   || (string.IsNullOrEmpty(txtNights.Text)))
                //{
                //    status = "Please supply valid hotel info.\\n";
                //    break;
                //}

            }
            return status;
        }

        private string validateVehicleInfo()
        {
            string status = string.Empty;

            for (int i = 0; i < rptVehicleInformation.Items.Count; i++)
            {
                RepeaterItem item = rptVehicleInformation.Items[i];

                CADateTimeControl caVehicleDate = (CADateTimeControl)item.FindControl("CADateTimeVehicleDate");
                TextBox txtTime = (TextBox)item.FindControl("txtTime");
                TextBox txtVehicleNum = (TextBox)item.FindControl("txtVehicleNum");
                TextBox txtFrom = (TextBox)item.FindControl("txtFrom");
                TextBox txtTo = (TextBox)item.FindControl("txtTo");
                TextBox txtClass = (TextBox)item.FindControl("txtClass");

            }
            return status;
        }

        private string validateApprover()
        {
            Employee approver = WorkFlowUtil.GetEmployeeApprover(Applicant);
            if (approver == null)
            {
                return "Unable to find an approver for the applicant. Please contact IT for further help.";
            }
            else
            {
                return string.Empty;
            }
        }

        public void Update()
        {


            updateGeneralInfo();
            updateTravelDetails();
            updateVehicleInfo();
            updateHotelInfo();


        }

        private void updateGeneralInfo()
        {



        }

        private void updateTravelDetails()
        {
            dtTravelDetails.Rows.Clear();

            for (int i = 0; i < rptTravelDetails.Items.Count; i++)
            {
                RepeaterItem item = rptTravelDetails.Items[i];

                CADateTimeControl caStartDate = (CADateTimeControl)item.FindControl("CADateTimeStartDate");
                CADateTimeControl caEndDate = (CADateTimeControl)item.FindControl("CADateTimeEndDate");
                TextBox txtDeparture = (TextBox)item.FindControl("txtDeparture");
                TextBox txtDestination = (TextBox)item.FindControl("txtDestination");
                DropDownList ddlVehicle = (DropDownList)item.FindControl("ddlVehicle");



                DataRow row = dtTravelDetails.Rows.Add();
                row["FromDate"] = caStartDate.IsDateEmpty ? string.Empty : caStartDate.SelectedDate.ToShortDateString();
                row["ToDate"] = caEndDate.IsDateEmpty ? string.Empty : caEndDate.SelectedDate.ToShortDateString();
                row["Departure"] = txtDeparture.Text;
                row["Destination"] = txtDestination.Text;
                row["Vehicle"] = ddlVehicle.SelectedValue;
            }

        }

        private void updateHotelInfo()
        {
            dtHotelInfo.Rows.Clear();

            for (int i = 0; i < rptHotelInformation.Items.Count; i++)
            {
                RepeaterItem item = rptHotelInformation.Items[i];

                CADateTimeControl caCheckInDate = (CADateTimeControl)item.FindControl("CADateTimeCheckInDate");
                CADateTimeControl caCheckOutDate = (CADateTimeControl)item.FindControl("CADateTimeCheckOutDate");
                TextBox txtNights = (TextBox)item.FindControl("txtNights");

                DataRow row = dtHotelInfo.Rows.Add();
                row["CheckInDate"] = caCheckInDate.IsDateEmpty ? string.Empty : caCheckInDate.SelectedDate.ToShortDateString();
                row["CheckOutDate"] = caCheckOutDate.IsDateEmpty ? string.Empty : caCheckOutDate.SelectedDate.ToShortDateString();
                row["Nights"] = txtNights.Text;
            }
        }

        private void updateVehicleInfo()
        {
            dtVehicle.Rows.Clear();

            for (int i = 0; i < rptVehicleInformation.Items.Count; i++)
            {
                RepeaterItem item = rptVehicleInformation.Items[i];

                CADateTimeControl caVehicleDate = (CADateTimeControl)item.FindControl("CADateTimeVehicleDate");
                TextBox txtTime = (TextBox)item.FindControl("txtTime");
                TextBox txtVehicleNum = (TextBox)item.FindControl("txtVehicleNum");
                TextBox txtFrom = (TextBox)item.FindControl("txtFrom");
                TextBox txtTo = (TextBox)item.FindControl("txtTo");
                TextBox txtClass = (TextBox)item.FindControl("txtClass");

                DataRow row = dtVehicle.Rows.Add();
                row["Date"] = caVehicleDate.IsDateEmpty ? string.Empty : caVehicleDate.SelectedDate.ToShortDateString();
                row["Time"] = txtTime.Text;
                row["VehicleNumber"] = txtVehicleNum.Text;
                row["VehicleFrom"] = txtFrom.Text;
                row["VehicleTo"] = txtTo.Text;
                row["Class"] = txtClass.Text;
            }
        }

        protected void rbbookticketNo_CheckedChanged(object sender, EventArgs e)
        {
            //ISharePointService sps = ServiceFactory.GetSharePointService(true);
            ////获取列表
            //SPList TRUserAssistant = sps.GetList(CAWorkFlowConstants.ListName.UserAssistant.ToString());
            //btUser.Enabled = true;
            //// btUser.Visible = true;
            //if (!string.IsNullOrEmpty(SPContext.Current.ListItem["btUser"] + ""))
            //{
            //    btUser.CommaSeparatedAccounts = SPContext.Current.ListItem["btUser"] + "";
            //}
            //else
            //{
            //    foreach (SPListItem item in TRUserAssistant.Items)
            //    {
            //        SPFieldUserValue value = new SPFieldUserValue(item.Web, item["Applicant"] as string);
            //        if (value.User.LoginName.Equals(this.Applicant.UserAccount, StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            value = new SPFieldUserValue(item.Web, item["Assistant"] as string);
            //            this.btUser.CommaSeparatedAccounts = value.User.Name;

            //        }
            //    }
            //}

        }

        protected void rbbookticketYes_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.rbbookticketYes.Checked)
            //{
            //    btUser.Enabled = false;
            //    // btUser.Visible = false;
            //    btUser.CommaSeparatedAccounts = string.Empty;

            //}
        }

        protected void RadioButtonYes_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButtonDisplay.Checked = false;
            //RadioButtonYes.Checked = true;
            //if (this.RadioButtonYes.Checked)
            //{
            //    btApplicant.Enabled = false;
            //    //btApplicant.Visible = false;
            //    this.btApplicant.CommaSeparatedAccounts = string.Empty;

            //}
        }

        protected void RadioButtonNo_CheckedChanged(object sender, EventArgs e)
        {
            //btApplicant.Enabled = true;
            ////  btApplicant.Visible = true;

            //ISharePointService sps = ServiceFactory.GetSharePointService(true);
            ////获取列表
            //SPList TRUserAssistant = sps.GetList(CAWorkFlowConstants.ListName.UserAssistant.ToString());

            //if (!string.IsNullOrEmpty(SPContext.Current.ListItem["btApplicant"] + ""))
            //{
            //    btApplicant.CommaSeparatedAccounts = SPContext.Current.ListItem["btApplicant"] + "";
            //}
            //else
            //{
            //    foreach (SPListItem item in TRUserAssistant.Items)
            //    {
            //        SPFieldUserValue value = new SPFieldUserValue(item.Web, item["Applicant"] as string);
            //        if (value.User.LoginName.Equals(this.Applicant.UserAccount, StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            value = new SPFieldUserValue(item.Web, item["Assistant"] as string);
            //            this.btApplicant.CommaSeparatedAccounts = value.User.Name;

            //        }

            //    }
            //}

        }
        protected void RadioButtonDisplay_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButtonDisplay.Checked = true;
            //RadioButtonYes.Checked = false;
            //if (this.RadioButtonDisplay.Checked)
            //{
            //    btApplicant.Enabled = false;
            //    //  btApplicant.Visible = false;

            //    this.btApplicant.CommaSeparatedAccounts = string.Empty;
            //}

        }
    }
}