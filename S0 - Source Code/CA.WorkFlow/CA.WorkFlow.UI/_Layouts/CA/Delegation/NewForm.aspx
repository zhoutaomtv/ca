<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" EnableEventValidation="false" MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.Delegation.NewForm" %>

<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Delegation Management</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Delegation Management</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
    <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" />
</asp:Content>