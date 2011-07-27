using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;

namespace CA.Web
{
    /// <summary>
    /// 控件参数
    /// 
    /// 数据验证实现： 默认由是否具有默认值 判断是否需要必添验证 ， 由实体类的数据类型判断 是否需要数据类型有效性验证
    /// 
    /// 如何实现复合验证： 自定义ｊｓ 
    /// 
    /// <example>
    /// <code>
    /// 自定义验证函数
    ///  function customValid( param )
    ////{
    ////     alert(param.Control.value);
    ////     param.MessagePanel.Show( param.Control , "ddd" ) ;
    ////     return true ;
    ////}
    /// </code>
    /// 
    /// </example>
    /// 
    /// </summary>
    public class ControlParameter : Parameter
    {
        private static Dictionary<ValidationType, string> _TypeValidationExpression = new Dictionary<ValidationType,string> () ;

//        Email : /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
//Phone : /^((\(\d{3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}$/,
//Mobile : /^((\(\d{3}\))|(\d{3}\-))?13\d{9}$/,
//Url : /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/,
//IdCard : /^\d{15}(\d{2}[A-Za-z0-9])?$/,
//Currency : /^\d+(\.\d+)?$/,
//Number : /^\d+$/,
//Zip : /^[1-9]\d{5}$/,
//QQ : /^[1-9]\d{4,8}$/,
//Integer : /^[-\+]?\d+$/,
//Double : /^[-\+]?\d+(\.\d+)?$/,
//English : /^[A-Za-z]+$/,
//Chinese : /^[\u0391-\uFFE5]+$/,
//UnSafe : /^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/,
        static ControlParameter()
        {
            _TypeValidationExpression.Add(ValidationType.Integer, @"/^[-\+]?\d+$/");
            _TypeValidationExpression.Add(ValidationType.Double, @"/^[-\+]?\d+(\.\d+)?$/");
            _TypeValidationExpression.Add(ValidationType.Currency, @"/^\d+(\.\d+)?$/");
            _TypeValidationExpression.Add(ValidationType.Email, @"/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/");
            _TypeValidationExpression.Add(ValidationType.Phone, @"/^((\(\d{3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}$/");
            _TypeValidationExpression.Add(ValidationType.Mobile, @"/^((\(\d{3}\))|(\d{3}\-))?13\d{9}$/");
            _TypeValidationExpression.Add(ValidationType.Number, @"/^\d+$/");
            _TypeValidationExpression.Add(ValidationType.IdCard, @"/^\d{15}(\d{2}[A-Za-z0-9])?$/");
            //_TypeValidationExpression.Add(ValidationType.Url, @"/^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>])*$/" );
            _TypeValidationExpression.Add(ValidationType.English, @"/^[A-Za-z]+$/");
            _TypeValidationExpression.Add(ValidationType.Chinese, @"/^[\u0391-\uFFE5]+$/");
            _TypeValidationExpression.Add(ValidationType.DateTime, @"/^(?:(?:1[6-9]|[2-9]\d)?\d{2}[\/\-\.](?:0?[1,3-9]|1[0-2])[\/\-\.](?:29|30))(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:1[6-9]|[2-9]\d)?\d{2}[\/\-\.](?:0?[1,3,5,7,8]|1[02])[\/\-\.]31)(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])[\/\-\.]0?2[\/\-\.]29)(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:16|[2468][048]|[3579][26])00[\/\-\.]0?2[\/\-\.]29)(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:1[6-9]|[2-9]\d)?\d{2}[\/\-\.](?:0?[1-9]|1[0-2])[\/\-\.](?:0?[1-9]|1\d|2[0-8]))(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$/" );
        }
        /// <summary>
        /// 获取指定类型的验证正则表达式
        /// </summary>
        /// <param name="vt"></param>
        /// <returns></returns>
        public string GetValidationExpression( ValidationType vt )
        {
            string expr = _TypeValidationExpression[vt];

            if( expr == null ) return "";
            else return expr ;
        }

        /// <summary>
        /// 查找控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        protected virtual Control FindControl(Control control, string controlID)
        {
            Control control1 = control;
            Control control2 = null;
            if (control == control.Page) 
            {
                return control.FindControl(controlID);
            }
            while ((control2 == null) && (control1 != control.Page))
            {
                control1 = control1.NamingContainer;
                if (control1 == null)
                {
                    throw new ObjectMapException("不存在控件[" + controlID + "]", this);
                    //throw new HttpException(SR.GetString("DataBoundControlHelper_NoNamingContainer", new object[] { control.GetType().Name, control.ID }));
                }
                control2 = control1.FindControl(controlID);
            }

            if (control2 == null)
            {
                throw new ObjectMapException("不存在控件[" + controlID + "]", this);
            }

            return control2;
        }

        private Control _MapControl;
        /// <summary>
        /// 获取绑定到的控件 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public Control GetMapControl(Control control)
        {
            if (_MapControl == null)
                _MapControl = FindControl(control, ControlID);

            return _MapControl;
        }

