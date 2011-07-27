using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace CA.SharePoint
{
    /// <summary>
    /// CA.SharePoint.LanguageHelper. SetThreadCulture();
    /// </summary>
    public class UICultureManager
    {
        const string Culture_Cookie_Key = "MCS_Culture_Cookie_Key";

        const string Chinese_Lan_Name = "zh-cn";
        const string English_Lan_Name = "en-us";

        const string Context_Instance_Key = "UICultureManager_CurrentInstance";

        private string _CurrentUICultureName;

        private UICultureManager()
        {
            _CurrentUICultureName = this.GetCurrentUICultureName();
        }

        public static UICultureManager CurrentInstance
        {
            get
            {
                if (System.Web.HttpContext.Current.Items[Context_Instance_Key] == null)
                {
                    UICultureManager cm = new UICultureManager();

                    System.Web.HttpContext.Current.Items.Add(Context_Instance_Key, cm);

                    return cm;
                }
                else
                {
                    return (UICultureManager)System.Web.HttpContext.Current.Items[Context_Instance_Key];
                }
            }
        }

        public string CurrentUICultureName
        {
            get
            {
                return _CurrentUICultureName;
            }
        }

        public void SetThreadCulture()
        {
            //System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;

            //HttpCookie c = Request.Cookies[CultureInfo_Name];

            //string cultureName = "";

            //if (c != null) //如果cookie中设置了语言
            //{
            //    cultureName = c.Value;
            //}
            //else //否则，从浏览器设置取,并记录到cookie
            //{
            //    if (Request.UserLanguages == null || Request.UserLanguages.Length == 0) return;

            //    cultureName = Request.UserLanguages[0].Split(';')[0];

            //    AddCookie(cultureName);
            //}

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(_CurrentUICultureName);

            System.Threading.Thread.CurrentThread.CurrentUICulture = culture ;

            SPContext.Current.Web.Locale = culture ;
            SPContext.Current.Web.CurrencyLocaleID = culture.LCID;
            SPContext.Current.Web.Properties["vti_defaultlanguage"] = _CurrentUICultureName;
            //SPContext.Current.Web.AllowUnsafeUpdates = true;
           // SPContext.Current.Web.Update();

            SPUtility.SetThreadCulture(culture,culture);

        }

        private string GetCurrentUICultureName()
        {
            System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;

            HttpCookie c = Request.Cookies[Culture_Cookie_Key];

            string cultureName = "";

            if (c != null) //如果cookie中设置了语言
            {
                cultureName = c.Value;
            }
            else //否则，从浏览器设置取
            {
                if (Request.UserLanguages == null || Request.UserLanguages.Length == 0) return English_Lan_Name;

                cultureName = Request.UserLanguages[0].Split(';')[0];
            }

            return cultureName ;
        }

        private void AddCookie(string cultureName)
        {
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

            if (Response.Cookies[Culture_Cookie_Key] != null)
            {
                Response.Cookies[Culture_Cookie_Key].Value = cultureName;
            }
            else
            {
                HttpCookie c = new HttpCookie(Culture_Cookie_Key);
                c.Value = cultureName;

                Response.Cookies.Add(c);
            }
        }

        public void SetThreadCulture( string cultureName )
        {
            _CurrentUICultureName = cultureName;

            AddCookie(cultureName);

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(cultureName);

            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
