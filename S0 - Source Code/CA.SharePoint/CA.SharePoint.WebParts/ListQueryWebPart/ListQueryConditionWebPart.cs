using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized; 
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;

using Microsoft.Office.Server;
using Microsoft.Office.Server.Search.Query;

using CA.Web; 
using CA.SharePoint.CamlQuery;
using CA.SharePoint.EditParts;

namespace CA.SharePoint
{
    

    /// <summary>
    /// 列表查询
    /// 按照配置生成查询条件，连接本Zone的列表
    /// </summary>
    public class ListQueryConditionWebPart : BaseSPListWebPart , IPropertyPersistenceService 
    {
        private Button _QueryBtn = new Button ();
        private Button _ExportBtn = new Button();
        private RadioButton _AndBtn = new RadioButton();
        private RadioButton _OrBtn = new RadioButton();

        private IList<IQueryControl> _KeyWorkControls = new List<IQueryControl>();


        [ResConnectionProvider("QueryCondition", "QueryConditionProvider")]
        public IQueryConditionProvider ProvideQueryCondition()
        {
            return new QueryConditionProvider(this.InternalQuery,this.ExportFields);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //初始化折叠状态
            if (Page != null && Page.IsPostBack)
            {
                string s = Page.Request.Form[this.ClientID + "_Collapsed"];
                this._Collapsed = ( s == "true" );
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnableViewState = true;
            this.EnsureChildControls();

            if (!Page.IsPostBack)
            {
                if (this.KeepConditionInSession)//页面第一次加载并且启用了会话级条件保存
                {
                    this.buildCaml();
                }
                else
                {
                    return;
                }
            }

           // string initXml = PreListViewXml;

            //if( !String.IsNullOrEmpty(initXml) )
            if (_AutoConnect && !String.IsNullOrEmpty(this.InternalQuery))
                SetCurrentListQuery(this.InternalQuery);
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterHiddenField( this.ClientID + "_Collapsed", this.Collapsed.ToString().ToLower() );

            Page.ClientScript.RegisterClientScriptBlock(typeof(ListQueryConditionWebPart), "ListQueryConditionWebPart", JS, true);

            SaveQueryControlPropertiesToSession();
             
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //输出bar begin 
            writer.Write("<table width='100%' class='ms-gb'><tr><td nowrap='true'>");

            writer.Write("<br/><img src='/_layouts/images/blank.gif' alt='' height=1 width=0>");

            if( this.Collapsed )
                writer.Write( "<img border='0' id='"+this.ClientID+"_Img' src='/_layouts/images/plus.gif' onclick=\"javascript:ListQueryConditionWebPart_ChangeDisplay('"+this.ClientID+"');return false;\"/>");
            else
                writer.Write("<img border='0' id='" + this.ClientID + "_Img' src='/_layouts/images/minus.gif' onclick=\"javascript:ListQueryConditionWebPart_ChangeDisplay('" + this.ClientID + "');return false;\"/>");

            writer.Write("&nbsp;<a href=\"javascript:\" onclick=\"javascript:ListQueryConditionWebPart_ChangeDisplay('"+this.ClientID+"');return false;\">"+this.Title+"</a> :&nbsp; " );

            writer.Write("</td></tr></table>");
            //输出bar end
 
            if( this.Collapsed )
                writer.Write( "<div id='"+this.ClientID+"_QueryPanel' style='display:none'>" );
            else
                writer.Write("<div id='" + this.ClientID + "_QueryPanel' style='display:'>");
                
            base.RenderContents(writer);
                
            writer.Write( "</div>" );

        }

        #region Gloabl JS
        const string JS = @"
function ListQueryConditionWebPart_ChangeDisplay(id){
var obj = document.getElementById(id+'_QueryPanel');
var imgObj = document.getElementById(id+'_Img');
var stateObj = document.getElementById(id+'_Collapsed');
if( obj.style.display == 'none' ){
    obj.style.display = '';
    imgObj.src='/_layouts/images/minus.gif';
    stateObj.value = 'false';
}
else {
    obj.style.display = 'none';
    imgObj.src='/_layouts/images/plus.gif';
    stateObj.value = 'true';
}
}

function ListQueryConditionWebPart_Export(webUrl,pageUrl,listId,queryDataId,template)
{
if( pageUrl == '' ) pageUrl = webUrl + '_layouts/CA/WordExport.aspx' ;


window.open( pageUrl + '?listId=' + listId + '&queryDataId=' + queryDataId + '&template=' + template ) ;
}
";
        #endregion

        protected string CurrentWebUrl
        {
            get
            {
                SPWeb web = base.GetCurrentSPWeb();
                
                string webUrl = web.Url;
                if (!webUrl.EndsWith("/"))
                    webUrl += "/";

                return webUrl;
            }
        }

        //void AddHtml(string html)
        //{
        //    this.Controls.Add( new LiteralControl( html ) );
        //}

        public override string Title
        {
            get
            {
                if (String.IsNullOrEmpty(base.Title))
                    base.Title = "Search";

                return base.Title;
            }
            set
            {
                base.Title = value;
            }
        }

        #region 公共属性

        private List<String> _QueryFields;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        public List<String>  QueryFields
        {
            get {

                if (_QueryFields == null)
                {
                    _QueryFields = new List<String>();

                    if (this.CurrentView != null)//第一次 取当前视图的字段作为查询字段
                    {
                        foreach (string f in this.CurrentView.ViewFields)
                            _QueryFields.Add(f);
                    }
                }

                return _QueryFields; 
            }
            set { _QueryFields = value; }
        }

        private List<String> _ExportFields;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        public List<String> ExportFields
        {
            get
            {

                if (_ExportFields == null)
                {
                    _ExportFields = new List<String>();

                    if (this.CurrentView != null)//第一次 取当前视图的字段作为查询字段
                    {
                        foreach (string f in this.CurrentView.ViewFields)
                            _ExportFields.Add(f);
                    }
                }

                return _ExportFields;
            }
            set { _ExportFields = value; }
        } 
        
        private bool _Collapsed = true ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListQueryConditionWebPart_Collapsed", "Collapse")]
        public bool Collapsed
        {
            get 
            {             
                return _Collapsed; 
            }
            set { _Collapsed = value; }
        }

        private bool _AutoConnect = true;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("AutoConnect", "Auto Connect List")]
        public bool AutoConnect
        {
            get { return _AutoConnect; }
            set { _AutoConnect = value; }
        }

