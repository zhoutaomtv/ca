<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.ChoppingApplication.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<%@ Register Src="ProjectMultiTable.ascx" TagName="ProjectMultiTable" TagPrefix="uc2" %>

<script type="text/javascript" language="javascript">
    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table2">
    <tr>
        <th>
            Applicant 用户名:
        </th>
        <td align="left">
            <asp:Label runat="server" ID="labPeople"></asp:Label>
        </td>
        <th>
            Department 部门:
        </th>
        <td align="left">
            <asp:Label runat="server" ID="labDepartment"></asp:Label>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table3">
    <tr>
        <th width="17%" style="text-align:left; vertical-align:top; color:Teal">
Kind Reminder:
        </th>
        <th style="text-align:left; color:Teal">
The applicant is responsible to make sure that the final version of the document conforms to the version reviewed and approved by the legal department.
        </th>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table3">
    <tr>
        <th width="24%">
            TrackingNumber 编号:
        </th>
        <td>
            <asp:Label runat="server" ID="labNumber"></asp:Label>&nbsp;
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_tableChoping">
    <tr class="workflow_table3">
        <th width="50%">
            <p>
                Responsible Functional Department and Manager</p>
            <p>
                负责的职能部门和经理<span style="color: Red;">*</span></p>
        </th>
        <td>
            <QFL:FormField ID="FormFieldManager" runat="server" FieldName="DepartHeadAccount">
            </QFL:FormField>
            <hr />
        </td>
    </tr>
    <tr class="workflow_table3">
        <th>
            <p>
                Manager or CFO approving the commercial and/or technical terms</p>
            <p>
                批准商业和/或技术条款的经理或首席执行官</p>
        </th>
        <td>
            <QFL:FormField ID="FormFieldCFCO" runat="server" FieldName="CFCOAccount">
            </QFL:FormField>
            <QFL:FormField ID="FormFieldCFCO2" runat="server" FieldName="CFCO2Account">
            </QFL:FormField>
            <hr />
        </td>
    </tr>
    <tr class="workflow_table3">
        <th>
            <p>
                Legal counsel reviewing the document:</p>
            <p>
                审阅文件的法律顾问：
            </p>
        </th>
        <td>
        <CAControls:CAPeopleFinder runat="server" ID="pfinderLegal" Enabled="false" MultiSelect="false" />          
            <asp:Label runat="server" ID="labLeagal" ></asp:Label>
            <hr />
        </td>
    </tr>
    <tr class="workflow_table3">
        <th>
            <p>
                Manager or CEO reviewing the commercial and/or technical term</p>
            <p>
                审阅商业和/或技术条款的经理或首席执行官
            </p>
        </th>
        <td>
            <QFL:FormField ID="FormFieldCEO" runat="server" FieldName="CEOAccount">
            </QFL:FormField>
            <hr />
        </td>
    </tr>
    <tr class="workflow_table3">
        <th>
            <p>
                Documents signed by</p>
            <p>
                文件签署人</p>
        </th>
        <td>
            <QFL:FormField ID="FormFieldSigner" runat="server" FieldName="Signer">
            </QFL:FormField>
            <hr />
        </td>
    </tr>
    <tr class="workflow_tableChoping">
        <th>
            <p>
                Chop chosen to use</p>
            <p>
                被选择使用的印章</p>
        </th>
        <td align="left">
            <QFL:FormField ID="FormField1" runat="server" FieldName="Chop">
            </QFL:FormField>
            <hr />
        </td>
    </tr>
    <tr class="workflow_table3">
        <th>
            <p>
                Contract Repository Department</p>
            <p>
                合同保管部门</p>
        </th>
        <td>
            <QFL:FormField runat="server" FieldName="RepositoryDepartment" ID="FormFieldDepartment">
            </QFL:FormField>
        </td>
    </tr>
</table>
<uc2:ProjectMultiTable ID="ProjectMultiTable1" runat="server" ControlMode="Display" />
