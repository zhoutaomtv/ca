<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CALeaveBalance.ascx.cs"
    Inherits="CA.SharePoint.WebControls.CALeaveBalance" %>

<script src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js" type="text/javascript"></script>

<script src="/_layouts/CAResources/themeCA/js/jquery.ui.core.js" type="text/javascript"></script>

<script src="/_layouts/CAResources/themeCA/js/jquery.ui.datepicker.js" type="text/javascript"></script>

<script src="/_layouts/CAResources/themeCA/js/jquery.ui.widget.js" type="text/javascript"></script>

<link rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui-1.8.6.custom.css" />
<link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery.ui.datepicker.css" />

<script type="text/javascript">
    $(function() {
        $(".alttb tr:even").addClass("altrow");
    });

    function popexcel(url) {
        var w = window.open(url, '_blank');
        w.location.href = url;
    }
</script>

<style type="text/css">
    .alttb
    {
        width: 100%;
        line-height: 22px;
    }
    .altrow
    {
        background-color: #eee;
    }
    .tr2
    {
        font-family: verdana;
        background-color: #d4e5ff;
    }
</style>
<table style="width:100%;">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblfYear" Text="Year"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtfYear"></asp:TextBox>
        </td>
        <td>
            <asp:Label runat="server" ID="lblfDepartment" Text="Department"></asp:Label>
        </td>
        <td>
            <asp:HiddenField ID="hidAssoDepts" runat="server" />
            <asp:DropDownList ID="ddlDepartments" runat="server" Width="150">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblfAnnualEntitlement" Text="AnnualEntitlement"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtfAnnualEntitlement"></asp:TextBox>
        </td>
        <td>
            <asp:Label runat="server" ID="lblfSickEntitlement" Text="SickEntitlement"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtfSickEntitlement"></asp:TextBox>
        </td>
    </tr>
    <tr>
        
        <td colspan="4">
            <asp:Label runat="server" ID="lblIsMe" Text="Only show direct report"></asp:Label>
            <asp:HiddenField ID="hidCurDispName" runat="server" />
            <asp:CheckBox ID="chkIsReportToMe" runat="server" />
        </td>
        
    </tr>
    <tr>
        <td colspan="4" style="text-align:right;">
            <asp:Button ID="btnFilter" runat="server" Text="Search" Width="80" />
            <asp:Button ID="btnExport" runat="server" Text="Export" Width="80" />
            <asp:Label runat="server" ID="lblTmpXlsUrl"></asp:Label>
        </td>
    </tr>
</table>
<asp:Label runat="server" ID="lblOP"></asp:Label>
<p>
    
   
</p>
