<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentNews.ascx.cs"
    Inherits="CA.SharePoint.WebControls.DepartmentNews" %>
<table width="612" border="0" cellpadding="0" cellspacing="0" class="homepages_News">
    <tr>
        <td width="50%" valign="top">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>                  
                    <td colspan="2" valign="top" class="line">
                        <span class="txt1">
                            <%=ListName%>
                        </span>
                    </td>
                    <td width="14%" valign="center" class="line">
                        <div class="viewbutton">
                            <a href="<%=ViewUrl %>" class="view">View More</a></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <HeaderTemplate>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <th width="83%">
                                        <p>
                                            <a href="<%=NewsViewUrl %><%#Eval("id") %>">
                                        <%#Eval("Title") %>       </a></p>
                                    </th>
                                    <th width="15%">
                                        <p>
                                       <%#Eval("LastModified")%> </p>
                                     
                                    </th>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
