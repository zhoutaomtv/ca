using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Text;
using CodeArt.SharePoint.CamlQuery;
using GemBox.Spreadsheet;
using System.IO;
using System.DirectoryServices;
using System.Data;

namespace CA.SharePoint.WebControls
{
    public partial class CALeaveBalance : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //hidCurDispName.Value = SPContext.Current.Web.CurrentUser.Name;

            this.btnFilter.Click += new EventHandler(btnFilter_Click);
            this.btnExport.Click += new EventHandler(btnExport_Click);
            if (!IsPostBack)
            {
                ddlDepartments.Items.Add(new ListItem("All", ""));

                if (SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                {
                    SetAllDepts();
                }
                else
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
                lblOP.Text = GenTable(FilterColl());
            }
        }

        private void SetAllDepts()
        {
            //add all departments to dropdownlist
            List<string> listdepts = new List<string>();
            foreach (SPListItem item in SPContext.Current.Site.RootWeb.Lists["Department"].Items)
            {
                listdepts.Add(item["Name"] + "");
            }
            listdepts.Sort();
            foreach (var dept in listdepts)
            {
                ddlDepartments.Items.Add(dept);
            }
            hidAssoDepts.Value = string.Join(",", listdepts.ToArray());

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            lblOP.Text = GenTable(FilterColl());

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Export(FilterColl());

        }

        private SPListItemCollection FilterColl()
        {
            lblTmpXlsUrl.Text = "";

            //string fEmployee = txtfEmployee.Text;
            string fYear = txtfYear.Text;
            string fAnnualEntitlement = txtfAnnualEntitlement.Text;
            string fSickEntitlement = txtfSickEntitlement.Text;
            string fDepartments = ddlDepartments.SelectedValue;//txtfDepartments.Text;


            QueryField qEmployee = new QueryField("Employee", false);
            QueryField qYear = new QueryField("Year", false);
            QueryField qAnnualEntitlement = new QueryField("AnnualEntitlement", false);
            QueryField qSickEntitlement = new QueryField("SickEntitlement", false);
            //QueryField qDepartment = new QueryField("Department", false);

            CamlExpression exp = null;

            if (!string.IsNullOrEmpty(fYear))
            {
                exp = LinkAnd(exp, qYear.Contains(fYear));
            }

            if (!string.IsNullOrEmpty(fAnnualEntitlement))
            {
                exp = LinkAnd(exp, qAnnualEntitlement.Equal(fAnnualEntitlement));
            }
            if (!string.IsNullOrEmpty(fSickEntitlement))
            {
                exp = LinkAnd(exp, qSickEntitlement.Equal(fSickEntitlement));
            }

            if (chkIsReportToMe.Checked && !SPContext.Current.Web.CurrentUser.IsSiteAdmin)
            {
                List<string> directReports = GetDirectReportsInternal(SPContext.Current.Web.CurrentUser.Name, fDepartments);
                CamlExpression exp2 = null;
                if (directReports.Count > 0)
                {
                    foreach (var report in directReports)
                    {
                        exp2 = LinkOr(exp2, qEmployee.Equal(report));
                    }
                }
                else
                {
                    exp2 = qEmployee.Equal("");
                }
                exp = LinkAnd(exp, exp2);
            }

            return ListQuery.Select()
                .From(SPContext.Current.Site.OpenWeb("workflowcenter").Lists["LeaveBalance"])
                .Where(exp)
                .OrderBy(new QueryField("Year", false), false)
                .OrderBy(new QueryField("Employee", false), true)
                .GetItems();
        }

        private CamlExpression LinkAnd(CamlExpression expr1, CamlExpression expr2)
        {
            if (expr1 == null)
                return expr2;
            else
                return expr1 && expr2;
        }

        private CamlExpression LinkOr(CamlExpression expr1, CamlExpression expr2)
        {
            if (expr1 == null)
                return expr2;
            else
                return expr1 || expr2;
        }



