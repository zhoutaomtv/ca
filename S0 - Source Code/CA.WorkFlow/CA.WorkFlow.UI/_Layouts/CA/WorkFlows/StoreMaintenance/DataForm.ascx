<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.StoreMaintenance.DataForm" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <td style="text-align:right;" >
            <asp:Label runat="server" ID="lblex" Visible="false" ></asp:Label>
        </td>
    </tr>
    <tr>
        <th style="text-align:left;">
            Tracking number 编号:  <asp:Label runat="server" ID="lblWorkflowNumber"></asp:Label>
        </th>
        <th>
            用户名: <asp:Label runat="server" ID="lblLoginName"></asp:Label>
        </th>
    </tr>
</table>
<asp:Panel runat="server" ID="Panel1">
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table">
    <tr class="workflow_table">
        <th width="30%">
            <p>Type</p>
            <p>类型</p>
        </th>
        <th width="24%" style="text-align:left;">
            <asp:DropDownList runat="server" ID="ddlType1" Width="100">
                <asp:ListItem Value="Service"></asp:ListItem>
                <asp:ListItem Value="Capex"></asp:ListItem>
            </asp:DropDownList>
        </th>
        <th width="28%">
            <p>Cost Center</p>
            <p>成本中心<span class="red">*</span></p>
        </th>
        <th width="24%" style="text-align:left;">
            <asp:DropDownList runat="server" ID="ddlCostCenter" Width="150">
            </asp:DropDownList>
        </th>
    </tr>
    <tr class="workflow_table">
        <th>
            <p>Budget Approved</p>
            <p>预算</p></th>
        <th style="text-align:left;">
            <p>
                <asp:DropDownList runat="server" ID="ddlBudgetApproved" Width="100">
                    <asp:ListItem Value="Yes"></asp:ListItem>
                    <asp:ListItem Value="No"></asp:ListItem>
                </asp:DropDownList>
            </p>
        </th>
        <th width="28%">
            <p>Budget Value </p>                                                
            <p>预算价值</p></th>
        <th width="24%" style="text-align:left;">
            <asp:TextBox runat="server" ID="txtBudgetValue" class="input_ts4"></asp:TextBox>
        </th>
    </tr>
    
</table>
</asp:Panel>

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table">
    
    <tr class="workflow_table">
        <th>
            <div align="center">
                <asp:ImageButton runat="server" ID="btnAdd1" ImageUrl="../images/pixelicious_001.png" OnClick="btnAdd1_Click" Width="18"/>
            </div>
        </th>
        <th>
            <p>Item </p>
            <p>序号</p></th>
        <th>
            <p>Reason of request</p>
            <p>理由请求<span class="red">*</span></p>
        </th>
        <th>
            <p>Description</p>
            <p>描述</p>
        </th>
        <th>
            <p>Remark</p>
            <p>备注</p>
        </th>
    </tr>
    
    <asp:Repeater ID="rpt1" runat="server" 
        OnItemCommand="rpt1_ItemCommand" 
        onitemdatabound="rpt1_ItemDataBound">
        <ItemTemplate>
            <tr class="workflow_table">
                <td>
                <div align="center">
                    <asp:ImageButton ID="btnDelete1" CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" />
                </div>
                </td>
                <td>
                    <asp:Label ID="lbl11" runat="server"></asp:Label>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txt12" runat="server" CssClass="input_ts4" TextMode="MultiLine"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txt13" runat="server" CssClass="input_ts4" TextMode="MultiLine"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txt14" runat="server" CssClass="input_ts4" TextMode="MultiLine"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    
    <asp:Repeater ID="rpt1Display" runat="server">
        <ItemTemplate>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <%# Eval("seq")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Reason")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Description")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Remark")%>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
    
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table">
        <tr class="workflow_table">
            <th>
                <div align="center">
                    <asp:ImageButton runat="server" ID="btnAdd2" ImageUrl="../images/pixelicious_001.png" OnClick="btnAdd2_Click" Width="18"/>
                </div>
            </th>
            <th><p>Item </p>
                <p>序号</p></th>
            <th><p>Reason of request</p>
                <p>理由请求<span class="red">*</span></p></th>
            <th><p>Unit price</p>
                <p>单价</p></th>
            <th><p>Quantity</p>
                <p>数量</p></th>
            <th><p>Subtotal</p>
                <p>总价</p></th>
        </tr>
       <asp:Repeater ID="rpt2" runat="server" 
        OnItemCommand="rpt2_ItemCommand" 
        onitemdatabound="rpt2_ItemDataBound">
        <ItemTemplate>
            <tr class="workflow_table">
                <td>
                    <div align="center">
                        <asp:ImageButton ID="btnDelete2" CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" />
                    </div>
                </td>
                <td>
                    <asp:Label ID="lbl21" runat="server"></asp:Label>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txt22" runat="server" CssClass="input_ts4" TextMode="MultiLine"></asp:TextBox>
                    &nbsp;
                </td>
                <td style="text-align:right;">
                    <asp:TextBox ID="txt23" runat="server" CssClass="input_ts4"  ></asp:TextBox>
                    &nbsp;
                </td>
                <td style="text-align:right;">
                    <asp:TextBox ID="txt24" runat="server" CssClass="input_ts4" ></asp:TextBox>
                    &nbsp;
                </td>
                <td style="text-align:right;"><!--
                    <asp:Label ID="lbl25" runat="server"></asp:Label>-->
                    <asp:TextBox ID="txt25" runat="server" CssClass="input_ts4" ></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    
    <asp:Repeater ID="rpt2Display" runat="server">
        <ItemTemplate>
            <tr class="workflow_table">
                <td>&nbsp;</td>
                <td>
                    <%# Eval("seq")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Reason")%>
                    &nbsp;
                </td>
                <td style="text-align:right;">
                    <%# Eval("Price")%>
                    &nbsp;
                </td>
                <td style="text-align:right;">
                    <%# Eval("Quantity")%>
                    &nbsp;
                </td>
                <td style="text-align:right;">
                    <%# Eval("Total")%>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    
        <tr class="workflow_table">
            <td>&nbsp;</td>
            <th>&nbsp;</th>
            <th>&nbsp;</th>
            <th>&nbsp;</th>
            <th>&nbsp;</th>
            <th>&nbsp;</th>
        </tr>
        <tr class="workflow_table">
            <td>&nbsp;</td>
            <th colspan="5">
                Payment term:
                100% paid after acceptance.
            </th>
        </tr>
    </table>
    


                                

<script type ="text/javascript" >
    

    function validForm() {
        
        return true;
    }

    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
    
    function formatFloat(src, pos) {
        return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
    }
    
    function Trim(s) {
        var ss = s.replace(/^[\s]+/g, "");
        ss = ss.replace(/[\s]+$/g, "");
        return ss;
    }
    function IsFloat(value) {
        value = Trim(value.toString());
        var re = /^\d{1,}$|\.\d{1,}$/;
        var re2 = /^-\d{1,}$|\.\d{1,}$/;
        var r = value.match(re);
        var r2 = value.match(re2);
        return r != null || r2 != null;
    }

    function IsInt(value) {
        value = Trim(value.toString());
        var re = /^\d{0,}$/;
        var re2 = /^-\d{0,}$/;
        var r = value.match(re);
        var r2 = value.match(re2);
        return r != null || r2 != null;
    }
    
    
</script>