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
namespace CA.SharePoint.Web
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
                if (!string.IsNullOrEmpty(item["Type1"] + ""))
                {
                    this.ddlType1.SelectedValue = item["Type1"].ToString();
                }
                //this.FCKeditor1.Value = item["Body"] + "";

                //if ( Convert.ToBoolean( item["AttachmentNews"]) )
                //{
                //    base.Script.ExcedJs("showAttachment();");
                //    this.RadioButtonList1.Items[0].Selected = true;
                //}
                //else
                //{
                //    base.Script.ExcedJs("showContent();");
                //    this.RadioButtonList1.Items[1].Selected = true;
                //}
            }
            //this.RadioButtonList1.Items[0].Attributes.Add("onclick", "showAttachment();");
            //this.RadioButtonList1.Items[1].Attributes.Add("onclick", "showContent();");
            this.btnSave.Click += new ImageClickEventHandler(btnSave_Click);
            this.btnCancle.Click += new ImageClickEventHandler(btnCancle_Click);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
           // string strNews = this.

            SPListItem item = SPContext.Current.ListItem;
            item["Title"] = this.txtTitle.Text.Trim();
            item["Type1"] = this.ddlType1.SelectedValue;
           // item["Body"] = this.FCKeditor1.Value.Trim();
          //  item["AttachmentNews"] = this.RadioButtonList1.SelectedValue;
         
            

            //added by wsq 2010-07-15

            if (this.upload.fileUpload.HasFile)
            {
                string fileName = this.upload.fileUpload.FileName.ToLower();
                var fileBytes = this.upload.fileUpload.FileBytes;

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    string tmppath = "/tmpfiles/" + item.ParentList.ID + "-" + item.ID + "/";
                    string mappath = Server.MapPath(tmppath);
                    if (Directory.Exists(mappath))  
                    {
                        //路径存在

                        if (File.Exists(mappath + fileName))   
                        {
                            //文件存在
                            using (FileStream file0 = File.OpenRead(mappath + fileName))
                            {
                                if (fileBytes.Length != file0.Length)   
                                {
                                    //文件大小不同
                                    CreateDocAndHtml(mappath, fileName, fileBytes, tmppath);
                                }
                            }
                        }
                        else
                        {
                            //文件不存在
                            CreateDocAndHtml(mappath, fileName, fileBytes, tmppath);
                        }
                    }
                    else
                    {
                        //路径不存在

                        Directory.CreateDirectory(mappath);
                        CreateDocAndHtml(mappath, fileName, fileBytes, tmppath);
                    }
                }  item.Attachments.Add(fileName, fileBytes);
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
