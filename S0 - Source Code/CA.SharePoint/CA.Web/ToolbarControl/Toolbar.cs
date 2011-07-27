using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.Web.ToolbarControl
{

    /// <summary>
    /// 服务器事件参数
    /// </summary>
    public class ToolItemClickEventArgs : EventArgs
    {
        internal ToolItemClickEventArgs(string itemValue)
        {
            ItemValue = itemValue;
        }

        public readonly string ItemValue ;

    }

    /// <summary>
    /// 事件代理
    /// </summary>
    public delegate void ToolItemClickEventHandler(object sender, ToolItemClickEventArgs e);


    [DefaultEvent("ToolItemClick"),
    ToolboxData("<{0}:Toolbar runat=server></{0}:Toolbar>")]
    [ParseChildren(true, "Items")]
    //[Designer(typeof(ServerEventDesigner))]
    public class Toolbar : System.Web.UI.WebControls.WebControl, System.Web.UI.IPostBackEventHandler
    {

        private List<ToolItem> _Items = new List<ToolItem>();
        public List<ToolItem> Items
        {
            get
            {
               return _Items ;
            }
        }

        private Unit _ItemWidth;
        public Unit ItemWidth
        {
            set
            {
                _ItemWidth = value;
            }
            get
            {
                return _ItemWidth;
            }
        }

        public event ToolItemClickEventHandler ToolItemClick ;


        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);

        //    Page.RegisterRequiresRaiseEvent(this);
        //}


    //    <table width="100%" border="0" cellpadding="0" cellspacing="0">
    //                    <tr>
    //                        <td class="HeadLine" onmouseover="OverStyle(this);" onmouseout="UnOverStyle(this);">
    //                            <img alt="Save" src="../Images/SaveIcon.gif" class="ImageAlign" />&nbsp;保存</td>
    //                        <td class="HeadLine" onmouseover="OverStyle(this);" onmouseout="UnOverStyle(this);" style="width: 61px">
    //                            <img alt="Del" src="../Images/DelIcon.gif" class="ImageAlign" />&nbsp;删除</td>
    //                        <td class="HeadLine" onmouseover="OverStyle(this);" onmouseout="UnOverStyle(this);">
    //                            <img alt="Print" src="../Images/PrintIcon.gif" class="ImageAlign" />&nbsp;打印</td>
    //                        <td class="HeadLineRest">
    //                            &nbsp;</td>
    //                    </tr>
    //</table>

        protected override void Render(HtmlTextWriter writer)
        {

            writer.Write("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr>\n");

            string widthHtml = "";
            if (!_ItemWidth.IsEmpty)
                widthHtml = "width='" + _ItemWidth.ToString() + "'";

            foreach (ToolItem item in this._Items)
            {
                if (!item.Visible) continue;

                if (item.NavigateUrl != "")
                {
                    writer.Write("\n<td  class=\"ToolItem\" " + widthHtml + " onclick=\"window.location='"+item.NavigateUrl+"'\" onmouseover=\"ToolItemOverStyle(this);\" onmouseout=\"ToolItemUnOverStyle(this);\" nowrap>");
                }
                else
                {

                    if (item.OnClientClick != "")
                    {
                        writer.Write("\n<td  class=\"ToolItem\" " + widthHtml + " onclick=\"if( " + item.OnClientClick + " ){" + Page.ClientScript.GetPostBackEventReference(this, item.Value) + "}\" onmouseover=\"ToolItemOverStyle(this);\" onmouseout=\"ToolItemUnOverStyle(this);\" nowrap>");
                    }
                    else
                    {
                        writer.Write("\n<td class=\"ToolItem\" " + widthHtml + " onclick=\"" + Page.ClientScript.GetPostBackEventReference(this, item.Value) + "\" onmouseover=\"ToolItemOverStyle(this);\" onmouseout=\"ToolItemUnOverStyle(this);\" nowrap>");
                    }
                }

              //  writer.Write("\n<td class=\"HeadLine\" onmouseover=\"OverStyle(this);\" onmouseout=\"UnOverStyle(this);\">");
				writer.Write("<table title=\"" + item.ToolTip + "\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr>");
				if (item.ImageUrl != "")
				{
					writer.Write("<td><img src=\""+item.ImageUrl+"\" border='0' class=\"ImageAlign\" /></td>");
					writer.Write("<td style=\"width:2px;\"><img alt=\"\" src=\"\" border=\"0\" width=\"1\" height=\"1\"></td>");
				}
				writer.Write("<td>");
                writer.Write( item.Text );
				writer.Write("</td>");
				writer.Write("</tr></table>");
                writer.Write("</td>");
            }

            writer.Write("\n<td class=\"ToolItemEmpty\">&nbsp;</td></tr></table>");
             
        }


        #region IPostBackEventHandler 成员

        public void RaisePostBackEvent(string eventArgument)
        {
            if (ToolItemClick != null)
            {
                ToolItemClickEventArgs args = new ToolItemClickEventArgs(eventArgument);
                ToolItemClick(this, args);
            }
        }

        #endregion
    }
}
