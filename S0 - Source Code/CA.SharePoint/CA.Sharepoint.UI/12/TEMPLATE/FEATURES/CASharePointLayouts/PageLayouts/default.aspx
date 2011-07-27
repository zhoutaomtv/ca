<%@ Page Language="C#" Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="OSRVWC" Namespace="Microsoft.Office.Server.WebControls" Assembly="Microsoft.Office.Server, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SPSWC" Namespace="Microsoft.SharePoint.Portal.WebControls"
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SEARCHWC" Namespace="Microsoft.Office.Server.Search.WebControls"
    Assembly="Microsoft.Office.Server.Search, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:content contentplaceholderid="PlaceHolderPageTitle" runat="server"> 
    <SharePoint:FieldValue id="PageTitle" FieldName="Title" runat="server" />
</asp:content>
<asp:content contentplaceholderid="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:content>
<asp:content contentplaceholderid="PlaceHolderPageImage" runat="server"></asp:content>
<asp:content contentplaceholderid="PlaceHolderTitleBreadcrumb" runat="server" />
<asp:content contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server">
</asp:content>
<asp:content contentplaceholderid="PlaceHolderBodyAreaClass" runat="server">
</asp:content>
<asp:content contentplaceholderid="PlaceHolderMain" runat="server">


                <div class="div_10px">
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="195" valign="top">
                                <!-- Left Zone -->
                                <WebPartPages:WebPartZone QuickAdd-GroupNames="Default" runat="server" Title="左边区域" ID="Left" FrameType="TitleBarOnly" />
                        </td>
                        <td width="14">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <!-- Top Middle Zone -->
                                        <WebPartPages:WebPartZone runat="server" Title="中上区域" ID="TopMiddle" FrameType="TitleBarOnly" />
                                    </td>
                                </tr>
                            </table>
                            <div class="div_10px">
                            </div>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <!-- Middle Left Zone -->
                                        <WebPartPages:WebPartZone runat="server" Title="中左区域" ID="MiddleLeft" FrameType="TitleBarOnly" />
                                    </td>
                                    <td width="15">
                                    </td>
                                    <td>
                                        <!-- Middle Right Zone -->
                                        <WebPartPages:WebPartZone runat="server" Title="中右区域" ID="MiddleRight" FrameType="TitleBarOnly" />
                                    </td>
                                </tr>
                            </table>
                            <div class="div_10px">
                            </div>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <!-- Bottom Middle Zone -->
                                        <WebPartPages:WebPartZone runat="server" Title="中下区域" ID="BottomMiddle" FrameType="TitleBarOnly" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <WebPartPages:WebPartZone runat="server" Title="下区域" ID="BottomZone" FrameType="TitleBarOnly" />
                        </td>
                    </tr>
                </table>
</asp:content>
