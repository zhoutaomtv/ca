using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;
using System.Web;

namespace CA.SharePoint
{
    /// <summary>
    /// 
    /// </summary>
    public class SharePointUtil
    {
        static public bool IsHiddenFolder(SPFolder f)
        {
            return f.Item == null;
            //return f.Properties.Count < 20;
        }

        static public string GetWebUrl( SPWeb web )
        {
            string webUrl = web.Url;
            if (!webUrl.EndsWith("/"))
                webUrl += "/";

            return webUrl;
        }

        public static StringDictionary GetFieldDictionary(SPList list)
        {
            StringDictionary fields = new StringDictionary();

            foreach (SPField f in list.Fields)
            {
                if (fields.ContainsKey(f.Title)) continue;

                fields.Add(f.Title, f.InternalName);
            }

            return fields;
        }


        static public int[] GetSelectedItemIDs( SPList list )
        {
            return GetSelectedItemIDs( list.ID.ToString() ) ;
        }

        static public int[] GetSelectedItemIDs(string listId )
        {
            if (System.Web.HttpContext.Current == null)
                throw new SPException("context is null.");

            string ids = System.Web.HttpContext.Current.Request["spItemSelectionCheckBox_" + listId];

            if (ids == null || ids == "")
                return null;

            String[] arr = ids.Split(',');

            int[] intIDs = new int[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                intIDs[i] = Convert.ToInt32(arr[i]);
            }

            return intIDs;
        }

        /// <summary>
        /// 修改视图中的查询条件部分
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="query"></param>
        static public void ChangeSchemaXmlQuery(XmlDocument doc, string query)
        {
            if (String.IsNullOrEmpty(query)) return;

            string innerQuery = GetInnerQuery(query);
            if (innerQuery == "") return;

            XmlNode queryNode = doc.DocumentElement.SelectSingleNode("Query");
            //queryNode.InnerXml = qxml;

            XmlNode whereNode = queryNode.SelectSingleNode("Where");

            if (whereNode != null)
                queryNode.RemoveChild(whereNode);

            XmlNode newWhereNode = doc.CreateElement("Where");
            newWhereNode.InnerXml = innerQuery;

            queryNode.AppendChild(newWhereNode);

            XmlNode ViewEmptyNode = doc.DocumentElement.SelectSingleNode("ViewEmpty");

            ViewEmptyNode.InnerXml = "<HTML><![CDATA[<font color='red'><b>No record found. Please change your search criteria to try again.</b></font>]]></HTML>";

        }

        /// <summary>
        /// 设置视图xml中的查询条件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="query1">条件1</param>
        /// <param name="query2">条件2</param>
        static public void ChangeSchemaXmlQuery(XmlDocument doc, string query1 , string query2)
        {
            string query = "";

            string innerQuery = GetInnerQuery(query1);           

            string innerQuery2 = GetInnerQuery(query2);

            if (innerQuery == "" && innerQuery2 == "" ) return;

            if (innerQuery != "" && innerQuery2 != "")
                query = "<And>" + innerQuery + innerQuery2 + "</And>" ;
            else
                query = innerQuery + innerQuery2 ;

            XmlNode queryNode = doc.DocumentElement.SelectSingleNode("Query");
            //queryNode.InnerXml = qxml;

            XmlNode whereNode = queryNode.SelectSingleNode("Where");

            if (whereNode != null)
                queryNode.RemoveChild(whereNode);

            XmlNode newWhereNode = doc.CreateElement("Where");
            newWhereNode.InnerXml = query;

            queryNode.AppendChild(newWhereNode);

            XmlNode ViewEmptyNode = doc.DocumentElement.SelectSingleNode("ViewEmpty");

            ViewEmptyNode.InnerXml = "<HTML><![CDATA[<font color='red'><b>No record found. Please change your search criteria to try again.</b></font>]]></HTML>";

        }


        static string GetInnerQuery(string q)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Query>" + q + "</Query>");

            XmlNode whereNode = doc.DocumentElement.SelectSingleNode("Where");

            if (whereNode != null)
                return whereNode.InnerXml;
            else
                return "";
        }

 

        /// <summary>
        /// 获取ViewXml中的排序结
        /// </summary>
        /// <param name="viewXml"></param>
        /// <returns></returns>
       public static string GetOrderBySection( string viewXml )
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(viewXml);

            XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("OrderBy");

