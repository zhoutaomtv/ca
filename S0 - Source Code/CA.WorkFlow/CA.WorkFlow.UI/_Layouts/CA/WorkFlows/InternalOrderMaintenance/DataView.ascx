<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs"
Inherits="CA.WorkFlow.UI.InternalOrderMaintenance.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<div id="ca-workflow-non-trade">
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Order Number
                <br />
                订单编号
            </td>
            <td class="value">
                <QFL:FormField ID="Order_Number" runat="server" FieldName="Order Number" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Internal Order Type
                <br />
                订单类型
            </td>
            <td class="value">
                <asp:Label ID="Internal_Order_Type" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Description
                <br />
                订单描述
            </td>
            <td class="value">
                <asp:Label ID="Description" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Business Area(Branch)
                <br />
                业务范围
            </td>
            <td class="value">
                <asp:Label ID="Business_Area" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Attachment
                <br />
                原始预算明细
            </td>
            <td class="value">
                <asp:Label ID="Attachment1" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Original Value
                <br />
                原始价值
            </td>
            <td class="value">
                <asp:Label ID="Origin_Value" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td class="label align-center">
                Effective Date
                <br />
                生效日期
            </td>
            <td class="value">
           
                 <asp:Label ID="Effective_Date" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        
        <tr>
            <td class="label align-center">
                Expired Date
                <br />
                失效日期
            </td>
            <td class="value">
          
                 <asp:Label ID="Expired_Date" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="3" class="label align-center">
                Budget Change History 预算变更历史
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                <SharePoint:SPGridView ID="SPGridView1" GridLines="Both" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="3">
                    <FooterStyle CssClass="GridViewFooterStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridViewPagerStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <HeaderStyle ForeColor="red" CssClass="GridViewHeaderStyle" />
                    <Columns>
                        <asp:BoundField DataField="Change_x0020_Date" HeaderText="变更日期" />
                        <asp:BoundField DataField="Value_x0020_Change" HeaderText="变更价值" />
                        <asp:BoundField DataField="Value_x0020_After_x0020_Change" HeaderText="变更后价值" />
                    </Columns>
                </SharePoint:SPGridView>
                <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="SPGridView1">
                </SharePoint:SPGridViewPager>
                <asp:HiddenField ID="hidLastValue" runat="server" />
            </td>
        </tr>
    </table>

    <table class="ca-workflow-form-table full-width">
        <%--<tr>
            <td class="label align-center w25">
                Value Change Type
                <br />
                价值变动类型
            </td>
            <td class="value">
                <QFL:FormField ID="Value_Change_Type" runat="server" FieldName="Value Change Type" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>--%>
        <tr>
            <td class="label align-center w25">
                Ater Value
                <br />
                变更后价值
            </td>
            <td class="value">
                <QFL:FormField ID="Value_After_Change" runat="server" FieldName="Value After Change" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Attachment
                <br />
                变更后预算明细
            </td>
            <td class="value">
                <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
                </QFL:FormAttachments>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Reason For Change Value
                <br />
                变动原因
            </td>
            <td class="value">
                <QFL:FormField ID="Reason_For_Change_Value" runat="server" FieldName="Reason For Change Value" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
    </table>
</div>