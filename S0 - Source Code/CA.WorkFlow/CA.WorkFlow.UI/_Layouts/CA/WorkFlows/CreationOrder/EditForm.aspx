<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.CreationOrder.EditForm" %>

<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="QFC" Namespace="QuickFlow.UI.Controls" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Master Data Creation Form-Internal Order
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Master Data Creation Form-Internal Order
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
    <qfl:listformcontrol id="ListFormControl2" runat="server" formmode="New">
        <div class="ca-workflow-form-buttons noPrint">
            <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="true" />
            <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="false" />
            <input type="button" value="Cancel" onclick="window.location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
        </div>
        <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" ClientValidationMethod="return validate();" />
        <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
        </cc1:TaskTraceList>
        <SharePoint:FormDigest ID="FormDigest2" runat="server">
        </SharePoint:FormDigest>
    </qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>