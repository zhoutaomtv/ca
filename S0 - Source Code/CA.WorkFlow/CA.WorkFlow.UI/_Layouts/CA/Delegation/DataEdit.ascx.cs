namespace CA.WorkFlow.UI.Delegation
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint;

    public partial class DataEdit : BaseWorkflowUserControl
    {
        private static readonly string DelegableModulesListName = "Modules";
        private static readonly string DelegableModulesListKeyName = "BelongsTo";
        private static readonly string DelegationListName = "Delegates";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                var currentUser = SPContext.Current.Web.CurrentUser;

                this.lblUser.Text = currentUser.Name;
                this.hidUserAccount.Value = currentUser.LoginName;

                this.PopulateDelegableModules();

                SPListItem delegation;

                if (this.IsValidDelegation(out delegation))
                {
                    this.pfSelector.CommaSeparatedAccounts = delegation["DelegateToLoginName"].ToString();

                    this.hidSelectedCategories.Value = delegation["Modules"].ToString();

                    this.dtBegin.SelectedDate = (DateTime)delegation["BeginOn"];
                    this.dtEnd.SelectedDate = (DateTime) delegation["EndOn"];
                    this.hidLoadStatus.Value = "InitialUpdate";
                }
                else
                {
                    var today = DateTime.Now;
                    this.dtBegin.SelectedDate = today;
                    this.dtEnd.SelectedDate = today.AddDays(1);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string error;

            if (this.Validate(out error))
            {
                var delegationList = SharePointUtil.GetList(DelegationListName);

                string delegated = this.hidUserAccount.Value;
                string agent = this.pfSelector.Accounts[0].ToString();

                string startDate = this.dtBegin.SelectedDate.ToShortDateString();
                string endDate = this.dtEnd.SelectedDate.ToShortDateString();

                string modules = this.hidSelectedCategories.Value;

                if (!DelegationExists(delegationList, delegated, agent, startDate, endDate, modules))
                {
                    var delegation = delegationList.Items.Add();

                    delegation["Approver"] = GetUser(delegated);
                    delegation["DelegateTo"] = GetUser(agent);
                    delegation["ApproverLoginName"] = delegated;
                    delegation["DelegateToLoginName"] = agent;
                    delegation["BeginOn"] = startDate;
                    delegation["EndOn"] = endDate;
                    delegation["Modules"] = modules;
                    delegation["Title"] = "My Delegation";

                    delegation.Web.AllowUnsafeUpdates = true;
                    delegation.Update();

                    this.Response.Redirect("/WorkFlowCenter/Lists/Delegates/AllItems.aspx", true);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(typeof(DataEdit), "alert", "<script type=\"text/javascript\">alert('Duplicate delegation Found. ');</script>");
                }
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(typeof (DataEdit), "alert", "<script type=\"text/javascript\">alert('" + error + "');</script>");
            }
        }

        private bool IsValidDelegation(out SPListItem delegation)
        {
            bool valid = false;

            delegation = null;

            int id;

            if (int.TryParse(this.Request.QueryString["ID"], out id))
            {
                var delegationList = SharePointUtil.GetList(DelegationListName);

                try
                {
                    delegation = delegationList.GetItemById(id);
                    valid = delegation["ApproverLoginName"].Equals(this.hidUserAccount.Value);
                }
                catch(ArgumentException)
                {
                }
            }

            return valid;
        }

        private static SPUser GetUser(string loginName)
        {
            SPUser user = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                var currentSite = SPContext.Current.Site;

                using (var site = new SPSite(currentSite.ID))
                {
                    using (SPWeb web = site.OpenWeb(currentSite.RootWeb.ID))
                    {
                        user = web.EnsureUser(loginName);
                    }
                }
            });

            return user;
        }

        private static bool DelegationExists(SPList delegations, string delegated, string agent, string startDate, string endDate, string modules)
        {
            var qfDelegated = new QueryField("ApproverLoginName", false);
            var qfAgent = new QueryField("DelegateToLoginName", false);
            var qfStartDate = new QueryField("BeginOn", false);
            var qfEndDate = new QueryField("EndOn", false);
            var qfModules = new QueryField("Modules", false);

            CamlExpression exp = qfDelegated.Equal(delegated);

            exp &= qfAgent.Equal(agent);

            exp &= qfStartDate.Equal(startDate);

            exp &= qfEndDate.Equal(endDate);

            exp &= qfModules.Equal(modules);

            return ListQuery.Select().From(delegations).Where(exp).GetItems().Count != 0;
        }

        private bool Validate(out string error)
        {
            bool valid = true;
            error = string.Empty;

            if (this.pfSelector.Accounts.Count == 0)
            {
                valid = false;
                error += "- Please select a user account for your delegation. \\r\\n";
            }

            if (this.hidSelectedCategories.Value.Trim(',').Length == 0)
            {
                valid = false;
                error += "- Please select at least 1 module for delegation assignment.\\r\\n";
            }

            if (this.dtBegin.SelectedDate > this.dtEnd.SelectedDate)
            {
                valid = false;
                error += "- The start date should be smalller than or equal with the end date.\\r\\n";
            }

            return valid;
        }

        private void PopulateDelegableModules()
        {
            var dt = ListQuery.Select()
                .From(SharePointUtil.GetList(DelegableModulesListName))
                .Where(null)
                .OrderBy(new QueryField(DelegableModulesListKeyName, false), false)
                .GetDataTable();

            var modules = from s in dt.AsEnumerable()
                          group s by s.Field<string>(DelegableModulesListKeyName)
                          into grp
                          orderby grp.Key
                          select new {Category = grp.Key, Moudles = grp.Select(r => r)};

            this.mossMenu.CustomSelectionEnabled = true;

            foreach (var module in modules)
            {
                var parentItem = new MenuItem(module.Category);
                this.mossMenu.Items.Add(parentItem);

                var rows = module.Moudles;

                foreach (var subItem in rows.Select(row => new MenuItem(row.Field<string>("Title"), row.Field<string>("Tag"))))
                {
                    parentItem.ChildItems.Add(subItem);
                }
            }
        }
    }
}