<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testExportExcel.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.ConstructionPurchasing.testExportExcel" %>

<%@ Register src="DataForm.ascx" tagname="DataForm" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Edit"/>
    
    </div>
    </form>
</body>
</html>
