<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.TravelRequset.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

<script type="text/javascript" language="javascript">
    function CheckIsCancel(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
    }
    function RadioChecked() {
        if ($('#' + '<%=rbbookticketNo.ClientID %>').is(':checked')) {
            $('#divbtUser :input').removeAttr('disabled');
            $('#divbtUser :span').removeAttr('disabled');
        } else {

            $('#divbtUser :input').attr('disabled', true);
            $('#divbtUser :span').attr('disabled', true);
        }

    }
    function radioChecked() {
        if ($('#' + '<%=RadioButtonNo.ClientID %>').is(':checked')) {
            $('#divbtApplicant :input').removeAttr('disabled');
            $('#divbtApplicant :span').removeAttr('disabled');

        } else {

            $('#divbtApplicant :input').attr('disabled', true);
            $('#divbtApplicant :span').attr('disabled', true);
        }
    }


    function disablebtuser() {
        $('#divbtUser :input').attr('disabled', true);
        $('#divbtUser :span').attr('disabled', true);
    }
    
    function disableapplicant() {
        $('#divbtApplicant :input').attr('disabled', true);
        $('#divbtApplicant :span').attr('disabled', true);
    }
</script>

<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table3">
    <tr>
        <th width="20%" class="th2">
            Tracking Number 编号
        </th>
        <td>
            &nbsp;
            <asp:Label ID="lblWorkflowNumber" runat="server" Text=""></asp:Label>
        </td>
    </tr>
