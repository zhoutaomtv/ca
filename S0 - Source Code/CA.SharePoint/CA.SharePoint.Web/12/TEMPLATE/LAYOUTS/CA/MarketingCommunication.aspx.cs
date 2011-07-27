using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using Microsoft.SharePoint;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using CA.SharePoint.Utilities.Common;

namespace CA.SharePoint.Web
{
    public partial class MarketingCommunication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SPFolder folder = findFolder();

                if (folder == null)
                {
                    form1.Visible = false;
                    Page.Response.Redirect("/_layouts/AccessDenied.aspx");
                }

                trvRoot.Nodes.Clear();
                TreeNode rootNode = OutputDirectory(folder);
                rootNode.Expanded = true;
                rootNode.ImageUrl = "/_layouts/CAResources/themeCA/images/root.gif";
                trvRoot.Nodes.Add(rootNode);

                //buildTree(folder);
                List<string> users = UserProfileUtil.UserListInGroup("MarketingNewsPublisher");
                if (!users.Contains(SPContext.Current.Web.CurrentUser.LoginName))
                {
                    PanelAdd.Visible = false;
                }
                else
                {
                    PanelAdd.Visible = true;
                }
            }

            BindRpt();
        }

        private void BindRpt()
        {
            //ISharePointService sps = ServiceFactory.GetSharePointService(false);
            //SPList list = sps.GetList(CA.SharePoint.Utilities.Common.CAConstants.ListName.MarketingCommunicationNotes);
            string rootweburl = ConfigurationManager.AppSettings["rootweburl"] + "";
            if (string.IsNullOrEmpty(rootweburl))
            {
                rootweburl = "http://cnshsps.cnaidc.cn:91";
            }
            SPSite site = new SPSite(rootweburl);
            SPList list = null;
            try
            {
                list = site.RootWeb.Lists["MarketingCommunicationNotes"];
            }
            catch
            {
                list = null;
            }

            if (list == null)
            {
                form1.Visible = false;
                Page.Response.Redirect("/_layouts/AccessDenied.aspx");
            }

            string strperpage = ConfigurationManager.AppSettings["marketingcommunicationperpage"] + "";
            uint perpage = string.IsNullOrEmpty(strperpage) ? 10 : uint.Parse(strperpage);

            SPQuery query = new SPQuery();
            query.Query = @"<Where><IsNotNull><FieldRef Name='Title' /></IsNotNull></Where><OrderBy><FieldRef Name ='Created' Type='Date' Ascending = 'FALSE' /></OrderBy>";
            query.RowLimit = perpage;

            SPListItemCollection Lists = list.GetItems(query);
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Title");
            dt.Columns.Add("Content");
            dt.Columns.Add("Modified");
            for (var i = 0; i < Lists.Count; i++)
            {
                DataRow row = dt.Rows.Add();
                row["ID"] = Lists[i]["ID"];
                row["Title"] = Lists[i]["Title"];
                row["Modified"] = Lists[i]["Modified"];
                row["Content"] = repaceLink(Lists[i]["Content"] + "");
            }

            rptNotes.DataSource = dt;
            rptNotes.DataBind();
        }

        private SPFolder findFolder()
        {
            SPFolder folder = null;

            ISharePointService sps = ServiceFactory.GetSharePointService(false);

            string libraryFolder = ConfigurationManager.AppSettings["marketingcommunicationlibrary"] + "";
            string mclistname = "Marketing Communication";
            SPList list;
            try
            {
                list = sps.GetList(mclistname);
            }
            catch
            {
                return null;
            }

            if (string.IsNullOrEmpty(libraryFolder))
            {
                folder = list.RootFolder;
            }
            else if (!libraryFolder.Contains('/'))
            {
                folder = list.RootFolder;
            }
            else
            {
                //int index = libraryFolder.IndexOf('/');
                //string library = libraryFolder.Substring(0, index);
                //string pathName = libraryFolder.Substring(index + 1);

                //try
                //{
                //    SPList list = sps.GetList(library);
                //    folder = SharePointUtil.GetFolder(SPContext.Current.Web, list, pathName);
                //}
                //catch
                //{
                //    folder = null;
                //}
                folder = SharePointUtil.GetFolder(SPContext.Current.Web, list, libraryFolder, false);
            }
            
            return folder;
        }

        private TreeNode OutputDirectory(SPFolder directory)
        {
            if (directory == null)
                return null;

            TreeNode node = new TreeNode(directory.Name);
            node.SelectAction = TreeNodeSelectAction.Expand;
            node.PopulateOnDemand = false;

            //次级目录
            Dictionary<string, SPFolder> subDirectories = sortFolders(directory.SubFolders);
            foreach (KeyValuePair<string, SPFolder> pair in subDirectories.OrderBy(pair => pair.Key))
            {
                if (!pair.Value.Name.Equals("Forms", StringComparison.CurrentCultureIgnoreCase))
                {
                    //OutputDirectory(pair.Value, node);
                    TreeNode childNode = new TreeNode();

                    childNode.Text = pair.Value.Name;
                    childNode.Value = pair.Value.Url;
                    childNode.Expanded = false;
                    childNode.Selected = false;
                    childNode.SelectAction = TreeNodeSelectAction.Expand;

                    // 设置通过Ajax动态获取节点
                    childNode.PopulateOnDemand = true;

                    childNode.Checked = false;

                    node.ChildNodes.Add(childNode);
                }
            }

            //该级目录下文件
            Dictionary<string, SPFile> Files = sortFiles(directory.Files);
            foreach (KeyValuePair<string, SPFile> pair in Files.OrderBy(pair => pair.Key))
            {
                TreeNode child = new TreeNode(pair.Value.Name);
                child.NavigateUrl = pair.Value.ServerRelativeUrl;
                child.Target = "_blank";
                child.ImageUrl = "/_layouts/CAResources/themeCA/images/file.gif";
                node.ChildNodes.Add(child);
            }

            return node;
        }

        private string repaceLink(string content)
        {
            Regex httpregex = new Regex(@"<a href=", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            content = httpregex.Replace(content, "<a target=\"_blank\" style=\"cursor:hand; color:Blue\" href=");

            return content;
        }

        private Dictionary<string, SPFolder> sortFolders(SPFolderCollection folders)
        {
            Dictionary<string, SPFolder> sortedFolders = new Dictionary<string, SPFolder>();

            foreach (SPFolder folder in folders)
            {
                sortedFolders.Add(folder.Name, folder);
            }

            return sortedFolders;
        }

        private Dictionary<string, SPFile> sortFiles(SPFileCollection files)
        {
            Dictionary<string, SPFile> sortedFiles = new Dictionary<string, SPFile>();

            foreach (SPFile file in files)
            {
                sortedFiles.Add(file.Name, file);
            }

            return sortedFiles;
        }

        /// <summary>
        /// 动态加载树的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateNode(Object sender, TreeNodeEventArgs e)
        {
            SPFolder directory = SPContext.Current.Web.GetFolder(e.Node.Value);

            if (directory.Exists)
            {
                //次级目录
                Dictionary<string, SPFolder> subDirectories = sortFolders(directory.SubFolders);
                foreach (KeyValuePair<string, SPFolder> pair in subDirectories.OrderBy(pair => pair.Key))
                {
                    if (!pair.Value.Name.Equals("Forms", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //OutputDirectory(pair.Value, node);
                        TreeNode childNode = new TreeNode();

                        childNode.Text = pair.Value.Name;
                        childNode.Value = pair.Value.Url;
                        childNode.Expanded = false;
                        childNode.Selected = false;
                        childNode.SelectAction = TreeNodeSelectAction.Expand;

                        // 设置通过Ajax动态获取节点
                        childNode.PopulateOnDemand = true;

                        childNode.Checked = false;

                        e.Node.ChildNodes.Add(childNode);
                    }
                }

                //该级目录下文件
                Dictionary<string, SPFile> Files = sortFiles(directory.Files);
                foreach (KeyValuePair<string, SPFile> pair in Files.OrderBy(pair => pair.Key))
                {
                    TreeNode child = new TreeNode(pair.Value.Name);
                    child.NavigateUrl = pair.Value.ServerRelativeUrl;
                    child.Target = "_blank";
                    child.ImageUrl = "/_layouts/CAResources/themeCA/images/file.gif";
                    e.Node.ChildNodes.Add(child);
                }
            }

        }

        //private void buildTree(SPFolder folder)
        //{
        //    trvRoot.Nodes.Clear();

        //    TreeNode rootNode = OutputDirectory(folder, null);
        //    rootNode.Expanded = true;
        //    rootNode.ImageUrl = "/_layouts/CAResources/themeCA/images/root.gif";
        //    trvRoot.Nodes.Add(rootNode);
        //}

        //        private TreeNode OutputDirectory(SPFolder directory, TreeNode parentNode)
        //        {
        //            if (directory == null)
        //                return null;

        //            TreeNode node = new TreeNode(directory.Name);
        ////            node.ImageUrl = "/_layouts/CAResources/themeCA/images/folder.gif";
        //            node.SelectAction = TreeNodeSelectAction.Expand;

        //            //node.NavigateUrl = directory.ServerRelativeUrl;
        //            Dictionary<string, SPFolder> subDirectories = sortFolders(directory.SubFolders);

        //            foreach (KeyValuePair<string, SPFolder> pair in subDirectories.OrderBy(pair => pair.Key))
        //            {
        //                if (!pair.Value.Name.Equals("Forms", StringComparison.CurrentCultureIgnoreCase))
        //                    OutputDirectory(pair.Value, node);
        //            }

        //            Dictionary<string, SPFile> Files = sortFiles(directory.Files);
        //            foreach (KeyValuePair<string, SPFile> pair in Files.OrderBy(pair => pair.Key))
        //            {
        //                TreeNode child = new TreeNode(pair.Value.Name);
        //                child.NavigateUrl = pair.Value.ServerRelativeUrl;
        //                child.Target = "_blank";
        //                child.ImageUrl = "/_layouts/CAResources/themeCA/images/file.gif";
        //                node.ChildNodes.Add(child);
        //            }

        //            if (parentNode == null)
        //                return node;
        //            else
        //            {
        //                parentNode.ChildNodes.Add(node);
        //                return parentNode;
        //            }
        //        }

        //public static SPFolder GetFolder(SPWeb web, SPList list, string folderURL)
        //{
        //    if (String.IsNullOrEmpty(folderURL))
        //        return list.RootFolder;

        //    //string folderFullURL = list.RootFolder.Url + "/" + folderURL.TrimStart('/');
        //    string folderFullURL = folderURL.TrimStart('/');
        //    SPFolder f = web.GetFolder(folderFullURL);
        //    if (f.Exists)
        //        return f;
        //    else
        //        return null;
        //}
    }
}
