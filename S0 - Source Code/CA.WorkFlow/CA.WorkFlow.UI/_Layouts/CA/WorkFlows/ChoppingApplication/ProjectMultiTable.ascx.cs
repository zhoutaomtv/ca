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
    public partial class ProjectMultiTable : QFUserControl, IValidator
    {
        protected void Page_Load(object sender, EventArgs e)
        {          
            if (IsPostBack)
                return;
            FillProjects();
        }

        public string Projects
        {
            get
            {
                string tmp = string.Empty;
                foreach (DataRow row in DataTableProject.Rows)
                {
                    tmp += row["Name"] + ";";
                }

                return tmp;
            }
        }
        
        public string WorkFlowNumber
        {
            get
            {
                return ViewState["WorkFlowNumber"]+"";
            }
            set { ViewState["WorkFlowNumber"] = value; }
        }

        private void FillProjects()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listProjectFiles = sps.GetList(DataForm.Constants.DocumentChoppingProjectFiles);           
            SPFolder folder = SharePointUtil.EnsureFolder(SPContext.Current.Web, listProjectFiles, this.WorkFlowNumber);
            SPQuery query = new SPQuery();
            query.Folder = folder;          
            SPListItemCollection items = listProjectFiles.GetItems(query);

            DataTable dt = items.GetDataTable();
          

            DataRow rowAdd = null;
            this.DataTableProject.Rows.Clear();
            if (dt != null && dt.Rows != null)
            {
                this.FileMultiTable1.ProjectFolderUrl = this.WorkFlowNumber + "/" + dt.Rows[0]["FileLeafRef"];
                foreach (DataRow row in dt.Rows)
                {
                    rowAdd = this.DataTableProject.Rows.Add();
                    rowAdd["ID"] = row["ID"] + "";
                    rowAdd["Name"] = row["FileLeafRef"] + "";
                    rowAdd = null;
                }
            }
            else
            {
                this.FileMultiTable1.ProjectFolderUrl = string.Empty;
                ClearText();
            }

            BindDTProject(rptProjects);
        }

        private void BindDTProject(Repeater rpt)
        {
            rpt.DataSource = this.DataTableProject;
            rpt.DataBind();
        }     

        public override void SetControlMode()
        {
            base.SetControlMode();
            this.FileMultiTable1.ControlMode = this.ControlMode;
            if (this.ControlMode == SPControlMode.Display)
            {
                this.trAdd.Visible = false;
            }
            else
            {
                this.trAdd.Visible = true;
            }
        }

        public DataTable DataTableProject
        {
            get
            {
                this.EnSureDataTable();
                return (DataTable)ViewState["dtProject"];
            }
            set
            {
                ViewState["dtProject"] = value;
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
            if (ViewState["dtProject"] == null)
            {
                DataTableProject = new DataTable();

                DataTableProject.Columns.Add("ID");
                DataTableProject.Columns.Add("Name");         
            }           
        }

        private DataRow ConvertToDataRow(RepeaterItem item)
        {
            TextBox txtName = (TextBox)item.FindControl("txtName");
            DataRow row = this.DataTableProject.NewRow();
            row["Name"] = txtName.Text;
            return row;
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
            string projectName=this.txtName.Text.Trim();
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listProjectFiles = sps.GetList(DataForm.Constants.DocumentChoppingProjectFiles);         
         
            SPFolder folder = SharePointUtil.EnsureFolder(SPContext.Current.Web, listProjectFiles, this.WorkFlowNumber);

            SPFolder item = folder.SubFolders.Add(projectName);
            FillProjects();

            this.FileMultiTable1.ProjectFolderUrl = this.WorkFlowNumber + "/" + projectName;
            ClearText();
        }

        private void ClearText()
        {
            this.txtName.Text = "";
        }

        protected void rptProjects_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "link")
            {
                this.FileMultiTable1.ProjectFolderUrl = this.WorkFlowNumber + "/" + e.CommandArgument;                
            }
            else if (e.CommandName == "delete")
            {
                string folderUrl = this.WorkFlowNumber + "/" + e.CommandArgument;
                ISharePointService sps = ServiceFactory.GetSharePointService(true);
                sps.DeleteFolder(DataForm.Constants.DocumentChoppingProjectFiles, folderUrl);
                FillProjects();
            }            
        }
     

        protected void rptProjects_ItemDataBound(object sender, RepeaterItemEventArgs e)
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