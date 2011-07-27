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
using QuickFlow.Core;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using System.Text;
using CA.WorkFlow.UI;


namespace CA.WorkFlow.UI.StoreSampling
{
    public partial class DataForm : QFUserControl
    {
        public string WorkflowNumber
        {
            get
            {
                return lblWorkflowNumber.Text;
            }
            set{
                lblWorkflowNumber.Text = value;
                ucFileUpload.WorkflowNumber = value;
            }
        }

        public string FileName
        {
            get
            {
                return ucFileUpload.FileFullName;
            }
        }

       

        public string StoreNumber
        {
            get
            {
                return ddlStoreNumber.SelectedValue;
            }
        }

        public string PickedBy
        {
            get
            {
                return CAPeopleFinder1.Accounts.Count > 0 ? CAPeopleFinder1.Accounts[0] + "" : string.Empty;
            }
            set
            {
                CAPeopleFinder1.CommaSeparatedAccounts = value;
            }
        }

        public string CostCenter
        {
            get
            {
                return ddlCostCenter.SelectedValue;
            }
        }
        public string TextBoxCostCenter
        {
            get
            {
                return txtCostCenter.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                lblLoginName.Text = SPContext.Current.Site.RootWeb.CurrentUser.LoginName;

                ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);
                SPListItemCollection stores = sps.GetList("Stores").GetItems(new SPQuery());
                for (int i = 0; i < stores.Count; i++)
                {
                    ddlStoreNumber.Items.Add(new ListItem(stores[i]["Store Number"] + " " + stores[i]["DisplayName"], stores[i]["Store Number"] + ""));
                    ddlCostCenter.Items.Add(new ListItem(stores[i]["Cost Center"] + " ", stores[i]["Cost Center"] + ""));
                }

                ddlCostCenter.Items.Insert(0, new ListItem("Select...",""));
                LoadData();
                
            }
            
        }


        

        private void LoadData()
        {
            if (this.ControlMode != SPControlMode.New)
            {
                //load main
                SPListItem curItem = SPContext.Current.ListItem;

                lblWorkflowNumber.Text = curItem["WorkflowNumber"] + "";
                ucFileUpload.WorkflowNumber = curItem["WorkflowNumber"] + "";
                //txtStoreNumber.Text = curItem["Store Number"] + "";
                ddlIssuedTo.SelectedValue = curItem["Issued to"] + "";
                txtActualQuantity.Text = curItem["Actual Quantity"] + "";
                this.CADateTime1.SelectedDate = DateTime.Parse(curItem["Picked Time"] + "");
                ddlStoreNumber.SelectedValue = curItem["Store Number"] + "";
               
                ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);
                SPListItemCollection stores = sps.GetList("Stores").GetItems(new SPQuery());
                string costcenter= curItem["Cost Center"] + "";
                for (int i = 0; i < stores.Count; i++)
                {
                    if (costcenter.Equals(stores[i]["Cost Center"] + "", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ddlCostCenter.SelectedValue = costcenter;
                        txtCostCenter.Text = string.Empty;
                        break;
                    }
                    else
                    {
                        ddlCostCenter.SelectedValue = string.Empty;
                        txtCostCenter.Text = costcenter;
                    }
                }
                CAPeopleFinder1.CommaSeparatedAccounts = new SPFieldLookupValue(curItem["Picked by"] + "").LookupValue;

                string fileName = SPContext.Current.ListItem["FileName"] + "";
                if (!string.IsNullOrEmpty(fileName))
                {
                    ucFileUpload.WorkflowNumber = lblWorkflowNumber.Text;
                    ucFileUpload.FileFullName = fileName;
                }

                if (this.ControlMode == SPControlMode.Edit)
                {
                    switch (WorkflowContext.Current.Task.Step)
                    {
                        case "BuyerApprove":
                            FreezeForm();
                            break;
                        case "StoreManagerApprove":
                            FreezeForm();
                            break;
                        case "AreaManagerApprove":
                            FreezeForm();
                            break;
                        case "BSSHeadApprove":
                            FreezeForm();
                            break;
                        case "BSSTeamApprove":
                            hideCostCenter.Visible = true;
                            ddlCostCenter.Enabled = true;
                            txtCostCenter.Enabled = true;
                            FreezeForm();
                            break;
                        case "FinanceConfirm":
                            hideCostCenter.Visible = true;
                            FreezeForm();
                            break;
                        default:
                            break;
                    }
                }
                else if (this.ControlMode == SPControlMode.Display)
                {
                    hideCostCenter.Visible = true;
                    FreezeForm();
                }
            }
        }

        
        void FreezeForm()
        {
            //Panel1.Enabled = false;
            ddlStoreNumber.Enabled = false;
            ddlIssuedTo.Enabled = false;
            txtActualQuantity.Enabled = false;
            CADateTime1.Enabled = false;
            CAPeopleFinder1.Enabled = false;
            if (string.IsNullOrEmpty(ucFileUpload.FileFullName))
            {
                Panel2.Enabled = false;
            }
            else
            {
                ucFileUpload.HideDelete = true;
            }
            //else
            //{
            //    this.ControlMode = SPControlMode.Display;
            //}
        }

        

        //public override void SetControlMode()
        //{
        //    base.SetControlMode();

        //    if (ControlMode == SPControlMode.Display)
        //    {
        //        rpt1.Visible = false;
        //        rpt1Display.Visible = true;
        //        btnAdd1.Visible = false;
        //    }
        //    else
        //    {
        //        rpt1.Visible = true;
        //        rpt1Display.Visible = false;
        //        btnAdd1.Visible = true;
        //    }
        //}

        public string Validate()
        {
            return ValidateMain();
        }
      
        private string ValidateMain()
        {
            StringBuilder output = new StringBuilder();

            //if (string.IsNullOrEmpty(txtStoreNumber.Text))
            //    output.Append("Please supply a Store number.\\n");
            if (string.IsNullOrEmpty(ddlIssuedTo.SelectedValue))
                output.Append("Please supply Issued To.\\n");
            //if (string.IsNullOrEmpty(hidPickedBy.Value) && string.IsNullOrEmpty(lblPickedBy.Text))
            if (string.IsNullOrEmpty(CAPeopleFinder1.CommaSeparatedAccounts))
                output.Append("Please check Picked By.\\n");
            return output.ToString();
        }
      
        public string Submit()
        {
            return ucFileUpload.SubmitFile();
        }
        
      
       
    }
}