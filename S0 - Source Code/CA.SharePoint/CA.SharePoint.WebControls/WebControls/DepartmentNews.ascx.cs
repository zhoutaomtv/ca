using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using System.Globalization;
using System.Data;


namespace CA.SharePoint.WebControls
{
    public partial class DepartmentNews : System.Web.UI.UserControl
    {
        public string ListUrl
        {
            get;
            set;
        }

        public string NewsViewUrl
        { get; set; }

        public string ViewUrl
        { get; set; }

        public string ListName
        { get; set; }

                
        protected void Page_Load(object sender, EventArgs e)
        {
            filldata();
        }
        private void filldata()
        {
            string strDeptName = string.Empty;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);
            string strDept = this.Page.Request["dept"];
            if (string.IsNullOrEmpty(strDept))
            {
                return;
            }
            foreach (SPListItem item in list.Items)
            {
                if ((item["DisplayName"] + "").ToLower() == strDept.ToLower())
                {
                    strDeptName = item["DisplayName"] + "";
                    break;
                }
            }
            if (string.IsNullOrEmpty(strDeptName))
            {
                strDeptName = strDept;
            }

            SPList lst = sps.GetList(strDeptName + " News");

            if (lst != null)
            {
                SPQuery query = new SPQuery();
                query.RowLimit = 5;
                query.Query = @"<Where><Eq><FieldRef Name='Approved'/><Value Type='Text'>1</Value></Eq></Where>"+
                "<OrderBy><FieldRef ID=\"" + SPBuiltInFieldId.Created_x0020_Date.ToString("B") + "\" Ascending=\"FALSE\"/></OrderBy>";
                
                ListName = lst.Title;

                DataTable dt = lst.GetItems(query).GetDataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Columns.Add("LastModified");

                    foreach (DataRow row in dt.Rows)
                    {
                        row["LastModified"] = Convert.ToDateTime(row["Modified"]).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                    }
                    dt.DefaultView.Sort = "Modified Desc";

                    this.Repeater1.DataSource = dt;
                    this.Repeater1.DataBind();
                }

                this.ViewUrl = lst.DefaultViewUrl;
                this.ListUrl = SPContext.Current.Web.Url + lst.RootFolder.ServerRelativeUrl;
                this.NewsViewUrl = this.ListUrl + @"/DispForm.aspx?id=";

                if (!string.IsNullOrEmpty(Page.Request["Dept"]))
                    this.ViewUrl += ("?Dept=" + Page.Request["Dept"]);
            }
        }
      
    }
}