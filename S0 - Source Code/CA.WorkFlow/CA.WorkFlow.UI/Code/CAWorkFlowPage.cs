using System;
using System.Data;
using System.Configuration;
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
using CA.Web;
using System.Threading;
using System.Collections.Specialized;
using System.Collections;
using CodeArt.SharePoint.CamlQuery;
using System.Collections.Generic;

namespace CA.WorkFlow.UI
{
    public class CAWorkFlowPage:Page
    { 
        public SPContext CurrentContext
        {
            get
            {
                return SPContext.Current;
            }
        }

        public SPList CurrentList
        {
            get
            {
                return CurrentContext.List;
            }
        }
        private string str = string.Empty;
        public string TaskOutcome
        {   
            get
            {
                return  str;
            }
            set
            {
               this.str=value;
            }
        }

        public SPListItem CurrentListItem
        {
            get
            {
                return CurrentContext.ListItem;
            }
        }

        private SPControlMode _ControlMode = SPControlMode.Invalid;
        public virtual SPControlMode ControlMode
        {
            get
            {
                return this._ControlMode;
            }
            set
            {
                this._ControlMode = value;
            }
        }

        private Employee _CurrentEmployee=null;
        public Employee CurrentEmployee
        {
            get
            {
                if (_CurrentEmployee == null)
                {
                    try
                    {
                        this.ViewState["CA_CurrentEmployee"] = UserProfileUtil.GetEmployee(SPContext.Current.Web.CurrentUser.LoginName);
                        _CurrentEmployee = (Employee)this.ViewState["CA_CurrentEmployee"];
                    }
                    catch {
                        _CurrentEmployee = new Employee { DisplayName = SPContext.Current.Web.CurrentUser.Name };
                    }                  
                }
                return _CurrentEmployee;
            }
        }

        private Script _script;
        public Script Script
        {
            get
            {
                if (_script == null)
                    _script = new Script(this);
                return _script;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SPContext.Current.FormContext.SetFormMode(this.ControlMode, true);

        }

        public virtual void SetControlMode()
        {
            SetMode(this);
        }

        protected virtual void SetMode(Control ctl)
        {
            if (ctl.Controls.Count > 0)
            {
                foreach (Control tmp in ctl.Controls)
                {
                    if (tmp is BaseFieldControl)
                    {
                        ((BaseFieldControl)tmp).ControlMode = this.ControlMode;
                        continue;
                    }
                    else if (tmp is FormComponent)
                    {
                        ((FormComponent)tmp).ControlMode = this.ControlMode;
                    }
                    else if (tmp is QFUserControl)
                    {
                        ((QFUserControl)tmp).ControlMode = this.ControlMode;
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {          
           // AddPermisson();
            base.OnLoad(e);           
        }

        protected override void OnPreInit(EventArgs e)
        {
            CodeArt.SharePoint.MultiLanSupport.UICultureManager.CurrentInstance.SetThreadCulture();
            base.OnPreInit(e);

            ApplyMaster();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e); 
            ApplyWaitUI();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);            
        }

        protected virtual void ApplyWaitUI()
        {
            this.Form.Attributes.Add("onsubmit", "return CAShowWaitUI();");
        }

        protected virtual void HideWaitUI()
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "hidDiv", "HideWaitUI();");
        }

        /// <summary>
        /// 将站点模板页应用到页面
        /// </summary>
        protected virtual void ApplyMaster()
        {
            //if (SPContext.Current != null)
            //{
            //    this.MasterPageFile = SPContext.Current.Web.CustomMasterUrl;
            //    return;
            //}

        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

     


        protected virtual void Back()
        {
            //if (Page.Request.QueryString["Source"] != null)
            //    Page.Response.Redirect(Page.Request.QueryString["Source"]);
            //else
            Thread.Sleep(3000);
            Page.Response.Redirect(@"/ca/Mytasks.aspx");
        }
        

        public readonly PermissionSet PermissionSet = new PermissionSet();

        protected virtual void AddPermisson()
        {
            PermissionSet.Add(HttpContext.Current.User.Identity.Name, PermissionType.Edit);
        }

        protected void UpdatePermissions()
        {
            PermissionSet permissionSet = this.PermissionSet;

            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                   {
                       using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                       {
                           SPList list = web.Lists[this.CurrentList.ID];

                           SPListItem item = list.GetItemById(Convert.ToInt32(WorkflowContext.Current.DataFields["ID"]));
                           try
                           {
                               item.BreakRoleInheritance(true);
                           }
                           catch { }
                           item.ParentList.ParentWeb.AllowUnsafeUpdates = true;
                           //算法应该优化...
                           for (int index = item.RoleAssignments.Count - 1; index >= 0; index--)
                           {
                               SPRoleAssignment ra = item.RoleAssignments[index];

                               // 如果某个RoleAssignment的Member为系统管理员, 则不要删除之
                               if (ra.Member.Name.ToLower() == "SHAREPOINT\\system".ToLower()) continue;

                               if (ra.Member is SPGroup)
                               {
                                   item.RoleAssignments.Remove(index);
                               }
                               else if (ra.Member is SPUser)
                               {
                                   SPUser user = (SPUser)ra.Member;
                                   if (user.IsDomainGroup)
                                   {
                                       item.RoleAssignments.Remove(index);
                                   }
                                   else if (HasEditPermission(ra))
                                   {
                                       permissionSet.Add(user.LoginName, PermissionType.Edit);
                                       item.RoleAssignments.Remove(index);
                                   }
                               }
                               else//删除所有管理权限
                               {
                                   //permissionSet.Add( ra.Member.Name, PermissionType.View);
                                   item.RoleAssignments.Remove(index);
                               }

                               //item.RoleAssignments.Remove(index);
                           }

                           //添权限
                           foreach (Permission p in permissionSet)
                           {
                               if (p.PermissionType == PermissionType.Edit)
                               {
                                   PermissionUtil.AddManageListItemPermission(item, p.Identity);
                               }
                               else
                               {
                                   PermissionUtil.AddViewListItemPermission(item, p.Identity);
                               }
                           }
                       }
                   }

               });
        }
        

