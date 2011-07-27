<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiTable.ascx.cs" Inherits="CA.WorkFlow.UI.TimeOff.MultiTable" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
            <tr class="workflow_table3">
                <th colspan="8" class="th2">
                    Leave Record 请假记录
                </th>
            </tr>
            <tr class="workflow_table3">
                <td style="width: 20;">
                    <div align="center">
                        <CAControls:CAImageButton runat="server" ID="imgbtnAdd" ImageUrl="../images/pixelicious_001.png" OnClick="imgbtnAdd_Click" Width="18" CausesValidation="false" />
                    </div>
                </td>
                <th class="th2">
                    Type 类型
                </th>
                <th colspan="2" class="th2">
                    From Date 起始日期
                </th>
                <th colspan="2" class="th2">
                    To Date 结束日期
                </th>
                <th class="th2">
                    Days 天数
                </th>
            </tr>
            <asp:Repeater ID="RepeaterEdit" runat="server" OnItemCommand="RepeaterEdit_ItemCommand" OnItemDataBound="RepeaterEdit_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td>
                            <CAControls:CAImageButton ID="ImageButton1" CommandName="delete" CommandArgument="ID" runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" CausesValidation="false" />
                        </td>
                        <td>
                            <CAControls:DDLLeaveType ID="ddlType" runat="server" Width="150px">
                            </CAControls:DDLLeaveType>
                        </td>
                        <td>
                            <CAControls:CADateTimeControl runat="server" ID="datetimeForm" DateOnly="true">
                            </CAControls:CADateTimeControl>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFromTime" onchange="SumDate(this,'1')">
                                <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <CAControls:CADateTimeControl runat="server" ID="datetimeTo" DateOnly="true">
                            </CAControls:CADateTimeControl>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFromTo" onchange="SumDate(this,'1')">
                                <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDays" Width="25px" ContentEditable="false"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="RepeaterDisplay" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="labType" Text='<%#Eval("LeaveType") %>'></asp:Label>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="labFrom" Text='<%#Eval("DateFrom") %>'></asp:Label>
                            <asp:Label runat="server" ID="labFromTime" Text='<%#Eval("DateFromTime") %>'></asp:Label>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="labTo" Text='<%#Eval("DateTo") %>'></asp:Label>
                            <asp:Label runat="server" ID="labToTime" Text='<%#Eval("DateToTime") %>'></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="labDays" Text='<%#Eval("LeaveDays") %>'></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<asp:Button runat="server" ID="btnSure" OnClick="btnSure_Click" Text="Sure" Visible="false" />
<asp:Button runat="server" ID="btnReload" OnClick="btnReload_Click" Text="Reload" CausesValidation="False" CssClass="hidden" />
<asp:HiddenField runat="server" ID="hidMixedDates" />
<script type="text/javascript" language="javascript">
    function SumDate(obj, flag) {
        var objID = "";
        if (flag == "1") {
            objID = obj.id;
        }
        else {
            objID = obj;
        }
        var temp = objID.substring(0, 80);
        var fromdatetime = temp + "datetimeForm_datetimeFormDate";
        var todatetime = temp + "datetimeTo_datetimeToDate";
        var fromhalf = temp + "ddlFromTime";
        var tohalf = temp + "ddlFromTo";
        var sumdate = temp + "txtDays";
        var startdate = document.getElementById(fromdatetime).value;
        var endDate = document.getElementById(todatetime).value;
        var obj1 = document.getElementById(fromhalf);
        var obj2 = document.getElementById(tohalf);
        datediffstr(startdate, endDate, obj1, obj2, sumdate);
    }
    function datediffstr(startdate, endDate, obj1, obj2, sumdate) {
        var s_half = obj1.options[obj1.selectedIndex].value;
        var e_half = obj2.options[obj2.selectedIndex].value;

        if (startdate && endDate) {
            var s = new Date(Date.parse(startdate.replace(/-/g, "/")));
            var e = new Date(Date.parse(endDate.replace(/-/g, "/")));

            if (s > e) {
                alert('Start date should be smaller than or equal to the end date.');
                $('#' + sumdate).val('');
            }
            else {
                $('#<%= this.hidMixedDates.ClientID %>').val(sumdate + '|' + startdate + '|' + endDate + '|' + s_half + '|' + e_half);
                $('#<%= this.btnReload.ClientID %>').click();
            }
        }
    }
</script>