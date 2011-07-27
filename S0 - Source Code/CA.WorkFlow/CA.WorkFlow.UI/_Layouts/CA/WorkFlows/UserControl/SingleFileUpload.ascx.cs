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

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.UserControl
{
    public partial class SingleFileUpload : QFUserControl
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

        //public string FileName
        //{
        //    get
        //    {
        //        return ViewState["filename"] == null ? string.Empty : ViewState["filename"].ToString();
        //    }
        //    set
        //    {
        //        ViewState["filename"] = value;
        //        if (string.IsNullOrEmpty(value))
        //            ViewState["filefullname"] = null;
        //        else
        //            ViewState["filefullname"] = WorkFlowUtil.GetAttachmentFolder(SPContext.Current.List.Title, WorkflowNumber) + value;
        //    }
        //}

        public string WorkflowNumber
        {
            get
            {
                return ViewState["workflownumber"] == null ? string.Empty : ViewState["workflownumber"].ToString();
            }
            set
            {
                ViewState["workflownumber"] = value;
            }
        }

        public bool HideDelete
        {
            get
            {
                return ViewState["hidedelete"] == null ? false : (bool)ViewState["hidedelete"];
            }
            set
            {
                ViewState["hidedelete"] = value;
            }
        }
        public bool IsValid
        {
            get
            {
                return (fulFileName.HasFile || !string.IsNullOrEmpty(lnkFileName.NavigateUrl));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SaveAttachment();

            if (string.IsNullOrEmpty(FileFullName))
            {
                fulFileName.Visible = true;
                lnkFileName.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                fulFileName.Visible = false;
                lnkFileName.Visible = true;
                btnDelete.Visible = HideDelete?false:(ControlMode != Microsoft.SharePoint.WebControls.SPControlMode.Display);
                lnkFileName.Text = FileFullName.Substring(FileFullName.LastIndexOf('/') + 1);
                lnkFileName.NavigateUrl = FileFullName;
            }
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

        private void SaveAttachment()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            if ((fulFileName.Visible) && (fulFileName.HasFile))
            {
                string fileName = fulFileName.FileName;
                try
                {
                    Stream fStream = fulFileName.PostedFile.InputStream;
                    byte[] contents = new byte[fStream.Length];

                    fStream.Read(contents, 0, (int)fStream.Length);
                    fStream.Close();

                    SPList list = SharePointUtil.GetList(ConfigurationManager.AppSettings["attachmentLibraryList"]);
                    string path = SPContext.Current.List.Title + "/" + (string.IsNullOrEmpty(WorkflowNumber) ? "temp" : WorkflowNumber);
                    SPFolder folder = sps.EnsureFolder2(list, path);

                    folder.Item.Web.AllowUnsafeUpdates = true;
                    if (FileExists(folder.Files, fileName))
                        folder.Files[fileName].Delete();
                    SPFile file = folder.Files.Add(fileName, contents);
                    FileFullName = file.ServerRelativeUrl;
                }
                catch (Exception ex)
                {
                    FileFullName = null;
                    throw new Exception("Unable to save file. " + ex.Message);
                }
            }
        }

        public string SubmitFile()
        {
            if (string.IsNullOrEmpty(WorkflowNumber))
                return string.Empty;

            string fileName = FileFullName.Substring(FileFullName.LastIndexOf('/') + 1);
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPSite site = new SPSite(SPContext.Current.Site.ID);

            SPList list = SharePointUtil.GetList(ConfigurationManager.AppSettings["attachmentLibraryList"]);
            string temppath = SPContext.Current.List.Title + "/temp";
            string path = SPContext.Current.List.Title + "/" + WorkflowNumber;
            SPFolder tempFolder = sps.EnsureFolder2(list, temppath);
            SPFolder folder = sps.EnsureFolder2(list, path);

            if (FileExists(tempFolder.Files, fileName))
            {
                folder.Item.Web.AllowUnsafeUpdates = true;
                tempFolder.Item.Web.AllowUnsafeUpdates = true;
                try
                {
                    FileFullName = folder.ServerRelativeUrl + "/" + fileName;
                    tempFolder.Files[fileName].MoveTo(FileFullName, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to save file. " + ex.Message);
                }
            }
            return FileFullName;
        }

        private void DeleteFile()
        {
            string fileName = lnkFileName.Text;
            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            try
            {
                SPSite site = new SPSite(SPContext.Current.Site.ID);
                SPList list = SharePointUtil.GetList(ConfigurationManager.AppSettings["attachmentLibraryList"]);
                string path = SPContext.Current.List.Title + "/temp";
                SPFolder folder = sps.EnsureFolder2(list, path);

                if (FileExists(folder.Files, fileName))
                {
                    folder.Item.Web.AllowUnsafeUpdates = true;
                    folder.Files[fileName].Delete();
                }

                if (string.IsNullOrEmpty(WorkflowNumber))
                    return;

                path = SPContext.Current.List.Title + "/" + WorkflowNumber;
                folder = sps.EnsureFolder2(list, path);
                if (FileExists(folder.Files, fileName))
                {
                    folder.Item.Web.AllowUnsafeUpdates = true;
                    folder.Files[fileName].Delete();
                }
            }
            catch (Exception ex)
            {
                FileFullName = null;
                throw new Exception("Unable to delete file. " + ex.Message);
            }
        }

        private bool FileExists(SPFileCollection files, string fileName)
        {
            foreach (SPFile file in files)
            {
                if (file.Name.Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}