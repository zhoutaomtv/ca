
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="CA.SharePoint.Web.TestPage" Title="User Detail" %>

    <%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div class="left">
       <uc1:AllTasks ID="AllTasks1" runat="server" />
</div>
</asp:Content>