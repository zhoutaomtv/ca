using System;
using System.Collections.Generic;
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

using Microsoft.SharePoint;
using CA.SharePoint.CamlQuery;

namespace CA.SharePoint
{
    /// <summary>
    /// 复选框列表字段对应的查询控件
    /// </summary>
    class QueryControlMultiChioce : Control, IQueryControl 
    {
        private SPFieldMultiChoice _Field;

        private CheckBoxList _ListControl;

        public QueryControlMultiChioce(SPField f)
        {
            _Field = (SPFieldMultiChoice)f;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnsureChildControls();
        }
       
        protected override void CreateChildControls()
        {
            if (this.ChildControlsCreated) return;

            _ListControl = new  CheckBoxList();
            _ListControl.RepeatDirection = RepeatDirection.Horizontal;
 
            foreach (String key in _Field.Choices)
            {
                ListItem item = new ListItem(key);
                _ListControl.Items.Add( item );
            }
             
            this.Controls.Add(_ListControl);

            this.ChildControlsCreated = true;
        }

        private string _FieldName;

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        string GetSelectedIndex()
        {
            string ids = "";
            for (int i = 0; i < _ListControl.Items.Count; i ++  )
            {
                if (_ListControl.Items[i].Selected)
                {
                    if (ids != "") ids += ",";
                    ids += i;
                }
            }

            return ids;
        }

        void  SetSelectedItems( string ids )
        {
            if (String.IsNullOrEmpty(ids)) return;

            string[] arr = ids.Split(',');

            foreach (string id in arr)
            {
                _ListControl.Items[Convert.ToInt32(id)].Selected = true;
            }
        }

        public CAMLExpression<object> QueryExpression
        {
            get
            {
                this.EnsureChildControls();

                if (_PropertyPersistenceService != null)
                {
                    if (Page.IsPostBack)
                        _PropertyPersistenceService.SetPropertyValue( this, "", GetSelectedIndex() );
                    else
                        SetSelectedItems(_PropertyPersistenceService.GetPropertyValue(this, ""));
                }

                QueryField f = new QueryField(_FieldName);

                CAMLExpression<object> expr = null;

                foreach (ListItem item in _ListControl.Items)
                {
                    if (item.Selected)
                    {
                        if (expr == null)
                            expr = f.Equal(item.Value);
                        else
                            expr = expr | f.Equal(item.Value);
                    }
                }
                 
                return expr ;
            }
        }

        private IPropertyPersistenceService _PropertyPersistenceService;
        public IPropertyPersistenceService PropertyPersistenceService
        {
            set
            {
                _PropertyPersistenceService = value;
            }
        }

    }


     
}
