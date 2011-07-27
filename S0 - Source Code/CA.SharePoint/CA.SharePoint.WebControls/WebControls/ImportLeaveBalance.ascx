<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportLeaveBalance.ascx.cs" Inherits="CA.SharePoint.WebControls.ImportLeaveBalance" %>
<script language="javascript">
    $(document).ready(function() {
        $("#" + "<%=btnImport.ClientID %>").bind("click", function() {
            if (!confirm("Are you sure you want to import the data?"))
                return false;
        })
    });
</script>
<table cellpadding="0" border="0" cellspacing="0" width="100%">
    <tr>
        <td><asp:FileUpload ID="btnFileUpload" runat="server" Width="300px"/>&nbsp;&nbsp;<a href="/_LAYOUTS/CA/WebControls/model.xls" style="color:Blue">download template file</a></td>
        <td>
            <asp:Button ID="btnImport" runat="server" Text="Import" Width="80" onclick="btnImport_Click" 
                />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnExport" runat="server" Text="Export" 
                Visible="false" /></td>
    </tr>
</table>