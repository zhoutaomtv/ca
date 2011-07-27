using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;

using Microsoft.SharePoint;
//using Microsoft.SharePoint.Publishing;

using CA.SharePoint.WebPartSkin;

namespace CA.SharePoint
{
    /// <summary>
    /// 列表项信息查看
    /// </summary>
    public class ListItemViewWebPart : BaseSPListWebPart
    {
        

        protected override void RenderContents(HtmlTextWriter writer)
        {            
            SkinElement el = GetSkin();

            if (el == null)
                return;

            string skinKey =  base.TemplateID + "_body";

            IList<ReplaceTag> tags = ReplaceTagManager.GetInstance().GetReplaceTags(skinKey, el.Body);

            string v ;

            SPListItem item = SPContext.Current.ListItem;
            ITagValueProvider fieldValueProvider = new SPListItemValueProvider(item, 1);

            StringBuilder sb = new StringBuilder(el.Body);
            foreach (ReplaceTag tag in tags)
            {
                if (tag.ValueProvider == "spfield")
                    sb.Replace(tag.TagValue, fieldValueProvider.GetValue(tag));
            }

            writer.Write(sb.ToString());
        }   

    }
}
