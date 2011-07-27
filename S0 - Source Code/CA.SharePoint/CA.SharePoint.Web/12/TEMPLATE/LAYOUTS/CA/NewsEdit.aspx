<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="NewsEdit.aspx.cs" Inherits="CA.SharePoint.Web.NewsEdit"
    Title="Untitled Page" %>
<%@ Register Src="FileUpload.ascx" TagName="FileUpload" TagPrefix="uc1" %>
<%@ Register Assembly="CA.SharePoint.WebControls" Namespace="CA.SharePoint.WebControls"
    TagPrefix="cc1" %>
    <%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Publish News
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script language="javascript">
        $(document).ready(function() {
            $("#btnSave").bind("click", function() {
                if ($("#txtTitle").val() == "") {
                    alert("Title can not be blank.");
                    $("#txtTitle").focus();
                    return false;
                }
            });
        });
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
        <tr>
            <td nowrap="true" valign="top" width="50px" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Type<span class="ms-formvalidation"> *</span></nobr>
                </h3>
            </td>
            <td valign="top" class="ms-formbody">
                <span dir="none">
                    <asp:DropDownList runat="server" ID="ddlType1">
                        <asp:ListItem>Head Office Recruitment</asp:ListItem>
                        <asp:ListItem>Store Recruitment</asp:ListItem>
                    </asp:DropDownList>
                </span>
            </td>
        </tr>
      <%--  <tr >
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
        <tr id="trAttachment">
            <td nowrap="true" valign="top" width="50px" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Attachment<span class="ms-formvalidation"> </span></nobr>
                </h3>
            </td>
            <td valign="top" class="ms-formbody">               
                             <uc1:FileUpload runat="server" ID="upload" />     
            </td>
        </tr>
        <tr id="trContent">
            <td nowrap="true" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Body</nobr>
                </h3>
            </td>
            <td valign="top" class="ms-formbody">
             <QFL:FormField ID="formFieldBody" runat="server" FieldName="Body" ControlMode="Edit">
            </QFL:FormField>
               
            </td>
        </tr>
    </table>
    <table class="ms-formtoolbar" cellpadding="2" cellspacing="0" border="0" id="ctl00_m_g_d8d7a69b_30ec_4f3f_81e2_9a89b8a7f8fc_ctl00_toolBarTbl"
        width="100%">
        <tr>
            <td width="99%" class="ms-toolbar" nowrap>
                <img src="/_layouts/images/blank.gif" width="1" height="18" alt="">
            </td>
            <td class="ms-toolbar" nowrap="true">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="right" width="100%" nowrap>
                            <asp:ImageButton runat="server" ImageUrl="/_layouts/CAResources/themeCA/images/send.gif" ID="btnSave" Text="Submit" />
                        &nbsp;
                        <asp:ImageButton runat="server" ImageUrl="/_layouts/CAResources/themeCA/images/cancel.gif" ID="btnCancle" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </td>
           
        </tr>
    </table>
</asp:Content>
