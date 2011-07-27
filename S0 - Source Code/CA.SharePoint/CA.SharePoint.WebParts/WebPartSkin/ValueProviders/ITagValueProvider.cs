
using System;
namespace CA.SharePoint.WebPartSkin
{
    /// <summary>
    /// 标签值提供程序接口
    /// </summary>
    interface ITagValueProvider
    {
        string GetValue(ReplaceTag tag);
    }
}
