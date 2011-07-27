<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" 
 Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" Title="" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="Welcome" src="~/_controltemplates/Welcome.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="DesignModeConsole" src="~/_controltemplates/DesignModeConsole.ascx" %>
  
  <asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:ListFormPageTitle ID="ListFormPageTitle1" runat="server"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:ListProperty Property="LinkTitle" runat="server" id="ID_LinkTitle"/>: <SharePoint:ListItemProperty id="ID_ItemProperty" MaxLength=40 runat="server"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageImage" runat="server">
	<IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt="">
</asp:Content>
 
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">

<table cellpadding=0 cellspacing=0 id="onetIDListForm">
 <tr>
  <td>
 <WebPartPages:WebPartZone runat="server" FrameType="None" ID="Main" Title="loc:Main"  Visible="false" />
 <IMG SRC="/_layouts/images/blank.gif" width=590 height=1 alt="">
  </td>
 </tr>
</table>
<!-- begin -->


 <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr> 
          <td class="FormToolBar">
		<SharePoint:InformationBar ID="InformationBar1" runat="server"/> 
		<SharePoint:FormToolBar ControlMode="Display" ID="FormToolBar1" runat="server" Visible="false"/>
          </td>
        </tr>
      </table>

 
		<table width="95%" border="0" cellspacing="8" cellpadding="0">
		 <tr> 
             <td align="center" height="30px" class="title">
			<b><SharePoint:FormField ControlMode="Display" ID="FormField2" runat="server"  FieldName="Title" /></b></td>
                    </tr>
                    
                   
                      <tr> 
                      <td align="right">
			<SharePoint:CreatedModifiedInfo ControlMode="Display" ID="CreatedModifiedInfo1" runat="server"/></td>
                    </tr>
                     <tr> 
                      <td height="1" align="center" class="seprow"></td>
                    </tr>
                    <tr> 
                      <td valign="top" height="300px" class="content">
			<SharePoint:FormField ControlMode="Display" ID="FormField1" runat="server"  FieldName="Body" /></td>
                    </tr>
		    <tr> 
                      <td>
			<table border="0" cellspacing="0" cellpadding="0">
			<tr>
				<td>&nbsp;&nbsp;</td>
				<td>
				<table border="0" cellspacing="0" cellpadding="0">
					<SharePoint:FormComponent ControlMode="Display" ID="FormComponent1" TemplateName="AttachmentRows" runat="server" />
					
				</table>
				<SharePoint:AttachmentUpload  Visible="false" ID="AttachmentUpload1" runat="server"/>
				</td>
			</tr>
			</table>
		      </td>
                    </tr>
		   
 		 
		    <tr>
		 	<td height="30px"></td>
                    </tr>
                    <tr> 
                      <td align="center">
                      
                      <table>
                      <tr>
                      <td>
                      
                      <SharePoint:GoBackButton ControlMode="Display" ID="GoBackButton2" Visible="false" runat="server" />
                      <input type="button" value='<asp:Literal ID="Literal1" runat="server" Text="<%$Resources:ca,BtnBack%>" />'  onclick="javascript:IsCloseWindow();return false;"/>
                      <!--img src="/_wpresources/DisForm/b_close.gif" onclick="javascript:IsCloseWindow();return false;" width="122" height="25"-->
                      </td>
                      </tr>
                      </table>
                      </td>
                    </tr>
                    <tr> 
                      <td align="center">&nbsp;</td>
                    </tr>
                  </table>
		 

<script language="javascript">
function IsCloseWindow()
{
	var url = window.location.href;
	if(url.indexOf("?")==-1) 
	{
		self.close();
	}
	else
	{
		history.back(-1);
	}
}
</script>



<!-- end -->
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderTitleLeftBorder" runat="server">
<table cellpadding=0 height=100% width=100% cellspacing=0>
 <tr><td class="ms-areaseparatorleft"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td></tr>
</table>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
<script id="onetidPageTitleAreaFrameScript">
	document.getElementById("onetidPageTitleAreaFrame").className="ms-areaseparator";
</script>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">
<style type="text/css">
.ms-bodyareaframe {
	padding: 8px;
	border: none;
}
</style>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderId="PlaceHolderBodyLeftBorder" runat="server">
<div class='ms-areaseparatorleft'><IMG SRC="/_layouts/images/blank.gif" width=8 height=100% alt=""></div>
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderId="PlaceHolderTitleRightMargin" runat="server">
<div class='ms-areaseparatorright'><IMG SRC="/_layouts/images/blank.gif" width=8 height=100% alt=""></div>
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
<div class='ms-areaseparatorright'><IMG SRC="/_layouts/images/blank.gif" width=8 height=100% alt=""></div>
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderId="PlaceHolderTitleAreaSeparator" runat="server"/>