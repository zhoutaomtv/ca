<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.NewStoreBudgetApplication.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/SingleFileUpload.ascx" TagPrefix="uc" TagName="SingleFileUpload" %>
<script type="text/javascript" language="javascript">
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
<table width="100%" border="0" cellpadding="1" cellspacing="1" class="workflow_table4">
    <tr>
        <th width="30%">
            <p>
                Store Name<span class="ms-formvalidation"> *</span></p>
            <p>
                店铺名称</p>
        </th>
        <td>
           <QFL:FormField ID="ffStoreName" runat="server" FieldName="StoreName" CssClass="input_ts4" >
            </QFL:FormField><%--<asp:TextBox ID ="txtStoreName" runat="server"/> --%>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Gross Area (GSA)<span class="ms-formvalidation"> *</span></p>
            <p>
                总面积</p>
        </th>
        <td>
        <QFL:FormField ID="ffGrossArea" runat="server" FieldName="GrossArea">
            </QFL:FormField>
            <%--<asp:TextBox ID ="txtGrossArea" runat="server" CssClass="input_ts4" /> --%>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Sales Area (NSA)<span class="ms-formvalidation"> *</span></p>
            <p>
                销售区域面积</p>
        </th>
        <td>
        <QFL:FormField ID="ffNSA" runat="server" FieldName="NSA">
            </QFL:FormField>
            <%--<asp:TextBox ID ="txtNSA" runat="server" CssClass="input_ts4" /> --%>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Total Cost<span class="ms-formvalidation"> *</span></p>
            <p>
                总投资</p>
        </th>
        <td>
        <QFL:FormField ID="ffTotalCost" runat="server" FieldName="TotalCost">
            </QFL:FormField>
            <%--<asp:TextBox ID ="txtTotalCost" runat="server" CssClass="input_ts4" /> --%>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Cost/GSA<span class="ms-formvalidation"> *</span></p>
            <p>
                总面积单方造价</p>
        </th>
        <td>
        <QFL:FormField ID="ffCrossPerm2" runat="server" FieldName="CrossPerm2">
            </QFL:FormField>
             <%--<asp:TextBox ID ="txtCrossPerm2" runat="server" CssClass="input_ts4" /> --%>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Cost/NSA<span class="ms-formvalidation"> *</span></p>
            <p>
                销售区域面积单方造价</p>
        </th>
        <td>
        <QFL:FormField ID="ffNSAPerm2" runat="server" FieldName="NSAPerm2">
            </QFL:FormField>
            <%--<asp:TextBox ID ="txtNSAPerm2" runat="server" CssClass="input_ts4" /> --%>
        </td>
    </tr>
   
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflow_table3">
     <tr>
        <th width="30%">
            <p>
                The budget detail<span class="ms-formvalidation"> *</span></p>
            <p>
                预算明细</p>
        </th>
        <td>
           <uc:SingleFileUpload ID="ucFileUpload" runat="server" /><%--&nbsp;<QFL:FormAttachments runat="server" ID="attacthment" ></QFL:FormAttachments> --%>
        </td>
    </tr>
</table>
