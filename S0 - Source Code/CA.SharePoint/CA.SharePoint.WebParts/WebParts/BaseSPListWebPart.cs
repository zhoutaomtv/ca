
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
//using Microsoft.SharePoint.Publishing;
using CA.SharePoint.WebPartSkin;


namespace CA.SharePoint 
{
    /// <summary>
    /// 跟SPList关联的WebPart的基类
    /// </summary>
    public class BaseSPListWebPart : BaseSPWebPart
    {
        private string _ListName;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_ListName")]
        //[ManagedLinkAttribute] //Microsoft.SharePoint.Publishing.WebControls.AssetUrlSelector
        public virtual string ListName
        {
            get
            {
                return _ListName;
            }
            set
            {
                _ListName = value;
            }
        }

        private string _ListId;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        //[ResWebDisplayName("BaseSPWebPart_ListName")]
        //[ManagedLinkAttribute] //Microsoft.SharePoint.Publishing.WebControls.AssetUrlSelector
        public virtual string ListId
        {
            get
            {
                return _ListId;
            }
            set
            {
                _ListId = value;
            }
        }

        private SPView _CurrentView = null;
        /// <summary>
        /// 列表的当前视图
        /// </summary>
        protected SPView CurrentView
        {
            get
            {
                if (_CurrentView != null)
                    return _CurrentView;

                if (this.ViewID != Guid.Empty)
                {
                    SPList list = this.GetCurrentSPList();

                    if (list != null)
                    {
                        try
                        {
                            _CurrentView = list.Views[this.ViewID];
                        }
                        catch (ArgumentException ex)
                        {
                            this.RegisterError(ex);
                            return null;
                        }

                        if (_CurrentView.AggregationsStatus == null)
                            _CurrentView = GetRealView(_CurrentView);
                    }
                }
                //else if( !String.IsNullOrEmpty(this.ViewName) )
                //{
                //     SPList list = this.GetCurrentSPList();

               //     if (list != null)
                //     {
                //         try{
                //         _CurrentView = list.Views[this.ViewName];
                //         } catch (  ArgumentException ex )
                //         {
                //             this.RegisterError( ex ) ;
                //             return null ;
                //         }

               //         _CurrentView = GetRealView(_CurrentView);
                //     }
                //}
                else //没有配置view时，取上下文view或默认view
                {
                    try
                    {
                        if (SPContext.Current.ViewContext != null)
                        {
                            _CurrentView = SPContext.Current.ViewContext.View;
                        }
                    }
                    catch (ArgumentException e) { }

                    if (_CurrentView == null)
                    {
                        SPList list = this.GetCurrentSPList();

                        if (list != null)
                        {
                            //_CurrentView = list.DefaultView ;
                            _CurrentView = GetRealView(list.DefaultView);
                        }
                    }                    
                }

                return _CurrentView;
            }           
        }
        /// <summary>
        /// 不在Ｖｉｅｗ本页面无法取得真实的数据
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        SPView GetRealView(SPView view)
        {

            //return this.GetCurrentSPWeb().GetViewFromUrl(view.Url);

            SPSite site = new SPSite(SPContext.Current.Site.ID);
            SPWeb web = site.OpenWeb(this.GetCurrentSPWeb().ID);
            SPView v = web.GetViewFromUrl(view.Url);
            web.Dispose();
            site.Dispose();
            return v;
        }

        private Guid _ViewID = Guid.Empty;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        public Guid ViewID
        {
            get
            {
                //if (Guid.Empty == _ViewID)
                //    _ViewID = Guid.NewGuid();

                return _ViewID;
            }
            set { _ViewID = value; }
        }

        //private string _ViewName;
        //[Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable]
        //[ResWebDisplayName( "BaseSPWebPart_ViewName","视图" )]
        //public string ViewName
        //{
        //    get { return _ViewName; }
        //    set { _ViewName = value; }
        //}

        /// <summary>
        /// 关联的列表
        /// </summary>
        internal protected SPList List
        {
            get
            {
                return GetCurrentSPList();
            }
            set
            {
                this._CurSPList = value;
            }
        }

        private SPList _CurSPList; 
        /// <summary>
        /// 获取当前list
        /// </summary>
        /// <returns></returns>
        protected virtual SPList GetCurrentSPList()
        {

            if (_CurSPList != null)
            {                
                return _CurSPList;
            }

            if (!String.IsNullOrEmpty(_ListId))
            {
                try
                {
                    _CurSPList = GetCurrentSPWeb().Lists[ new Guid(_ListId) ];
                }
                catch (ArgumentException ex)
                {
                    base.RegisterError(ex);
                    return null;
                }

            }
            else if (!String.IsNullOrEmpty(_ListName))
            {
                try
                {
                    _CurSPList = GetCurrentSPWeb().Lists[_ListName];
                }
                catch (ArgumentException ex)
                {
                    base.RegisterError(ex);
                    return null;
                }

            }
            else if (Page != null && Page.Request.QueryString["ListId"] != null)
            {
                _CurSPList = base.Web.Lists[new Guid(Page.Request.QueryString["ListId"])];
            }
            else
            {
                _CurSPList = SPContext.Current.List;
            }

            return _CurSPList;
        }

        protected override string ParserSkinTags(string skinKey, string skin)
        {
            IList<ReplaceTag> tags = ReplaceTagManager.GetInstance().GetReplaceTags(skinKey, skin);

            if (tags.Count == 0) return skin;

            StringBuilder sb = new StringBuilder(skin);

            ITagValueProvider wpProvider = new WebPartValueProvider(this);

            ITagValueProvider listValueProvider = null;

            foreach (ReplaceTag tag in tags)
            {
                if (tag.ValueProvider == "webpart")
                    sb.Replace(tag.TagValue, wpProvider.GetValue(tag));
                else if (tag.ValueProvider == "splist")
                {
                    if (listValueProvider == null)
                        listValueProvider = new SPListValueProvider(this.GetCurrentSPList());

                    sb.Replace(tag.TagValue, listValueProvider.GetValue(tag));
                }
            }

            return sb.ToString();
        }

        protected bool IsHiddenFolder(SPFolder f)
        {
            return f.Properties.Count < 20;
        }

        protected void RedirectToListSettingPageUrl()
        {
            if (Page.Request.QueryString["ListId"] != null)
            {
                string webUrl = base.Web.ServerRelativeUrl;
                if (!webUrl.EndsWith("/"))
                    webUrl += "/";

                string sourceUrl = webUrl + "_layouts/listedit.aspx?List=" + this.List.ID.ToString("B").ToUpper();
                Page.Response.Redirect(sourceUrl);
            }
        }

    }
}
