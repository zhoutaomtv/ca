
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;

namespace CA.SharePoint.WebPartSkin
{
    /// <summary>
    /// 管理站点的皮肤
    /// </summary>
    public class SiteSkinManager : SkinManager
    {
         
        private static Hashtable _SiteSkinManagers = new Hashtable();

        public static SiteSkinManager GetSiteSkinManager()
        {
            SPContext c = SPContext.Current ;

            if (_SiteSkinManagers.ContainsKey(c.Site.ID))
            {
                return _SiteSkinManagers[c.Site.ID] as SiteSkinManager;
            }
            else
            {
                SiteSkinManager sm = new SiteSkinManager();
                sm.LoadSiteSkin();
                try
                {
                    _SiteSkinManagers.Add(c.Site.ID, sm);
                }
                catch { }

                return sm;
            }
        }

        void LoadSiteSkin( )
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPQuery qry = new SPQuery();
                        qry.Query = "<Where><Contains><FieldRef Name='FileLeafRef'/><Value Type='Text'>" +
                            ".temp.html</Value></Contains></Where>";

                        //try opening the file
                        SPList xList = elevatedWeb.GetCatalog(SPListTemplateType.MasterPageCatalog);
                        if (xList == null) return;
                        SPListItemCollection xItems = xList.GetItems(qry);

                        if ((xItems != null) && (xItems.Count > 0))
                        {
                            foreach (SPListItem item in xItems)
                            {
                                string html = Encoding.UTF8.GetString(item.File.OpenBinary());
                                ParserHtmlTemplate(html);
                            }
                        }

                    }
                }
            });         
        }

        public override void ClearSkinCache()
        {
            SPContext c = SPContext.Current;

            if (_SiteSkinManagers.ContainsKey(c.Site.ID))
            {
                _SiteSkinManagers.Remove(c.Site.ID);
            }

            //base.ClearSkinCache();
        }
    }


}
