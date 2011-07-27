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
    class QueryControlText : TextBox, IQueryControl
    {

        private string _FieldName;

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.CssClass = "ms-input";
            //this.Style.Add)

           
        }

        //protected override void CreateChildControls()
        //{
        //    base.CreateChildControls();
        //    if (!Page.IsPostBack && _PropertyPersistenceService != null)
        //    {
        //        this.Text = "" + _PropertyPersistenceService.GetPropertyValue(this, "text");
        //    }
        //}

        public CAMLExpression<object> QueryExpression
        {
            get
            {
                this.Text = this.Text.Trim();

                if ( _PropertyPersistenceService != null)
                {   
                    if( Page.IsPostBack )
                        _PropertyPersistenceService.SetPropertyValue(this, "text", this.Text);
                    else
                        this.Text = "" + _PropertyPersistenceService.GetPropertyValue(this, "text");
                }

                if (!String.IsNullOrEmpty(this.Text))
                {
                    QueryField f = new QueryField(_FieldName);
                    return f.Contains(this.Text);
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
