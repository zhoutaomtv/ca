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

namespace CA.WorkFlow.UI.ITHardwareOrSoftwareApplication
{
    public partial class DataForm : QFUserControl
    {
        private float cost = 0;
        private Employee _Applicant;
        public Employee Applicant
        {
            get { return _Applicant; }
            set { _Applicant = value; }
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

        public string WorkflowNumber
        {
            get
            {
                return lblWorkflowNumber.Text;
            }
        }
        public string Name
        {
            get { return txtName.Value; }
        }
        public string Department
        {
            get { return txtDept.Value; }
        }
        public string ApplyDate
        {
            get { return txtDate.Value; }
        }
        public bool IsFOCO
        {
            get { return chkBox.Checked ? true : false; }
        }

        public string UserName
        {
            get { return SPContext.Current.Web.CurrentUser.LoginName; }
        }

        public string ReasonforRequest
        {
            get { return txtarea.Value; }
        }
        private bool _flag;
        public bool Flag
        {
            get { return this._flag; }
            set { _flag = value; }
        }

        private bool _selectEmp = false;
        public bool SelectEmp
        {
            get { return _selectEmp; }
            set { _selectEmp = value; }
        }
        public override void SetControlMode()
        {
            base.SetControlMode();
            if (this.ControlMode == SPControlMode.New)
            {
                this.trDisplay.Style.Add("display", "none");
                this.tbldispaly.Style.Add("display", "");
                Flag = true;
            }
            else if (this.ControlMode == SPControlMode.Edit)
            {
                this.trDisplay.Style.Add("display", "none");

                Flag = false;
            }
            else if (this.ControlMode == SPControlMode.Display)
            {
                this.trDisplay.Style.Add("display", "");
                this.tbldispaly.Style.Add("display", "none");
                txtName.Disabled = false;
                txtDept.Disabled = false;
                txtDate.Disabled = false;
                chkBox.Disabled = false;
                Flag = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Applicant = this.CurrentEmployee;
                switch (this.ControlMode)
                {
                    #region
                    case SPControlMode.New:
                        this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                        InitData(Applicant);
                        this.DataTableRecord.Rows.Add();
                        BindDTRecord(this.rptItRequest);
                        break;
                    case SPControlMode.Edit:
                        FillData();
                        switch (WorkflowContext.Current.Step.ToString())
                        {
                            case "ITMember":
                                BindDTRecord(this.rptItRequest);
                                this.txtName.Disabled = true;
                                this.txtDept.Disabled = true;
                                this.txtDate.Disabled = true;
                                this.txtarea.Disabled = true;
                                imgbtnAddItem.Visible = false;
                                this.tbldispaly.Style.Add("display", "none");
                                break;
                            case "UpdateTask":
                                BindDTRecord(this.rptItRequest);
                                break;
                            case "ITHeader":
                                BindDTRecord(this.rptItRequestDiplay);
                                this.txtName.Disabled = true;
                                this.txtDept.Disabled = true;
                                this.txtDate.Disabled = true;
                                this.txtarea.Disabled = true;
                                imgbtnAddItem.Visible = false;
                                trFOCO.Style.Add("display", "");
                                break;
                            default:
                                BindDTRecord(this.rptItRequestDiplay);
                                this.txtName.Disabled = true;
                                this.txtDept.Disabled = true;
                                this.txtDate.Disabled = true;
                                this.txtarea.Disabled = true;
                                imgbtnAddItem.Visible = false;
                                break;
                        }
                        break;
                    case SPControlMode.Display:
                        FillData();
                        BindDTRecord(this.rptItRequestDiplay);
                        this.txtName.Disabled = true;
                        this.txtDept.Disabled = true;
                        this.txtDate.Disabled = true;
                        this.txtarea.Disabled = true;
                        this.chkBox.Disabled = true;
                        imgbtnAddItem.Visible = false;
                        break;
                    #endregion
                }
            }
            else
            {
                this.CAPeopleFinder.Load += new EventHandler(CAPeopleFinder_Load);
            }
        }
        private bool _FindUser = true;
        public bool FindUser
        {
            get { return this._FindUser; }
            set { _FindUser = value; }
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
            }
            FillEmployeeData(this.Applicant);
        }

        private void FillEmployeeData(Employee emp)
        {
            if (FindUser)
            {
                this.txtName.Value = emp.DisplayName;
                this.txtDept.Value = emp.Department;
                //txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void InitData(Employee employee)
        {
            txtName.Value = employee.DisplayName;
            txtDept.Value = employee.Department;
            txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void FillData()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList listBalance = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ItRequestWorkFlow.ToString());

            QueryField field = new QueryField("ID");

            SPListItemCollection items = sps.Query(listBalance, field.Equal(Request["ID"] != null ? Request["ID"].ToString() : ""), 1);

            //如果有值
            if (items.Count > 0)
            {
                this.txtName.Value = items[0]["Name"] + "";
                //this.lblPopulateName.Text = items[0]["PopulateName"] + "";
                this.txtDept.Value = items[0]["Department"] + "";
                this.txtDate.Value = items[0]["ApplyDate"] + "";
                if (items[0]["FOCOApprove"].ToString() == "True")
                {
                    this.chkBox.Checked = true;
                }
                this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                this.lblWorkflowNumber.Text = items[0]["WorkFlowNumber"] + "";
                txtarea.Value = items[0]["ReasonforRequest"] + "";
                hdApplyUser.Value = items[0]["AppliedUser"] + "";
            }

            //根据field来查询
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ITRequestItems.ToString());
            field = new QueryField("WorkFlowNumber", false);
            string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            items = sps.Query(listRecord, field.Equal(strTimeOffNumber), 100);

            DataTable dt = items.GetDataTable();
            if (dt == null || dt.Rows == null)
                return;

            DataRow rowAdd = null;

            foreach (DataRow row in dt.Rows)
            {
                rowAdd = this.DataTableRecord.Rows.Add();
                rowAdd["HardwareOrSoftwareName"] = row["HardwareOrSoftwareName"] + "";
                rowAdd["Cost"] = row["Cost"] + "";
                rowAdd = null;
            }
        }

