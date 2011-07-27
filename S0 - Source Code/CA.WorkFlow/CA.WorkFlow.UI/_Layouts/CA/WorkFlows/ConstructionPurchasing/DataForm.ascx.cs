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
using System.IO;

namespace CA.WorkFlow.UI.ConstructionPurchasing
{
    public partial class DataForm : QFUserControl
    {
        private float cost = 0;
        private int number = 0;
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
        #region begin页面基本信息
        public string ApplyName
        {
            get { return this.lblLogUser.Text; }
        }
        public string ApplyDepartment
        {
            get { return Applicant.Department; }
        }
        public string ApplyDate
        {
            get { return lblApplyDate.Text; }
        }
        public string RequestType
        {
            get { return ddlType.SelectedItem.Text; }
        }
        public string BudgetApproved
        {
            get { return ddlBudget.SelectedItem.Text; }
        }
        public string CurrencyType
        { get { return ddlCurrency.SelectedItem.Text; } }
        public string CostCenter
        { get { return ddlCostCenter.SelectedItem.Text; } }
        public string BudgetValue
        { get { return txtBudgetValue.Text; } }
        public string ProduceandDeliveryDate
        {
            get 
            { 
                return txtPandDDate.SelectedDate.ToString("yyyy-MM-dd"); 
            } 
        }
        public string Installation
        { get { return txtInstallation.Text; } }
        public string Freight
        { get { return txtFreight.Text; } }
        public string Packaging
        { get { return txtPackaging.Text; } }
        #endregion end

        public string UserName
        {
            get { return SPContext.Current.Web.CurrentUser.LoginName; }
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
                Flag = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Applicant = this.CurrentEmployee;
            if (!IsPostBack)
            {
                InitData();
                switch (this.ControlMode)
                { 
                    case SPControlMode.New:
                        this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                        this.lblApplyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        
                        this.DataTableRecord.Rows.Add();
                        BindDTRecord(this.rptItRequest);
                        break;
                    case SPControlMode.Edit:
                        FillData();
                        switch (WorkflowContext.Current.Step.ToString())
                        {
                            case "ConstructionHead":
                                BindDTRecord(this.rptItRequestDiplay);
                                imgbtnAddItem.Visible = false;
                                imgbtnAddItem.Visible = false;
                                DisplayContorl();
                                Displayfees();
                                break;
                            case "UpdateTask":
                                BindDTRecord(this.rptItRequest);
                                break;
                            case "Construction":
                                BindDTRecord(this.rptItRequest);
                                imgbtnAddItem.Visible = true;
                                DisplayContorl();
                                break;
                            case "PlacesTheOrder":
                                BindDTRecord(this.rptItRequestDiplay);
                                tblChk.Style.Add("display", "");
                                imgbtnAddItem.Visible = false;
                                DisplayContorl();
                                Displayfees();
                                ExportExcel();
                                break;
                            case "OrderHandover":
                                BindDTRecord(this.rptItRequestDiplay);
                                imgbtnAddItem.Visible = false;
                                DisplayContorl();
                                Displayfees();
                                tblChk.Style.Add("display", "");
                                trChk.Style.Add("display", "");
                                chkPlacesOrder.Enabled = false;
                                break;
                            default:
                                BindDTRecord(this.rptItRequestDiplay);
                                imgbtnAddItem.Visible = false;
                                DisplayContorl();
                                Displayfees();
                                break;
                        }
                        break;
                    case SPControlMode.Display:
                        FillData();
                        BindDTRecord(this.rptItRequestDiplay);
                        DisplayContorl();
                        Displayfees();
                        imgbtnAddItem.Visible = false;
                        chkPlacesOrder.Enabled = false;
                        chkOrderHandover.Enabled = false;
                        tblChk.Style.Add("display", "");
                        trChk.Style.Add("display", "");
                        break;
                }
            }
        }

        private void DisplayContorl()
        {
            ddlType.Enabled = false;
            ddlCurrency.Enabled = false;
            ddlCostCenter.Enabled = false;
            ddlBudget.Enabled = false;
            txtBudgetValue.Enabled = false;
            txtPandDDate.Enabled = false;
        }

