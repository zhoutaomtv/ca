using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using QuickFlow.Core;
using CA.WorkFlow.UI;
using System.Data;
using CA.WorkFlow.UI.Code;
using System.IO;
using Microsoft.SharePoint;
using QuickFlow.UI.Controls;
using CodeArt.SharePoint.CamlQuery;

namespace CA.WorkFlows.BusinessCard
{
    public partial class NewForm : CAWorkFlowPage
    {
        private string _WorkFlowNumber;

        public string WorkFlowNumber
        {
            get { return _WorkFlowNumber; }
            set { _WorkFlowNumber = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.OnClientClick = "return CheckValue('')";
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartWorkflowButton btn = sender as StartWorkflowButton;
            string HRManager = string.Empty;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
                WorkflowContext.Current.DataFields["Flag"] = "Save";
                WorkflowContext.Current.DataFields["Status"] = "NonSubmit";
            }
            else
            {
                //验证组里用户是否为空
                List<string> list = WorkFlowUtil.UserListInGroup("wf_Reception");
                if (list.Count == 0)
                {
                    DisplayMessage("Unable to submit the application. There is no user in wf_Reception group. Please contact IT for further help.");
                    e.Cancel = true;
                    return;
                }
                HRManager = UserProfileUtil.GetDepartmentManager("HR");
                if (string.IsNullOrEmpty(HRManager))
                {
                    DisplayMessage("Unable to submit the application. There is no HR manager defined. Please contact IT for further help.");
                    e.Cancel = true;
                    return;
                }
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", false);
                WorkflowContext.Current.DataFields["Flag"] = "Submit";
                WorkflowContext.Current.DataFields["Status"] = "In Progress";
            }
            try
            {
                string strDepartment = DataForm1.Applicant.Department;

                WorkflowContext.Current.UpdateWorkflowVariable("DepartmentHeadApprove_User", HRManager);
                WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistConfirm_Group", "wf_Reception");
                WorkflowContext.Current.UpdateWorkflowVariable("DepartmentTaskTitle", DataForm1.Applicant.DisplayName + "'s business card request needs approval");
                WorkflowContext.Current.UpdateWorkflowVariable("ReceptionistTaskTitle", DataForm1.Applicant.DisplayName + "'s business card request needs confirm");
                WorkflowContext.Current.UpdateWorkflowVariable("UpdateTaskTitle", "Please complete business card request for " + DataForm1.Applicant.DisplayName);

                //ReceptionistConfirm_User ReceptionistTaskTitle
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
                DateTime time = new DateTime();
                //this.WorkFlowNumber = time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString();// CreateWorkFlowNumber();
                this.WorkFlowNumber = CreateWorkFlowNumber();
                WorkflowContext.Current.DataFields["WorkFlowNumber"] = this.WorkFlowNumber;
                CreatePdf();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            //string deptHead = UserProfileUtil.GetDepartmentManager(this.CurrentEmployee.Department);
        }

        private string CreateWorkFlowNumber()
        {
            return "BC_" + WorkFlowUtil.CreateWorkFlowNumber("BusinessCard").ToString("000000");
        }

        private void CreatePdf()
        { 
            string strFileName = this.WorkFlowNumber + ".pdf";
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
    }
}
