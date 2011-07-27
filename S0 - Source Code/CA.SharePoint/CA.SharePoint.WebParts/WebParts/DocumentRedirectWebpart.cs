using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace CA.SharePoint
{
    public class DocumentRedirectWebpart :WebPart
    {
        private bool _isEnableRedirect = false;

        /// <summary>
        /// 是否允许页面转向
        /// </summary>
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        public bool EnableRedirect
        {
            get { return _isEnableRedirect; }
            set { _isEnableRedirect = value; }
        }

        protected override void OnInit(EventArgs e)
        {          
            base.OnInit(e);            
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_isEnableRedirect)
            {
                string url = GetCurrentUserDeptDocument();
              
                this.Page.Response.Redirect(url);    
               
            }
            base.OnLoad(e);
        }

        private string GetCurrentUserDeptDocument()
        { 
            //get user dept

            return "/documentcenter/PublicDocuments/Public";

        }
    }
}
