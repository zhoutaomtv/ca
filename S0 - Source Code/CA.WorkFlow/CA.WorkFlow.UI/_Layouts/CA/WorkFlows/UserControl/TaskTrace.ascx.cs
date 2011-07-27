using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.UC
{
    public partial class TaskTrace : System.Web.UI.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            this.ApplicantLabel.Text = this.Applicant;

            if (!this.Page.IsPostBack) 
            {
                this.Trace1.GridLines = GridLines.Horizontal;
                this.Trace1.BorderStyle = BorderStyle.Solid;
            }
        }

        public string Applicant
        {
            get { return this.ViewState["Applicant"].AsString(); }
            set { this.ViewState["Applicant"] = value;  }
        }
    }
}