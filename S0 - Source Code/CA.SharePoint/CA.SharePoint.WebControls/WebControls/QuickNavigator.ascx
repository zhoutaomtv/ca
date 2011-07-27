<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickNavigator.ascx.cs" Inherits="CA.SharePoint.WebControls.QuickNavigator" %>
<asp:Panel runat="server" ID="psetup">
    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0" class="QuickNavigation">
        <tr>
            <td class="top1">
                <div class="newsboldtxt">
                    <asp:Label runat="server" ID="nav0"></asp:Label></div>
                <div class="newsboldtxt">
                    <asp:Label runat="server" ID="nav1"></asp:Label></div>
                <div class="newsboldtxt">
                    <asp:Label runat="server" ID="nav2"></asp:Label></div>
                <div class="newsboldtxt">
                    <asp:Label runat="server" ID="nav3"></asp:Label></div>
                <div class="newsboldtxt">
                    <asp:Label runat="server" ID="nav4"></asp:Label></div>
                <div class="newsboldtxt02">
                    <a href="#" onclick="showsetup();">
                        <img src="/_layouts/CAResources/themeCA/images/cog_go.png" border="0" /></a>
                </div>
            </td>
        </tr>
    </table>
    <div id="divsetup" align="center" style="position: absolute; top: 30%; left: 30%; width: 483; height: 362; text-align: center;
        z-index: 10003; background: none; display: none">
        <table width="483" border="0" align="center" cellpadding="0" cellspacing="0" height="362" background="/_layouts/CAResources/themeCA/images/setup.jpg">
            <tr>
                <td width="483" valign="top" style="padding-top: 25px">
                    <table width="90%" border="0" align="center" cellpadding="0" cellspacing="12" class="Detai2">
                        <tr>
                            <td valign="middle" style="padding-top: 20px">
                                <table width="90%" border="0" align="center" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td width="10%" align="left">
                                            Name:
                                        </td>
                                        <td width="22%" align="left">
                                            <asp:TextBox runat="server" ID="txtTitle0" MaxLength="15" CssClass="input_ts"></asp:TextBox>
                                        </td>
                                        <td width="6%" align="left">
                                            Url:
                                        </td>
                                        <td width="62%" align="left">
                                            <asp:TextBox runat="server" ID="txtValue0" CssClass="input_ts2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="padding-top: 15px">
                                <table width="90%" border="0" align="center" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td width="10%" align="left">
                                            Name:
                                        </td>
                                        <td width="22%" align="left">
                                            <asp:TextBox runat="server" ID="txtTitle1" MaxLength="15" CssClass="input_ts"></asp:TextBox>
                                        </td>
                                        <td width="6%" align="left">
                                            Url:
                                        </td>
                                        <td width="62%" align="left">
                                            <asp:TextBox runat="server" ID="txtValue1" CssClass="input_ts2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="padding-top: 10px">
                                <table width="90%" border="0" align="center" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td width="10%" align="left">
                                            Name:
                                        </td>
                                        <td width="22%" align="left">
                                            <asp:TextBox runat="server" ID="txtTitle2" MaxLength="15" CssClass="input_ts"></asp:TextBox>
                                        </td>
                                        <td width="6%" align="left">
                                            Url:
                                        </td>
                                        <td width="62%" align="left">
                                            <asp:TextBox runat="server" ID="txtValue2" CssClass="input_ts2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="padding-top: 10px">
                                <table width="90%" border="0" align="center" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td width="10%" align="left">
                                            Name:
                                        </td>
                                        <td width="22%" align="left">
                                            <asp:TextBox runat="server" ID="txtTitle3" MaxLength="15" CssClass="input_ts"></asp:TextBox>
                                        </td>
                                        <td width="6%" align="left">
                                            Url:
                                        </td>
                                        <td width="62%" align="left">
                                            <asp:TextBox runat="server" ID="txtValue3" CssClass="input_ts2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="padding-top: 10px">
                                <table width="90%" border="0" align="center" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td width="10%" align="left">
                                            Name:
                                        </td>
                                        <td width="22%" align="left">
                                            <asp:TextBox runat="server" ID="txtTitle4" MaxLength="15" CssClass="input_ts"></asp:TextBox>
                                        </td>
                                        <td width="6%" align="left">
                                            Url:
                                        </td>
                                        <td width="62%" align="left">
                                            <asp:TextBox runat="server" ID="txtValue4" CssClass="input_ts2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="padding-top: 10px">
                                <asp:Button runat="server" ID="btnSubmit" Text="OK" CssClass="but_bg2" OnClientClick="return chkform();" />
                                <input type="button" class="but_bg2" value="Cancel" onclick="closesetup();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
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
    //divsetup
    function showsetup() {
        showbg();
        $('#divsetup').show('slow');
        // divsetup.style.display="block";
    }
    function closesetup() {
        hidebg();
        $('#divsetup').hide('fast');
        //divsetup.style.display="none";
    }

    function chkform() {

        var titles = $('.input_ts');
        for (var i = 0; i < titles.length; i++) {
            var len = 0;
            var arr = titles[i].value.split("");
            for (var j = 0; j < arr.length; j++) {
                if (arr[j].charCodeAt(0) <= 255) {
                    len += 1;
                } else {
                    len += 2;
                }
            }
            if (len > 16) {
                alert('Title can not exceed 16 characters.');
                return false;
            }
        }
        return true;

    }
</script>
