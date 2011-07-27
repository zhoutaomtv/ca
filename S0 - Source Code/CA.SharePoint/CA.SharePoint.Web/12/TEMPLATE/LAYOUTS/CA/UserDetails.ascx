<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs"
    Inherits="CA.SharePoint.Web.UserDetails" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" runat="server">
    <tr>
        <th width="108" valign="top">
            <div id="projectthumnail">
                <img width="100" src="<%=strPhotoUrl %>" style="vertical-align:top" />
            </div>
        </th>
        <th  align="left" valign="top">
            <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <th width="15%">
                        Name:
                    </th>
                    <th width="35%" align="left">
                        <%=strUserName%>&nbsp;
                    </th>
                    <th width="15%">
                        Dept:
                    </th>
                    <th width="35%">
                        <%=strDept%>&nbsp;
                    </th>
                </tr>
                <tr>
                    <th>
                        Cell:
                    </th>
                    <th>
                        <%=strMobile%>&nbsp;
                    </th>
                    <th>
                        Phone:
                    </th>
                    <th>
                        <%=strPhone%>&nbsp;
                    </th>
                </tr>
                <tr>
                    <th width="10%">
                        Email:
                    </th>
                    <th colspan="3">
                        <a href="mailto:<%=strEmail%>">
                            <%=strEmail%>&nbsp;</a>
                    </th>
                </tr>
                <tr>
                    <th>
                        Title:
                    </th>
                    <th colspan="3">
                        <%=strTitle%>&nbsp;
                    </th>
                </tr>
                
            </table>
        </th>
    </tr>
</table>
