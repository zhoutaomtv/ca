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

namespace CA.WorkFlow.UI.ChangeRequest
{
    public partial class DataForm : QFUserControl
    {
        public string WorkflowNumber
        {
            get
            {
                return lblWorkflowNumber.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.ControlMode == SPControlMode.New)
                {
                    lblLoginName.Text = SPContext.Current.Site.RootWeb.CurrentUser.LoginName;

                    //txtChangeRequestNumber.Enabled = false;
                    DisablePart2();
                    //attacthment.ControlMode = SPControlMode.Edit;
                }
                else
                {
                    SPListItem curItem = SPContext.Current.ListItem;

                    lblLoginName.Text = new SPFieldLookupValue(curItem["Created By"].ToString()).LookupValue;

                    lblWorkflowNumber.Text = curItem["WorkflowNumber"] + "";
                    ddlPriority.SelectedValue = curItem["Priority"] + "";
                    ddlArea.SelectedValue = curItem["Area"] + "";
                    ddlSystem.SelectedValue = curItem["System"] + "";
                    ddlRequirementType.SelectedValue = curItem["RequirementType"] + "";
                    txtSubject.Text=curItem["Subject"]+"";
                    txtDescription.Text = curItem["Description"] + "";
                    txtBusinessLogic.Text = curItem["BusinessLogic"] + "";
                    txtChangeRequestNumber.Text = curItem["ChangeRequestNumber"] + "";

                    if (this.ControlMode == SPControlMode.Edit)
                    {
                        switch (WorkflowContext.Current.Task.Step)
                        {
                            case "ITHeadApprove":
                                DisablePartAll();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;
                            case "ITHeadApprove2":
                                DisablePartAll();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;
                            case "ITAppManagerGroupExecutes":
                                DisablePartAll();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;
                            case "ITAppManagerGroupSupplies":
                                DisablePart1();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;
                            case "BusinessManagerGroupApprove":
                                DisablePartAll();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;
                            case "BusinessManagerGroupApprove2":
                                DisablePartAll();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;
                            //case "ITAndBMGroupApprove":
                            //    DisablePartAll();
                            //    break;
                            //case "ITAndBMGroupApprove2":
                            //    DisablePartAll();
                            //    break;
                            case "EmployeeSubmit":
                                DisablePart2();
                                break;
                            case "EmployeeTests":
                                DisablePartAll();
                                attacthment.ControlMode = SPControlMode.Display;
                                break;

                        }
                    }
                    else if (this.ControlMode == SPControlMode.Display)
                    {
                        DisablePartAll();
                    }
                }

            }
           

        }

        void DisablePart1()
        {
            //Panel1.Enabled = false;
            ddlArea.Enabled = false;
            ddlPriority.Enabled = false;
            ddlRequirementType.Enabled = false;
            ddlSystem.Enabled = false;
            txtSubject.Enabled = false;
            //txtDescription.Enabled = false;
            //txtBusinessLogic.Enabled = false;
            attacthment.ControlMode = SPControlMode.Display;
        }

        void DisablePart2()
        {
            //Panel2.Enabled = false;
            txtChangeRequestNumber.Enabled = false;
        }

        void DisablePartAll()
        {
            DisablePart1();
            DisablePart2();
        }
    }
}