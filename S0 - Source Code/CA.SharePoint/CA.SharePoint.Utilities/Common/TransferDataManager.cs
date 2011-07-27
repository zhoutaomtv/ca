
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
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
//using Microsoft.SharePoint.WebPartPages;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search.Query;

using CA.Web;
using CA.SharePoint.CamlQuery;


namespace CA.SharePoint
{
    /// <summary>
    /// 页面临时传输数据管理
    /// </summary>
    public abstract class TransferDataManager
    {
        public static readonly TransferDataManager Instance = new DocLibTransferDataManager();

        public virtual object GetData(Type t, Guid id)
        {
            return null;
        }

        public virtual object GetData(Type t, Guid id,bool autoClear)
        {
            object obj = GetData(t, id);

            if (autoClear && obj != null)
                ClearData(id);

            return obj ;
        }

        public virtual Guid StoreData(object obj)
        {
            return Guid.Empty;
        }

        public virtual void ClearData(Guid id)
        {
        }
    }

    class DocLibTransferDataManager : TransferDataManager
    {
        SPList EnsureList(SPWeb web)
        {
            SPList list = null;

            try
            {
                list = web.Lists["__TransferData"];
            }
            catch { }

            if (list == null)
            {
                web.AllowUnsafeUpdates = true;
                Guid listId = web.Lists.Add("__TransferData", "List for temp transferd data,never delete this list.", SPListTemplateType.DocumentLibrary);
                list = web.Lists[listId];
            }

            return list;
        }

        public override Guid StoreData(object obj)
        {
            SPList list = null;

            string xml = Common.SerializeUtil.Seralize(obj);
            byte[] content = Encoding.UTF8.GetBytes(xml);

            Guid id = default(Guid);

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                    {
                        list = this.EnsureList(elevatedWeb);
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPFile file = list.RootFolder.Files.Add(Guid.NewGuid().ToString() + ".xml", content, true);

                        id = file.Item.UniqueId;
                    }
                }
            });

            return id;

        }

        public override void ClearData(Guid id)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                  {
                      using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                      {
                          try
                          {
                              SPList list = this.EnsureList(elevatedWeb);
                              elevatedWeb.AllowUnsafeUpdates = true;
                              SPListItem item = list.GetItemByUniqueId(id);
                              item.Delete();
                          }
                          catch { }
                      }
                  }
              });

        }

        public override object GetData(Type t, Guid id)
        {
            object obj = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                  {
                      using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                      {
                          SPList list = this.EnsureList(elevatedWeb);

                          try
                          {
                              SPListItem item = list.GetItemByUniqueId(id);

                              SPFile file = item.File;

                              XmlDocument doc = new XmlDocument();
                              doc.Load(item.File.OpenBinaryStream());

                              obj = Common.SerializeUtil.Deserialize(t, doc.OuterXml);
                          }
                          catch { }
                      }
                  }
              });

            return obj;
        }
    }
}
