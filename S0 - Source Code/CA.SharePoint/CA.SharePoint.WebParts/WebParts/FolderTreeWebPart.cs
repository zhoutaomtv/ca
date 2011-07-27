
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
 
namespace CA.SharePoint
{
    /// <summary>
    /// 文挡库添加树导航的webpart
    /// </summary>     
    public class FolderTreeWebPart : BaseSPListWebPart  
    {

        //private string _ListUrl = "http://jyserver:9000/000/Forms/AllItems.aspx";
        //[WebBrowsable]
        //[Personalizable( PersonalizationScope.Shared)]
        //[DisplayName("列表Url")]
        //public string ListUrl
        //{
        //    get { return _ListUrl; }
        //    set { _ListUrl = value; }
        //}

                

        private bool _ShowLines = true;
        [WebBrowsable]
        [Personalizable(PersonalizationScope.Shared)]
        [WebDisplayName("TreeNode Show Lines")]
        public bool ShowLines
        {
            get { return _ShowLines; }
            set { _ShowLines = value; }
        }

       
        private TreeView _Tree;
        protected override void CreateChildControls()
        {
            if (this.ChildControlsCreated) return;

            base.CreateChildControls();

            if (Page.IsCallback) return;

            _Tree = new TreeView();
            _Tree.ShowLines = this.ShowLines;      

            try
            {
                SPList list = base.GetCurrentSPList();

                SPFolder f = list.RootFolder;                         

                string pageUrl = Page.Request.RawUrl;

                if (pageUrl.IndexOf("?") != -1)                    
                    pageUrl = pageUrl.Split('?')[0];              

                pageUrl += "?RootFolder=";

                string currentUrl = Page.Request.QueryString["RootFolder"];

                string webUrl = base.GetCurrentSPWeb().ServerRelativeUrl;

                if (!webUrl.EndsWith("/"))
                    webUrl += "/";
                 
                this.buildSub(pageUrl, webUrl, f, _Tree.Nodes);               

                this.Controls.Add(_Tree);

                this.ChildControlsCreated = true;
                
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

       
        //创建文件夹树
        void buildSub(string pageUrl, string webUrl ,  SPFolder root, TreeNodeCollection nodes)
        {
            foreach (SPFolder f in root.SubFolders)
            {
                //if (  f.Name.ToLower() == "forms") continue;

                if (IsHiddenFolder(f)) continue;

                TreeNode n = new TreeNode();
                n.ImageUrl = "/_layouts/images/folder.gif";
                n.Text = f.Name;

                n.NavigateUrl = pageUrl + Page.Server.UrlEncode(webUrl + f.Url);               

                nodes.Add(n);

                buildSub(pageUrl, webUrl , f, n.ChildNodes);
            }
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            this.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            this.Style.Add(HtmlTextWriterStyle.Cursor, "hand");

            this.Attributes.Add("onmouseout", "WebPart_hiddenContent('" + this.ClientID + "')");

            writer.Write("<a onmouseover=\"WebPart_showContent('"+this.ClientID+"')\">" + this.Title + "</a>");

            base.RenderContents(writer);
        }

        string js = @"function WebPart_showContent(contentObjId )
            {
                var contentObj = document.getElementById( contentObjId );
                
                var x = document.body.scrollLeft + event.clientX;
                var y = document.body.scrollTop + event.clientY;
                
                contentObj.style.pixelLeft = x - 50 ;
                contentObj.style.pixelTop =  y ;
                contentObj.style.display='';         
            }
            
            function WebPart_hiddenContent(contentObjId )
            {
                var contentObj = document.getElementById( contentObjId );

                contentObj.style.display='none';         
            }";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "WebPart_Show_Hidden", js, true);
        }
     
    }
}
 //style="position:absolute;z-index:1000; border:2px; background-color:Red; height:200px; width:200px; left: 0px; top: 0px; display:none" >