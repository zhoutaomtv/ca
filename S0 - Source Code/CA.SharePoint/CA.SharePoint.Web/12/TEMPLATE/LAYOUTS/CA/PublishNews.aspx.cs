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
            }
            //this.RadioButtonList1.Items[0].Attributes.Add("onclick", "showAttachment();");
            //this.RadioButtonList1.Items[1].Attributes.Add("onclick", "showContent();");
            this.btnSave.Click+=new ImageClickEventHandler(btnSave_Click);
            this.btnCancle.Click += new ImageClickEventHandler(btnCancle_Click);

            FileUpload1.Attributes["onkeydown"] = "if (event.keycode == 8){return false;}return true;";
            FileUpload1.Attributes["onbeforeeditfocus"] = "return false;";
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            string fileName = "";
            //add by caixiang 9.13 
            if (this.FileUpload1.HasFile)
            {
                fileName = this.FileUpload1.FileName.ToLower();    
                if (!fileName.EndsWith(".doc") && !fileName.EndsWith(".docx") && !fileName.EndsWith(".pdf"))
                {
                    base.Script.Alert("only word or pdf file can be uploaded.");
                    return;
                }
            }
        

            SPList list = SPContext.Current.List;
            SPListItem item = list.Items.Add();
            item["Title"] = this.txtTitle.Text.Trim();
            item["Body"] = this.formFieldBody.Value;
            item["Type1"] = this.ddlType1.SelectedValue;
            item.Web.AllowUnsafeUpdates = true;
            item.Update();           

            //added by wsq 2010-07-15
            if (this.FileUpload1.HasFile)
            {
                fileName = this.FileUpload1.FileName.ToLower();            
            
                var fileBytes = this.FileUpload1.FileBytes;

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    string tmppath = "/tmpfiles/" + item.ParentList.ID + "-" + item.ID + "/";
                    //string tmppath = "/tmpfiles/" + item.RecurrenceID + "/";
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
