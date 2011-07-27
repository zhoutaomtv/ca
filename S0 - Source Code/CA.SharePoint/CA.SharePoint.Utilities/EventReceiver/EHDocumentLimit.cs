using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Data;
using CA.SharePoint.Utilities.Common;

namespace CA.SharePoint.Utilities
{
    public class EHDocumentLimit : SPItemEventReceiver
    {
        public override void ItemAdding(SPItemEventProperties properties)
        {
            DataTable dt = (new SharePointDBUtil()).GetSiteSizeTable(properties.SiteId);

            DataRow row = dt.Select(string.Format("tp_Title='{0}'", properties.ListTitle))[0];

            if (Convert.ToDouble(row["TotalSize"]) > 1024 * 1024 * 1024 * (double.Parse(properties.ReceiverData)))
            {
                properties.Cancel = true;
                properties.ErrorMessage = string.Format("The size limit of the library is {0}GB！", properties.ReceiverData);
            }
        }

    }
}
