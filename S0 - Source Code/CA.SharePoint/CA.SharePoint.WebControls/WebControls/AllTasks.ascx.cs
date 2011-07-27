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
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections.Generic;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint.WebControls;

namespace CA.SharePoint.WebControls
{
    public partial class AllTasks : System.Web.UI.UserControl
    {        
        DataTable EnsureDataTableRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TaskTitle");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("WorkflowUrl");
            dt.Columns.Add("WorkflowName");
            return dt;
        }


        protected override void CreateChildControls()
        {
            //SPBoundField f = new SPBoundField();
            //f.DataField = SPBuiltInFieldId.Title.ToString("B");
            //f.HeaderText = "Task";
            //this.gvList.Columns.Add(f);

            //f = new SPBoundField();
            //f.DataField = SPBuiltInFieldId.Created_x0020_Date.ToString("B");
            //f.HeaderText = "Start Time";
            //this.gvList.Columns.Add(f);

            ////f = new SPBoundField();
            ////f.DataField = SPBuiltInFieldId.Created_x0020_Date.ToString("B");
            ////f.HeaderText = "";
            ////this.gvList.Columns.Add(f);

            //HyperLinkField link = new HyperLinkField();
            //link.HeaderText = "Approve";
            //link.Text = "Approve";

            base.CreateChildControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {          
            this.gvList.DataSource = GetUserTasks();
            this.gvList.DataBind();
        }    

        DataTable GetUserTasks()
        {
            DataTable dtBind = EnsureDataTableRow();


            SPWeb web = SPContext.Current.Site.RootWeb;

            SPSiteDataQuery query = new SPSiteDataQuery();


            string swhere = @"<Where><And>
            <Eq>
            <FieldRef Name=""AssignedTo"" LookupId=""TRUE""/>
            <Value Type=""Integer"">{0}</Value>
            </Eq>
            <Neq>
            <FieldRef Name=""Status"" />
            <Value Type=""Text"">{1}</Value>
            </Neq>
            </And></Where>";

            string scompleted = SPUtility.GetLocalizedString("$Resources:core,Tasks_Completed;", "core", web.Language);

            swhere = String.Format(swhere, web.CurrentUser.ID, scompleted);

            string sorderby = "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.WorkflowName.ToString("B") + "\"/><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\" Ascending=\"FALSE\"/></OrderBy>";
            query.Query = swhere + sorderby;

            query.Lists = "<Lists ServerTemplate=\"107\"/>";

            query.ViewFields = "<FieldRef ID=\"" + SPBuiltInFieldId.Title.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskDueDate.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.UniqueId.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Completed.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.PercentComplete.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskStatus.ToString("B") + "\" Nullable=\"TRUE\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.WorkflowLink.ToString("B") + "\" Nullable=\"TRUE\" Type=\"URL\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FileRef.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.ID.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FSObjType.ToString("B") + "\"/>";
            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.WorkflowName.ToString("B") + "\"/>";
            //query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_By.ToString("B") + "\"/>";

            query.Webs = "<Webs Scope=\"Recursive\" />";

            string createdDateFieldId = SPBuiltInFieldId.Created_x0020_Date.ToString("B");

            string uIdField = SPBuiltInFieldId.UniqueId.ToString("B");
            string listField = SPBuiltInFieldId.FileRef.ToString("B");
           
            DataTable t = web.GetSiteData(query);
            if (t != null && t.Rows.Count > 0)
            {
                t.DefaultView.Sort = createdDateFieldId + " Desc";
            }

            string[] sep = new string[] { ";#" };
            t.Columns.Add("WorkFlowUrl");

            DataRow dr = null;
            foreach (DataRow row in t.Rows)
            {
                dr = dtBind.Rows.Add();
                string createdDate = "" + row[createdDateFieldId];
                string[] tempArr = createdDate.Split(sep, StringSplitOptions.None);

                if (tempArr.Length > 1)
                    row[createdDateFieldId] = Convert.ToDateTime(tempArr[1]).ToString("yyyy-MM-dd");

                row[uIdField] = row[uIdField].ToString().Split(sep, StringSplitOptions.None)[1];
                //35;#WorkFlowCenter/Lists/Tasks/35_.000
                string workflowUrl = row[listField].ToString().Split(sep, StringSplitOptions.None)[1];
                int index = workflowUrl.LastIndexOf(@"/");
                workflowUrl = SPContext.Current.Site.RootWeb.Url + "/" + workflowUrl.Remove(index) + "/DispForm.aspx?ID=" + row[SPBuiltInFieldId.ID.ToString("B")];
                row["WorkFlowUrl"] = workflowUrl;

                dr["TaskTitle"] = row[SPBuiltInFieldId.Title.ToString("B")];
                dr["StartTime"] = row[createdDateFieldId];
              //  dr["CreatedBy"] = row[SPBuiltInFieldId.Created_x0020_By.ToString("B")];
                dr["WorkflowName"] = row[SPBuiltInFieldId.WorkflowName.ToString("B")];
                dr["WorkflowUrl"] = workflowUrl + "&Source="+ this.Page.Request.RawUrl;
            }

            return dtBind;
        }

//        DataTable GetRootTasks()
//        {

//            SPWeb web = SPContext.Current.Site.RootWeb;

//            SPSiteDataQuery query = new SPSiteDataQuery();


//            string swhere = @"<Where><And>
//            <Eq>
//            <FieldRef Name=""AssignedTo"" LookupId=""TRUE""/>
//            <Value Type=""Integer"">{0}</Value>
//            </Eq>
//            <Neq>
//            <FieldRef Name=""Status"" />
//            <Value Type=""Text"">{1}</Value>
//            </Neq>
//            </And></Where>";

//            string scompleted = SPUtility.GetLocalizedString("$Resources:core,Tasks_Completed;", "core", web.Language);

//            swhere = String.Format(swhere, web.CurrentUser.ID, scompleted);

//            string sorderby = "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\"/></OrderBy>";
//            query.Query = swhere + sorderby;

//            query.Lists = "<Lists ServerTemplate=\"107\"/>";

//            query.ViewFields = "<FieldRef ID=\"" + SPBuiltInFieldId.Title.ToString("B") + "\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskDueDate.ToString("B") + "\" Nullable=\"TRUE\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.UniqueId.ToString("B") + "\" Nullable=\"TRUE\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Completed.ToString("B") + "\" Nullable=\"TRUE\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.PercentComplete.ToString("B") + "\" Nullable=\"TRUE\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.TaskStatus.ToString("B") + "\" Nullable=\"TRUE\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.WorkflowLink.ToString("B") + "\" Nullable=\"TRUE\" Type=\"URL\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FileRef.ToString("B") + "\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.ID.ToString("B") + "\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\"/>";
//            query.ViewFields = query.ViewFields + "<FieldRef ID=\"" + SPBuiltInFieldId.FSObjType.ToString("B") + "\"/>";

//            query.Webs = "<Webs Scope=\"Recursive\" />";

//            string createdDateFieldId = SPBuiltInFieldId.Created_x0020_Date.ToString("B");

//            string uIdField = SPBuiltInFieldId.UniqueId.ToString("B");
//            string listField = SPBuiltInFieldId.FileRef.ToString("B");

//            query.RowLimit = 5;
//            DataTable t = web.GetSiteData(query);
//            if (t != null && t.Rows.Count > 0)
//            {
//                t.DefaultView.Sort = createdDateFieldId + " Desc";
//            }

//            string[] sep = new string[] { ";#" };
//            t.Columns.Add("WorkFlowUrl");
//            foreach (DataRow row in t.Rows)
//            {
//                string createdDate = "" + row[createdDateFieldId];
//                string[] tempArr = createdDate.Split(sep, StringSplitOptions.None);

//                if (tempArr.Length > 1)
//                    row[createdDateFieldId] = Convert.ToDateTime(tempArr[1]).ToString("yyyy-MM-dd");

//                row[uIdField] = row[uIdField].ToString().Split(sep, StringSplitOptions.None)[1];
//                //35;#WorkFlowCenter/Lists/Tasks/35_.000
//                string workflowUrl = row[listField].ToString().Split(sep, StringSplitOptions.None)[1];
//                int index = workflowUrl.LastIndexOf(@"/");
//                workflowUrl = SPContext.Current.Site.RootWeb.Url + "/DispForm.aspx?ID=" + row[SPBuiltInFieldId.ID.ToString("B")];
//                row["WorkFlowUrl"] = workflowUrl;
//            }

//            return t;
//        }
    }
}