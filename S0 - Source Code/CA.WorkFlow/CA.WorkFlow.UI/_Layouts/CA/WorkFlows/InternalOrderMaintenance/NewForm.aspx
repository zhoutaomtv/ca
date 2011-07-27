<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" EnableEventValidation="false" MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.InternalOrderMaintenance.NewForm" %>

<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register TagPrefix="QFC" Namespace="QuickFlow.UI.Controls" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Internal Order Maintenance
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Internal Order Maintenance
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
    <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="New">
        <div class="ca-workflow-form-buttons noPrint">
            <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="InternalOrderMaintenanceWF" runat="server" Text="Submit" />
            <QFC:StartWorkflowButton ID="StartWorkflowButton2" WorkflowName="InternalOrderMaintenanceWF" runat="server" Text="Save" CausesValidation="false" />
            <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
        </div>
        <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" />
        <SharePoint:FormDigest ID="FormDigest1" runat="server">
        </SharePoint:FormDigest>
    </qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>