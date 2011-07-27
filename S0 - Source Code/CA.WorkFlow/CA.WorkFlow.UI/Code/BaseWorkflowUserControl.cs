namespace CA.WorkFlow.UI
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using SharePoint.Utilities.Common;

    public class BaseWorkflowUserControl : UserControl
    {
        protected static string ClientValidationMethod = "return beforeSubmit(event);";

        public bool RequireValidation
        {
            get
            {
                return Convert.ToBoolean(this.ViewState["RequireValidation"]);
            }
            set
            {
                this.ViewState["RequireValidation"] = value;
            }
        }

        public string CurrentStep
        {
            get
            {
                return this.ViewState["CurrentStep"].AsString();
            }
            set
            {
                this.ViewState["CurrentStep"] = value;
            }
        }

        public string CurrentStatus
        {
            get
            {
                return this.ViewState["CurrentStatus"].AsString();
            }
            set
            {
                this.ViewState["CurrentStatus"] = value;
            }
        }

        public string ViewMode
        {
            get
            {
                return this.ViewState["ViewMode"].AsString();
            }
            set
            {
                this.ViewState["ViewMode"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.RegisterClientValidation();

            base.OnLoad(e);
        }

        protected virtual void RegisterClientValidation()
        {
            if (this.RequireValidation && !this.IsPostBack)
            {
                foreach (var wc in (from Control c in this.Parent.Controls where c.ID != null let b = c as IButtonControl where b != null && b.CausesValidation select c).OfType<WebControl>())
                {
                    wc.Attributes["onclick"] = ClientValidationMethod;
                }
            }
        }

        public virtual bool Validate()
        {
            return true;
        }

        public virtual bool Validate(string s)
        {
            return true;
        }

        public virtual void UpdateValues()
        {
        }
    }
}