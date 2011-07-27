
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls; 
 
namespace CA.SharePoint
{
    /// <summary>
    /// 文挡库添加树导航的webpart
    /// </summary>
    [Guid("D55A1423-B38E-4c2e-9303-C234D7B7453E")]
    public class ListTreeViewWebPart : ListViewWebPart, ICallbackEventHandler
    {
                

        private bool _EnableCallback = false  ;
        [WebBrowsable]
        [Personalizable(PersonalizationScope.Shared)]
        [ResWebDisplayName("ListTreeViewWebPart_EnableCallback")]
        public bool EnableCallback
        {
            get { return _EnableCallback; }
            set { _EnableCallback = value; }
        }

        private string _TreeNavigatorWith = "20%";
        [WebBrowsable]
        [Personalizable(PersonalizationScope.Shared)]
        [ResWebDisplayName("ListTreeViewWebPart_TreeNavigatorWith")]
        public string TreeNavigatorWith
        {
            get { return _TreeNavigatorWith; }
            set { _TreeNavigatorWith = value; }
        }

        private bool _ShowLines = true;
        [WebBrowsable]
        [Personalizable(PersonalizationScope.Shared)]
        [ResWebDisplayName("ListTreeViewWebPart_ShowLines")]
        public bool ShowLines
        {
            get { return _ShowLines; }
            set { _ShowLines = value; }
        }

        private int _ExpandDepth = 0 ;
        [WebBrowsable]
        [Personalizable(PersonalizationScope.Shared)]
        [ResWebDisplayName("ListTreeViewWebPart_ExpandDepth","Expand Depth")]
        public int ExpandDepth
        {
            get { return _ExpandDepth; }
            set { _ExpandDepth = value; }
        }

        private string _FolderSortField;
        [WebBrowsable]
        [Personalizable(PersonalizationScope.Shared)]
        [ResWebDisplayName("ListTreeViewWebPart_ExpandDepth", "Folder Sort Field")]
        public string FolderSortField
        {
            get { return _FolderSortField; }
            set { _FolderSortField = value; }
        }

        /// <summary>
        /// 无法控制，故隐藏
        /// </summary>
        //[WebBrowsable(false)]
        //public override bool ShowViewList
        //{
        //    get
        //    {
        //        return base.ShowViewList;
        //    }
        //    set
        //    {
        //        base.ShowViewList = value;
        //    }
        //}

        protected TreeView NavigationTree;
        
        protected override void CreateChildControls()
        {
            //if (this.ChildControlsCreated) return;        

            if (this.Page.IsCallback) return;

            if (List == null)
            {
                base.RegisterShowToolPanelControl("Please", "open Tools panel", "， configure “List Name”。");
                return;
            }

            this.Controls.Clear(); 

            if (this.ShowToolbar && this.ToolbarControl != null)
                this.Controls.Add(this.ToolbarControl);

            base.AddHtml("<table width='100%' class='CA-TreeListViewWebPart-table' cellspacing='0' cellpadding='0'><tr><td width='" + this.TreeNavigatorWith + "' valign='top'>");

            if (!Page.IsCallback)
                this.CreateTreeNodes();

            base.AddHtml("</td><td  valign='top' id='" + this.ClientID + "__FolderContent'>");

            bool htmlCreated = CreateContentHtml(false); //始终不创建工具栏

            if (!htmlCreated)
                CreateContentList(false);   //始终不创建工具栏

             base.AddHtml("</td></table>");       
        }



