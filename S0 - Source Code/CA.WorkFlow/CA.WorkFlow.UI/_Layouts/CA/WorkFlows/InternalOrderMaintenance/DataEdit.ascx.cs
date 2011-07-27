namespace CA.WorkFlow.UI.InternalOrderMaintenance
{
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint.WebControls;
    using System.Text;

    public partial class DataEdit : InternalOrderUserControl
    {
        private readonly ObjectDataSource dataSource = new ObjectDataSource();

        protected override void OnLoad(EventArgs e)
        {
            this.dataSource.ID = "InternalOrderChangeHistoryDS";
            this.dataSource.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSource.SelectMethod = "GetChangeHistory";
            this.Controls.Add(this.dataSource);

            if (!this.Page.IsPostBack)
            {
                if (OrderNumber != null)
                {
                    SPListItem orderItem = this.GetOrderInfo(OrderNumber, this.Department);
                    if (orderItem != null)
                    {
                        this.SetOrderInfo(orderItem);

                        this.dataSource.SelectParameters.Clear();
                        this.SPGridView1.DataSourceID = "InternalOrderChangeHistoryDS";
                        this.dataSource.SelectParameters.Add("orderNumber", DbType.String, OrderNumber);
                        this.dataSource.SelectParameters.Add("department", DbType.String, Department);
                        this.SPGridView1.DataBind();
                    }                    
                }                
            }            

            this.RegisterClientValidation();
            base.OnLoad(e);
        }

        protected void BtnFinder_Click(object sender, EventArgs e)
        {
            string orderNumber = this.Order_Number.Value.AsString();

            if (orderNumber.IsNullOrWhitespace())
            {
                return;
            }

            //Set order info
            SPListItem orderItem = this.GetOrderInfo(orderNumber, this.Department);
            if (orderItem != null)
            {
                this.SetOrderInfo(orderItem);

                //Set Budget Change History
                this.dataSource.SelectParameters.Clear();
                this.SPGridView1.DataSourceID = "InternalOrderChangeHistoryDS";
                this.dataSource.SelectParameters.Add("orderNumber", DbType.String, orderNumber);
                this.dataSource.SelectParameters.Add("department", DbType.String, this.Department);
                this.SPGridView1.DataBind();
            }
            else
            {
                this.Internal_Order_Type.Text = string.Empty;
                this.Description.Text = string.Empty;
                this.Business_Area.Text = string.Empty;
                this.Origin_Value.Text = string.Empty;
                this.Effective_Date.Text = string.Empty;
                this.Expired_Date.Text = string.Empty;
                this.Attachment1.Text = string.Empty;
            }
        }

        //Set the order info in ASP.NET label controls for page
        private void SetOrderInfo(SPListItem orderItem)
        {
            //Set the control value
            this.Internal_Order_Type.Text = orderItem["Internal Order Type"] as string;
            this.Description.Text = orderItem["Description"] as string;
            this.Business_Area.Text = orderItem["Business Area"] as string;
            this.Origin_Value.Text = orderItem["Original Value"].AsString();
            //this.Effective_Physical_Year.Text = orderItem["Effective Physical Year"] as string;
            this.Effective_Date.Text = DateTime.Parse(orderItem["Effective Date"].AsString()).ToShortDateString();
            var expiredDate = orderItem["Expired Date"].AsString();
            this.Expired_Date.Text = expiredDate.IsNotNullOrWhitespace() ? DateTime.Parse(expiredDate).ToShortDateString() : string.Empty;

            this.Attachment1.Text = GetAttachTable(orderItem);            
        }

        //Validate the fields are not blank and the input order number is valid.
        //Add: If there is order maintenance for this order is running, NOT ALLOW submit.
        public override bool Validate()
        {
            if (this.Order_Number.Value.AsString().IsNullOrWhitespace())
            {
                return false;
            }
            if (this.Value_After_Change.Value.AsString().IsNullOrWhitespace())
            {
                return false;
            }
            if (this.Reason_For_Change_Value.Value.AsString().IsNullOrWhitespace())
            {
                return false;
            }

            //Check the inputed Order Number is valid
            var result = this.GetOrderInfo(this.Order_Number.Value.AsString(), this.Department);
            if (result == null)
            {
                msg = "The inputed order number is invalid.";
                return false;
            }

            //Check whehter there is running maintenance for the order.
            bool isExist = this.IsExistRunningMaintenance(this.Order_Number.Value.AsString(), this.Department);
            if (isExist)
            {
                msg = "There is the running internal order maintenance for the inputed order number.";
                return false;
            }

            return true;
        }

        public string OrderNumber { set; get; }
        public string Department { set; get; }
        public string msg { set; get; }

        //Return the last value for the speical order
        public double GetLastValue(string orderNumber)
        {
            double lastValue = 0d;
            SPListItem item = GetLatestHistory(orderNumber, this.Department);
            if (item != null)
            {
                lastValue = Convert.ToDouble(item["Value After Change"]);
            }
            else
            {
                item = GetOrderInfo(orderNumber, this.Department);
                lastValue = Convert.ToDouble(item["Original Value"]);
            }
            return lastValue;
        }
    }
}