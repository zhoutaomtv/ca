<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Organization.ascx.cs"
    Inherits="CA.SharePoint.WebControls.Organization" %>
<%--<%@ Register TagPrefix="uc" TagName="UserDetails" Src="UserDetails.ascx" %>--%>
<%--<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>--%>
<script type="text/javascript" language="javascript">
//Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//function EndRequestHandler(sender, args) {
//    setScrollPos();
//} 
//function saveScrollPos(){
//    document.getElementById("scrollPos").value = 
//             document.getElementById("divEmployee").scrollTop;
//}
//function setScrollPos(){
//    document.getElementById("divEmployee").scrollTop = 
//             document.getElementById("scrollPos").value;
    //}
    $(document).ready(function() {
        var treeView = document.getElementById("<%=trvEmployees.ClientID %>"); 
        var items = treeView.getElementsByTagName("A");
        for (var i = 0; i < items.length; i++) {
            var eventStr = items[i].href;
            if (eventStr.indexOf("TreeView_ToggleNode") == -1) {
                items[i].onclick = ""; 
                items[i].href = "###";
            }
        }
    });
    function SelectUser(user,dept) {
        $.ajax({
            type: "POST",
            dataType: "html",
            url: '/_LAYOUTS/CA/WebControls/Handler1.ashx',
            data: "user=" + user+"&dept="+dept,
            success: function(html) {
                userDetail.innerHTML=html;
            } 
        });
    }
</script>
<input type="hidden" id="scrollPos" name="scrollPos" value="0" />
<%--
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <table width="612" border="0" cellpadding="0" cellspacing="0" class="Department_News">
            <tr>
                <td width="50%" valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td colspan="2" width="53%" valign="top" class="line">
                                <span class="txt1">Organization</span>
                            </td>
                            <td width="47%" valign="top" class="line">
                                <div class="viewbutton">
                                    <a href="Organization.aspx?Dept=<%=strDept%>" class="view">View More</a></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <th width="190">
                                            <div id="divEmployee" style="width: 180px;overflow:auto; overflow-y: scroll; height: 200px; ">
                                                <asp:TreeView ID="trvEmployees" runat="server"
                                                    ImageSet="Contacts" NodeIndent="10" ShowLines="True">
                                                    <ParentNodeStyle Font-Bold="True" ForeColor="#5555DD" />
                                                    <HoverNodeStyle Font-Underline="False" />
                                                    <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" />
                                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                        NodeSpacing="0px" VerticalPadding="0px" />
                                                </asp:TreeView>
                                            </div>
                                        </th>
                                        <th class="Organization">
                                            <div id="userDetail"></div>
                                            <%--<uc:UserDetails ID="ucUserDetails" runat="server" />--%>
                                        </th>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>