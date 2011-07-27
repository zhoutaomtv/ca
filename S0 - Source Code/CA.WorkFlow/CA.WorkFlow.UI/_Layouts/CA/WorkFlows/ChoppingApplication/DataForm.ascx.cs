using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using QuickFlow.Core;


namespace CA.WorkFlow.UI.ChoppingApplication
{
    public partial class DataForm : QFUserControl
    {
        public struct Constants
        {
            public const string WorkflowName = "Chopping Application Workflow";

            public const string HeadApproveStep = "DepartmentHeaderApprove";
            public const string HREeviewStep = "HRReview";
            public const string HRHeadApproveStep = "HRHeadApprove";
            public const string CEOApprove = "CEOApprove";
            public const string LegalHeadApprove = "LegalHeadApprove";

            public const string FieldNameRepositoryDepartmen = "RepositoryDepartment";
            public const string FieldNameChop = "Chop";

            public const string DocumentChoppingProjectFiles = "ChoppingProjectFiles";
        }

        public override SPControlMode ControlMode
        {
            get
            {
                return base.ControlMode;
            }
            set
            {
                base.ControlMode = value;
                this.FormField1.ControlMode = value;
                this.FormFieldCEO.ControlMode = value;
                this.FormFieldCFCO.ControlMode = value;
                this.FormFieldCFCO2.ControlMode = value;
                this.FormFieldDepartment.ControlMode = value;
                this.FormFieldManager.ControlMode = value;
                this.FormFieldSigner.ControlMode = value;              
                
            }
        }

        public string CEOAccount
        {
            get
            {
                try
                {
                    return GetUserAccountByID(this.FormFieldCEO.Value);
                }
                catch
                {
                    return string.Empty;
                }
            }

        }

        public string DeptManagerAccount
        {
            get
            {
                return GetUserAccountByID(this.FormFieldManager.Value);
            }
        }

        public string SignerAccount
        {
            get
            {
                return GetUserAccountByID(this.FormFieldSigner.Value);
            }
        }

        public string CFCOAccount
        {
            get
            {
                return GetUserAccountByID(this.FormFieldCFCO.Value);
            }
        }

        public string CFCO2Account
        {
            get
            {
                return GetUserAccountByID(this.FormFieldCFCO2.Value);
            }
        }

        public string WorkFlowNumber
        {
            get
            {
                return ViewState["WorkFlowNumber"] + "";
            }
            set { ViewState["WorkFlowNumber"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillData();
            }
            else
            {
                switch (this.ControlMode)
                {
                    case SPControlMode.New:
                        string LegalCounsel = UserProfileUtil.GetDepartmentManager("legal");
                        this.pfinderLegal.CommaSeparatedAccounts = LegalCounsel;
                        break;
                }
            }
        }

        public string Projects
        {
            get
            {
                return this.ProjectMultiTable1.Projects;
            }
        }

        private void FillData()
        {
            string LegalCounsel = UserProfileUtil.GetDepartmentManager("legal");
            switch (this.ControlMode)
            {
                case SPControlMode.New:
                    this.labPeople.Text = this.CurrentEmployee.DisplayName;
                    this.labDepartment.Text = this.CurrentEmployee.Department;
                    string workflowNum = CreateWorkFlowNumber();
                    this.WorkFlowNumber = workflowNum;
                    this.labNumber.Text = workflowNum;

                    this.ProjectMultiTable1.WorkFlowNumber = workflowNum;
                    
                    this.pfinderLegal.CommaSeparatedAccounts = LegalCounsel;
                   
                    this.labLeagal.Visible = false;
                    break;
                case SPControlMode.Edit:
                    this.labPeople.Text = new SPFieldLookupValue(SPContext.Current.ListItem["Created By"] + "").LookupValue;
                    this.ProjectMultiTable1.WorkFlowNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
                    this.labDepartment.Text = SPContext.Current.ListItem["Department"] + "";
                    this.labNumber.Text =  SPContext.Current.ListItem["WorkFlowNumber"] + "";
                    this.labLeagal.Text = new SPFieldLookupValue(SPContext.Current.ListItem["LegalCounsel"] + "").LookupValue;
                    this.pfinderLegal.Visible = false;
                    break;
                case SPControlMode.Display:
                    this.labPeople.Text = new SPFieldLookupValue(SPContext.Current.ListItem["Created By"] + "").LookupValue;
                    this.ProjectMultiTable1.WorkFlowNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
                    this.labNumber.Text = SPContext.Current.ListItem["WorkFlowNumber"] + "";
                    this.labDepartment.Text = SPContext.Current.ListItem["Department"] + "";
                    this.labLeagal.Text = new SPFieldLookupValue(SPContext.Current.ListItem["LegalCounsel"] + "").LookupValue;
                    this.pfinderLegal.Visible = false;
                    break;
            }

        }


        private string CreateWorkFlowNumber()
        {
            return "CA_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + WorkFlowUtil.CreateWorkFlowNumber("Chopping").ToString("000000");
        }

        private string GetUserAccountByID(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else if (string.IsNullOrEmpty(obj + ""))
            {
                return string.Empty;
            }
            int id = Int32.Parse(obj + "");
            SPUser user = SPContext.Current.Site.RootWeb.SiteUsers.GetByID(id);
            return user.LoginName;
        }

        private int GetUserIDByAccount(string account)
        {
            SPUser user = SPContext.Current.Site.RootWeb.SiteUsers[account];
            return user.ID;
        }
    }
}