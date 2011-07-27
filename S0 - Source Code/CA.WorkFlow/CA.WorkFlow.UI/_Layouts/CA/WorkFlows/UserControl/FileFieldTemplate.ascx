<%@ Control Language="C#" AutoEventWireup="true" %>
<style>
    .fileUploadPanel
    {
        display: none;
        border-right: 1px dotted;
        border-top: 1px dotted;
        border-left: 1px dotted;
        border-bottom: 1px dotted;
        position: absolute;
        height: 266px;
        background-color: inactiveborder;
        left: 100px;
        width: 522px;
        top: 100px;
        text-align: center;
        z-index: 1000;
        display: none;
        position: absolute;
        left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft);
        top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);
        filter: progid:DXImageTransform.Microsoft.Glow(Color=#000000, Strength=3);
    }
    .fileUpload{width:300px;}
</style>
<table cellpadding="0" cellspacing="0" width="80%" style="border-right: activecaption 1px solid;
    border-top: activecaption 1px solid; border-left: activecaption 1px solid; border-bottom: activecaption 1px solid;"
    bgcolor="#F7F6F3">
    <tr>
        <td>
            <div id="FilePreview" runat="server">
            </div> 
        </td>
    </tr>
    <tr>
        <td>
            <asp:PlaceHolder runat="server" ID="EditPanel">            
            </asp:PlaceHolder> 
            
             <asp:HyperLink runat="server" ID="CreateLink" NavigateUrl="#">
            <img src="/_Layouts/Images/NEWDOC.GIF" border="0" />新建</a>
            </asp:HyperLink>         
            
            <asp:PlaceHolder runat="server" ID="AppendPanel">     
            <a href="javascript:" id="<%=this.ClientID%>_addFile"
                title="添加一个文件(若文件已存在,则会覆盖)" onclick="fileField_Open('<%=this.ClientID%>')" style="cursor: pointer">
                <img src="/_Layouts/images/ATTACH.GIF" border="0" alt="" />附加</a> 
             </asp:PlaceHolder>  
                           
             <asp:PlaceHolder runat="server" ID="DeletePanel">     
                <a href="javascript:"
                    id="<%=this.ClientID%>_delFile" title="删除" onclick="fileField_Remove('<%=this.ClientID%>')"
                    style="cursor: pointer">
                    <img src="/_Layouts/SmartForm/delItem.gif" border="0" alt="删除此文件" />删除</a>
            </asp:PlaceHolder>           
            
            <asp:HiddenField runat="server" ID="HiddenDocumentKey" />
              
        </td>
    </tr>
</table>
<div id='<%=this.ClientID%>_upDiv' style="" class="fileUploadPanel">
    <iframe style="position: absolute; z-index: -1; width: 100%; height: 100%; top: 0;
        left: 0; scrolling: no;" frameborder="0" src="about:blank"></iframe>
    <br />
    <br />
    <br />
    <br />
    选择要附加的文件：<br /> 
    <br />
    <div id="<%=this.ClientID%>_FileUploaderContainer">
        <asp:FileUpload runat="server" ID="FileUploader" CssClass="fileUpload" />
    </div>
    <br />
    <br />
    <input type="button" value="  确定  " onclick="fileField_OkClick('<%=this.ClientID%>')" />    
    &nbsp; &nbsp;
    <input type="button" value="  取消  " onclick="fileField_CancelClick('<%=this.ClientID%>')" />
</div>
<asp:HiddenField runat="server" ID="DeleteFlag" />
 

