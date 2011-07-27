<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.SupplierReinspection.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/SingleFileUpload.ascx" TagPrefix="uc" TagName="SingleFileUpload" %>
<script language="javascript" type="text/javascript">
    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
</script>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflow_table4">
    <tr>
        <th  align="right">
            Tracking Number 编号
             &nbsp;
            <asp:Label ID="lblWorkflowNumber" runat="server" Text=""></asp:Label>
        </th>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflow_table3">
    <tr>
        <th class="th2">
            <p>
                Order Details File<span class="ms-formvalidation"> *</span></p>
            <p>
                订单明细文件</p>
        </th>
    </tr>
    <tr>
        <td>
            <uc:SingleFileUpload ID="ucFileUpload" runat="server" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th colspan="6" class="th2">
            <p><b>PO Items</b></p>
            <p>PO 条目</p>
        </th>
    </tr>
    <asp:Repeater ID="rptPODetails" runat="server" OnItemCommand="rptPODetails_ItemCommand"
        OnItemDataBound="rptPODetails_ItemDataBound">
        <HeaderTemplate>
            <tr>
                <th>
                    <div align="center">
                        <asp:ImageButton runat="server" ID="btnAddDetail" CommandName="add" ImageUrl="../images/pixelicious_001.png"
                            Width="18" />
                    </div>
                </th>
                <th>
                    <p>
                        PO Number<span class="ms-formvalidation"> *</span></p>
                    <p>
                        PO编号</p>
               </th>
                <th>
                    <p>
                        Amount<span class="ms-formvalidation"> *</span></p>
                    <p>
                        金额</p>
                </th>
                <th>
                    <p>
                        Process Date</p>
                    <p>
                        处理日期</p>
                </th>
                <th>
                    <p>
                        Invoice Number</p>
                    <p>
                        发票编号</p>
                </th>
                <th>
                    <p>
                        Document Number</p>
                    <p>
                        文档编号</p>
                </th>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:ImageButton ID="btnDeleteDetail" CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png"
                        Width="18" />
                </td>
                <td>
                    <asp:TextBox ID="txtPONumber" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptPODetailsUpdate" runat="server" OnItemDataBound="rptPODetailsUpdate_ItemDataBound">
        <HeaderTemplate>
            <tr>
                <th>
                    <p>
                        PO Number</p>
                    <p>
                        PO编号</p>
               </th>
                <th>
                    <p>
                        Amount</p>
                    <p>
                        金额</p>
                </th>
                <th>
                    <p>
                        Process Date<span class="ms-formvalidation"> *</span></p>
                    <p>
                        处理日期</p>
                </th>
                <th>
                    <p>
                        Invoice Number<span class="ms-formvalidation"> *</span></p>
                    <p>
                        发票编号</p>
                </th>
                <th>
                    <p>
                        Document Number<span class="ms-formvalidation"> *</span></p>
                    <p>
                        文档编号</p>
                </th>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("PONumber")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Amount")%>
                    &nbsp;
                </td>
                <td>
                    <cc1:CADateTimeControl ID="cdtProcessDate" runat="server" DateOnly="true" />
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="95%"></asp:TextBox>
                </td>
                <td style="width: 22%">
                    <asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%"></asp:TextBox>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptPODetailsDisplay" runat="server">
        <HeaderTemplate>
            <tr>
                <th>
                    <p>
                        PO Number</p>
                    <p>
                        PO编号</p>
               </th>
                <th>
                    <p>
                        Amount</p>
                    <p>
                        金额</p>
                </th>
                <th>
                    <p>
                        Process Date</p>
                    <p>
                        处理日期</p>
                </th>
                <th>
                    <p>
                        Invoice Number</p>
                    <p>
                        发票编号</p>
                </th>
                <th>
                    <p>
                        Document Number</p>
                    <p>
                        文档编号</p>
                </th>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("PONumber")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Amount")%>&nbsp;
                </td>
                <td>
                    <%# Eval("ProcessDate")%>&nbsp;
                </td>
                <td>
                    <%# Eval("InvoiceNumber")%>&nbsp;
                </td>
                <td>
                    <%# Eval("DocumentNumber")%>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
