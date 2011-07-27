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
using CodeArt.SharePoint.CamlQuery;
using CA.WorkFlow.UI;
using QuickFlow.Core;

namespace CA.WorkFlows.BusinessCard
{
    public partial class DataForm : QFUserControl
    {
        private Employee _Applicant;
        public Employee Applicant
        {
            get { return _Applicant; }
            set { _Applicant = value; }
        }
        public struct Constants
        {
            public const string WorkflowName = "Business Card Application";
            public const string ApplicantUpdate = "applicantedit";
            public const string DepartmentHeadApprove = "DepartmentHeadApprove";
            public const string ReceptionistConfirm = "ReceptionistConfirm";
        }
        public string ApplicantName
        {
            get
            {
                if (this.Applicant != null && !string.IsNullOrEmpty(this.Applicant.DisplayName))
                {
                    return this.Applicant.DisplayName;
                }
                else
                {
                    return SPContext.Current.ListItem["Applicant"] + "";
                }
            }
        }

        public string ApplicantColorCard
        {
            get
            {
                return this.hdColorCard.Value;
            }
        }
        private bool _flag;
        public bool Flag
        {
            get { return this._flag; }
            set { _flag = value; }
        }
        public string ApplicantReason
        {
            get
            {
                return this.txtReasonForApplication.Text;
            }
        }
        public string ApplicantAddr
        {
            get { return this.lblAddr.Text; }
        }
        public string ApplicantAddrChi
        {
            get { return this.lblAddrChi.Text; }
        }
        public string ApplicantDept
        {
            get { return this.txtDeptName.Text; }
        }
        public string ApplicantChiName
        {
            get { return this.txtUserName.Text; }
        }
        public string ApplicantJobTitle
        {
            get { return this.txtJobTitle.Text; }
        }
        public DateTime ApplicantDate
        {
            get { return DateTime.Parse(this.lblApplyDate.Text); }
        }
        public string ApplicantMobile
        {
            get { return txtMobilePhone.Text; }
        }
        public string ApplicantWorkFlowNuber
        {
            get { return lblID.Text; }
        }
        public string EnglishDepartment
        {
            get { return txtEnglishDept.Text; }
        }
        public string JobTitle
        {
            get { return txtEnglishJobTitle.Text; }
        }
        public string Telephone
        {
            get { return txtTelehpone.Text; }
        }
        public string Fax
        {
            get { return txtFax.Text; }
        }
        public string EmailAddress
        {
            get { return txtEmail.Text; }
        }
        public string EnglishName
        {
            get { return txtEnglishName.Text; }
        }
        public string Department
        {
            get { return txtDeptName.Text; }
        }
        private string _ApplicantApplyUser;
        public string ApplicantApplyUser
        {
            get { return _ApplicantApplyUser; }
            set { _ApplicantApplyUser = value; }
        }

