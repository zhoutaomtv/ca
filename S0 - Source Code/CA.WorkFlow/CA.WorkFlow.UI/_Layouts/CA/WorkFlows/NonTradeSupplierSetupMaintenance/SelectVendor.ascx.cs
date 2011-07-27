namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using QuickFlow.UI.ListForm;
    using SharePoint.Utilities.Common;

    public partial class SelectVendor : NonTradeSupplierSetupMaintenanceControl
    {
        private readonly ObjectDataSource dataSource = new ObjectDataSource();

        public string ApplicantAccount { get; set; }
        public string Department { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            this.dataSource.ID = "VendorDS";
            this.dataSource.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSource.SelectMethod = "GetDataTable";
            this.Controls.Add(this.dataSource);
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.SPGridView1.BorderStyle = BorderStyle.Solid;
                this.SPGridView1.GridLines = GridLines.Horizontal;

                this.SPGridView1.DataSource = null;
                this.SPGridView1.DataBind();
            }
        }

        protected void btnQuery_Click(object sendor, EventArgs e)
        {
            string enName = this.ENName.Text.Trim();
            string cnName = this.CNName.Text.Trim();
            this.hidSelectedWorkflowNumber.Value = string.Empty; //Clear old hidden value once clicking query button

            var lfc = this.Parent.FindControl("ListFormControl1") as ListFormControl;
            bool isNewVendor = (lfc.FindControl("DataForm1") as DataEdit).RecordType.Equals("New", StringComparison.InvariantCultureIgnoreCase);

            this.dataSource.SelectParameters.Clear();
            this.dataSource.SelectParameters.Add("workflowNumber", string.Empty);
            this.dataSource.SelectParameters.Add("enName", DbType.String, enName);
            this.dataSource.SelectParameters.Add("cnName", DbType.String, cnName);
            this.dataSource.SelectParameters.Add("isCompleted", DbType.Boolean, (!isNewVendor).ToString());
            this.dataSource.SelectParameters.Add("department", DbType.String, this.Department);

            this.SPGridView1.DataSourceID = "VendorDS";
            this.SPGridView1.DataBind();
        }

        protected void SPGridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SPGridView1.PageIndex = e.NewPageIndex;
            this.SPGridView1.DataBind();
        }

        protected void btnCopyVendor_Click(object sender, EventArgs e)
        {
            string selectedWorkflowNumber = this.hidSelectedWorkflowNumber.Value;

            if (selectedWorkflowNumber.IsNullOrWhitespace())
            {
                return;
            }

            var lfc = this.Parent.FindControl("ListFormControl1") as ListFormControl;

            (lfc.FindControl("DataForm1") as DataEdit).SetVendorByWFNumber(selectedWorkflowNumber);
            this.Reset();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        private void Reset()
        {
            this.hidSelectedWorkflowNumber.Value = string.Empty;

            this.ENName.Text = this.CNName.Text = string.Empty;

            this.SPGridView1.DataSource = null;
            this.SPGridView1.DataSourceID = null;
            this.SPGridView1.DataBind();
        }
    }
}