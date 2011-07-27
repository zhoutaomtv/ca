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
using System.Text;
using CA.SharePoint.Utilities.Common;
using System.Collections.Generic;

namespace CA.SharePoint.WebControls
{
    public partial class QuickNavigator : System.Web.UI.UserControl
    {
        //modified by wsq 2010-07-09

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);

            if (!IsPostBack)
            {
                FillData();
            }
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            var curLoginName = SPContext.Current.Web.CurrentUser.LoginName;

            //删除原有
            string swhere = @"<Where><Eq><FieldRef Name='userLoginName'/><Value Type='Text'>{0}</Value></Eq></Where>";
            SPQuery query = new SPQuery();
            query.Query = String.Format(swhere, curLoginName);

            SPListItemCollection coll = SPContext.Current.Site.RootWeb.Lists["QuickNavigator"].GetItems(query);
            for (var i = coll.Count-1; i >=0; i--)
            {
                SPListItem item = coll[i];
                item.Web.AllowUnsafeUpdates = true;
                item.Delete();
            }

            //添加
            for (var i = 0; i < 5; i++)
            {
                TextBox txtTitle = (TextBox)psetup.FindControl("txtTitle" + i.ToString());
                TextBox txtValue = (TextBox)psetup.FindControl("txtValue" + i.ToString());
                if (!String.IsNullOrEmpty(txtTitle.Text))
                {
                    SPListItem itemToAdd = SPContext.Current.Site.RootWeb.Lists["QuickNavigator"].Items.Add();

                    itemToAdd["userLoginName"] = SPContext.Current.Site.RootWeb.CurrentUser.LoginName;
                    itemToAdd["Title"] = txtTitle.Text;
                    itemToAdd["ValueCell"] = txtValue.Text;

                    itemToAdd.Web.AllowUnsafeUpdates = true;
                    itemToAdd.Update();
                }
            }

            FillData();
        }

        void FillData()
        {
            string curLoginName = SPContext.Current.Web.CurrentUser.LoginName;

            string swhere = @"<Where><Eq><FieldRef Name='userLoginName'/><Value Type='Text'>{0}</Value></Eq></Where>";
            SPQuery query = new SPQuery();
            query.Query = String.Format(swhere, curLoginName);
            SPListItemCollection coll = SPContext.Current.Site.RootWeb.Lists["QuickNavigator"].GetItems(query);

            int count = coll.Count;
            for (int i = 0; i < 5; i++)
            {
                TextBox txtTitle = (TextBox)psetup.FindControl("txtTitle" + i.ToString());
                TextBox txtValue = (TextBox)psetup.FindControl("txtValue" + i.ToString());
                Label lblNav = (Label)psetup.FindControl("nav" + i.ToString());

                int num = i + 1;
                var prefix = "<p>0" + num + "</p>";

                if (i >= count)
                {
                    txtTitle.Text = "";
                    txtValue.Text = "";

                    lblNav.Text = prefix;
                }
                else
                {
                    txtTitle.Text = coll[i]["Title"] + "";
                    txtValue.Text = coll[i]["ValueCell"] + "";
                    lblNav.Text = "<a href=\"" + txtValue.Text + "\" target=\"_blank\">" + prefix + "<h1>" + txtTitle.Text + "</h1></a>";
                }

            }

        }

        
    }
}