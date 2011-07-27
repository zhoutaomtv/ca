
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
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Specialized;
using System.Xml;
using CA.SharePoint.WebPartSkin;

namespace CA.SharePoint.EditParts
{
    /// <summary>
    ///  webpart皮肤编辑控件
    /// </summary>
    public class SkinEditorPart : EditorPart
    {
        private DropDownList _dlSkinScope;
        private DropDownList _dlSkinList;
 
        private RadioButton _rdUserExistSkin;
        private RadioButton _rdUserNewSkin;

        private HiddenField _hdSkinHtml ;

        private HyperLink _editLink;

        private LinkButton _btnClearSkinCache;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EnsureChildControls();

        }

        protected override void CreateChildControls()
        {
           // base.CreateChildControls();

            base.Title = WPResource.GetString("SkinEditPart_Title");

            _dlSkinScope = new DropDownList();
            _dlSkinScope.ID = this.ID + "_SkinScope";
            _dlSkinScope.Items.Add(new ListItem("Farm Skin", SkinScope.Farm.ToString()));
            _dlSkinScope.Items.Add(new ListItem("App Skin", SkinScope.App.ToString()));
            _dlSkinScope.Items.Add(new ListItem("Site Skin", SkinScope.Site.ToString()));
            _dlSkinScope.SelectedIndexChanged += new EventHandler(_dlSkinScope_SelectedIndexChanged);
            _dlSkinScope.AutoPostBack = true;         
            

            _dlSkinList = new DropDownList();

            _dlSkinList.ID = this.ID + "_SkinList";
            _dlSkinList.Width = new Unit("150px") ;

            _rdUserExistSkin = new RadioButton();
            _rdUserExistSkin.ID = this.ID + "_rdUserExistSkin";
            _rdUserExistSkin.Text = "Use Exist Skin";
            _rdUserExistSkin.GroupName = this.ID + "_skinMode";
            this.Controls.Add( _rdUserExistSkin );

            _rdUserNewSkin = new RadioButton();
            _rdUserNewSkin.ID = this.ID + "_rdUserNewSkin";
            _rdUserNewSkin.Text = "Use New Skin";
            _rdUserNewSkin.GroupName = this.ID + "_skinMode";
            this.Controls.Add( _rdUserNewSkin );

            _rdUserExistSkin.AutoPostBack = true;
            _rdUserExistSkin.CheckedChanged += new EventHandler(_rdUserExistSkin_CheckedChanged);
            _rdUserNewSkin.AutoPostBack = true;
            _rdUserNewSkin.CheckedChanged += new EventHandler(_rdUserNewSkin_CheckedChanged);

            _hdSkinHtml = new HiddenField();
            //_hdSkinHtml.TextMode = TextBoxMode.MultiLine;
            _hdSkinHtml.ID = this.ID + "_hdSkinHtml";
            
            this.Controls.Add(_hdSkinHtml);

            _btnClearSkinCache = new LinkButton();
            _btnClearSkinCache.Text = "Update";
            _btnClearSkinCache.ToolTip = "Update Skin Cache";

            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            this.Controls.Add(_btnClearSkinCache);
            _btnClearSkinCache.Click += new EventHandler(_btnClearSkinCache_Click);

            this.Controls.Add( new LiteralControl("<br/>") );

            this.Controls.Add(_dlSkinScope);
            this.Controls.Add(_dlSkinList);            

            _dlSkinList.EnableViewState = true;

            //_dlSkinList.Items.Clear();


            BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            IDictionary<String, SkinElement> skins = SkinManager.GetSkinManager(wp.SkinScope).AllSkins;

            ListItem item = null;
            item = new ListItem(" ", "");
            _dlSkinList.Items.Add(item);

            foreach (SkinElement s in skins.Values)
            {
                item = new ListItem(s.ID, s.ID.ToLower());
                _dlSkinList.Items.Add(item);
            }

            //BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            _editLink = new HyperLink();
            _editLink.Text = "Edit Current Skin";
            _editLink.NavigateUrl = "javascript:MCS_EditInputValue('" + _hdSkinHtml.ClientID + "')";

            this.Controls.Add(new LiteralControl("<br/>"));
            this.Controls.Add(_editLink);

            this.ChildControlsCreated = true;
        }

