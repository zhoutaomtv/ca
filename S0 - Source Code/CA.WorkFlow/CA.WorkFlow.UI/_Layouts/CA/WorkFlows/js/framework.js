$(document).ready(function() {
	$('ul#filter a').click(function() {
		$(this).css('outline','none');
		$('ul#filter .current').removeClass('current');
		$(this).parent().addClass('current');
		
		var filterVal = $(this).text().toLowerCase().replace(' ','-');
				
		if(filterVal == 'all') {
			$('ul#portfolio li.hidden').fadeIn('slow').removeClass('hidden');
		} else {
			
			$('ul#portfolio li').each(function() {
				if(!$(this).hasClass(filterVal)) {
					$(this).fadeOut('normal').addClass('hidden');
				} else {
					$(this).fadeIn('slow').removeClass('hidden');
				}
			});
		}
		
		return false;
	});
});
var __SubmitConfirmMessage="";
function CAShowWaitUI()
{    
    if (!Page_ClientValidate()) 
    {
 	__SubmitConfirmMessage="";  
	return false;
    }

    var Show=true; 
    
    if(__SubmitConfirmMessage!="")
    {             
        Show=confirm(__SubmitConfirmMessage);     
        __SubmitConfirmMessage="";  
    }
    else
    {
      return true;
    }
    
    if(!Show)
    {
        return false;
    }
    
    var divMask=document.createElement("DIV");
    divMask.id = "mask";      
    divMask.innerHTML=" <IFRAME style='background-color: #333;width:100%;height:100%; filter:alpha(opacity=0); scrolling:no; top:0; left:0;'> ";     
    document.body.appendChild(divMask);              

    var divPopup = document.createElement("DIV");
    divPopup.id = "popup";
    divPopup.innerHTML=" <img src='/_Layouts/CAResources/themeCA/images/Green_Big_Rotate.gif' />";             
    document.body.appendChild(divPopup);         

    divPopup.style.display = "block";
    divPopup.style.left = window.screen.availWidth/2 -100 +"px";
    divPopup.style.top = window.screen.availHeight/2 + window.document.body.scrollTop - 100 +"px";

    divMask.style.display = "block";            
    divMask.style.width = document.body.clientWidth+"px";              
  
   if(document.body.scrollHeight<window.document.body.clientHeight)
    {
        divMask.style.height = document.documentElement.clientHeight+"px";
    }
    else
    {
         divMask.style.height = document.body.scrollHeight +"px";     
    }           
  
    return true;
}

function HideWaitUI()
{
    var divMask=document.getElementById('mask');
    var divPopup=document.getElementById('popup');
    
    if(divMask!=null)
    {
        divMask.style.display="none";
        
    }
    if(divPopup!=null)
    {
        divPopup.style.display="none";
    }
    
}

function SmartForm_GetValue( obj )
{
    return SmartForm_Trim( obj.value ) ;
}

function SmartForm_Trim(s) {
    var m = s.match(/^\s*(\S+(\s+\S+)*)\s*$/);
    return (m == null) ? "" : m[1];
}

var SmartForm_MessagePanels = new Array();

function SmartForm_ShowMessage( ctl , msg )
{
    var id = ctl.id + "_messagepanel";  
    
    var msgPanel = SmartForm_GetMessagePanel(ctl);
    msgPanel.Show( ctl , msg ) ; 
}

function SmartForm_ShowMessageById( ctl , msg , msgId )
{
    var id = msgId + "_messagepanel";  
    
    var msgPanel = SmartForm_GetMessagePanelById(msgId);
    msgPanel.Show( ctl , msg ) ; 
}

function SmartForm_GetMessagePanelById( ctlId )
{
    var id = ctlId + "_messagepanel";
    
    for( var i = 0 ;i < SmartForm_MessagePanels.length ; i ++ )
    {
        if( SmartForm_MessagePanels[i].ID == id )
            return SmartForm_MessagePanels[i];
    }
    
    var newPanel = new SmartForm_MessagePanel( id );
    SmartForm_MessagePanels.push( newPanel ) ;
    return newPanel;
}

