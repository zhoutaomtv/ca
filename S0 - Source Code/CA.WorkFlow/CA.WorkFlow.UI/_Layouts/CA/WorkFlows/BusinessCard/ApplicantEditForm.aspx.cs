using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CA.SharePoint;
using QuickFlow.Core;
using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using CA.WorkFlow.UI;
using System.IO;
using CA.WorkFlow.UI.Code;
using System.Collections.Generic;

namespace CA.WorkFlows.BusinessCard
{
    public partial class ApplicantEditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.actions.OnClientClick += "return CheckValue(this.value);";
            this.actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(actions_ActionExecuting);
            Common.IsShowComments(trComments, tblCommentsList);
        }

        void actions_ActionExecuting(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            if (e.Action.Equals("End", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.DataFields["Status"] = "Cancelled";
                return;
            }

            //验证组里用户是否为空
            List<string> list = WorkFlowUtil.UserListInGroup("wf_Reception");
            if (list.Count == 0)
            {
                DisplayMessage("Unable to submit the application. There is no user in wf_Reception group. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            string HRManager = UserProfileUtil.GetDepartmentManager("HR");
            if (string.IsNullOrEmpty(HRManager))
            {
                DisplayMessage("Unable to submit the application. There is no HR manager defined. Please contact IT for further help.");
                e.Cancel = true;
                return;
            }
            try
            {
                WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadApprove_User", HRManager);
                WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistConfirm_Group", "wf_Reception");
                WorkflowContext.Current.UpdateWorkflowVariable("DepartmentTaskTitle", DataForm1.Applicant.DisplayName + "'s business card request needs approval");
                WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistTaskTitle", DataForm1.Applicant.DisplayName + "'s business card request needs confirm");
                WorkflowContext.Current.UpdateWorkflowVariable("UpdateTaskTitle", "Please complete business card request for " + DataForm1.Applicant.DisplayName);

                WorkflowContext.Current.DataFields["UserName"] = this.DataForm1.EnglishName;
                WorkflowContext.Current.DataFields["PopulateName"] = this.DataForm1.Applicant.PopulateName;
                WorkflowContext.Current.DataFields["Department"] = this.DataForm1.EnglishDepartment;
                WorkflowContext.Current.DataFields["JobTitle"] = this.DataForm1.JobTitle;
                WorkflowContext.Current.DataFields["Telephone"] = this.DataForm1.Telephone;
                WorkflowContext.Current.DataFields["Fax"] = this.DataForm1.Fax;
                WorkflowContext.Current.DataFields["EmailAddress"] = this.DataForm1.EmailAddress;
                WorkflowContext.Current.DataFields["MobilePhone"] = this.DataForm1.ApplicantMobile; 
                WorkflowContext.Current.DataFields["ColorOfCard"] = this.DataForm1.ApplicantColorCard;
                WorkflowContext.Current.DataFields["ReasonApplication"] = this.DataForm1.ApplicantReason;

                WorkflowContext.Current.DataFields["DepartmentChi"] = this.DataForm1.ApplicantDept;
                WorkflowContext.Current.DataFields["JobTitleChi"] = this.DataForm1.ApplicantJobTitle;
                WorkflowContext.Current.DataFields["Addr"] = this.DataForm1.ApplicantAddr;
                WorkflowContext.Current.DataFields["AddrChi"] = this.DataForm1.ApplicantAddrChi;
                WorkflowContext.Current.DataFields["UserNameChi"] = this.DataForm1.ApplicantChiName;
                WorkflowContext.Current.DataFields["ApplyDate"] = this.DataForm1.ApplicantDate;
                WorkflowContext.Current.DataFields["AppliedUser"] = this.DataForm1.Applicant.UserAccount;

                WorkflowContext.Current.DataFields["Flag"] = "Submit";
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
                CreatePdf(((Label)DataForm1.FindControl("lblID")).Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void actions_ActionExecuted(object sender, EventArgs e)
        {
            
        }
        private void UpdateRecords()
        {
            
            SPListItem item = SPContext.Current.ListItem;
            item["UserName"] = this.DataForm1.EnglishName;
            item["PopulateName"] = this.DataForm1.Applicant.PopulateName;
            item["Department"] = this.DataForm1.EnglishDepartment;
            item["JobTitle"] = this.DataForm1.JobTitle;
            item["Telephone"] = this.DataForm1.Telephone;
            item["Fax"] = this.DataForm1.Fax;
            item["EmailAddress"] = this.DataForm1.EmailAddress;
            item["MobilePhone"] = this.DataForm1.ApplicantMobile; 
            item["ColorOfCard"] = this.DataForm1.ApplicantColorCard;
            item["ReasonApplication"] = this.DataForm1.ApplicantReason;
            item["DepartmentChi"] = this.DataForm1.ApplicantDept;
            item["JobTitleChi"] = this.DataForm1.ApplicantJobTitle;
            item["Addr"] = this.DataForm1.ApplicantAddr;
            item["AddrChi"] = this.DataForm1.ApplicantAddrChi;
            item["UserNameChi"] = this.DataForm1.ApplicantChiName;
            item["ApplyDate"] = this.DataForm1.ApplicantDate;
            item["AppliedUser"] = this.DataForm1.Applicant.UserAccount;
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        item.Web.AllowUnsafeUpdates = true;
                        item.Update();
                        item.Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occured while updating the items");
            }
            //item.Web.AllowUnsafeUpdates = true;
            //item.Update();
        }

        private void CreatePdf(string strWorkFlowNumber)
        {
            string strFileName = strWorkFlowNumber + ".pdf";
            string strPath = Server.MapPath("/tmpfiles/pdf");// "d:/pdf";
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
            dr[2] = "C&A (China) Co., Ltd.";
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
            if (!string.IsNullOrEmpty(((TextBox)DataForm1.FindControl("txtTelehpone")).Text.Trim()))
                dr[0] = "+86(21)" + ((TextBox)DataForm1.FindControl("txtTelehpone")).Text;
            else
                dr[0] = string.Empty;
            dr[1] = "+86(21)" + ((TextBox)DataForm1.FindControl("txtMobilePhone")).Text;
            if (!string.IsNullOrEmpty(((TextBox)DataForm1.FindControl("txtFax")).Text.Trim()))
                dr[2] = "+86(21)" + ((TextBox)DataForm1.FindControl("txtFax")).Text;
            else
                dr[2] = string.Empty;
            dr[3] = ((TextBox)DataForm1.FindControl("txtEmail")).Text.Replace("C-AND-A.CN", "c-and-a.cn");
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UpdateRecords();
            base.Back();
        }
    }
}
