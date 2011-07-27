<%@ Assembly Name="CA.SharePoint.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09605105b1332138" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="NewsApprove.aspx.cs" Inherits="CA.WorkFlow.NewsApprove"
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
    <asp:Literal runat="server" ID="litTitle"></asp:Literal>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="left">
        <div class="user">
            <em>
                <asp:Literal runat="server" ID="litTime"></asp:Literal></em>
            <h3>
                <asp:Literal runat="server" ID="litCreated"></asp:Literal></h3>
        </div>
        <div>
            <p>
                <asp:Literal runat="server" ID="litBody"></asp:Literal></p>
        </div>
        <div runat="server" id="divFile">
            <p>
                <iframe src="<%=FileUrl %>" style="width: 590px; overflow-x: scroll; overflow-y: scroll;
                    height: 600px;"></iframe>
                </p>
        </div>
        <div runat ="server" id="divEmail">
            <asp:CheckBox ID="ckbEmail" runat="server" Text="Send Email to all employees" Enabled="false" />
        </div>
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
                     <QFC:ActionsButton ID="actions" runat="server" />
                    </tr>
                </table>
            </td>          
        </tr>
    </table>
        <SharePoint:FormDigest ID="FormDigest1" runat="server">
        </SharePoint:FormDigest>
</asp:Content>
