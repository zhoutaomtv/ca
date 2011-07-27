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
using CA.WorkFlow.UI;
using Microsoft.SharePoint;

namespace CA.WorkFlows.BusinessCard
{
    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.actions.Attributes.Add("onclick", "return CheckComments();");
            hdBody.Value = this.body.ClientID;
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
            //ActionsButton1.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            if (!Page.IsPostBack)
            {
                if (WorkflowContext.Current.Task.Step == "ReceptionistConfirm")
                {
                    hrefPDF.Visible = true;
                    //string beforeSubmitJS = "\nvar exportRequested = false; \n";
                    //beforeSubmitJS += "var beforeFormSubmitFunction = theForm.onsubmit;\n";
                    //beforeSubmitJS += "theForm.onsubmit = function(){ \n";
                    //beforeSubmitJS += "var returnVal = beforeFormSubmitFunction; \n";
                    //beforeSubmitJS += "if(exportRequested && returnVal) {_spFormOnSubmitCalled=false; exportRequested=false;} \n";
                    //beforeSubmitJS += "return returnVal; \n";
                    //beforeSubmitJS += "}; \n";
                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alterFormSubmitEvent", beforeSubmitJS, true);
                    //this.btnExportPDF.Attributes["onclick"] = "javascript:exportRequested=true;";
                }
                else
                {
                    hrefPDF.Visible = false;
                }
            }
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (WorkflowContext.Current.Task.Step == "ReceptionistConfirm")
            {
                string strForkFlowNumber = ((Label)DataForm1.FindControl("lblID")).Text;
                if (!string.IsNullOrEmpty(strForkFlowNumber))
                {
                    hdUrl.Value = "PDF.aspx?WorkFlowNumber=" + strForkFlowNumber;
                }
                else
                {
                    hrefPDF.Visible = false;
                }
            }
        }
        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            WorkflowContext.Current.DataFields["Flag"] = "Approve";
            //if (WorkflowContext.Current.Step == "DepartmentHeadApprove")
            //{
            //    WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistConfirm_User", @"ca\ztao");
                //WorkflowContext.Current.UpdateWorkflowVariable("IsSickLeave", true);
                //WorkflowContext.Current.UpdateWorkflowVariable("HRHeadAccount", @"cx\ztao");
            //}
            if (WorkflowContext.Current.Step == "ReceptionistConfirm" && e.Action == "Confirm")
            {
                WorkflowContext.Current.DataFields["Status"] = "Completed";
            }
            else
            {
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }
            SPFieldUserValueCollection col = WorkFlowUtil.GetApproversValue();
            WorkflowContext.Current.DataFields["Approvers"] = col;
        }

        public string strUrl
        {
            get { return ViewState["strUrl"] == null ? "" : ViewState["strUrl"].ToString(); }
            set { ViewState["strUrl"] = value; }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            Response.Redirect(hdUrl.Value);
        }
        private void CreatePdf( string strWorkFlowNumber)
        {
            string strFileName = strWorkFlowNumber + ".pdf";
            string strPath = "~/pdf";
            DirectoryInfo dinfo = new DirectoryInfo(strPath);
            if (!dinfo.Exists)
            {
                Directory.CreateDirectory(strPath);
            }
            string strFilePath = strPath + "/" + strFileName;
            string strPicPath = "C:/Program Files/Common Files/Microsoft Shared/web server extensions/12/TEMPLATE/LAYOUTS/CAResources/themeCA/images/";
            if (!string.IsNullOrEmpty(DataForm1.ApplicantColorCard))
            {
                strPicPath = strPicPath + DataForm1.ApplicantColorCard + ".jpg";
            }
            else
            {
                strPicPath = "C:/Program Files/Common Files/Microsoft Shared/web server extensions/12/TEMPLATE/LAYOUTS/CAResources/themeCA/images/Card0.jpg";
            }
            SetTable1();
            SetTable2();
            SetTable3();
            SetTable4();
            ExportPDF.CreatePDF(dt1, dt2, dt3, dt4, "Business Card Application", strFilePath, strPicPath, DataForm1.ApplicantColorCard);
            dt1.Dispose();
            dt2.Dispose();
            dt3.Dispose();
            dt4.Dispose();
            dt1 = null;
            dt2 = null;
            dt3 = null;
            dt4 = null;

            FileInfo file = new FileInfo(strFilePath);
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name)); //解决中文文件名乱码    
            HttpContext.Current.Response.AddHeader("Content-length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "appliction/octet-stream";
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.End();

            //网页中打开
            //string path = strFilePath.Replace("\\", "/");
            //FileStream MyFileStream = new FileStream(path, FileMode.Open);
            //ViewPdf(MyFileStream);
        }
        public static DataTable dt1;
        public static DataTable dt2;
        public static DataTable dt3;
        public static DataTable dt4;
        public void SetTable1()
        {
            dt1 = new DataTable();
            DataRow dr;
            dt1.Columns.Add(new DataColumn("1"));
            dt1.Columns.Add(new DataColumn("2"));
            dt1.Columns.Add(new DataColumn("3"));
            dt1.Columns.Add(new DataColumn("4"));
            dr = dt1.NewRow();
            dr[0] = "";
            dr[1] = "Name 姓名";
            dr[2] = "Company 公司";
            dr[3] = "Department 部门";
            dt1.Rows.Add(dr);
            dr = dt1.NewRow();
            dr[0] = "Chinese 中文";
            dr[1] = ((TextBox)DataForm1.FindControl("txtUserName")).Text;
            dr[2] = "西雅衣家(中国)商业有限公司";
            dr[3] = ((TextBox)DataForm1.FindControl("txtDeptName")).Text;
            dt1.Rows.Add(dr);
            dr = dt1.NewRow();
            dr[0] = "English 英文";
            dr[1] = ((TextBox)DataForm1.FindControl("txtEnglishName")).Text;
            dr[2] = "C&A (China )Co., Ltd.";
            dr[3] = ((TextBox)DataForm1.FindControl("txtEnglishDept")).Text;
            dt1.Rows.Add(dr);
        }
        public void SetTable2()
        {
            dt2 = new DataTable();
            DataRow dr;
            dt2.Columns.Add(new DataColumn("11"));
            dt2.Columns.Add(new DataColumn("22"));
            dt2.Columns.Add(new DataColumn("33"));
            dr = dt2.NewRow();
            dr[0] = "";
            dr[1] = "Job Title 职位";
            dr[2] = "Address 地址";
            dt2.Rows.Add(dr);
            dr = dt2.NewRow();
            dr[0] = "Chinese 中文";
            dr[1] = ((TextBox)DataForm1.FindControl("txtJobTitle")).Text;
            dr[2] = DataForm1.ApplicantAddrChi;
            dt2.Rows.Add(dr);
            dr = dt2.NewRow();
            dr[0] = "English 英文";
            dr[1] = ((TextBox)DataForm1.FindControl("txtEnglishJobTitle")).Text;
            dr[2] = DataForm1.ApplicantAddr;
            dt2.Rows.Add(dr);
        }
        public void SetTable3()
        {
            dt3 = new DataTable();
            DataRow dr;
            dt3.Columns.Add(new DataColumn("111"));
            dt3.Columns.Add(new DataColumn("222"));
            dt3.Columns.Add(new DataColumn("333"));
            dt3.Columns.Add(new DataColumn("444"));
            dr = dt3.NewRow();
            dr[0] = "Telephone 电话";
            dr[1] = "Mobile Phone 手机";
            dr[2] = "Fax 传真";
            dr[3] = "E-Mail 电子邮箱";
            dt3.Rows.Add(dr);
            dr = dt3.NewRow();
            if (!string.IsNullOrEmpty(((TextBox)DataForm1.FindControl("txtTelehpone")).Text))
                dr[0] = "+86(21)" + ((TextBox)DataForm1.FindControl("txtTelehpone")).Text;
            else
                dr[0] = string.Empty;
            dr[1] = "+86(21)" + ((TextBox)DataForm1.FindControl("txtMobilePhone")).Text;
            if (!string.IsNullOrEmpty(((TextBox)DataForm1.FindControl("txtFax")).Text))
                dr[2] = "+86(21)" + ((TextBox)DataForm1.FindControl("txtFax")).Text;
            else
                dr[2] = string.Empty;
            dr[3] = ((TextBox)DataForm1.FindControl("txtEmail")).Text.ToLower();
            dt3.Rows.Add(dr);
        }
        public void SetTable4()
        {
            dt4 = new DataTable();
            DataRow dr;
            dt4.Columns.Add(new DataColumn("5"));
            dt4.Columns.Add(new DataColumn("6"));
            dr = dt4.NewRow();
            dr[0] = "Reason for Application 申请理由";
            dr[1] = "Template color";
            dt4.Rows.Add(dr);
            dr = dt4.NewRow();
            dr[0] = ((TextBox)DataForm1.FindControl("txtReasonForApplication")).Text;
            dr[1] = DataForm1.ApplicantColorCard;
            dt4.Rows.Add(dr);
        }

        private void ViewPdf(Stream fs)
        {
            byte[] buffer = new byte[fs.Length];
            fs.Position = 0;
            fs.Read(buffer, 0, (int)fs.Length);
            Response.Clear();
            Response.AddHeader("Content-Length", fs.Length.ToString());
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "inline;FileName=out.pdf");
            fs.Close();
            Response.BinaryWrite(buffer);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
        }
    }
}
