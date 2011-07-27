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
using QuickFlow.Core;
using System.Text;

namespace CA.WorkFlow.UI.Equipment
{
    //added by wsq 2010-07-23
    public partial class DataForm : QFUserControl
    {
        public string Manager
        {
            get
            {
                return CAPeopleFinder1.Accounts.Count > 0 ? CAPeopleFinder1.Accounts[0] + "" : string.Empty;
            }
        }

        public string FunctionalManager
        {
            get
            {
                return CAPeopleFinder2.Accounts.Count > 0 ? CAPeopleFinder2.Accounts[0] + "" : string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.ControlMode == SPControlMode.New)
                {
                    lblHRName.Text = SPContext.Current.Site.RootWeb.CurrentUser.LoginName;
                    //txtOnBoardAt.Text = DateTime.Now.ToShortDateString();
                    this.CADateTime1.SelectedDate = DateTime.Now;
                    DisableDepartmentHeadPart();
                }
                else
                {
                    SPListItem curItem = SPContext.Current.ListItem;

                    lblHRName.Text = new SPFieldLookupValue(curItem["Created By"].ToString()).LookupValue;
                    txtEmployeeName.Text = curItem["EmployeeName"] + "";
                    txtEmployeeTitle.Text = curItem["EmployeeTitle"] + "";
                    txtEmployeeID.Text = curItem["EmployeeID"] + "";
                    //txtOnBoardAt.Text = DateTime.Parse(curItem["OnboardAt"] + "").ToShortDateString();
                    this.CADateTime1.SelectedDate = DateTime.Parse(curItem["OnboardAt"] + "");

                    txtDepartment.Text = curItem["Department"] + "";

                    txtRemark.Text = curItem["Remark"] + "";
                    radComputer.SelectedValue = curItem["Computer"] + "";
                    radEmail.SelectedValue = curItem["Email"] + "";
                    radSap.SelectedValue = curItem["Sap"] + "";
                    radTelephone.SelectedValue = curItem["Telephone"] + "";

                    CAPeopleFinder1.CommaSeparatedAccounts = new SPFieldLookupValue(curItem["Manager"] + "").LookupValue;
                    CAPeopleFinder2.CommaSeparatedAccounts = new SPFieldLookupValue(curItem["FunctionalManager"] + "").LookupValue;

                    if (this.ControlMode == SPControlMode.Edit)
                    {
                        switch (WorkflowContext.Current.Task.Step)
                        {
                            case "FunctionalManagerApprove":
                                DisableHrPart();
                                break;
                            case "DepartmentHeadApprove":
                                DisableHrPart();
                                break;
                            case "ITConfirm":
                                DisableHrPart();
                                DisableDepartmentHeadPart();
                                break;
                            //case "CFCOApprove":
                            //    DisableHrPart();
                            //    DisableDepartmentHeadPart();
                            //    break;
                            case "HRSubmit":
                                DisableDepartmentHeadPart();
                                break;

                        }
                    }
                    else if (this.ControlMode == SPControlMode.Display)
                    {
                        DisableHrPart();
                        DisableDepartmentHeadPart();
                    }
                }
                
            }
            
        }

        void DisableHrPart()
        {
            txtEmployeeName.Enabled = false;
            txtEmployeeTitle.Enabled = false;
            txtEmployeeID.Enabled = false;
            //txtOnBoardAt.Enabled = false;
            txtDepartment.Enabled = false;
            this.CAPeopleFinder1.Enabled = false;
            this.CAPeopleFinder2.Enabled = false;
            this.CADateTime1.Enabled = false;
        }

        void DisableDepartmentHeadPart()
        {
            radComputer.Enabled = false;
            radEmail.Enabled = false;
            radSap.Enabled = false;
            radTelephone.Enabled = false;
            txtRemark.Enabled = false;
        }

        public string Validate()
        {
            StringBuilder output = new StringBuilder();

            if (string.IsNullOrEmpty(txtEmployeeName.Text))
                output.Append("Please supply a employee name.\\n");

            if (string.IsNullOrEmpty(this.txtEmployeeTitle.Text))
                output.Append("Please supply a employee title.\\n");
            if (string.IsNullOrEmpty(this.txtEmployeeID.Text))
                output.Append("Please supply a employee id.\\n");
            //if (string.IsNullOrEmpty(CAPeopleFinder2.CommaSeparatedAccounts))
            //    output.Append("Please supply a functional manager.\\n");
            if (string.IsNullOrEmpty(CAPeopleFinder1.CommaSeparatedAccounts))
                output.Append("Please supply a department manager.\\n");

            return output.ToString();
        }

        
    }
}