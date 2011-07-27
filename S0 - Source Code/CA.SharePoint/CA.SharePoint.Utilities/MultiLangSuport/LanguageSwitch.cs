using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace CA.SharePoint
{
    public class LanguageSwitch : LinkButton
    {
        public LanguageSwitch()
        {
            this.Command += new CommandEventHandler(LanguageSwitcher_Command);
            this.CssClass = "ms-globallinks";
        }

        const string Chinese_Lan_Name = "zh-cn";
        const string English_Lan_Name = "en-us";

        private UICultureManager CultureManager = UICultureManager.CurrentInstance;

        void LanguageSwitcher_Command(object sender, CommandEventArgs e)
        {
            string name = CultureManager.CurrentUICultureName ;

            if (name.Equals(Chinese_Lan_Name, StringComparison.CurrentCultureIgnoreCase))
            {
                CultureManager.SetThreadCulture(English_Lan_Name);
            }
            else
            {
                CultureManager.SetThreadCulture(Chinese_Lan_Name);
            }

            Page.Response.Redirect(Page.Request.RawUrl);
        }

        protected override void OnInit(EventArgs e)
        {
            CultureManager.SetThreadCulture();

            this.Page.UICulture = CultureManager.CurrentUICultureName ;

            //this.Page.ResponseRequest.Cookies.Add(new HttpCookie("mcs2", "mcs2"));  
            this.Page.PreRender += new EventHandler(Page_PreRender);
            this.Page.Load += new EventHandler(Page_Load);
            base.OnInit(e);                        
        }

        void Page_Load(object sender, EventArgs e)
        {
            CultureManager.SetThreadCulture();
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            CultureManager.SetThreadCulture();
        }
                

        protected override void OnPreRender(EventArgs e)
        {            
            string name = CultureManager.CurrentUICultureName;

            if (name.Equals(Chinese_Lan_Name, StringComparison.CurrentCultureIgnoreCase))
            {
                this.Text = "English";
                //this.CommandArgument = "en-us";
            }
            else
            {
                this.Text = "ÖÐÎÄ";
                //this.CommandArgument = "zh-cn";
            }

            base.OnPreRender(e);
        }

    }
}
//zh-cn,en-us