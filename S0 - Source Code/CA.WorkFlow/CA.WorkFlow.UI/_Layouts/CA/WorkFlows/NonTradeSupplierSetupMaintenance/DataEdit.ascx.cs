namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using QuickFlow.UI.ListForm;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;

    public partial class DataEdit : NonTradeSupplierSetupMaintenanceControl
    {
        public int PaymentTerm { get {
            string v = this.Payment_Term.Value.AsString();
            if (v.IsNullOrWhitespace())
            {
                return int.MaxValue;
            }
            return Convert.ToInt32(v);
        } }

        public string RecordType { get { return this.Record_Type.Value.AsString(); } }
        public string ApplicantAccount { get; set; }
        public string DepartmentVal { get; set; }
        public string msg { set; get; }

        protected override void OnLoad(EventArgs e)
        {
            string cssClass = this.RecordType.Equals("New", StringComparison.InvariantCultureIgnoreCase) ? "hidden" : "block";

            this.trVendorId.Attributes["class"] = cssClass;
            this.tblChangeReason.Attributes["class"] = "ca-workflow-form-table " + cssClass;

            this.RegisterClientValidation();
        }

        public void SetVendorByVendId(string vendId)
        {
            bool isNewType = this.RecordType.Equals("New", StringComparison.InvariantCultureIgnoreCase);
            var result = this.FilterVendor(vendId, !isNewType, isNewType ? this.ApplicantAccount : null, isNewType ? null : this.DepartmentVal);

            SetVendor(result, isNewType);
        }

        public void SetVendorByWFNumber(string selectedWorkflowNumber)
        {
            bool isNewType = this.RecordType.Equals("New", StringComparison.InvariantCultureIgnoreCase);
            var result = this.FilterVendor(selectedWorkflowNumber, string.Empty, string.Empty, !isNewType, null, this.DepartmentVal);

            SetVendor(result, isNewType);
        }

        private void SetVendor(SPListItemCollection result, bool isNewType)
        {
            if (result.Count == 0)
            {
                return;
            }

            var vendor = result[0];

            this.Vendor_ID.Value = vendor[this.Vendor_ID.FieldName];
            foreach (var f in this.Controls.Cast<Control>().Where(c => c.ID != null).OfType<FormField>())
            {
                string fieldName = f.FieldName;

                if (vendor.Fields.ContainsField(fieldName))
                {
                    f.Value = vendor[fieldName];
                }
            }
            //if (isNewType)
            //{
            //    foreach (var f in this.Controls.Cast<Control>().Where(c => c.ID != null).OfType<FormField>())
            //    {
            //        string fieldName = f.FieldName;

            //        if (vendor.Fields.ContainsField(fieldName))
            //        {
            //            f.Value = vendor[fieldName];
            //        }
            //    }
            //}
            //else
            //{                
            //    this.Vendor_ID.Value = vendor[this.Vendor_ID.FieldName];
            //    this.CN_Name_of_Vendor.Value = vendor[this.CN_Name_of_Vendor.FieldName];
            //    this.EN_Name_of_Vendor.Value = vendor[this.EN_Name_of_Vendor.FieldName];
            //}
        }

        public override void UpdateValues()
        {
            bool isNewForm = string.Equals(this.RecordType, "New", StringComparison.InvariantCultureIgnoreCase);

            if (isNewForm)
            {
                this.Vendor_ID.Value = string.Empty;
                this.Reason_For_Change.Value = string.Empty;
            }
        }

        public override bool Validate()
        {
            bool isNewForm = string.Equals(this.RecordType, "New", StringComparison.InvariantCultureIgnoreCase);

            if (isNewForm)
            {
                if (this.CN_Name_of_Vendor.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.EN_Name_of_Vendor.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.CN_Address_of_Vendor.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.EN_Address_of_Vendor.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.CN_City_Country.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.EN_City_Country.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.CN_Postal_Code.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.EN_Postal_Code.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Company_Telephone_No.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Last_Name_of_Contact_Person.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.First_Name_of_Contact_Person.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Department.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }
                if (this.Phone_of_Contact_Person.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Business_License_No.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Tax_Registration_License_No.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Name_of_Bank.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Branch_Name.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Country_of_Bank.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                if (this.Bank_Account_No.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                string paymentTerm = this.Payment_Term.Value.AsString();

                if (paymentTerm.IsNullOrWhitespace())
                {
                    return false;
                }

                int v;

                return int.TryParse(paymentTerm, out v) && v >= 0;
            }
            else
            {
                if (this.Vendor_ID.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }

                //Check the inputed Vend ID is valid.
                //According to requirement, the vendor ID won't be checked again. since there are some old data.
                //var result = this.FilterVendor(this.Vendor_ID.Value.AsString());

                //if (result.Count == 0)
                //{
                //    return false;
                //}

                var isExist = this.isExistRunningVendor(this.Vendor_ID.Value.AsString(), this.DepartmentVal);
                if (isExist)
                {
                    msg = "There is the running non-trade supplier setup maintenance for the inputed supplier id.";
                    return false;
                }

                //If the record type is "Change" or "Block", the last one record should be null or the one with record type "Block"
                var vendor = this.GetLastVendRecord(this.Vendor_ID.Value.AsString(), this.DepartmentVal);
                if (vendor != null && vendor["Record Type"].AsString().Equals("Block", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!this.RecordType.Equals("Release", StringComparison.CurrentCultureIgnoreCase))
                    {
                        msg = "The inputed supplier is blocked, you need to release it first.";
                        return false;
                    }
                }

                if (this.Reason_For_Change.Value.AsString().IsNullOrWhitespace())
                {
                    return false;
                }
            }

            return true;
            
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            var vendId = this.Vendor_ID.Value.AsString();
            if (vendId.IsNullOrWhitespace())
            {
                return;
            }
            SetVendorByVendId(vendId);
        }

       
    }
}