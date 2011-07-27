<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs"
    Inherits="CA.WorkFlow.UI.ConstructionPurchasing.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<script language="javascript" type="text/javascript">
    var tempCost = 0;
    var temptxt = 0;
    function AddCost(obj) {
        if (Trim(obj.value) != "" && !IsFloat(Trim(obj.value))) {
            alert("Please input Float!");
            return false;
        }
        else {
            var SumCost = 0;
            SumCost = parseFloat(tempCost) + parseFloat(Trim(obj.value) == "" ? 0 : Trim(obj.value));
            document.getElementById("<%=txtSumCost.ClientID %>").value = formatFloat(SumCost, 2);
            tempCost = 0;
        }
    }
    function SubCost(obj) {
        var TotleCost = Trim(document.getElementById("<%=txtSumCost.ClientID %>").value);
        if (Trim(obj.value) != "" && IsFloat(Trim(obj.value))) {
            if (TotleCost != "") {
                var SumCost = parseFloat(TotleCost);
                tempCost = formatFloat(SumCost - parseFloat(obj.value), 2);
            }
            else {
                tempCost = formatFloat(parseFloat(obj.value), 2);
            }
            temptxt = formatFloat(parseFloat(obj.value), 2);
        }
        else {
            tempCost = formatFloat(parseFloat(TotleCost == "" ? 0 : (TotleCost-temptxt)), 2);
        }
    }
    function formatFloat(src, pos) {
        return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
    }
    function CheckCost(obj) {
        if (Trim(obj.value) != "" && !IsFloat(Trim(obj.value))) {
            alert("Please input Float!");
            return false;
        }
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
    function CheckValue(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
        var obj = document.getElementById("<%=ddlBudget.ClientID %>");
        if (obj.options[obj.selectedIndex].value == "Yes") {
            if (Trim(document.getElementById("<%=txtBudgetValue.ClientID %>").value) == "") {
                alert("Please supply a budget value!");
                document.getElementById("<%=txtBudgetValue.ClientID %>").focus();
                return false;
            }
        }
    }
    function ShowColor() {
        var obj = document.getElementById("<%=ddlBudget.ClientID %>");
        if (obj.options[obj.selectedIndex].value == "Yes") {
            spanColor.style.display = "";
        }
        else {
            spanColor.style.display = "none";
        }
    }
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3"
    style="display: none">
    <tr>
        <th width="15%" id="thDisplay" runat="server">
            ID:
        </th>
        <td width="20%" id="trDisplay" runat="server">
            <asp:Label ID="lblWorkflowNumber" runat="server"></asp:Label>
        </td>
        <th width="15%">
            用户名:
        </th>
        <td width="20%">
            <asp:Label ID="lblLogUser" runat="server"></asp:Label>
        </td>
        <th width="15%">
            申请时间：
        </th>
        <td width="20%">
            <asp:Label ID="lblApplyDate" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="25%" class="th2">
            <p>
                Type</p>
            <p>
                类型</p>
        </th>
        <td width="25%">
            <asp:DropDownList ID="ddlType" runat="server" Width="120px">
                <asp:ListItem Value="Service">Service</asp:ListItem>
                <asp:ListItem Value="Expense">Expense</asp:ListItem>
                <asp:ListItem Value="Capex">Capex</asp:ListItem>
            </asp:DropDownList>
        </td>
        <th width="25%" class="th2">
            <p>
                Budget Approved</p>
            <p>
                预算</p>
        </th>
        <td width="25%">
            <asp:DropDownList ID="ddlBudget" runat="server" Width="120px" onchange="ShowColor();">
                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                <asp:ListItem Value="No">No</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th class="th2">
            <p>
                Currency Type</p>
            <p>
                货币类型</p>
        </th>
        <td>
            <asp:DropDownList ID="ddlCurrency" runat="server" Width="120px">
                <asp:ListItem Value="RMB">RMB</asp:ListItem>
                <asp:ListItem Value="US$">US$</asp:ListItem>
            </asp:DropDownList>
        </td>
        <th class="th2">
            <p>
                Budget Value</p>
            <p>
                预算价值&nbsp;<span class="red" id="spanColor" style="display:block" runat="server">*</span></p>
        </th>
        <td>
            <asp:TextBox ID="txtBudgetValue" runat="server" CssClass="input_ts4"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th class="th2">
            <p>
                Produce and Delivery Date</p>
            <p>
                生成和交付日期</p>
        </th>
        <td>
            <CAControls:CADateTimeControl runat="server" ID="txtPandDDate" DateOnly="true" >
            </CAControls:CADateTimeControl> 
        </td>
        <th class="th2">
            <p>
                Cost Center</p>
            <p>
                成本中心</p>
        </th>
        <td>
            <asp:DropDownList ID="ddlCostCenter" runat="server" Width="120px">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3" id="tblFees" runat="server">
    <tr>
        <th colspan="3" class="th2">
            <p>
                Service fees 硬件或软件介绍</p>
        </th>
    </tr>
    <tr>
        <th width="33%" class="th2">
             <p>
                Installation</p>
             <p>
                安装</p>
        </th>
        <th width="33%" class="th2">
            <p>
                Freight</p>
            <p>
                货物</p>
        </th>
        <th width="34%" class="th2">
            <p>
                Packaging</p>
            <p>
                包装</p>
        </th>
    </tr>
    <tr>
        <td align="center">
            <asp:TextBox ID="txtInstallation" runat="server" CssClass="input_ts4"></asp:TextBox></td>
        <td align="center">
            <asp:TextBox ID="txtFreight" runat="server" CssClass="input_ts4"></asp:TextBox></td>
        <td align="center">
            <asp:TextBox ID="txtPackaging" runat="server" CssClass="input_ts4"></asp:TextBox></td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3" id="tblDetails">
    <tr>
        <th width="5%"><div align="center">
            <asp:ImageButton runat="server" ID="imgbtnAddItem" ImageUrl="../images/pixelicious_001.png"
                style="height: 18px" onclick="imgbtnAddItem_Click" /></div>
        </th>
        <th width="7%" class="th2">
            <p>
                Item</p>
        </th>
        <th width="8%" class="th2">
            <p>
                Item Code</p>
            <p>
                编码</p>
        </th>
        <th width="20%" class="th2">
             <p>
                Discription</p>
            <p>
                描述</p>
        </th>
        <th width="10%" class="th2">
            <p>
                Quantity</p>
            <p>
                数量</p>
        </th>
        <th width="10%" class="th2">
            <p>
                Unit</p>
            <p>
                单位</p>
        </th>
        <th width="10%" class="th2">
            <p>
                Unit Price</p>
            <p>
                单价</p>
        </th>
        <th width="10%" class="th2">
            <p>
                Total Price</p>
            <p>
                总价</p>
        </th>
        <th width="20%" class="th2">
            <p>
                Remark</p>
            <p>
                备注</p>
        </th>
    </tr>
    <asp:Repeater ID="rptItRequest" runat="server" 
        onitemcommand="rptItRequest_ItemCommand" 
        onitemdatabound="rptItRequest_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td><div align="center"><asp:ImageButton ID="ImageButton1" CommandName="delete" CommandArgument="ID" runat="server"
                        ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" /></div></td>
                <td align="center"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                <td align="center">
                    <asp:TextBox ID="txtItemCode" runat="server" CssClass="input_ts4"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtDiscription" runat="server" CssClass="input_ts4" TextMode="MultiLine" Rows="2"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="input_ts4"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtUnit" runat="server" CssClass="input_ts4"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input_ts4"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtTotalPrice" runat="server" CssClass="input_ts4" onblur="AddCost(this)" onfocus="SubCost(this)">></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtRemark" runat="server" CssClass="input_ts4" TextMode="MultiLine" Rows="2"></asp:TextBox></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptItRequestDiplay" runat="server" 
        onitemdatabound="rptItRequestDiplay_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                <td>
                    <%#Eval("ItemCode")%>
                </td>
                <td>
                    <%#Eval("Discription")%>
                </td>
                <td>
                    <%#Eval("Quantity")%>
                </td>
                <td>
                    <%#Eval("Unit")%>
                </td>
                <td>
                    <%#Eval("UnitPrice")%>
                </td>
                <td>
                    <%#Eval("TotalPrice")%>
                </td>
                <td>
                    <%#Eval("Remark")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr>
        <td colspan="6"></td>
        <th class="th2" align="right"><p>Total：</p></th>
        <td colspan="2"><asp:TextBox ID="txtSumCost" runat="server" CssClass="input_ts4" Enabled="false"></asp:TextBox></td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3" id="tblChk" runat="server" style="display:none">
    <tr>
        <th width="20%" class="h2">
            <p>Places the order</p>
        </th>
        <td>
            <asp:CheckBox ID="chkPlacesOrder" runat="server" />
        </td>
    </tr>
    <tr id="trChk" runat="server" style="display:none">
        <th width="20%" class="h2">
            <p>Order Handover</p>
        </th>
        <td>
            <asp:CheckBox ID="chkOrderHandover" runat="server" />
        </td>
    </tr>
</table>
<input type="hidden" runat="server" id="hdApplyUser"  />
