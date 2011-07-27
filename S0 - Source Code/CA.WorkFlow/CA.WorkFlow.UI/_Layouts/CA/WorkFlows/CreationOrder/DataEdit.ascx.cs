namespace CA.WorkFlow.UI.CreationOrder
{
    using System;
    using SharePoint.Utilities.Common;

    public partial class DataEdit : BaseWorkflowUserControl
    {
        public string msg { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack && this.CurrentStatus.IsNullOrWhitespace())
            {
                this.Origin_Value.Value = 0;
                //this.Effective_Physical_Year.Value = DateTime.Now.Year;
                this.Effective_Date.Value = DateTime.Now;
            }
        }

        public override bool Validate()
        {
            if (this.Description.Value.AsString().IsNullOrWhitespace())
            {
                return false;
            }

            if (this.Business_Area.Value.AsString().IsNullOrWhitespace())
            {
                return false;
            }

            string effectiveDate = this.Effective_Date.Value.AsString();

            DateTime dt1;
            DateTime dt2;

            if (effectiveDate.IsNullOrWhitespace() || !DateTime.TryParse(effectiveDate, out dt1))
            {
                return false;
            }

            string t = this.Origin_Value.Value.AsString();

            float v;

            if (t.IsNullOrWhitespace() || !float.TryParse(t, out v) || v < 0)
            {
                return false;
            }

            string expiredDate = this.Expired_Date.Value.AsString();

            if (expiredDate.IsNotNullOrWhitespace())
            {
                if (!DateTime.TryParse(expiredDate, out dt2))
                {
                    return false;
                }

                if (dt1 > dt2)
                {
                    msg = "effective date should be smaller than or equal to the expired date.";
                    return false;
                }
            }

            return true;
        }
    }
}