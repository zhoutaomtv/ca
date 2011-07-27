<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.CreationOrder.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<div id="ca-workflow-non-trade">
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Internal Order Number
                <br />
                订单编号
            </td>
            <td class="value">
                <QFL:FormField ID="Order_Number" runat="server" FieldName="Order Number" ControlMode="Edit">
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
                <QFL:FormField ID="Internal_Order_Type" runat="server" FieldName="Internal Order Type" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Description
                <br />
                订单描述
            </td>
            <td class="value">
                <QFL:FormField ID="Description" runat="server" FieldName="Description" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Business Area(Branch)
                <br />
                业务范围
            </td>
            <td class="value">
                <QFL:FormField ID="Business_Area" runat="server" FieldName="Business Area" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Original Value
                <br />
                原始价值
            </td>
            <td class="value">
                <QFL:FormField ID="Origin_Value" runat="server" FieldName="Original Value" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Effective Date
                <br />
                生效日期
            </td>
            <td class="value">
                <QFL:FormField ID="Effective_Date" runat="server" FieldName="Effective Date" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        
        <tr>
            <td class="label align-center">
                Expired Date
                <br />
                失效日期
            </td>
            <td class="value">
                <QFL:FormField ID="Expired_Date" runat="server" FieldName="Expired Date" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center">
                Attachment
                <br />
                预算明细
            </td>
            <td class="value">
                <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
                </QFL:FormAttachments>
            </td>
        </tr>
    </table>
</div>
<% if (this.RequireValidation) {%>
<script type="text/javascript">
    function validate(event) {
        var error = '';

        var et = event.srcElement || event.target;
        if (jQuery.trim(et.value) === 'Reject') {
            return true;
        }

        if (ca.util.emptyString($('#<%=this.Order_Number.ClientID%>_ctl00_ctl00_TextField').val())) {
            error = '- Please fill in the Order Number field.\n';
        }

        if (error) {
            alert(error);
        }

        return error.length === 0;
    }

    function beforeSubmit(event) {
        return validate(event);
    }
</script>
<% }%>