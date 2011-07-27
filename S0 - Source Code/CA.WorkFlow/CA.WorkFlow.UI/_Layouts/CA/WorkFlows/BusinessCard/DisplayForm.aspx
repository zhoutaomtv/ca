<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" Inherits="CA.WorkFlows.BusinessCard.DisplayForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
    <%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
    <%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Business Card Application -名片申请表
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script language="javascript" type="text/javascript">
        function strUrl(obj) {
            var url = document.getElementById("<%= hdUrl.ClientID%>").value;
            obj.href = url;
        }
    </script>
    <br />
    <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" >
            <tr>
                <td >
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="80%">
                                <div class="workflow_table_button" style="padding-bottom:10px">
                                    <%--<asp:Button ID="btnExportPDF" runat="server" Text="ExportPDF" OnClick="btnExportPDF_Click"
                                        CssClass="negative" />--%>
                                    <input type="button" value="Cancel"  onclick="window.history.back(-1)"/>
                                </div>
                            </td>
                            <td width="20%" align="right"><a id="hrefPDF" onclick="strUrl(this)" style="cursor:hand" runat="server">
                                    ExportPDF</a><input id="hdUrl" type="hidden" runat="server"/></td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Display" />
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
                                <%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="30">
                                            <div class="buttons">
                                                <a onclick="window.history.back(-1)" class="negative">
                                                    <img src="../images/Cancel.png" alt="" />Cancel </a>
                                            </div>
                                        </td>
                                    </tr>
                                </table>--%>
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
