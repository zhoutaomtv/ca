namespace CA.WorkFlow.UI.InternalOrderMaintenance
{
    using System;
    using Microsoft.SharePoint;
    using QuickFlow.Core;
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

            this.Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            this.Actions.ActionExecuted += new EventHandler<EventArgs>(Actions_ActionExecuted);

            this.DataForm1.OrderNumber = WorkflowContext.Current.DataFields["Order Number"].AsString();
            this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
            this.DataForm1.Department = WorkflowContext.Current.DataFields["Department"].AsString();

            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        void Actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (WorkflowContext.Current.Step == "DepartmentManagerTask")
            {
                if (e.Action == "Approve")
                {
                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IODepartmentManagerApprove;
                    SaveToApprovers();
                }
                else if (e.Action == "To CFO")
                {
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
                    double lastValue = Double.Parse(WorkflowContext.Current.DataFields["Last Value"].ToString());
                    double currValue = Double.Parse(WorkflowContext.Current.DataFields["Value After Change"].ToString());
                    WorkflowContext.Current.DataFields["Change Date"] = DateTime.Now.ToString("g");
                    WorkflowContext.Current.DataFields["Value Change"] = currValue - lastValue;
                    if (currValue - lastValue >= 0)
                    {
                        WorkflowContext.Current.DataFields["Value Change Type"] = "增加/supplement";
                    }
                    else
                    {
                        WorkflowContext.Current.DataFields["Value Change Type"] = "减少/reduction";
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

                    WorkflowContext.Current.DataFields["Status"] = CAWorkflowStatus.IOFinanceReject;
                    SendMail(true);
                }                
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
            string title = "InternalOrderMaintance";
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
                string changeType = WorkflowContext.Current.DataFields["Value Change Type"].AsString(); //Value Change Type
                string orderNumber = WorkflowContext.Current.DataFields["Order Number"].ToString(); //Order Number
                string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString(); //Approvers
                string applicant = WorkflowContext.Current.DataFields["Applicant"].ToString();  //Applicant
                string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/InternalOrderMaintenance/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/InternalOrderMaintenanceWorkflow/MyApply.aspx";
                //string comment = WorkflowContext.Current.TaskFields["Body"].AsString();

                List<Employee> employees = WorkFlowUtil.GetEmployees(approvers);
                string approversNames = WorkFlowUtil.GetApproversNames(employees);
                Employee applicantUser = WorkFlowUtil.GetEmployee(applicant);
                List<string> parameters = new List<string> { 
                    string.Empty, 
                    isReject ? "rejected" : "approved", 
                    changeType.IsNotNullOrWhitespace() ? changeType : "N/A", 
                    orderNumber, 
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