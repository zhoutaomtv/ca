<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CALeaveData.ascx.cs" Inherits="CA.SharePoint.WebControls.CALeaveData" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/site.css" />
<script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
<script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
<script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
<script type="text/javascript">
    _spOriginalFormAction = document.forms[0].action;
    _spSuppressFormOnSubmitWrapper = true;
    function popexcel(url) {
        var w = window.open(url, '_blank');

        if (w) {
            w.location.href = url;
        }
    }
</script>
<style type="text/css">
    #ca-leave-data
    {
        width: 620px;
        height: 600px;
    }

    .valign-middle
    {
        vertical-align: middle;
    }

    .h-line
    {
        margin: 10px 0;
        border-bottom: 2px solid #9DABB6;
    }

    .hr-line p
    {
        margin-bottom: 10px;
    }
</style>
<div id="ca-leave-data">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="h-line">
                <h3 class="float-left">
                    <br />
                    Search:
                </h3>
                <div class="ca-workflow-form-buttons">
                    <asp:Button ID="btnApply" Text="Apply" runat="server" OnClick="btnApply_Click" />
                    <asp:Button ID="btnDownload" Text="Download" runat="server" OnClick="btnDownload_Click" />
                </div>
            </div>
            <div class="valign-middle">
                Year/Month:
                <asp:DropDownList ID="ddlStartMonthes" runat="server" CssClass="valign-middle" AutoPostBack="true" OnSelectedIndexChanged="ddlStartMonthes_Changed" />
            </div>
            <div class="h-line">
                <p>
                    Non-Paid Leave Report:<br />
                </p>
            </div>
            <SharePoint:SPGridView ID="spgvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table" EnableTheming="False" OnPageIndexChanging="spgvResult_PageIndexChanging">
                <AlternatingRowStyle CssClass="each-row ms-alternating" />
                <RowStyle CssClass="each-row" />
                <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                <Columns>
                    <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
                    <asp:BoundField DataField="TimeWageType" HeaderText="Wage Type" />
                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:BoundField DataField="Number" HeaderText="Number" />
                    <asp:BoundField DataField="DataType" HeaderText="Data Type" />
                </Columns>
            </SharePoint:SPGridView>
            <div class="align-center">
                <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="spgvResult">
                </SharePoint:SPGridViewPager>
            </div>
            <asp:ObjectDataSource ID="dsLeaveData" runat="server" SelectMethod="Query" TypeName="CA.SharePoint.WebControls.CALeaveData" />
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnApply" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDownload" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlStartMonthes" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="ca-workflow-form-note">
        <div class="top">
            &nbsp;</div>
        <div class="middle align-center">
            Wage Type
        </div>
        <div class="middle">
            *6HS0&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Not full-paid sick leave hours<br />
            *6HS1&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;No-pay leave hours <br />
            *6HS6&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Maternity leave days<br />
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>