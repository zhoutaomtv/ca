<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.ascx.cs"
    Inherits="CA.SharePoint.Web.FileUpload" %>
<asp:FileUpload ID="fulFileName" runat="server" Width="95%" />
<asp:HyperLink ID="lnkFileName" runat="server" Target="_blank">[lnkFileName]</asp:HyperLink>
<asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return alert('Are you sure you want to remove this file?');" onclick="btnDelete_Click" />