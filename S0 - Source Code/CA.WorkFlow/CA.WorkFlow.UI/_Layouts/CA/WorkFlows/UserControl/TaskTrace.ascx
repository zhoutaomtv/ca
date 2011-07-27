<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskTrace.ascx.cs" Inherits="CA.WorkFlow.UI.UC.TaskTrace" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

<br/>
<div class="hr-line"></div>

<div class="trace-list-title">
    Applicant: <asp:Label ID="ApplicantLabel" runat="server" CssClass="bold" />
</div>
<div class="trace-list-grid full-width">
     <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" 
         BorderWidth="1px" GridLines="Horizontal" />
</div>