        private bool HasEditPermission(SPRoleAssignment ra)
        {
            foreach (SPRoleDefinition ed in ra.RoleDefinitionBindings)
            {
                if ((ed.BasePermissions & SPBasePermissions.EditListItems) == SPBasePermissions.EditListItems ||
                    (ed.BasePermissions & SPBasePermissions.DeleteListItems) == SPBasePermissions.DeleteListItems)
                {
                    return true;
                }
            }
            return false;
        }

        //Validate whether the task and item are consistent. If no, return false.
        protected bool SecurityValidate(string uTaskId, string uListGUID, string uId, bool isCheckUser)
        {
            bool isValid = false;
            uListGUID = "{" + uListGUID + "}";
            SPList list = WorkFlowUtil.GetWorkflowList("Tasks");
            SPListItem lc = list.GetItemById(Convert.ToInt32(uTaskId));
            int id = Convert.ToInt32(lc["WorkflowItemId"]);
            string listGUID = lc["WorkflowListId"] as string;
            isValid = (id == Convert.ToInt32(uId)) && String.Equals(uListGUID, listGUID, StringComparison.CurrentCultureIgnoreCase);
            if (isCheckUser)
            {
                string assignTo = lc["AssignedTo"].ToString();
                string currUser = SPContext.Current.Web.CurrentUser.Name;
                isValid = assignTo.Contains(currUser);
            }
            return isValid;
        }

        //Save the current approver into the "Approvers" column
        protected void SaveToApprovers()
        {
            //format: ca\test1;ca\test2;
            string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString();
            string currentAcc = SPContext.Current.Web.CurrentUser.LoginName;
            if (!approvers.Contains(currentAcc))
            {
                approvers += currentAcc + ";";
                WorkflowContext.Current.DataFields["Approvers"] = approvers;
            }
        }

        //Add one account to list
        protected void AddToEmployees(List<Employee> employees, Employee e)
        {
            bool isExist = false;
            foreach (Employee emp in employees)
            {
                if (emp.UserAccount.Equals(e.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                employees.Add(e);
            }
        }

        protected void RedirectToTask()
        {
            this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }
    }
}
