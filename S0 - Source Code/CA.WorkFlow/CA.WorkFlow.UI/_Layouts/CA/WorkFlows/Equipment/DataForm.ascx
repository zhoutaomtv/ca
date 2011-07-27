<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.Equipment.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflow_table3">
    <tr>
        <th>
            <div align="center">
                Filled BY HR:
                <asp:Label runat="server" ID="lblHRName"></asp:Label>
            </div>
        </th>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th width="20%">
            Employee Name
            <br />
            员工姓名<span class="red">*</span>
        </th>
        <td width="30%">
            <asp:TextBox runat="server" ID="txtEmployeeName" class="input_ts4"></asp:TextBox>
        </td>
        <th width="20%">
            On-board Date
            <br />
            入职时间
        </th>
        <td width="30%">
            <CAControls:CADateTimeControl runat="server" ID="CADateTime1" DateOnly="true">
            </CAControls:CADateTimeControl>
        </td>
    </tr>
    <tr>
        <th width="20%">
            Employee ID
            <br />
            员工编号<span class="red">*</span>
        </th>
        <td width="30%">
            <asp:TextBox runat="server" ID="txtEmployeeID" class="input_ts4"></asp:TextBox>
        </td>
        <th width="20%" rowspan="2">
            Functional Manager
            <br />
            职能经理
        </th>
        <td width="30%" rowspan="2">
            <CAControls:CAPeopleFinder ID="CAPeopleFinder2" runat="server" AllowTypeIn="true"
                MultiSelect="false" Width="200" />
        </td>
    </tr>
    <tr>
        <th width="20%">
            &nbsp; Employee Title<br />
            员工职位<span class="red">*</span>
        </th>
        <td width="30%">
            <asp:TextBox runat="server" ID="txtEmployeeTitle" class="input_ts4"></asp:TextBox>
        </td>
        
    </tr>
    <tr>
        <th width="20%">
            Department
            <br />
            部门<span class="red">*</span>
        </th>
        <td width="30%">
            <asp:TextBox runat="server" ID="txtDepartment" class="input_ts4"></asp:TextBox>
        </td>
        <th width="20%">
            Department Head
            <br />
            部门经理<span class="red">*</span>
        </th>
        <td width="30%">
            <CAControls:CAPeopleFinder ID="CAPeopleFinder1" runat="server" AllowTypeIn="true"
                MultiSelect="false" Width="200" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th colspan="2">
            <div align="center">
                Filled BY Manager</div>
        </th>
    </tr>
    <tr>
        <th width="20%">
            Computer 电脑<span class="red">*</span>
        </th>
        <td>
            <asp:RadioButtonList runat="server" ID="radComputer" RepeatDirection="Horizontal">
                <asp:ListItem Value="Desktop"></asp:ListItem>
                <asp:ListItem Value="Laptop"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th>
            Telephone 电话<span class="red">*</span>
        </th>
        <td>
            <asp:RadioButtonList runat="server" ID="radTelephone" RepeatDirection="Horizontal">
                <asp:ListItem Value="Dir.Line"></asp:ListItem>
                <asp:ListItem Value="Extension"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th valign="middle">
            SAP<span class="red">*</span>
        </th>
        <td>
            <asp:RadioButtonList runat="server" ID="radSap" RepeatDirection="Horizontal">
                <asp:ListItem Value="Yes"></asp:ListItem>
                <asp:ListItem Value="No"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th>
            E-mail 邮箱<span class="red">*</span>
        </th>
        <td>
            <asp:RadioButtonList runat="server" ID="radEmail" RepeatDirection="Horizontal">
                <asp:ListItem Value="Yes"></asp:ListItem>
                <asp:ListItem Value="No"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th>
            Remark 备注
        </th>
        <td>
            <asp:TextBox runat="server" ID="txtRemark" class="input_ts4" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
</table>

<script type="text/javascript">
    var employeeName, department, manager, employeeTitle,employeeID,radComputer,radTelephone,radSap,radEmail;
    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
    function validForm(object) {
        var str = "";
        employeeName = document.getElementById("<%= txtEmployeeName.ClientID %>").value;
        department = document.getElementById("<%= txtDepartment.ClientID %>").value;
        employeeTitle = document.getElementById("<%= txtEmployeeTitle.ClientID %>").value;
        employeeID = document.getElementById("<%= txtEmployeeID.ClientID %>").value;
        radComputer = document.getElementById("<%=radComputer.ClientID %>");
        radTelephone = document.getElementById("<%=radTelephone.ClientID %>");
        radSap = document.getElementById("<%=radSap.ClientID %>");
        radEmail = document.getElementById("<%=radEmail.ClientID %>");
        if ($.trim(employeeName) == "" && object.value == "Submit") {
            str += "Please supply a employee name.\n";
        }
        if ($.trim(employeeID) == "" && object.value == "Submit") {
            str += "Please supply a employee ID.\n";
        }
        if ($.trim(employeeTitle) == "" && object.value == "Submit") {
            str += "Please supply a employee title.\n";
        }
        if ($.trim(department) == "" && object.value == "Submit") {
            str += "Please supply a department.\n";
        }
        if (!chked(radComputer) && object.value == "Approve") {
            str += "Please supply a computer.\n";
        }
        if (!chked(radTelephone) && object.value == "Approve") {
            str += "Please supply a telephone.\n";
        }
        if (!chked(radSap) && object.value == "Approve") {
            str += "Please supply a sap.\n";
        }
        if (!chked(radEmail) && object.value == "Approve") {
            str += "Please supply a email.\n";
        }
        if (str != "") {
            alert(str.substring(0, str.length-1));
            return false;
        }
        return true;
    }
    function chked(divtbl) {
        var checkboxs = divtbl.getElementsByTagName('input');
        var chked = false;
        for (var i = 0; i < checkboxs.length; i++) {
            if (checkboxs[i].checked)
            { chked = true; break; }
        }
        if (!chked)
        { return false; }
        else
        { return true; }
    }
    
</script>

