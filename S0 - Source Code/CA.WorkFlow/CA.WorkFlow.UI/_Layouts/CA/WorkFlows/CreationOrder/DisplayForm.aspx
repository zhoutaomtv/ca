<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.CreationOrder.Display" %>

<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Master Data Creation Form-Internal Order
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Master Data Creation Form-Internal Order
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <qfl:listformcontrol id="ListFormControl3" runat="server" formmode="New">
        <div class="ca-workflow-form-buttons noPrint">
            <input type="button" value="Cancel" onclick="window.history.go(-1)" />
        </div>
        <uc1:DataForm ID="DataForm1" runat="server" />
        <uc2:TaskTrace ID="TaskTrace1" runat="server" />
        <SharePoint:FormDigest ID="FormDigest3" runat="server">
        </SharePoint:FormDigest>
    </qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>