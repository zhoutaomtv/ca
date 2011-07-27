using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace CA.SharePoint
{
    /// <summary>
    /// 给列表项添加菜单
    /// </summary>
    public class ListItemLinkMenuWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {        

        private string _NavigationUrl = "/_layouts/CA/soundmail.aspx";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        public string NavigationUrl
        {
            get { return _NavigationUrl; }
            set { _NavigationUrl = value; }
        }

        

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //base.Render(writer);
            writer.Write("\n<script language=\"javascript\">\n");
            writer.Write("function Custom_AddDocLibMenuItems(m, ctx){\n");
            writer.Write("var strDisplayText = '"+ this.Title +"';    \n");     // 菜单项的显示文字

            writer.Write("var strAction=\"window.location='" + this.NavigationUrl + "?ListId='+ ctx.listName +'&ItemId='+currentItemID;\" ; \n");        // 菜单项的实际功能

            writer.Write("var strImagePath = '';\n");        // 菜单项的显示图片 

            writer.Write("CAMOpt(m, strDisplayText, strAction, strImagePath);\n");

            // 添加一个分隔栏
            writer.Write("CAMSep(m);\n");


            // 如果为true，不显示系统默认的菜单项
            // 如果为fasle,显示系统默认的菜单项

            writer.Write("return false;}\n");

            writer.Write("</script>\n");
        }


    }

   
}
