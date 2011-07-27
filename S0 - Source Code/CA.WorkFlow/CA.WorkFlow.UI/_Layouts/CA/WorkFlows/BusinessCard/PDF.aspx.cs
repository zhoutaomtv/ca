using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using System.IO;
using System.Data;
using CA.WorkFlow.UI.Code;

namespace CA.WorkFlows.BusinessCard
{
    public partial class PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pdf();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            
        }

        private void pdf()
        {
            string strFileName = Request["WorkFlowNumber"] + ".pdf";
            string strPath = Server.MapPath("/tmpfiles/pdf");// "d:/pdf";
            string strFilePath = strPath + "/" + strFileName;
            FileInfo file = new FileInfo(strFilePath);
            if (file.Exists)
            {
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name)); //解决中文文件名乱码    
                Response.AddHeader("Content-length", file.Length.ToString());
                Response.ContentType = "appliction/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            
        }
        
    }
}