        private void Displayfees()
        {
            txtFreight.Enabled = false;
            txtInstallation.Enabled = false;
            txtPackaging.Enabled = false;
        }

        public void InitData()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);
            SPList list = sps.GetList("Stores");
            SPListItemCollection item = list.Items; ;
            DataTable dt = item.GetDataTable();
            ddlCostCenter.DataSource = dt;
            ddlCostCenter.DataTextField = "Title";
            ddlCostCenter.DataValueField = "Title";
            ddlCostCenter.DataBind();
            txtPandDDate.SelectedDate = DateTime.Now;
        }

        private void FillData()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList listBalance = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionPurchasingWorkflow.ToString());

            QueryField field = new QueryField("ID");

            SPListItemCollection items = sps.Query(listBalance, field.Equal(Request["ID"] != null ? Request["ID"].ToString() : ""), 1);

            //如果有值
            if (items.Count > 0)
            {
                
                this.ddlType.Items.FindByValue(items[0]["RequestType"] + "").Selected = true ;
                this.ddlBudget.Items.FindByValue(items[0]["BudgetApproved"] + "").Selected = true;
                if (items[0]["BudgetApproved"] + "" == "No")
                {
                    spanColor.Style.Add("Display", "none");
                }
                this.ddlCurrency.Items.FindByValue(items[0]["CurrencyType"] + "").Selected = true;
                this.ddlCostCenter.Items.FindByValue(items[0]["CostCenter"] + "").Selected = true;
                this.lblLogUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
                this.lblWorkflowNumber.Text = items[0]["WorkFlowNumber"] + "";
                if (!string.IsNullOrEmpty(items[0]["ProduceandDeliveryDate"] + ""))
                {
                    txtPandDDate.SelectedDate = Convert.ToDateTime(items[0]["ProduceandDeliveryDate"].ToString());
                }
                txtBudgetValue.Text = items[0]["BudgetValue"] + "";
                txtInstallation.Text = items[0]["Installation"] + "";
                this.txtFreight.Text = items[0]["Freight"] + "";
                txtPackaging.Text = items[0]["Packaging"] + "";
                if (items[0]["PlacesOrder"] + "" == "Yes")
                {
                    chkPlacesOrder.Checked = true;
                }
                if (items[0]["OrderHandover"] + "" == "Yes")
                {
                    chkOrderHandover.Checked = true;
                }
            }

            //根据field来查询
            SPList listRecord = sps.GetList(CAWorkFlowConstants.WorkFlowListName.ConstructionItems.ToString());
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
                rowAdd["ItemCode"] = row["ItemCode"] + "";
                rowAdd["Discription"] = row["Discription"] + "";
                rowAdd["Quantity"] = row["Quantity"] + "";
                rowAdd["Unit"] = row["Unit"] + "";
                rowAdd["UnitPrice"] = row["UnitPrice"] + "";
                rowAdd["TotalPrice"] = row["TotalPrice"] + "";
                rowAdd["Remark"] = row["Remark"] + "";
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

                DataTableRecord.Columns.Add("ItemCode");
                DataTableRecord.Columns.Add("Discription");
                DataTableRecord.Columns.Add("Quantity");
                DataTableRecord.Columns.Add("Unit");
                DataTableRecord.Columns.Add("UnitPrice");
                DataTableRecord.Columns.Add("TotalPrice");
                DataTableRecord.Columns.Add("Remark");
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

            TextBox txtItemCode = (TextBox)item.FindControl("txtItemCode");
            TextBox txtDiscription = (TextBox)item.FindControl("txtDiscription");
            TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
            TextBox txtUnit = (TextBox)item.FindControl("txtUnit");
            TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
            TextBox txtTotalPrice = (TextBox)item.FindControl("txtTotalPrice");
            TextBox txtRemark = (TextBox)item.FindControl("txtRemark");

            DataRow row = this.DataTableRecord.NewRow();
            row["ItemCode"] = txtItemCode.Text;
            row["Discription"] = txtDiscription.Text;
            row["Quantity"] = txtQuantity.Text;
            row["Unit"] = txtUnit.Text;
            row["UnitPrice"] = txtUnitPrice.Text;
            row["TotalPrice"] = txtTotalPrice.Text;
            row["Remark"] = txtRemark.Text;
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
                ((TextBox)e.Item.FindControl("txtItemCode")).Text = row["ItemCode"] + "";
                ((TextBox)e.Item.FindControl("txtDiscription")).Text = row["Discription"] + "";
                ((TextBox)e.Item.FindControl("txtQuantity")).Text = row["Quantity"] + "";
                ((TextBox)e.Item.FindControl("txtUnit")).Text = row["Unit"] + "";
                ((TextBox)e.Item.FindControl("txtUnitPrice")).Text = row["UnitPrice"] + "";
                ((TextBox)e.Item.FindControl("txtTotalPrice")).Text = row["TotalPrice"] + "";
                ((TextBox)e.Item.FindControl("txtRemark")).Text = row["Remark"] + "";
                if (WorkflowContext.Current.Step.ToString() == "ITMember")
                {
                    ((ImageButton)e.Item.FindControl("ImageButton1")).Style.Add("display", "none");
                }
                if (e.Item.FindControl("lblNumber") != null)
                {
                    ((Label)e.Item.FindControl("lblNumber")).Text = Convert.ToString(++number);
                }
                cost += (((TextBox)e.Item.FindControl("txtTotalPrice")).Text == "" ? 0 : float.Parse(((TextBox)e.Item.FindControl("txtTotalPrice")).Text));
                
            }
            txtSumCost.Text = cost.ToString("#0.00");
        }

        protected void imgbtnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            this.DataTableRecord.Rows.Add();
            this.BindDTRecord(this.rptItRequest);
        }

        protected void rptItRequestDiplay_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.FindControl("lblNumber") != null)
                {
                    ((Label)e.Item.FindControl("lblNumber")).Text = Convert.ToString(++number);
                }
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;
                cost += (row["TotalPrice"] == null || row["TotalPrice"].ToString() == "") ? 0 : float.Parse(row["TotalPrice"].ToString());
            }
            txtSumCost.Text = cost.ToString("#0.00");
        }

        private void ExportExcel()
        {
            string strFileName = "standard_PO_" + lblWorkflowNumber.Text + ".xls";
            string strPath = Server.MapPath("/tmpfiles/excel");// "d:/pdf";
            DirectoryInfo dinfo = new DirectoryInfo(strPath);
            if (!dinfo.Exists)
            {
                Directory.CreateDirectory(strPath);
            }
            string strFilePath = strPath + "\\" + strFileName;
            DataTable dtRecords = this.DataTableRecord;
            OperateExcel.ExportConstructionPurchasing(Server.MapPath("standard PO.xls"), dtRecords, CostCenter, txtPandDDate.SelectedDate.ToString("yyyy-MM-dd"),
                txtSumCost.Text, Installation, Freight, Packaging, strFilePath);
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
                    TextBox txtItemCode = (TextBox)item.FindControl("txtItemCode");
                    TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                    TextBox txtUnit = (TextBox)item.FindControl("txtUnit");
                    TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                    TextBox txtTotalPrice = (TextBox)item.FindControl("txtTotalPrice");
                    if (string.IsNullOrEmpty(txtItemCode.Text)
                        || string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtUnit.Text)
                        || string.IsNullOrEmpty(txtUnitPrice.Text) || string.IsNullOrEmpty(txtTotalPrice.Text))
                    {
                        status = "Please supply valid Construction Purchasing details";
                    }
                    else
                    {
                        try
                        {
                            float.Parse(txtTotalPrice.Text);
                        }
                        catch
                        {
                            status = "Total Price is not valid";
                        }
                    }
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
                    TextBox txtTotalPrice = (TextBox)item.FindControl("txtTotalPrice");
                    if (!string.IsNullOrEmpty(txtTotalPrice.Text))
                    {
                        try
                        {
                            float.Parse(txtTotalPrice.Text);
                        }
                        catch
                        {
                            status = "Total Price is not valid";
                        }
                    }
                }
                return status;
            }
        }
    }
}