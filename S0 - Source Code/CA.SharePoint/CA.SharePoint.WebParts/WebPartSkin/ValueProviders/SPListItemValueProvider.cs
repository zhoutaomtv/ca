
using System;
using System.Collections.Generic;
using System.Text;
using CA.SharePoint.WebPartSkin;

using Microsoft.SharePoint;

namespace CA.SharePoint.WebPartSkin
{
    /// <summary>
    /// 列表项值提供程序
    /// </summary>
    class SPListItemValueProvider : CA.SharePoint.WebPartSkin.ITagValueProvider  
    {
        private SPListItem _Item;
        private string _DispFormUrl ;
        private int _RowNum;
        public SPListItemValueProvider(SPListItem item,int rowNum)
        {
            _Item = item;
            _RowNum = rowNum;
        }

        public string GetValue( ReplaceTag tag )
        {
            string v = "";

            if (tag.Name == "dispform")
            {
                if (_DispFormUrl == null)
                    _DispFormUrl = this.GetDispFormUrl(_Item["EncodedAbsUrl"].ToString());  //(_Item.ParentList.DefaultViewUrl);

                v = _DispFormUrl + _Item.ID ;
            }
            else if (tag.Name == "dispform2")
            {
                if (_DispFormUrl == null)
                    _DispFormUrl = this.GetDispFormUrl2(_Item["EncodedAbsUrl"].ToString());  //(_Item.ParentList.DefaultViewUrl);

                v = _DispFormUrl + _Item.ID;
            }
            else if (tag.Name == "rownumber")
            {
                v = _RowNum.ToString();
            }
            else
            {
                try
                {
                    object o = _Item[tag.Name];

                    return tag.FormatValue(o);

                    //v = o == null ? "" : o.ToString();
                }
                catch (System.ArgumentException ex)
                {
                    throw new Exception("Tag " + tag.TagValue + " field " + tag.Name + " doesn't exist, please pay attention to the case.", ex);
                }                
            }

            return v;
        }

        private string GetDispFormUrl(string itemUrl)
        {
            StringBuilder sb = new StringBuilder("");
            string[] arr = itemUrl.Split('/');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                sb.Append(arr[i]);
                sb.Append("/");
            }

            return sb.ToString() + "DispForm.aspx?id=" ;
        }

        private string GetDispFormUrl2(string itemUrl)
        {
            StringBuilder sb = new StringBuilder("");
            string[] arr = itemUrl.Split('/');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                sb.Append(arr[i]);
                sb.Append("/");
            }

            return sb.ToString() + "DispForm2.aspx?id=";
        }
    }
}
