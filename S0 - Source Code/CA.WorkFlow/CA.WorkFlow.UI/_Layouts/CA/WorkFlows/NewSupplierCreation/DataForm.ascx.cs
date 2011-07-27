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
using QuickFlow.Core;

namespace CA.WorkFlow.UI.NewSupplierCreation
{
    public partial class DataForm : QFUserControl
    {
        private Employee _Applicant;
        public Employee Applicant
        {
            get { return _Applicant; }
            set { _Applicant = value; }
        }
        private string strStatus = "";
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

        public string WorkflowNumber
        {
            get
            {
                return lblWorkflowNumber.Text;
            }
        }
        public string Supplier
        {
            get { return txtSupplier.Text; }
        }
        public string SubDivision
        {
            get { return ddlSubDivision.SelectedValue; }
        }
        public string IsMondia
        {
            get { return ddlMondial.SelectedValue; }
        }
        public string Status
        {
            get { return ddlStatus.SelectedValue; }
        }
        //public string UploadFile
        //{
        //    get { return string.IsNullOrEmpty(fulOrderDetails.FileName) == true ? "" : fulOrderDetails.FileName; }
        //}
        public string UserName
        {
            get { return SPContext.Current.Web.CurrentUser.LoginName; }
        }
        public override SPControlMode ControlMode
        {
            get
            {
                return base.ControlMode;
            }
            set
            {
                base.ControlMode = value;
            }
        }
        public override void SetControlMode()
        {
            base.SetControlMode();
            if (this.ControlMode == SPControlMode.New)
            {
                this.trDisplay.Style.Add("display", "none");
            }
            else if (this.ControlMode == SPControlMode.Edit)
            {
                this.trDisplay.Style.Add("display", "none");
            }
            else if (this.ControlMode == SPControlMode.Display)
            {
                this.trDisplay.Style.Add("display", "");
                ddlSubDivision.Enabled = false;
                txtSupplier.ReadOnly = true;
                ddlMondial.Enabled = false;
                ddlStatus.Enabled = false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Applicant = this.CurrentEmployee;
            if (!IsPostBack)
            {
                InitData();
                switch(this.ControlMode)
                {
                    case SPControlMode.New:
                        this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                        if (!string.IsNullOrEmpty(Applicant.Department))
                        {
                            try
                            {
                                ddlSubDivision.Items.FindByValue(Applicant.Department).Selected = true;
                            }
                            catch (Exception ex)
                            { }
                        }
                        break;
                    case SPControlMode.Edit:

                        if (WorkflowContext.Current.Step.ToString() != "TaskUpdate")
                        {
                            ddlSubDivision.Enabled = false;
                            txtSupplier.ReadOnly = true;
                            ddlMondial.Enabled = false;
                            attacthment.ControlMode = SPControlMode.Display;
                        }
                        else
                        {
                            attacthment.ControlMode = SPControlMode.Edit;
                        }
                        FillData();
                        if (WorkflowContext.Current.Step.ToString() == "BSSTeam")
                        {
                            trShow.Style.Add("display", "");
                            updateList.Style.Add("display", "");
                            ddlMondial.Enabled = false;

                            if (ddlMondial.SelectedItem.Text == "No")
                            {
                                ddlStatus.Items.Clear();
                                ddlStatus.Items.Insert(0, new ListItem("Waiting Factory Assessment Form", "Waiting Factory Assessment Form"));
                                ddlStatus.Items.Insert(1, new ListItem("Factory Assessment Ongoing", "Factory Assessment Ongoing"));
                                ddlStatus.Items.Insert(2, new ListItem("Factory Assessment Success", "Factory Assessment Success"));
                                ddlStatus.Items.Insert(3, new ListItem("Factory Assessment Failed", "Factory Assessment Failed"));

                                ddlStatus.Items.Insert(4, new ListItem("Supplier Application Form Recieved", "Supplier Application Form Recieved"));
                                ddlStatus.Items.Insert(5, new ListItem("Contract Signed & System Setup OK", "Contract Signed & System Setup OK"));
                            }
                            else
                            {
                                ddlStatus.Items.Insert(0, new ListItem("Supplier Application Form Recieved", "Supplier Application Form Recieved"));
                                ddlStatus.Items.Insert(1, new ListItem("Contract Signed & System Setup OK", "Contract Signed & System Setup OK"));
                            }
                            if (!string.IsNullOrEmpty(strStatus))
                            {
                                ddlStatus.Items.FindByText(strStatus).Selected = true;
                            }
                        }
                        break;
                    case SPControlMode.Display:
                        FillData();
                        updateList.Style.Add("display", "");
                        ddlSubDivision.Enabled = false;
                        txtSupplier.ReadOnly = true;
                        ddlStatus.Enabled = false;
                        ddlMondial.Enabled = false;
                        break;
                }
            }
        }
        private void InitData()
        {
            ddlSubDivision.Items.Clear();
            ddlSubDivision.Items.Insert(0, new ListItem("Div1-Clockhouse", "Div1-Clockhouse"));
            ddlSubDivision.Items.Insert(1, new ListItem("Div1-Yessica", "Div1-Yessica"));
            ddlSubDivision.Items.Insert(2, new ListItem("Div2-M Angelo Litrico","Div2-M Angelo Litrico"));
            ddlSubDivision.Items.Insert(3, new ListItem("Div2-M Clockhouse", "Div2-M Clockhouse"));
            ddlSubDivision.Items.Insert(4, new ListItem("Div3", "Div3"));
            ddlSubDivision.Items.Insert(5, new ListItem("Div4", "Div4"));
        }
        private void FillData()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList listBalance = sps.GetList(CAWorkFlowConstants.WorkFlowListName.NewSupplierCreationWorkFlow.ToString());

            QueryField field = new QueryField("ID");

            SPListItemCollection items = sps.Query(listBalance, field.Equal(Request["ID"] != null ? Request["ID"].ToString() : ""), 1);
            //Page.RegisterStartupScript("status", "<script>SelectStatus();</script>");
            //如果有值
            if (items.Count > 0)
            {
                this.txtSupplier.Text = items[0]["Supplier"] + "";
                //this.lblPopulateName.Text = items[0]["PopulateName"] + "";
                //this.txtSubDivision.Text = items[0]["SubDivision"] + "";
                this.ddlSubDivision.Items.FindByValue(items[0]["SubDivision"] + "").Selected = true;
                this.ddlMondial.Items.FindByValue(items[0]["IsMondial"] + "").Selected = true;
                strStatus = items[0]["Status"] + "";
                this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                this.lblWorkflowNumber.Text = items[0]["WorkFlowNumber"] + "";
                this.lblStatusList.Text = items[0]["StatusList"] + "";
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