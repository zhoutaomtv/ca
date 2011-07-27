namespace CA.WorkFlow.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.Design.WebControls;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [Designer(typeof (MossMenuDesigner))]
    [ToolboxData("<{0}:CAMossMenu runat=\"server\" />")]
    public class CAMossMenu : Menu
    {
        private readonly Dictionary<string, MenuItem> menuItems = new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase);

        private string idPrefix;

        public CAMossMenu()
        {
            this.PerformTargetBinding = true;
            this.SelectStaticItemsOnly = true;
            this.CustomSelectionEnabled = true;
        }

        /// <summary>
        ///   Controls whether or not the control performs custom selection/highlighting.
        /// </summary>
        [Category("Behavior")]
        public bool CustomSelectionEnabled { get; set; }

        /// <summary>
        ///   Controls whether only static items may be selected or if
        ///   dynamic (fly-out) items may be selected too.
        /// </summary>
        [Category("Behavior")]
        public bool SelectStaticItemsOnly { get; set; }

        /// <summary>
        ///   Controls whether or not to bind the Target property of any menu
        ///   items to the Target property in the SiteMapNode's Attributes
        ///   collection.
        /// </summary>
        [Category("Behavior")]
        public bool PerformTargetBinding { get; set; }

        /// <summary>
        ///   Gets the ClientID of this control.
        /// </summary>
        public override string ClientID
        {
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            get
            {
                if (this.idPrefix == null)
                {
                    this.idPrefix = SPUtility.GetNewIdPrefix(this.Context);
                }

                return SPUtility.GetShortId(this.idPrefix, this);
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void OnMenuItemDataBound(MenuEventArgs e)
        {
            base.OnMenuItemDataBound(e);

            if (this.CustomSelectionEnabled)
            {
                this.menuItems[e.Item.NavigateUrl] = e.Item;
            }

            if (this.PerformTargetBinding)
            {
                // try to bind to the Target property if the data item is a SiteMapNode
                var smn = e.Item.DataItem as SiteMapNode;

                if (smn != null)
                {
                    string target = smn["Target"];

                    if (!string.IsNullOrEmpty(target))
                    {
                        e.Item.Target = target;
                    }
                }
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.Page.ClientScript.RegisterStartupScript(
                typeof (CAMossMenu),
                "overrideMenu_HoverStatic",
                "if (typeof(overrideMenu_HoverStatic) == 'function' && typeof(Menu_HoverStatic) == 'function')\n" +
                "{\n" +
                "_spBodyOnLoadFunctionNames.push('enableFlyoutsAfterDelay');\n" +
                "Menu_HoverStatic = overrideMenu_HoverStatic;\n" +
                "}\n",
                true);

            this.Page.ClientScript.RegisterStartupScript(
                typeof (CAMossMenu),
                "MenuHttpsWorkaround_" + this.ClientID,
                this.ClientID + "_Data.iframeUrl='/_layouts/images/blank.gif';",
                true);

            if (this.Orientation == Orientation.Vertical &&
                ((string.IsNullOrEmpty(this.StaticPopOutImageUrl) && this.StaticEnableDefaultPopOutImage) ||
                 (string.IsNullOrEmpty(this.DynamicPopOutImageUrl) && this.DynamicEnableDefaultPopOutImage)))
            {
                SPWeb currentWeb = SPContext.Current.Web;
                if (currentWeb != null)
                {
                    bool isBidiWeb = SPUtility.IsRightToLeft(currentWeb, currentWeb.Language);

                    string arrowUrl = "/_layouts/images/" + (isBidiWeb ? "largearrowleft.gif" : "largearrowright.gif");

                    if (string.IsNullOrEmpty(this.StaticPopOutImageUrl) && this.StaticEnableDefaultPopOutImage)
                    {
                        this.StaticPopOutImageUrl = arrowUrl;
                    }

                    if (string.IsNullOrEmpty(this.DynamicPopOutImageUrl) && this.DynamicEnableDefaultPopOutImage)
                    {
                        this.DynamicPopOutImageUrl = arrowUrl;
                    }
                }
            }

            var dataSource = this.GetDataSource() as SiteMapDataSource;
            SiteMapProvider provider = (dataSource != null) ? dataSource.Provider : null;

            if (provider == null)
            {
                return;
            }

            if (this.CustomSelectionEnabled)
            {
                MenuItem selectedMenuItem = this.SelectedItem;
                SiteMapNode currentNode = provider.CurrentNode;

                while (selectedMenuItem == null && currentNode != null)
                {
                    this.menuItems.TryGetValue(currentNode.Url, out selectedMenuItem);

                    currentNode = currentNode.ParentNode;
                }

                if (this.SelectStaticItemsOnly)
                {
                    while (selectedMenuItem != null && selectedMenuItem.Depth >= this.StaticDisplayLevels)
                    {
                        selectedMenuItem = selectedMenuItem.Parent;
                    }

                    if (selectedMenuItem != null && selectedMenuItem.Selectable)
                    {
                        selectedMenuItem.Selected = true;
                    }
                }
            }
        }
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    [SuppressMessage("Microsoft.Security", "CA2117:AptcaTypesShouldOnlyExtendAptcaBaseTypes")]
    public sealed class MossMenuDesigner : MenuDesigner
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void DataBind(BaseDataBoundControl dataBoundControl)
        {
            try
            {
                dataBoundControl.DataBind();
            }
            catch
            {
                base.DataBind(dataBoundControl);
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override string GetDesignTimeHtml()
        {
            var menu = (Menu) this.ViewControl;
            int oldDisplayLevels = menu.MaximumDynamicDisplayLevels;

            string designTimeHtml;

            try
            {
                menu.MaximumDynamicDisplayLevels = 0;

                designTimeHtml = base.GetDesignTimeHtml();
            }
            catch (Exception e)
            {
                designTimeHtml = this.GetErrorDesignTimeHtml(e);
            }
            finally
            {
                menu.MaximumDynamicDisplayLevels = oldDisplayLevels;
            }

            return designTimeHtml;
        }
    }
}