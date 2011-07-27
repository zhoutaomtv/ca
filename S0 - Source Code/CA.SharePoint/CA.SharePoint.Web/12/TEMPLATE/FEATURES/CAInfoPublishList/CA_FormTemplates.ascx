<%@ Control Language="C#"   AutoEventWireup="false" %>
<%@Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.WebControls"%>
<%@Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.Utilities"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>

 

<SharePoint:RenderingTemplate ID="CA_DispForm" runat="server">
	<Template>
		<SPAN id='part1'>
			<SharePoint:InformationBar ID="InformationBar1" runat="server"/> 
			 
			<SharePoint:FormToolBar ID="FormToolBar1" runat="server"/> 
			<TABLE class="ms-formtable" style="margin-top: 8px;" border=0 cellpadding=0 cellspacing=0 width=100%>
			<SharePoint:ChangeContentType ID="ChangeContentType1" runat="server"/> 
			<SharePoint:FolderFormFields ID="FolderFormFields1" runat="server"/> 
			<SharePoint:ListFieldIterator ID="ListFieldIterator1" Visible="false" runat="server"/>
			 
			<div align="center">
			<SharePoint:FormField ID="FormField2" runat="server"  FieldName="Title" /></div>
			<hr style="height:1px; border:0;" />
			<SharePoint:FormField ID="FormField1" runat="server"  FieldName="Body" />
 
			<SharePoint:ApprovalStatus ID="ApprovalStatus1" runat="server"/> 
			<SharePoint:FormComponent ID="FormComponent1" TemplateName="AttachmentRows" runat="server" /> 
			</TABLE>
			<table cellpadding=0 cellspacing=0 width=100%><tr><td class="ms-formline"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td></tr></table>
			<TABLE cellpadding=0 cellspacing=0 width=100% style="padding-top: 7px"><tr><td width=100%>
			<SharePoint:ItemHiddenVersion ID="ItemHiddenVersion1" runat="server"/> 
			<SharePoint:ParentInformationField ID="ParentInformationField1" runat="server"/> 
			<SharePoint:InitContentType ID="InitContentType1" runat="server"/> 
			 
			</td></tr></TABLE>
			<div align="center" style="text-align:center">
			<hr/>
			<SharePoint:CreatedModifiedInfo ID="CreatedModifiedInfo1" runat="server"/>
			<SharePoint:GoBackButton ID="GoBackButton2" runat="server"/>
			</div>
		</SPAN>
		<SharePoint:AttachmentUpload ID="AttachmentUpload1" runat="server"/> 
	</Template>

</SharePoint:RenderingTemplate>

 