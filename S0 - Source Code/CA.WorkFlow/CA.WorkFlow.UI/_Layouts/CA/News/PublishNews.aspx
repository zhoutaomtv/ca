<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="PublishNews.aspx.cs" Inherits="CA.WorkFlow.PublishNews"
    Title="Untitled Page" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
 <%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
 <%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    New Item
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
 <QFL:ListFormControl ID="ListFormControl2" runat="server" FormMode="New">
 <script language="javascript">
    $(document).ready(function() {
    $("#" + "<%=StartWorkflowButton1.ClientID %>").bind("click", function() {
            if ($("#" + "<%=txtTitle.ClientID %>").val() == "") {
                alert("Title can not be blank.");
                $("#" + "<%=txtTitle.ClientID %>").focus();
                return false;
            }
        });
    });
//    $(document).ready(function() {
//    $("#" + "<%=StartWorkflowButton2.ClientID %>").bind("click", function() {
//            alert(1);
//            if ($("#" + "<%=txtTitle.ClientID %>").val() == "") {
//                alert("Please input Title!");
//                $("#" + "<%=txtTitle.ClientID %>").focus();
//                return false;
//            }
//        });
//    });
    </script>
    <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0"
        width="98%">
        <tr>
            <td nowrap="true" valign="top" width="50px" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Title<span class="ms-formvalidation"> *</span></nobr>
                </h3>
            </td>
            <td valign="top" class="ms-formbody">
                <span dir="none">
                    <asp:TextBox runat="server" ID="txtTitle"></asp:TextBox>
                </span>
            </td>
        </tr>
     <%--   <tr>
            <td nowrap="true" valign="top" width="50px" class="ms-formlabel">
                Attachment
            </td>
            <td valign="top" class="ms-formbody">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                <asp:ListItem Text="Attachment" Value="true" Selected="True" ></asp:ListItem>
                <asp:ListItem Text="Content" Value="false" Selected="false" ></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>--%>
        <tr id="trAttachment" >
            <td nowrap="true" valign="top" width="50px" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Attachment<span class="ms-formvalidation"> </span></nobr>
                </h3>
            </td>
            <td valign="top" class="ms-formbody">
                <span dir="none">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </span>
            </td>
        </tr>
        <tr id="trContent">
            <td nowrap="true" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Body</nobr>
                </h3>
            </td>
            <td valign="top" class="ms-formbody" width="100%">
              <QFL:FormField ID="formFieldBody" runat="server" FieldName="Body" ControlMode="New">
            </QFL:FormField>
              <%--  <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Height="600px" AutoDetectLanguage="false" BasePath="~/_layouts/ca/fckeditor/">
                </FCKeditorV2:FCKeditor>--%>
            </td>
        </tr>
    </table>
        <div runat ="server" id="divEmail">
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbEmail" runat="server" Text="Send Email to all employees" /><br />
                    </td>
                </tr>
                <tr>
                    <th style="color: Red">
                        Warning: Checking this means every employee will receive an email about this news!
                    </th>
                </tr>
            </table>
        </div>
    <table class="ms-formtoolbar" cellpadding="2" cellspacing="0" border="0" id="ctl00_m_g_d8d7a69b_30ec_4f3f_81e2_9a89b8a7f8fc_ctl00_toolBarTbl"
        width="98%">
        <tr>
            <td width="99%" class="ms-toolbar" nowrap>
                <img src="/_layouts/images/blank.gif" width="1" height="18" alt="">
            </td>
            <td class="ms-toolbar" nowrap="true">
                <table cellpadding="0" cellspacing="0" width="98%">
                    <tr>
                        <td align="right" width="100%" nowrap>
                        <CAControls:CAStartWFButton  ID="StartWorkflowButton1" runat="server" Text="Submit" />
                        &nbsp;<CAControls:CAStartWFButton  ID="StartWorkflowButton2" runat="server" Text="Save"/>         
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" />  &nbsp;                     
                        </td>
                    </tr>
                </table>
            </td>          
        </tr>
    </table>
    
    <SharePoint:FormDigest ID="FormDigest1" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
</asp:Content>
