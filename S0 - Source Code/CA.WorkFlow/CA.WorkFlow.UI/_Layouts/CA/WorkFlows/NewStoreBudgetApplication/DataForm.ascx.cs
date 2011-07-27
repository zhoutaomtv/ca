using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;


using CA.WorkFlow.UI;
using CodeArt.SharePoint.CamlQuery;
using System.Text;

namespace CA.WorkFlow.UI.NewStoreBudgetApplication
{
    public partial class DataForm : QFUserControl
    {
        private Employee _Applicant;
        public Employee Applicant
        {
            get { return _Applicant; }
            set { _Applicant = value; }
        }

        public string ApplicantName
        {
            get
            {
                if (this.Applicant != null && !string.IsNullOrEmpty(this.Applicant.DisplayName))
                {
                    return this.Applicant.DisplayName;
                }
                else
                {
                    return SPContext.Current.ListItem["Applicant"] + "";
                }
            }
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

  
        public string amount
        {
            get
            {
                return ffTotalCost.Value.ToString();
            }
        }

    
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Applicant = this.CurrentEmployee;
            if (!IsPostBack)
            {   
                
                if (this.ControlMode == SPControlMode.New)
                {
                    this.lblWorkflowNumber.Text = "NA";
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

           
        }
        public string validateGeneralInfo()
        {
            string status = string.Empty;

            if (string.IsNullOrEmpty(this.ffStoreName.Value.ToString()))
                status += "Please supply Store Name.\\n";
            if (string.IsNullOrEmpty(this.ffGrossArea.Value.ToString()))
                status += "Please supply Gross Area.\\n";
            if (string.IsNullOrEmpty(this.ffNSA.Value.ToString()))
                status += "Please supply NSA.\\n";
            if (string.IsNullOrEmpty(this.ffTotalCost.Value.ToString()))
                status += "Please supply Total Cost.\\n";
            if (string.IsNullOrEmpty(this.ffCrossPerm2.Value.ToString()))
                status += "Please supply Gross/㎡.\\n";
            if (string.IsNullOrEmpty(this.ffNSAPerm2.Value.ToString()))
                status += "Please supply NSA/㎡.\\n";
            if(!ucFileUpload.IsValid)
               status += "Please supply the budget detail."; 

            return status;
        }

        public string Submit()
        {
            return ucFileUpload.SubmitFile();
        }
    }
}