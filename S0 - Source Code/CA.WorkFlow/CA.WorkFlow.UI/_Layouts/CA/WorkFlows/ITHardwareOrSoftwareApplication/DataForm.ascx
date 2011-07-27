<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs"
    Inherits="CA.WorkFlow.UI.ITHardwareOrSoftwareApplication.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<script language="javascript" type="text/javascript">
    var tempCost = 0;
    var temptxt = 0;
    function AddCost(obj) {
        if (Trim(obj.value) != "" && !IsFloat(Trim(obj.value))) {
            alert("Invalid data format!");
            //obj.value="";
            return false;
        }
        else {
            var SumCost = 0;
            SumCost = parseFloat(tempCost) + parseFloat(Trim(obj.value) == "" ? 0 : Trim(obj.value));
            document.getElementById("<%= txtSumCost.ClientID%>").value = formatFloat(SumCost, 2);
            tempCost = 0;
        }
    }
    function SubCost(obj) {
        var TotleCost = Trim(document.getElementById("<%= txtSumCost.ClientID%>").value);
        if (Trim(obj.value) != "" && IsFloat(Trim(obj.value))) {
            if (TotleCost != "") {
                var SumCost = parseFloat(TotleCost);
                tempCost = formatFloat(SumCost - parseFloat(obj.value), 2);
            }
            else {
                tempCost = formatFloat(parseFloat(obj.value), 2);
            }
            temptxt = formatFloat(parseFloat(obj.value), 2);
            //alert(tempCost);
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
            alert("Invalid data format!");
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
        if (Trim(document.getElementById("<%= txtName.ClientID%>").value) == "") {
            alert("Please supply a name!");
            document.getElementById("<%= txtName.ClientID%>").focus();
            return false;
        }
        if (Trim(document.getElementById("<%= txtDept.ClientID%>").value) == "") {
            alert("Please supply a department!");
            document.getElementById("<%= txtDept.ClientID%>").focus();
            return false;
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
<table width="100%" border="0" id="tbldispaly" runat="server" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="30%">
            Choose Employee 选择员工：
        </th>
        <td>
            <CAControls:CAPeopleFinder ID="CAPeopleFinder" runat="server" AllowTypeIn="true"
                MultiSelect="false" AutoPostBack="true" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="25%" class="th2">
            <p>
                Name</p>
            <p>
                姓名<span class="red">*</span></p>
        </th>
        <th width="25%" class="th2">
            <p>
                Department</p>
            <p>
                部门<span class="red">*</span></p>
        </th>
        <th width="25%" class="th2">
            <p>
                Date</p>
            <p>
                时间</p>
        </th>
    </tr>
    <tr>
        <td>
            <input type="text" class="input_ts4" id="txtName" runat="server" />
        </td>
        <td>
            <input type="text" class="input_ts4" id="txtDept" runat="server" />
        </td>
        <td>
            <input type="text" class="input_ts4" id="txtDate" runat="server" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="5%"><div align="center">
            <asp:ImageButton runat="server" ID="imgbtnAddItem" ImageUrl="../images/pixelicious_001.png"
                style="height: 18px" onclick="imgbtnAddItem_Click" /></div>
        </th>
        <th width="70%" class="th2">
            <p>
                Hardware or Software Name</p>
            <p>
                硬件或软件名称</p>
        </th>
        <th width="25%" class="th2">
            <p>
                Cost</p>
            <p>
                价格</p>
        </th>
    </tr>
    <asp:Repeater ID="rptItRequest" runat="server" 
        onitemcommand="rptItRequest_ItemCommand" 
        onitemdatabound="rptItRequest_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td width="5%"><div align="center"><asp:ImageButton ID="ImageButton1" CommandName="delete" CommandArgument="ID" runat="server"
                        ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" /></div></td>
                <td align="center">
                    <asp:TextBox ID="txtHardName" runat="server" CssClass="input_ts4"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="txtCost" runat="server" CssClass="input_ts4" onblur="AddCost(this)" onfocus="SubCost(this)"></asp:TextBox></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptItRequestDiplay" runat="server" 
        onitemdatabound="rptItRequestDiplay_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td colspan="2">
                    <%#Eval("HardwareOrSoftwareName")%>
                </td>
                <td>
                    <%#Eval("Cost")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr>
        <th colspan="2">
            Total Cost:
        </th>
        <td>
            <asp:TextBox ID="txtSumCost" runat="server" CssClass="input_ts4" Enabled="false"></asp:TextBox>
        </td>
    </tr>
    <tr id="trFOCO" runat="server" style="display:none">
        <th colspan="2">
            Needs CFO Approval(是否需要CFO审批):
        </th>
        <td><input id="chkBox" runat="server" type="checkbox" /></td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th class="th2">
            <p>
                Reason for Request</p>
            <p>
                申请理由</p>
        </th>
    </tr>
    <tr>
        <td align="center">
            <textarea id="txtarea" style="width:450px" rows="4" class="input_ts4" runat="server"></textarea>
        </td>
    </tr>
</table>
<input type="hidden" runat="server" id="hdApplyUser" />
<input type="hidden" runat="server" id="hdSearchUser" />
