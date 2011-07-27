using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using CodeArt.SharePoint.CamlQuery;
using QuickFlow;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.ChangeRequest
{
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)DataForm1.FindControl("txtSubject")).Text))
            {
                e.Cancel = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please supply a Subject .');", true);
                return;
            }

            WorkflowContext curContext = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            string ITManager = UserProfileUtil.GetDepartmentManager("IT");
            if (string.IsNullOrEmpty(ITManager))
            {
                DisplayMessage("Unable to submit the application. There is no IT manager defined. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }

            //string passTo = ((TextBox)DataForm1.FindControl("txtManager")).Text; //UserProfileUtil.GetDepartmentManager("HR");
            string isNew = "Yes";
            if (((DropDownList)DataForm1.FindControl("ddlRequirementType")).SelectedValue == "Bug fix")
            {
                isNew = "No";
            }

            fields["Priority"] = ((DropDownList)DataForm1.FindControl("ddlPriority")).SelectedValue;
            fields["Area"] = ((DropDownList)DataForm1.FindControl("ddlArea")).SelectedValue;
            fields["System"] = ((DropDownList)DataForm1.FindControl("ddlSystem")).SelectedValue;
            fields["RequirementType"] = ((DropDownList)DataForm1.FindControl("ddlRequirementType")).SelectedItem.Text;
            fields["Subject"] = ((TextBox)DataForm1.FindControl("txtSubject")).Text;
            fields["Description"] = ((TextBox)DataForm1.FindControl("txtDescription")).Text;
            fields["BusinessLogic"] = ((TextBox)DataForm1.FindControl("txtBusinessLogic")).Text;
            
            fields["WorkflowNumber"] = CreateWorkflowNumber();
            fields["Status"] = "In Progress";
            curContext.UpdateWorkflowVariable("IsRequestNew", isNew);

            string taskTitle = SPContext.Current.Web.CurrentUser.Name + "'s Change Request";
            curContext.UpdateWorkflowVariable("ITHead", ITManager);
            curContext.UpdateWorkflowVariable("BusinessManagerGroup", "wf_BusinessManager");
            curContext.UpdateWorkflowVariable("ITAppManagerGroup", "wf_ITApplicationManager");

            //NameCollection bms = WorkFlowUtil.GetUsersInGroup("Business Manager Group");
            //bms.Add(itheadAccount);
            //curContext.UpdateWorkflowVariable("ITAndBMAccounts", bms);

            curContext.UpdateWorkflowVariable("EmployeeSubmitTitle", "Please complete request");
            curContext.UpdateWorkflowVariable("EmployeeTestsTitle", "Please confirm request tests");
            curContext.UpdateWorkflowVariable("ITHeadApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("ITHeadApprove2Title", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("ITAppManagerGroupExcutesTitle", taskTitle + " needs confirm");
            curContext.UpdateWorkflowVariable("ITAppManagerGroupSuppliesTitle", taskTitle + " needs information");
            curContext.UpdateWorkflowVariable("BusinessManagerGroupApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("BusinessManagerGroupApprove2Title", taskTitle + " needs approval");
        }

        private string CreateWorkflowNumber()
        {
            return "CR_" + WorkFlowUtil.CreateWorkFlowNumber("ChangeRequest").ToString("000000");
        }
    }
}
