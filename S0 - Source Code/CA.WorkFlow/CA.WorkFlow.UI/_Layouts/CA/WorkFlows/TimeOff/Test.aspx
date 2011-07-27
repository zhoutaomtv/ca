<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.TimeOff.Test" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI.Controls" TagPrefix="cc1" %>
<%@ Register namespace="SmartForm" assembly="SmartForm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5c316ab1553f48f2" TagPrefix="SF" %>
<%@ Register src="../UserControl/FileFieldTemplate.ascx" tagname="FileFieldTemplate" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/WorkFlow.css" rel="stylesheet" type="text/css" />

    <script type="text/JavaScript">
    function showMsg(ctl,args)
        {
            if(confirm(document.add.txt_title.value))
            {
                args.IsValid=true;
            }
            else
            {
                args.IsValid=false;
            }
        }
    </script>

    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%-- <asp:GridView runat="server" ID="gvTable" CssClass="workflower_table" 
                    AutoGenerateColumns="False">
             
             <Columns>
             <asp:TemplateField>
             <ItemTemplate>
              <asp:Label  ID="lab4" runat="server" Text='<%# Eval("Name")%> '></asp:Label>
             </ItemTemplate>
             <EditItemTemplate>
               <asp:TextBox ID="TextBox4" runat="server" Text='<%# Eval("Name")%> '></asp:TextBox>
             </EditItemTemplate>
             </asp:TemplateField>
             </Columns>
                 <EmptyDataTemplate>
                   
                 </EmptyDataTemplate>
             
             
             </asp:GridView>--%>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflower_table">
            <tr class="workflower_table">
                <th colspan="5">
                    Leave Record
                </th>
            </tr>
            <tr class="workflower_table">
                <th rowspan="2">
                    <asp:ImageButton runat="server" Visible="false" ImageUrl="../images/pixelicious_001.png"
                        Width="18" Height="18" ID="imgbtnAdd" />
                    &nbsp;
                </th>
                <th rowspan="2">
                    Type
                </th>
                <th colspan="2">
                    Date
                </th>
                <th rowspan="2">
                    Days
                </th>
            </tr>
            <tr class="workflower_table">
                <th>
                    From
                </th>
                <th>
                    To
                </th>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                <ItemTemplate>
                    <tr class="workflower_table">
                        <td>
                            <asp:ImageButton ID="ImageButton1" CommandName="delete" CommandArgument="ID" runat="server"
                                ImageUrl="../images/pixelicious_028.png" Width="18" Height="18" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lab1" Text='<%# Eval("Name")%>'></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr runat="server" id="trTemplate">
                <td>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <cc1:CATableFiled ID="table1" runat="server" >        
          <cc1:CAFileControl2 ID="CAFileControltmp2" runat="server" DocLibName="Files" />    
        </cc1:CATableFiled>
        
        <sf:TableField ID="table2" runat="server">
        <sf:TextField runat="server" ID="test1"></sf:TextField>
        </sf:TableField>
       <cc1:CAFileControl2 ID="CAFileControltmp" runat="server" DocLibName="Files" />
       <cc1:SPDocLibField ID="spdoc1" runat="server" DocLibName="Files" ShowVersion="true" ControlMode="Edit" LinkTitle="uplaods files" AutoStoreByTime="true" />
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <asp:TextBox ID="txt_title" runat="server" Width="240" Text="dddddddddddd"></asp:TextBox>
        <asp:TextBox ID="txt_hidden" runat="server" Width="0" ReadOnly="True" Text="fffffffffff"></asp:TextBox>
        <asp:CustomValidator ID="Customvalidator2" runat="server" OnServerValidate="Val2"
            ClientValidationFunction="showMsg" ControlToValidate="txt_hidden" Display="Dynamic"></asp:CustomValidator>
    </div>
    </form>
</body>
</html>
