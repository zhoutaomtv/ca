using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.IO;
using CA.SharePoint;
using System.Configuration;
using Microsoft.SharePoint.WebControls;

namespace CA.SharePoint.Web
{
    public partial class FileUpload :UserControl
    {
        public string FileFullName
        {
            get
            {
                return ViewState["filefullname"] == null ? string.Empty : ViewState["filefullname"].ToString();
            }
            set
            {
                ViewState["filefullname"] = value;
            }
        }

        public System.Web.UI.WebControls.FileUpload fileUpload
        {
            get
            {
                return this.fulFileName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.ListItem.Attachments.Count > 0)
            {
                this.lnkFileName.Text = SPContext.Current.ListItem.Attachments[0];
                this.FileFullName = SPContext.Current.ListItem.Attachments[0];
                this.lnkFileName.NavigateUrl = SPContext.Current.ListItem.Attachments.UrlPrefix + this.FileFullName;

                this.fulFileName.Visible = false;
            }
            else
            {
                this.btnDelete.Visible = false;
                this.lnkFileName.Visible = false;
            }
           //this.lnkFileName.Text
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteFile();
            lnkFileName.Text = "";
            lnkFileName.NavigateUrl = "";
            fulFileName.Visible = true;
            lnkFileName.Visible = false;
            btnDelete.Visible = false;
            FileFullName = null;
        }

        private void DeleteFile()
        {
            SPListItem item = SPContext.Current.ListItem;
            item.Attachments.Delete(FileFullName);
            item.Web.AllowUnsafeUpdates = true;
            item.Update();
        }     
    }
}