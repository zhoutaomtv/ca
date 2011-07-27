<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/_Layouts/CA/Application.Master"
    CodeBehind="ApproveForm.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.TravelRequset.ApproveForm" %>

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
    Travel Request Form-出差申请表
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="Display">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflower_bj">
            <tr>
                <td>
                    <div class="workflow_table_button" style="padding-bottom: 10px">
                        <QFC:ActionsButton ID="actions" runat="server" />
                        <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="1" class="workflower_table">
                        <tr>
                            <td>
                                Comments 批注:
                                <QFL:TaskPanel runat="server" ID="TaskPanel1">
                                    <QFL:CommentTaskField runat="server" ID="ctfComments" />
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
                                <table width="596" border="0" cellspacing="0" cellpadding="0" class="note">
                                    <tr>
                                        <th valign="top" class="top">
                                            &nbsp;
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            注：为证明员工在外出差的天数以申请出差用餐补贴，员工出差回来后请将本表、来回机票、登机牌、住宿发票及酒店入住登记情况等原件附在报销申请表后一并提交。
                                            <br />
                                            Note:In order to evidence the number of days spent out of Shanghai for reimbursement
                                            of meal expenses during the business trip,the employee should attach this form together
                                            with the original air-tickets,boarding pass,accommodation invoices,check-in and
                                            check-out records etc to the Expense Claim Form and submit all of them after returning
                                            from the business trip.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <a href="/WorkFlowCenter/FlowCharts/TravelRequest.doc" style="cursor: hand; color: Blue;
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

    <script language="javascript" type="text/javascript">
        function CheckComments() {
            var obj = "<%=ctfComments.ClientID %>";
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
                    var obj = "<%=ctfComments.ClientID %>";
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

</asp:Content>
