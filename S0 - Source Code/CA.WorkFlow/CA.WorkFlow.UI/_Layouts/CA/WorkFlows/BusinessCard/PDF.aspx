<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="CA.WorkFlows.BusinessCard.PDF" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <p>
        <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Display" /></p>
</asp:Content>
