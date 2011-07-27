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
using CA.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace CA.WorkFlow.UI.Equipment
{
    //added by wsq 2010-07-23
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckIsCancel(this.value);";
            //this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);

            if (WorkflowContext.ContextInitialized)
            {
                if(WorkflowContext.Current.Task.Step == "HRSubmit")
                {
                    PanelComm.Visible = false;
                }
                
            }
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (WorkflowContext.ContextInitialized)
            {
                WorkflowContext curContext = WorkflowContext.Current;
                WorkflowDataFields fields = curContext.DataFields;
                string taskTitle = new SPFieldLookupValue(fields["Created By"] + "").LookupValue + "'s Equipment Application";

                SPWeb web = SPContext.Current.Web;
                SPUser curuser = web.CurrentUser;
                fields["Status"] = "In Progress";
            
                switch (WorkflowContext.Current.Task.Step)
                {
                    case "DepartmentHeadApprove":
                        if (e.Action == "Approve")
                        {
                            fields["Computer"] = ((RadioButtonList)DataForm1.FindControl("radComputer")).SelectedValue;
                            fields["Email"] = ((RadioButtonList)DataForm1.FindControl("radEmail")).SelectedValue;
                            fields["Sap"] = ((RadioButtonList)DataForm1.FindControl("radSap")).SelectedValue;
                            fields["Telephone"] = ((RadioButtonList)DataForm1.FindControl("radTelephone")).SelectedValue;
                            fields["Remark"] = ((TextBox)DataForm1.FindControl("txtRemark")).Text;
                        }
                        fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                        break;
                    case "FunctionalManagerApprove":
                        if (e.Action == "Approve")
                        {
                            fields["Computer"] = ((RadioButtonList)DataForm1.FindControl("radComputer")).SelectedValue;
                            fields["Email"] = ((RadioButtonList)DataForm1.FindControl("radEmail")).SelectedValue;
                            fields["Sap"] = ((RadioButtonList)DataForm1.FindControl("radSap")).SelectedValue;
                            fields["Telephone"] = ((RadioButtonList)DataForm1.FindControl("radTelephone")).SelectedValue;
                            fields["Remark"] = ((TextBox)DataForm1.FindControl("txtRemark")).Text;
                        }

                        fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                        //curuser.ID.ToString() + ";#" + curuser.LoginName.ToString() + ";#"; 
                        break;
                    case "ITConfirm":
                        //fields["Remark"] = ((TextBox)DataForm1.FindControl("txtRemark")).Text;
                        if (e.Action == "Confirm")
                        {
                            fields["Status"] = "Completed";
                        }
                        fields["Approvers"] = WorkFlowUtil.GetApproversValue();
                        break;
                    //case "CFCOApprove":
                    //    break;
                    case "HRSubmit":
                        if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
                        {
                            WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                            return;
                        }
                        string msg = DataForm1.Validate();
                        if (!string.IsNullOrEmpty(msg))
                        {
                            DisplayMessage(msg);
                            e.Cancel = true;
                            return;
                        }

                        string passTo = DataForm1.Manager;
                        fields["EmployeeName"] = ((TextBox)DataForm1.FindControl("txtEmployeeName")).Text;
                        fields["EmployeeTitle"] = ((TextBox)DataForm1.FindControl("txtEmployeeTitle")).Text;
                        fields["EmployeeID"] = ((TextBox)DataForm1.FindControl("txtEmployeeID")).Text;
                        fields["OnboardAt"] = ((CADateTimeControl)DataForm1.FindControl("CADateTime1")).SelectedDate.ToShortDateString();
                        fields["Department"] = ((TextBox)DataForm1.FindControl("txtDepartment")).Text;
                        
                        fields["Manager"] = EnsureUser(passTo);
                        if (!string.IsNullOrEmpty(DataForm1.FunctionalManager))
                        {
                            fields["FunctionalManager"] = EnsureUser(DataForm1.FunctionalManager);
                        }
                        else
                            fields["FunctionalManager"] = string.Empty;

                        curContext.UpdateWorkflowVariable("DH", passTo);
                        curContext.UpdateWorkflowVariable("FunctionalManager", DataForm1.FunctionalManager);
                        //fields["Remark"] = ((TextBox)DataForm1.FindControl("txtRemark")).Text;
                        break;
                }
            }
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        public static SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            }
             );

            return user;
        }


    }
}
