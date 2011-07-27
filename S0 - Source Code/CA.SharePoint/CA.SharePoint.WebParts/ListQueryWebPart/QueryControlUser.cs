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
    /// ÓÃ»§²éÑ¯×Ö¶Î
    /// </summary>
    class QueryControlUser : Control, IQueryControl 
    {
        private SPFieldUser _Field;

        private Microsoft.SharePoint.WebControls.PeopleEditor _PeopleEditor ;

        public QueryControlUser(SPField f)
        {
            _Field = (SPFieldUser)f;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnsureChildControls();
        }
       
        protected override void CreateChildControls()
        {
            if (this.ChildControlsCreated) return;

            _PeopleEditor = new Microsoft.SharePoint.WebControls.PeopleEditor();
            this.Controls.Add(_PeopleEditor);
 
             _PeopleEditor.MultiSelect = _Field.AllowMultipleValues ;
             _PeopleEditor.Rows = 1;
             _PeopleEditor.Width = new Unit("120px");

             if (_PropertyPersistenceService != null)
             {
                 if (!Page.IsPostBack)
                   _PeopleEditor.CommaSeparatedAccounts =
                       _PropertyPersistenceService.GetPropertyValue(this, "");
             }

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
                        _PropertyPersistenceService.SetPropertyValue(this, "", _PeopleEditor.CommaSeparatedAccounts);                    
                }

                CAMLExpression<object> expr = null;

                QueryField f = new QueryField ( _FieldName ) ;

                 foreach (Microsoft.SharePoint.WebControls.PickerEntity p in _PeopleEditor.ResolvedEntities )
                {
                    if (expr == null)
                        expr = f.Contains(p.EntityData["DisplayName"].ToString());
                    else
                        expr = expr | f.Contains(p.EntityData["DisplayName"].ToString());
                }      

                //foreach (string s in _PeopleEditor.Accounts)
                //{
                //    if (expr == null)
                //        expr = f.Contains(s);
                //    else
                //        expr = expr | f.Contains(s);
                //}                               

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
