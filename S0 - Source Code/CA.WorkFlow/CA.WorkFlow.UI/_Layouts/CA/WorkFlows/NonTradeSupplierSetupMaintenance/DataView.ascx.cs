namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using Microsoft.SharePoint.WebControls;
    using SharePoint.Utilities.Common;
    using QuickFlow.Core;

    public partial class DataView : NonTradeSupplierSetupMaintenanceControl
    {
        public string RecordType { get; set; }
        public string msg { set; get; }
        public string DepartmentVal { set; get; }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                bool isNewType = this.RecordType.Equals("New", StringComparison.InvariantCultureIgnoreCase);
                // At MDMTask step, the user will fill in the vend id which got from SAP system. In another step, the field will be can't editable
                if (this.CurrentStep.IsNotNullOrWhitespace())
                {
                    bool isNewMdmTask = isNewType && this.CurrentStep.Equals("MDMTask", StringComparison.InvariantCultureIgnoreCase);
                    this.RequireValidation = isNewMdmTask;
                    this.Vendor_ID.ControlMode = isNewMdmTask ? SPControlMode.Edit : SPControlMode.Display;
                }
                else
                {
                    //When CurrentStep is empty, the page should be DisplayForm. The Vend ID should not be editable.
                    this.RequireValidation = false;
                    this.Vendor_ID.ControlMode = SPControlMode.Display;
                }


                //In Approve and Display pages, the RecordType should be set by the record type.
                this.tblChangeReason.Visible = !isNewType;

                this.RegisterClientValidation();

                base.OnLoad(e);
            }
            
        }

        public override bool Validate()
        {
            return this.Vendor_ID.Value.AsString().IsNotNullOrWhitespace();
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
                if (this.Vendor_ID.Value.AsString().IsNotNullOrWhitespace())
                {
                    msg = "Please fill in Order Number field.";
                    return false;
                }
                if (this.isExistVendor(this.Vendor_ID.Value.ToString(), DepartmentVal))
                {
                    msg = "There is the existed non-trade supplier. Please assign a new one.";
                    return false;
                }
            }
            return true;
        }
    }
}