        private int _RepeatColumns = 2;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListQueryConditionWebPart_RepeatColumns", "Display Column Count")]
        public int RepeatColumns
        {
            get { return _RepeatColumns; }
            set 
            {
                if( value > 0 )
                    _RepeatColumns = value; 
            }
        }

        private bool _EnableExport = false;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListQueryConditionWebPart_EnableExport", "Enable Export")]
        public bool EnableExport
        {
            get { return _EnableExport; }
            set { _EnableExport = value; }
        }

        private string _ExportTempalte;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListQueryConditionWebPart_ExportTempalte", "Export Template（URL）")]
        public string ExportTempalte
        {
            get { return _ExportTempalte; }
            set { _ExportTempalte = value; }
        }

        private string _ExportPage = "/_layouts/CA/ExcelExport.aspx";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ListQueryConditionWebPart_ExportPage", "Export Page（URL）")]
        public string ExportPage
        {
            get { return _ExportPage; }
            set { _ExportPage = value; }
        }

        private bool _KeepConditionInSession = false ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Keep Condition In Session")]
        public bool KeepConditionInSession
        {
            get { return _KeepConditionInSession; }
            set { _KeepConditionInSession = value; }
        }

        #endregion

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Controls.Clear();
            _KeyWorkControls.Clear();

            if (List == null)
            {
                base.RegisterShowToolPanelControl("Please", "Open Tools Panel", ",Configure “List Name” and Search Field。");
                return;
            }
            else
            {
                buildHorizontalQueryPanel();
            }

            this.ChildControlsCreated = true;
        }
        
        /// <summary>
        /// 重新创建查询控件
        /// </summary>
        internal void ReBuildQueryPanel()
        {
            this.Controls.Clear();
            _KeyWorkControls.Clear();

            buildHorizontalQueryPanel();

            this.ChildControlsCreated = true;
        }

