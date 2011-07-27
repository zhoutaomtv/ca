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

namespace CA.WorkFlow.UI.SupplierReinspection
{
    public partial class DataForm : QFUserControl
    {
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
            set
            {
                lblWorkflowNumber.Text = value;
                ucFileUpload.WorkflowNumber = value;
            }
        }

        public string POFileName
        {
            get
            {
                return ucFileUpload.FileFullName;
            }
        }

        public DataTable dtPODetails
        {
            get
            {
                return ViewState["dtPODetails"] != null ? (DataTable)ViewState["dtPODetails"] : CreatePODetails();
            }
            set
            {
                ViewState["dtPODetails"] = value;
            }
        }

        private bool _needUpdateMode = true;

        public bool NeedUpdateMode
        {
            get
            {
                return _needUpdateMode;
            }
            set
            {
                _needUpdateMode = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Applicant = this.CurrentEmployee;
                if (this.ControlMode == SPControlMode.New)
                {
                    //this.formfieldEmployee.Value = SPContext.Current.Web.CurrentUser.LoginName;
                    this.lblWorkflowNumber.Text = "NA";
                    dtPODetails.Rows.Add();
                    rptPODetails.DataSource = dtPODetails;
                    rptPODetails.DataBind();
                }
                else
                {
                    FillData();
                }
            }
        }
        private void FillData()
        {
            lblWorkflowNumber.Text = SPContext.Current.ListItem["WorkflowNumber"] + "";

            string fileName = SPContext.Current.ListItem["FileName"] + "";
            ucFileUpload.WorkflowNumber = lblWorkflowNumber.Text;
            if (!string.IsNullOrEmpty(fileName))
            {
                ucFileUpload.FileFullName = fileName;
            }

            GetPODetails();
        }

        // PO Details
        private DataTable CreatePODetails()
        {
            dtPODetails = new DataTable();
            dtPODetails.Columns.Add("RequestID");
            dtPODetails.Columns.Add("PONumber");
            dtPODetails.Columns.Add("Amount");
            dtPODetails.Columns.Add("ProcessDate");
            dtPODetails.Columns.Add("InvoiceNumber");
            dtPODetails.Columns.Add("DocumentNumber");

            return dtPODetails;
        }