        private string GenTable(SPListItemCollection coll)
        {
            StringBuilder op = new StringBuilder();
            op.Append("<table class=\"alttb\">");
            op.Append("  <tr class=\"tr2\">");
            op.Append("    <td>Employee</td>");
            op.Append("    <td>Year</td>");
            op.Append("    <td>AnnualEntitlement</td>");
            op.Append("    <td>SickEntitlement</td>");
            op.Append("    <td>AnnualBalance</td>");
            op.Append("    <td>SickBalance</td>");
            op.Append("    <td></td>");
            op.Append("  </tr>");
            foreach (SPListItem item in coll)
            {
                #region
                //Employee empl = UserProfileUtil.GetEmployeeByDisplayName(new SPFieldLookupValue(item["Employee"] + "").LookupValue);
                string dept = GetDepartmentInternal(new SPFieldLookupValue(item["Employee"] + "").LookupValue);
                string[] deptsArray = dept.Split(';'); //Don 2010-12-29
                if (deptsArray.Length == 0) deptsArray = dept.Split(','); //Don 2010-12-29

                if (!string.IsNullOrEmpty(dept))
                {
                    if (string.IsNullOrEmpty(ddlDepartments.SelectedValue))
                    {
                        if (deptsArray.Length == 0)//Don 2010-12-29
                        {
                            if (ContainsDepartment(hidAssoDepts.Value.Split(','),dept))
                            {
                                op.Append(GenTr(item));
                            }
                        }
                        else
                        {
                            foreach (string dep in deptsArray)
                            {
                                if (ContainsDepartment(hidAssoDepts.Value.Split(','), dep.ToLower()))
                                {
                                    op.Append(GenTr(item)); break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Don 2010-12-29
                        if (deptsArray.Length == 0)
                        {
                            if (ddlDepartments.SelectedValue.ToLower() == dept.ToLower())
                            {
                                op.Append(GenTr(item));
                            }
                        }
                        else
                        {
                            foreach (string dep in deptsArray)
                            {
                                if (ddlDepartments.SelectedValue.ToLower() == dep.ToLower())
                                {
                                    op.Append(GenTr(item)); break;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            op.Append("</table>");

            return op.ToString();
        }

        private bool ContainsDepartment(string[] departments, string dept)
        {
            bool result = false;
            foreach (string department in departments)
            {
                if (department.ToLower() == dept)
                {
                    result = true; break;
                }
            }
            return result;
        }

        private string GenTr(SPListItem item)
        {
            StringBuilder op = new StringBuilder();
            op.Append("  <tr>");
            op.Append("    <td>" + new SPFieldLookupValue(item["Employee"] + "").LookupValue + "</td>");
            op.Append("    <td>" + item["Year"] + "</td>");
            op.Append("    <td>" + item["AnnualEntitlement"] + "</td>");
            op.Append("    <td>" + item["SickEntitlement"] + "</td>");
            op.Append("    <td>" + item["AnnualBalance"] + "</td>");
            op.Append("    <td>" + item["SickBalance"] + "</td>");
            op.Append("    <td><a href=\"/WorkFlowCenter/Lists/LeaveBalance/EditForm.aspx?ID=" + item["ID"] + "\">edit</a></td>");
            op.Append("  </tr>");
            return op.ToString();
        }

        protected void Export(SPListItemCollection coll)
        {
            string tmpPath = "/tmpfiles/LeaveBalance/";
            string strPath = Server.MapPath(tmpPath);
            DirectoryInfo dinfo = new DirectoryInfo(strPath);
            if (!dinfo.Exists)
            {
                Directory.CreateDirectory(strPath);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            string strFilePath = strPath + "/" + fileName;
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new ExcelFile();
            GemBox.Spreadsheet.ExcelWorksheet sheet1 = objExcelFile.Worksheets.Add("sheet1");

            int headIdx = 6;
            int bodyIdx = headIdx + 1;
            sheet1.Rows[0].InsertEmpty(bodyIdx + coll.Count);

            //sheet1.Cells[0, 0].Value = "Employee:";
            //sheet1.Cells[0, 1].Value = txtfEmployee.Text;
            sheet1.Cells[1, 0].Value = "Year:";
            sheet1.Cells[1, 1].Value = txtfYear.Text;
            sheet1.Cells[2, 0].Value = "AnnualEntitlement:";
            sheet1.Cells[2, 1].Value = txtfAnnualEntitlement.Text;
            sheet1.Cells[3, 0].Value = "SickEntitlement:";
            sheet1.Cells[3, 1].Value = txtfSickEntitlement.Text;
            sheet1.Cells[4, 0].Value = "Department:";
            if (string.IsNullOrEmpty(ddlDepartments.SelectedValue))
            {
                sheet1.Cells[4, 1].Value = ddlDepartments.SelectedItem.Text;//hidAssoDepts.Value;
            }
            else
            {
                sheet1.Cells[4, 1].Value = ddlDepartments.SelectedItem.Text;
            }
            sheet1.Cells[5, 0].Value = "Reports to:";
            if (chkIsReportToMe.Checked)
            {
                sheet1.Cells[5, 1].Value = hidCurDispName.Value;
            }

            sheet1.Cells[headIdx, 0].Value = "Employee";
            sheet1.Cells[headIdx, 1].Value = "Department";
            sheet1.Cells[headIdx, 2].Value = "Year";
            sheet1.Cells[headIdx, 3].Value = "AnnualEntitlement";
            sheet1.Cells[headIdx, 4].Value = "SickEntitlement";
            sheet1.Cells[headIdx, 5].Value = "AnnualBalance";
            sheet1.Cells[headIdx, 6].Value = "SickBalance";

            if (coll.Count > 0)
            {
                int i = bodyIdx;
                foreach (SPListItem item in coll)
                {
                    #region
                    string dept = GetDepartmentInternal(new SPFieldLookupValue(item["Employee"] + "").LookupValue);
                    string[] deptsArray = dept.Split(';'); //Don 2010-12-29
                    if (deptsArray.Length == 0) deptsArray = dept.Split(','); //Don 2010-12-29

                    if (!string.IsNullOrEmpty(dept))
                    {
                        if (string.IsNullOrEmpty(ddlDepartments.SelectedValue))
                        {
                            if (deptsArray.Length == 0)//Don 2010-12-29
                            {
                                if (ContainsDepartment(hidAssoDepts.Value.Split(','), dept))
                                {
                                    AddRow(item,ref sheet1,ref i);
                                }
                            }
                            else
                            {
                                foreach (string dep in deptsArray)
                                {
                                    if (ContainsDepartment(hidAssoDepts.Value.Split(','), dep.ToLower()))
                                    {
                                        AddRow(item,ref sheet1,ref i);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Don 2010-12-29
                            if (deptsArray.Length == 0)
                            {
                                if (ddlDepartments.SelectedValue.ToLower() == dept.ToLower())
                                {
                                    AddRow(item,ref sheet1,ref i);
                                }
                            }
                            else
                            {
                                foreach (string dep in deptsArray)
                                {
                                    if (ddlDepartments.SelectedValue.ToLower() == dep.ToLower())
                                    {
                                        AddRow(item,ref sheet1,ref i);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

            }
            objExcelFile.SaveXls(strFilePath);
            //lblTmpXlsUrl.Text = "<a href=\"" + tmpPath + fileName + "\" target=\"_blank\">" + fileName + "</a>";
            lblTmpXlsUrl.Text = "<script  type=\"text/javascript\">popexcel('" + tmpPath + fileName + "');</script>";

        }

        private void AddRow(SPListItem item,ref GemBox.Spreadsheet.ExcelWorksheet sheet1,ref int i)
        {
            string strEmpl = new SPFieldLookupValue(item["Employee"] + "").LookupValue;
            sheet1.Cells[i, 0].Value = strEmpl;
            sheet1.Cells[i, 1].Value = GetDepartmentInternal(strEmpl);
            sheet1.Cells[i, 2].Value = item["Year"] + "";
            sheet1.Cells[i, 3].Value = item["AnnualEntitlement"] + "";
            sheet1.Cells[i, 4].Value = item["SickEntitlement"] + "";
            sheet1.Cells[i, 5].Value = item["AnnualBalance"] + "";
            sheet1.Cells[i, 6].Value = item["SickBalance"] + "";
            i++;
        }

        private static List<string> GetDirectReportsInternal(string username, string selecteddept)
        {
            List<string> reports = new List<string>();
            //Employee curempl = UserProfileUtil.GetEmployeeByDisplayName("Jeff Qian");

            try
            {

                using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://dc=cnaidc,dc=cn", "cnaidc\\spsservice", "ciicit#4%6"))
                {
                    using (DirectorySearcher ds = new DirectorySearcher(directoryEntry))
                    {
                        ds.SearchScope = SearchScope.Subtree;
                        ds.PropertiesToLoad.Clear();
                        ds.PropertiesToLoad.Add("directReports");
                        ds.Filter = String.Format("(&(objectCategory=person)(objectClass=user)(displayName={0}))", username);
                        //ds.ServerPageTimeLimit = TimeSpan.FromSeconds(2);
                        SearchResult sr = ds.FindOne();
                        if (sr != null)
                        {
                            foreach (string report in sr.Properties["directReports"])
                            {
                                //if report in selecteddept, then add it.
                                using (DirectoryEntry directoryEntry2 = new DirectoryEntry(string.Format("LDAP://{0}", report), "cnaidc\\spsservice", "ciicit#4%6"))
                                {
                                    using (DirectorySearcher ds2 = new DirectorySearcher(directoryEntry2))
                                    {
                                        //ds2.SearchScope = SearchScope.Subtree;
                                        ds2.PropertiesToLoad.Clear();
                                        ds2.PropertiesToLoad.Add("department");
                                        ds2.PropertiesToLoad.Add("displayName");
                                        SearchResult sr2 = ds2.FindOne();
                                        if (sr2 != null)
                                        {
                                            using (DirectoryEntry entry = sr2.GetDirectoryEntry())
                                            {
                                                if (entry.Properties["displayName"].Value != null && entry.Properties["department"].Value != null)
                                                {
                                                    string department = entry.Properties["department"].Value.ToString();
                                                    if (string.IsNullOrEmpty(selecteddept) || selecteddept.ToLower() == department.ToLower())
                                                    {
                                                        reports.Add(entry.Properties["displayName"].Value.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return reports;

        }

        private static string GetDepartmentInternal(string username)
        {
            string dept = "";

            try
            {
                using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://dc=cnaidc,dc=cn", "cnaidc\\spsservice", "ciicit#4%6"))
                {
                    using (DirectorySearcher ds = new DirectorySearcher(directoryEntry))
                    {
                        ds.SearchScope = SearchScope.Subtree;
                        ds.PropertiesToLoad.Clear();
                        ds.PropertiesToLoad.Add("department");
                        ds.Filter = String.Format("(&(objectCategory=person)(objectClass=user)(displayName={0}))", username);
                        //ds.ServerPageTimeLimit = TimeSpan.FromSeconds(2);
                        SearchResult sr = ds.FindOne();
                        if (sr != null)
                        {
                            using (DirectoryEntry entry = sr.GetDirectoryEntry())
                            {
                                if (entry.Properties["department"].Value != null)
                                {
                                    dept = entry.Properties["department"].Value.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                //throw e;
            }

            return dept;

        }
    }
}