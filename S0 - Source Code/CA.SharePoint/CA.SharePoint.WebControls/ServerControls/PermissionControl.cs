using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections.Specialized;
using Microsoft.SharePoint;

namespace CA.SharePoint.WebControls
{
    public class PermissionControl:Control
    {
        private string _permissionGroups=string.Empty;

        /// <summary>
        /// allow "," "，",";"三种分割
        /// </summary>
        public string PermissionGroups
        {
            get { return _permissionGroups; }
            set { _permissionGroups = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.Visible = false;
            string[] groups = this.PermissionGroups.Split(new char[] { ';', ',', '，' });

            if (groups.Length > 0)
            {
                foreach (string tmpGroup in groups)
                {
                    if (SharePointUtil.IsCurrentUserInGroup(tmpGroup))
                    {
                        this.Visible = true;
                        break;
                    }
                }
            }

            base.OnPreRender(e);
        }
    }
}
