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
using CA.SharePoint.Utilities.Common;

namespace CA.SharePoint.WebControls
{
    public partial class UserDetails : System.Web.UI.UserControl
    {
        private Employee _user = null;

        public Employee User
        {
            set { _user = value; }
            get { return _user; }
        }

        protected string strIdType = string.Empty;
        protected string strTitle = string.Empty;
        protected string strPhone = string.Empty;
        protected string strMobile = string.Empty;
        protected string strEmail = string.Empty;
        protected string strPhotoUrl = string.Empty;
        protected string strDept = string.Empty;
        protected string strMore = string.Empty;
        protected string strUserName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindData()
        {
            if (User == null)
                return;

            strUserName = User.DisplayName + "";
            strPhone = User.Phone + "";
            strMobile = User.Mobile + "";
            strEmail = User.WorkEmail + "";
            strPhotoUrl = User.PhotoUrl + "";
            strDept = User.AllDepartment + "";
            strMore = User.More + "";
            strTitle = User.Title + "";

            string[] depts = strDept.Split(new char[] { ';' });
            if (depts.Contains("MTM"))
            {
                strDept = "MTM";
            }
        }
    }
}