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
    /// 查询字段接口
    /// </summary>
    interface IQueryControl
    {
        /// <summary>
        /// 字段名
        /// </summary>
        String FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段产生的查询条件，可以为null
        /// </summary>
        CAMLExpression<object> QueryExpression
        {
            get;
        }

        IPropertyPersistenceService PropertyPersistenceService
        {
            set;
        }
    }
    /// <summary>
    /// 控件属性持久化服务---将控件属性在某个范围内保持
    /// </summary>
    interface IPropertyPersistenceService
    {
        string GetPropertyValue( Control ctl , string name );

        void SetPropertyValue(Control ctl, string name, string value);
    }
     
}
