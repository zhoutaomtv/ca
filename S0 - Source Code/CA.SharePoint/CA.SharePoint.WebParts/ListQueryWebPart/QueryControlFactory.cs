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

namespace CA.SharePoint
{
    /// <summary>
    /// 查询字段控件工厂
    /// </summary>
    class QueryControlFactory
    {
        /// <summary>
        /// 返回某个SPField对应的查询字段
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        static public IQueryControl GetQueryControl(SPField field , IPropertyPersistenceService s )
        {
            IQueryControl ctl = null;

            if (field.FieldValueType == typeof(DateTime))
            {
                ctl = new QueryControlDate();
            }            
            else if (field is SPFieldChoice)
            {
                ctl = new QueryControlChoice(field);
            }
            else if (field is SPFieldMultiChoice )
            {
                ctl = new QueryControlMultiChioce(field);
            }
            else if (field is SPFieldBoolean)
            {
                ctl = new QueryControlBoolean(field);
            }
            else if (field is SPFieldUser)
            {
                ctl = new QueryControlUser(field);
            }
            else if (field is SPFieldAttachments)
            {
                ctl = new QueryControlAttachments();
            }
            else
            {
                ctl = new QueryControlText();
            }

            ctl.PropertyPersistenceService = s;

            return ctl;

           // return StringKeyWordControl(); 
        }

    }

     
}