        void _btnClearSkinCache_Click(object sender, EventArgs e)
        {
            BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            SkinManager.GetSkinManager(wp.SkinScope).ClearSkinCache();
        }

        void _rdUserNewSkin_CheckedChanged(object sender, EventArgs e)
        {
            this._dlSkinScope.Enabled = false;
            this._dlSkinList.Enabled = false ;

            this._editLink.Visible = true;

            BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            //若新皮肤为空，则自动获取全局皮肤
            if (_hdSkinHtml.Value.Trim() == "" && !String.IsNullOrEmpty( wp.TemplateID ) )
            {            
                SkinManager sm = SkinManager.GetSkinManager(wp.SkinScope);
                SkinElement skin = sm.GetSkin(wp.TemplateID);

                if (skin != null)
                {
                    _hdSkinHtml.Value = skin.SkinHtml ;
                }
            }                
        }

        void _rdUserExistSkin_CheckedChanged(object sender, EventArgs e)
        {
             this._dlSkinScope.Enabled = true;
            this._dlSkinList.Enabled = true;

            this._editLink.Visible = false ;
        }
              
        /// <summary>
        /// 皮肤范围 改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _dlSkinScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            SkinScope skinScope = (SkinScope)Enum.Parse(typeof(SkinScope) , this._dlSkinScope.SelectedValue );

            IDictionary<String, SkinElement> skins = SkinManager.GetSkinManager(skinScope).AllSkins;

            this._dlSkinList.Items.Clear();

            ListItem item = null;
            item = new ListItem(" ", "");
            _dlSkinList.Items.Add(item);

            foreach (SkinElement s in skins.Values) //重建皮肤列表
            {
                item = new ListItem(s.ID, s.ID.ToLower());
                _dlSkinList.Items.Add(item);

                if (String.Compare(s.ID, wp.TemplateID, true) == 0)
                    item.Selected = true;
            }
        }

       
        /// <summary>
        /// 将editpart的值应用到webpart
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            this.EnsureChildControls();

            BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            wp.SkinScope = (SkinScope)Enum.Parse(typeof(SkinScope) , this._dlSkinScope.SelectedValue );

            wp.TemplateID = _dlSkinList.SelectedValue;

            wp.PersistSkin = this._rdUserNewSkin.Checked;

            if (wp.PersistSkin )
            {
                if (_hdSkinHtml.Value.Trim() != "")
                {
                    wp.PersistedSkinElement = new SkinElement(_hdSkinHtml.Value);
                }
                else if (wp.TemplateID != "") //若清空了皮肤，则自动获取 全局皮肤
                {
                    SkinManager sm = SkinManager.GetSkinManager( wp.SkinScope );
                    SkinElement skin = sm.GetSkin(wp.TemplateID);

                    if (skin != null)
                    {
                        wp.PersistedSkinElement = skin;
                    }
                }                
            }

            return true;
        }

        /// <summary>
        /// 将webpart的值同步到editpart
        /// </summary>
        public override void SyncChanges()
        {
            this.EnsureChildControls();

            BaseSPWebPart wp = (BaseSPWebPart)WebPartToEdit;

            _dlSkinScope.SelectedValue = wp.SkinScope.ToString();

            if (!String.IsNullOrEmpty(wp.TemplateID))
            {
                _dlSkinList.SelectedValue = wp.TemplateID.ToLower();
            }

            _rdUserNewSkin.Checked = wp.PersistSkin;
            _rdUserExistSkin.Checked = !wp.PersistSkin;

            _dlSkinList.Enabled = !wp.PersistSkin;
            _dlSkinScope.Enabled = !wp.PersistSkin;

            _editLink.Visible = wp.PersistSkin;

            if (wp.PersistSkin)
            {
                if (wp.PersistedSkinElement != null )
                {
                    _hdSkinHtml.Value = wp.PersistedSkinElement.SkinHtml;
                }               
            }
        }

    }
}
