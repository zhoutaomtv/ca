<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.ChangeRequest.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
    
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
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
    <tr><td style="text-align:center;" colspan="4">General Information</td></tr>
    <tr >
        <th width="20%">
            <p>Priority</p>
            <p>优先级</p>
        </th>
        <td width="30%" style="text-align:left;">
            <asp:DropDownList runat="server" ID="ddlPriority"  Width="98%">
                <asp:ListItem Value="High"></asp:ListItem>
                <asp:ListItem Value="Medium"></asp:ListItem>
                <asp:ListItem Value="Low"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <th width="20%">
            <p>Area</p>
            <p>区域</p>
        </th>
        <td style="text-align:left;">
            <asp:DropDownList runat="server" ID="ddlArea"  Width="98%">
                <asp:ListItem Value="Report"></asp:ListItem>
                <asp:ListItem Value="Interface"></asp:ListItem>
                <asp:ListItem Value="Enhancement"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr >
        <th >
            <p>System</p>
            <p>系统</p>
        </th>
        <td style="text-align:left;">
            <asp:DropDownList runat="server" ID="ddlSystem"  Width="98%">
                <asp:ListItem Value="SAP"></asp:ListItem>
                <asp:ListItem Value="BW"></asp:ListItem>
                <asp:ListItem Value="POS"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <th >
            <p>RequirementType</p>
            <p>需求类型</p>
        </th>
        <td style="text-align:left;">
           <asp:DropDownList runat="server" ID="ddlRequirementType"  Width="98%">
                <asp:ListItem Value="Bug fix"></asp:ListItem>
                <asp:ListItem Value="New requirement"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
    <th><p>Subject</p>
            <p>主题<span class="red">*</span></p>
    </th>
    <td colspan="3" style="text-align:left;">
    <asp:TextBox runat="server" ID="txtSubject" Width="98%" ></asp:TextBox>
    </td>
    </tr>
    
    <tr >
        <th>
            <p>Description</p>
            <p>描述</p>
        </th>
        <td colspan="3" style="text-align:left;">
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="98%" Rows="10"></asp:TextBox>
        </td>
    </tr>
    


    <tr>
        <th >
            <p>Business Logic Of<br/> the Change</p>
            <p>业务逻辑变更</p>
        </th>
        <td colspan="3" style="text-align:left;">
            <asp:TextBox runat="server" ID="txtBusinessLogic" TextMode="MultiLine" Width="98%" Rows="10"></asp:TextBox>
            
           
        </td>
        
        
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td width="20%" class="workflow_table" style="text-align:center; font-family: Georgia;font-size: 12px;">
            
                <p>Attachments</p>
            <p>
                附件</p>
        </td>
        <td colspan="3" class="workflow_table3">
            &nbsp;<QFL:FormAttachments runat="server" ID="attacthment"></QFL:FormAttachments>
        </td>
    </tr>
</table>
</asp:Panel>
<br/>
<asp:Panel runat="server" ID="Panel2">
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table">
    <tr>
        <th width="30%">
            <p>Change Request Number</p>
            <p>变更号码</p>
        </th>
        <td style="text-align:left;">
            <asp:TextBox runat="server" ID="txtChangeRequestNumber"></asp:TextBox>
            
           
        </td>
        
        
    </tr>
</table>
</asp:Panel>

<script type ="text/javascript" >
    var crnumber,subject;
    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
    function validForm() {
        crnumber = document.getElementById("<%= txtChangeRequestNumber.ClientID %>").value;
        if ($.trim(crnumber) == "") {
            alert("Please supply a Change Request Number.");
            return false;
        }
        return true;
    }
    function validSubmit() {
        subject = document.getElementById("<%=txtSubject.ClientID %>").value;
        if ($.trim(subject) == "") {
            alert("Please supply a subject.")
            return false;
        }
        return true;
    }    
</script>