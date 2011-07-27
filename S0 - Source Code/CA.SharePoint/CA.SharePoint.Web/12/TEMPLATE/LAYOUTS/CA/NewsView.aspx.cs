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
    public partial class NewsView : SPLayoutsPageBase
    {
        private string _FileUrl=string.Empty;

        public string FileUrl
        {
            get { return _FileUrl; }
            set { _FileUrl = value; }
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                SPListItem item = SPContext.Current.ListItem;

                this.PagePath.Add(SPContext.Current.List.Title, SPContext.Current.List.DefaultViewUrl);
                this.PagePath.Add(item.Title, "");

                this.Title = SPContext.Current.List.Title;

                if (item.Attachments.Count>0)
                {                 
                    this.divFile.Visible = true;

                    SPFolder folder = SPContext.Current.List.RootFolder.SubFolders["Attachments"].SubFolders[item.ID.ToString()];

                    SPFile file = folder.Files[0];

                    
                    //this.FileUrl = file.ServerRelativeUrl;
                    this.FileUrl = this.Page.Request.RawUrl.Replace("NewsView", "NewsFormDoc");
                    //this.Page.Response.Redirect(this.Page.Request.RawUrl.Replace("NewsView", "NewsFormDoc"));
                }
                else
                {                 
                    this.divFile.Visible = false;
                }

                this.litTitle.Text = item.Title;
                this.litCreated.Text =new SPFieldLookupValue (item["Modified By"] + "").LookupValue;
                this.litTime.Text = DateTime.Parse( item["Modified"] + "").ToString("yyyy-MM-dd");
                this.litBody.Text = item["Body"] + "";
            }      
        }

        protected string GetFrameSrc()
        {
            return this.Page.Request.RawUrl.Replace("NewsView", "NewsFormDoc");
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

        //public string dd()
        //{
        //    SPSite oSiteCollection = SPContext.Current.Site;
        //    SPWebCollection collWebsites = oSiteCollection.AllWebs;

        //    SPWeb oWebsite = collWebsites["Site_Name"];
        //    SPFolder oFolder = oWebsite.Folders["Shared Documents"];

        //    foreach (SPWeb oWebsiteNext in collWebsites)
        //    {
        //        SPList oList = oWebsiteNext.Lists["List_Name"];
        //        SPListItem oListItem = oList.Items[0];
        //        SPAttachmentCollection collAttachments = oListItem.Attachments;

        //        collAttachments[

        //        SPFileCollection collFiles = oFolder.Files;

        //        foreach (SPFile oFile in collFiles)
        //        {
        //            string strFileName = oFile.Name;

        //            byte[] binFile = oFile.OpenBinary();

        //            collAttachments.Add(strFileName, binFile);
        //        }

        //        oListItem.Update();
        //        oWebsiteNext.Dispose();
        //    }
        //    oWebsite.Dispose();
        //}
    }
}
