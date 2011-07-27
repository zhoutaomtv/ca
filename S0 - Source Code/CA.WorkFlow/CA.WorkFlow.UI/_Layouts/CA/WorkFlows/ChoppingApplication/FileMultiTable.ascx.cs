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
using Microsoft.SharePoint.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;


namespace CA.WorkFlow.UI.ChoppingApplication
{
    public partial class FileMultiTable : QFUserControl, IValidator
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack)
            //    return;           
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!string.IsNullOrEmpty(this.ProjectFolderUrl)&&this.ControlMode!=SPControlMode.Display)
            {              
                this.trAdd.Visible = true;               
            }
            else
            {
                this.trAdd.Visible = false;
            }
//            if (!string.IsNullOrEmpty(this.ProjectFolderUrl))
                FillProjectFiles();
//            else
//                this.labProject.Text = string.Empty;
        }

        public string ProjectFolderUrl
        {
            get
            {
                return ViewState["ProjectFolderUrl"] + "";
            }
            set
            {
                ViewState["ProjectFolderUrl"] = value;
            }
        }

        public DataTable DataTableProjectFiles
        {
            get
            {
                this.EnSureDataTable();
                return (DataTable)ViewState["dtProjectFiles"];
            }
            set
            {                
                ViewState["dtProjectFiles"] = value;
            }
        }

        private bool _SureDataTable = false;
        public bool SureDataTable
        {
            get { return _SureDataTable; }
            set { _SureDataTable = value; }
        }

        private void EnSureDataTable()
        {
            if (ViewState["dtProjectFiles"] == null)
            {
                DataTableProjectFiles = new DataTable();
                DataTableProjectFiles.Columns.Add("ID");
                DataTableProjectFiles.Columns.Add("Name");
                DataTableProjectFiles.Columns.Add("Price");
                DataTableProjectFiles.Columns.Add("Count");
                DataTableProjectFiles.Columns.Add("Description");
                DataTableProjectFiles.Columns.Add("FileUrl");
            }
        }

        private void FillProjectFiles()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listProjectFiles = sps.GetList(DataForm.Constants.DocumentChoppingProjectFiles);
            SPFolder folder = null;
            if (!string.IsNullOrEmpty(this.ProjectFolderUrl))
                folder = SharePointUtil.GetFolder(SPContext.Current.Web, listProjectFiles, this.ProjectFolderUrl, true);
            this.DataTableProjectFiles.Rows.Clear();
            if (folder != null)
            {
                SPQuery query = new SPQuery();
                query.Folder = folder;

                this.labProject.Text = folder.Name;
                SPListItemCollection items = listProjectFiles.GetItems(query);

                DataTable dt = items.GetDataTable();
                DataRow rowAdd = null;

                if (dt != null && dt.Rows != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        SPListItem item = items.GetItemById(Convert.ToInt32(row["ID"]));
                        string url = SPContext.Current.Site.RootWeb.Url + item.File.ServerRelativeUrl;
                        rowAdd = this.DataTableProjectFiles.Rows.Add();
                        rowAdd["Name"] = row["FileLeafRef"] + "";
                        rowAdd["Price"] = row["Price"] + "";
                        rowAdd["Count"] = row["Count"] + "";
                        rowAdd["ID"] = row["ID"] + "";
                        rowAdd["Description"] = row["Description0"] + "";
                        rowAdd["FileUrl"] = url;
                        rowAdd = null;
                    }
                }
            }
            else
            {
                this.labProject.Text = string.Empty;
            }

            BindDTProject(Repeater1);
        }

        private void BindDTProject(Repeater rpt)
        {
            rpt.DataSource = this.DataTableProjectFiles;
            rpt.DataBind();
        }     

        public override void SetControlMode()
        {
            base.SetControlMode();
            if (this.ControlMode == SPControlMode.Display)
            {
                this.trAdd.Visible = false;
            }
            else
            {
                this.trAdd.Visible = true;
            }
        }  

  

        #region IValidator Members
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.Validators.Add(this);
        }

        private string _ErrorMessage = "Please supply valid details.";
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        private bool _IsValid=true;
        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
            set
            {
                this._IsValid = value;
            }
        }

        public void Validate()
        {
            if (this.ControlMode == SPControlMode.Display)
            { this.IsValid = true; return; }

           
            this.IsValid = true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!IsValid)
            {
                string script = "alert('" + ErrorMessage + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);              
            }
        }
        #endregion

        protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listProjectFiles = sps.GetList(DataForm.Constants.DocumentChoppingProjectFiles);
            SPFolder folder = SharePointUtil.EnsureFolder(SPContext.Current.Web, listProjectFiles, this.ProjectFolderUrl);
            if (this.fileUpload.HasFile)
            {               
                bool fileExist = SPContext.Current.Web.GetFile(@"ChoppingProjectFiles/"+this.ProjectFolderUrl + @"/" + this.fileUpload.FileName).Exists;

                if (fileExist)
                {
                    string script = string.Format("alert('A file with the name {0} already exists');", this.fileUpload.FileName);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);
                    return;
                }
                byte[] contents = this.fileUpload.FileBytes;
                SPFile file = folder.Files.Add(this.fileUpload.FileName, contents);

               
                if(!string.IsNullOrEmpty(this.txtPrice.Text.Trim()))
                {
                    double price =0;
                    if( double.TryParse(this.txtPrice.Text.Trim(),out price))
                    {
                        file.Item["Price"] = price;    
                    }                    
                }
                if (!string.IsNullOrEmpty(this.txtCount.Text.Trim()))
                {
                    double count = 0;
                    if (double.TryParse(this.txtCount.Text.Trim(), out count))
                    {
                        file.Item["Count"] = count;
                    }
                }
                file.Item["Description"] = this.txtDescription.Text.Trim();
                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                        {
                            file.Item.Web.AllowUnsafeUpdates = true;
                            file.Item.Update();
                            file.Item.Web.AllowUnsafeUpdates = false;                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("An error occured while updating the items");
                }

                
            }
            ClearText();
        
           // FillProjectFiles();
        }

        private void ClearText()
        {
            this.txtPrice.Text = "";
            this.txtCount.Text = "";
            this.txtDescription.Text = "";
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int fileID = Convert.ToInt32(e.CommandArgument);
                ISharePointService sps = ServiceFactory.GetSharePointService(true);
                SPList listProjectFiles = sps.GetList(DataForm.Constants.DocumentChoppingProjectFiles);
                SPListItem item = listProjectFiles.GetItemById(fileID);
                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                        {
                            item.Web.AllowUnsafeUpdates = true;
                            item.Delete();
                            item.Web.AllowUnsafeUpdates = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("An error occured while updating the items");
                }

              
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (this.ControlMode == SPControlMode.Display)
                {
                    ((ImageButton)e.Item.FindControl("ImageButton1")).Visible = false;
                }
            }
        }
    }
}