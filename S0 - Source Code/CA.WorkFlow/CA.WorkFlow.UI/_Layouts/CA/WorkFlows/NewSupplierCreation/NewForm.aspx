<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs"
    Inherits="CA.WorkFlow.UI.NewSupplierCreation.NewForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    New Trade Supplier Creation - 新增供应商
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="workflow_table_button" style="padding-bottom: 10px">
                                    <QFC:StartWorkflowButton ID="StartWorkflowButton1" runat="server" Text="Submit" />
                                    <QFC:StartWorkflowButton ID="StartWorkflowButton2" runat="server" Text="Save" />
                                    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                                </div>
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
                                            <a href="/WorkFlowCenter/FlowCharts/NewSupplierCreation.doc" style="cursor: hand;
                                                color: Blue; font-size: 14px">Click here to view the flowchart of the workflow</a>
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
