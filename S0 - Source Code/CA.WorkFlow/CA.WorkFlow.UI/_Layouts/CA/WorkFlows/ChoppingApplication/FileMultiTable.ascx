<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileMultiTable.ascx.cs"
    Inherits="CA.WorkFlow.UI.ChoppingApplication.FileMultiTable" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
<tr>
<td colspan="5" align="center">
<asp:Label runat="server" ID="labProject"></asp:Label>
</td>
</tr>
    <tr>
        <td width="5%">
            &nbsp;
        </td>
        <td>
            文件上传/Upload File
        </td>
        <td>
            价格/Price
        </td>
        <td>
            份数/Count
        </td>
        <td width="25%">
            描述/Description
        </td>
    </tr>
    <asp:Repeater runat="server" ID="Repeater1" 
        onitemcommand="Repeater1_ItemCommand" 
        onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <CAControls:CAImageButton ID="ImageButton1" CommandName="delete" CommandArgument='<%#Eval("ID") %>' runat="server"
                        ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" CausesValidation="false" OnClientClick="" />
                </td>
                <td>
                    <%--<sf:SPFileField ID="CAFileControltmp2" runat="server" DocLibName="ChoppingProjectFiles" ControlMode="Display"></sf:SPFileField>--%>
                <a href='<%#Eval("FileUrl") %>' target="_blank"><asp:Label runat="server" ID="labFile" Text='<%#Eval("Name") %>'></asp:Label></a>
                </td>
                <td>
                   <asp:Label runat="server" ID="labPrice" Text='<%#Eval("Price") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="labCount" Text='<%#Eval("Count") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="labDescription" Text='<%#Eval("Description") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
    <tr id="trAdd" runat="server">
        <td>
               <CAControls:CAImageButton runat="server" ID="imgbtnAdd" ImageUrl="../images/pixelicious_001.png"
                            OnClick="imgbtnAdd_Click" OnClientClick="return Check();" Width="18" CausesValidation="false" />
        </td>
        <td>
        <asp:FileUpload runat="server" ID="fileUpload" Width="200px"/>
            <%--<sf:SPFileField ID="CAFileControltmp2" runat="server" DocLibName="ChoppingProjectFiles" ControlMode="Edit"></sf:SPFileField>--%>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtPrice"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtCount"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtDescription"></asp:TextBox>
        </td>
    </tr>
</table>
<script type="text/javascript" language="javascript">
 function Trim(s) {
        var ss = s.replace(/^[\s]+/g, "");
        ss = ss.replace(/[\s]+$/g, "");
        return ss;
    }
    function IsFloat(value) {
        value = Trim(value.toString());
        var re = /^\d{1,}$|\.\d{1,}$/;
        var re2 = /^-\d{1,}$|\.\d{1,}$/;
        var r = value.match(re);
        var r2 = value.match(re2);
        return r != null || r2 != null;
    }

    function IsInt(value) {
        value = Trim(value.toString());
        var re = /^\d{0,}$/;
        var re2 = /^-\d{0,}$/;
        var r = value.match(re);
        var r2 = value.match(re2);
        return r != null || r2 != null;
    }

function Check()
{
  var txtCount=document.getElementById("<%=txtCount.ClientID %>");
  if(!IsInt(Trim(txtCount.value))||Trim(txtCount.value)=="")
  {
    alert("Please supply valid details.");
    return false;
  }  
  return true;
}
</script>