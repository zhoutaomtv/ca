<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance.NewForm" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Non Trade Supplier Setup & Maintenance
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="New">
            <div class="ca-workflow-form-buttons float-left noPrint">
                <button class="ca-copy-button">Copy from history</button>
            </div>
            <div class="ca-workflow-form-buttons noPrint">
                <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="NonTradeSupplierSetupMaintenanceWF" runat="server" Text="Submit" />
                <QFC:StartWorkflowButton ID="StartWorkflowButton2" WorkflowName="NonTradeSupplierSetupMaintenanceWF" runat="server" Text="Save" CausesValidation="false" />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" />
            <SharePoint:FormDigest ID="FormDigest1" runat="server">
            </SharePoint:FormDigest>
        </qfl:listformcontrol>
        <uc2:DataForm ID="dfSelectVendor" runat="server" />
    </div>
    <script type="text/javascript">
        function dispatchAction(sender) {
            if ($('#ca-workflow-non-trade input:radio').eq(1).attr('checked')) {
                updateValues(sender);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            *please attach officially chopped copies of Business License, Tax Registration License,
            and Bank information from vendors<br />
            <br />
            *Payment term less than 30 days required CFO’s special approval<br />
            <br />
            *if change vendor information, please attach officially chopped request from vendor
            <br />
            <br />
            *if block/release vendor, please attach reason in the box above
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