        private string _ControlID;
        /// <summary>
        /// 控件ID
        /// </summary>
        /// 
        //[Editor(typeof(System.Web.UI.Design), typeof(System.Drawing.Design.UITypeEditor)), Category("Setting")]
        //[
        [TypeConverter(typeof(ControlIDConverter) )]
        public string ControlID
        {
            get
            {
                return _ControlID;
            }
            set
            {
                _ControlID = value;
            }
        }

        private string _PropertyName;
        /// <summary>
        /// 要绑定到的控件的属性
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _PropertyName;
            }
            set
            {
                _PropertyName = value;
            }
        }

        private string getDefaultPropertyName(Control ctl)
        {
            if (ctl is TextBox)
                return "Text";
            else if (ctl is CheckBox)
                return "Checked";
            else if (ctl is DropDownList)
                return "SelectedValue";
            else if (ctl is RadioButton)
                return "Checked";
            else if (ctl is Button)
                return "Text";
            else if (ctl is ListBox)
                return "SelectedValue";
            else if (ctl is Image)
                return "ImageUrl";
            else
                if (ctl is Label)
                    return "Text";
                return "";

        }

        private void checkPropertyName(Control ctl)
        {
            if (String.IsNullOrEmpty(PropertyName)) //若未指定PropertyName，则按控件类型获取默认PropertyName
            {
                PropertyName = this.getDefaultPropertyName(ctl);

                if (String.IsNullOrEmpty(PropertyName))
                    throw new ObjectMapException("未指定PropertyName", this);
            }
        }

        public override object Evaluate(HttpContext context, Control control)
        {
            Control ctl = this.GetMapControl(control);

            checkPropertyName(ctl);

            object obj = DataBinder.Eval(ctl, PropertyName);

            return obj;
        }

        public override void UpdateUI(object data, Control control)
        {
            Control ctl = GetMapControl(control);

            checkPropertyName(ctl);

            object v;

            if( String.IsNullOrEmpty( OutputFormatString ) )
                v = DataBinder.Eval(data, Name);
            else
                v = DataBinder.Eval( data, Name , OutputFormatString );            

            System.Reflection.PropertyInfo prop = ctl.GetType().GetProperty(PropertyName);

            if (prop == null)
                throw new ObjectMapException( String.Format( "控件[{0}]不存在[{1}]属性" , ctl.ID , this.PropertyName ) );

            if (v == null)
                prop.SetValue(ctl, null, null);
            else if (prop.PropertyType == typeof(string))
                prop.SetValue(ctl, v.ToString(), null);
            else
            {
                object typeValue = Convert.ChangeType(v, prop.PropertyType);

                prop.SetValue(ctl, typeValue, null) ;
            }
        }

        private bool _Required = false ;
        /// <summary>
        /// 是否必须输入
        /// </summary>
        public bool Required
        {
            get
            {
                return _Required;
            }
            set
            {
                _Required = value;
            }
        }

        private bool _AutoValid = true;
        /// <summary>
        /// 是否自动执行非空，长度，类型验证 
        /// </summary>
        public bool AutoValid
        {
            get
            {
                return _AutoValid;
            }
            set
            {
                _AutoValid = value;
            }
        }

        private string _ValidationExpression;
        /// <summary>
        /// 验证正则表达式,采用 / / 写法
        /// </summary>
        public string ValidationExpression
        {
            get
            {
                return _ValidationExpression;
            }
            set
            {
                _ValidationExpression = value;
            }
        }


        private int _MaxLength;
        /// <summary>
        /// 控件值长度
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _MaxLength;
            }
            set
            {
                _MaxLength = value;
            }
        }


        private string _ErrorMessage;
        /// <summary>
        /// 为空时信息
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        private string _ErrorTypeMessage;
        /// <summary>
        /// 数据格式错误时信息
        /// </summary>
        public string ErrorTypeMessage
        {
            get
            {
                return _ErrorTypeMessage;
            }
            set
            {
                _ErrorTypeMessage = value;
            }
        }

        private string _ErrorMaxLengthMessage;
        /// <summary>
        /// 数据格式错误时信息
        /// </summary>
        public string ErrorMaxLengthMessage
        {
            get
            {
                return _ErrorMaxLengthMessage;
            }
            set
            {
                _ErrorMaxLengthMessage = value;
            }
        }

        private string _ClientValidationFunction ;
        /// <summary>
        /// 客户端验证函数名 : bool valid( param )
        /// </summary>
        public string ClientValidationFunction
        {
            get
            {
                return _ClientValidationFunction;
            }
            set
            {
                _ClientValidationFunction = value;
            }
        }

        private ValidationType _ValidationType = ValidationType.None ;
        /// <summary>
        /// 数据格式错误时信息
        /// </summary>
        public ValidationType ValidationType
        {
            get
            {
                return _ValidationType;
            }
            set
            {
                _ValidationType = value;

                ValidationExpression = _TypeValidationExpression[value] ;
            }
        }

        private string _OutputFormatString;
        /// <summary>
        /// 输出格式化字符串
        /// </summary>
        public string OutputFormatString
        {
            get
            {
                return _OutputFormatString;
            }
            set
            {
                _OutputFormatString = value;
            }
        }

        
       
    }

  
}
