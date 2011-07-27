﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" Inherits="CA.WorkFlow.UI.TimeOff.NewForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<%@ Register Src="DataFormNew.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Leave Application Form -假期申请表
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <QFL:ListFormControl ID="ListFormControl2" runat="server" FormMode="New">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="workflow_table_button" style="padding-bottom: 10px">
                                <CAControls:CAStartWFButton ID="StartWorkflowButton1" runat="server" Text="Submit" />
                                <CAControls:CAStartWFButton ID="StartWorkflowButton2" runat="server" Text="Save"
                                    CausesValidation="false" />
                                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                                <asp:Label runat="server" ID="labMessage" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:DataForm ID="DataForm1" runat="server" ControlMode="New" />
                                <table width="596" border="0" align="center" cellpadding="0" cellspacing="0" class="note">
                                    <tr>
                                        <th valign="top" class="top">
                                            &nbsp;
                                        </th>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <th colspan="4">
                                                        TYPE OF LEAVE 假期类别
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        A
                                                    </th>
                                                    <th>
                                                        <p>
                                                            Annual Leave 年假</p>
                                                    </th>
                                                    <th>
                                                        E
                                                    </th>
                                                    <th>
                                                        <p>
                                                            Marriage Leave 婚假</p>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        B
                                                    </th>
                                                    <th>
                                                        Sick Leave 病假
                                                    </th>
                                                    <th>
                                                        F
                                                    </th>
                                                    <th>
                                                        Compassionate Leave 丧假
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        C
                                                    </th>
                                                    <th>
                                                        Maternity Leave 产假
                                                    </th>
                                                    <th>
                                                        G
                                                    </th>
                                                    <th>
                                                        Leave in-lieu-of Overtime 加班调休
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        D
                                                    </th>
                                                    <th>
                                                        No Pay Leave 不带薪假
                                                    </th>
                                                    <th>
                                                        H
                                                    </th>
                                                    <th>
                                                        Others 其他
                                                    </th>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <a href="/WorkFlowCenter/FlowCharts/LeaveApplication.doc" style="cursor: hand; color: Blue;
                                                font-size: 14px">Click here to view the flowchart of the workflow</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th valign="top" class="foot">
                                            &nbsp;
                                        </th>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <SharePoint:FormDigest ID="FormDigest1" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
</asp:Content>