        private void GetPODetails()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.PODetails + "");

            QueryField field = new QueryField("RequestID", false);

            SPListItemCollection items = sps.Query(list, field.Equal(string.IsNullOrEmpty(lblWorkflowNumber.Text) ? string.Empty : lblWorkflowNumber.Text), 0);

            foreach (SPListItem item in items)
            {
                DataRow row = dtPODetails.Rows.Add();
                row["RequestID"] = item["RequestID"];
                row["PONumber"] = item["PONumber"];
                row["Amount"] = item["Amount"];
                row["ProcessDate"] = item["ProcessDate"];
                row["InvoiceNumber"] = item["InvoiceNumber"];
                row["DocumentNumber"] = item["DocumentNumber"];
            }

            //this.rptPODetails.DataSource = dtPODetails;
            //this.rptPODetails.DataBind();
            if (rptPODetails.Visible)
            {
                if (dtPODetails.Rows.Count == 0)
                {
                    dtPODetails.Rows.Add();
                }

                this.rptPODetails.DataSource = dtPODetails;
                this.rptPODetails.DataBind();
            }
            else if (rptPODetailsDisplay.Visible)
            {
                this.rptPODetailsDisplay.DataSource = dtPODetails;
                this.rptPODetailsDisplay.DataBind();
            }
            else if (rptPODetailsUpdate.Visible)
            {
                this.rptPODetailsUpdate.DataSource = dtPODetails;
                this.rptPODetailsUpdate.DataBind();
            }
        }

        //protected void btnAddDetail_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (rptPODetails.Items.Count > 0)
        //    {
        //        RepeaterItem item = rptPODetails.Items[rptPODetails.Items.Count - 1];
        //        TextBox txtPONumber = (TextBox)item.FindControl("txtPONumber");
        //        TextBox txtAmount = (TextBox)item.FindControl("txtAmount");

        //        DataRow row = dtPODetails.Rows[dtPODetails.Rows.Count - 1];
        //        row["PONumber"] = txtPONumber.Text;
        //        row["Amount"] = txtAmount.Text;
        //    }
        //    dtPODetails.Rows.Add();
        //    this.rptPODetails.DataSource = dtPODetails;
        //    this.rptPODetails.DataBind();
        //}

        protected void rptPODetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("add", StringComparison.CurrentCultureIgnoreCase))
            {
                if (rptPODetails.Items.Count > 0)
                {
                    dtPODetails.Rows.Clear();

                    foreach (RepeaterItem item in rptPODetails.Items)
                    {
                        TextBox txtPONumber = (TextBox)item.FindControl("txtPONumber");
                        TextBox txtAmount = (TextBox)item.FindControl("txtAmount");

                        DataRow row = dtPODetails.Rows.Add();
                        row["PONumber"] = txtPONumber.Text;
                        row["Amount"] = txtAmount.Text;
                    }
                }
                dtPODetails.Rows.Add();
                this.rptPODetails.DataSource = dtPODetails;
                this.rptPODetails.DataBind();
            }
            else if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                updatePODetails();
                dtPODetails.Rows.Remove(dtPODetails.Rows[e.Item.ItemIndex]);
                this.rptPODetails.DataSource = dtPODetails;
                this.rptPODetails.DataBind();
            }
        }

        protected void rptPODetails_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;
                TextBox txtPONumber = (TextBox)e.Item.FindControl("txtPONumber");
                txtPONumber.Text = row["PONumber"].ToString();
                TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
                txtAmount.Text = row["Amount"].ToString();

            }
        }

        protected void rptPODetailsUpdate_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                CADateTimeControl caDateTime = (CADateTimeControl)e.Item.FindControl("cdtProcessDate");
                caDateTime.SelectedDate = string.IsNullOrEmpty(row["ProcessDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["ProcessDate"].ToString());
                TextBox txtInvoiceNumber = (TextBox)e.Item.FindControl("txtInvoiceNumber");
                txtInvoiceNumber.Text = row["InvoiceNumber"].ToString();
                TextBox txtDocumentNumber = (TextBox)e.Item.FindControl("txtDocumentNumber");
                txtDocumentNumber.Text = row["DocumentNumber"].ToString();
            }
        }

        public override void SetControlMode()
        {
            base.SetControlMode();

            if (!NeedUpdateMode)
                return;

            if (ControlMode == SPControlMode.New || ControlMode == SPControlMode.Edit)
            {
                rptPODetails.Visible = true;
                rptPODetailsUpdate.Visible = false;
                rptPODetailsDisplay.Visible = false;
                //btnAddDetail.Visible = true;
            }
            else if (ControlMode == SPControlMode.Display)
            {
                rptPODetails.Visible = false;
                rptPODetailsUpdate.Visible = false;
                rptPODetailsDisplay.Visible = true;
                //btnAddDetail.Visible = false;
            }
        }

        public void ShowUpdateTable()
        {
            rptPODetails.Visible = false;
            rptPODetailsUpdate.Visible = true;
            rptPODetailsDisplay.Visible = false;
            //btnAddDetail.Visible = false;
        }

        // Validation  验证
        public string Validate()
        {
            string status = string.Empty;

            status += validateGeneralInfo();
            if (rptPODetails.Visible)
                status += validatePODetails();
            else if (rptPODetailsUpdate.Visible)
                status += validatePODetailsUpdate();

            return status;
        }

        private string validateGeneralInfo()
        {
            string status = string.Empty;

            //TODO:  add file validation
            if (!ucFileUpload.IsValid)
                status += "Please supply an order details file.\\n";

            return status;
        }

        private string validatePODetails()
        {
            string status = string.Empty;

            for (int i = 0; i < rptPODetails.Items.Count; i++)
            {
                RepeaterItem item = rptPODetails.Items[i];

                TextBox txtPONumber = (TextBox)item.FindControl("txtPONumber");
                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");

                if ((string.IsNullOrEmpty(txtPONumber.Text))
                || (string.IsNullOrEmpty(txtAmount.Text)))
                {
                    status = "Please supply valid PO details.\\n";
                    break;
                }
            }
            return status;
        }

        private string validatePODetailsUpdate()
        {
            string status = string.Empty;

            for (int i = 0; i < rptPODetailsUpdate.Items.Count; i++)
            {
                RepeaterItem item = rptPODetailsUpdate.Items[i];

                CADateTimeControl cdtProcessDate = (CADateTimeControl)item.FindControl("cdtProcessDate");
                TextBox txtInvoiceNumber = (TextBox)item.FindControl("txtInvoiceNumber");
                TextBox txtDocumentNumber = (TextBox)item.FindControl("txtDocumentNumber");

                if (cdtProcessDate.IsDateEmpty || (string.IsNullOrEmpty(txtInvoiceNumber.Text))
                || (string.IsNullOrEmpty(txtDocumentNumber.Text)))
                {
                    status = "Please supply valid PO details.\\n";
                    break;
                }
            }
            return status;
        }

        public void Update()
        {
            updateGeneralInfo();
            if (rptPODetails.Visible)
                updatePODetails();
            else if (rptPODetailsUpdate.Visible)
                updatePODetailsUpdate();
        }

        private void updateGeneralInfo()
        {
        }

        private void updatePODetails()
        {
            dtPODetails.Rows.Clear();

            for (int i = 0; i < rptPODetails.Items.Count; i++)
            {
                RepeaterItem item = rptPODetails.Items[i];

                TextBox txtPONumber = (TextBox)item.FindControl("txtPONumber");
                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");

                DataRow row = dtPODetails.Rows.Add();
                row["PONumber"] = txtPONumber.Text;
                row["Amount"] = txtAmount.Text;
            }
        }

        private void updatePODetailsUpdate()
        {
            for (int i = 0; i < rptPODetailsUpdate.Items.Count; i++)
            {
                RepeaterItem item = rptPODetailsUpdate.Items[i];

                CADateTimeControl cdtProcessDate = (CADateTimeControl)item.FindControl("cdtProcessDate");
                TextBox txtInvoiceNumber = (TextBox)item.FindControl("txtInvoiceNumber");
                TextBox txtDocumentNumber = (TextBox)item.FindControl("txtDocumentNumber");

                DataRow row = dtPODetails.Rows[i];
                row["ProcessDate"] = (cdtProcessDate.IsDateEmpty? string.Empty : cdtProcessDate.SelectedDate.ToShortDateString());
                row["InvoiceNumber"] = txtInvoiceNumber.Text;
                row["DocumentNumber"] = txtDocumentNumber.Text;
            }
        }

        public string Submit()
        {
                return ucFileUpload.SubmitFile();
        }
    }
}