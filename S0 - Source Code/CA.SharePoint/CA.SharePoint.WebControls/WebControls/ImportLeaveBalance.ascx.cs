using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using GemBox.Spreadsheet;
using System.Data;
using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint.Utilities;

namespace CA.SharePoint.WebControls
{
    public partial class ImportLeaveBalance : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private string Import(string strFileName)
        {
          

            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new ExcelFile();
            objExcelFile.LoadXls(strFileName);
            DataTable dtNew = new DataTable();
            string strValue = string.Empty;
            string[] columns = new string[] {"Employee", "Year", "AnnualEntitlement", "SickEntitlement", "AnnualBalance", "SickBalance" };
            for (int i = 0; i < columns.Length; i++)
            {
                dtNew.Columns.Add(columns[i]);
            }
            ReadXlsSheetDataBySpreadSheet(objExcelFile, dtNew);
            List<string> Employee = new List<string>();
            //Dictionary<string[],string> dit = new Dictionary<string[], string>();
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                if (dtNew.Rows[i]["Employee"].ToString() == "" || dtNew.Rows[i]["Year"].ToString() == "" ||
                    dtNew.Rows[i]["AnnualEntitlement"].ToString() == "" || dtNew.Rows[i]["SickEntitlement"].ToString() == "")
                {
                    strValue = "Invalid LeaveBalance format, please supply Employee,Year,AnnualEntitlement or SickEntitlement.";
                    return strValue;
                }
                else
                {
                    try
                    {
                        dtNew.Rows[i]["Employee"] = UserProfileUtil.GetEmployeeByDisplayName(dtNew.Rows[i]["Employee"] + "").UserAccount;
                    }
                    catch (Exception ex)
                    {
                        strValue = dtNew.Rows[i]["Employee"] + " does not exist.";
                        return strValue;
                    }
                    if (!Employee.Contains(dtNew.Rows[i]["Employee"].ToString().ToUpper() + dtNew.Rows[i]["Year"].ToString().ToUpper()))
                    {
                        Employee.Add(dtNew.Rows[i]["Employee"].ToString().ToUpper() + dtNew.Rows[i]["Year"].ToString().ToUpper());
                    }
                    else
                    {
                        strValue = "Found duplicate records for the same employee(" + dtNew.Rows[i]["Employee"] + ") in the same year.";
                        return strValue;
                    }
                }
            }
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList("LeaveBalance");
            SPListItem item = null;
            foreach (DataRow dr in dtNew.Rows)
            {
                QueryField field1 = new QueryField("Year", false);
                QueryField field2 = new QueryField("Employee", false);

                DataTable dt = sps.QueryDataTable(list, field1.Equal(dr["Year"]), 0, null);
                int id;
                if (FindUser(dt, dr["Employee"].ToString().Substring(dr["Employee"].ToString().IndexOf('\\')+1),out id))
                {
                    item = list.Items.GetItemById(id);
                }
                else
                {
                    item = list.Items.Add();
                }
                try
                {
                    //item["Department"] = UserProfileUtil.GetEmployeeByDisplayName(dr["Employee"] + "").Department.Split(new char[] { ';', '/' })[0].ToString();//dr["Department"];
                    item["Employee"] = EnsureUser(dr["Employee"].ToString());
                    item["Year"] = dr["Year"];
                    item["AnnualEntitlement"] = Convert.ToDouble(dr["AnnualEntitlement"]);
                    item["SickEntitlement"] = Convert.ToDouble(dr["SickEntitlement"]);
                    item["AnnualBalance"] = Convert.ToDouble(string.IsNullOrEmpty(dr["AnnualBalance"].ToString()) ? "0" : dr["AnnualBalance"].ToString());
                    item["SickBalance"] = Convert.ToDouble(string.IsNullOrEmpty(dr["SickBalance"].ToString()) ? "0" : dr["SickBalance"].ToString());
                    item.Web.AllowUnsafeUpdates = true;
                    item.Update();
                }
                catch(Exception ex)
                {
                    strValue = "Data is invalid or " + dr["Employee"].ToString() + " does not exist.";
                    break;
                }
            }
            return strValue;
        }

        private bool FindUser(DataTable dt, string strLoginName,out int id)
        {
            id = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["Employee"].ToString().ToUpper() == strLoginName.ToUpper())
                    {
                        id = int.Parse(dt.Rows[i]["ID"].ToString());
                        return true;
                    }
                }
            }
            return false;
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

        public static void ReadXlsSheetDataBySpreadSheet(GemBox.Spreadsheet.ExcelFile ExcelFileObj, DataTable SourceDataTable)
        {
            try
            {
                int RowCount = ExcelFileObj.Worksheets[0].Rows.Count;

                for (int i = 1; i < RowCount; i++)
                {
                    DataRow dr = SourceDataTable.NewRow();
                    bool newRow = false;
                    for (int j = 0; j < SourceDataTable.Columns.Count; j++)
                    {
                        dr[j] = ExcelFileObj.Worksheets[0].Rows[i].Cells[j].Value == null ? DBNull.Value : ExcelFileObj.Worksheets[0].Rows[i].Cells[j].Value;
                        if (dr[j] != DBNull.Value)
                            newRow = true;
                    }
                    if(newRow)
                        SourceDataTable.Rows.Add(dr);
                }
            }
            catch
            {
                throw;
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!this.btnFileUpload.HasFile)
            {
                string strAlert = "Please select the file you want to import!";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Msg", "<script>alert('" + strAlert + "')</script>");
                return;
            }

            HttpPostedFile grDataFile = this.btnFileUpload.PostedFile;
            if (grDataFile.ContentType == "application/vnd.ms-excel" || grDataFile.ContentType == "application/octet-stream")
            {
                string strPath = Server.MapPath("/tmpfiles/LeaveBalance/");// "d:/pdf";
                DirectoryInfo dinfo = new DirectoryInfo(strPath);
                if (!dinfo.Exists)
                {
                    Directory.CreateDirectory(strPath);
                }
                string strFileName = strPath + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                grDataFile.SaveAs(strFileName);
                string strMsg = string.Empty;
                strMsg = Import(strFileName);
                if (!string.IsNullOrEmpty(strMsg))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Msg", "<script>alert('" + strMsg + "')</script>");
                else
                    Response.Redirect(Request.RawUrl);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),"Msg", "<script>alert('Only excel file can be imported.')</script>");
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

        }
    }
}