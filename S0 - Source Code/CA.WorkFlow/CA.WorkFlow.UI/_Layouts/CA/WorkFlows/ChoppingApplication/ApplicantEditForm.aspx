﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    AutoEventWireup="true" CodeBehind="ApplicantEditForm.aspx.cs" Inherits="CA.WorkFlow.UI.ChoppingApplication.ApplicantEditForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Chopping Application - 公司盖章
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <QFL:ListFormControl ID="ListFormControl2" runat="server" FormMode="Edit">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="workflow_table_button" style="padding-bottom: 10px">
                                <QFC:ActionsButton ID="actions" runat="server" />
                                <asp:Button runat="server" ID="btnSave" Text="Save" />
                                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Edit" />
                                <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
                                </cc1:TaskTraceList>
                                <table width="596" border="0" align="center" cellpadding="0" cellspacing="0" class="note">
                                    <tr>
                                        <th valign="top" class="top">
                                            &nbsp;
                                        </th>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <a href="/WorkFlowCenter/FlowCharts/Chopping.doc" style="cursor: hand; color: Blue;
                                                font-size: 14px">Click here to view the flowchart of the workflow</a><br />
                                            <a href="/WorkFlowCenter/FlowCharts/Chop%20use%20policy%20related.doc" style="cursor: hand;
                                                color: Blue; font-size: 14px">Click here to view Chop use policy related</a>
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
