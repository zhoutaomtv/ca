
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
 

namespace CA.SharePoint
{
    /// <summary>
    ///  站点事件日志
    /// </summary>
    public abstract class SiteLogManager
    {
        const string SiteLog_ListName = "__SiteLog";

        public static SiteLogManager GetSiteLogManager(SPWeb web)
        {
            if( SPContext.Current != null )
                return new ListSiteLogManager(web, SiteLog_ListName , true );
            else
                return new ListSiteLogManager(web, SiteLog_ListName, false );
        }

        public abstract void Write(string source, string title, string content);         


    }

    class ListSiteLogManager : SiteLogManager
    {
        private SPWeb _web;
        private string _listName;
        private bool _ElevatedPrivileges = false;
        public ListSiteLogManager( SPWeb web , string listName , bool ElevatedPrivileges )
        {
            _web = web;
            _listName = listName;
            _ElevatedPrivileges = ElevatedPrivileges;
        }

        SPList EnsureList( SPWeb web , string listName )
        {
            SPList list = null;

            try
            {
                list = web.Lists[listName];
            }
            catch { }

            if (list == null)
            {
                web.AllowUnsafeUpdates = true;
                Guid listId = _web.Lists.Add(listName, "List for logs.", SPListTemplateType.GenericList);
                list = _web.Lists[listId];

                SPView view = list.DefaultView;
                EnsureField(list, "Source", SPFieldType.Text , view);
                EnsureField( list , "Content", SPFieldType.Note, view);

                view.Update();
            }

            return list;
        }

        protected void EnsureField(SPList list, string fieldName, SPFieldType fieldType, SPView view)
        {
            if (!list.Fields.ContainsField(fieldName))
            {
                string filedName = list.Fields.Add(fieldName, fieldType, false);
                view.ViewFields.Add(filedName);
            } 
        }

        protected void AddItem(SPList list, string source, string title, string content)
        {
            SPListItem item = list.Items.Add();

            item["Title"] = title;
            item["Source"] = source;
            item["Content"] = content;

            item.Update();
        }

        public override void Write(string source , string title , string content )
        {
            if (this._ElevatedPrivileges)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite elevatedsiteColl = new SPSite(_web.Site.ID))
                    {
                        using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(_web.ID))
                        {
                            SPList list = this.EnsureList(elevatedWeb, _listName);
                            elevatedWeb.AllowUnsafeUpdates = true;

                            AddItem(list, source, title, content);
                        }
                    }
                });

            }
            else
            {
                SPList list = this.EnsureList(_web, _listName);
                AddItem(list, source, title, content);
            }
        }       
    }

  
}
