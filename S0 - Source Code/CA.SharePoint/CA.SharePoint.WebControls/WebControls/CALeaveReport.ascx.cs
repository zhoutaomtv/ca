using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Text;
using CodeArt.SharePoint.CamlQuery;
using GemBox.Spreadsheet;
using System.IO;
using System.DirectoryServices;

namespace CA.SharePoint.WebControls
{
    public partial class CALeaveReport : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //site = SPContext.Current.Site.RootWeb;
            //wfweb = SPContext.Current.Site.OpenWeb("workflowcenter");
            //hidCurDispName.Value = SPContext.Current.Web.CurrentUser.Name;

            this.btnFilter.Click += new EventHandler(btnFilter_Click);
            this.btnExport.Click += new EventHandler(btnExport_Click);
            if (!IsPostBack)
            {
                ddlLeaveType.Items.Add(new ListItem("All", ""));
                ddlLeaveType.Items.Add(new ListItem("Annual Leave"));
                ddlLeaveType.Items.Add(new ListItem("Compassionate Leave"));
                ddlLeaveType.Items.Add(new ListItem("Leave in-lieu-of Overtime"));
                ddlLeaveType.Items.Add(new ListItem("Marriage Leave"));
                ddlLeaveType.Items.Add(new ListItem("Maternity Leave"));
                ddlLeaveType.Items.Add(new ListItem("No Pay Leave"));
                ddlLeaveType.Items.Add(new ListItem("Others"));
                ddlLeaveType.Items.Add(new ListItem("Sick Leave"));

                ddlDepartments.Items.Add(new ListItem("All", ""));

                DateTime now = DateTime.Now;
                txtfDateFrom.Text = now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtfDateTo.Text = now.ToString("yyyy-MM-dd");


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
                        else
                        {
                            ddlDepartments.Items.Add(new ListItem("Mine", ""));
                            txtfApplicant.Text = SPContext.Current.Web.CurrentUser.Name;
                            lblfDepartment.Visible = false;
                            ddlDepartments.Visible = false;
                            //lblfApplicant.Visible = false;
                            //txtfApplicant.Visible = false;
                            txtfApplicant.Enabled = false;
                            lblIsMe.Visible = false;
                            chkIsReportToMe.Visible = false;

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

            string fApplicant = txtfApplicant.Text;
            string fLeaveType = ddlLeaveType.SelectedValue;//txtfLeaveType.Text;
            string fLeaveDays = txtfLeaveDays.Text;
            string fDateFrom = txtfDateFrom.Text;
            string fDateTo = txtfDateTo.Text;
            string fDepartments = ddlDepartments.SelectedValue;//txtfDepartments.Text;


            QueryField qApplicant = new QueryField("Applicant", false);
            QueryField qLeaveType = new QueryField("LeaveType", false);
            QueryField qLeaveDays = new QueryField("LeaveDays", false);
            QueryField qDateFrom = new QueryField("DateFrom", false);
            QueryField qDateTo = new QueryField("DateTo", false);
            QueryField qDepartment = new QueryField("Department", false);
            QueryField qStatus = new QueryField("Status", false);

            CamlExpression exp = null;
            CamlExpression exp2 = null;

            if (!string.IsNullOrEmpty(fApplicant))
            {
                exp = LinkAnd(exp, qApplicant.Contains(fApplicant));
            }

            if (!string.IsNullOrEmpty(fLeaveType))
            {
                exp = LinkAnd(exp, qLeaveType.Contains(fLeaveType));
            }
            if (!string.IsNullOrEmpty(fLeaveDays))
            {
                exp = LinkAnd(exp, qLeaveDays.Equal(fLeaveDays));
            }

            if (!string.IsNullOrEmpty(fDateFrom))
            {
                exp = LinkAnd(exp, qDateTo >= fDateFrom);
            }
            if (!string.IsNullOrEmpty(fDateTo))
            {
                exp = LinkAnd(exp, qDateFrom <= fDateTo);
            }
            if (!string.IsNullOrEmpty(hidAssoDepts.Value))
            {
                if (!string.IsNullOrEmpty(fDepartments))
                {
                    exp2 = qDepartment.Equal(fDepartments);
                }
                else
                {
                    string[] depts = hidAssoDepts.Value.Split(',');

                    foreach (string dept in depts)
                    {
                        exp2 = LinkOr(exp2, qDepartment.Equal(dept));
                    }
                }
                exp = LinkAnd(exp, exp2);
            }

            if (chkIsReportToMe.Checked && !SPContext.Current.Web.CurrentUser.IsSiteAdmin)
            {
                List<string> directReports = GetDirectReportsInternal(SPContext.Current.Web.CurrentUser.Name, fDepartments);
                exp2 = null;
                if (directReports.Count > 0)
                {
                    foreach (var report in directReports)
                    {
                        exp2 = LinkOr(exp2, qApplicant.Equal(report));
                    }
                }
                else
                {
                    exp2 = qApplicant.Equal("");
                }
                exp = LinkAnd(exp, exp2);
            }

            exp = LinkAnd(exp, qStatus.NotEqual("Cancelled"));
            exp = LinkAnd(exp, qStatus.NotEqual("Canceled"));

            return ListQuery.Select()
                .From(SPContext.Current.Site.OpenWeb("workflowcenter").Lists["LeaveRecord"])
                .Where(exp)
                .OrderBy(new QueryField("Applicant", false), true)
                .OrderBy(new QueryField("DateFrom", false), true)
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
            op.Append("    <td>ID</td>");
            op.Append("    <td>Applicant</td>");
            op.Append("    <td>Department</td>");
            op.Append("    <td>LeaveType</td>");
            op.Append("    <td>DateFrom</td>");
            op.Append("    <td>DateTo</td>");
            op.Append("    <td>LeaveDays</td>");
            op.Append("    <td>Status</td>");
            op.Append("  </tr>");
            foreach (SPListItem item in coll)
            {
                string[] uv = (item["WorkFlowNumber"] + "").Split(',');
                if (uv.Length > 1)
                {
                    op.Append("  <tr>");
                    op.Append("    <td><a href=\"" + uv[0] + "\" target=\"_blank\">" + uv[1] + "</a></td>");
                    op.Append("    <td>" + item["Applicant"] + "</td>");
                    op.Append("    <td>" + item["Department"] + "</td>");
                    op.Append("    <td>" + item["LeaveType"] + "</td>");
                    op.Append("    <td>" + DateTime.Parse(item["DateFrom"] + "").ToShortDateString() + "</td>");
                    op.Append("    <td>" + DateTime.Parse(item["DateTo"] + "").ToShortDateString() + "</td>");
                    op.Append("    <td>" + item["LeaveDays"] + "</td>");
                    op.Append("    <td>" + item["Status"] + "</td>");
                    op.Append("  </tr>");
                }
            }
            op.Append("</table>");

            return op.ToString();
        }

        protected void Export(SPListItemCollection coll)
        {
            string tmpPath = "/tmpfiles/LeaveRecord/";
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

            sheet1.Cells[0, 0].Value = "Applicant:";
            sheet1.Cells[0, 1].Value = txtfApplicant.Text;
            sheet1.Cells[1, 0].Value = "LeaveType:";
            sheet1.Cells[1, 1].Value = ddlLeaveType.SelectedItem.Text;
            sheet1.Cells[2, 0].Value = "LeaveDays:";
            sheet1.Cells[2, 1].Value = txtfLeaveDays.Text;
            sheet1.Cells[3, 0].Value = "DateRange:";
            sheet1.Cells[3, 1].Value = txtfDateFrom.Text + " - " + txtfDateTo.Text;
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
                sheet1.Cells[5, 1].Value = SPContext.Current.Web.CurrentUser.Name;
            }


            sheet1.Cells[headIdx, 0].Value = "WorkFlowNumber";
            sheet1.Cells[headIdx, 1].Value = "Applicant";
            sheet1.Cells[headIdx, 2].Value = "Department";
            sheet1.Cells[headIdx, 3].Value = "LeaveType";
            sheet1.Cells[headIdx, 4].Value = "DateFrom";
            sheet1.Cells[headIdx, 5].Value = "DateTo";
            sheet1.Cells[headIdx, 6].Value = "LeaveDays";
            sheet1.Cells[headIdx, 7].Value = "Status";

            if (coll.Count > 0)
            {
                int i = bodyIdx;
                foreach (SPListItem item in coll)
                {
                    string[] uv = (item["WorkFlowNumber"] + "").Split(',');
                    sheet1.Cells[i, 0].Value = uv[1];
                    sheet1.Cells[i, 1].Value = item["Applicant"] + "";
                    sheet1.Cells[i, 2].Value = item["Department"] + "";
                    sheet1.Cells[i, 3].Value = item["LeaveType"] + "";
                    sheet1.Cells[i, 4].Value = item["DateFrom"] + "";
                    sheet1.Cells[i, 5].Value = item["DateTo"] + "";
                    sheet1.Cells[i, 6].Value = item["LeaveDays"] + "";
                    sheet1.Cells[i, 7].Value = item["Status"] + "";
                    i += 1;
                }

            }
            objExcelFile.SaveXls(strFilePath);
            //lblTmpXlsUrl.Text = "<a href=\"" + tmpPath + fileName + "\" target=\"_blank\">" + fileName + "</a>";
            lblTmpXlsUrl.Text = "<script  type=\"text/javascript\">popexcel('" + tmpPath + fileName + "');</script>";
        }

        private static List<string> GetDirectReportsInternal(string username, string selecteddept)
        {
            List<string> reports = new List<string>();
            //Employee curempl = UserProfileUtil.GetEmployeeByDisplayName("Jeff Qian");

            try
            {

                using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://dc=cnaidc,dc=cn", "candic\\spsservice", "ciicit#4%6"))
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
                                                    string[] depts = department.Split(',');
                                                    if (depts.Length == 0) depts = department.Split(';');
                                                    if (string.IsNullOrEmpty(selecteddept) || ContainsDepartment(depts, selecteddept))
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

        private static bool ContainsDepartment(string[] departments, string dept)
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
    }
}