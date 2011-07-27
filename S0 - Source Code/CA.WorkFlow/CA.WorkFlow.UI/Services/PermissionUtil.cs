using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Xml;
using System.Xml.XPath;
using System.IO;


namespace CA.WorkFlow
{
    public class PermissionUtil
    {
        /// <summary> MOSS权限级别定义. 提供给只需要查看某个资源项目的用户. 该权限级别具备的权限包括: 查看项目  -  查看列表中的项目、文档库中的文档和查看 Web 讨论评论。 </summary>
        public static readonly string SPRoleDefinationName_ViewListItem = "SPRoleDefinition_ViewListItem";

        /// <summary> MOSS权限级别定义. 提供给需要对某个资源项目进行管理的用户. 该权限级别具备的权限包括: 编辑项目, 查看项目, 打开项目.</summary>
        public static readonly string SPRoleDefinationName_ManageListItem = "SPRoleDefinition_ManageListItem";

        //public static readonly string SPRoleDefinationName_DownLoadFile = "文档下载";

        public static readonly string SPRoleDefinationName_ModifyFile = "文档编辑";

        /// <summary> 根据名称获取某个SPWeb上某个权限级别定义</summary>
        /// <param name="web">站点对应的SPWeb</param>
        /// <param name="roleDefinitionByName">站点角色定义的名称</param>
        /// <returns></returns>
        private static SPRoleDefinition GetSPRoleDefinition(SPWeb web, string roleDefinitionByName)
        {
            foreach (SPRoleDefinition rd in web.RoleDefinitions)
                if (rd.Name == roleDefinitionByName)
                    return rd;

            return null;
        }

        /// <summary> 在SPWeb上创建只包括查看权限的权限级别定义</summary>
        /// <param name="web">站点对应的SPWeb</param>
        /// <returns></returns>
        private static SPRoleDefinition CreateViewListItemsRoleDefinition(SPWeb web)
        {
            SPRoleDefinition result = new SPRoleDefinition();
            result.Name = SPRoleDefinationName_ViewListItem;
            result.BasePermissions =
                // Edit By Leo @ 23:02 2007-11-26. OpenItems权限使得User可以将文档下载到本地进行编辑, 应该禁用掉.
                //SPBasePermissions.OpenItems |  
                SPBasePermissions.ViewListItems;

            web.Site.RootWeb.RoleDefinitions.Add(result);

            return result;
        }


        /// <summary> 在SPWeb上创建只包括管理权限的权限级别定义</summary>
        /// <param name="web">站点对应的SPWeb</param>
        /// <returns></returns>
        private static SPRoleDefinition CreateManageListItemRoleDefinition(SPWeb web)
        {
            SPRoleDefinition result = new SPRoleDefinition();
            result.Name = SPRoleDefinationName_ManageListItem;
            result.BasePermissions = 
                SPBasePermissions.EditListItems |
                SPBasePermissions.OpenItems |
                SPBasePermissions.ViewListItems |
                SPBasePermissions.ManagePermissions |
                SPBasePermissions.AddListItems |
                SPBasePermissions.DeleteListItems;

            //SPBasePermissions

            web.Site.RootWeb.RoleDefinitions.Add(result);

            return result;

        }

        private static SPRoleDefinition CreateModifyFileRoleDefinition(SPWeb web)
        {
            SPRoleDefinition result = new SPRoleDefinition();
            result.Name = SPRoleDefinationName_ModifyFile;
            result.BasePermissions =
                SPBasePermissions.EditListItems |
                SPBasePermissions.OpenItems |
                SPBasePermissions.ViewListItems |
                SPBasePermissions.ManagePermissions |
                SPBasePermissions.AddListItems |
                SPBasePermissions.DeleteListItems |
                SPBasePermissions.ViewPages |
                SPBasePermissions.Open;

            //SPBasePermissions

            web.Site.RootWeb.RoleDefinitions.Add(result);

            return result;

        }

       


        public static void AddPermissions(SPListItem item, PermissionSet set)
        {
            foreach (Permission p in set)
            {
                if (p.PermissionType == PermissionType.Edit)
                {
                    AddManageListItemPermission(item, p.Identity);
                   // AddModifyFilePermission(item, p.Identity);
                }
                else
                    AddViewListItemPermission(item, p.Identity);
            }
        }
        

        /// <summary>
        /// 为某个SPPrincipal(LoginName, MOSS User Group)赋于MOSS中某项资源的管理权限
        /// </summary>
        /// <param name="item">MOSS中的某个ListItem</param>
        /// <param name="spPrincipal">MOSS中可以赋于权限的某种SPPrincipal对象. 例如LoginName, MOSS User Group</param>
        public static void AddManageListItemPermission(SPListItem item, string spPrincipal)
        {
            SPRoleDefinition rd = GetSPRoleDefinition(item.Web, SPRoleDefinationName_ManageListItem);

            if (rd == null)
                rd = CreateManageListItemRoleDefinition(item.Web);

            AddRoleAssignmentToListItem(item, spPrincipal, rd);
        }

        public static void AddModifyFilePermission(SPListItem item, string spPrincipal)
        { 
          SPRoleDefinition rd = GetSPRoleDefinition(item.Web, SPRoleDefinationName_ModifyFile);

            if (rd == null)
                rd = CreateModifyFileRoleDefinition(item.Web);

            AddRoleAssignmentToListItem(item, spPrincipal, rd);
        
        }


