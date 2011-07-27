using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using QuickFlow;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.Equipment
{
    //added by wsq 2010-07-23
    public partial class NewForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg = DataForm1.Validate();
            if (!string.IsNullOrEmpty(msg))
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }
            //验证组里用户是否为空
            List<string> list = WorkFlowUtil.UserListInGroup("wf_IT");
            if (list.Count == 0)
            {
                DisplayMessage("Unable to submit the application. There is no user in wf_IT group. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            WorkflowContext curContext = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string taskTitle = SPContext.Current.Web.CurrentUser.Name + "'s Equipment Application";

            string passTo = DataForm1.Manager;

            fields["EmployeeName"] = ((TextBox)DataForm1.FindControl("txtEmployeeName")).Text;
            fields["EmployeeTitle"] = ((TextBox)DataForm1.FindControl("txtEmployeeTitle")).Text;
            fields["EmployeeID"] = ((TextBox)DataForm1.FindControl("txtEmployeeID")).Text;

            //fields["OnboardAt"] = ((TextBox)DataForm1.FindControl("txtOnboardAt")).Text;
            fields["OnboardAt"] = ((CADateTimeControl)DataForm1.FindControl("CADateTime1")).SelectedDate.ToShortDateString();
            fields["Department"] = ((TextBox)DataForm1.FindControl("txtDepartment")).Text;
            fields["Manager"] = EnsureUser(passTo);
            if (!string.IsNullOrEmpty(DataForm1.FunctionalManager))
            {
                fields["FunctionalManager"] = EnsureUser(DataForm1.FunctionalManager);
            }
            else
                fields["FunctionalManager"] = string.Empty; 
            
            //fields["Remark"] = ((TextBox)DataForm1.FindControl("txtRemark")).Text;
            fields["WorkflowNumber"] = CreateWorkflowNumber();
            fields["Status"] = "In Progress";
            
            curContext.UpdateWorkflowVariable("HRSubmitTitle", "Please complete equipment application");
            curContext.UpdateWorkflowVariable("FunctionalManagerApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("DepartmentHeadApproveTitle", taskTitle + " needs approval");
            curContext.UpdateWorkflowVariable("ITGroupConfirmTitle", taskTitle + " needs confirm");
            //curContext.UpdateWorkflowVariable("CFCOGroupApproveTitle", taskTitle + " needs approval");

            curContext.UpdateWorkflowVariable("DH", passTo);
            curContext.UpdateWorkflowVariable("FunctionalManager", DataForm1.FunctionalManager);
            curContext.UpdateWorkflowVariable("ITGroup", "wf_IT");
            //curContext.UpdateWorkflowVariable("CFCOGroup", "wf_CFCO");

            
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            List<string> mailList = new List<string>();
            List<SPUser> users = WorkFlowUtil.GetSPUsersInGroup("wf_IT");
            foreach (SPUser user in users)
            {
                mailList.Add(user.Email);
            }
            string EmployeeName=((TextBox)DataForm1.FindControl("txtEmployeeName")).Text;
            StringDictionary dict = new StringDictionary();
            dict.Add("to", string.Join(";", mailList.ToArray()));
            dict.Add("subject",EmployeeName+"'s new employee equipment request" );


            string mcontent = EmployeeName + "'s new employee equipment request has been submitted. Workflow number is "
                + SPContext.Current.ListItem["WorkflowNumber"] + ".<br/><br/>" + @" Please view the detail by clicking <a href='"
                + SPContext.Current.Web.Url
                + "/_layouts/CA/WorkFlows/Equipment/DisplayForm.aspx?List="
                + SPContext.Current.ListId.ToString()
                + "&ID="
                + SPContext.Current.ListItem.ID
                + "'>here</a>.";

            SPUtility.SendEmail(SPContext.Current.Web, dict, mcontent);
          
        }


        private string CreateWorkflowNumber()
        {
            return "NEEA_" + WorkFlowUtil.CreateWorkFlowNumber("NewEmployeeEquipmentApplication").ToString("000000");
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
