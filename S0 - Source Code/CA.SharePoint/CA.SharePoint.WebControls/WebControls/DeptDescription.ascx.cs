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
using CA.SharePoint.Utilities.Common;
using System.Collections.Generic;

namespace CA.SharePoint.WebControls
{
    public partial class DeptDescription : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillData();
            }
        }

        private void FillData()
        {
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
                    Label1.Text = item["DisplayName"].ToString() + " Department";
                    Label2.Text = item["Body"].ToString();
                    break;
                }
            }
            //add by caixiang 7.29 如果没有匹配的部门 默认为传入参数的部门
            if (string.IsNullOrEmpty(this.Label1.Text))
            {
                this.Label1.Text = strDept + " Department";
            }
        }
    }
}