<%@ Assembly Name="CA.SharePoint.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09605105b1332138" %>

<%@ Page MasterPageFile="~/_layouts/application.master" Language="C#" AutoEventWireup="true"
    CodeBehind="DocumentLimit.aspx.cs" Inherits="CA.SharePoint.Web.DocumentLimit" %>

<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea">
   <%=ListName %> Document Limit Setting
</asp:Content>
<asp:Content runat="server" ID="c1" ContentPlaceHolderID="PlaceHolderMain">
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <tr id="HideSection1">
            <td class="ms-sectionline" height="1" colspan="4">
                <img src="/_layouts/images/blank.gif" width="1" height="1" alt="">
            </td>
        </tr>
        <tr id="HideSection2">
            <td nowrap rowspan="2">
            </td>
            <td class="ms-descriptiontext" rowspan="2" valign="top" id="align07" style="width: 200px;">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td class="ms-sectionheader" height="22" valign="top">
                            <h3 class="ms-standardheader">
                                Document Size Limit</h3>
                        </td>
                    </tr>
                    <tr>
                        <td id="onetidNewColumnDescription" class="ms-descriptiontext">
                            Document Size Limit &nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtFileLimit"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtFileLimit" Type="Double"
                                ErrorMessage="please put in validation value."></asp:RangeValidator>
                        </td>
                        <td width="10">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td class="ms-authoringcontrols" colspan="2">
                <img src="/_layouts/images/blank.gif" width="1" height="1" alt="">
            </td>
        </tr>
        <tr valign="top" id="HideSection5">
            <td class="ms-authoringcontrols" style="width: 200px;">
                <asp:TextBox runat="server" ID="txtFileLimit" Width="100"></asp:TextBox>GB
            </td>
            <td class="ms-authoringcontrols">
                <asp:Button runat="server" ID="btnSure" Text="Save" CssClass="ms-ButtonHeightWidth" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="ms-ButtonHeightWidth" CausesValidation="false"/>
            </td>
        </tr>
        <tr id="HideSection6">
            <td colspan="2">
                &nbsp;
            </td>
            <td class="ms-authoringcontrols" colspan="2" height="21">
            </td>
        </tr>
        <tr id="Tr1">
            <td class="ms-sectionline" height="1" colspan="4">
                <img src="/_layouts/images/blank.gif" width="1" height="1" alt="">
            </td>
        </tr>
    </table>
</asp:Content>
