namespace CA.WorkFlow.UI.CreationOrder
{
    using System;
    using Microsoft.SharePoint.WebControls;
    using SharePoint.Utilities.Common;
    using QuickFlow.Core;

    public partial class DataView : CreateOrderUserControl
    {
        public string msg { set; get; }
        public string Department { set; get; }

        protected override void OnLoad(EventArgs e)
        {
            bool isFinanceAnalystTaskStep = this.CurrentStep.IsNotNullOrWhitespace() && string.Equals(this.CurrentStep, "FinanceAnalystTask", StringComparison.InvariantCultureIgnoreCase);

            this.RequireValidation = isFinanceAnalystTaskStep;

            this.Order_Number.ControlMode = isFinanceAnalystTaskStep ? SPControlMode.Edit : SPControlMode.Display;

            this.RegisterClientValidation();
            base.OnLoad(e);
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
            else if (action.Equals("Confirm", StringComparison.CurrentCultureIgnoreCase))
            {
                if (this.Order_Number.Value.AsString().IsNullOrWhitespace())
                {
                    msg = "Please fill in Order Number field.";
                    return false;
                }
                if (this.isExistOrder(this.Order_Number.Value.AsString(), Department))
                {
                    msg = "There is the existed internal order. Please assign a new one.";
                    return false;
                }
            }
            return true;
        }
    }
}