        //生成查询输入控件-水平布局
        void buildHorizontalQueryPanel()
        {
            if (base.List == null) return;

            AddHtml("<table class='ms-formtable' style='margin-top: 4px;margin-left: 8px;' border=0 cellpadding=0 cellspacing=0 >");

            int rowFieldCount = 0;

            IPropertyPersistenceService persisSvr = null;

            if (this.KeepConditionInSession)
                persisSvr = this;            

            foreach (string fName in QueryFields )
            {
                if (!List.Fields.ContainsField(fName))
                    continue;

                SPField f = List.Fields.GetFieldByInternalName(fName);

                IQueryControl qCtl = QueryControlFactory.GetQueryControl(f,persisSvr);
                qCtl.FieldName = fName;
                _KeyWorkControls.Add(qCtl);

                Control ctl = (Control)qCtl;

                ctl.ID = fName + "_valuecontrol";

                if (rowFieldCount == 0)
                {
                    AddHtml("<tr>");
                }

                //AddHtml("<td nowrap='true' valign='top' class='ms-formlabel'><H3 class='ms-standardheader'>" + f.Title + "</H3>");
                AddHtml("<td nowrap='true' valign='top' class='ms-formlabel'>" + f.Title );

                AddHtml("</td><td valign='top' class='ms-formbody' >");
                this.Controls.Add(ctl);
                AddHtml("&nbsp;&nbsp;&nbsp;&nbsp;</td>");

                rowFieldCount++;

                if (rowFieldCount == this.RepeatColumns )
                {
                    AddHtml("</tr>");
                    rowFieldCount = 0;
                }
            }

            int maxRowCount = this.RepeatColumns  * 2 ;
            bool needNewRow = false ;

            if (rowFieldCount == 0 ) //恰好字段填充完表格列
            {
                needNewRow = true;
                rowFieldCount = 0;
            }
            else if (rowFieldCount + 2 >= maxRowCount) //字段没有填充完表格列，但剩余不足三列
            {
                needNewRow = true;
                rowFieldCount = 0;
                AddHtml("</tr>");
            }
            else //剩余列多于三列，没必要生成新行
            {
                needNewRow = false;
            }

            if( needNewRow )
                AddHtml("<tr>");
 
            AddHtml("<td colspan='" + (this.RepeatColumns * 2 - rowFieldCount * 2) + "' valign='top'>");                      

            AddHtml("&nbsp;&nbsp;");

            _QueryBtn.Text = " Search ";

            _QueryBtn.ID = "queryBtn";
            //_QueryBtn.CssClass = "ms-ButtonHeightWidth";
            _QueryBtn.Click += new EventHandler(_QueryBtn_Click);
            this.Controls.Add(_QueryBtn);

            _ExportBtn.Text = " Export ";
            _ExportBtn.ID = "exportBtn";
            _ExportBtn.Visible = EnableExport;
            _ExportBtn.Click += new EventHandler(_ExportBtn_Click);
            this.Controls.Add(_ExportBtn);

            AddHtml("</td>");

            AddHtml("<tr></table>");
        }

        void _ExportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.buildCaml();

                Guid queryDataId = Guid.Empty;

                int[] selectedIds = SharePointUtil.GetSelectedItemIDs(this.List);

                QueryCondition qp = new QueryCondition();
                qp.ItemIDs = selectedIds;

                qp.Where = this.InternalQuery;
 
                qp.ExportFields = this.ExportFields;

                SPView view = base.CurrentView;

                if (view != null)
                {
                    qp.ViewId = view.ID;
                    qp.OrderBy = SharePointUtil.GetOrderBySection(view.SchemaXml);
                }

                qp.Query = qp.Where + qp.OrderBy;

