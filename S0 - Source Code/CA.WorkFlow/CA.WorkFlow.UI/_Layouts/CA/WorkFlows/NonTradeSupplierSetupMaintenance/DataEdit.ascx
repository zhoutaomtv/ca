<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance.DataEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<div id="ca-workflow-non-trade">
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Type<br />
                类型
            </td>
            <td class="value">
                <QFL:FormField ID="Record_Type" runat="server" FieldName="Record Type" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table hidden full-width">
        <tr>
            <td colspan="2" class="warning">
                For Change Application: if information of related supplier is not available, please just fill-in the supplier name and the changed information.
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
                <div class="float-left w25">
                    <QFL:FormField ID="Vendor_ID" runat="server" FieldName="Vendor ID" ControlMode="Edit">
                    </QFL:FormField>
                </div>
                <div class="float-left">
                    <a href="javascript:void(0)" id="btnLoadInfo" title="Find Supplier">
                        <img src="/_layouts/CAResources/themeCA/images/load.gif" alt="Find Supplier" width="18px" />
                    </a>
                    <a href="javascript:void(0)" title="Search Dialog ..." >
                        <img class="ca-copy-button" src="/_layouts/CAResources/themeCA/images/find.ico" alt="Search Dialog ..." width="20px" />
                    </a>
                </div>                
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
                            中文<span class="clr-red">*</span>
                        </td>
                        <td class="value">
                            <QFL:FormField ID="CN_Name_of_Vendor" runat="server" FieldName="CN Name of Vendor" ControlMode="Edit">
                            </QFL:FormField>
                        </td>
                    </tr>
                    <tr>
                        <td class="label align-center">
                            English<span class="clr-red">*</span>
                        </td>
                        <td class="value">
                            <QFL:FormField ID="EN_Name_of_Vendor" runat="server" FieldName="EN Name of Vendor" ControlMode="Edit">
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
                            中文<span class="clr-red">*</span>
                        </td>
                        <td>
                            <table class="inner-table">
                                <tr>
                                    <td colspan="4" class="value">
                                        <QFL:FormField ID="CN_Address_of_Vendor" runat="server" FieldName="CN Address of Vendor" ControlMode="Edit">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label align-center w15">
                                        城市 /
                                        <br />
                                        国家
                                    </td>
                                    <td class="label align-center w35">
                                        <QFL:FormField ID="CN_City_Country" runat="server" FieldName="CN City And Country" ControlMode="Edit">
                                        </QFL:FormField>
                                    </td>
                                    <td class="label align-center w15">
                                        邮编
                                    </td>
                                    <td class="value">
                                        <QFL:FormField ID="CN_Postal_Code" runat="server" FieldName="CN Postal Code" ControlMode="Edit">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="label align-center last">
                            English<span class="clr-red">*</span>
                        </td>
                        <td>
                            <table class="inner-table">
                                <tr>
                                    <td colspan="4" class="value">
                                        <QFL:FormField ID="EN_Address_of_Vendor" runat="server" FieldName="EN Address of Vendor" ControlMode="Edit">
                                        </QFL:FormField>
                                    </td>
                                </tr>
                                <tr class="last">
                                    <td class="label align-center w15">
                                        City /
                                        <br />
                                        Country
                                    </td>
                                    <td class="label align-center w35">
                                        <QFL:FormField ID="EN_City_Country" runat="server" FieldName="EN City And Country" ControlMode="Edit">
                                        </QFL:FormField>
                                    </td>
                                    <td class="label align-center w15">
                                        Postal code
                                    </td>
                                    <td class="value">
                                        <QFL:FormField ID="EN_Postal_Code" runat="server" FieldName="EN Postal Code" ControlMode="Edit">
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
                Company Telephone NO.<span class="clr-red">*</span>
            </td>
            <td class="value">
                <QFL:FormField ID="Company_Telephone_No" runat="server" FieldName="Company Telephone No" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Fax NO.
            </td>
            <td class="value">
                <QFL:FormField ID="Fax_Number" runat="server" FieldName="Fax Number" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Email Address
            </td>
            <td class="value">
                <QFL:FormField ID="EMail" runat="server" FieldName="E-Mail" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="4" class="label align-center w25">
                Contact Person Information 联系人信息
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Last Name<span class="clr-red">*</span>
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Last_Name_of_Contact_Person" runat="server" FieldName="Last Name of Contact Person" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                First Name<span class="clr-red">*</span>
            </td>
            <td class="value">
                <QFL:FormField ID="First_Name_of_Contact_Person" runat="server" FieldName="First Name of Contact Person" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Position
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Position_of_Contact_Person" runat="server" FieldName="Position of Contact Person" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Department<span class="clr-red">*</span>
            </td>
            <td class="value">
                <QFL:FormField ID="Department" runat="server" FieldName="Department" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Phone NO.<span class="clr-red">*</span>
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Phone_of_Contact_Person" runat="server" FieldName="Phone of Contact Person" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Email Address
            </td>
            <td class="value">
                <QFL:FormField ID="Email_Address_of_Contact_Person" runat="server" FieldName="Email Address of Contact Person" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <table class="ca-workflow-form-table full-width">
        <tr>
            <td colspan="4" class="label align-center w25">
                Payment Term Information 帐期信息
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Payment Term<span class="clr-red">*</span>
                <br />
                (as agreed with Supplier)
            </td>
            <td class="value">
                <div class="float-left w25">
                    <QFL:FormField ID="Payment_Term" runat="server" FieldName="Payment Term" ControlMode="Edit">
                    </QFL:FormField>
                </div>
                <div class="float-left">
                    days after receipt of invoice</div>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Currency<span class="clr-red">*</span>
            </td>
            <td class="value" colspan="3">
                <QFL:FormField ID="Currency" runat="server" FieldName="Currency" ControlMode="Edit">
                </QFL:FormField>
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
                Business License NO.<span class="clr-red">*</span>
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Business_License_No" runat="server" FieldName="Business License No" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Tax Registration License NO.<span class="clr-red">*</span>
            </td>
            <td class="value">
                <QFL:FormField ID="Tax_Registration_License_No" runat="server" FieldName="Tax Registration License No" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Name of Bank<span class="clr-red">*</span>
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Name_of_Bank" runat="server" FieldName="Name of Bank" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Branch Name<span class="clr-red">*</span>
            </td>
            <td class="value">
                <QFL:FormField ID="Branch_Name" runat="server" FieldName="Branch Name" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Address of Bank
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Address_of_Bank" runat="server" FieldName="Address of Bank" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Country of Bank<span class="clr-red">*</span>
            </td>
            <td class="value">
                <QFL:FormField ID="Country_of_Bank" runat="server" FieldName="Country of Bank" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Bank Key
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Bank_Key" runat="server" FieldName="Bank Key" ControlMode="Edit">
                </QFL:FormField>
            </td>
            <td class="label align-center w25">
                Swift Code
            </td>
            <td class="value">
                <QFL:FormField ID="Swift_Code" runat="server" FieldName="Swift Code" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
        <tr class="last">
            <td class="label align-center w25">
                Bank Account NO.<span class="clr-red">*</span>
            </td>
            <td class="label align-center w25">
                <QFL:FormField ID="Bank_Account_No" runat="server" FieldName="Bank Account No" ControlMode="Edit">
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
                <QFL:FormField ID="Reason_For_Change" runat="server" FieldName="Reason for Change" ControlMode="Edit">
                </QFL:FormField>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnReload" OnClick="btnReload_Click" runat="server" CssClass="hidden" />
