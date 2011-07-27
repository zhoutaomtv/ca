using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;


namespace CA.SharePoint
{
    /// <summary>
    /// 资源访问类，访问App_GlobalResources 下的mcswp.resx资源文件
    /// </summary>
    public static class WPResource
    {
        const string resource = "cawp";
        //此处为资源文件名，这些资源文件放在App_GlobalResources,以wss.resx，wss.zh-CN.resx的规则命名
        //wss为系统默认的资源文件，你可以用myresource.resx,myresource.zh_CN.resx来命名

        /// <summary>
        /// 获取资源字符串的值
        /// </summary>
        /// <param name="key">资源字符串key</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            try
            {
                string value = HttpContext.GetGlobalResourceObject(resource, key) as string;

                if (value == null || value == "")
                    return key;
                else
                    return value;
            }
            catch (Exception ex)
            {
                throw new SPException("access resource file [" + resource + "] error,please confirm files in App_GlobalResources.", ex);
            }
        }

        /// <summary>
        /// 获取资源字符串的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">key不存在时的默认值</param>
        /// <returns></returns>
        public static string GetString(string key,string defaultValue)
        {
            try
            {
                string value = HttpContext.GetGlobalResourceObject(resource, key) as string;

                if (value == null || value == "")
                    return String.IsNullOrEmpty( defaultValue ) ? key : defaultValue  ;
                else
                    return value;
            }
            catch (Exception ex)
            {
                throw new SPException("access resource file [" + resource + "] error,please confirm files in App_GlobalResources.", ex);
            }
        }

        public static string GetString(string key , params string[] args )
        { 
            string value = HttpContext.GetGlobalResourceObject(resource, key) as string;

            if (value == null || value == "")
                return key;
            else
                return String.Format( value , args );
        }
    }

    /// <summary>
    /// 属性分类，从资源中获取
    /// </summary>
    public class ResCategoryAttribute : CategoryAttribute
    {
        public ResCategoryAttribute(string key)
            : base(key)
        { }

         private string _DefaultValue;
        public ResCategoryAttribute(string key, string defaultValue)
            : base(key)
        {
            _DefaultValue = defaultValue;
        }

        protected override string GetLocalizedString(string value)
        {
            return WPResource.GetString(value,_DefaultValue);
        }
    }

    /// <summary>
    /// 属性显示名，从资源中获取
    /// </summary>
    public class ResWebDisplayNameAttribute : WebDisplayNameAttribute
    {
        public ResWebDisplayNameAttribute(string key)
            : base(key)
        { }

        private string _DefaultValue;
        public ResWebDisplayNameAttribute(string key,string defaultValue)
            : base(key)
        {
            _DefaultValue = defaultValue;
        }


        public override string DisplayName
        {
            get
            {
                return WPResource.GetString(base.DisplayName,_DefaultValue);
            }
        }
    }


    public class ResConnectionProviderAttribute : ConnectionProviderAttribute
    {
        public ResConnectionProviderAttribute(string name) : base(name)
        {
        }

        public ResConnectionProviderAttribute(string name,string id) : base(name,id)            
        {
        }

        public override string DisplayName
        {
            get
            {
                return WPResource.GetString(base.DisplayName);
            }
        }
    }

    public class ResConnectionConsumerAttribute : ConnectionConsumerAttribute
    {
        public ResConnectionConsumerAttribute(string name) : base(name)
        {
        }

        public ResConnectionConsumerAttribute(string name, string id)
            : base(name, id)            
        {
        }

        public override string DisplayName
        {
            get
            {
                return WPResource.GetString(base.DisplayName);
            }
        }
    }
}