        /// <summary>
        /// 为某个SPPrincipal赋于针对某个SPListItem的权限. 权限由rd来决定.
        /// </summary>
        /// <param name="item">某个需要赋于权限的SPListItem</param>
        /// <param name="spPrincipalName">用户登录名称或者是MOSS User Group, 如果是EMail地址, 则略过</param>
        /// <param name="rd">需要赋于的权限级别名称</param>
        public static void AddRoleAssignmentToListItem(SPListItem item, string spPrincipalName, SPRoleDefinition rd)
        {
            // 表明这个LoginName是电子邮件地址类型:SmtpAddress
            if (spPrincipalName.IndexOf("@") >= 0)
                return;

            SPRoleAssignment ra = null;

            // 如果spPrincipalName里面包含\, 则表示是一个AccountId, 否则是一个SPGroup
            if (spPrincipalName.IndexOf("\\") >= 0)
                ra = new SPRoleAssignment(spPrincipalName, string.Empty, string.Empty, string.Empty);
            else
            {
                try
                {
                    SPGroup group = item.Web.Groups[spPrincipalName];

                    if (group != null)
                    {
                        ra = new SPRoleAssignment(group);
                    }
                }
                catch { }
            }

            if (ra == null)
                return;

            SPRoleDefinition rd_temp = null;

            // 特别处理的地方：rd在创建并添加到Web.RoleDefinitions之后， 还不能直接使用rd这个变量，可能是复制了一个副本，然后再添加到Collection中去的，所以这里需要从Collection中查找副本， 然后再对其进行引用
            // 不可以直接写为： ra.RoleDefinitionBindings.Add(rd) ; 会报错称不能使用Web中未定义的RD
            // 这个问题的本质原因在于创建类RoleDefinition之后, 前面拿到的SPWeb不会同步更新, 重新打开SPWeb就可以解决问题
            for (int index = 0; index < item.Web.RoleDefinitions.Count; index++)
                if (item.Web.RoleDefinitions[index].Name.ToLower() == rd.Name.ToLower())
                    rd_temp = item.Web.Site.RootWeb.RoleDefinitions[index];
            ra.RoleDefinitionBindings.Add(rd_temp);

            item.BreakRoleInheritance(false);
            item.RoleAssignments.Add(ra);
        }

        /// <summary>
        /// 取消某个SPListItem从父节点继承的权限
        /// </summary>
        /// <param name="item"></param>
        public static void BreakAndClearRoleInheritance(SPListItem item)
        {
            //item.ParentList.ParentWeb.AllowUnsafeUpdates = true;

            SPWeb web = item.ParentList.ParentWeb;
            web.AllowUnsafeUpdates = true ;

            try
            {
                item.BreakRoleInheritance(true);
            }
            catch { }

            web.AllowUnsafeUpdates = true;

            int count_ra = item.RoleAssignments.Count;

            // 如果某个RoleAssignment的Member为系统管理员, 则不要删除之
            for (int index = count_ra - 1; index >= 0; index--)
                if (item.RoleAssignments[index].Member.Name.ToLower() != "SHAREPOINT\\system".ToLower())
                    item.RoleAssignments.Remove(index);
        }



        public static void AddViewListItemPermission(SPListItem item, params string[] loginNames)
        {
            if (loginNames == null || loginNames.Length == 0)
                return;

            SPRoleDefinition rd = GetSPRoleDefinition(item.Web, SPRoleDefinationName_ViewListItem);

            if (rd == null)
                rd = CreateViewListItemsRoleDefinition(item.Web);

            foreach (string loginName in loginNames)
                if (!string.IsNullOrEmpty(loginName))
                    AddRoleAssignmentToListItem(item, loginName, rd);
        }

        public static void SetRightInfoFormListItem_Manage(SPListItem item, params string[] loginNames)
        {
            if (loginNames == null || loginNames.Length == 0)
                return;

            SPRoleDefinition rd = GetSPRoleDefinition(item.Web, SPRoleDefinationName_ManageListItem);

            if (rd == null)
                rd = CreateManageListItemRoleDefinition(item.Web);

            foreach (string loginName in loginNames)
                if (!string.IsNullOrEmpty(loginName))
                    AddRoleAssignmentToListItem(item, loginName, rd);
        }

        //public static void SetRightInfoFormListItem_Manage(string itemUrl, params string[] loginNames)
        //{
        //    if (string.IsNullOrEmpty(itemUrl) || loginNames == null || loginNames.Length == 0)
        //        return;

        //    SPSite site = null;
        //    SPWeb web = null;

        //    try
        //    {
        //        site = new SPSite(itemUrl);
        //        web = site.OpenWeb();
        //        SPListItem item = web.GetListItem(itemUrl);
        //        SetRightInfoFormListItem_Manage(item, loginNames);
        //    }
        //    finally
        //    {
        //        if (web != null) web.Close();
        //        if (site != null) site.Close();
        //    }

        //}

        //public static void SetRightInfoFormListItem_ViewOnly(string itemUrl, string[] loginNames)
        //{
        //    if (string.IsNullOrEmpty(itemUrl) || loginNames == null || loginNames.Length == 0)
        //        return;

        //    SPSite site = null;
        //    SPWeb web = null;

        //    try
        //    {
        //        site = new SPSite(itemUrl);
        //        web = site.OpenWeb();
        //        SPListItem item = web.GetListItem(itemUrl);
        //        SetRightInfoFormListItem_ViewOnly(item, loginNames);
        //    }
        //    finally
        //    {
        //        if (web != null) web.Close();
        //        if (site != null) site.Close();
        //    }  
        //}




    }
}
