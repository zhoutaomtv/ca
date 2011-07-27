using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Web.Caching;

namespace CA.SharePoint
{
    public class SiteNavigation : BaseSPWebPart
    {
        private string _HomeName = "Home";
        [
        DefaultValue("Home"),
        Description("Home page link name"),
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

        protected override void OnPreRender(EventArgs e)
        {
            base.RegisterCommonJs("SiteNavigation.js");

            base.OnPreRender(e);
        }

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                ShowNavigationDetail(base.GetCurrentSPSite().RootWeb, writer);
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }

        private void ShowNavigationDetail(SPWeb spWeb, System.Web.UI.HtmlTextWriter writer)
        {
            if (IsShowHome)
            {
                //添加主页连接
                CreateHome(writer);
            }
            //显示顶级站点quickLaunch
            SPNavigationNodeCollection rootquickLaunchList = spWeb.Navigation.QuickLaunch;
            if ((rootquickLaunchList != null) && (rootquickLaunchList.Count > 0))
            {
                foreach (SPNavigationNode node in rootquickLaunchList)
                {
                    foreach (SPNavigationNode node2 in node.Children)
                    {
                        CreateLevelOne(node2,writer,"");
                    }
                }
            }


            bool showSubWeb = true;

            if (spWeb.AllProperties.Contains("__IncludeSubSitesInNavigation"))
                showSubWeb = Convert.ToBoolean(spWeb.AllProperties["__IncludeSubSitesInNavigation"]);

            //显示非顶级站点quickLaunch
            if (showSubWeb)             
                ShowSubwebs(spWeb,writer);
        
        }

        private void ShowSubwebs(SPWeb spWeb, System.Web.UI.HtmlTextWriter writer)
        {
            //显示非顶级站点quickLaunch
            SPNavigationNodeCollection nodes = spWeb.Navigation.UseShared ? null : spWeb.Navigation.TopNavigationBar;

            int temp = 1;
            if ((nodes != null) && (nodes.Count > 0))
            {
                foreach (SPNavigationNode childNode in nodes)
                {
                    string divId = this.ClientID.Replace("_","").ToString() + temp;//第二级目录div对应id

                    if (childNode.Title != "Home")
                        CreateLevelOne(childNode, writer, divId);
                    else
                        continue;
                    //QuickLaunch
                    if (System.Web.HttpContext.Current.Request.Cookies[divId] != null && System.Web.HttpContext.Current.Request.Cookies[divId].Value == divId)
                        writer.WriteLine("<div id='" + divId + "' style='display:block;'>");
                    else
                        writer.WriteLine("<div id='" + divId + "' style='display:none;'>");

                    foreach (SPWeb web in spWeb.GetSubwebsForCurrentUser())
                    {
                        if (web.Url.EndsWith(childNode.Url) && web.Title == childNode.Title)
                        {
                            SPNavigationNodeCollection quickLaunchList = web.Navigation.QuickLaunch;
                            if ((quickLaunchList != null) && (quickLaunchList.Count > 0))
                            {
                               

                                writer.WriteLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' class='nav-sub-table'>");
                                foreach (SPNavigationNode node in quickLaunchList)
                                {
                                    foreach (SPNavigationNode node2 in node.Children)
                                    {
                                        writer.WriteLine("<tr><td width='20%'>&nbsp;</td>");
                                        writer.WriteLine("<td width='10%' class='nav-sub-center'>&nbsp;</td>");
                                        writer.WriteLine("<td align='left' NOWRAP>&nbsp;<a class='nav-sub-right' href=\"" + node2.Url + "\">" + node2.Title + "</a></td>");
                                        writer.WriteLine("</tr>");
                                    }
                                }
                                writer.WriteLine("</table>");
                            }
                        }
                        web.Dispose();
                    }

                    temp++;

                    writer.WriteLine("</div>");

                }
            }
        }

        private void CreateLevelOne(SPNavigationNode node, System.Web.UI.HtmlTextWriter writer,string divId)
        {
            writer.WriteLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            writer.WriteLine("<tr class='nav-group-tr'><td>");
            writer.WriteLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            if (divId != "")
                writer.WriteLine("<tr onclick=\"showChildren('" + divId + "');\" style='cursor:hand;'><td width='10%' class='nav-group-left'>&nbsp;</td>");
            else
                writer.WriteLine("<tr><td width='10%' class='nav-group-left'>&nbsp;</td>");
            writer.WriteLine("<td align='left' NOWRAP >&nbsp;<a class='nav-group-center' href=\"" + node.Url + "\"><strong>" + node.Title + "</strong></a>&nbsp;</td>");
            if (divId != "")
                writer.WriteLine("<td width='10%' class='nav-group-right'>&nbsp;</td></tr>");
            else
                writer.WriteLine("<td width='10%'>&nbsp;</td></tr>");
            writer.WriteLine("<tr><td colspan='3' class='nav-sep'></td></tr>");
            writer.WriteLine("</table>");
            writer.WriteLine("</td></tr>");
            writer.WriteLine("</table>");
        }

        private void CreateHome(System.Web.UI.HtmlTextWriter writer)
        {
            SPSite spSite = base.GetCurrentSPSite();
            writer.WriteLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            writer.WriteLine("<tr class='nav-group-tr'><td>");
            writer.WriteLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            writer.WriteLine("<tr><td width='10%' class='nav-group-left'>&nbsp;</td>");
            writer.WriteLine("<td align='left' NOWRAP>&nbsp;<a class='nav-group-center' href=\""+spSite.Url+"\"><strong>" + this.HomeName + "</strong></a>&nbsp;</td>");
            writer.WriteLine("<td width='10%'>&nbsp;</td></tr>");
            writer.WriteLine("<tr><td colspan='3' class='nav-sep'></td></tr>");
            writer.WriteLine("</table>");
            writer.WriteLine("</td></tr>");
            writer.WriteLine("</table>");
        }

    }
}
