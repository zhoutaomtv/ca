using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CA.Web
{
    /// <summary>
    /// 表单数据参数
    /// </summary>
    public class FormParameter : Parameter
    {
        public override object Evaluate(HttpContext context, Control control)
        {
            if (string.IsNullOrEmpty(FormField))
                throw new ObjectMapException("FormField为空", this);

            if ((context != null) && (context.Request != null))
            {
                return context.Request.Form[this.FormField];
            }
            return null;

        }

        private string _FormField;
        /// <summary>
        /// 表单域
        /// </summary>
        public string FormField
        {
            get
            {
                //object obj1 = base.ViewState["FormField"];
                //if (obj1 == null)
                //{
                //    return string.Empty;
                //}
                return _FormField;
            }
            set
            {
                _FormField = value;
                //if (this.FormField != value)
                //{
                //    base.ViewState["FormField"] = value;
                //    base.OnParameterChanged();
                //}
            }
        }

    }
}
