using System;
using System.Collections.Generic;
using System.Text;

namespace CA.WorkFlow
{
    /// <summary>
    /// 权限类型
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// 查看表单
        /// </summary>
        View = 1,

        /// <summary>
        /// 编辑表单
        /// </summary>
        Edit = 2
    }

    /// <summary>
    /// 权限
    /// </summary>
    public class Permission
    {
        public string Identity;
        public PermissionType PermissionType = PermissionType.Edit;
    }

    /// <summary>
    /// 权限集合
    /// </summary>
    public class PermissionSet : List<Permission>
    {
        public void Add(string id, PermissionType pt)
        {
            this.Add(new Permission() { Identity = id, PermissionType = pt });
        }

        public void AddEditPermission(string id)
        {
            this.Add(new Permission() { Identity = id, PermissionType = PermissionType.Edit });
        }

        public void AddViewPermission(string id)
        {
            this.Add(new Permission() { Identity = id, PermissionType = PermissionType.View });
        }
    }
}
