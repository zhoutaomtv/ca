<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveForm.aspx.cs" MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.InternalOrderMaintenance.ApproveForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Internal Order Maintenance
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Internal Order Maintenance
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
    <QFL:ListFormControl ID="ListFormControl3" runat="server" FormMode="New">
        <div class="ca-workflow-form-buttons noPrint">
            <QFC:ActionsButton ID="Actions" runat="server" />
            <QFC:MoreApproveButton runat="server" ID="More" />
            <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
        </div>
        <table class="ca-workflow-form-table full-width">
            <tr>
                <td class="label align-center w25">
                    Comments<br />
                    批注:
                </td>
                <td class="value" id="comment-task">
                    <QFL:TaskPanel runat="server" ID="task1">
                        <QFL:CommentTaskField runat="server" ID="CommentTaskField1" />
                    </QFL:TaskPanel>
                </td>
            </tr>
        </table>
        <uc1:DataForm ID="DataForm1" runat="server" />
        <uc2:TaskTrace ID="TaskTrace1" runat="server" />
        <SharePoint:FormDigest ID="FormDigest3" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
    <script type="text/javascript">
        function dispatchAction(sender) {
            if (sender.value === 'Reject') {
                if (ca.util.emptyString($('#comment-task textarea').val())) {
                    alert('Please fill in the Reject Comments.');
                    return false;
                }
            }
            else if (sender.value === 'To CFO') {
                return confirm('Requires CFO approval?');
            }
            return true;
        }
    </script>
</asp:Content>