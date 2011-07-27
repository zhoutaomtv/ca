<%@ Assembly Name="CA.SharePoint.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09605105b1332138" %>

<%@ Page MasterPageFile="~/_layouts/application.master" Language="C#" AutoEventWireup="true"
    CodeBehind="CA_MySetting.aspx.cs" Inherits="CA.SharePoint.Web.CA_MySetting" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="OSRVWC" Namespace="Microsoft.Office.Server.WebControls" Assembly="Microsoft.Office.Server, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SPSWC" Namespace="Microsoft.SharePoint.Portal.WebControls"
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SEARCHWC" Namespace="Microsoft.Office.Server.Search.WebControls"
    Assembly="Microsoft.Office.Server.Search, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content runat="server" ID="c1" ContentPlaceHolderID="PlaceHolderMain">
    <sharepoint:cssregistration name="portal.css" runat="server" />
    <img src="/_layouts/images/trans.gif" height="8" width="1" alt=""><br clear="all">
    <spswc:profilepropertyloader runat="server" />
    <table width="1px" cellpadding="0" cellspacing="0">
        <tr>
            <td width="100%">
                <spswc:profileeditor id="ProfileEditor" runat="server" />
            </td>
            <td width="20">
                &nbsp;
            </td>
        </tr>
    </table>
    <sharepoint:formdigest runat="server" id="FormDigest" />
</asp:Content>
