
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
//using Microsoft.SharePoint.Publishing;
using System.Reflection;
using CA.SharePoint.WebPartSkin;
using CA.SharePoint.EditParts;

namespace CA.SharePoint
{

    class ExtandViewToolBar : ViewToolBar
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            ToolBar toolbarControl = (ToolBar)this.TemplateContainer.FindControl("toolBarTbl");
            if (toolbarControl != null)
            {

            }
        }

        public ToolBar InnerToolBar
        {
            get
            {
                this.EnsureChildControls();

                ToolBar toolbarControl = (ToolBar)this.TemplateContainer.FindControl("toolBarTbl");

                return toolbarControl;
            }
        }
    }

     /// <summary>
    /// 具有skin加载能力的webpart基类
    /// </summary>
    public class ListViewWebPart : BaseSPListWebPart
    {

        private IQueryConditionProvider _QueryConditionProvider;

        [ResConnectionConsumer("QueryCondition", "QueryConditionConsumer",
           AllowsMultipleConnections = true)]
        public virtual void ConsumeQueryCondition(IQueryConditionProvider provider)
        {
            _QueryConditionProvider = provider;

            SetQuery();
        }


        private bool _ShowToolbar = false ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListViewWebPart_ShowToolbar")]
        public bool ShowToolbar
        {
            get { return _ShowToolbar; }
            set { _ShowToolbar = value; }
        }

        private string _ToolbarTemplateName;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Toolbor Template Name")]
        public virtual string ToolbarTemplateName
        {
            get { return _ToolbarTemplateName; }
            set { _ToolbarTemplateName = value; }
        }

        private int _RowLimit = 0;
        /// <summary>
        /// 最多显示行数
        /// </summary>
        /// 
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListViewWebPart_RowLimit")]
        public int RowLimit
        {
            set { _RowLimit = value; }
            get { return _RowLimit; }
        }

        private bool _ShowViewList = false ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListViewWebPart_ShowViewList","Show View List")]
        public  virtual bool ShowViewList
        {
            get { return _ShowViewList; }
            set { _ShowViewList = value; }
        }

        /// <summary>
        /// 是否使用已经存在的视图，而不是新建单独的视图
        /// </summary>
        private bool _UseExistView = true;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        public bool UseExistView
        {
            get {  return _UseExistView; }
            set { _UseExistView = value; }
        }

        private Guid _RelationViewID;
        /// <summary>
        /// 关联的视图--有本控件创建
        /// </summary>
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        public Guid RelationViewID
        {
            get { return _RelationViewID; }
            set { _RelationViewID = value; }
        }

        private string _RootFolder;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        public string RootFolder
        {
            get { return _RootFolder; }
            set { _RootFolder = value; }
        }

        // private bool _AutoGenerateEmptyRows = true;
        /// <summary>
        /// 当实际行数不足时（行数小于MaxRowCount）,是否自动生成空行
        /// </summary>
        /// 
        //[Localizable(true)]
        //[Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable]
        //[ResWebDisplayName("ListViewWebPart_AutoGenerateEmptyRows")]
        //public bool AutoGenerateEmptyRows
        //{
        //    set { _AutoGenerateEmptyRows = value; }
        //    get { return _AutoGenerateEmptyRows; }
        //}        

        protected string GetBodySkinKey()
        {
            return this.TemplateID + "_body";
        }               

        private ViewToolBar _toolbar;
        public ViewToolBar ToolbarControl
        {
            get
            { 
                if (this._toolbar != null)
                {
                    return this._toolbar;
                }

                SPList list = this.GetCurrentSPList();

                if (list == null) return null;                     

                 SPView view = this.CurrentView;

                 if (view == null) return null;
                
                //SPWeb web = this.GetCurrentSPWeb();
                //(HttpContext context, Guid viewId, Guid listId, SPWeb web);
                SPContext context = SPContext.GetContext( base.Context , view.ID , list.ID , list.ParentWeb )  ;
                //context.ViewContext.View = view;
                //context.ViewContext.ViewType = this.ViewType;
                //context.ViewContext.Qualifier = base.Qualifier;

                this._toolbar = new ExtandViewToolBar();
                this._toolbar.ID = this.ID + "_ExtandViewToolBar1";
                this._toolbar.RenderContext = context;
                this._toolbar.EnableViewState = false;

                if (!String.IsNullOrEmpty(ToolbarTemplateName))
                {
                    this._toolbar.TemplateName = this.ToolbarTemplateName;
                }
                else if (!String.IsNullOrEmpty(view.ToolbarTemplateName))
                {
                    this._toolbar.TemplateName = view.ToolbarTemplateName;
                }
                     
                return this._toolbar;
            }
        }          
 
        /// <summary>
        /// 删除视图schemal的Toolbar节点
        /// </summary>
        /// <param name="doc"></param>
        void HiddenToolbar(XmlDocument doc)
        {
            XmlNode toolNode = doc.DocumentElement.SelectSingleNode("Toolbar");

             if (toolNode != null)
                doc.DocumentElement.RemoveChild(toolNode);
        }
        /// <summary>
        /// 修改视图schemal 的RowLimit节点
        /// </summary>
        /// <param name="doc"></param>
        void ChangeRowLimit(XmlDocument doc)
        {
            XmlNode rowLimitNode = doc.DocumentElement.SelectSingleNode("RowLimit");

            if (rowLimitNode != null) 
            {
                rowLimitNode.Attributes.RemoveAll();
                rowLimitNode.InnerText = this.RowLimit.ToString();
            }
        }
        
        void ProcessToolbarSchema(XmlDocument doc )
        {
            XmlNode toolNode = doc.DocumentElement.SelectSingleNode("Toolbar");

            if (this.ShowToolbar)
            {
                if (toolNode == null)
                {
                    toolNode = doc.CreateElement("Toolbar");
                    XmlAttribute att = doc.CreateAttribute("Type");
                    att.Value = "Standard";
                    toolNode.Attributes.Append(att);
                    doc.DocumentElement.AppendChild(toolNode);
                }
                else
                {
                    toolNode.InnerXml = "";
                    toolNode.Attributes["Type"].Value = "Standard" ;
                }
            }
            else
            {
                if (toolNode != null)
                    doc.DocumentElement.RemoveChild(toolNode);
            }
        }

        protected Microsoft.SharePoint.WebPartPages.ListViewWebPart ContentList = null ;
        protected LiteralControl ContentHtml;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.EnsureChildControls();
        }

        private XmlDocument _ListViewXml;
        /// <summary>
        /// 设置列表的查询条件
        /// </summary>
        void SetQuery()
        {
            this.EnsureChildControls();
            if (ContentList != null)
            {
                //ProcessToolbarSchema(doc);

                if (this._QueryConditionProvider != null && !String.IsNullOrEmpty(this._QueryConditionProvider.QueryCondition.Query))
                {
                    SharePointUtil.ChangeSchemaXmlQuery(_ListViewXml, this._QueryConditionProvider.QueryCondition.Query , this.StaticQuery);
                }

                ContentList.ListViewXml = _ListViewXml.InnerXml ;
            }
        }

        protected virtual string StaticQuery
        {
            get
            {
                return this.CurrentView.Query;
            }
        }

        public void ReCreateChildControls()
        {
            CreateChildControls();
        }

        /// <summary>
        /// 内容控件是否被创建
        /// </summary>
        public bool ContentCreated = false;

        protected override void CreateChildControls()
        {
            //if (this.ChildControlsCreated) return;        

            this.Controls.Clear();

            base.CreateChildControls();            

            if (List == null)
            {
                base.RegisterShowToolPanelControl("Please", "open Tools panel", "， configure “List Name”。");
                return;
            }

            bool htmlCreated = CreateContentHtml(true);//首先按照body皮肤生成列表内容

            if (!htmlCreated)
                CreateContentList(true);  //若皮肤不存在，则创建默认的列表webpart
        }

        protected virtual bool CreateContentHtml( bool createToolbar )
        {
            if (!String.IsNullOrEmpty(this.TemplateID)) //指定皮肤 
            {             
                SkinElement el = GetSkin();

                if (el != null && !String.IsNullOrEmpty(el.Body))
                {
                    if ( createToolbar && this.ShowToolbar && this.ToolbarControl != null)
                    {
                        this.Controls.Add(this.ToolbarControl);
                    }

                    ContentHtml = new LiteralControl();
                    ContentHtml.ID = this.ID + "_ContentHtml";
                    ContentHtml.Text = this.GetContentHtml(el.Body);

                    this.Controls.Add(ContentHtml);
                    this.ContentCreated = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 创建默认的ListViewWebPart
        /// </summary>
        /// <param name="createToolbar"></param>
        protected virtual void CreateContentList(bool createToolbar)
        {
            SPView view = this.CurrentView;

            if (view == null) return;

            //if (!String.IsNullOrEmpty(ToolbarTemplateName))
            //{
            //    view.ToolbarTemplateName = this.ToolbarTemplateName;
            //}

            ContentList = new Microsoft.SharePoint.WebPartPages.ListViewWebPart();
            ContentList.ID = this.ID + "ContentList";
            ContentList.Title = this.Title;
            ContentList.ListName = List.ID.ToString("B").ToUpper();

            if( !String.IsNullOrEmpty((base.WebName)))
                ContentList.WebId = base.Web.ID;


            if ( ShowViewList && createToolbar )
                ContentList.ViewGuid = view.ID.ToString("B").ToUpper();
            //给ViewGuid赋值时，会显示出视图选择菜单

            _ListViewXml = new XmlDocument();
            _ListViewXml.LoadXml(view.HtmlSchemaXml);

            if (this.ShowToolbar == false || createToolbar == false)
                this.HiddenToolbar(_ListViewXml);

            if (this.RowLimit > 0)
            {
                ChangeRowLimit(_ListViewXml);
            }

            ContentList.ListViewXml = _ListViewXml.InnerXml;

            ContentList.EnableViewState = true;

            //ContentList.FrameType = Microsoft.SharePoint.WebPartPages.FrameType.None;

            ContentList.ChromeType = PartChromeType.None; //不显示标题

            if (!String.IsNullOrEmpty(ToolbarTemplateName))
            {
                foreach (Control ctl in ContentList.Controls)
                {
                    if (ctl is ViewToolBar)
                    {
                        ViewToolBar bar = (ViewToolBar)ctl;
                        bar.TemplateName = this.ToolbarTemplateName;
                        //bar.AlternateTemplateName = this.ToolbarTemplateName;
                        break;
                    }
                }
            }

            this.Controls.Add(ContentList);

            this.ContentCreated = true;
        }

        /// <summary>
        /// 输出工具栏
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderToolbar(HtmlTextWriter writer)
        {
            if (this.ShowToolbar && this.ToolbarControl != null)
                this.ToolbarControl.RenderControl(writer);
        }        
        /// <summary>
        /// 按照皮肤模板生成html
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>       
        private string GetContentHtml(string skin)
        {
            StringBuilder html = new StringBuilder();

            try
            {

                SPList list = base.GetCurrentSPList();

                if (list == null)
                {
                    return "";
                }

                string skinKey = GetBodySkinKey();

                SPListItemCollection items = null;                 

                SPView view = base.CurrentView ;

                SPQuery q = new SPQuery(view);

                if (this._QueryConditionProvider != null)
                {
                    q.Query = this._QueryConditionProvider.QueryCondition.Query;
                }

                if (RowLimit > 0)
                    q.RowLimit = (uint)this.RowLimit;

                q.ViewFields = "";

                if (!String.IsNullOrEmpty(_RootFolder))
                {
                    SPFolder folder = base.Web.GetFolder(_RootFolder);
                    if (folder != null)
                        q.Folder = folder;
                }
                else
                {

                    string rootFolder = Page.Request.QueryString["RootFolder"];

                    if (rootFolder != null && rootFolder != "")
                    {
                        SPFolder folder = base.Web.GetFolder(rootFolder);
                        if (folder != null)
                            q.Folder = folder;
                    }

                }

                items = list.GetItems(q);

                int rowNumber = 0;

                IList<ReplaceTag> tags = ReplaceTagManager.GetInstance().GetReplaceTags(skinKey, skin);

                ITagValueProvider listValueProvider = new SPListValueProvider(list);
                ITagValueProvider wpValueProvider = new WebPartValueProvider(this);

                foreach (SPListItem item in items)
                {
                    StringBuilder sb = new StringBuilder(skin);

                    rowNumber++;

                    string v;

                    ITagValueProvider fieldValueProvider = new SPListItemValueProvider(item, rowNumber);

                    foreach (ReplaceTag tag in tags)
                    {
                        if (tag.ValueProvider == "spfield")
                            sb.Replace(tag.TagValue, fieldValueProvider.GetValue(tag));
                        else if (tag.ValueProvider == "splist")
                            sb.Replace(tag.TagValue, listValueProvider.GetValue(tag));
                        else if (tag.ValueProvider == "webpart")
                            sb.Replace(tag.TagValue, wpValueProvider.GetValue(tag));
                    }

                    html.Append(sb.ToString());
                }

                return html.ToString();
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);

                return "" ;
            }
        } 

        public override EditorPartCollection CreateEditorParts()
        {
            ArrayList editorArray = new ArrayList();

            ViewEditorPart edPart = new ViewEditorPart();
            edPart.ID = "ViewEditorPart1";
            editorArray.Add(edPart);  

            EditorPartCollection initEditorParts = base.CreateEditorParts();

            EditorPartCollection editorParts = new EditorPartCollection(initEditorParts, editorArray);

            return editorParts;
        }

    }
}
