<%@ Control Language="C#" AutoEventWireup="true" %>

<script src="/_layouts/CAResources/themeCA/js/droplinemenu.js" type="text/javascript"></script>

<script type="text/javascript">
    droplinemenu.buildmenu("mydroplinemenu")
</script>

<table border="0" align="center" cellspacing="0" cellpadding="0" class="menu_table"
    width="100%">
    <tr>
        <td background="/_layouts/CAResources/themeCA/images/menu-trans.png">
            <div id="mydroplinemenu" class="droplinebar">
                <ul>
                    <li><a href="/default.aspx">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:ca,Home%>" />
                    </a></li>
                    <li>
                        <a href="#"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:ca,Company%>" /></a>
                        <ul>
                            <li><a href="/Lists/CompanyNews/">
                                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:ca,CompanyNews%>" />
                            </a></li>
                            <li><a href="/Lists/Announcements/">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:ca,Announcement%>" />
                            </a></li>
                            <li><a href="/Lists/Recruitment/">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:ca,Recruitment%>" />
                            </a></li>
                            <li><a href="/Policy/">
                                <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:ca,Policy%>" />
                            </a></li>
                        </ul>
                    </li>
                    <li><a href="/CA/Departments.aspx">
                        <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:ca,Department%>" />
                    </a></li>
                    <li>
                        <a href="#"><asp:Literal ID="Literal7" runat="server" Text="<%$Resources:ca,Workflow%>" /></a>
                        <ul>
                            <li><a href="/WorkFlowCenter/default.aspx" title="Start Workflow">
                                <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:ca,StartWorkflow%>" />
                            </a></li>
                            <li><a href="/CA/MyTasks.aspx" title="My Tasks">
                                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:ca,MyTasks%>" />
                            </a></li>
                            <li><a href="/WorkFlowCenter/history.aspx" title="Task History">
                                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:ca,TaskHistory%>" />
                            </a></li>
                        </ul>
                    </li>
                    <li><a href="/documentcenter/default.aspx">
                        <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:ca,Library%>" />
                    </a></li>
                    <li><a href="/SearchCenter/Pages/default.aspx">
                        <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:ca,Search%>" />
                    </a></li>
                </ul>
            </div>
        </td>
    </tr>
</table>
<%--<table border="0" align="center" cellspacing="0" cellpadding="0" class="menu_table">
    <tr>
        <td>
            <ul id="menu">
                <li onmouseover="displaySubMenu(this)" onmouseout="hideSubMenu(this)">
                    <!--[if lte IE 6]><a href="#nogo">
						<table>
							<tr>
								<td><![endif]-->
                    <dl class="one">
                        <dt><a href="/default.aspx">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:ca,Home%>" />
                        </a></dt>
                    </dl>
                    <!--[if lte IE 6]></td>
							</tr>
						</table>
						</a><![endif]-->
                </li>
                <li onmouseover="displaySubMenu(this)" onmouseout="hideSubMenu(this)">
                    <!--[if lte IE 6]><a href="#nogo">
						<table>
							<tr>
								<td><![endif]-->
                    <dl class="two">
                        <dt>
                            <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:ca,Company%>" /></dt>
                        <dd>
                            <a href="/Lists/CompanyNews/">
                                <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:ca,CompanyNews%>" />
                            </a>
                        </dd>
                        <dd>
                            <a href="/Lists/Announcements/">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:ca,Announcement%>" />
                            </a>
                        </dd>
                        <dd>
                            <a href="/Lists/Recruitment/">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:ca,Recruitment%>" />
                            </a>
                        </dd>
                        <dd>
                            <a href="/DocumentCenter/Policy/">
                                <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:ca,Policy%>" />
                            </a>
                        </dd>
                    </dl>
                    <!--[if lte IE 6]></td>
							</tr>
						</table>
						</a><![endif]-->
                </li>
                <li onmouseover="displaySubMenu(this)" onmouseout="hideSubMenu(this)">
                    <!--[if lte IE 6]><a href="#nogo">
						<table>
							<tr>
								<td><![endif]-->
                    <dl class="one">
                        <dt><a href="/CA/Departments.aspx">
                            <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:ca,Department%>" />
                        </a></dt>
                    </dl>
                    <!--[if lte IE 6]></td>
							</tr>
						</table>
						</a><![endif]-->
                </li>
                <li onmouseover="displaySubMenu(this)" onmouseout="hideSubMenu(this)">
                    <!--[if lte IE 6]><a href="#nogo">
						<table>
							<tr>
								<td><![endif]-->
                    <dl class="two">
                        <dt>
                            <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:ca,Workflow%>" /></dt>
                        <dd>
                            <a href="/WorkFlowCenter/default.aspx" title="Start Workflow">
                                <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:ca,StartWorkflow%>" />
                            </a>
                        </dd>
                        <dd>
                            <a href="/WorkFlowCenter/Lists/Tasks/MyItems.aspx" title="My Tasks">
                                <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:ca,MyTasks%>" />
                            </a>
                        </dd>
                        <dd>
                            <a href="/WorkFlowCenter/Lists/Tasks/MyItems.aspx" title="Task History">
                                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:ca,TaskHistory%>" />
                            </a>
                        </dd>
                    </dl>
                    <!--[if lte IE 6]></td>
							</tr>
						</table>
						</a><![endif]-->
                </li>
                <li onmouseover="displaySubMenu(this)" onmouseout="hideSubMenu(this)">
                    <!--[if lte IE 6]><a href="#nogo">
						<table>
							<tr>
								<td><![endif]-->
                    <dl class="one">
                        <dt><a href="/documentcenter/default.aspx">
                            <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:ca,Library%>" />
                        </a></dt>
                    </dl>
                    <!--[if lte IE 6]></td>
							</tr>
						</table>
						</a><![endif]-->
                </li>
                <li onmouseover="displaySubMenu(this)" onmouseout="hideSubMenu(this)">
                    <!--[if lte IE 6]><a href="#nogo">
						<table>
							<tr>
								<td><![endif]-->
                    <dl class="one">
                        <dt><a href="/SearchCenter/Pages/default.aspx">
                            <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:ca,Search%>" />
                        </a></dt>
                    </dl>
                    <!--[if lte IE 6]></td>
							</tr>
						</table>
						</a><![endif]-->
                </li>
            </ul>
            <!-- end of info -->
        </td>
    </tr>
</table>--%>

<script language="javascript" type="text/javascript">
    function showbg() {
        var sWidth, sHeight;
        sWidth = screen.width;
        sHeight = screen.height;
        var bgObj = document.createElement("div");
        bgObj.setAttribute('id', 'bgDiv');
        bgObj.style.position = "absolute";
        bgObj.style.top = "0";
        bgObj.style.background = "#ccc";
        bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
        bgObj.style.opacity = "0.6";
        bgObj.style.left = "0";
        bgObj.style.width = sWidth + "px";
        bgObj.style.height = sHeight + "px";
        bgObj.style.zIndex = "10000";
        document.body.appendChild(bgObj);
    }

    function hidebg() {
        var bgObj = document.getElementById("bgDiv");
        document.body.removeChild(bgObj);
    }
    
</script>

