<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI.TimeOff.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Leave Application Form -假期申请表
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <script language="javascript" type="text/javascript">      
        function CheckComments() {
            var obj = "<%=body.ClientID %>";
            obj = obj + "_ctl00_ctl00_TextField";
            if (Trim(document.getElementById(obj).value) == "") {
                alert("Please supply comments when reject a request.");
                document.getElementById(obj).focus();
                return false;
            }
        }
        function Trim(s) {
            var ss = s.replace(/^[\s]+/g, "");
            ss = ss.replace(/[\s]+$/g, "");
            return ss;
        }

        $().ready(function() {
            if ($(".ms-ButtonHeightWidth").length > 1) {

                $($(".ms-ButtonHeightWidth")[1]).bind("click", function() {
                    var obj = "<%=body.ClientID %>";
                    obj = obj + "_ctl00_ctl00_TextField";
                    if (Trim(document.getElementById(obj).value) == "") {
                        alert("Please supply comments when reject a request.");
                        document.getElementById(obj).focus();
                        return false;
                    }
                });
            }
        })
    </script>

    <QFL:ListFormControl ID="ListFormControl2" runat="server" FormMode="Display">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="workflow_table_button" style="padding-bottom: 10px">
                                <QFC:ActionsButton ID="actions" runat="server" />
                                <input type="button" value="Cancel" onclick="window.history.back(-1)" />
                                <asp:Label runat="server" ID="labMessage" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Comments:
                                <QFL:TaskPanel runat="server" ID="task1">
                                    <QFL:CommentTaskField runat="server" ID="body" />
                                </QFL:TaskPanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Display" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
                                </cc1:TaskTraceList>
                            </td>
                        </tr>
                        <tr>
                            <td>
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
        <SharePoint:FormDigest ID="FormDigest2" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
</asp:Content>
