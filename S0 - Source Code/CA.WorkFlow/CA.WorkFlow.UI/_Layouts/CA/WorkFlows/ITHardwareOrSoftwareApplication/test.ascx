<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="test.ascx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.ITHardwareOrSoftwareApplication.test" %>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr >
        <th colspan="3" class="th2">
            <p>
                Hardware or Software Description</p>
            <p>
                硬件或软件介绍</p>
        </th>
    </tr>
    <tr>
        <th width="5%">
           <asp:ImageButton runat="server" ID="imgbtnSubmit" ImageUrl="../images/pixelicious_001.png"
                OnClick="imgbtnSubmit_Click" />
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
                <td><asp:ImageButton ID="ImageButton1" CommandName="delete" CommandArgument="ID" runat="server"
                        ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" /></td>
                <td align="center"><input id="txtHardName" runat="server" type="text" class="input_ts4"/></td>
                <td align="center"><input id="txtCost" runat="server" type="text" class="input_ts4"/></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>