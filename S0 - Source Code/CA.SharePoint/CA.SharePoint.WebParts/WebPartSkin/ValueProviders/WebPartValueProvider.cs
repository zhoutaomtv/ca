
using System;
using System.Collections.Generic;
using System.Text;
using CA.SharePoint.WebPartSkin;

using Microsoft.SharePoint;

namespace CA.SharePoint.WebPartSkin
{
    /// <summary>
    /// webpart本身的属性值提供程序
    /// </summary>
    class WebPartValueProvider : CA.SharePoint.WebPartSkin.ITagValueProvider  
    {
        private BaseSPWebPart _wp;

        public WebPartValueProvider(BaseSPWebPart wp )
        {
            _wp = wp ;
        }

        public string GetValue( ReplaceTag tag )
        {
            string v = "";

            switch (tag.Name.ToLower())
            {
                case "title" :
                    return _wp.Title ;
                case "titleurl":
                    return _wp.TitleUrl ;
                case "resourcepath" :
                    return _wp.ResourcePath ;
                case "tooltip":
                    return _wp.ToolTip;
                case "uniqueid":
                    return _wp.UniqueID;
                case "clientid":
                    return _wp.ClientID;
                case "connecterrormessage":
                    return _wp.ConnectErrorMessage;
                case "cssclass":
                    return _wp.CssClass;
                case "description":
                    return _wp.Description;
                case "displaytitle":
                    return _wp.DisplayTitle;
                //case "rowlimit":
                //    return _wp.RowLimit.ToString() ;
                case "siteurl":
                    return _wp.SiteUrl;
                case "subtitle":
                    return _wp.Subtitle;
                case "width":
                    return "" + _wp.Width;
                case "height":
                    return "" + _wp.Height;
                case "helpurl":
                    return _wp.HelpUrl;
                //case "listname":
                //    return _wp.ListName;
                case "titleiconimageurl":
                    return _wp.TitleIconImageUrl;
                //case "imageurl":
                //    return _wp.TitleIconImageUrl;
                case "wrap":
                    return _wp.Wrap ? "" : "nowrap";                   
                default :
                    return "";
            }

            //if (tag.Name == "viewurl")
            //{
            //    v = _Item.Url;
            //}
            //else
            //{
            //    try
            //    {
            //        object o = _Item[tag.Name];

            //        v = o == null ? "" : o.ToString();
            //    }
            //    catch (System.ArgumentException ex)
            //    {
            //        throw new Exception("标签 " + tag.TagValue + " 指定的字段 " + tag.Name + " 不存在,请注意大小写", ex);
            //    }

            //    if (tag.MaxLength > 0 && v.Length > tag.MaxLength)
            //    {
            //        v = v.Substring(0, tag.MaxLength) + "...";
            //    }
            //}

            return v;
        }
    }
}
