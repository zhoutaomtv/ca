
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

namespace CA.SharePoint.WebPartSkin
{
/*
 * 
<!--skin begin-->
<!--id:commonList-->
<!--description:-->

<!--header-->
<!--ref:commonList-->
<!--header-->

<!--body-->
body
<!--body 

<!--footer-->
footer
<!--footer-->

<!--skin end-->

 */
    //public enum SkinPart
    //{
    //    Header = 1 ,
    //    Body = 2 ,
    //    Footer = 3 ,
    //    Loading = 4
    //}

    /// <summary>
    /// 一个控件皮肤的配置元素
    /// </summary>
    [Serializable]
    public class SkinElement
    {
        private static Regex _IdReg = new Regex(@"<!--id:.*-->");

        private static char[] _TrimedChars = new char[] {  ' ' , '\n', '\r' };

        private static Regex _RefIdReg = new Regex(@"<!--ref:.*-->");

        private string GetTagInnerValue(string v) // 取<!-- -->中间的字符
        {
            string temp = v.Trim() ;

            return temp.Substring(4, temp.Length - 7 ); //
        }

        private string GetTagIncludeHtml(string html, string tag) // 取<!-- -->html<!-- -->包含的html
        {
            string sep = "<!--" + tag + "-->";

            string trimed = html.Trim( _TrimedChars ) ;

            return trimed.Substring(sep.Length, trimed.Length - 2 * sep.Length);
        }

        string GetSkinID(string html)
        {
            Match m = _IdReg.Match(html);

            string tagValue = GetTagInnerValue( m.Value ) ;

            return tagValue.Split(':')[1];
        }

        public SkinElement() { }

        public SkinElement(string temp )
        {
            _SkinHtml = temp;

            ID = this.GetSkinID(temp);
            //SkinNode = node;

            //if (node.Attributes["BaseSkin"] != null)
            //    BaseSkin = node.Attributes["BaseSkin"].Value.ToLower();

            Body = GetPartSkin(temp, "body");
            //EmptyItemSkin = GetPartSkin(node, "EmptyItem");

            Header = GetPartSkin(temp, "header");
            Footer = GetPartSkin(temp, "footer");
            Loading = GetPartSkin(temp, "loading");
        }

        private string _SkinHtml;

        public string SkinHtml
        {
            get { return _SkinHtml; }
            set { _SkinHtml = value; }
        }

        /// <summary>
        /// 皮肤id
        /// </summary>
        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        private readonly string _BaseSkin;
        public string BaseSkin
        {
            get { return _BaseSkin; }
        } 
 

        /// <summary>
        /// 获取部分皮肤 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetPartSkin(string skinHtml , string name)
        {
            string reg = "<!--"+ name +"-->" + @"[\s\S]*" + "<!--"+ name +"-->" ;

            Regex _IdReg = new Regex( reg);

            Match m = _IdReg.Match(skinHtml);

            if (m == null|| !m.Success) return "";

            //ref id

            return GetTagIncludeHtml(m.Value , name );
        }

        private string GetRefID(string html)
        {
            Match m = _IdReg.Match(html);

            if (m == null)
                return "";

            string tagValue = GetTagInnerValue(m.Value);

            return tagValue.Split(':')[1];
        }

        /// <summary>
        /// 项目 
        /// </summary>
        private string _Body;

        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// 头 
        /// </summary>
        private string _Header;

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        /// <summary>
        /// 尾 
        /// </summary>
        private string _Footer;

        public string Footer
        {
            get { return _Footer; }
            set { _Footer = value; }
        }

        /// <summary>
        /// 加载中 
        /// </summary>
        private string _Loading;

        public string Loading
        {
            get { return _Loading; }
            set { _Loading = value; }
        }

        /// <summary>
        /// 空项
        /// </summary>
        private string _Empty;

        public string Empty
        {
            get { return _Empty; }
            set { _Empty = value; }
        }

        //public override string ToString()
        //{
        //    return CA.Common.ObjectUtil.ConvertToString(this);
        //}

    }
}
