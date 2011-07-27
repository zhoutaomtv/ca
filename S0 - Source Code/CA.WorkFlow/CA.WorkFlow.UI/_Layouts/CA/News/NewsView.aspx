<%@ Assembly Name="CA.SharePoint.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09605105b1332138" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="NewsView.aspx.cs" Inherits="CA.WorkFlow.NewsView"
    Title="Untitled Page" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    <asp:Literal runat="server" ID="litTitle"></asp:Literal>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="left">
        <div class="user">
            <em>
                <asp:Literal runat="server" ID="litTime"></asp:Literal></em>
            <h3>
                <asp:Literal runat="server" ID="litCreated"></asp:Literal></h3>
        </div>
        <div>
            <p>
                <asp:Literal runat="server" ID="litBody"></asp:Literal></p>
        </div>
        <div runat="server" id="divFile">
            <p>
                <iframe src="<%=FileUrl %>" style="width: 590px; overflow-x: scroll; overflow-y: scroll;
                    height: 600px;"></iframe>
                </p>
        </div>
    </div>
</asp:Content>
