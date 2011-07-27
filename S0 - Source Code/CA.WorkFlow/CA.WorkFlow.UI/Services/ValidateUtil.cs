using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI
{

    public enum SmartFieldValueType
    {
        /// <summary>
        /// 文本
        /// </summary>
        String = 20,

        /// <summary>
        /// 整数 
        /// </summary>
        Integer = 2,

        /// <summary>
        /// 小数
        /// </summary>
        Double = 3,

        /// <summary>
        /// 货币
        /// </summary>
        Currency = 4,

        //Date ,

        //Time ,


        //时间
        DateTime = 5,

        /// <summary>
        /// email
        /// </summary>
        Email = 6,

        /// <summary>
        /// 电话
        /// </summary>
        Phone = 7,

        Mobile = 8,

        //Url,

        /// <summary>
        /// 身份证
        /// </summary>
        IdCard = 9,

        /// <summary>
        /// 数字
        /// </summary>
        Number = 10,

        //Zip,

        //QQ,


        /// <summary>
        /// 英文字符
        /// </summary>
        English = 11,

        /// <summary>
        /// 
        /// </summary>
        Url = 12,

        // Chinese = 12,     

        DocDocument = 13

    }

    /// <summary>
    /// 校验字段接口
    /// </summary>
    public interface IRequiredValidateField : IValidator
    {
        bool Required { get; set; }

        string ValidFunction { get; set; }

        string ValidationGroup { get; set; }       
    }

    /// <summary>
    /// 校验字段接口
    /// </summary>
    public interface IValidateField : IValidator, IRequiredValidateField
    {

        string ValidationExpression { get; set; }

        int MaxLength { set; get; }

        string ErrorDataTypeMessage { get; set; }

        string ErrorMaxLengthMessage { get; set; }

        SPControlMode ControlMode { get; set; }

    }

    public class ValidateUtil
    {
        public static void EnableValid(Page page, IValidateField valField)
        {        

            if (valField.ControlMode == SPControlMode.Display)
                return;


            if (!valField.Required && String.IsNullOrEmpty(valField.ValidFunction) && String.IsNullOrEmpty(valField.ValidationExpression))
                return;

            Control ctl = (Control)valField;

            string validatorId = ctl.ClientID + "_validator";

            string evaluationfunction = valField.ValidFunction;
            if (String.IsNullOrEmpty(evaluationfunction))
                evaluationfunction = "SmartForm_Valid";

            page.ClientScript.RegisterExpandoAttribute(validatorId, "required", valField.Required.ToString(), false);

            if (!String.IsNullOrEmpty(valField.ValidationGroup))
                page.ClientScript.RegisterExpandoAttribute(validatorId, "validationGroup", valField.ValidationGroup.ToString(), false);

            page.ClientScript.RegisterExpandoAttribute(validatorId, "evaluationfunction", evaluationfunction, false);

            page.ClientScript.RegisterExpandoAttribute(validatorId, "customfunction", evaluationfunction, false);

            page.ClientScript.RegisterExpandoAttribute(validatorId, "controltovalidate", ctl.ClientID, false);


            if (!String.IsNullOrEmpty(valField.ValidationExpression))
                page.ClientScript.RegisterExpandoAttribute(validatorId, "validationexpression", valField.ValidationExpression.ToString(), false);

            if (String.IsNullOrEmpty(valField.ErrorMessage))
                valField.ErrorMessage = "此字段必填！";

            if (String.IsNullOrEmpty(valField.ErrorMaxLengthMessage))
                valField.ErrorMaxLengthMessage = "此字段超过允许长度[" + valField.MaxLength + "]！";

            if (String.IsNullOrEmpty(valField.ErrorDataTypeMessage))
                valField.ErrorDataTypeMessage = "此字段数据类型不合法！";

            page.ClientScript.RegisterExpandoAttribute(validatorId, "errorMessage", "" + valField.ErrorMessage, false);
            page.ClientScript.RegisterExpandoAttribute(validatorId, "errorMaxLengthMessage", "" + valField.ErrorMaxLengthMessage, false);
            page.ClientScript.RegisterExpandoAttribute(validatorId, "errorDataTypeMessage", "" + valField.ErrorDataTypeMessage, false);

            if (valField.MaxLength > 0)
                page.ClientScript.RegisterExpandoAttribute(validatorId, "maxLength", "" + valField.MaxLength, false);


            string text1 = "document.getElementById(\"" + validatorId + "\")";
            page.ClientScript.RegisterArrayDeclaration("Page_Validators", text1);

            Type baseValidatorType = typeof(System.Web.UI.WebControls.BaseValidator);

            if (!page.ClientScript.IsClientScriptBlockRegistered(baseValidatorType, "ValidatorIncludeScript"))
            {
                page.ClientScript.RegisterClientScriptResource(baseValidatorType, "WebUIValidation.js");
                page.ClientScript.RegisterStartupScript(baseValidatorType, "ValidatorIncludeScript", "\r\n<script type=\"text/javascript\">\r\n<!--\r\nvar Page_ValidationActive = false;\r\nif (typeof(ValidatorOnLoad) == \"function\") {\r\n    ValidatorOnLoad();\r\n}\r\n\r\nfunction ValidatorOnSubmit() {\r\n    if (Page_ValidationActive) {\r\n        return ValidatorCommonOnSubmit();\r\n    }\r\n    else {\r\n        return true;\r\n    }\r\n}\r\n// -->\r\n</script>\r\n        ");
                page.ClientScript.RegisterOnSubmitStatement(baseValidatorType, "ValidatorOnSubmit", "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;");
            }
        }
    } 
}
