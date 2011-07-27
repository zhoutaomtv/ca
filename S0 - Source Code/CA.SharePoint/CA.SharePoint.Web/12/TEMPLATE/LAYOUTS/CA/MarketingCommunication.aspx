<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingCommunication.aspx.cs"
    Inherits="CA.SharePoint.Web.MarketingCommunication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Marketing Communication</title>
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/table.css" />
    <script language="javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.3.2.min.js"></script>
<script language="javascript" type="text/javascript">
    function doShowTable(obj) {
        obj = document.getElementById(obj);
        document.all.showWindow.style.height = document.body.scrollHeight;
        document.body.style.overflow = "hidden";
        obj.style.top = document.body.scrollTop + 200;
        obj.style.left = '350px';
        document.all.showWindow.style.display = "";
        obj.style.display = "";
    }
    function doHideLetter(obj) {
        obj = document.getElementById(obj);
        document.body.style.overflow = "";
        document.all.showWindow.style.display = 'none';
        var speed = 300;
        $(obj).slideUp(speed);
        //obj.style.display = 'none';
    }
    function GetPosition(el) {
        var left = 0;
        var top = 0;

        if (el.getBoundingClientRect) {
            var box = el.getBoundingClientRect();
            left = box.left + Math.max(document.documentElement.scrollLeft, document.body.scrollLeft) - document.documentElement.clientLeft;
            top = box.top + Math.max(document.documentElement.scrollTop, document.body.scrollTop) - document.documentElement.clientTop;
        }
        else {
            while (el.offsetParent != null) {
                left += el.offsetLeft;
                top += el.offsetTop;
                el = el.offsetParent;
            }

            left += el.offsetLeft;
            top += el.offsetTop;
        }
        return { x: left, y: top };
    }
    var flag = true;
    $(document).ready(function() {
        var speed = 300;
        $(".abcde").each(function(i) {
            $($(".abcde")[i]).bind("click", function(event) {
                //document.body.scrollTop = document.body.scrollHeight;
                event.stopPropagation();
                var offset = $(event.target).offset();
                var a = window.screen.availHeight;
                var b = event.clientY;
                var c = a - b - 150;
                if (c > 240) {
                    $($(".showWindow1")[i]).css({ top: offset.top + $(event.target).height() + "px", left: offset.left });
                }
                else {
                    var d = offset.top - $(event.target).height() - 240;
                    $($(".showWindow1")[i]).css({ top: d + "px", left: offset.left });
                }
                $($(".showWindow1")[i]).slideDown(speed);
                document.all.showWindow.style.display = "";
            });
        });
    });
    </script>

    <style type="text/css">
        .BoxCase
        {
            width: 420px;
            height: 240px;
            border: solid 1px outset #E7c984;
            text-align: left;
            padding: 6px;
			FILTER: alpha(opacity=85);
            background: #FFE7B2;
            font-size: 12px;
            line-height: 17px;
            letter-spacing: 2px;
        }
         #showWindow { BACKGROUND: #e8e8e8; FILTER: Alpha(Opacity=15); LEFT: 0px; WIDTH: 100%; POSITION: absolute; TOP: 0px; HEIGHT: 150%;}
         .showWindow1 { padding: 6px;background: #FFE7B2;BORDER-RIGHT: #000 1px solid; BORDER-TOP: #000 1px solid; width: 480px;height: 240px;Z-INDEX: 50; LEFT: 250px; BORDER-LEFT: #000 1px solid; BORDER-BOTTOM: #000 1px solid; POSITION: absolute; TOP: 250px ;font-size: 12px;
            line-height: 17px;
            letter-spacing: 2px;}
            
        #trvRoot
        {
        border:0 solid red;overflow-x: scroll;word-break : keep-all; width:350px; height:100%;
        }
    </style>
</head>
<body scroll="yes">
    <form id="form1" runat="server">
    <table width="100%" background="/_layouts/CAResources/themeCA/images/homepage_bj.jpg">
        <tr>
            <td align="center">
                <!--top -->
                <table width="950" height="300" style="width: 950px; height: 300px; border: 0px solid red;"
                    cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="100%" align="center" valign="middle">
                            &nbsp;<img src="/_layouts/CAResources/themeCA/images/test.png" alt="" style="border: 0;" />
                        </td>
                    </tr>
                </table>
                <!--topend -->
            </td>
        </tr>
        <tr>
            <td>
                <!--center -->
                <table width="950" height="600" border="0" align="center" cellpadding="0" cellspacing="0"
                    bgcolor="#FFFFFF" style="padding: 10px; text-align: center;">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; padding-left: 10px;">
                            <div class="right_big">
                                <div class="right_small">
                                    <table width="95%" cellpadding="0" cellspacing="0">
                                        <tr valign="top">
                                            <td align="left" width="40%" style="padding-top: 20px;">
                                                <asp:TreeView ID="trvRoot" runat="server" NodeIndent="15"
                                                    AutoGenerateDataBindings="False" ExpandDepth="1"
                                                    PopulateNodesFromClient="true" EnableClientScript="true" OnTreeNodePopulate="PopulateNode">
                                                    <ParentNodeStyle Font-Bold="False" />
                                                    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                                    <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" 
                                                        HorizontalPadding="0px" VerticalPadding="0px" />
                                                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
                                                        HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                                                </asp:TreeView>
                                            </td>
                                            <td style="padding: 10px; border-left: 1px solid #ccc; width:500px;">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="Company_New">
                                                    <tr>
                                                        <td width="50%" valign="top" class="line" style="text-align: left;">
                                                            <span class="txt1">Marketing Communication News</span>
                                                        </td>
                                                        <td width="50%" valign="center" class="line">
                                                                <div class="viewbutton">
                                                                    <a href="/Lists/MarketingCommunicationNotes/AllItems.aspx" class="view">
                                                                        View More</a></div>
                                                        </td>
                                                    </tr>
                                                    <asp:Repeater ID="rptNotes" runat="server">
                                                        <ItemTemplate>
                                                            <tr >
                                                                <td class="h2" style="width:70%" align="left">
                                                                    <a href="javascript:" style="cursor:hand;word-wrap:break-all;" class="abcde"><%#DataBinder.Eval(Container.DataItem,"Title") %></a></td>
                                                                <td class="h2"  style="width:30%" align="right"><%#DataBinder.Eval(Container.DataItem,"Modified","{0:yyyy-MM-dd HH:mm}") %></td>
                                                            </tr>
                                                            <div id='strLayerTable<%#Eval("ID") %>'style="DISPLAY:none" class="showWindow1">
                                                                <div style="word-break:break-all;width:94%; text-align:center; font-weight:bold;float:left"><%#DataBinder.Eval(Container.DataItem,"Title") %></div><div><span id="spanClose" style="cursor:hand; width:5%; text-align:right; color:Red" onclick='doHideLetter("strLayerTable<%#Eval("ID") %>")'>[X]</span></div>
                                                                <br />
                                                                <div style="word-wrap:break-word;width:100%;text-indent:20px; height:240px;overflow-y:auto">
                                                                <%#DataBinder.Eval(Container.DataItem,"Content") %></div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <br />
                                                <asp:Panel runat="server" ID="PanelAdd">
                                                    <div style="text-align: right; width: 100%;">
                                                        <a href="/Lists/MarketingCommunicationNotes/NewForm.aspx" style="text-decoration:underline; font-size: 1.4em;">Add New Marketing Communication</a>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <!--end -->
            </td>
        </tr>
    </table>
     <div id="showWindow" style="DISPLAY:none; cursor:pointer">
	    </div>
    </form>
</body>
</html>