function SmartForm_GetMessagePanel( ctl )
{
    var id = ctl.id + "_messagepanel";
    
    for( var i = 0 ;i < SmartForm_MessagePanels.length ; i ++ )
    {
        if( SmartForm_MessagePanels[i].ID == id )
            return SmartForm_MessagePanels[i];
    }
    
    var newPanel = new SmartForm_MessagePanel( id );
    SmartForm_MessagePanels.push( newPanel ) ;
    return newPanel;
}

function SmartForm_HiddenMessagePanel( ctl )
{
    var id = ctl.id + "_messagepanel";
    
    for( var i = 0 ;i < SmartForm_MessagePanels.length ; i ++ )
    {
        if( SmartForm_MessagePanels[i].ID == id ){
            SmartForm_MessagePanels[i].Hidden();
            return;
         }
    } 
}

function SmartForm_HiddenMessagePanelByCtlId( id )
{
    var id = id + "_messagepanel";
    
    for( var i = 0 ;i < SmartForm_MessagePanels.length ; i ++ )
    {
        if( SmartForm_MessagePanels[i].ID == id ){
            SmartForm_MessagePanels[i].Hidden();
            return;
         }
    } 
}

function SmartForm_IsVisible(ctl)
{
    return ctl.offsetWidth + ctl.offsetHeight > 0 ;   
}

var SmartForm_IsValid = true ;
var SmartForm_FocusCtl = null ;

function SmartForm_SetFirstFocus(ctl)
{
 if(SmartForm_FocusCtl==null || SmartForm_FocusCtl == ctl )
 {
	 try
	 {
		 ctl.focus();
		 SmartForm_FocusCtl = ctl;
	  }
	  catch(e){}
 }
}

function SmartForm_ClearFocus(ctl)
{
  if(ctl==SmartForm_FocusCtl)	
      SmartForm_FocusCtl = null;
}

function SmartForm_Valid(obj) {      
   
   var valueGet = false ;     
   var v ;   
   
   var ctl = document.getElementById(obj.controltovalidate);   
   
   if( ctl == null || !SmartForm_IsVisible(ctl) ){        
       SmartForm_HiddenMessagePanelByCtlId(obj.controltovalidate);
       SmartForm_ClearFocus(ctl);
       return true;
   }
  
   try{
        v = SmartForm_GetValue( ctl ) ;
        valueGet = true ;
   }catch(e){ valueGet = false ; }
   
   if( valueGet )
   {
        //非空验证
       if( obj.required == "True" && v == "" )
       {
            SmartForm_ShowMessage( ctl , obj.errorMessage );               
            
            try{
              //if( SmartForm_IsValid )              
              // ctl.focus(); //第一个未通过验证的控件聚焦 
               SmartForm_SetFirstFocus(ctl);
               
           }catch(e){}
                    
            SmartForm_IsValid = false ;
            
            return false ;
       }
       //alert(obj.maxLength);
       //长度验证
       if( obj.maxLength != null && obj.maxLength != "" && obj.maxLength != "0" )
       { 
            var maxLength = parseInt(obj.maxLength);
            if( v.length > maxLength )
            {
                SmartForm_ShowMessage( ctl , obj.errorMaxLengthMessage);
                //param.MessagePanel.Show( param.Control , param.ErrorMaxLengthMessage ) ;
                
                if( SmartForm_IsValid ) ctl.focus();                   
                SmartForm_IsValid = false ;
                
                return false ;              
            }
       }
  
        //类型验证
        if( obj.validationexpression != null && obj.validationexpression != "" && v != "" )
       {               
           // var raRegExp = new RegExp(obj.validationexpression );         
            
            if( !RegularExpressionValidatorEvaluateIsValid(obj) )
            {
                SmartForm_ShowMessage( ctl , obj.errorDataTypeMessage);                         
                if( SmartForm_IsValid ) ctl.focus(); //第一个未通过验证的控件聚焦           
                SmartForm_IsValid = false ;                        
                return false ;
            }               
       }       
   }           
   
    //自定义函数验证
   if( obj.customfunction != "" && obj.customfunction != "SmartForm_Valid" )
   {     
         var r = true ;                    
         r = eval(  obj.customfunction + "(obj);"  );
          return r ;
   }           

    SmartForm_HiddenMessagePanel(ctl);
    SmartForm_ClearFocus(ctl);
    return true  ;   
}