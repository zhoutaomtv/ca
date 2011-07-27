using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GemBox.Spreadsheet;
using System.Data;
using System.IO;
namespace CA.WorkFlow
{
    public class OperateExcel
    {
        public static void ExportConstructionPurchasing(string strFilePath, DataTable dt, string strCostCenter,string strDate, string strTotalPrice, 
            string strInstallation, string strFreigh, string strPackaging,string strSavePath)
        {
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new ExcelFile();
            objExcelFile.LoadXls(strFilePath);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];
            GemBox.Spreadsheet.ExcelWorksheet worksheet2 = objExcelFile.Worksheets[1];
            objExcelFile = new ExcelFile();
            GemBox.Spreadsheet.ExcelWorksheet worksheet3 = objExcelFile.Worksheets.AddCopy(worksheet1.Name, worksheet1);
            GemBox.Spreadsheet.ExcelWorksheet worksheet4 = objExcelFile.Worksheets.AddCopy(worksheet2.Name, worksheet2);
            int len = dt.Rows.Count;
            int j = 0;
            int k = 0;
            for (int i = 10; i < 10 + len; i++)
            {
                worksheet3.Rows[i].InsertEmpty(1);
                worksheet3.Cells[i, 0].Value = ++j;
                worksheet3.Cells[i, 1].Value = dt.Rows[k]["ItemCode"].ToString();
                worksheet3.Cells[i, 2].Value = strCostCenter;
                worksheet3.Cells.GetSubrangeAbsolute(i, 3, i, 6).Merged = true;
                worksheet3.Cells[i, 3].Value = dt.Rows[k]["Discription"].ToString();
                worksheet3.Cells[i, 7].Value = strDate;
                worksheet3.Cells.GetSubrangeAbsolute(i, 8, i, 9).Merged = true;
                worksheet3.Cells[i, 8].Value = dt.Rows[k]["Quantity"].ToString();
                worksheet3.Cells[i, 10].Value = dt.Rows[k]["Unit"].ToString();
                worksheet3.Cells.GetSubrangeAbsolute(i, 11, i, 14).Merged = true;
                worksheet3.Cells[i, 14].Value = dt.Rows[k]["UnitPrice"].ToString(); 
                worksheet3.Cells.GetSubrangeAbsolute(i, 15, i, 19).Merged = true;
                worksheet3.Cells[i, 19].Value = dt.Rows[k]["TotalPrice"].ToString();
                worksheet3.Cells.GetSubrangeAbsolute(i, 20, i, 24).Merged = true;
                worksheet3.Cells[i, 24].Value = "";
                k++;
            }
            j = 0; k = 0;
            worksheet3.Cells[11 + len, 0].Value = strTotalPrice;
            worksheet3.Cells[11 + len, 2].Value = strInstallation;
            worksheet3.Cells[11 + len, 4].Value = strPackaging;
            worksheet3.Cells[11 + len, 6].Value = strFreigh;

            for (int i = 10; i < 10 + len; i++)
            {
                worksheet4.Rows[i].InsertEmpty(1);
                worksheet4.Cells[i, 0].Value = ++j;
                worksheet4.Cells[i, 1].Value = dt.Rows[k]["ItemCode"].ToString();
                worksheet4.Cells[i, 2].Value = strCostCenter;
                worksheet4.Cells.GetSubrangeAbsolute(i, 3, i, 6).Merged = true;
                worksheet4.Cells[i, 3].Value = dt.Rows[k]["Discription"].ToString();
                worksheet4.Cells[i, 7].Value = "";
                worksheet4.Cells.GetSubrangeAbsolute(i, 8, i, 9).Merged = true;
                worksheet4.Cells[i, 8].Value = dt.Rows[k]["Quantity"].ToString();
                worksheet4.Cells[i, 10].Value = dt.Rows[k]["Unit"].ToString();
                worksheet4.Cells.GetSubrangeAbsolute(i, 11, i, 14).Merged = true;
                worksheet4.Cells[i, 14].Value = dt.Rows[k]["UnitPrice"].ToString();
                worksheet4.Cells.GetSubrangeAbsolute(i, 15, i, 19).Merged = true;
                worksheet4.Cells[i, 19].Value = dt.Rows[k]["TotalPrice"].ToString();
                worksheet4.Cells.GetSubrangeAbsolute(i, 20, i, 24).Merged = true;
                worksheet4.Cells[i, 24].Value = "";
                k++;
            }
            worksheet4.Cells[11 + len, 0].Value = strTotalPrice;
            worksheet4.Cells[11 + len, 2].Value = strInstallation;
            worksheet4.Cells[11 + len, 4].Value = strPackaging;
            worksheet4.Cells[11 + len, 6].Value = strFreigh;

            if (File.Exists(strSavePath))
            {
                File.Delete(strSavePath);
            }
            objExcelFile.SaveXls(strSavePath);
        }

        public static void ExportConstructionPurchasing(string strFilePath)
        {
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("E43X-6VAB-CTVW-E9C8");
            GemBox.Spreadsheet.ExcelFile objExcelFile = new ExcelFile();
            objExcelFile.LoadXls(strFilePath);
            GemBox.Spreadsheet.ExcelWorksheet worksheet1 = objExcelFile.Worksheets[0];
            GemBox.Spreadsheet.ExcelWorksheet worksheet2 = objExcelFile.Worksheets[1];
            objExcelFile = new ExcelFile();
            GemBox.Spreadsheet.ExcelWorksheet worksheet = objExcelFile.Worksheets.AddCopy(worksheet1.Name, worksheet1);
            //int len = dt.Rows.Count;
            int j = 0;
            for (int i = 10; i <= 15; i++)
            {
                worksheet.Rows[i].InsertEmpty(1);
                worksheet.Cells[i, 0].Value = i;
                worksheet.Cells[i, 1].Value = i+1;
                worksheet.Cells[i, 2].Value = i+2;
                worksheet.Cells.GetSubrangeAbsolute(i, 3, i, 6).Merged = true;
                worksheet.Cells[i, 3].Value = i+3;
                worksheet.Cells[i, 7].Value = i + 4;
                worksheet.Cells.GetSubrangeAbsolute(i, 8, i, 9).Merged = true;
                worksheet.Cells[i, 8].Value = i+5;
                worksheet.Cells[i, 10].Value = i+6;
                worksheet.Cells.GetSubrangeAbsolute(i, 11, i, 14).Merged = true;
                worksheet.Cells[i, 14].Value = i+7;
                worksheet.Cells.GetSubrangeAbsolute(i, 15, i, 19).Merged = true;
                worksheet.Cells[i, 19].Value = i+8;
                worksheet.Cells.GetSubrangeAbsolute(i, 20, i, 24).Merged = true;
                worksheet.Cells[i, 24].Value = i + 9;
            }
            //worksheet.Cells.GetSubrangeAbsolute(17, 0, 17, 1).Merged = true;
            worksheet.Cells[17, 0].Value = "111";
            //worksheet.Cells.GetSubrangeAbsolute(17, 2, 17, 3).Merged = true;
            worksheet.Cells[17, 2].Value = "222";
            //worksheet.Cells.GetSubrangeAbsolute(17, 4, 17, 5).Merged = true;
            worksheet.Cells[17, 4].Value = "333";
            worksheet.Cells[17, 6].Value = "444";
            string strNewFileName = "text.xls";
            strFilePath = strFilePath.Substring(0, strFilePath.LastIndexOf("\\")) + "\\" + strNewFileName;
            objExcelFile.SaveXls(strFilePath);
        }
    }
}