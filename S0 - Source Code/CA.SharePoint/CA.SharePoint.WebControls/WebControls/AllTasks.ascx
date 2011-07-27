<%@ Register TagPrefix="sp" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTasks.ascx.cs" Inherits="CA.SharePoint.WebControls.AllTasks" %>
<%@ OutputCache VaryByParam="None" Duration="5" %>
<sp:SPGridView ID="gvList" runat="server" AutoGenerateColumns="False" AllowPaging="False" EnableViewState="false" AllowGrouping="true" AllowGroupCollapse="true" PageSize="3" GroupField="WorkflowName" EmptyDataText="you don't have any task.">
    <AlternatingRowStyle CssClass="ms-alternating" />
    <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
    <Columns>
        <sp:SPMenuField HeaderText="Task" MenuTemplateId="MenuList" TokenNameAndValueFields="WorkflowUrl=WorkflowUrl" TextFields="TaskTitle" NavigateUrlFields="WorkflowUrl" NavigateUrlFormat="{0}" />
        <asp:BoundField HeaderText="Start Date" DataField="StartTime" />
    </Columns>
</sp:SPGridView>
<sp:MenuTemplate ID="MenuList" runat="server">
    <sp:MenuItemTemplate ID="viewFormMenu" runat="server" Text="Open Task" ImageUrl="/_layouts/images/ApViewItem.gif" ClientOnClickNavigateUrl="%WorkflowUrl%" />
</sp:MenuTemplate>