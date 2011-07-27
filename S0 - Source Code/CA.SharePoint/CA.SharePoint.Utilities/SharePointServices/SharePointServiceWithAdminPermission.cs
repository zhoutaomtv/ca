using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Collections;
using CodeArt.SharePoint.CamlQuery;
using System.Data;

namespace CA.SharePoint
{
    class SharePointServiceWithAdminPermission : ISharePointService 
    {
        private SPWeb _web;
        //private ISharePointService _contextService = ServiceFactory.GetSharePointService(false);

        public SharePointServiceWithAdminPermission()
        {
            _web = SPContext.Current.Web;
        }

        public SharePointServiceWithAdminPermission(SPWeb web)
        {
            _web = web;
        }

        public SPUser GetUser(string loginName)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        user = web.EnsureUser(loginName);   
                    }
                }
            });
            return user;   
        }

        public SPList GetList(string name)
        {
            SPList spList = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SharePointService svr = new SharePointService(web);
                        spList = svr.GetList(name);
                    }
                }
            });

            return spList;
        }

        public SPList GetDocumentLibrary(string name)
        {
            throw new NotSupportedException();
        }

        public void AddListItem( SPList list , IDictionary dic)
        {

        }

        public void UpdateListItem(SPList list, IDictionary dic , int id )
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SPList list2 = web.Lists[list.ID];
                        web.AllowUnsafeUpdates = true;
                        SPListItem item = list2.GetItemById(id);

                        SharePointService svr = new SharePointService(web);
                        svr.UpdateListItem(list2, dic, item.ID);
                    }
                }
            });
        }

        /// <summary>
        /// add by caixiang
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dic"></param>
        /// <param name="ids"></param>
        public void UpdateListItems(SPList list, IDictionary dic, int[] ids)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SPList list2 = web.Lists[list.ID];

                        SharePointService svr = new SharePointService(web);
                        foreach (int id in ids)
                        {
                            SPListItem item = list2.GetItemById(id);
                            svr.UpdateListItem(list2, dic, item.ID);
                        }
                    }
                }
            });
        }

        public void AddFile(SPDocumentLibrary list, byte[] data, string fielName, IDictionary props)
        {

        }


        public void UpdateFile(SPDocumentLibrary list, byte[] data, string fielName, IDictionary props , int id )
        {

        }

        public SPFile GetFile(SPDocumentLibrary lib, int id)
        {
            SPListItem item = lib.GetItemById(id) ;

            return item.File;
        }

        //public SPListItemCollection Query(SPList list, string expr, int rowLimit , bool rec )
        //{
        //    SPListItemCollection items = null;
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
        //            {
        //                SPList list2 = web.Lists[list.ID];

        //                SharePointService svr = new SharePointService(web);
        //                items = svr.Query(list2, expr, rowLimit, rec);                       
        //            }
        //        }
        //    });
        //    return items;
        //}

        public SPListItemCollection Query( SPList list , CamlExpression expr , int rowLimit , params OrderPair[] orders )
        {
            SPListItemCollection items = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SPList list2 = web.Lists[list.ID];

                        SharePointService svr = new SharePointService(web);
                        items = svr.Query(list2, expr, rowLimit, orders);
                    }
                }
            });
            return items;
        }

        public DataTable QueryDataTable(SPList list, CamlExpression expr, int rowLimit , params OrderPair[] orders)
        {

            DataTable data = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SPList list2 = web.Lists[list.ID];

                        SharePointService svr = new SharePointService(web);
                        data = svr.QueryDataTable(list2, expr, rowLimit, orders);
                    }
                }
            });
            return data;
        }


        public SPListItem GetItem(SPList list, int id)
        {
            SPListItem item = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SPList list2 = web.Lists[list.ID];
                        item = list2.GetItemById(id);                       
                    }
                }
            });

            return item;
        }

        public SPListItem GetItem(SPList list, CamlExpression expr)
        {
            SPListItem item = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        SPList list2 = web.Lists[list.ID];

                        SharePointService svr = new SharePointService(web);
                        item = svr.GetItem(list2, expr);
                    }
                }
            });

            return item;
        }

        public void DeleteFolder(string listName, string folderName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPSite site = new SPSite(this._web.Site.ID))
               {
                   using (SPWeb web = site.OpenWeb(this._web.ID))
                   {
                       SharePointService svr = new SharePointService(web);
                       svr.DeleteFolder(listName, folderName);                     
                   }
               }
           });
        }

        public SPFolder EnsureFolder2(SPList list, string folderName)
        {
            SPFolder folder = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this._web.ID))
                    {
                        if (String.IsNullOrEmpty(folderName))
                            folder = list.RootFolder;

                        string folderURL = list.RootFolder.Url + "/" + folderName.TrimStart('/');

                        SPFolder f = web.GetFolder(folderURL);
                        if (f.Exists == false)
                        {
                            web.AllowUnsafeUpdates = true;

                            SPFolder parentFolder = list.RootFolder;

                            string[] fs = folderName.Trim('/').Split('/');

                            foreach (string fName in fs)
                            {
                                SPFolder curFolder = null;
                                try
                                {
                                    curFolder = parentFolder.SubFolders[fName];
                                }
                                catch (ArgumentException)
                                {
                                    curFolder = null;
                                }

                                if (curFolder == null)
                                    curFolder = parentFolder.SubFolders.Add(fName);

                                parentFolder = curFolder;
                            }
                            folder = parentFolder;
                        }
                        folder = f;
                    }
                }
            });
            return folder;
        }
     
       
    }
}
