using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search.Query;

using CA.Web; 
using CA.SharePoint.CamlQuery;

namespace CA.SharePoint
{    
    
    /// <summary>
    /// 由SPField的 RenderControl实现的IQueryKeyWordControl，可以满足所有字段类型，但是会在导航栏显示 “新建项目”
    /// </summary>
    class RenderControlKeyWordControl : Control , IQueryControl
    {
        private SPField _Field;
        public RenderControlKeyWordControl(SPField f)
        {
            _Field = f;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnsureChildControls();
        }

        private BaseFieldControl _FieldRenderingControl ;
        protected override void CreateChildControls()
        {
            _FieldRenderingControl = _Field.FieldRenderingControl ;
            _FieldRenderingControl.ControlMode = SPControlMode.New;
            this.Controls.Add(_FieldRenderingControl);

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
                object objValue = _FieldRenderingControl.Value;

                if (objValue == null)
                    return null ;

                string sValue = objValue.ToString() ;                

                if (!String.IsNullOrEmpty(sValue))
                {
                    QueryField f = new QueryField(_FieldName);
                    return f.Contains(sValue);
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
