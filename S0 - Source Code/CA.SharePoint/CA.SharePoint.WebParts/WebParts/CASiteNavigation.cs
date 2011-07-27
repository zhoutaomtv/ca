using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Navigation;
using System.ComponentModel;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;

namespace CA.SharePoint
{
    public class CASiteNavigation : BaseSPWebPart
    {
        private const string script = @"if($('#{0}')[0].style.display=='none'){$('#{0}').show('fast');}else{$('#{0}').hide('fast');}";

        private const string ULIDTmp = "CANavigationUL_";

        private bool _IsShowTopLink = false;

        [
         Personalizable(PersonalizationScope.Shared),
         WebBrowsable,
         WebDisplayName("Is need to Show Top Link")
        ]
        public bool IsShowTopLink
        {
            get { return _IsShowTopLink; }
            set { _IsShowTopLink = value; }
        }

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                ShowNavigationDetail(base.GetCurrentSPSite().OpenWeb(), writer);

                if (SPContext.Current.List != null)
                {
                    string listName = SPContext.Current.List.Title;
                    string listUrl = SPContext.Current.List.DefaultViewUrl;

                    base.Script.ExcedJs(string.Format(@"$('ul:has(li a[href*={0}])').show('fast');$('li a[href*={0}]')[0].style.color='#35beff';", listUrl));
                }
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }


        private void ShowNavigationDetail(SPWeb spWeb, System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteLine(@"<div class='CANavigationDiv'>");
            //显示顶级站点quickLaunch
            SPNavigationNodeCollection rootquickLaunchList = spWeb.Navigation.QuickLaunch;
            if ((rootquickLaunchList != null) && (rootquickLaunchList.Count > 0))
            {
                int nIndex = 0;
                foreach (SPNavigationNode node in rootquickLaunchList)
                {
                    nIndex++;

                    if (node.Children.Count == 0)
                    {
                        continue;
                    }

                    //<h2>Pictures</h2>
                    //<div class="bg"></div>
                    //<ul>
                    if (IsShowTopLink)
                    {
                        writer.WriteLine(string.Format("<h2 onclick=\"{2}\"><a href='{0}'>{1}</a></h2>", node.Url, node.Title, GetClickScript(nIndex)));
                    }
                    else
                    {
                        //"if($('#{0}')[0].style.display=='none'){$('#ultmp').show('fast');}else{$('#ultmp').hide('fast');}"
                        writer.WriteLine(string.Format("<h2 onclick=\"{1}\">{0}</h2>", node.Title, GetClickScript(nIndex)));
                    }
                    writer.WriteLine(@"<div class='CANavigationTitleFooter'></div>");
                    if (node.Title.ToLower() != "workflows")
                    {
                        
                            writer.WriteLine(string.Format(@"<ul style='display:none;' id='CANavigationUL_{0}'>", nIndex.ToString()));
                        
                    }
                    else
                    {
                        writer.WriteLine(string.Format(@"<ul style='display:block;' id='CANavigationUL_{0}'>", nIndex.ToString()));
                    }

                    foreach (SPNavigationNode node2 in node.Children)
                    {
                        // <li><a href="#">IT图片库</a></li>
                        writer.WriteLine(string.Format(@"<li><a href='{0}'>{1}</a></li>", node2.Url, node2.Title));
                    }
                   
                        writer.WriteLine(@"</ul>");
                    
                    // </ul>
                }               
                //回收站
                //<tr><td nowrap>
                //<a id="ctl00_PlaceHolderLeftNavBar_idNavLinkRecycleBin" href="/_layouts/recyclebin.aspx"><img align='absmiddle' alt="" src="/_layouts/images/recycbin.gif" style='border-width:0px;'>&nbsp;回收站</a>
                //</td></tr>
            }
            writer.WriteLine("</div>");
            if (this.Page.Request.RawUrl.ToLower().Contains("documentcenter"))
            {
                writer.WriteLine("<div class='CANavigationDiv2'><h3><a href=\"/documentcenter/_layouts/ca/adgroupinformation.aspx\" target=\"_blank\">User Group Information</a></h3></div>");
                writer.WriteLine(string.Format("<div class='CANavigationDiv2'><h3><a href='{1}'><img align='absmiddle' alt='' src='/documentcenter/_layouts/images/recycbin.gif' style='border-width:0px;'>{0}</a></h3></div>", "RecycleBin", "/documentcenter/_layouts/recyclebin.aspx"));
            }
        }

        private string GetClickScript(int nIndex)
        {
            return "if($('#" + ULIDTmp + nIndex + "')[0].style.display=='none'){$('#" + ULIDTmp + nIndex + "').show('fast');}else{$('#" + ULIDTmp + nIndex + "').hide('fast');}";
        }

        //          <div class="sub_right">
        //          <h2>Pictures</h2>
        //          <div class="bg"></div>
        //          <ul>
        //            <li><a href="#">IT图片库</a></li>
        //            <li><a href="#">Picture Demo</a></li>
        //          </ul>
        //          <div class="bg"></div>
        //          <h2>Documents</h2>
        //          <div class="bg"></div>
        //          <ul>
        //            <li><a href="#">Shared Documents</a></li>
        //            <li><a href="#">IT</a></li>
        //            <li><a href="#">HR</a></li>
        //          </ul>
        //          <div class="bg"></div>
        //          <h2>WorkFlow</h2>
        //          <div class="bg"></div>
        //          <ul>
        //            <li><a href="#">年假申请</a></li>
        //            <li><a href="#">TASKS</a></li>
        //            <li><a href="#">员工年假表</a></li>
        //          </ul>
        //        </div>

    }
}
