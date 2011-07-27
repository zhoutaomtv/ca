<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs"
MasterPageFile="~/_Layouts/CA/Layout.Master"
Inherits="CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="SelectVendor.ascx" TagName="DataForm" TagPrefix="uc2" %>
<%@ Register TagPrefix="QFC" Namespace="QuickFlow.UI.Controls" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Non Trade Supplier Setup & Maintenance
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.custom.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Non Trade Supplier Setup & Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons float-left noPrint">
                <button class="ca-copy-button">Copy from history</button>
            </div>
            <div class="ca-workflow-form-buttons noPrint">
                <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="true" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="false" />
                <input type="button" value="Cancel" onclick="window.location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx';" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true"  />
            <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
            </cc1:TaskTraceList>
            <SharePoint:FormDigest ID="FormDigest2" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
        <uc2:DataForm ID="dfSelectVendor" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>