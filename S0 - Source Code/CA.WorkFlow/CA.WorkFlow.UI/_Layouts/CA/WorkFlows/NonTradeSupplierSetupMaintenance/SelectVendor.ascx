<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectVendor.ascx.cs" Inherits="CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance.SelectVendor" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<div id="ca-select-vendor" class="hidden">
    <div class="ca-workflow-form-buttons">
        <asp:Button ID="btnCopyVendor" runat="server" Text="Select" OnClick="btnCopyVendor_Click" OnClientClick="return setVal();" />
        <button id="ca-close-dialog">
            Close</button>
        <asp:Button ID="btnClose" runat="server" CssClass="hidden" OnClick="btnClose_Click" />
    </div>
    <table class="ca-workflow-form-table">
        <tr>
            <td class="label align-center w25">
                Supplier Name:
            </td>
            <td class="label align-center w25">
                <asp:TextBox ID="ENName" runat="server"></asp:TextBox>
            </td>
            <td class="label w25 align-center">
                供应商名称:
            </td>
            <td class="value align-center">
                <asp:TextBox ID="CNName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="last">
            <td colspan="4" class="value">
                <div class="ca-workflow-form-buttons">
                    <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" CssClass="button" />
                </div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <SharePoint:SPGridView id="SPGridView1" runat="server" autogeneratecolumns="False" allowpaging="True" pagesize="5" onpageindexchanging="SPGridView1_PageIndexChanging" BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                EnableTheming="False" GridLines="Horizontal">
                <alternatingrowstyle cssclass="each-row ms-alternating" />
                <rowstyle cssclass="each-row" />
                <selectedrowstyle cssclass="ms-selectednav" font-bold="True" />
                <columns>
                    <asp:BoundField DataField="Title" HeaderText="WorkFlowNumber" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Vendor_x0020_ID" HeaderText="编号" />
                    <asp:BoundField DataField="EN_x0020_Name_x0020_of_x0020_Ven" HeaderText="Supplier Name" />
                    <asp:BoundField DataField="CN_x0020_Name_x0020_of_x0020_Ven" HeaderText="供应商名称" />
                </columns>
            </SharePoint:SPGridView>
            <div class="align-center">
                <SharePoint:SPGridViewPager id="SPGridViewPager1" runat="server" gridviewid="SPGridView1">
                </SharePoint:SPGridViewPager>
            </div>
            <asp:HiddenField ID="hidSelectedWorkflowNumber" runat="server" />
            <script type="text/javascript">
                $(function () {
                    $('#ca-select-vendor').dialog({
                        title: "Select a vendor to copy",
                        autoOpen: false,
                        width: 706,
                        height: 400,
                        modal: true,
                        resizable: false,
                        draggable: false,
                        open: function (type, data) {
                            $(this).parent().appendTo('form');
                        },
                        close: function () {
                            $('#<%= this.btnClose.ClientID %>').click();
                        }
                    });
                    $('#ca-supplier-form .ca-copy-button').click(function () {
                        $('#ca-select-vendor').dialog('open');
                        return false;
                    });

                    $('#ca-close-dialog').unbind('click').click(function () {
                        $('#ca-select-vendor').dialog('close');
                        return false;
                    });

                    $('#<%= this.SPGridView1.ClientID %> tr.each-row').die('click hover').live({
                        click: function () {
                            $(this).siblings().removeClass('selected');
                            $(this).addClass('selected');
                            $('#<%= this.hidSelectedWorkflowNumber.ClientID %>').val($(this).find('td:first').text());
                        },
                        hover: function () {
                            $(this).toggleClass('hover');
                        }
                    });
                });

                function setVal() {
                    if (ca.util.emptyString($('#<%= this.hidSelectedWorkflowNumber.ClientID %>').val())) {
                        alert('Please select one record first.');
                        return false;
                    }
                    $('#ca-workflow-non-trade input:text').val('');
                    $('#ca-workflow-non-trade textarea').val('');
                    $('#ca-workflow-non-trade select').val('RMB');
                }

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
        </Triggers>
    </asp:UpdatePanel>
</div>