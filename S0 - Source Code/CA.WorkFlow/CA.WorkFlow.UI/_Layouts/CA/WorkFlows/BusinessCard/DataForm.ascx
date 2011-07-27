<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlows.BusinessCard.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="CAControls" %>
<link rel="stylesheet" type="text/CSS" href="../css/style_1.css" title="C85M30Y0K0" id="C85M30Y0K0" />
<link rel="stylesheet" type="text/CSS" href="../css/style_2.css" title="C0M55Y100K0" id="C0M55Y100K0"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_3.css" title="C43M100Y0K0" id="C43M100Y0K0"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_4.css" title="C30M0Y100K10" id="C30M0Y100K10"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_5.css" title="C0M100Y48K0" id="C0M100Y48K0"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_6.css" title="ff" id="ff"/>
<%--<link rel="stylesheet" type="text/CSS" href="../css/style_1.css" title="aa" id="aa" />
<link rel="stylesheet" type="text/CSS" href="../css/style_2.css" title="bb" id="bb"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_3.css" title="cc" id="cc"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_4.css" title="dd" id="dd"/>
<link rel="stylesheet" type="text/CSS" href="../css/style_5.css" title="ee" id="ee"/>--%>
<script type="text/javascript">
    //setActiveStyleSheet("C0M55Y100K0");
    var flag = true;
    function setActiveStyleSheet(title) {
        if (!flag) return;
        var i, a, main;
        for (i = 0; (a = document.getElementsByTagName("link")[i]); i++) {
            if (a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title")) {
                a.disabled = true;
                if (a.getAttribute("title") == title) {
                    a.disabled = false;
                }
            }
        }
        document.getElementById("<%= hdColorCard.ClientID %>").value = title;
        //alert(title);
    }
    function UpdateFlag() {
        flag = false;
    }
    function getActiveStyleSheet() {
        var i, a;
        for (i = 0; (a = document.getElementsByTagName("link")[i]); i++) {
            if (a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title") && !a.disabled) return a.getAttribute("title");
        }
        return null;
    }

    function getPreferredStyleSheet() {
        var i, a;
        for (i = 0; (a = document.getElementsByTagName("link")[i]); i++) {
            if (a.getAttribute("rel").indexOf("style") != -1
       && a.getAttribute("rel").indexOf("alt") == -1
       && a.getAttribute("title")
       ) return a.getAttribute("title");
        }
        return null;
    }

    function createCookie(name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else expires = "";
        documents.cookie = name + "=" + value + expires + "; path=/";
    }
    function Trim(str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    } 
    function CheckValue(str) {
        if (str == "End")
            return confirm("Are you sure you want to end this application?");
        if (Trim(document.getElementById("<%= txtUserName.ClientID%>").value) == "") {
            alert("Please supply a chinese name!");
            document.getElementById("<%= txtUserName.ClientID%>").focus();
            return false;
        }
        if (Trim(document.getElementById("<%= txtDeptName.ClientID%>").value) == "") {
            alert("Please supply a chinese department name!");
            document.getElementById("<%= txtDeptName.ClientID%>").focus();
            return false;
        }
        if (Trim(document.getElementById("<%= txtJobTitle.ClientID%>").value) == "") {
            alert("Please supply a chinese jobTitle!");
            document.getElementById("<%= txtJobTitle.ClientID%>").focus();
            return false;
        }
        if (Trim(document.getElementById("<%= txtMobilePhone.ClientID%>").value) == "") {
            alert("Please supply a mobilePhone!");
            document.getElementById("<%= txtMobilePhone.ClientID%>").focus();
            return false;
        }
       
    }
</script>

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3" style="display:none">
    <tr>
        <th width="10%" id="thDisplay" runat="server">
            ID: 
        </th>
        <td  width="20%" id="trDisplay" runat="server">
            <asp:Label ID="lblID" runat="server"></asp:Label>
        </td>
        <th width="10%">
            用户名:
        </th>
        <td width="20%">
            <asp:Label ID="lblLogUser" runat="server"></asp:Label>
        </td>
        <th width="10%">
            申请时间：
        </th>
        <td width="20%"><asp:Label ID="lblApplyDate" runat="server"></asp:Label></td>
    </tr>
</table>
<table width="100%" border="0" id="tbldispaly" runat="server" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="20%">
            Choose Employee 选择员工：
        </th>
        <td>
            <CAControls:CAPeopleFinder ID="CAPeopleFinder" runat="server" AllowTypeIn="true"
                MultiSelect="false" AutoPostBack="true" />
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="20%">&nbsp;
            
        </th>
        <th width="20%" class="th2">
            <p>
                Name</p>
            <p>
                姓名</p>
        </th>
        <th width="30%" class="th2">
            <p>
                Company</p>
            <p>
                公司</p>
        </th>
        <th width="20%" class="th2">
            <p>
                Department</p>
            <p>
                部门</p>
        </th>
    </tr>
    <tr>
        <th width="20%">
            <p>
                Chinese</p>
            <p>
                中文<span class="red">*</span></p>
        </th>
        <td>
            <asp:TextBox ID="txtUserName" runat="server" Width="95%" CssClass="input_ts4"></asp:TextBox>
        </td>
        <td>
            西雅衣家(中国)商业有限公司
        </td>
        <td>
            <asp:TextBox ID="txtDeptName" runat="server" Width="95%" CssClass="input_ts4"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th width="20%">
            <p>
                English</p>
            <p>
                英文</p>
        </th>
        <td>
            <asp:TextBox  Width="95%" CssClass="input_ts4" ID="txtEnglishName" runat="server"></asp:TextBox>
        </td>
        <td>
            C&amp;A (China) Co., Ltd.</td>
        <td>
            <asp:TextBox ID="txtEnglishDept" runat="server" Width="95%" CssClass="input_ts4" ></asp:TextBox>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="20%">&nbsp;
            
        </th>
        <th width="20%" class="th2">
            <p>
                Job Title</p>
            <p>
                职位</p>
        </th>
        <th width="40%" class="th2">
            <p>
                Address</p>
            <p>
                地址</p>
        </th>
    </tr>
    <tr>
        <th>
            <p>
                Chinese</p>
            <p>
                中文<span class="red">*</span></p>
        </th>
        <td>
            <asp:TextBox ID="txtJobTitle" runat="server" Width="95%" CssClass="input_ts4"></asp:TextBox>
        </td>
        <td>
            <asp:Label ID="lblAddrChi" runat="server" Text="上海市延安西路2299号,上海世贸商城3A88室"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>
            <p>
                English</p>
            <p>
                英文</p>
        </th>
        <td>
            <asp:TextBox ID="txtEnglishJobTitle" runat="server" Width="95%" CssClass="input_ts4" ></asp:TextBox>
        </td>
        <td>
            <asp:Label ID="lblAddr" runat="server" Text="3A88 Shanghai Mart,2299 Yan'an Road West"></asp:Label>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
    <tr>
        <th width="20%" class="th2">
            <p>
                Telephone</p>
            <p>
                电话</p>
        </th>
        <th width="20%" class="th2">
            <p>
                Mobile Phone</p>
            <p>
                手机<span class="red">*</span></p>
        </th>
        <th width="20%" class="th2">
            <p>
                Fax</p>
            <p>
                传真</p>
        </th>
        <th width="20%" class="th2">
            <p>
                E-Mail</p>
            <p>
                电子邮箱</p>
        </th>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtTelehpone" runat="server"  Width="95%" CssClass="input_ts4"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="input_ts4" Width="95%"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtFax" runat="server"  Width="95%" CssClass="input_ts4"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server"  Width="95%" CssClass="input_ts4"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th colspan="2" class="th2">
            <p>
                Reason for Application</p>
            <p>
                申请理由</p>
        </th>
        <th colspan="2" class="th2">
            <p>
                Color Selection</p>
            <p>
                颜色选择</p>
        </th>
    </tr>
    <tr>
        <td colspan="2">
            <asp:TextBox ID="txtReasonForApplication" runat="server" Rows="3" TextMode="MultiLine"
                CssClass="input_ts4" Width="95%"></asp:TextBox>
        </td>
        <td colspan="2" align="center">
            <table width="90%" height="25" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td runat="server" id="tdColor" align="center">
                        <a onclick="setActiveStyleSheet('C85M30Y0K0'); return false;"><img src="../images/color1.gif" style="cursor:hand" width="10" height="10" border="0"></a> 
                        <a onclick="setActiveStyleSheet('C0M55Y100K0'); return false;"><img src="../images/color2.gif" style="cursor:hand" width="10" height="10" border="0"></a> 
                        <a onclick="setActiveStyleSheet('C43M100Y0K0'); return false;"><img src="../images/color3.gif" style="cursor:hand" width="10" height="10" border="0"></a>
                        <a onclick="setActiveStyleSheet('C30M0Y100K10'); return false;"><img src="../images/color4.gif" style="cursor:hand" width="10" height="10" border="0"></a> 
                        <a onclick="setActiveStyleSheet('C0M100Y48K0'); return false;"><img src="../images/color5.gif" style="cursor:hand" width="10" height="10" border="0"></a>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table border="0" cellspacing="0" cellpadding="0" class="template" align="center">
    <tr>
        <td height="208" valign="top">
            <p>
               </p>
        </td>
    </tr>
</table>
<input type="hidden" runat="server" id="hdColorCard" />
<input type="hidden" runat="server" id="hdApplyUser" />
<input type="hidden" runat="server" id="hdSearchUser" />