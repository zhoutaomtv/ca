using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Drawing;
using System.ComponentModel;

namespace CA.SharePoint
{
    public class FlashWebPart : BaseSPWebPart // DragableWebPart
    {
        public FlashWebPart()
        {
        }

        private string _FilePath = "";
      
       //[Category("自定义控件设置"), 
       // Description("设置flash路径"), 
       // Browsable(true), 
       // Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        public string FilePath {
            get { return _FilePath; }
            set {_FilePath = value; }
        }

        //[Description("设置flash背景颜色,默认黑色"), Editor(typeof(ColorUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //public string BgColor {
        //    get { return _strBgColor; }
        //    set { _strBgColor = value; }
        //}

        private Unit _Width = new Unit ("200px");
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        public Unit FlashWidth
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private Unit _Height = new Unit ("200px");
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        public Unit FlashHeight
        {
            get { return _Height; }
            set { _Height = value; }
        }
            
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (String.IsNullOrEmpty(_FilePath))
            {
                writer.Write("FilePath property not defined.");
                return;
            }

            string realFilePath = base.ResolveUrl(this.FilePath);

            writer.WriteLine("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' ");
            writer.WriteLine("codebase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=5,0,2,0 Width = " + FlashWidth.Value.ToString() + " Height = " + FlashHeight.Value.ToString() + " > ");

            writer.WriteLine("<param name=movie value=" + realFilePath + ">");
            writer.WriteLine("<param name=quality value=high> ");

            if (!this.BackColor.IsEmpty)
                writer.WriteLine("<param name=BGCOLOR value=" + ColorTranslator.ToHtml(this.BackColor) + ">");

            writer.WriteLine("<param name=SCALE value=showall> ");

            writer.WriteLine("<embed src=" + realFilePath + " "); //= high 

            writer.WriteLine("pluginspage=http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash type=application/x-shockwave-flash ");
            //writer.WriteLine("Width = '" + Width.Value.ToString() + "' ");
            //writer.WriteLine("Height = '" + Height.Value.ToString() + "' ");

            writer.WriteLine("Width = '" + FlashWidth + "' ");
            writer.WriteLine("Height = '" + FlashHeight + "' ");

            if( !this.BackColor.IsEmpty )
                writer.WriteLine("bgcolor= " + ColorTranslator.ToHtml(this.BackColor) + " ");

            writer.WriteLine("scale= showall></embed></object>");

            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //writer.Write(sb.ToString());
            //writer.RenderEndTag();
                 
        }

    }
}
