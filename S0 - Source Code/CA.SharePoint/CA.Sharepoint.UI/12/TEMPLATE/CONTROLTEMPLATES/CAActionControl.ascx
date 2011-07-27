<%@ Register TagPrefix="codeart" Namespace="CodeArt.SharePoint.MultiLanSupport" Assembly="CodeArt.SharePoint.MultiLanSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fb342a992e9c6c52" %>
<%@ Register TagPrefix="wssuc" TagName="Welcome" Src="~/_controltemplates/Welcome.ascx" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" %>
<table width="240" border="0" align="right" cellpadding="0" cellspacing="5">
    <tr>     
        <td>
            <div align="right">
                <a href="#">
                    <wssuc:Welcome id="IdWelcome" runat="server" EnableViewState="false"></wssuc:Welcome></a></div>
        </td>
<%--        <td>
            <div align="center">
                <a href="#">
                    <codeart:LanguageSwitch runat="server" ID="languageChange" /></a></div>
        </td>
        <td>
            <div align="center">
                <a href="#">Help</a></div>
        </td>--%>
           <td>
            <div align="right">
                <a href="#">
                    <SharePoint:SiteActions runat="server" AccessKey="<%$Resources:wss,tb_SiteActions_AK%>"
                        ID="SiteActionsMenuMain" PrefixHtml="&lt;div&gt;&lt;div&gt;" SuffixHtml="&lt;/div&gt;&lt;/div&gt;"
                        MenuNotVisibleHtml="&amp;nbsp;">
                        <CustomTemplate>
                            <SharePoint:FeatureMenuTemplate ID="FeatureMenuTemplate1" runat="server" FeatureScope="Site"
                                Location="Microsoft.SharePoint.StandardMenu" GroupId="SiteActions" UseShortId="true">
                                <SharePoint:MenuItemTemplate runat="server" ID="MenuItem_Create" Text="<%$Resources:wss,viewlsts_pagetitle_create%>"
                                    Description="<%$Resources:wss,siteactions_createdescription%>" ImageUrl="/_layouts/images/Actionscreate.gif"
                                    MenuGroupId="100" Sequence="100" UseShortId="true" ClientOnClickNavigateUrl="~site/_layouts/create.aspx"
                                    PermissionsString="ManageLists, ManageSubwebs" PermissionMode="Any" />
                                <SharePoint:MenuItemTemplate runat="server" ID="MenuItem_EditPage" Text="<%$Resources:wss,siteactions_editpage%>"
                                    Description="<%$Resources:wss,siteactions_editpagedescription%>" ImageUrl="/_layouts/images/ActionsEditPage.gif"
                                    MenuGroupId="100" Sequence="200" ClientOnClickNavigateUrl="javascript:MSOLayout_ChangeLayoutMode(false);" />
                                <SharePoint:MenuItemTemplate runat="server" ID="MenuItem_Settings" Text="<%$Resources:wss,settings_pagetitle%>"
                                    Description="<%$Resources:wss,siteactions_sitesettingsdescription%>" ImageUrl="/_layouts/images/ActionsSettings.gif"
                                    MenuGroupId="100" Sequence="300" UseShortId="true" ClientOnClickNavigateUrl="~site/_layouts/settings.aspx"
                                    PermissionsString="EnumeratePermissions,ManageWeb,ManageSubwebs,AddAndCustomizePages,ApplyThemeAndBorder,ManageAlerts,ManageLists,ViewUsageData"
                                    PermissionMode="Any" />
                            </SharePoint:FeatureMenuTemplate>
                        </CustomTemplate>
                    </SharePoint:SiteActions>
                </a>
            </div>
        </td>
    </tr>
</table>
