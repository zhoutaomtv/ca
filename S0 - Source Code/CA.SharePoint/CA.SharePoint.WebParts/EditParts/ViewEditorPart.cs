
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
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Specialized;
using System.Xml;
using CA.SharePoint.WebPartSkin;

namespace CA.SharePoint.EditParts
{
    /// <summary>
    ///  View编辑控件
    /// </summary>
    public class ViewEditorPart : EditorPart
    {
        public ViewEditorPart()
        {

        }

        protected DropDownList _ViewList;

        RadioButton _rbUserExistView;
        RadioButton _rbUserNewView;

          
        protected override void CreateChildControls()
        {
            ListViewWebPart wp = (ListViewWebPart)this.WebPartToEdit;

            this.Controls.Clear();
            //base.CreateChildControls();

            base.Title = WPResource.GetString("ViewEditorPart_Title","View");

            _rbUserExistView = new RadioButton();
            _rbUserExistView.GroupName = this.ID + "UserExistView";
            _rbUserExistView.Text = "Use Exist View";
            this.Controls.Add(_rbUserExistView);

            _rbUserNewView = new RadioButton();
            _rbUserNewView.GroupName = this.ID + "UserExistView";
            _rbUserNewView.Text = "Use New View";
            this.Controls.Add( _rbUserNewView );

            this._rbUserExistView.Checked = wp.UseExistView;
            this._rbUserNewView.Checked = !wp.UseExistView;

            this.Controls.Add( new LiteralControl("<br/>") );

            _ViewList = new DropDownList();

            this.Controls.Add(_ViewList);

            _ViewList.EnableViewState = true;
            _ViewList.Width = new Unit("100px");

            _ViewList.Items.Clear();           

            if (wp.List == null) return;

            ListItem item = null; 

            if ( !wp.UseExistView && wp.ViewID != Guid.Empty)
            {
                item = new ListItem( "<Current View>" , wp.ViewID.ToString() );
                _ViewList.Items.Add(item);
                item.Selected = true;
            }
            //else
            //{
            //    item = new ListItem(" ", "");
            //    _ViewList.Items.Add(item);
            //}

            //string viewId = wp.ViewID.ToString().ToLower();

            foreach (SPView v in wp.List.Views)
            {
                if (v.Hidden) continue;
                item = new ListItem(v.Title , v.ID.ToString());
                _ViewList.Items.Add(item);

                //item.Selected = (viewId == item.Value.ToLower()) ;
            }            

            if (wp.ViewID != Guid.Empty)
            {
                this.Controls.Add(new LiteralControl("<br/>"));
                HtmlAnchor link = new HtmlAnchor();
                link.InnerText = "Edit Current View";

                link.HRef = SPHttpUtility.HtmlUrlAttributeEncode(
                    SPHttpUtility.UrlPathEncode(SPContext.Current.Web.Url + "/_layouts/viewedit.aspx", true, true) +
                    "?List=" + SPHttpUtility.UrlKeyValueEncode("{" + wp.List.ID.ToString().ToUpper() + "}") +
                    "&View=" + SPHttpUtility.UrlKeyValueEncode("{" + wp.ViewID.ToString().ToUpper() + "}") +
                    "&Source=" + SPHttpUtility.UrlKeyValueEncode(base.WebPartManager.Page.Request.Url.AbsoluteUri));

                this.Controls.Add(link);

                _ViewList.SelectedValue =wp.ViewID.ToString() ;
            }
            else
            {
                _ViewList.SelectedValue = wp.List.DefaultView.ID.ToString();
            }
        }
        
        SPView CreateNewView(SPList list, Guid exampleViewId , string viewTitle )
        {
            SPView view = list.Views[exampleViewId];

            StringCollection fs = new StringCollection();

            foreach (string s in view.ViewFields)
            {
                fs.Add(s);
            }

            string viewName = "WPView" + list.Views.Count;

            SPView newView = list.Views.Add( viewName , fs, view.Query, view.RowLimit, view.Paged, false);

            if (!String.IsNullOrEmpty(viewTitle))
                newView.Title = viewTitle;

            newView.Hidden = true;
            newView.Update();

            return newView;
        }       

        public override bool ApplyChanges()
        {
            this.EnsureChildControls();

            ListViewWebPart wp = (ListViewWebPart)WebPartToEdit;

            if (wp.List == null) return true;

            wp.UseExistView = this._rbUserExistView.Checked;

            if( !wp.ContentCreated )
                wp.ReCreateChildControls();

            if (String.IsNullOrEmpty(_ViewList.SelectedValue)) return true;

            Guid selectedViewId = new Guid( _ViewList.SelectedValue );

            if (wp.UseExistView) //使用已有视图时，直接修改视图ID属性
            {
                wp.ViewID = selectedViewId;
                return true;
            }

            //使用新视图时，如要操作新的视图对象，创建或修改

            if (wp.ViewID == Guid.Empty || wp.RelationViewID == Guid.Empty || ! SharePointUtil.ViewExist( wp.List.Views , wp.ViewID ) )
            //新建或视图不存在（被删除）, RelationViewID为空，说明以前采用的是已经存在的视图
            { 
                SPView newView = this.CreateNewView(wp.List, selectedViewId , "" );

                wp.ViewID = newView.ID;
                wp.RelationViewID = newView.ID;
            }
            else 
            {
                wp.ViewID = wp.RelationViewID ;

                SPView targetView = wp.List.Views[selectedViewId];

                SPView thisView = wp.List.Views[wp.ViewID];

                //if (!String.IsNullOrEmpty(wp.ViewName))
                //    thisView.Title = wp.ViewName;

                thisView.Query = targetView.Query;
                thisView.RowLimit = targetView.RowLimit;
                thisView.Paged = targetView.Paged;
                thisView.ViewFields.DeleteAll();

                foreach (string s in targetView.ViewFields)
                {
                    thisView.ViewFields.Add(s);
                }

                thisView.Update();
            }           

            return true;
        }



        public override void SyncChanges()
        {
            CreateChildControls();
        }
         

    }
}
