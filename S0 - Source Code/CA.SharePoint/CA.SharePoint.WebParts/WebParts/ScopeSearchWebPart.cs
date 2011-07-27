
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
using Microsoft.SharePoint.Utilities;

namespace CA.SharePoint
{
    /// <summary>
    /// 查询范围
    /// </summary>
    public enum SearchScope
    {
        //[ResWebDisplayName("所有站点")]
        AllSite ,
        //[ResWebDisplayName("本站点")]
        Web,
        //[ResWebDisplayName("列表")]
        List 
    }

    /// <summary>
    /// 搜索框
    /// </summary>
    public class ScopeSearchWebPart : BaseSPWebPart
    {
        private TextBox _txtSearchContent = new TextBox();


        private string _SearchResultPageUrl = "/MCS_Search.aspx";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ScopeSearchWebPart_SearchResultPageUrl")]
        public string SearchResultPageUrl
        {
            get { return _SearchResultPageUrl; }
            set { _SearchResultPageUrl = value; }
        }

        private SearchScope _SearchScope = SearchScope.List ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ScopeSearchWebPart_SearchScope")]
        public SearchScope SearchScope
        {
            get { return _SearchScope; }
            set { _SearchScope = value; }
        }

        private string _SearchUrlPrifix;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ScopeSearchWebPart_SearchUrlPrifix")]
        public string SearchUrlPrifix
        {
            get { return _SearchUrlPrifix; }
            set { _SearchUrlPrifix = value; }
        }

        private string _SearchBoxLabel = "";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ScopeSearchWebPart_SearchBoxLabel")]
        public string SearchBoxLabel
        {
            get { return _SearchBoxLabel; }
            set { _SearchBoxLabel = value; }
        }

        private Unit _SearchBoxWidth;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("ScopeSearchWebPart_SearchBoxWidth")]
        public Unit SearchBoxWidth
        {
            get { return _SearchBoxWidth; }
            set { _SearchBoxWidth = value; }
        }

        protected override void CreateChildControls()
        {
            if (this.ChildControlsCreated) return;

            base.CreateChildControls();
            _txtSearchContent.ID = "searchKeyWords";
            this.Controls.Add(_txtSearchContent); 

            this.ChildControlsCreated = true;
        }

        //encodeURIComponent为moss系统函数
        private const string WebPart_Js = @"
function ScopeSearchWebPart_OnKeyPress(event1,id,pageUrl,u,cs){
        var kCode = String.fromCharCode(event1.keyCode);
        if(kCode == '\n' || kCode == '\r')
        {   
        ScopeSearchWebPart_Submit(id,pageUrl,u,cs);
        }
 }

function ScopeSearchWebPart_Submit(id,pageUrl,u,cs){
    var obj = document.getElementById(id);

    if(obj.value==''){
        alert('请输入一个或多个要搜索的单词');
        obj.focus();
        return ;
    }

    window.location= pageUrl + '&k=' + encodeURIComponent(obj.value) + '&u=' + encodeURIComponent(u) + '&cs=' + encodeURIComponent(cs)  ;
 }        

";
        private string _PageUrl;
        private string _sScope;
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            _PageUrl = this.SearchResultPageUrl;
            if (_PageUrl.IndexOf("?") == -1)
                _PageUrl += "?";

            //if (!String.IsNullOrEmpty(this._SearchScope))
            //    _PageUrl += "&u=" + Page.Server.UrlEncode( this._SearchScope ) ;

            //_PageUrl += "&cs=" + Page.Server.UrlEncode("此列表")   ;

            switch( _SearchScope )
            {
                case SearchScope.AllSite :
                    _sScope = "All Site";
                    break;
                case SearchScope.Web:
                    _sScope = "This Site";
                    break;
                case SearchScope.List:
                    _sScope = "This List";
                    break;
                default:
                    break;
            }

            //_SearchUrlPrifix = Page.Server.UrlEncode(_SearchUrlPrifix);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ScopeSearchWebPart_Js", WebPart_Js, true);

            if (!_SearchBoxWidth.IsEmpty)
                _txtSearchContent.Width = _SearchBoxWidth;

            this.CssClass = "ms-sbcell ms-sbtext";

            _txtSearchContent.CssClass = "ms-sbplain";
            _txtSearchContent.ToolTip = "Enter search word";
            _txtSearchContent.Attributes.Add("onkeypress",
                String.Format("javascript:ScopeSearchWebPart_OnKeyPress(event,'{0}','{1}','{2}','{3}');", this._txtSearchContent.ClientID, _PageUrl, ( _SearchUrlPrifix ), _sScope));

        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"ms-sbcell ms-sbtext\">");

            writer.Write(_SearchBoxLabel);

            base.RenderContents(writer);

            writer.Write("</td><td class=\"ms-sbgo ms-sbcell\">");

            writer.Write("<a href=\"" +
                String.Format("javascript:ScopeSearchWebPart_Submit('{0}','{1}','{2}','{3}');", this._txtSearchContent.ClientID, _PageUrl, (_SearchUrlPrifix), _sScope) + 
                "\" >");

            writer.Write( "<img title=\"Start search\" onmouseover=\"this.src='/_layouts/images/gosearch.gif'\" onmouseout=\"this.src='/_layouts/images/gosearch.gif'\" alt=\"开始搜索\" src=\"/_layouts/images/gosearch.gif\" style=\"border-width:0px;\" />" );
            writer.Write("</a>");

            writer.Write("</td></tr></table>");

        }

    }

    
     
}
/*

									<td class="ms-sbcell ms-sbtext"><span title="CCCCC" style="display:block;">CCCCC</span>
</td><td class="ms-sbcell"><input name="ctl00$ctl08$g_f9452186_ed95_4af9_828a_bdf48b151fa1$SCF0B7D17_InputKeywords"
type="text" maxlength="197" id="ctl00_ctl08_g_f9452186_ed95_4af9_828a_bdf48b151fa1_SCF0B7D17_InputKeywords" accesskey="S"
title="输入搜索文字" class="ms-sbplain" alt="输入搜索文字" onkeypress="javascript:return SCF0B7D17_OSBEK(event);" style="width:170px;" />
</td><td class="ms-sbgo ms-sbcell"><a id="ctl00_ctl08_g_f9452186_ed95_4af9_828a_bdf48b151fa1_SCF0B7D17_go" 
title="开始搜索" href="javascript:SCF0B7D17_Submit()">
<img title="开始搜索" onmouseover="this.src='/_layouts/images/gosearch.gif'" onmouseout="this.src='/_layouts/images/gosearch.gif'" alt="开始搜索" 
src="/_layouts/images/gosearch.gif" style="border-width:0px;" /></a>

*/