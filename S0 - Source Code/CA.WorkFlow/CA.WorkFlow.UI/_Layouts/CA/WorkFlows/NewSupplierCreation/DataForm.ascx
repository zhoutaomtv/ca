<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.NewSupplierCreation.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function SelectStatus() {
        var mondial = document.getElementById("<%= ddlMondial.ClientID%>").value;
        var obj = document.getElementById("<%= ddlStatus.ClientID%>");
        if (mondial == "No") {
            var opt = document.createElement("option");
            opt.innerHTML = "Factory Assessment Failed";
            opt.value = "Factory Assessment Failed";
            obj.insertBefore(opt, obj.firstChild);
            opt = document.createElement("option");
            opt.innerHTML = "Factory Assessment Success";
            opt.value = "Factory Assessment Success";
            obj.insertBefore(opt, obj.firstChild);
            opt = document.createElement("option");
            opt.innerHTML = "Factory Assessment Ongoing";
            opt.value = "Factory Assessment Ongoing";
            obj.insertBefore(opt, obj.firstChild);
            opt = document.createElement("option");
            opt.innerHTML = "Waiting Factory Assessment Form";
            opt.value = "Waiting Factory Assessment Form";
            obj.insertBefore(opt, obj.firstChild);
            obj.selectedIndex = 0;
        }
        else {
            for (var i = 0; i < 4; i++) {
                obj.options.remove(0);
            }
            //                if (obj.options[0].value == "Waiting factory assessment form") {
            //                obj.options.remove(0);
            //            }
        }
    }
    function Trim(str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    }
    function CheckValue(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");

        if (Trim(document.getElementById("<%= txtSupplier.ClientID%>").value) == "") {
            alert("Please supply a supplier!");
            document.getElementById("<%= txtSupplier.ClientID%>").focus();
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
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="25%">
            <p>
                Supplier Name</p>
            <p>
                供应商名称 <span class="red">*</span></p>
        </th>
        <td width="25%">
            <h5>
                <asp:TextBox ID="txtSupplier" runat="server" CssClass="input_ts4"></asp:TextBox>
            </h5>
        </td>
        <th width="25%">
            <p>
                Sub Division</p>
            <p>
                子部门 <span class="red">*</span></p>
        </th>
        <td width="25%">
            <asp:DropDownList ID="ddlSubDivision" Width="150px" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Is Mondial</p>
            <p>
                是否是Mondial</p>
        </th>
        <td colspan="3">
            <asp:DropDownList ID="ddlMondial" runat="server" Width="100px" onchange="SelectStatus();">
                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                <asp:ListItem Value="No">No</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trShow" style="display: none" runat="server">
        <th>
            <p>
                Status</p>
            <p>
                状态</p>
        </th>
        <td colspan="3">
            <asp:DropDownList ID="ddlStatus" runat="server" Width="230px">
                <%--<asp:ListItem Value="Supplier Application Form Received">Supplier Application Form Received</asp:ListItem>
                <asp:ListItem Value="Contract Signed">Contract Signed & System Setup OK</asp:ListItem>--%>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Upload Supplier Profile</p>
            <p>
                上传供应商档案</p>
        </th>
        <td colspan="3">
            &nbsp;<QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
    <tr id="updateList" runat="server" style="display: none">
        <th>
            <p>
                Status History</p>
            <p>
                修改历史</p>
        </th>
        <td colspan="3">
            <asp:Label ID="lblStatusList" runat="server" Width="100%"></asp:Label>
        </td>
    </tr>
</table>
