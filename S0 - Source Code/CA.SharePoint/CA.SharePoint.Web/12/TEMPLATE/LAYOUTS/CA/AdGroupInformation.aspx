<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdGroupInformation.aspx.cs" Inherits="CA.SharePoint.Web.AdGroupInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script language="javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.3.2.min.js"></script>
    <style type="text/css">
        body {
            font-family:Arial, Helvetica, sans-serif;
            font-size:13px;
        }
        table
        {
            width:800px;  
        }
        .s1
        {
            padding-left:10px;
            padding-right:10px;
        }
        .tr1
        {
	background-color:#eaf3ff;
	font-weight: bold;
        }
        .tr1:hover
        {
            cursor:hand;
        }
        .tr2
        {
	background-color:#f5f8fc;
	color: #666666;
	font-size: 11px;
        }
        .td2
        {
            padding-left:20px;
        }
        .bf
        {
            font-weight:bold; 
            font-size:17px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style="padding-bottom:20px; font-weight:bold; font-size:28px; text-align:center;">C&A China User Group Information</div>
    <asp:Label runat="server" ID="lblop"></asp:Label>
    </div>
    </form>
    <script type="text/javascript">
        $().ready(function() {
            $('.tr2').hide();
        })

        $('.tr1').click(function() {
            $('.tr2').hide();
            $(this).next().toggle();
        });

    </script>
</body>
</html>
