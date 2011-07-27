using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Web;

namespace CA.SharePoint
{
    public class SPHttpApplication : Microsoft.SharePoint.ApplicationRuntime.SPHttpApplication
    {
        public override void Init()
        {
            base.Init();

            //this.BeginRequest += new EventHandler(SPHttpApplication_BeginRequest);

            this.AcquireRequestState += new EventHandler(SPHttpApplication_AcquireRequestState);

            
        }

        void SPHttpApplication_AcquireRequestState(object sender, EventArgs e)
        {
            //System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;

            //if (Request.UserLanguages == null || Request.UserLanguages.Length == 0) return;

            //string lan = Request.UserLanguages[0].Split(';')[0];

            //System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(lan);

            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            UICultureManager.CurrentInstance.SetThreadCulture();
        }

        //void SPHttpApplication_BeginRequest(object sender, EventArgs e)
        //{
        //    System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request ;

        //    if (Request.UserLanguages == null || Request.UserLanguages.Length == 0) return;

        //    string lan = Request.UserLanguages[0].Split(';')[0];
            
        //    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo (lan);

        //    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

        //   // Response.Write(lan);

        //    try
        //    {
        //        Microsoft.SharePoint.SPContext.Current.Web.Locale = culture;
        //    }
        //    catch { }
        //}
    }
}
 

