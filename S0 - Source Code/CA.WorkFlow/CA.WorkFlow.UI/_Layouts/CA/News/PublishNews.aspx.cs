using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Microsoft.SharePoint;
using CA.SharePoint;
using System.IO;
using CA.SharePoint.Utilities.Common;
using QuickFlow.Core;
using QuickFlow.UI.Controls;
using CA.WorkFlow.UI;

namespace CA.WorkFlow
{
    public partial class PublishNews : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string listName = "Publih News";
                if (SPContext.Current.List != null)
                {
                    listName = SPContext.Current.List.Title;
                }
                this.Title = SPContext.Current.List.Title;
                this.PagePath.Add(listName, SPContext.Current.List.DefaultViewUrl);
                this.PagePath.Add("New Item", "");
                if (SPContext.Current.List.Title.Equals("Internal Announcement"))
                {
                    divEmail.Visible = true;
                }
                else
                {
                    divEmail.Visible = false;
                }
            }

            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton1.Executed += new EventHandler(StartWorkflowButton1_Executed);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Executing);
            this.StartWorkflowButton2.Executed += new EventHandler(StartWorkflowButton1_Executed);

            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            FileUpload1.Attributes["onkeydown"] = "if (event.keycode == 8){return false;}return true;";
            FileUpload1.Attributes["onbeforeeditfocus"] = "return false;";
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            GoRedirect();
        }

        void StartWorkflowButton1_Executed(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.List.GetItemById(SPContext.Current.ListItem.ID);
            item["Title"] = this.txtTitle.Text.Trim();
            if (SPContext.Current.List.Title.Equals("Internal Announcement"))
                item["Email"] = this.ckbEmail.Checked.ToString();

            //SaveForm();
            var fileName = "";


            //added by wsq 2010-07-15
            if (this.FileUpload1.HasFile)
            {
                fileName = this.FileUpload1.FileName.ToLower();
                var fileBytes = this.FileUpload1.FileBytes;

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    string tmppath = "/tmpfiles/" + item.ParentList.ID + "-" + item.RecurrenceID + "/";
                    string mappath = Server.MapPath(tmppath);

                    Directory.CreateDirectory(mappath);
                    try
                    {
                        using (FileStream nFile = new FileStream(mappath + fileName, FileMode.Create))
                        {
                            nFile.Seek(0, SeekOrigin.Begin);
                            nFile.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    Util.WordToHtml(mappath, fileName, tmppath + fileName);
                }
                //else if (!fileName.EndsWith(".pdf"))
                //{
                //    base.Script.Alert("only word or pdf file can be uploaded.");

                //    e.Cancel = true;
                //    return;
                //}
                
                item.Attachments.Add(fileName, fileBytes);
                
            }

            item.Web.AllowUnsafeUpdates = true;
            item.Update();

            GoRedirect();
        }

        void StartWorkflowButton1_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartWorkflowButton btnStart = sender as StartWorkflowButton;

            if (string.Equals(btnStart.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("IsSave", true);
            }      

            string ApproveUrl = @"_Layouts/CA/News/NewsApprove.aspx";
            string ApproveTaskTitle = @"Please approve news";
            string EditUrl = @"_Layouts/CA/News/NewsEdit.aspx";
            string EditTaskTitle = "Please update news";
            WorkflowContext.Current.UpdateWorkflowVariable("approve_TaskFormURL", ApproveUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("approve_TaskTitle", ApproveTaskTitle);
            WorkflowContext.Current.UpdateWorkflowVariable("edit_TaskFormURL", EditUrl);
            WorkflowContext.Current.UpdateWorkflowVariable("edit_TaskTitle", EditTaskTitle);

            QuickFlow.NameCollection approveUsers = WorkFlowUtil.GetNewsApproveUsers(SPContext.Current.List.Title);
            WorkflowContext.Current.UpdateWorkflowVariable("approve_Users", approveUsers);


            //if (!SPContext.Current.List.Fields.ContainsField("Sumiter"))
            //{
            //    SPContext.Current.List.Fields.Add("Sumiter", SPFieldType.Text, false);
            //}   
            string strNews = this.formFieldBody.Value + "";

            
            //item["Body"] = this.FCKeditor1.Value.Trim();

            if (this.FileUpload1.HasFile)
            {
                var fileName = this.FileUpload1.FileName.ToLower();

                if (!fileName.EndsWith(".doc") && !fileName.EndsWith(".docx") && !fileName.EndsWith(".pdf"))
                {
                    base.Script.Alert("only word or pdf file can be uploaded.");

                    e.Cancel = true;
                    return;
                }
            }
            
            
        }

        void btnSave_Click(object sender, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            //SaveForm();
            string strNews = this.formFieldBody.Value+"";

            SPListItem item = SPContext.Current.List.GetItemById(SPContext.Current.ListItem.ID);
            item["Title"] = this.txtTitle.Text.Trim();
            //item["Body"] = this.FCKeditor1.Value.Trim();
            

            var fileName = "";
            

            //added by wsq 2010-07-15
            if (this.FileUpload1.HasFile)
            {
                fileName = this.FileUpload1.FileName.ToLower();
                var fileBytes = this.FileUpload1.FileBytes;

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    string tmppath = "/tmpfiles/" + item.ParentList.ID+"-"+ item.RecurrenceID + "/";
                    string mappath = Server.MapPath(tmppath);

                    Directory.CreateDirectory(mappath);
                    try
                    {
                        using (FileStream nFile = new FileStream(mappath + fileName, FileMode.Create))
                        {
                            nFile.Seek(0, SeekOrigin.Begin);
                            nFile.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    Util.WordToHtml(mappath, fileName, tmppath + fileName);
                }
                else if (!fileName.EndsWith(".pdf"))
                {
                    base.Script.Alert("only word or pdf file can be uploaded.");

                    e.Cancel = true;
                    return;
                }

                item.Attachments.Add(fileName, fileBytes);
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }

            GoRedirect();
        }

        private void SaveForm()
        {
           // string strNews = this.FCKeditor1.Value;

            string strNews = this.formFieldBody.Value+"";

            SPListItem item = SPContext.Current.List.GetItemById(SPContext.Current.ListItem.ID);
            item["Title"] = this.txtTitle.Text.Trim();
            //item["Body"] = this.FCKeditor1.Value.Trim();
            

            var fileName = "";
            

            //added by wsq 2010-07-15
            if (this.FileUpload1.HasFile)
            {
                fileName = this.FileUpload1.FileName.ToLower();
                var fileBytes = this.FileUpload1.FileBytes;

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    string tmppath = "/tmpfiles/" + item.ParentList.ID+"-"+ item.RecurrenceID + "/";
                    string mappath = Server.MapPath(tmppath);

                    Directory.CreateDirectory(mappath);
                    try
                    {
                        using (FileStream nFile = new FileStream(mappath + fileName, FileMode.Create))
                        {
                            nFile.Seek(0, SeekOrigin.Begin);
                            nFile.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    Util.WordToHtml(mappath, fileName, tmppath + fileName);
                }
                else if (!fileName.EndsWith(".pdf"))
                {
                    base.Script.Alert("only word or pdf file can be uploaded.");

                    return;
                }

                item.Attachments.Add(fileName, fileBytes);
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }

            GoRedirect();
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            GoRedirect();
        }

        private void GoRedirect()
        {
            if (Request.QueryString["Source"] != null && !string.IsNullOrEmpty(Request.QueryString["Source"].ToString()))
                this.Page.Response.Redirect(Request.QueryString["Source"].ToString());
            else
                this.Page.Response.Redirect(SPContext.Current.List.DefaultViewUrl);
        }
    }
}
