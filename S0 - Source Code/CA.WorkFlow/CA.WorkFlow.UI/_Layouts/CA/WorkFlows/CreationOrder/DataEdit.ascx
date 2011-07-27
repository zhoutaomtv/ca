<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.CreationOrder.DataEdit" %>
<%@ Register TagPrefix="QFL" Namespace="QuickFlow.UI.ListForm" %>
<div id="ca-workflow-creation-order">
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Internal Order Type
                <br />
                订单类型
            </td>
            <td class="value">
                <QFL:FormField ID="Internal_Order_Type" runat="server" FieldName="Internal Order Type" ControlMode="Edit">
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
                <QFL:FormField ID="Description" runat="server" FieldName="Description" ControlMode="Edit">
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
                <QFL:FormField ID="Business_Area" runat="server" FieldName="Business Area" ControlMode="Edit">
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
                <QFL:FormField ID="Origin_Value" runat="server" FieldName="Original Value" ControlMode="Edit">
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
                <QFL:FormField ID="Effective_Date" runat="server" FieldName="Effective Date" ControlMode="Edit">
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
                <QFL:FormField ID="Expired_Date" runat="server" FieldName="Expired Date" ControlMode="Edit">
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
                <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Edit">
                </QFL:FormAttachments>
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    $('#<%= this.Effective_Date.ClientID %>_ctl00_ctl00_DateTimeField_DateTimeFieldDate').attr('contentEditable', 'false');
    $('#<%= this.Expired_Date.ClientID %>_ctl00_ctl00_DateTimeField_DateTimeFieldDate').attr('contentEditable', 'false');

    $('#ca-workflow-creation-order').find('input:text,input:file,textarea').focus(function () {
        $(this).addClass("focus");
    }).blur(function () {
        $(this).removeClass("focus");
    });
    $('#ca-workflow-creation-order').find('input:text,input:file').keypress(function (event) {
        return ca.util.disableEnterKey(event);
    });

    function beforeSubmit(sender) {
        return validate();
    }

    function validate() {
        var error = '';

        if (ca.util.emptyString($('#<%= this.Description.ClientID %>_ctl00_ctl00_TextField').val())) {
            error += '- Please fill in the Description field.\n';
        }

        if (ca.util.emptyString($('#<%= this.Business_Area.ClientID %>_ctl00_ctl00_TextField').val())) {
            error += '- Please fill in the Business Area field.\n';
        }

        var originalValue = $('#<%= this.Origin_Value.ClientID %>_ctl00_ctl00_TextField').val();

        if (ca.util.emptyString(originalValue)) {
            error += '- Please fill in the Original Value field.\n';
        }
        else if (!/^\d+\.?\d{0,2}$/.test(originalValue)) {
            error += '- Please fill in the Original Value field with a valid number like "8.88".\n';
        }

        //        var physicalYear = $('#<%= this.Expired_Date.ClientID %>_ctl00_ctl00_TextField').val();

//        if (ca.util.emptyString(physicalYear)) {
//            error += '- Please fill in the Effective Physical Year field.\n';
//        }
//        else if (!/^\d{4}$/.test(physicalYear)) {
//            error += '- Please fill in the Effective Physical Year field with a valid year like "2011".\n';
//        }

        if (error) {
            alert(error);
        }

        return error.length === 0;
    }
</script>