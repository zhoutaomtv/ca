using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.WorkFlow;
using System.IO;
using System.Data;
namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.ConstructionPurchasing
{
    public partial class testExportExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadExcel();
        }

        private void LoadExcel()
        {
            string strFileName = "standard_PO_" + Request["WorkFlowNumber"] + ".xls";
            string strPath = Server.MapPath("/tmpfiles/excel");// "d:/pdf";
            string strFilePath = strPath + "/" + strFileName;
            FileInfo file = new FileInfo(strFilePath);
            if (file.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Type", "text/plain");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(System.IO.Path.GetFileName(strFilePath), System.Text.Encoding.GetEncoding("GB2312")));
                HttpContext.Current.Response.WriteFile(strFilePath); // 将文件以流的形式传输
                HttpContext.Current.Response.End();
            }
        }
    }
}
