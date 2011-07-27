using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace CA.SharePoint.WebPartSkin
{
    /// <summary>
    /// 管理替换标签
    /// </summary>
    public class ReplaceTagManager
    {
        private static readonly ReplaceTagManager _Instance = new ReplaceTagManager();

        public static ReplaceTagManager GetInstance()
        {
            //return new ReplaceTagManager();

            return _Instance;
        }

        private static Regex _reg = new Regex(@"\[\[.+?\]\]");

        private Hashtable _CachedSkinTags = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">皮肤的唯一键，作为缓存数据的主键</param>
        /// <param name="html">皮肤字符串</param>
        /// <returns></returns>
        public IList<ReplaceTag> GetReplaceTags(string key, string html)
        {
            //if (_CachedSkinTags.ContainsKey(key))
            //    return (IList<ReplaceTag>)_CachedSkinTags[key];

            IList<ReplaceTag> tags = new List<ReplaceTag>();

            MatchCollection mc = _reg.Matches(html);

            foreach (Match m in mc)
            {
                ReplaceTag tag = new ReplaceTag(m.Value.Trim());

                tags.Add(tag);
            }

            //try
            //{
            //    _CachedSkinTags.Add(key, tags);
            //}
            //catch { }

            return tags;
        }

    }

    //[[spfield:title,dtype:datetime,format:3456,maxlength:10]]
    public class ReplaceTag
    {
        private static char[] _pattern = new char[] { '[', ']' };

        public ReplaceTag( string tag )
        {
            TagValue = tag;

            string temp = tag.Trim(_pattern);

            string[] arr = temp.Split( ',' );

            string[] firstSet = arr[0].Split(':');

            if (firstSet.Length > 1)
            {
                this.ValueProvider = firstSet[0];
                this.Name = firstSet[1];
            }

            foreach (string s in arr)
            {
                string[] arr2 = s.Split( ':' );

                if( arr2.Length > 1 )
                    FormatSet.Add(arr2[0].ToLower(), arr2[1]);
                else
                    FormatSet.Add(arr2[0].ToLower(), "" );
            }

            if( FormatSet.ContainsKey("format") )
                this.Format = FormatSet["format"];

            if (FormatSet.ContainsKey("maxlength"))
                this.MaxLength = Convert.ToInt32(FormatSet["maxlength"]);

            if (FormatSet.ContainsKey("dtype"))
                this.DataType = FormatSet["dtype"];

            
        }

        public readonly IDictionary<string, string> FormatSet = new  Dictionary<string,string>();

        public readonly string ValueProvider;

        public readonly string Name  ;

        public readonly string Format;

        public readonly int MaxLength = -1 ;

        public readonly string TagValue;

        public readonly string DataType ;

        public string FormatValue(object v)
        {
            if (v == null)
            {
                return "";
            }
            else if (this.DataType == "datetime")
            {
                if (String.IsNullOrEmpty(Format))
                    return v.ToString();
                else
                    return Convert.ToDateTime(v).ToString(Format);
            }
            else if (this.DataType == "spuser")
            {
                string[] arr = v.ToString().Split( new string[] { ";#" } , StringSplitOptions.None );
                if( arr.Length == 1 )
                    return arr[0];
                else
                    return arr[1];
            }
            else
            {
                string sv = v.ToString();

                if (this.MaxLength >= 0 && sv.Length > this.MaxLength)
                {
                    return sv.Substring(0, this.MaxLength) + "...";
                }
                else
                {
                    return sv;
                }
            }
        }

    }
}
