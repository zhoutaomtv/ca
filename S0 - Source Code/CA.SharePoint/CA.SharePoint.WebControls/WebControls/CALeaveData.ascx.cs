namespace CA.SharePoint.WebControls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CodeArt.SharePoint.CamlQuery;
    using GemBox.Spreadsheet;
    using Microsoft.SharePoint;

    public partial class CALeaveData : UserControl
    {
        protected static string LeaveDataListName = "LeaveData";
        protected static string LeaveDownloadsListName = "LeaveDownloads";

        protected const int MonthlyStartDay = 19;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.spgvResult.BorderStyle = BorderStyle.Solid;
                this.spgvResult.GridLines = GridLines.Horizontal;

                this.BindYearMonthes();

                this.SetDownloadButtonVisibility();
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.dsLeaveData.SelectParameters.Clear();

            this.dsLeaveData.SelectParameters.Add("selectedMonth", this.ddlStartMonthes.SelectedValue);

            this.spgvResult.DataSourceID = this.dsLeaveData.ID;
            this.spgvResult.DataBind();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            var result = this.Query(this.ddlStartMonthes.SelectedValue);

            if (result != null && this.ExportData(result))
            {
                this.AddLeaveDownloads();
            }
        }

        protected void ddlStartMonthes_Changed(object sender, EventArgs e)
        {
            this.SetDownloadButtonVisibility();
            this.btnApply_Click(null, null);
        }

        protected void spgvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.spgvResult.PageIndex = e.NewPageIndex;
            this.spgvResult.DataBind();
        }

        private void BindYearMonthes()
        {
            this.ddlStartMonthes.Items.Clear();

            var list = SharePointUtil.GetList(LeaveDataListName);

            if (list != null)
            {
                var monthes = new List<string>();

                var items = list.Items;

                foreach (string dt in from SPListItem item in items select Convert.ToDateTime(item["StatMon"]).ToString("yyyy-MM") into dt where !monthes.Contains(dt) select dt)
                {
                    monthes.Add(dt);
                }

                this.ddlStartMonthes.DataSource = monthes.OrderBy(n => n);
                this.ddlStartMonthes.DataBind();

                this.ddlStartMonthes.SelectedValue = DateTime.Now.ToString("yyyy-MM");
            }
        }

        public DataTable Query(string selectedMonth)
        {
            if (string.IsNullOrEmpty(selectedMonth))
            {
                return null;
            }

            var monthField = new QueryField("StatMon", false);

            CamlExpression exp = monthField.Equal(selectedMonth + "-1");

            return ListQuery.Select()
                .From(SharePointUtil.GetList(LeaveDataListName))
                .Where(exp)
                .OrderBy(new QueryField("EmployeeName", false), false)
                .OrderBy(new QueryField("TimeWageType", false), true)
                .OrderBy(new QueryField("Date", false), true)
                .GetDataTable();
        }

        private void SetDownloadButtonVisibility()
        {
            this.btnDownload.Visible = false;

            if (this.ddlStartMonthes.Items.Count != 0)
            {
                DateTime selectedDate = DateTime.Parse(this.ddlStartMonthes.SelectedValue + "-1");

                DateTime currentDate = DateTime.Now;

                if (selectedDate.Month < currentDate.Month)
                {
                    this.btnDownload.Visible = true;
                }
                else if (selectedDate.Month == currentDate.Month && currentDate.Day > MonthlyStartDay)
                {
                    this.btnDownload.Visible = true;
                }
            }
        }

        private void AddLeaveDownloads()
        {
            var list = SharePointUtil.GetList(LeaveDownloadsListName);

            if (list != null)
            {
                string title = this.ddlStartMonthes.SelectedValue + "-01";

                var items = list.Items;

                if (items.Cast<SPListItem>().Any(itm => itm["Title"] != null && title.Equals(itm["Title"])))
                {
                    return;
                }

                var newItem = items.Add();

                newItem["Title"] = title;
                newItem["DownloadAt"] = DateTime.Now;

                newItem.Update();
            }
        }

        private bool ExportData(DataTable list)
        {
            if (this.ddlStartMonthes.Items.Count == 0)
            {
                return false;
            }

            try
            {
                const string tmpPath = "/tmpfiles/LeaveData/";

                string serverPath = this.Server.MapPath(tmpPath);

                Directory.CreateDirectory(serverPath);

                string fileName = "LeaveData-" + this.ddlStartMonthes.SelectedValue + ".xls";

                string filePath = Path.Combine(serverPath, fileName);

                if (!File.Exists(filePath))
                {
                    SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");

                    var excelFile = new ExcelFile();
                    ExcelWorksheet sheet1 = excelFile.Worksheets.Add("sheet1");

                    var rows = list.Rows;

                    sheet1.Rows[0].InsertEmpty(2 + rows.Count);

                    sheet1.Cells[0, 0].Value = "Year/Month: ";
                    sheet1.Cells[0, 1].Value = this.ddlStartMonthes.SelectedValue;

                    sheet1.Cells[1, 0].Value = "Employee ID";
                    sheet1.Cells[1, 1].Value = "Employee Name";
                    sheet1.Cells[1, 2].Value = "Wage Type";
                    sheet1.Cells[1, 3].Value = "Date";
                    sheet1.Cells[1, 4].Value = "Number";
                    sheet1.Cells[1, 5].Value = "Data Type";

                    int i = 2;

                    foreach (DataRow row in rows)
                    {
                        sheet1.Cells[i, 0].Value = row["EmployeeID"];
                        sheet1.Cells[i, 1].Value = row["EmployeeName"];
                        sheet1.Cells[i, 2].Value = row["TimeWageType"];

                        if (row["Date"] != null)
                        {
                            sheet1.Cells[i, 3].Value = Convert.ToDateTime(row["Date"]).ToString("yyyy.MM.dd");
                        }

                        sheet1.Cells[i, 4].Value = row["Number"];
                        sheet1.Cells[i++, 5].Value = row["DataType"];
                    }

                    excelFile.SaveXls(filePath);
                }

                ScriptManager.RegisterStartupScript(this.btnDownload, typeof(Button), "downloadExcel", "popexcel('" + tmpPath + fileName + "');", true);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}