</div>
<script type="text/javascript">
    $('#ca-workflow-non-trade').find('input:text,input:file,textarea').focus(function () {
        $(this).addClass('focus');
    }).blur(function () {
        $(this).removeClass('focus');
    });

    $('#btnLoadInfo').click(function () {
        $('#<%= this.btnReload.ClientID %>').click();
        return false;
    });

    $('#ca-workflow-non-trade input:text').keypress(function (event) {
        return ca.util.disableEnterKey(event);
    });

    $('#ca-workflow-non-trade input:radio').change(function (event) {
        if ($(this).val() === 'ctl00') {
            $('#<%= this.tblChangeReason.ClientID %>').addClass('hidden');
            $('#<%= this.trVendorId.ClientID %>').addClass('hidden');
            $('#ca-supplier-form .ca-copy-button:first').attr('disabled', '');
            $('#ca-supplier-form .clr-red').removeClass('hidden');
        }
        else {
            $('#<%= this.tblChangeReason.ClientID %>').removeClass('hidden');
            $('#<%= this.trVendorId.ClientID %>').removeClass('hidden');
            $('#ca-supplier-form .ca-copy-button:first').attr('disabled', 'disabled');
            $('#ca-supplier-form .clr-red').addClass('hidden');
        }
        if ($('#ca-workflow-non-trade input:radio').eq(1).attr('checked')) {
            $('#ca-workflow-non-trade .ca-workflow-form-table').eq(1).removeClass('hidden');
        } else {
            $('#ca-workflow-non-trade .ca-workflow-form-table').eq(1).addClass('hidden');
        }
        $('#ca-workflow-non-trade input:text').val('');
        $('#ca-workflow-non-trade textarea').val('');
        $('#ca-workflow-non-trade select').val('RMB');
    });

    /*Keep the state of "Copy from History" button*/
    $(function () {
        if (!$('#ca-workflow-non-trade input:radio:first').attr('checked')) {
            $('#ca-supplier-form .ca-copy-button:first').attr('disabled', 'disabled');
        }
        if ($('#ca-workflow-non-trade input:radio').eq(1).attr('checked')) {
            $('#ca-workflow-non-trade .ca-workflow-form-table').eq(1).removeClass('hidden');
        }
        $('#ca-workflow-non-trade input:text').change(function (event) {
            $(this).attr('changed', 'true');
        });
        $('#ca-workflow-non-trade select').change(function (event) {
            $(this).attr('changed', 'true');
        });
    });

    function beforeSubmit(sender) {
        if (validate()) {
            if (!$('#ca-workflow-non-trade input:radio:first').attr('checked')) {
                updateValues(sender);
            }
        }
        else {
            return false;
        }
        return true;
    }

    function updateValues(sender) {
        $('#ca-workflow-non-trade input:text').each(function (event) {
            if ($(this).attr('changed') != 'true')
            {
                switch ($(this).attr('title'))
                {
                    case 'Vendor ID':
                        break;
                    case 'CN Name of Vendor':
                        break;
                    case 'EN Name of Vendor':
                        break;
                    default:
                        $(this).val('');
                }
            }
        });
        $('#ca-workflow-non-trade select').each(function (event) {
            if ($(this).attr('changed') != 'true') {
                $(this).val('RMB');
            }
        });
    }    

    function validate() {
        var error = '';

        var type = $('#ca-workflow-non-trade input:checked').val();

        if (type === 'ctl00') {
            if (ca.util.emptyString($('#<%=this.CN_Name_of_Vendor.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Name(中文) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.EN_Name_of_Vendor.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Name(English) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.CN_Address_of_Vendor.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Address(中文) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.EN_Address_of_Vendor.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Address(English) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.CN_City_Country.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Address(城市 / 国家) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.EN_City_Country.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Address(City / Country) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.CN_Postal_Code.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Address(邮编) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.EN_Postal_Code.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier Address(Postal code ) field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Company_Telephone_No.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Company Telephone NO field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Last_Name_of_Contact_Person.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Last Name of Contact Person  field.\n';
            }

            if (ca.util.emptyString($('#<%=this.First_Name_of_Contact_Person.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the First Name of Contact Person field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Department.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Department of Contact Person field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Phone_of_Contact_Person.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Phone of Contact Person field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Business_License_No.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Business License NO field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Tax_Registration_License_No.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Tax Registration License NO field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Name_of_Bank.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Bank Name field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Branch_Name.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Branch Name field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Country_of_Bank.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Country of Bank field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Bank_Account_No.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Bank Account NO field.\n';
            }

            var paymentTerm = $('#<%=this.Payment_Term.ClientID%>_ctl00_ctl00_TextField').val();

            if (!/^\d+$/.test(paymentTerm)) {
                error += '- Please fill in the Payment Term field with a valid positive number.\n';
            }
        }
        else {
            if (ca.util.emptyString($('#<%=this.Vendor_ID.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Supplier ID field.\n';
            }

            if (ca.util.emptyString($('#<%=this.Reason_For_Change.ClientID%>_ctl00_ctl00_TextField').val())) {
                error += '- Please fill in the Reason For Change field.\n';
            }
        }

        if (error) {
            alert(error);
        }

        return error.length === 0;
    };
</script>