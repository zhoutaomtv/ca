<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoreOrganization.ascx.cs"
    Inherits="CA.SharePoint.WebControls.MoreOrganization" %>
<%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<table width="612" border="0" cellspacing="0" cellpadding="0" class="Department_News">
    <tr>
        <td colspan="2" width="47%" valign="top" class="line">
            <span class="txt1">
                <%=strDeptName %>
                Department</span>
        </td>
        <td width="47%" valign="middle" class="line">
            <div class="viewbutton">
                <a href="javascript:window.history.back(-1);" class="view">Back</a></div>
        </td>
</tr>
<tr>
 <td colspan="3" valign="top">
<asp:Repeater ID="rptEmployees" runat="server" OnItemDataBound="rptEmployees_ItemDataBound">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <uc1:UserDetails ID="UserDetailsList" runat="server" />
                </td>
            </tr>
        </table>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>
</td>
    </tr>
</table>