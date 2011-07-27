var __SubmitConfirmMessage="";
_spBodyOnLoadFunctionNames.push("customOnLoad") 
function customOnLoad()
{
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
}

function CAShowWaitUI()
{    
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