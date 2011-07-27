using System;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace CA.SharePoint
{
    public interface ISharePointService
    {      
        SPUser GetUser(string loginName);

        DataTable QueryDataTable(SPList list, CamlExpression expr, int rowLimit, params OrderPair[] orders);
       
        void AddFile(Microsoft.SharePoint.SPDocumentLibrary list, byte[] data, string fielName, System.Collections.IDictionary props);
        void AddListItem(Microsoft.SharePoint.SPList list, System.Collections.IDictionary dic);
        Microsoft.SharePoint.SPList GetDocumentLibrary(string name);
        Microsoft.SharePoint.SPFile GetFile(Microsoft.SharePoint.SPDocumentLibrary lib, int id);
        Microsoft.SharePoint.SPListItem GetItem(Microsoft.SharePoint.SPList list, CodeArt.SharePoint.CamlQuery.CamlExpression expr);

        Microsoft.SharePoint.SPListItem GetItem(Microsoft.SharePoint.SPList list, int id );

        Microsoft.SharePoint.SPList GetList(string name);
        Microsoft.SharePoint.SPListItemCollection Query(Microsoft.SharePoint.SPList list, 
            CodeArt.SharePoint.CamlQuery.CamlExpression expr, int rowLimit, params OrderPair[] orders);
        //Microsoft.SharePoint.SPListItemCollection Query(Microsoft.SharePoint.SPList list, string expr, int rowLimit, bool rec);
        void UpdateFile(Microsoft.SharePoint.SPDocumentLibrary list, byte[] data, string fielName, System.Collections.IDictionary props, int id);
        void UpdateListItem(Microsoft.SharePoint.SPList list, System.Collections.IDictionary dic, int id);
        void UpdateListItems(Microsoft.SharePoint.SPList list, System.Collections.IDictionary dic,  int[] ids);
        SPFolder EnsureFolder2(SPList list, string folderName);
        void DeleteFolder(string listName, string folderName);
    }
}
