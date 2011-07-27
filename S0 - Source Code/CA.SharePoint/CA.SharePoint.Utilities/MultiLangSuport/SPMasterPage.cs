using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CA.SharePoint
{
    public class SPMasterPage :  System.Web.UI.MasterPage
    {
        protected override void  OnInit(System.EventArgs e)
        {
 	         base.OnInit(e);

            // this._ThemeImagePath = this.ResolveUrl("~/App_Themes/" + ThemeManager.GetInstance().CurrentTheme + "/Images");

        }

 

        private string _ThemeImagePath;
        /// <summary>
        /// 主题图片文件夹路径
        /// </summary>
        protected string ImagePath
        {
            get
            {
                return _ThemeImagePath;
            }
        }
    }
}
