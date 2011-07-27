using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace CA.SharePoint
{
    public class RedirectUrlWebpart : BaseSPWebPart
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

        protected override void OnLoad(EventArgs e)
        {
            string file = SPContext.Current.File.Url;
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            string fileExt = file.Substring(file.LastIndexOf(".") + 1);

            string fileUrl = SPContext.Current.Web.Url + "/" + file;

            if (fileExt.ToUpper() == "ASPX" || fileExt.ToUpper() == "ASP")
            {
                if (_isEnableRedirect)
                {
                    HttpContext.Current.Response.Redirect(fileUrl);
                    return;
                }
            }

            base.OnLoad(e);

        }
    }
}
