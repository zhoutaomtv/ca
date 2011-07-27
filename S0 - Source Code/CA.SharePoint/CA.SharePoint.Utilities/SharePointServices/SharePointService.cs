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
    class SharePointService : ISharePointService 
    {
        private SPWeb _web;
        public SharePointService( SPWeb web )
        {
            _web = web;
        }

        public SharePointService()
        {
            _web = SPContext.Current.Web;
        }

        public SPUser GetUser(string loginName)
        {
            return _web.EnsureUser(loginName);         
        }

        public SPList GetList(string name)
        {
            return _web.Lists[name];
        }

        public SPList GetDocumentLibrary(string name)
        {
            return _web.Lists[name] as SPDocumentLibrary ;
        }

        public void AddListItem( SPList list , IDictionary dic)
        {

        }

        public void UpdateListItem(SPList list, IDictionary dic , int id )
        {
            SPListItem item = list.GetItemById(id);
            foreach (string fieldName in dic.Keys)
            {
                item[fieldName] = dic[fieldName];
            }
            item.Update();
        }

        public void UpdateListItems(SPList list, IDictionary dic, int[] ids)
        { 
        
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
        //    SPQuery q = new SPQuery();

        //    if (!string.IsNullOrEmpty(expr))
        //        q.Query = expr;

        //    if (rowLimit >= 0)
        //        q.RowLimit = (uint)rowLimit;

        //    SPListItemCollection items = list.GetItems(q);

        //    return items;
        //}

        public SPListItemCollection Query(SPList list, CamlExpression expr, int rowLimit, params OrderPair[] orders)
        {
            IList<OrderPair> dic = new List<OrderPair>();

            if (orders != null)
            {
                foreach (OrderPair p in orders)
                    dic.Add(p);
            }

            SPQuery q = new SPQuery();

            //if (expr != null)
            q.Query = CamlBuilder.Where(list, expr,dic);

            if (rowLimit >= 0)
                q.RowLimit = (uint)rowLimit;

            SPListItemCollection items = list.GetItems(q);

            return items;
        }

        public DataTable QueryDataTable(SPList list, CamlExpression expr, int rowLimit , params OrderPair[] orders)
        {
            
            SPListItemCollection items = this.Query(list,expr,rowLimit,orders);

             DataTable t = items.GetDataTable();

             if (t != null)
             {
                 foreach (DataColumn col in t.Columns)
                 {
                     SPField f = list.Fields.GetFieldByInternalName(col.ColumnName);
                     if (f.ReadOnlyField) continue;
                     col.ColumnName = f.Title;
                 }

             }
            return t ;
        }


        public SPListItem GetItem(SPList list, int id)
        {
            SPListItem item = list.GetItemById(id);
            return item;
        }

        public SPListItem GetItem(SPList list, CamlExpression expr)
        {
            SPListItemCollection items = this.Query(list,expr,1);
            if (items.Count > 0)
                return items[0];
            else
                return null;
        }

        public SPFolder EnsureFolder2(SPList list, string folderName)
        {
            return null;
        }

        public void DeleteFolder( string listName, string folderName)
        {
            SPList list =this._web.Lists[listName];
            string folderURL = list.RootFolder.Url + "/" + folderName;
            SPFolder f = this._web.GetFolder(folderURL);
            if (f.Exists)
            {
                this._web.AllowUnsafeUpdates = true;
                f.Delete();
            }
        }
        
    }
}
