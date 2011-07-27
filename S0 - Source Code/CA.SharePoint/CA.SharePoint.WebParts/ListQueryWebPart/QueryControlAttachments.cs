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
    /// 字符串类型的查询控件
    /// </summary>
    class QueryControlAttachments : Control, IQueryControl
    {

        //private SPFieldChoice _Field;
 
        private ListControl _ListControl;

        //public QueryControlAttachments(SPField f)
        //{
        //   // _Field = (SPFieldChoice)f;
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnsureChildControls();           
        }
       
        protected override void CreateChildControls()
        {
            if (this.ChildControlsCreated) return;

             _ListControl = new DropDownList();

            _ListControl.CssClass = "ms-RadioText";

            ListItem emptyItem = new ListItem("--","");
            _ListControl.Items.Add( emptyItem );

            ListItem item1 = new ListItem("Yes", "1");
            _ListControl.Items.Add(item1);
            
            ListItem item2 = new ListItem("No", "0");
            _ListControl.Items.Add(item2);
             
            this.Controls.Add(_ListControl);

            this.ChildControlsCreated = true;
        }

        private string _FieldName;

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public CAMLExpression<object> QueryExpression
        {
            get
            {
                this.EnsureChildControls();

                if (_PropertyPersistenceService != null)
                {
                    if (Page.IsPostBack)
                        _PropertyPersistenceService.SetPropertyValue(this, "", _ListControl.SelectedValue );
                    else
                        _ListControl.SelectedValue = "" + _PropertyPersistenceService.GetPropertyValue(this, "");
                }

                if (!String.IsNullOrEmpty(_ListControl.SelectedValue))
                {
                    QueryField f = new QueryField(_FieldName);
                    return f.Contains(_ListControl.SelectedValue);
                }

                return null;
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
