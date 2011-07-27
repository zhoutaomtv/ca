
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Web.Caching;

namespace CA.SharePoint
{
    public class SiteNavigationHorizontal : BaseSPWebPart
    {
        private string _HomeName = "Home";
        [
        DefaultValue("Home"),
        Description("Homepage link name"),
        Category("Display")
        ]
        public string HomeName
        {
            get { return _HomeName; }
            set { _HomeName = value; }
        }

        private bool _IsShowHome = false;

        [
        DefaultValue(false),
        Description("Add homepage link"),
        Category("Display")
        ]
        public bool IsShowHome
        {
            get { return _IsShowHome; }
            set { _IsShowHome = value; }
        }

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                ShowNavigationDetail(base.GetCurrentSPSite() , writer);
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.RegisterCommonJs("SiteNavigationHorizontal.js");

            base.OnPreRender(e);
        }

        private void ShowNavigationDetail(SPSite spSite, System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<ul id='SiteNavigationHorizontal_nav'>");

           // SPSite spSite = base.GetCurrentSPSite();

            SPWeb spWeb = spSite.RootWeb;

            if (IsShowHome)
            {
                //添加主页连接
               
                writer.Write("<li class='home'>");
                writer.Write("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                writer.Write("<tr>");
                writer.Write("<td class=\"menu_lbg\"></td>");
                writer.Write("<td align=\"center\" class=\"menu_cbg\" nowrap><a class='Homelinks' href=\"" + spSite.Url + "\">" + this._HomeName + "</a></td>");
                writer.Write("<td class=\"menu_rbg\"></td>");
                writer.Write("</tr>");
                writer.Write("</table>");
                writer.Write("</li>");
            }
            
            //显示顶级站点quickLaunch
            SPNavigationNodeCollection rootquickLaunchList = spWeb.Navigation.QuickLaunch;
            if ((rootquickLaunchList != null) && (rootquickLaunchList.Count > 0))
            {
                foreach (SPNavigationNode node in rootquickLaunchList)
                {
                    foreach (SPNavigationNode node2 in node.Children)
                    {
                        writer.Write("<li>");
                        writer.Write("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                        writer.Write("<tr>");
                        writer.Write("<td class=\"menu_lbg\"></td>");
                        writer.Write("<td align=\"center\" class=\"menu_cbg\" nowrap><a class='ms-golballinks' href=\"" + node2.Url + "\">" + node2.Title + "</a></td>");
                        writer.Write("<td class=\"menu_rbg\"></td>");
                        writer.Write("</tr>");
                        writer.Write("</table>");
                        writer.Write("</li>");
                    }
                }
            }

            bool showSubWeb = true;

            if (spWeb.AllProperties.Contains("__IncludeSubSitesInNavigation"))
                showSubWeb = Convert.ToBoolean(spWeb.AllProperties["__IncludeSubSitesInNavigation"]);

            //显示非顶级站点quickLaunch
            if (showSubWeb)
                ShowSubwebs(spWeb,writer);

            writer.Write("</ul>");
        
        }

        private void ShowSubwebs(SPWeb spWeb, System.Web.UI.HtmlTextWriter writer)
        {
            //显示非顶级站点quickLaunch
            SPNavigationNodeCollection nodes = spWeb.Navigation.UseShared ? null : spWeb.Navigation.TopNavigationBar;
            if ((nodes != null) && (nodes.Count > 0))
            {
                foreach (SPNavigationNode childNode in nodes)
                {
                    if (childNode.Title != "Home")
                    {
                        writer.Write("<li>");
                        writer.Write("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                        writer.Write("<tr>");
                        writer.Write("<td class=\"menu_lbg\"></td>");
                        writer.Write("<td align=\"center\" class=\"menu_cbg\" nowrap><a class='ms-golballinks' href=\"" + childNode.Url + "\">" + childNode.Title + "</a></td>");
                        writer.Write("<td class=\"menu_rbg\"></td>");
                        writer.Write("</tr>");
                        writer.Write("</table>");
                        writer.Write("</li>");
                    }
                    else
                        continue;
                    //QuickLaunch
                    foreach (SPWeb web in spWeb.GetSubwebsForCurrentUser())
                    {
                        if (web.Url.EndsWith(childNode.Url) && web.Title == childNode.Title)
                        {
                            SPNavigationNodeCollection quickLaunchList = web.Navigation.QuickLaunch;
                            if ((quickLaunchList != null) && (quickLaunchList.Count > 0))
                            {
                                writer.Write("<ul class=\"bg\">");
                                foreach (SPNavigationNode node in quickLaunchList)
                                {
                                    foreach (SPNavigationNode node2 in node.Children)
                                    {
                                        writer.Write("<li>");
                                        writer.Write("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                                        writer.Write("<tr>");
                                        writer.Write("<td class=\"menu_lbg_sub\"></td>");
                                        writer.Write("<td align=\"center\" class=\"menu_cbg_sub\" nowrap><a class='sub_golballinks' href=\"" + node2.Url + "\">" + node2.Title + "</a></td>");
                                        writer.Write("<td class=\"menu_rbg_sub\"></td>");
                                        writer.Write("</tr>");
                                        writer.Write("</table>");
                                        writer.Write("</li>");
                                    }
                                }
                                writer.Write("</ul>");
                            }
                        }
                        web.Dispose();
                    }
                }
            }
        }
    }
}
