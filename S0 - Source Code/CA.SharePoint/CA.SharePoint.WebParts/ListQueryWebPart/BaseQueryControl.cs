
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
    class BaseQueryControl : Control , IQueryControl
    {

        private string _FieldName;

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
         

        public virtual CAMLExpression<object> QueryExpression
        {
            get
            {              
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
