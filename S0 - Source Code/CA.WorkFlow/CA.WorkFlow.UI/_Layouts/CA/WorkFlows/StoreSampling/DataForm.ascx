<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.StoreSampling.DataForm" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<%@ Register Src="../UserControl/SingleFileUpload.ascx" TagPrefix="uc" TagName="SingleFileUpload" %>

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
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr >
        <th width="20%">
            <p>
                Store Number</p>
            <p>
                门店号<span class="red">*</span>
            </p>
        </th>
        <td>
            <asp:DropDownList runat="server" ID="ddlStoreNumber" Width="150">
            </asp:DropDownList>
        </td>
        <th width="20%">
            <p>
                Issued to</p>
            <p>
                发送至</p>
        </th>
        <td width="30%">
            <asp:DropDownList runat="server" ID="ddlIssuedTo">
                <asp:ListItem Value="head office staff(buying)"></asp:ListItem>
                <asp:ListItem Value="store staff(uniform)"></asp:ListItem>
                <asp:ListItem Value="marketing staff"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr >
        <th width="20%">
            <p>
                Actual Quantity</p>
            <p>
                数量
            </p>
        </th>
        <td>
            <h5>
                <asp:TextBox runat="server" ID="txtActualQuantity" class="input_ts4"></asp:TextBox>
             
            </h5>
        </td>
        <th width="20%">
            <p>
                Picked Time</p>
            <p>
                取样日期</p>
        </th>
        <td width="30%">
           <CAControls:CADateTimeControl runat="server" ID="CADateTime1" DateOnly="true" >
            </CAControls:CADateTimeControl>
        </td>
        
    </tr>
    <tr >
        <th>
            <p>
                Picked by</p>
            <p>
                取货人<span class="red">*</span></p>
        </th>
        <td>
            <CAControls:CAPeopleFinder ID="CAPeopleFinder1" runat="server" AllowTypeIn="true"
                MultiSelect="false"  width="150" />
        </td>
        <th width="20%">
            <p>
                Upload Detail File</p>
            <p>
                上传附件</p>
        </th>
        <td width="30%">
        <asp:Panel runat="server" ID="Panel2">
           <uc:SingleFileUpload ID="ucFileUpload" runat="server" />
           </asp:Panel>
        </td>
        
    </tr>
    <tr id="hideCostCenter" runat="server" visible="false"> <th width="20%">
            <p>
                Cost Center</p>
            <p>
                成本中心<span class="red">*</span>
            </p>
        </th>
        <td>
            <asp:DropDownList runat="server" ID="ddlCostCenter" Width="150" Enabled="false">
            </asp:DropDownList>
        </td>
        <td>
            <asp:TextBox ID="txtCostCenter" runat="server" Enabled="false"></asp:TextBox></td>
        </tr>
    
</table>
</asp:Panel>




<script type ="text/javascript" >
    var storeNumber, issuedTo, pickedBy, buying;

    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
    function validForm() {
        
        //if ($.trim(storeNumber) == "") {
        //    alert("Store number can not be blank.");
        //    return false;
        //}
        
        
        return true;
    } 
</script>