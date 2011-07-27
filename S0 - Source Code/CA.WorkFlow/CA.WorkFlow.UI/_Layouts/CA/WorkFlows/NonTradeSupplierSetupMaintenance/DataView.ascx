<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm"
    TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<div id="ca-workflow-non-trade">
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Type
                <br />
                类型
            </td>
            <td class="value">
                <QFL:FormField ID="Record_Type" runat="server" FieldName="Record Type" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="2" class="label align-center w25">
                General Information 基本信息
            </td>
        </tr>
        <tr id="trVendorId" runat="server">
            <td class="label w25 align-center">
                Supplier ID
            </td>
            <td class="value">
                <QFL:FormField ID="Vendor_ID" runat="server" FieldName="Vendor ID" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Name of Supplier
                <br />
                (please state full name)
            </td>
            <td>
                <table class="inner-table">
                    <tr>
                        <td class="label align-center w15">
                            中文
                        </td>
                        <td class="value">
                            <QFL:FormField ID="CN_Name_of_Vendor" runat="server" FieldName="CN Name of Vendor" ControlMode="Display">
                            </QFL:FormField>
                        </td>
                    </tr>
                    <tr>
                        <td class="label align-center w15">
                            English
                        </td>
                        <td class="value">
                            <QFL:FormField ID="EN_Name_of_Vendor" runat="server" FieldName="EN Name of Vendor" ControlMode="Display">
                            </QFL:FormField>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Address of Supplier
            </td>
            <td>
                <table class="inner-table">
                    <tr>
                        <td class="label align-center w15">
                            中文
                        </td>
                        <td>
                            <table class="inner-table">
                                <tr>
                                    <td colspan="4" class="value">
                                        <QFL:FormField ID="CN_Address_of_Vendor" runat="server" FieldName="CN Address of Vendor" ControlMode="Display">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label align-center w15">
                                        城市 /
                                        <br />
                                        国家
                                    </td>
                                    <td class="label w35">
                                        <QFL:FormField ID="CN_City_Country" runat="server" FieldName="CN City And Country" ControlMode="Display">
                                        </QFL:FormField>
                                    </td>
                                    <td class="label align-center w15">
                                        邮编
                                    </td>
                                    <td class="value">
                                        <QFL:FormField ID="CN_Postal_Code" runat="server" FieldName="CN Postal Code" ControlMode="Display">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="label align-center last">
                            English
                        </td>
                        <td>
                            <table class="inner-table">
                                <tr>
                                    <td colspan="4" class="value">
                                        <QFL:FormField ID="EN_Address_of_Vendor" runat="server" FieldName="EN Address of Vendor" ControlMode="Display">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                                <tr class="last">
                                    <td class="label align-center w15">
                                        City /
                                        <br />
                                        Country
                                    </td>
                                    <td class="label w35">
                                        <QFL:FormField ID="EN_City_Country" runat="server" FieldName="EN City And Country" ControlMode="Display">
                                        </QFL:FormField>
                                    </td>
                                    <td class="label align-center w15">
                                        Postal code
                                    </td>
                                    <td class="value">
                                        <QFL:FormField ID="EN_Postal_Code" runat="server" FieldName="EN Postal Code" ControlMode="Display">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="2" class="label align-center w25">
                Contact Information 联系信息
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Company Telephone NO.
            </td>
            <td class="value">
                <QFL:FormField ID="Company_Telephone_No" runat="server" FieldName="Company Telephone No" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Fax NO.
            </td>
            <td class="value">
                <QFL:FormField ID="Fax_Number" runat="server" FieldName="Fax Number" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Email Address
            </td>
            <td class="value">
                <QFL:FormField ID="EMail" runat="server" FieldName="E-Mail" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="4" class="label align-center w25">
                Contact Person 联系人信息
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Last Name
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Last_Name_of_Contact_Person" runat="server" FieldName="Last Name of Contact Person" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                First Name
            </td>
            <td class="value">
                <QFL:FormField ID="First_Name_of_Contact_Person" runat="server" FieldName="First Name of Contact Person" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Position
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Position_of_Contact_Person" runat="server" FieldName="Position of Contact Person" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Department
            </td>
            <td class="value">
                <QFL:FormField ID="Department" runat="server" FieldName="Department" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Phone NO.
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Phone_of_Contact_Person" runat="server" FieldName="Phone of Contact Person" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Email Address
            </td>
            <td class="value">
                <QFL:FormField ID="Email_Address_of_Contact_Person" runat="server" FieldName="Email Address of Contact Person" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="4" class="label align-center w25">
                Payment Term 帐期信息
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Payment Term
                <br />
                (as agreed with vendor)
            </td>
            <td class="value">
                <span id="payment-term-span">
                    <QFL:FormField ID="Payment_Term" runat="server" FieldName="Payment Term" ControlMode="Display">
                    </QFL:FormField>
                </span>                
                <span>days after receipt of invoice</span>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Currency
            </td>
            <td class="value" colspan="3">
                <span id="currency-span">
                    <QFL:FormField ID="Currency" runat="server" FieldName="Currency" ControlMode="Display">
                    </QFL:FormField>
                </span>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="4" class="label align-center w25">
                Account Information 帐户信息
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Business License NO.
            </td>
            <td class="label w25">
                <QFL:FormField ID="Business_License_No" runat="server" FieldName="Business License No" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Tax Registration License NO.
            </td>
            <td class="value">
                <QFL:FormField ID="Tax_Registration_License_No" runat="server" FieldName="Tax Registration License No" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Name of Bank
            </td>
            <td class="label w25">
                <QFL:FormField ID="Name_of_Bank" runat="server" FieldName="Name of Bank" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Branch Name
            </td>
            <td class="value">
                <QFL:FormField ID="Branch_Name" runat="server" FieldName="Branch Name" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Address of Bank
            </td>
            <td class="label w25">
                <QFL:FormField ID="Address_of_Bank" runat="server" FieldName="Address of Bank" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Country of Bank
            </td>
            <td class="value">
                <QFL:FormField ID="Country_of_Bank" runat="server" FieldName="Country of Bank" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Bank Key
            </td>
            <td class="label w25">
                <QFL:FormField ID="Bank_Key" runat="server" FieldName="Bank Key" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Swift Code
            </td>
            <td class="value">
                <QFL:FormField ID="Swift_Code" runat="server" FieldName="Swift Code" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Bank Account NO.
            </td>
            <td class="label w25">
                <QFL:FormField ID="Bank_Account_No" runat="server" FieldName="Bank Account No" ControlMode="Display">
                </QFL:FormField>
            </td>
            <td colspan="2" class="value">
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width" id="tblChangeReason" runat="server">
        <tr>
            <td colspan="2" class="label align-center w25">
                Reason for Change 变更原因
            </td>
        </tr>
        <tr>
            <td class="label w25 align-center">
                Reason for Change
            </td>
            <td class="value">
                <QFL:FormField ID="Reason_For_Change" runat="server" FieldName="Reason for Change" ControlMode="Display">
                </QFL:FormField>
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

        if (ca.util.emptyString($('#<%=this.Vendor_ID.ClientID%>_ctl00_ctl00_TextField').val())) {
            error = '- Please fill in the Supplier ID field.\n';
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
<script type="text/javascript">
    $(function () {
        if (jQuery.trim($('#payment-term-span').text()).length == 0) {
            $('#payment-term-span').next("span").addClass('hidden');
            $('#currency-span').addClass('hidden');
        }
    });
</script>