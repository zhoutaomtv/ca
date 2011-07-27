namespace CA.WorkFlow.UI.InternalOrderMaintenance
{
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint.WebControls;
    using QuickFlow.Core;

    public partial class DataView : InternalOrderUserControl
    {
        private readonly ObjectDataSource dataSource = new ObjectDataSource();
        
        public string OrderNumber { set; get; }
        public string Department { set; get; }
        public string msg { set; get; }

        protected override void OnLoad(EventArgs e)
        {
            this.dataSource.ID = "InternalOrderChangeHistoryDS";
            this.dataSource.TypeName = this.GetType().FullName + "," + this.GetType().Assembly.FullName;
            this.dataSource.SelectMethod = "GetChangeHistory";
            this.Controls.Add(this.dataSource);
            base.OnLoad(e);

            SPListItem orderItem = this.GetOrderInfo(OrderNumber, Department);
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

        public override bool Validate(string action)
        {
            bool isValid = false;
            if (action.Equals("Reject", StringComparison.CurrentCultureIgnoreCase))
            {
                isValid = WorkflowContext.Current.TaskFields["Body"].AsString().IsNotNullOrWhitespace();
                if (!isValid)
                {
                    msg = "Please fill in the Reject Comments.";
                    return isValid;
                }
            }
            return true;
        }
    }
}