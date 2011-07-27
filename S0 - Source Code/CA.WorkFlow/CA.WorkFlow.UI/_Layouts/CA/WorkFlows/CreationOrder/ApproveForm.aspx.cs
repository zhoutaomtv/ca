namespace CA.WorkFlow.UI.CreationOrder
{
    using System;
    using Microsoft.SharePoint;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using System.Collections.Generic;
    using System.Collections;
    using System.Diagnostics;
    using QuickFlow;

    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check security
            string uListGUID = this.Request.QueryString["List"];
            string uID = this.Request.QueryString["ID"];
            string uTaskId = this.Request.QueryString["TaskId"];
            if (!this.SecurityValidate(uTaskId, uListGUID, uID, true))
            {
                RedirectToTask();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            //Set the currenty step, the user control will set the control mode of Order Number by this value
            this.DataForm1.CurrentStep = WorkflowContext.Current.Step;
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            this.DataForm1.Department = WorkflowContext.Current.DataFields["Department"].AsString();


            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            if (WorkflowContext.Current.Step == "DepartmentManagerTask")
            {
                if (e.Action == "Approve")
                {
                    WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskUsers", GetTaskUsers("wf_FinanceAnalyst_IO"));

                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IODepartmentManagerApprove;
                    SaveToApprovers();
                }
                else if (e.Action == "To CFO")
                {
                    WorkflowContext.Current.UpdateWorkflowVariable("CFOTaskUsers", GetTaskUsers("wf_CFO"));

                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IOToCFO;
                    SaveToApprovers();
                }
                else if (e.Action == "Reject")
                {
                    if (!Validate(e.Action, e))
                    {
                        return;
                    }

                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IODepartmentManagerReject;
                    SendMail(true);
                }
            }
            else if (WorkflowContext.Current.Step == "CFOTask")
            {
                if (e.Action == "Approve")
                {
                    WorkflowContext.Current.UpdateWorkflowVariable("FinanceAnalystTaskUsers", GetTaskUsers("wf_FinanceAnalyst_IO"));

                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IOCFOApprove;
                    SaveToApprovers();
                }
                else if (e.Action == "Reject")
                {
                    if (!Validate(e.Action, e))
                    {
                        return;
                    }

                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IOCFOReject;
                    SendMail(true);
                }
            }
            else if (WorkflowContext.Current.Step == "FinanceAnalystTask")
            {
                if (e.Action == "Confirm")
                {
                    if (!Validate(e.Action, e))
                    {
                        return;
                    }

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

                    WorkflowContext.Current.DataFields["Order Number"] = string.Empty;
                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IOFinanceReject;
                    SendMail(true);
                }
            }
        }

        //Return task users object according to special group
        private NameCollection GetTaskUsers(string group)
        {
            return WorkFlowUtil.GetTaskUsers(group, "116");
        }

        private bool Validate(string action, ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {                
                DisplayMessage(this.DataForm1.msg.IsNotNullOrWhitespace() ? this.DataForm1.msg : "Please fill in Order Number field.");
                e.Cancel = true;
                flag = true;
            }
            return flag;
        }

        //title - identity for the feature
        //isReject
        private void SendMail(bool isReject)
        {
            string title = "InternalOrderCreation";
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
                string orderType = WorkflowContext.Current.DataFields["Internal Order Type"].ToString(); //Internal Order Type
                string orderNumber = WorkflowContext.Current.DataFields["Order Number"].ToString(); //Order Number
                string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString(); //Approvers
                string applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();  //Applicant
                string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/CreationOrder/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/CreationOrder/MyApply.aspx";
                //string comment = WorkflowContext.Current.TaskFields["Body"].AsString();

                List<Employee> employees = WorkFlowUtil.GetEmployees(approvers);
                string approversNames = WorkFlowUtil.GetApproversNames(employees);
                Employee applicantUser = WorkFlowUtil.GetEmployee(applicant);
                List<string> parameters =  new List<string> { 
                    string.Empty, 
                    isReject ? "rejected" : "approved", 
                    orderType, 
                    isReject ? "N/A" : orderNumber, 
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