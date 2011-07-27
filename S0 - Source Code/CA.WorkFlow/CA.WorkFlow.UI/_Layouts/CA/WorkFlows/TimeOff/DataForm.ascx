<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.TimeOff.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<%@ Register Src="MultiTable.ascx" TagName="MultiTable" TagPrefix="uc2" %>
<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table3">
    <tr>
        <th width="24%">
            Tracking Number 编号:
        </th>
        <td>
            <span runat="server" id="naSpan" visible="false">NA</span>&nbsp;
            <QFL:FormField ID="formFieldNumber" runat="server" FieldName="WorkFlowNumber" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table runat="server" id="tableDisplay" width="100%" border="0" cellpadding="0" cellspacing="1"
    align="center" class="workflow_table3">
    <tr>
        <th width="24%">
            Name 姓名:
        </th>
        <td width="24%">
            <QFL:FormField ID="formFieldApplicant" runat="server" FieldName="Applicant" ControlMode="Display">
            </QFL:FormField>
        </td>
        <th width="28%">
            Department 部门:
        </th>
        <td width="24%">
            <QFL:FormField ID="formFieldDeptment" runat="server" FieldName="Department" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <th>
            Position 职位:
        </th>
        <td>
            <QFL:FormField ID="formFieldJobTitle" runat="server" FieldName="JobTitle" ControlMode="Display">
            </QFL:FormField>
        </td>
        <th>
            Employee NO. 员工编号:
        </th>
        <td>
            <QFL:FormField ID="formFieldEmployeeID" runat="server" FieldName="EmployeeID" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table runat="server" id="tableNew" width="100%" border="0" cellpadding="0" cellspacing="0"
    class="workflow_table3">
    <tr style="display: none;">
        <th width="24%">
            choose Employee 选择员工:
        </th>
        <td colspan="3">
            <CAControls:CAPeopleFinder ID="CAPeopleFinder1" runat="server" AllowTypeIn="true"
                MultiSelect="false" AutoPostBack="true" />
        </td>
    </tr>
    <tr>
        <th width="24%">
            Name 姓名:
        </th>
        <td width="24%">
            <asp:Label runat="server" ID="labPeople"></asp:Label>
        </td>
        <th width="28%">
            Department 部门:
        </th>
        <td width="24%">
            <asp:Label runat="server" ID="labDepartment"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>
            Positon 职位:
        </th>
        <td>
            <asp:Label runat="server" ID="labPostion"></asp:Label>
        </td>
        <th>
            Employee NO 员工编号:
        </th>
        <td>
            <asp:Label runat="server" ID="labEmployeeNO"></asp:Label>
        </td>
    </tr>
</table>
<uc2:MultiTable ID="MultiTable1" runat="server" />
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th colspan="4" class="th2">
            Leave Balance 剩余假期
        </th>
    </tr>
    <tr>
        <th width="35%">
            <p>
                Annual Leave Entitlement of
                <%= CurrentYear %>:</p>
            <p>
                <%= CurrentYear %>年度可用年假:</p>
        </th>
        <td>
            <asp:Label runat="server" ID="labAnnualEntitlement"></asp:Label>
        </td>
        <th width="35%">
            <p>
                Annual Leave Balance of
                <%=CurrentYear%>:</p>
            <p>
                <%=CurrentYear%>年度剩余年假:</p>
        </th>
        <td>
            <asp:Label runat="server" ID="labAnnulLeave"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                Sick Leave Entitlement of
                <%=CurrentYear%>:</p>
            <p>
                <%=CurrentYear%>年度可用病假:</p>
        </th>
        <td>
            <asp:Label runat="server" ID="labSickEntitlement"></asp:Label>
        </td>
        <th>
            <p>
                Sick Leave Balance of
                <%=CurrentYear%>:</p>
            <p>
                <%=CurrentYear%>年度剩余病假:</p>
        </th>
        <td>
            <asp:Label runat="server" ID="labSickLeave"></asp:Label>
        </td>
    </tr>
</table>

<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr class="workflow_table3" runat="server" id="trAttachment" visible="false">
        <td width="35%">
            Attachments 附件:
        </td>
        <td>
            &nbsp;<QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Edit">
            </QFL:FormAttachments>
        </td>
    </tr>
</table>