                queryDataId = TransferDataManager.Instance.StoreData(qp);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "WordExport",
                    "\nListQueryConditionWebPart_Export('" + this.CurrentWebUrl + "','" + this.ExportPage + "','" + List.ID + "','" + queryDataId + "','" + this.ExportTempalte + "');\n", true);
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }        
        }         
         
        /// <summary>
        /// 
        /// </summary>
        void buildCaml()
        {
            CAMLExpression<object> expr = null;

            for (int i = 0; i < _KeyWorkControls.Count; i++)
            {
                IQueryControl wd = _KeyWorkControls[i];

                CAMLExpression<object> tempExpr = wd.QueryExpression;

                if (tempExpr == null)
                    continue;

                if (expr == null)
                    expr = tempExpr;
                else
                {
                    //if (_AndBtn.Checked)
                        expr = expr & tempExpr;
                    //else
                    //    expr = expr | tempExpr;
                }
            }

            try
            {
                string q;
                
                if (expr == null)
                {
                    q = "";
                }
                else
                { 
                    q = CAMLBuilder.Where(expr);
                }

                this.InternalQuery = q;

                if (_AutoConnect)
                    this.SetCurrentListQuery(q);

            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
           
            }
        }
                 
        
        /// <summary>
        /// 查找当前Zone的Microsoft.SharePoint.WebPartPages.ListViewWebPart
        /// 设置查询条件
        /// </summary>
        /// <param name="qxml"></param>
        private void SetCurrentListQuery(string qxml)
        {
            //if (String.IsNullOrEmpty(qxml)) return;            

            string listId = "{" + this.List.ID.ToString() + "}";

            

            foreach (WebPart wp in this.Zone.WebParts)
            {
                if (wp is Microsoft.SharePoint.WebPartPages.ListViewWebPart)
                {
                    Microsoft.SharePoint.WebPartPages.ListViewWebPart listWp = (Microsoft.SharePoint.WebPartPages.ListViewWebPart)wp;

                    if ( String.Compare( listWp.ListName , listId , true ) != 0 ) continue ;

                    if (String.IsNullOrEmpty(qxml))
                    {
                        listWp.ListViewXml = this.List.Views[new Guid(listWp.ViewGuid)].HtmlSchemaXml; //还原                        
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(listWp.ListViewXml); 

                       // SPUtil.ChangeSchemaXmlQuery(doc, qxml , this.CurrentView.Query );

                        //change order of query condition, for calendar view 09-1-19
                        SharePointUtil.ChangeSchemaXmlQuery(doc, this.CurrentView.Query, qxml );                        


                        //listWp.ViewGuid = ""; 
                        //当列表连接了其他webpart时，必须强制清空ViewGuid，否则列表会取视图的查询条件
                        //但是这样做,首先，试图选择 实效， 本类也需要重新梳理，不然，查询条件清空时，还原ViewXml时会找不到ViewGuid


                        listWp.ListViewXml = doc.InnerXml;
                    }
              
                    break;
                }
            }
        }
                
        void _QueryBtn_Click(object sender, EventArgs e)
        {
            buildCaml();
        }

        public override EditorPartCollection CreateEditorParts()
        {
            ArrayList editorArray = new ArrayList();

            ListQueryFieldEditorPart edPart = new ListQueryFieldEditorPart();
            edPart.ID = "ListFieldEditorPart1";
            editorArray.Add(edPart);

            ListExportFieldEditorPart expPart = new ListExportFieldEditorPart();
            expPart.ID = "ListFieldEditorPart2";
            editorArray.Add(expPart);

            EditorPartCollection initEditorParts = base.CreateEditorParts();

            EditorPartCollection editorParts = new EditorPartCollection(initEditorParts, editorArray);

            return editorParts;           
        }

        [Personalizable(false)]
        private string InternalQuery
        {
            get
            {               
                  return ViewState["InternalQuery"] != null ? (String)ViewState["InternalQuery"] : "";                
            }
            set
            {                 
                 ViewState["InternalQuery"] = value;               
            }
        }

        #region 属性持久化服务

        private NameValueCollection _queryControlProperties;

        void SaveQueryControlPropertiesToSession()
        {
            if (_queryControlProperties != null && Page.IsPostBack)
                base.Context.Session["QueryContition_" + this.WebPartUniqueId] = _queryControlProperties;
        }

        void EnsureQueryControlProperties()
        {
            if (_queryControlProperties == null)
            {
                _queryControlProperties = base.Context.Session["QueryContition_" + this.WebPartUniqueId]
                    as NameValueCollection;

                if (_queryControlProperties == null)
                    _queryControlProperties = new NameValueCollection();
            }
        }

        public string GetPropertyValue(Control ctl, string name)
        {          
            EnsureQueryControlProperties();
            return _queryControlProperties[ctl.UniqueID+"_"+name];
        }

        public void SetPropertyValue(Control ctl, string name, string value)
        {
            EnsureQueryControlProperties();
            _queryControlProperties[ctl.UniqueID + "_" + name] = value;
        }

        

        #endregion

    }

    
    

    
}
