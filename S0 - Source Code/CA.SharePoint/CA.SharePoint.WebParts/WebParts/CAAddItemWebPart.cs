using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using System.Data;
using System.IO;
using System.Xml;
using Microsoft.VisualBasic;
using CodeArt.SharePoint.CamlQuery;

namespace CA.SharePoint
{
    /// <summary>
    /// 增加新闻按钮
    /// </summary>
    public class CAAddItemWebPart : BaseSPWebPart
    {
        private HyperLink hyperLink = null;

        protected override void CreateChildControls()
        {
            if (base.ChildControlsCreated) return;

            hyperLink = new HyperLink();
            hyperLink.CssClass = "CA_additem";

            SPList list = SPContext.Current.List;
            string urlNew = list.ContentTypes[0].NewFormUrl;
            string text = "Add new " + list.Title;

            hyperLink.Text = text;
            hyperLink.NavigateUrl = SPContext.Current.Web.Url + urlNew + "?List=" + list.ID;

            hyperLink.Visible = IsSubmiter();
            this.Controls.Add(hyperLink);

            base.CreateChildControls();

        }


        private bool IsSubmiter()
        {
            QueryField field = new QueryField("Title");
            CA.SharePoint.ISharePointService sps = CA.SharePoint.ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList("NewsApproveConfig");
            SPListItemCollection items = sps.Query(list, field.Equal(SPContext.Current.List.Title), 1);

            if (items != null && items.Count > 0)
            {
                string users = items[0]["Submitted"] + "";
                string strCurrentUser = string.Empty;
                if (SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                    strCurrentUser = HttpContext.Current.User.Identity.Name;
                else
                    strCurrentUser = SPContext.Current.Web.CurrentUser.LoginName;

                if (users.ToLower().Contains(strCurrentUser.ToLower()))
                    return true;
            }
            return false;
        }
    }
}