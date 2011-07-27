<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="CA.SharePoint.Web.UserDetail" Title="User Detail" %>

    <%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div class="left">
        <uc1:UserDetails ID="ucUser" runat="server" />
        <asp:Label ID="lblmessage" runat="server" Text="&nbsp;&nbsp;The user doesn't exist in domain." Visible="false"  Font-Size= "Large" ForeColor="Blue" ></asp:Label>
</div>
</asp:Content>