            if (nodes == null || nodes.Count == 0)
                return "";
            else
                return nodes[0].OuterXml;
        }

        /// <summary>
        /// 指定ID的试图是否存在
        /// </summary>
        /// <param name="views"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        static public bool ViewExist(SPViewCollection views, Guid viewId)
        {
            foreach (SPView view in views)
            {
                if (view.ID == viewId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取按名称排序后的子文件夹列表
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        static public IList<SPFolder> GetSortedFolders(SPFolderCollection folders)
        {
            List<SPFolder> sortedFolders = new List<SPFolder>();

            foreach (SPFolder f in folders)
            {
                if (f.Item != null) //为非系统文件夹
                    sortedFolders.Add(f);
            }

            sortedFolders.Sort(CompareFolder);

            return sortedFolders;
        }

        static int CompareFolder(SPFolder f1, SPFolder f2)
        {
            return f1.Name.CompareTo(f2.Name);
        }

        /// <summary>
        /// 获取按某个字段排序后的子文件夹
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="sortFieldName"></param>
        /// <returns></returns>
        static public IList<SPFolder> GetSortedFolders(SPFolderCollection folders , string sortFieldName )
        {
           
            List<SPFolder> sortedFolders = new List<SPFolder>();

            foreach (SPFolder f in folders)
            {
                if( f.Item != null ) //为非系统文件夹
                    sortedFolders.Add(f);
            }

            SPFolderComparer c = new SPFolderComparer(sortFieldName);

            sortedFolders.Sort(c);

            return sortedFolders;
        }

        class SPFolderComparer : IComparer<SPFolder>
        {
            private string _sortFieldName;
            public SPFolderComparer(string sortFieldName)
            {
                _sortFieldName = sortFieldName;
            }

            #region IComparer<SPFolder> 成员

            public int Compare(SPFolder x, SPFolder y)
            {
                //if (IsHiddenFolder(x) || IsHiddenFolder(y)) return 0;                

                if (!x.Item.Fields.ContainsField(_sortFieldName) || !y.Item.Fields.ContainsField(_sortFieldName))
                    return 0;

                string sX = "" + x.Item[_sortFieldName];
                if (sX == "")
                    sX = "0";

                string sY = "" + y.Item[_sortFieldName];
                if (sY == "")
                    sY = "0";

                return Convert.ToInt32(sX).CompareTo(Convert.ToInt32(sY));

            }

            #endregion
        }

        public static SPList TryGetList(SPWeb web, string listName)
        {
            if (String.IsNullOrEmpty(listName))
                throw new ArgumentNullException("listName");

            try
            {
                return web.Lists[listName];
            }
            catch
            {
                return null;
            }
        }

        public static SPList GetList(SPWeb web, string listName)
        {
            if (String.IsNullOrEmpty(listName))
                throw new ArgumentNullException("listName");

            try
            {
                return web.Lists[listName];
            }
            catch (Exception ex)
            {               
                throw new SPException(String.Format("List [{1}] not exist in site [{0}] or user no access to the list.", web.Url, listName), ex);
            }
        }

        //public static double GetListSize(SPWeb web, string listName)
        //{ 

        //}

        public static void CheckListField(SPList list, string fieldName)
        {
            try
            {
                list.Fields.GetField(fieldName);
            }
            catch (Exception ex)
            {
                throw new SPException(String.Format("Field [{1}] not exist in list [{0}].", list.Title, fieldName));
            }
        }

        public static SPList GetList(string listName)
        {
            return GetList(SPContext.Current.Web, listName);
        }

        public static string GetWebLayoutsPath(SPList list)
        {
            string parentWebUrl = list.ParentWebUrl;
            if (!parentWebUrl.EndsWith("/"))
            {
                parentWebUrl = parentWebUrl + "/";
            }
            return (parentWebUrl + "_layouts/");

        }

        public static string GetWebLayoutsPath(SPWeb web)
        {
            string parentWebUrl = web.Url;
            if (!parentWebUrl.EndsWith("/"))
            {
                parentWebUrl = parentWebUrl + "/";
            }
            return (parentWebUrl + "_layouts/");
        }

        public static string GetWebLayoutsPath()
        {
            return GetWebLayoutsPath(SPContext.Current.Web);
        }


        public static SPFolder EnsureFolder(SPWeb web, SPList list, string folderName)
        {
            if (String.IsNullOrEmpty(folderName))
                return list.RootFolder;

            string folderURL = list.RootFolder.Url + "/" + folderName.TrimStart('/');

            SPFolder f = web.GetFolder(folderURL);
            if (f.Exists == false)
            {
                web.AllowUnsafeUpdates = true;
                SPListItem item = list.Items.Add();
                item.FileSystemObjectType = SPFileSystemObjectType.Folder;
                item["FileLeafRef"] = folderName;
                item["Title"] = folderName;
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
                f = web.GetFolder(folderURL);
            }
            return f;
        }

        public static SPFolder GetFolder(SPWeb web, SPList list, string folderURL, bool append)
        {
            if (String.IsNullOrEmpty(folderURL))
                return list.RootFolder;

            string folderFullURL = string.Empty;

            if (append)
                folderFullURL = list.RootFolder.Url + "/" + folderURL.TrimStart('/');
            else
                folderFullURL = folderURL.TrimStart('/');

            SPFolder f = web.GetFolder(folderFullURL);
            if (f.Exists)
                return f;
            else
                return null;
        }

        public static SPFolder TryGetFolder(SPWeb web, SPList list, string folderURL)
        {
            if (String.IsNullOrEmpty(folderURL))
                return list.RootFolder;

            string folderFullURL = list.RootFolder.Url + "/" + folderURL.TrimStart('/');

            SPFolder f = web.GetFolder(folderFullURL);
            if (f.Exists)
                return f;
            else
                return null;
        }

        public static void CheckFolder(SPWeb web, SPList list, string folderURL)
        {
            if (null == TryGetFolder(web, list, folderURL))
                throw new Exception(String.Format("Folder [{1}] not exist in list [{0}].", list.Title, folderURL));
        }

        public static void CheckFolder(SPList list, string folderURL)
        {
            CheckFolder(list.ParentWeb, list, folderURL);
        }

        public static bool FolderExist(SPWeb web, SPList list, string folderURL)
        {
            return null != TryGetFolder(web, list, folderURL);
        }

        public static SPFolder EnsureFolder2(SPWeb web, SPList list, string folderName)
        {
            if (String.IsNullOrEmpty(folderName))
                return list.RootFolder;

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
                return parentFolder;
            }
            return f;
        }

        /// <summary>
        /// 表单和附件的存储路径
        /// </summary>
        /// <returns></returns>
        public static string GetFileStoragePath()
        {
            return DateTime.Now.ToString("yyyy-MM");
            //return DateTime.Now.ToString("yyyy-mm").Replace("-", "/");
        }

        static SPList EnsureList(SPWeb web, string listName, SPListTemplateType listType)
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
                Guid listId = web.Lists.Add(listName, "", listType);
                list = web.Lists[listId];
            }

            return list;
        }

        public static void DeleteFolder(SPWeb web, SPList list, string folderName)
        {
            string folderURL = list.RootFolder.Url + "/" + folderName;
            SPFolder f = web.GetFolder(folderURL);
            if (f.Exists)
            {
                web.AllowUnsafeUpdates = true;
                f.Delete();
            }
        }

        public static void DeleteFile(SPWeb web, SPList list, string folderName, string fileName)
        {
            string folderURL = list.RootFolder.Url + "/" + folderName.TrimStart('/');
            SPFolder f = web.GetFolder(folderURL);
            if (f.Exists)
            {
                web.AllowUnsafeUpdates = true;
                try
                {
                    f.Files.Delete(fileName);
                }
                catch { }
            }
        }

        public static void DeleteItem(SPWeb web, SPList list, int id)
        {
            web.AllowUnsafeUpdates = true;
            try
            {
                SPListItem item = list.GetItemById(id);
                item.Delete();
            }
            catch { }
        }


        const string TempStoreLib = "__Temp";
        /// <summary>
        /// 用户在sharepoint将字符串作为文间下载，防止直接下载word会再次请求造成出错。
        /// 通过将文件保存到文档库中实现。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        public static void DownloadFile(string fileName, string fileContent)
        {
            SPList list = null;

            SPFile file = null;

            byte[] content = Encoding.UTF8.GetBytes(fileContent);

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                    {
                        list = EnsureList(elevatedWeb, TempStoreLib, SPListTemplateType.DocumentLibrary);
                        elevatedWeb.AllowUnsafeUpdates = true;
                        file = list.RootFolder.Files.Add(fileName, content, true);
                    }
                }
            });

            HttpResponse Response = HttpContext.Current.Response;
            Response.Write("<script>window.open('" + file.Item["EncodedAbsUrl"] + "')</script>");
        }

        public static bool IsUserInGroup(SPUser user, string groupName)
        {
            foreach (SPGroup tmpGroup in user.Groups)
            {
                if (tmpGroup.Name.ToLower() == groupName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCurrentUserInGroup(string groupName)
        {
            return IsUserInGroup(SPContext.Current.Web.CurrentUser, groupName);
        }

    }
}