        private bool _SureDataTable = false;

        public bool SureDataTable
        {
            get { return _SureDataTable; }
            set { _SureDataTable = value; }
        }


        public DataTable DataTableRecord
        {
            get
            {
                this.EnSureDataTable();

                return (DataTable)ViewState["dtRecord"];
            }
            set
            {
                ViewState["dtRecord"] = value;
            }
        }


        private void EnSureDataTable()
        {
            if (ViewState["dtRecord"] == null)
            {
                DataTableRecord = new DataTable();

                DataTableRecord.Columns.Add("HardwareOrSoftwareName");
                DataTableRecord.Columns.Add("Cost");
            }
            if (this.SureDataTable == true)
                return;

            this.SureDataTable = true;
            if (this.rptItRequest.Items.Count > 0)
            {
                this.DataTableRecord.Rows.Clear();

                foreach (RepeaterItem item in this.rptItRequest.Items)
                {
                    this.DataTableRecord.Rows.Add(ConvertToDataRow(item));
                }
            }
        }

        private DataRow ConvertToDataRow(RepeaterItem item)
        {

            TextBox txtHardName1 = (TextBox)item.FindControl("txtHardName");
            TextBox txtCost1 = (TextBox)item.FindControl("txtCost");

            DataRow row = this.DataTableRecord.NewRow();
            row["HardwareOrSoftwareName"] = txtHardName1.Text;
            row["Cost"] = txtCost1.Text;

            return row;
        }

        private void BindDTRecord(Repeater rpt)
        {
            rpt.DataSource = this.DataTableRecord;
            rpt.DataBind();
            if (rpt.Items.Count <= 0)
            {
                txtSumCost.Text = "0";
            }
        }

        protected void rptItRequest_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            cost = 0;
            if (e.CommandName == "delete")
            {
                this.DataTableRecord.Rows.Remove(DataTableRecord.Rows[e.Item.ItemIndex]);
                //string cost =DataTableRecord.Rows[e.Item.ItemIndex]["Cost"].ToString();
                //txtSumCost.Text = Convert.ToString(float.Parse(txtSumCost.Text == "" ? "0" : txtSumCost.Text) - float.Parse(cost == "" ? "0" : cost));
                BindDTRecord(this.rptItRequest);
            }
        }

        protected void rptItRequest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;
                ((TextBox)e.Item.FindControl("txtHardName")).Text = row["HardwareOrSoftwareName"] + "";
                ((TextBox)e.Item.FindControl("txtCost")).Text = row["Cost"] + "";

                //cost += (((TextBox)e.Item.FindControl("txtCost")).Text == "" ? 0 : float.Parse(((TextBox)e.Item.FindControl("txtCost")).Text));
                //2010-11-5
                float txtcost = 0;
                float.TryParse(((TextBox)e.Item.FindControl("txtCost")).Text, out txtcost);
                cost += txtcost;

                if (WorkflowContext.Current.Step.ToString() == "ITMember")
                {
                    ((ImageButton)e.Item.FindControl("ImageButton1")).Style.Add("display", "none");
                }
            }
            txtSumCost.Text = cost.ToString("#0.00");
        }

        protected void imgbtnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            cost = 0;
            this.DataTableRecord.Rows.Add();
            this.BindDTRecord(this.rptItRequest);
        }

        protected void rptItRequestDiplay_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                //cost += (row["Cost"] == null || row["Cost"].ToString() == "") ? 0 : float.Parse(row["Cost"].ToString());
                //2010-11-5
                float rowcost = 0;
                float.TryParse(row["Cost"].ToString(), out rowcost);
                cost += rowcost;


            }
            txtSumCost.Text = cost.ToString("#0.00");
        }

        public string Validate
        {
            get
            {
                string status = string.Empty;
                if (this.ControlMode == SPControlMode.Display)
                { status = ""; }

                for (int i = 0; i < this.rptItRequest.Items.Count; i++)
                {
                    RepeaterItem item = rptItRequest.Items[i];
                    TextBox txtHardName = (TextBox)item.FindControl("txtHardName");
                    TextBox txtCost = (TextBox)item.FindControl("txtCost");
                    //if (string.IsNullOrEmpty(txtCost.Text) || string.IsNullOrEmpty(txtHardName.Text))
                    if (string.IsNullOrEmpty(txtHardName.Text))
                    {
                        status = "Please supply valid IT request details";
                    }
                    //else
                    //{
                    //    try
                    //    {
                    //        float.Parse(txtCost.Text);
                    //    }
                    //    catch
                    //    {
                    //        status = "Cost is not valid";
                    //    }
                    //}
                }
                return status;
            }
        }

        public string ValidSavedate
        {
            get
            {
                string status = string.Empty;
                if (this.ControlMode == SPControlMode.Display)
                { status = ""; }

                for (int i = 0; i < this.rptItRequest.Items.Count; i++)
                {
                    RepeaterItem item = rptItRequest.Items[i];
                    TextBox txtCost = (TextBox)item.FindControl("txtCost");
                    if (string.IsNullOrEmpty(txtCost.Text))
                    {
                        status = "Please supply valid IT request details";
                    }
                    else
                    {
                        try
                        {
                            float.Parse(txtCost.Text);
                        }
                        catch
                        {
                            status = "Cost is not valid";
                        }
                    }
                }
                return status;
            }
        }
    }
}