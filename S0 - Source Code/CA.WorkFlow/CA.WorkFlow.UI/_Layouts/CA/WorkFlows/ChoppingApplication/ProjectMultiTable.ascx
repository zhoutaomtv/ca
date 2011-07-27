<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectMultiTable.ascx.cs"
    Inherits="CA.WorkFlow.UI.ChoppingApplication.ProjectMultiTable" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<%@ Register Src="FileMultiTable.ascx" TagName="FileMultiTable" TagPrefix="uc2" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="workflow_table">
    <tr>
        <th colspan="2" valign="top">
            <p>
                Project/项目</p>
            <p>
                &nbsp;</p>
        </th>
    </tr>
    <tr>
        <td width="30%" valign="top">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr class="workflow_table">
                    <th width="5%">
                       &nbsp;
                    </th>
                    <th width="95%">
                        <p>
                            Name/名称</p>
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="rptProjects" 
                    onitemcommand="rptProjects_ItemCommand" 
                    onitemdatabound="rptProjects_ItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                               <CAControls:CAImageButton ID="ImageButton1" CommandName="delete" CommandArgument='<%#Eval("Name") %>' runat="server"
                        ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" CausesValidation="false" />
                            </td>
                            <td>                                
                                <asp:LinkButton runat="server" ID="lnkButton" Text='<%#Eval("Name") %>' CommandName="link" CommandArgument='<%#Eval("Name") %>'></asp:LinkButton> 
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
                <tr id="trAdd" runat="server">
                    <td>
                        <CAControls:CAImageButton runat="server" ID="imgbtnAdd" ImageUrl="../images/pixelicious_001.png"
                            OnClick="imgbtnAdd_Click" Width="18" CausesValidation="false" OnClientClick="return CheckProject();"/>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top" width="70%">
            <uc2:FileMultiTable ID="FileMultiTable1" runat="server" />
        </td>
    </tr>
</table>
<script type="text/javascript" language="javascript">
function CheckProject()
{
    var txtProject=document.getElementById("<%=txtName.ClientID %>");
    if(txtProject.value=="")
    {
        alert("Project name requied.");
        return false;
    }
    return true;
}
</script>