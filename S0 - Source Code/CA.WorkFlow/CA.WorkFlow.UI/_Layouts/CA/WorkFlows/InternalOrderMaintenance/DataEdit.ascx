<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.InternalOrderMaintenance.DataEdit" %>
<%@ Register TagPrefix="QFL" Namespace="QuickFlow.UI.ListForm" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<div id="ca-workflow-order-maintain">
    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Order Number
                        <br />
                        订单编号
                    </td>
                    <td class="value">
                        <div class="float-left w35">
                            <QFL:FormField ID="Order_Number" runat="server" FieldName="Order Number" ControlMode="Edit">
                            </QFL:FormField>
                        </div>
                        <div class="ca-workflow-form-buttons float-left">
                            <asp:Button ID="btnFinder" runat="server" Text="Find" CssClass="form-button" OnClick="BtnFinder_Click" />
                        </div>
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
                    <td class="label align-center">
                        Budget Change History 预算变更历史
                    </td>
                </tr>
                <tr>
                    <td class="label align-center">
                        <SharePoint:SPGridView ID="SPGridView1" GridLines="Both" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="2">
                            <footerstyle cssclass="GridViewFooterStyle" />
                            <rowstyle cssclass="GridViewRowStyle" />
                            <selectedrowstyle cssclass="GridViewSelectedRowStyle" />
                            <pagerstyle cssclass="GridViewPagerStyle" />
                            <alternatingrowstyle cssclass="GridViewAlternatingRowStyle" />
                            <headerstyle forecolor="red" cssclass="GridViewHeaderStyle" />
                            <columns>
                        <asp:BoundField DataField="Change_x0020_Date" HeaderText="变更日期" />
                        <asp:BoundField DataField="Value_x0020_Change" HeaderText="变更价值" />
                        <asp:BoundField DataField="Value_x0020_After_x0020_Change" HeaderText="变更后价值" />
                    </columns>
                        </SharePoint:SPGridView>
                        <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="SPGridView1">
                        </SharePoint:SPGridViewPager>
                        <asp:HiddenField ID="hidLastValue" runat="server" />
                    </td>
                </tr>
            </table>
            <table class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        After Value
                        <br />
                        变更后价值
                    </td>
                    <td class="value">
                        <QFL:FormField ID="Value_After_Change" runat="server" FieldName="Value After Change" ControlMode="Edit">
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
                        <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Edit">
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
                        <QFL:FormField ID="Reason_For_Change_Value" runat="server" FieldName="Reason For Change Value" ControlMode="Edit">
                        </QFL:FormField>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                $('#ca-workflow-order-maintain').find('input:text,input:file,textarea').die('focus blur').live({
                    focus: function () {
                        $(this).addClass('focus');
                    },
                    blur: function () {
                        $(this).removeClass('focus');
                    }
                });

                $('#ca-workflow-order-maintain input:text').die('keypress').live('keypress', function (event) {
                    return ca.util.disableEnterKey(event);
                });

                function beforeSubmit(sender) {
                    return validate();
                }

                function validate() {
                    var error = '';
                    if (ca.util.emptyString($('#<%=this.Order_Number.ClientID%>_ctl00_ctl00_TextField').val())) {
                        error += '- Please fill in the Order Number field.\n';
                    }

                    var afterValue = $('#<%=this.Value_After_Change.ClientID%>_ctl00_ctl00_TextField').val();

                    if (ca.util.emptyString(afterValue)) {
                        error += '- Please fill in the Value After Change field.\n';
                    }
                    else if (!/^\d+\.?\d{0,2}$/.test(afterValue)) {
                        error += '- Please fill in the Value After Change field with a valid number like "8.88".\n';
                    }

                    if (ca.util.emptyString($('#<%=this.Reason_For_Change_Value.ClientID%>_ctl00_ctl00_TextField').val())) {
                        error += '- Please fill in the Reason For Change Value field.\n';
                    }

                    if (error) {
                        alert(error);
                    }

                    return error.length === 0;
                }
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFinder" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
        </Triggers>
    </asp:UpdatePanel>
</div>