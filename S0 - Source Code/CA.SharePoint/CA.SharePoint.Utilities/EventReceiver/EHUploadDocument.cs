using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Data;
using CA.SharePoint.Utilities.Common;

namespace CA.SharePoint.Utilities
{
    public class EHUploadDocument : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {
//            base.ItemAdded(properties);

            SPFile spf = properties.ListItem.File;
            DisableEventFiring();
            spf.MoveTo(ReplaceInvalidName(properties.AfterUrl));
            spf.Update();
            EnableEventFiring();
        }

        private string ReplaceInvalidName(string name)
        {
            char[] chars = new char[] { '#', '%', '*', ':', '<', '>', '?', '\\', '/', '{', '|', '}', '~' };

            name.Replace("&", "and");

            foreach (char c in chars)
                name.Replace(c, '_');

            return name;
        }
    }
}
