<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs"
MasterPageFile="~/_Layouts/CA/Layout.Master"
Inherits="CA.WorkFlow.UI.InternalOrderMaintenance.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Internal Order Maintenance
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
     <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Internal Order Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
    <QFL:ListFormControl ID="ListFormControl2" runat="server" FormMode="New">
        <div class="ca-workflow-form-buttons noPrint">
            <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="true" />
            <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="false" />
            <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
        </div>
        <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" />
        <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
        </cc1:TaskTraceList>
        <SharePoint:FormDigest ID="FormDigest2" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
</asp:Content>