
using System;
using System.Collections.Generic;
using System.Text;
using CA.SharePoint.WebPartSkin;

using Microsoft.SharePoint;

namespace CA.SharePoint.WebPartSkin
{
    /// <summary>
    /// 列表值（属性）提供程序
    /// </summary>
    class SPListValueProvider : CA.SharePoint.WebPartSkin.ITagValueProvider  
    {
        private SPList _List ;

        public SPListValueProvider(SPList list )
        {
            _List = list ;
        }

        public string GetValue( ReplaceTag tag )
        {
            string v = "";

            if (_List == null)
                return v;

            switch (tag.Name.ToLower())
            {
                case "title" :
                    return _List.Title;
                case "defaultviewurl":
                    return _List.DefaultViewUrl;
                case "weburl":
                    return _List.ParentWebUrl.EndsWith("/") ? _List.ParentWebUrl : _List.ParentWebUrl + "/" ;
                case "description":
                    return _List.Description;

                default :
                    return "";
            }
        }
    }
}
