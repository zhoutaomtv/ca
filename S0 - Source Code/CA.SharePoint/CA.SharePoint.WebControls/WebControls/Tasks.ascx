<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tasks.ascx.cs" Inherits="CA.SharePoint.WebControls.Tasks" %>
<asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
        <table width="94%" border="0" cellpadding="0" cellspacing="0" class="userinfo" align="center">
            <tr>
                <th  valign="top">
                    <span class="txt1">Tasks</span>
                </th>
                <th width="27%" valign="center" class="line">
                        <div class="viewbutton">
                            <a href="/ca/Mytasks.aspx" class="view">View More</a></div>
                </th>
            </tr> 
                <td valign="top" colspan="2">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="Tasks">

    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <th width="73%" valign="top">
                <p><a href="<%# Eval("WorkFlowUrl")%>" ><%#Eval(SPBuiltInFieldId.Title.ToString("B"))%></a></p>
            </th>
            <th width="27%" valign="top">
                <p><%#Eval(SPBuiltInFieldId.Created_x0020_Date.ToString("B"))%></p>
            </th>
        </tr>
    </ItemTemplate>    
    <FooterTemplate>
        </table></td> </tr> </table>
    </FooterTemplate>
</asp:Repeater>
