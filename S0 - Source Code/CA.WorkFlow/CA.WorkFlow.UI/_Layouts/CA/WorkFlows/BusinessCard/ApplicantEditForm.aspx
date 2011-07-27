<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    AutoEventWireup="true" CodeBehind="ApplicantEditForm.aspx.cs" Inherits="CA.WorkFlows.BusinessCard.ApplicantEditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Business Card Application -名片申请表
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="New">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflower_bj">
            <tr>
                <td>
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="workflow_table_button" style="padding-bottom: 10px">
                                    <QFC:ActionsButton ID="actions" runat="server" />
                                    <asp:Button runat="server" Text="Save" ID="btnSave" onclick="btnSave_Click" />
                                    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trComments" runat="server">
                            <td>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
                                    <tr>
                                        <th>
                                            Comments:
                                        </th>
                                        <td>
                                            <QFL:TaskPanel runat="server" ID="task1">
                                                <QFL:CommentTaskField runat="server" ID="body" />
                                            </QFL:TaskPanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Edit" />
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3"  id="tblCommentsList" runat="server">
                                    <tr>
                                        <td colspan="2">
                                            <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
                                            </cc1:TaskTraceList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="596" border="0" align="center" cellpadding="0" cellspacing="0" class="note">
                                    <tr>
                                        <th valign="top" class="top">
                                            &nbsp;
                                        </th>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                              <a href="/WorkFlowCenter/FlowCharts/NameCard.doc" style="cursor:hand; color:Blue; font-size:14px">Click here to view the flowchart of the workflow</a>
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
    </qfl:listformcontrol>
</asp:Content>
