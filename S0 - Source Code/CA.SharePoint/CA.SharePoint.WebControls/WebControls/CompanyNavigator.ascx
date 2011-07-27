<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyNavigator.ascx.cs"
    Inherits="CA.SharePoint.WebControls.CompanyNavigator" %>
<%@ Register Assembly="CA.SharePoint.WebControls" Namespace="CA.SharePoint.WebControls"
    TagPrefix="cc1" %>
  <table width="94%" border="0" cellpadding="0" cellspacing="0" class="userinfo" align="center">
    <tr>
        <th valign="top">
            <span class="txt1">Links</span>
        </th>
        <th width="27%" align="center">
            <cc1:PermissionControl ID="PermissionControl1" runat="server">
                <a href="<%=Link %>">
                    <img src="/_layouts/CAResources/themeCA/images/cog_gos.png" alt="setup" border="0"></a>
            </cc1:PermissionControl>&nbsp;
        </th>
    </tr>
    <asp:Repeater ID="repLinks" runat="server">
        <HeaderTemplate>
            <tr>
                <td colspan="2" valign="top">
                    <div id="rightnav">
                        <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href="<%#Eval("LinkUrl")%>" target="_blank" class="rightnav">
                <%#Eval("LinkName")%> &nbsp;</a> </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul> </div></td> </tr>
        </FooterTemplate>
    </asp:Repeater>
</table>
