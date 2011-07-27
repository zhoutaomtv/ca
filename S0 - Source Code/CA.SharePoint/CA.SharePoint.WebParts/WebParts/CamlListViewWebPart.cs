
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


using CA.SharePoint.WebPartSkin;
using CA.SharePoint.EditParts;

namespace CA.SharePoint
{

    
    public class CamlListViewWebPart : BaseSPListWebPart
    {

        private IQueryConditionProvider _QueryConditionProvider;

        [ResConnectionConsumer("QueryCondition", "QueryConditionConsumer",
           AllowsMultipleConnections = true)]
        public virtual void ConsumeQueryCondition(IQueryConditionProvider provider)
        {
            _QueryConditionProvider = provider;

            SetQuery();
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

        protected Microsoft.SharePoint.WebPartPages.ListViewWebPart ContentList = null ; 

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.EnsureChildControls();
        }

        private XmlDocument _ListViewXml;
        void SetQuery()
        {
            this.EnsureChildControls();
            if (ContentList != null)
            {
                //ProcessToolbarSchema(doc);

                if (this._QueryConditionProvider != null && !String.IsNullOrEmpty(this._QueryConditionProvider.QueryCondition.Query))
                {
                    SharePointUtil.ChangeSchemaXmlQuery(_ListViewXml, this._QueryConditionProvider.QueryCondition.Query);
                }

                ContentList.ListViewXml = _ListViewXml.InnerXml ;
            }
        }
            
 

        protected override void CreateChildControls()
        {
            //if (this.ChildControlsCreated) return;        

            this.Controls.Clear();

            base.CreateChildControls();            

            if (List == null)
            {
                base.RegisterShowToolPanelControl("Please", "open Tools panel", "，configure “List Name”。");
                return;
            }

            CreateContentList(); 
        }

        protected virtual string ListViewXml
        {
            get
            {
                return base.CurrentView.HtmlSchemaXml;
            }
        }
  
        protected virtual void CreateContentList()
        {
            if (base.List == null) return;

            ContentList = new Microsoft.SharePoint.WebPartPages.ListViewWebPart();
            ContentList.ID = this.ID + "ContentList";
           // ContentList.Title = this.Title;
            ContentList.ListName = List.ID.ToString("B").ToUpper();         
             
            ContentList.ListViewXml = this.ListViewXml;

            ContentList.EnableViewState = true;

            ContentList.ChromeType = PartChromeType.None; //不显示标题 

            this.Controls.Add(ContentList);         
        }          
   

        //public override EditorPartCollection CreateEditorParts()
        //{
        //    ArrayList editorArray = new ArrayList();

        //    ViewEditorPart edPart = new ViewEditorPart();
        //    edPart.ID = "ViewEditorPart1";
        //    editorArray.Add(edPart);  

        //    EditorPartCollection initEditorParts = base.CreateEditorParts();

        //    EditorPartCollection editorParts = new EditorPartCollection(initEditorParts, editorArray);

        //    return editorParts;
        //}

    }
}
