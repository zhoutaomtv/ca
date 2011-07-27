using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using Microsoft.SharePoint;

namespace CA.SharePoint.Web._12.TEMPLATE.LAYOUTS.CA
{
    public partial class LeaveData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected DataTable Query()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItemCollection depts = ListQuery.Select()
                     .From(SPContext.Current.Site.RootWeb.Lists["PersonXDepts"])
                     .Where(new QueryField("Person", false).Equal(SPContext.Current.Web.CurrentUser.LoginName)) //Person's Show field must be: Account
                     .GetItems();

                if (depts.Count > 0)
                {
                    if (depts[0]["Depts"] == null || string.IsNullOrEmpty(depts[0]["Depts"].ToString().Trim()))
                    {
                        SetAllDepts();
                    }
                    else
                    {
                        string[] arrdepts = depts[0]["Depts"].ToString().Trim().Split(',');
                        Array.Sort(arrdepts);

                        foreach (var dept in arrdepts)
                        {
                            ddlDepartments.Items.Add(dept);
                        }
                        hidAssoDepts.Value = string.Join(",", arrdepts);

                    }
                }
            });
        }
    }
}