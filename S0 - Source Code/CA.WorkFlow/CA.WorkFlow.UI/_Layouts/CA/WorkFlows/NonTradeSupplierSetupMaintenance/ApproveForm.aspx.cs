namespace CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance
{
    using System;
    using Microsoft.SharePoint;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using System.Collections.Generic;

    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, true))
            {
                RedirectToTask();
            }

            if (!this.Page.IsPostBack)
            {
                this.DataForm1.CurrentStep = WorkflowContext.Current.Task.Step;
                this.DataForm1.RecordType = WorkflowContext.Current.DataFields["Record Type"].AsString();
                this.DataForm1.DepartmentVal = WorkflowContext.Current.DataFields["DepartmentVal"].AsString();
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            //"MDMTask" is the last task in the normal workflow step. The user decision will be updated in Status field and shown in the page.
            string step = WorkflowContext.Current.Step;

            bool isApprove = e.Action.Equals("Approve");

            switch (step)
            {
                case "DepartmentHeadTask":
                    if (isApprove)
                    {
                        SaveToApprovers();
                        WorkflowContext.Current.DataFields["Status"] =  CAWorkflowStatus.NTVDepartmentHeadApprove;
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        WorkflowContext.Current.DataFields["Status"] =  CAWorkflowStatus.NTVDepartmentHeadReject;
                        SendMail(true);
                    }
                    break;
                case "CFOTask":
                    if (isApprove)
                    {
                        SaveToApprovers();
                        WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.NTVCFOApprove;
                    }
                    else
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.NTVCFOReject;
                        SendMail(true);
                    }
                    break;
                case "MDMTask":
                    if (e.Action == "Confirm")
                    {
                        WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.Completed;
                        SaveToApprovers();
                        SendMail(false);
                    }
                    else if (e.Action == "Reject")
                    {
                        if (!Validate(e.Action, e))
                        {
                            return;
                        }
                        WorkflowContext.Current.DataFields["Vendor ID"] = string.Empty;
                        WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.NTVFinanceReject;
                        SendMail(true);
                    }
                    break;
            }
        }

        private bool Validate(string action, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {
                this.lblError.Text = this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : string.Empty;
                e.Cancel = true;
                flag = false;
            }
            return flag;
        }

        //title - identity for the feature
        //isReject
        private void SendMail(bool isReject)
        {
            string title = "NonTradeSupplierSetupMaintenance";
            var emailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
            if (emailTemplate == null)
            {
                return;
            }
            string bodyTemplate = emailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string subject = emailTemplate["Subject"].AsString();
                
                string rootweburl = System.Configuration.ConfigurationManager.AppSettings["rootweburl"];
                string recordType = WorkflowContext.Current.DataFields["Record Type"].ToString(); //Record Type
                string vendId = WorkflowContext.Current.DataFields["Vendor ID"].AsString(); //Vend ID
                string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString(); //Approvers
                string applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();  //Applicant
                string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/NonTradeSupplierSetupMaintenance/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/NonTradeSupplierSetupMaintenanceWorkflow/MyApply.aspx";
                //string comment = WorkflowContext.Current.TaskFields["Body"].AsString();

                List<Employee> employees = WorkFlowUtil.GetEmployees(approvers);
                string approversNames = WorkFlowUtil.GetApproversNames(employees);
                Employee applicantUser = WorkFlowUtil.GetEmployee(applicant);
                List<string> parameters = new List<string> { 
                    string.Empty, 
                    isReject ? "rejected" : "approved", 
                    recordType, 
                    vendId.IsNotNullOrWhitespace() ? vendId : "N/A", 
                    applicantUser.DisplayName, 
                    approversNames.IsNotNullOrWhitespace() ? approversNames: "N/A", 
                    isReject ? CurrentEmployee.DisplayName : "N/A", 
                    detailLink
                };
                if (applicantUser != null)
                {
                    //Avoid the same user get the serveral mail
                    AddToEmployees(employees, applicantUser);
                }
                if (isReject)
                {
                    //Rejecter needs to get the notify mail
                    employees.Add(CurrentEmployee);
                }                

                WorkFlowUtil.SendMail(subject, bodyTemplate, parameters, employees);
            }
        }

    }
}