</table>
<table id="tblMsg" runat="server" visible="false" width="100%" border="0" cellspacing="0"
    cellpadding="1" class="workflow_table3">
    <tr>
        <td>
            <asp:Label ID="lblMessage" ForeColor="#CC00CC" runat="server" Text="Please note: "
                Font-Size="Medium"></asp:Label>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table3">
    <tr>
        <th width="20%" class="th2">
            Choose Employee<br />
            选择员工
        </th>
        <td>
            <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                AutoPostBack="true" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table4">
    <tr>
        <th width="25%" class="th2">
            <p>
                Chinese Name<span class="ms-formvalidation"> *</span></p>
            <p>
                中文姓名</p>
        </th>
        <th width="25%" class="th2">
            <p>
                English Name</p>
            <p>
                英文姓名</p>
        </th>
        <th width="25%" class="th2">
            <p>
                NamePinyin<span class="ms-formvalidation"> *</span></p>
            <p>
                姓名拼音</p>
        </th>
        <th width="25%" class="th2">
            <p>
                Department</p>
            <p>
                部门</p>
        </th>
    </tr>
    <tr>
        <td>
            <QFL:FormField ID="ffChineseName" runat="server" FieldName="ChineseName">
            </QFL:FormField>
        </td>
        <td>
            <asp:Label ID="lblEnglishName" runat="server" />
        </td>
        <td>
            <QFL:FormField ID="ffPinyin" runat="server" FieldName="NamePinyin">
            </QFL:FormField>
        </td>
        <td>
            <asp:Label ID="lblDepartment" runat="server" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" class="workflow_table4">
    <tr>
        <th width="25%" class="th2">
            <p>
                Job Title
            </p>
            <p>
                职位</p>
        </th>
        <th width="25%" class="th2">
            <p>
                ID/Passport No.<span class="ms-formvalidation"> *</span></p>
            <p>
                身份证/护照号码</p>
        </th>
        <th width="25%" class="th2">
            <p>
                Mobile Phone NO.</p>
            <p>
                手机</p>
        </th>
        <th width="25%" class="th2">
            <p>
                Credit Card NO.</p>
            <p>
                信用卡号码</p>
        </th>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblJobTitle" runat="server" Text=""></asp:Label>
        </td>
        <td>
            <QFL:FormField ID="ffIDNumber" runat="server" FieldName="IDNumber">
            </QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffMobile" runat="server" FieldName="Mobile">
            </QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffCreditCard" runat="server" FieldName="CreditCard">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <th colspan="2" class="th2">
            <p>
                出差目的 Travel Purpose:</p>
        </th>
        <td colspan="2">
            <div align="left" style="padding-left: 5px">
                <QFL:FormField ID="ffPurpose" runat="server" FieldName="Purpose">
                </QFL:FormField>
            </div>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th colspan="6" class="th2">
            <b>Travel Details 具体行程</b>
        </th>
    </tr>
    <tr>
        <th>
            <div align="center">
                <asp:ImageButton runat="server" ID="btnAddDetail" ImageUrl="../images/pixelicious_001.png"
                    OnClick="btnAddDetail_Click" Width="18" />
            </div>
        </th>
        <th class="th2">
            <p>
                Start Date<span class="ms-formvalidation"> *</span></p>
            <p>
                开始日期</p>
        </th>
        <th class="th2">
            <p>
                End Date<span class="ms-formvalidation"> *</span></p>
            <p>
                结束日期</p>
        </th>
        <th class="th2">
            <p>
                Departure<span class="ms-formvalidation"> *</span></p>
            <p>
                出发地</p>
        </th>
        <th class="th2">
            <p>
                Destination<span class="ms-formvalidation"> *</span></p>
            <p>
                目的地</p>
        </th>
        <th class="th2">
            <p>
                Vehicle</p>
            <p>
                交通工具</p>
        </th>
    </tr>
    <asp:Repeater ID="rptTravelDetails" runat="server" OnItemCommand="rptTravelDetails_ItemCommand"
        OnItemDataBound="rptTravelDetails_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td>
                    <div align="center">
                        <asp:ImageButton ID="btnDeleteDetail" CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png"
                            Width="18" />
                    </div>
                </td>
                <td>
                    <cc1:CADateTimeControl ID="CADateTimeStartDate" runat="server" DateOnly="true">
                    </cc1:CADateTimeControl>
                    &nbsp;
                </td>
                <td>
                    <cc1:CADateTimeControl ID="CADateTimeEndDate" runat="server" DateOnly="true">
                    </cc1:CADateTimeControl>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtDeparture" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtDestination" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:DropDownList ID="ddlVehicle" runat="server" Width="95%">
                        <asp:ListItem Text="Flight" Value="Flight" Selected="True" />
                        <asp:ListItem Text="Rail" Value="Rail" />
                        <asp:ListItem Text="Bus" Value="Bus" />
                    </asp:DropDownList>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptTravelDetailsDisplay" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("FromDate")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("ToDate")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Departure")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Destination")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Vehicle")%>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr>
        <th colspan="4" class="th2">
            Estimated days of travel 预计出差天数:<span class="ms-formvalidation"> *</span>
        </th>
        <td colspan="2">
            <QFL:FormField ID="ffEstimateDays" runat="server" FieldName="EstimateDays">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
      <th  class="th2" valign="top">
            Notes:
        </th>
        <td style="width:93%">
       
            <asp:TextBox ID="txtAirNotes" runat="server" Rows="2" TextMode="MultiLine" 
                Width="98%"></asp:TextBox>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th class="th2" colspan="2">
            <b>Do you want receptionist to book the flight ticket?</b>
        </th>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="rbbookticketYes" runat="server" GroupName="111" 
                Checked="true" OnCheckedChanged="rbbookticketYes_CheckedChanged" Text="Yes, receptionist books the flight ticket."
                Font-Bold="True" ForeColor="Black" />
        </td>
    </tr>
    <tr valign="top">
        <td style="width: 40%">
            <asp:RadioButton ID="rbbookticketNo" runat="server" GroupName="111" 
                OnCheckedChanged="rbbookticketNo_CheckedChanged" Text="No, please supply a name."
                Font-Bold="True" ForeColor="Black" />
        </td>
        <td style="width: 60%">
            <div id="divbtUser">
                <cc1:CAPeopleFinder ID="btUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                    Visible="true" AutoPostBack="false" Width="95%" />
            </div>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th colspan="4" class="th2">
            <b>Hotel Information 酒店信息</b>
        </th>
    </tr>
    <tr>
        <th>
            <div align="center">
                <asp:ImageButton runat="server" ID="btnAddHotel" ImageUrl="../images/pixelicious_001.png"
                    OnClick="btnAddHotel_Click" Width="18" />
            </div>
        </th>
        <th class="th2">
            <p>
                Check-in date</p>
            <p>
                入店日期</p>
        </th>
        <th class="th2">
            <p>
                Check-out date</p>
            <p>
                离店日期</p>
        </th>
        <th class="th2">
            <p>
                No.of nights</p>
            <p>
                居留夜晚数</p>
        </th>
    </tr>
    <asp:Repeater ID="rptHotelInformation" runat="server" OnItemCommand="rptHotelInformation_ItemCommand"
        OnItemDataBound="rptHotelInformation_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td>
                    <div align="center">
                        <asp:ImageButton ID="btnDeleteHotel" CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png"
                            Width="18" />
                    </div>
                </td>
                <td>
                    <cc1:CADateTimeControl ID="CADateTimeCheckInDate" runat="server" DateOnly="true">
                    </cc1:CADateTimeControl>
                    &nbsp;
                </td>
                <td>
                    <cc1:CADateTimeControl ID="CADateTimeCheckOutDate" runat="server" DateOnly="true">
                    </cc1:CADateTimeControl>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtNights" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptHotelInformationDisplay" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("CheckInDate")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("CheckOutDate")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Nights")%>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th  class="th2" valign="top">
            Notes:
        </th>
        <td style="width:93%" >
       
            <asp:TextBox ID="txtHotelNotes" runat="server" Rows="2" TextMode="MultiLine" 
                Width="98%"></asp:TextBox>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th class="th2" colspan="2">
            <b>Do you want receptionist to make the hotel reservation? </b>
        </th>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="RadioButtonDisplay" runat="server" GroupName="22" 
                Checked="true" OnCheckedChanged="RadioButtonDisplay_CheckedChanged" Text="No, I will make the reservation. "
                Font-Bold="True" ForeColor="Black" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="RadioButtonYes" runat="server" GroupName="22" 
                Checked="false" OnCheckedChanged="RadioButtonYes_CheckedChanged" Text="Yes, receptionist makes the hotel reservation."
                Font-Bold="True" ForeColor="Black" />
        </td>
    </tr>
    <tr valign="top">
        <td style="width: 40%">
            <asp:RadioButton ID="RadioButtonNo" runat="server" GroupName="22" AutoPostBack="false"
                OnCheckedChanged="RadioButtonNo_CheckedChanged" Text="No, please supply a name."
                Font-Bold="True" ForeColor="Black" />
        </td>
        <td style="width: 60%">
            <div id="divbtApplicant">
                <cc1:CAPeopleFinder ID="btApplicant" runat="server" AllowTypeIn="true" MultiSelect="false"
                    AutoPostBack="false" Visible="true" Width="95%" />
            </div>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="1" class="workflow_table3">
    <tr>
        <th colspan="7" class="th2">
            <b>Vehicle Information 交通信息</b>
        </th>
    </tr>
    <tr>
        <th>
            <div align="center">
                <asp:ImageButton runat="server" ID="btnAddVehicle" ImageUrl="../images/pixelicious_001.png"
                    OnClick="btnAddVehicle_Click" Width="18" />
            </div>
        </th>
        <th class="th2" style="width: 25%">
            <p>
                Date</p>
            <p>
                日期</p>
        </th>
        <th class="th2">
            <p>
                Time</p>
            <p>
                时间</p>
        </th>
        <th class="th2">
            <p>
                Number</p>
            <p>
                班次</p>
        </th>
        <th class="th2">
            <p>
                From</p>
            <p>
                起始地</p>
        </th>
        <th class="th2">
            <p>
                To</p>
            <p>
                目的地</p>
        </th>
        <th class="th2">
            <p>
                Class</p>
            <p>
                等级</p>
        </th>
    </tr>
    <asp:Repeater ID="rptVehicleInformation" runat="server" OnItemCommand="rptVehicleInformation_ItemCommand"
        OnItemDataBound="rptVehicleInformation_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td>
                    <div align="center">
                        <asp:ImageButton ID="btnDeleteVehicle" CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png"
                            Width="18" />
                    </div>
                </td>
                <td>
                    <cc1:CADateTimeControl ID="CADateTimeVehicleDate" runat="server" DateOnly="true" />
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtTime" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtVehicleNum" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtTo" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtClass" runat="server" CssClass="input_ts4"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptVehicleInformationDisplay" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Date")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Time")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("VehicleNumber")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("VehicleFrom")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("VehicleTo")%>
                    &nbsp;
                </td>
                <td>
                    <%# Eval("Class")%>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
