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
    public partial class NewsFormDoc : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SPListItem item = SPContext.Current.ListItem;
                if (item.Attachments.Count > 0)
                {
                   SPFolder folder =  SPContext.Current.List.RootFolder.SubFolders["Attachments"].SubFolders[item.ID.ToString()];

                   SPFile file = folder.Files[0];

                   if (file.Name.ToLower().EndsWith(".pdf"))
                   {
                       byte[] bytes = file.OpenBinary();

                       Response.Clear();
                       Response.ContentType = "application/pdf";
                       Response.AddHeader("Content-disposition", "inline;filename=" + file.Name);
                       Response.AddHeader("Content-Length", bytes.Length.ToString());
                       Response.BinaryWrite(bytes);
                       Response.End();
                   }
                   else if (file.Name.ToLower().EndsWith(".doc") || file.Name.ToLower().EndsWith(".docx"))
                   {
                       string tmppath = "/tmpfiles/" + item.ParentList.ID + "-" + item.ID + "/";
                       //string mappath = Server.MapPath(tmppath);
                       //try
                       //{

                       //    using (FileStream nFile = new FileStream(mappath + file.Name, FileMode.Create))
                       //    {
                       //        nFile.Seek(0, SeekOrigin.Begin);
                       //        nFile.Write(file.OpenBinary(), 0, file.OpenBinary().Length);
                       //    }
                       //}
                       //catch (Exception ex)
                       //{
                       //    throw ex;
                       //}

                       //string relaUrl = tmppath + file.Name;
                       //Response.Redirect(Util.WordToHtml(mappath, file.Name, relaUrl));
                       Response.Redirect(tmppath + Util.fileEndWithHtml(file.Name));
                       
                   }

                   
                   
                   
                }


            }      
        }

        string fileEndWithHtml(string fileName)
        {
            if(fileName.EndsWith(".doc"))
            {
                return fileName.Substring(0, fileName.Length - 3) + "html";
            }
            else if (fileName.EndsWith(".docx"))
            {
                return fileName.Substring(0, fileName.Length - 4) + "html";
            }
            return "blank.html";
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