        void CreateTreeNodes()
        {
            NavigationTree = new TreeView();
            NavigationTree.ID = "NavigationTree";
            NavigationTree.ShowLines = this.ShowLines;
            NavigationTree.ExpandDepth = _ExpandDepth;

            try
            {
                SPList list = base.GetCurrentSPList();

                if (list == null) return;

                SPView view = base.CurrentView;

                SPFolder rootFolder = list.RootFolder;

                string pageUrl = Page.Request.RawUrl;

                if (pageUrl.IndexOf("?") != -1)
                    pageUrl = pageUrl.Split('?')[0];

                pageUrl += "?RootFolder=";

                string currentUrl = Page.Request.QueryString["RootFolder"];

                string webUrl = base.GetCurrentSPWeb().ServerRelativeUrl;

                if (!webUrl.EndsWith("/"))
                    webUrl += "/";

                //root
                TreeNode rootNode = new TreeNode();
                rootNode.ImageUrl = "/_layouts/images/folder.gif";
                rootNode.Expand();
                rootNode.Text = list.Title;
                rootNode.NavigateUrl = "";
                NavigationTree.Nodes.Add(rootNode);

                if (EnableCallback)
                {
                    string serverCallJs = Page.ClientScript.GetCallbackEventReference(this, "args", "TreeListViewWebPart_Callback", "context", "TreeListViewWebPart_Callback", true);

                    pageUrl = getCurrentPageName(pageUrl);

                    string clientBackJs = "function TreeListViewWebPart_Callback(rvalue, context){document.getElementById('" + this.ClientID + "__FolderContent').innerHTML=rvalue;}\n";

                    //encodeURIComponent是moss带的js函数
                    string serverCallJsWrap = "function TreeListViewWebPart_LoadFolderContent( page , folder ){ theForm.action= page + encodeURIComponent(folder);" +
                        "var args = '';var context='';document.getElementById('" + this.ClientID + "__FolderContent').innerHTML='Loading...';" +
                        serverCallJs + " ;}\n";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", serverCallJsWrap + clientBackJs, true);

                    rootNode.NavigateUrl = String.Format("javascript:TreeListViewWebPart_LoadFolderContent('{0}','{1}')", pageUrl, (webUrl + rootFolder.Url));

                    buildAsyncSub(pageUrl, webUrl, rootFolder, rootNode.ChildNodes);
                }
                else
                {
                    rootNode.NavigateUrl = pageUrl + Page.Server.UrlEncode(webUrl + rootFolder.Url);

                    this.buildSub(pageUrl, webUrl, currentUrl, rootFolder, rootNode.ChildNodes);
                }

                this.Controls.Add(NavigationTree);

            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }

        private string getCurrentPageName(string url)
        {
            string[] temp = url.Split('/');
            return temp[temp.Length-1];
        }

        IList<SPFolder> GetSortedSubFolders(SPFolder  folder)
        {
            //List<SPFolder> sortedFolders = new List<SPFolder>();

            //foreach (SPFolder f in folders)
            //{
            //    sortedFolders.Add(f);
            //}

            //sortedFolders.Sort(CompareFolder);

            //return sortedFolders;

            if (String.IsNullOrEmpty(this.FolderSortField))
                return SharePointUtil.GetSortedFolders(folder.SubFolders);
            else
                return SharePointUtil.GetSortedFolders(folder.SubFolders, this.FolderSortField);
        }

        //int CompareFolder(SPFolder f1, SPFolder f2)
        //{
        //    return f1.Name.CompareTo(f2.Name);
        //}
       
        //创建文件夹树
        void buildSub(string pageUrl, string webUrl , string currentFolderUrl ,  SPFolder root, TreeNodeCollection nodes)
        {
            if (root.SubFolders.Count == 0) return;

            IList<SPFolder> sortedFolders = this.GetSortedSubFolders(root);

            foreach (SPFolder f in sortedFolders )
            {
                //if (  f.Name.ToLower() == "forms") continue;

                if (IsHiddenFolder(f)) continue;

                string folderUrl = webUrl + f.Url;
                
                TreeNode n = new TreeNode();

               // string url = webUrl + pageUrl + "?RootFolder=" + folderUrl ;// + Page.Server.UrlEncode(folderUrl);

               // n.NavigateUrl = "javascript:SubmitFormPost( '"+pageUrl+"' + encodeURIComponent('" + f.Url + "') );"; //EnterFolder and SubmitFormPost是系统js
                
                n.NavigateUrl = pageUrl + Page.Server.UrlEncode(folderUrl);                               
                
                //n.Expanded = Expanded ;
                n.ImageUrl = "/_layouts/images/folder.gif";
                nodes.Add(n);

                if (String.Compare(currentFolderUrl, folderUrl, true) == 0) //展开所有父
                {
                    n.Expand();
                    TreeNode temp = n.Parent;
                    while (temp != null)
                    {
                        temp.Expand();
                        temp = temp.Parent;
                    }
                    n.Select();
                    //n.Text = "<b>" + f.Name + "</b>(" + f.Files.Count + ")";

                    n.Text = "<b>" + f.Name + "</b>";
                }
                else
                {
                    //n.Collapse();
                    n.Text = f.Name ;
                }

                //n.Text = "<a href='"+url+"' onclick=\"javascript:EnterFolder('" + url + "');return false;\">" + n.Text + "<a>";

                buildSub(pageUrl, webUrl , currentFolderUrl , f, n.ChildNodes);
            }
        }

        //string js = @"function ";Page.Server.UrlEncode

        void buildAsyncSub(string pageUrl, string webUrl, SPFolder root, TreeNodeCollection nodes )
        {
            foreach (SPFolder f in root.SubFolders)
            {
                //if (f.Name.ToLower() == "forms") continue;
                if (IsHiddenFolder(f)) continue;

                TreeNode n = new TreeNode();
                n.Text = f.Name ; //  +"(" + f.Files.Count + ")";
                n.ImageUrl = "/_layouts/images/folder.gif";
                n.Collapse();
                n.NavigateUrl = String.Format("javascript:TreeListViewWebPart_LoadFolderContent('{0}','{1}')", pageUrl ,(webUrl + f.Url));

                nodes.Add(n);

                buildAsyncSub(pageUrl, webUrl, f, n.ChildNodes);
            }
        }

        void RenderChildControls(Control ctl, HtmlTextWriter w , int dept )
        {
            if (ctl == null) return;

            foreach (Control c in ctl.Controls)
            {
                if (c == null) continue;

                for (int i = 0; i < dept; i++)
                {
                    w.Write("&nbsp;&nbsp;");
                }

                if (c is RepeatedControls)
                {
                    RepeatedControls rc = c as RepeatedControls;

                    //RenderChildControls(rc.TemplateControl,w,dept + 1);
                }

                w.Write(c.GetType().FullName);
                w.Write("<hr>");

                RenderChildControls( c , w , dept + 1 );
            }
        }

        //protected override void RenderContents(HtmlTextWriter writer)
        //{
        //    base.RenderErrors(writer);

        //    //ExtandViewToolBar tb = base.ToolbarControl as ExtandViewToolBar;           

        //    if (List == null)
        //    {
        //        base.RenderContents(writer);
        //        return;
        //    }

        //    base.RenderToolbar(writer);

        //    writer.Write("<table width='100%' class='CA-TreeListViewWebPart-table' cellspacing='0' cellpadding='0'><tr><td width='" + TreeNavigatorWith + "' valign='top'>");            

        //    try
        //    {
        //        if (NavigationTree != null)
        //            NavigationTree.RenderControl(writer);
              
        //        //foreach (Control ctl in this.Controls)
        //        //    ctl.RenderControl(writer);

        //        //this.RenderChildren(writer);

        //    }
        //    catch { }

        //    writer.Write("</td><td  valign='top' id='"+ this.ClientID +"__FolderContent'>");

        //    if (base.ContentHtml != null)
        //        base.ContentHtml.RenderControl(writer);

        //    if (base.ContentList != null)
        //        base.ContentList.RenderControl(writer);
           
        //    //base.RenderBySkin(writer);

        //    writer.Write("</td></table>");           
        //}

         


        #region ICallbackEventHandler 成员

        public string GetCallbackResult()
        {
            //SPList list = base.GetCurrentSPList();

           // SPList list = base.GetCurrentSPWeb().GetListFromUrl("http://saicvt04:9032/inside/DocLib2/Forms/AllItems.aspx?RootFolder=%2finside%2fDocLib2%2fbbb&FolderCTID=&View=%7bE1FDBB28%2d932E%2d44A5%2d9268%2dAE68C0E7347A%7d");

            SPView view = base.CurrentView ;

          //  SPWeb web = base.GetCurrentSPWeb();
           // web.GetFolder("").;

            //string currentUrl = Page.Request.QueryString["RootFolder"];

            //SPFolder f = web.GetFolder( currentUrl  );
            
           // System.Text.Encoding.Convert

            //throw new Exception("The method or operation is not implemented.");
            return view.RenderAsHtml() ;
        }

        private string _eventArgument;
        public void RaiseCallbackEvent(string eventArgument)
        {
            //throw new Exception("The method or operation is not implemented.");
            _eventArgument = eventArgument;
        }

        #endregion
    }
}