        public override void SetControlMode()
        {
            base.SetControlMode();
            if (this.ControlMode == SPControlMode.New)
            {
                this.tbldispaly.Style.Add("display", "");
                this.trDisplay.Style.Add("display", "none");
                this.thDisplay.Style.Add("display", "none");
                this.Flag = true;
            }
            else if (this.ControlMode == SPControlMode.Edit)
            {
                this.tbldispaly.Style.Add("display", "");
                this.trDisplay.Style.Add("display", "none");
                this.thDisplay.Style.Add("display", "none");
                if (WorkflowContext.Current.Step.ToString() != "applicantedit")
                {
                    tdColor.Attributes.Add("onmousemove", "UpdateFlag();");
                }
                Flag = false;
            }
            else if (this.ControlMode == SPControlMode.Display)
            {
                this.tbldispaly.Style.Add("display", "none");
                this.trDisplay.Style.Add("display", "");
                this.thDisplay.Style.Add("display", "");
                txtDeptName.ReadOnly = true;
                txtEnglishName.ReadOnly = true;
                txtEnglishDept.ReadOnly = true;
                txtJobTitle.ReadOnly = true;
                txtEnglishJobTitle.ReadOnly = true;
                txtMobilePhone.ReadOnly = true;
                txtReasonForApplication.ReadOnly = true;
                txtUserName.ReadOnly = true;
                txtTelehpone.ReadOnly = true;
                txtFax.ReadOnly = true;
                txtEmail.ReadOnly = true;
                tdColor.Attributes.Add("onmousemove", "UpdateFlag();");
                Flag = false;
            } 
        }
        private bool _FindUser = true;
        public bool FindUser
        {
            get { return this._FindUser; }
            set { _FindUser = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Applicant = this.CurrentEmployee;
                if (this.ControlMode == SPControlMode.New)
                {
                    FillEmployeeData(this.Applicant);
                }
                else
                {
                    FillData();
                }
            }
            else
            {
                this.CAPeopleFinder.Load += new EventHandler(CAPeopleFinder_Load);
            }
        }
        protected void CAPeopleFinder_Load(object sender, EventArgs e)
        {
            string userAccount = SPContext.Current.Web.CurrentUser.LoginName;
            if (this.CAPeopleFinder.Accounts.Count > 0)
            {
                userAccount = this.CAPeopleFinder.Accounts[0] + "";
                this.Applicant = UserProfileUtil.GetEmployee(userAccount);
                if (hdSearchUser.Value != userAccount)
                {
                    hdSearchUser.Value = userAccount;
                    FindUser = true;
                }
                else
                {
                    FindUser = false;
                }
            }
            else
            {
                this.Applicant = UserProfileUtil.GetEmployee(userAccount);
                if (Flag == false)
                {
                    this.Applicant = UserProfileUtil.GetEmployee(hdApplyUser.Value);
                }
                FindUser = false;
                //Page.RegisterStartupScript("colorCard", "<script>setActiveStyleSheet('" + hdColorCard.Value + "');</script>");
            }
            FillEmployeeData(this.Applicant);
        }
        public void FillEmployeeData(Employee employee)
        {
            if (FindUser)
            {
                this.txtEnglishName.Text = employee.DisplayName;
                this.txtEnglishDept.Text = employee.Department;
                this.txtEnglishJobTitle.Text = employee.Title;
                this.txtTelehpone.Text = employee.Phone;
                this.txtMobilePhone.Text = employee.Mobile;
                this.txtFax.Text = employee.Fax;
                this.txtEmail.Text = employee.WorkEmail.Replace("C-AND-A.CN", "c-and-a.cn");
                this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                this.lblApplyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            Page.RegisterStartupScript("colorCard", "<script>setActiveStyleSheet('" + hdColorCard.Value + "');</script>");
        }

        private void FillData()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList listBalance = sps.GetList(CAWorkFlowConstants.WorkFlowListName.BusinessCard.ToString());

            QueryField field = new QueryField("ID");

            SPListItemCollection items = sps.Query(listBalance, field.Equal(Request["ID"] != null ? Request["ID"].ToString() : ""), 1);

            //如果有值
            if (items.Count > 0)
            {
                this.txtEnglishName.Text = items[0]["UserName"] + "";
                //this.lblPopulateName.Text = items[0]["PopulateName"] + "";
                this.txtEnglishDept.Text = items[0]["Department"] + "";
                this.txtEnglishJobTitle.Text = items[0]["JobTitle"] + "";
                this.txtTelehpone.Text = items[0]["Telephone"] + "";
                this.txtFax.Text = items[0]["Fax"] + "";
                this.txtEmail.Text = items[0]["EmailAddress"] + "";
                this.txtMobilePhone.Text = items[0]["MobilePhone"] + "";
                //this.ddlColorCard.SelectedItem.Text = items[0]["ColorOfCard"] + "";
                this.txtReasonForApplication.Text = items[0]["ReasonApplication"] + "";
                //this.lblAddr.Text = items[0]["Addr"] + "";
                this.txtDeptName.Text = items[0]["DepartmentChi"] + "";
                this.txtJobTitle.Text = items[0]["JobTitleChi"] + "";
                //this.lblAddrChi.Text = items[0]["AddrChi"] + "";
                this.txtUserName.Text = items[0]["UserNameChi"] + "";
                this.lblApplyDate.Text = items[0]["ApplyDate"] == null ? "" : DateTime.Parse(items[0]["ApplyDate"].ToString()).ToString("yyyy-MM-dd");
                this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                this.lblID.Text = items[0]["WorkFlowNumber"] + "";
                string strColorCard = items[0]["ColorOfCard"] == null ? "" : items[0]["ColorOfCard"].ToString();
                hdApplyUser.Value = items[0]["AppliedUser"] + "";
                hdColorCard.Value = strColorCard;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "colorCard", "setActiveStyleSheet('" + items[0]["ColorOfCard"].ToString() + "');", true);
                Page.RegisterStartupScript("colorCard", "<script>setActiveStyleSheet('" + strColorCard + "');</script>");
            }
            else
            {
                //this.labAnnulLeave.Text = "this user Entitlement cannot find,please contract with it administratro";
                //this.labSickLeave.Text = "";
                //this.labAnnualEntitlement.Text = "";
                //this.labSickEntitlement.Text = "";
            }
        }
        
    }
}