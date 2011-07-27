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

//<!-- FieldName="标题"
//          FieldInternalName="Title"
//          FieldType="SPFieldText"
//       -->
//<!-- FieldName="正文"
//            FieldInternalName="Body"
//            FieldType="SPFieldNote"
//         -->
namespace CA.WorkFlow
{
    public partial class NewsEdit : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SPList list=SPContext.Current.List;
                SPListItem item = SPContext.Current.ListItem;
                this.Title = list.Title;
                this.PagePath.Add(list.Title, SPContext.Current.List.DefaultViewUrl);
                this.PagePath.Add(item.Title, "");

                
                this.txtTitle.Text = item.Title;               
                //this.FCKeditor1.Value = item["Body"] + "";
                if (SPContext.Current.List.Title.Equals("Internal Announcement"))
                {
                    divEmail.Visible = true;
                    ckbEmail.Checked = item["Email"].ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    divEmail.Visible = false;
                }
            }
            this.btnSave.Click+=new EventHandler(btnSave_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.actions.ActionExecuted += new EventHandler<EventArgs>(actions_ActionExecuted);
        }

        void actions_ActionExecuted(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;
            item["Title"] = this.txtTitle.Text.Trim();

            //added by wsq 2010-07-15
            if (this.upload.fileUpload.HasFile)
            {
                string fileName = this.upload.fileUpload.FileName.ToLower();
                var fileBytes = this.upload.fileUpload.FileBytes;

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

        void btnCancel_Click(object sender, EventArgs e)
        {
            GoRedirect();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //string strNews = this.FCKeditor1.Value;

            SPListItem item = SPContext.Current.ListItem;
            item["Title"] = this.txtTitle.Text.Trim();
            //item["Body"] = this.FCKeditor1.Value.Trim();
            //item["Body"] = this.formFieldBody.v
         
            item.Web.AllowUnsafeUpdates = true;
            item.Update();

            //added by wsq 2010-07-15

            //added by wsq 2010-07-15
            if (this.upload.fileUpload.HasFile)
            {
                string fileName = this.upload.fileUpload.FileName.ToLower();
                var fileBytes = this.upload.fileUpload.FileBytes;

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

        void CreateDocAndHtml(string mappath, string fileName, byte[] fileBytes, string tmppath)
        {
            try
            {
                using (FileStream nFile = new FileStream(mappath + fileName, FileMode.Create))
                {
                    nFile.Seek(0, SeekOrigin.Begin);
                    nFile.Write(fileBytes, 0, fileBytes.Length);
                }

                Util.WordToHtml(mappath, fileName, tmppath + fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
