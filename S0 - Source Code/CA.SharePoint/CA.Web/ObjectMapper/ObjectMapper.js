//ObjectMapper js 
//Author : jianyi0115@163.com

function ObjectMapper_Valid(val) {
      
   if( typeof(ObjectMapper_Validators) == "undefined" || ObjectMapper_Validators.length == 0 ) return true ;
   
   var isValid = true ;
   var valueGet = false ;
   
   for( i = 0 ; i < ObjectMapper_Validators.length ; i ++ )
   {
       param = ObjectMapper_Validators[i];
       
       if( param.Control == null ) continue ;
       
       param.MessagePanel.Hidden() ;
       
       valueGet = true ;
       
       try{
            v = ObjectMapper_GetValue( param.Control ) ;
       }catch(e){ valueGet = false ; }
       
       //非空验证
       if( valueGet && param.AutoValid && param.Required && v == "" )
       {
            //alert( ObjectMapper_ErrMessage[i] );
            param.MessagePanel.Show( param.Control , param.ErrorMessage ) ;
            
            if( isValid ) param.Control.focus(); //第一个未通过验证的控件聚焦
           
            isValid = false ;
            
            continue ; //若非空验证未通过则，当前控件不进行其他验证
       }
       
       //长度验证
       if( valueGet && param.AutoValid && param.MaxLength > 0 )
       { 
            if( v.length > param.MaxLength )
            {
                param.MessagePanel.Show( param.Control , param.ErrorMaxLengthMessage ) ;
                
                if( isValid ) param.Control.focus();
               
                isValid = false ;
                
                continue ;              
            }
       }
       
       //类型验证
        if( valueGet && param.AutoValid && param.ValidationExpression != "" && v != "" )
       {
           if( typeof( param.ValidationExpression ) == "object" )
           {
                if( v.match( param.ValidationExpression ) == null )
                {
                    param.MessagePanel.Show( param.Control, param.ErrorTypeMessage ) ;
                    
                     if( isValid ) param.Control.focus();
                   
                    isValid = false ;
                    
                    continue ;              
                }
           }
       }
       
        //自定义函数验证
       if( param.ClientValidationFunction != "" )
       {
           var r = true ;
           
           if( param.ClientValidationFunction.indexOf(')') == -1 )
                r = eval(  param.ClientValidationFunction + "(param)"  );
           else
                 r = eval(  param.ClientValidationFunction );    
           
           if( !r ){ isValid = false ; continue ; }
       }
       
   }
   
   return isValid  ;   
}

function ObjectMapper_GetValue( obj )
{
    return ObjectMapper_Trim( obj.value ) ;
}


//延迟消息框对象创建, 动态创建table只能在页面加载完成后才可以
function ObjectMapper_MessagePanelWrapper( id )
{
    this._realPanel = null ;
    
    this.Show = function( ctl , msg ){
       if( this._realPanel == null )       
            this._realPanel = new ObjectMapper_MessagePanel( id + "_msg" );
    
        this._realPanel.Show( ctl , msg ) ;               
    }
      
    this.Hidden = function( ){      
            
         if( this._realPanel == null )       
            this._realPanel = new ObjectMapper_MessagePanel( param.Control.id + "_msg" );
    
        this._realPanel.Hidden( ) ;       
    }              
    
}


function ObjectMapper_Trim(s) {
    var m = s.match(/^\s*(\S+(\s+\S+)*)\s*$/);
    return (m == null) ? "" : m[1];
}


//消息框对象
function ObjectMapper_MessagePanel( id )
{

   // this._warningIconImageUrl = "../Images/ObjectMapper/alert-large.gif";
   // this._closeImageUrl =  "../Images/ObjectMapper/close.gif";
                
        var iframe = this._iframe= document.createElement("<iframe src=\"#\" scrolling='no' frameborder='0'  style='position:absolute;width:200px;height:100px;top:-100px;left:-100px;border:none;display:inline;z-index:0'></iframe>") ;


        var popupTableBody = document.createElement("tbody");
        var popupTableRow = document.createElement("tr");
        var calloutCell = document.createElement("td");
        var calloutTable = document.createElement("table");
        var calloutTableBody = document.createElement("tbody");
        var calloutTableRow = document.createElement("tr");
        var iconCell = document.createElement("td");
        var closeCell = document.createElement("td");
        
        var popupTable = this._popupTable = document.createElement("table");

        popupTable.id = id ;              
        
        var calloutArrowCell = this._calloutArrowCell = document.createElement("td");
        var warningIconImage = this._warningIconImage = document.createElement("img");
        var closeImage = this._closeImage = document.createElement("img");
        var errorMessageCell = this._errorMessageCell = document.createElement("td");
	    // 
	    // popupTable
	    //
        popupTable.cellPadding = 0;
        popupTable.cellSpacing = 0;
        popupTable.border = 0;
        popupTable.style.position = "absolute";
        popupTable.width = 130;
        popupTable.style.display = 'none';
            
        //
        // popupTableRow
        //        
        popupTableRow.vAlign = 'top';
        popupTableRow.style.height = "100%";
        //
        // calloutCell
        //
        calloutCell.width = 20;
        calloutCell.align = "right";
        calloutCell.style.height = "100%";
        calloutCell.style.verticalAlign = "top";
        //
        // calloutTable
        //
        calloutTable.cellPadding = 0;
        calloutTable.cellSpacing = 0;
        calloutTable.border = 0;
        calloutTable.style.height = "100%";
        //
        // _calloutArrowCell
        //
        calloutArrowCell.align = "right";
        calloutArrowCell.vAlign = "top";
        calloutArrowCell.style.fontSize = "1px";
        calloutArrowCell.style.paddingTop = "8px";
        //
        // iconCell
        //
        iconCell.width = 20;
        iconCell.style.borderTop = "1px solid black";
        iconCell.style.borderLeft = "1px solid black";
        iconCell.style.borderBottom = "1px solid black";
        iconCell.style.padding = "5px";
        iconCell.style.backgroundColor = 'LemonChiffon';
        //
        // _warningIconImage
        //
        warningIconImage.border = 0;
        warningIconImage.src = ObjectMapper_MessagePanelWarningImageUrl ;
        //
        // _errorMessageCell
        //
        errorMessageCell.style.backgroundColor = 'LemonChiffon';
        //errorMessageCell.style.fontFamily = 'verdana';
        //errorMessageCell.style.fontSize = '10px';
        errorMessageCell.style.padding = "5px";
        errorMessageCell.style.borderTop = "1px solid black";
        errorMessageCell.style.borderBottom = "1px solid black";
        errorMessageCell.width = '100%';
        errorMessageCell.className = ObjectMapper_MessagePanelCssClass ;
        //errorMessageCell.innerHTML = "XXX" ;
        
        this._errorMessageCell = errorMessageCell ;
        
        //
        // closeCell
        //
        closeCell.style.borderTop = "1px solid black";
        closeCell.style.borderRight = "1px solid black";
        closeCell.style.borderBottom = "1px solid black";
        closeCell.style.backgroundColor = 'lemonchiffon';
        closeCell.style.verticalAlign = 'top';
        closeCell.style.textAlign = 'right';
        closeCell.style.padding = '2px';
        //
        // closeImage
        //
        closeImage.src =  ObjectMapper_MessagePanelCloseImageUrl ;
        closeImage.style.cursor = 'pointer';
        
        var container =this ;
        
        closeImage.attachEvent("onclick",function(e){container.Hidden();}) ;

//        closeImage.onclick = function(){        
//          table = document.getElementById( id );
//          table.style.display = "none" ;  
//          
//        } ;
        //
        // Create the DOM tree
        //
        document.body.appendChild(iframe);//
        document.body.appendChild(popupTable)
        popupTable.appendChild(popupTableBody);
        popupTableBody.appendChild(popupTableRow);
        popupTableRow.appendChild(calloutCell);
        calloutCell.appendChild(calloutTable);
        calloutTable.appendChild(calloutTableBody);
        calloutTableBody.appendChild(calloutTableRow);
        calloutTableRow.appendChild(calloutArrowCell);
        popupTableRow.appendChild(iconCell);
        iconCell.appendChild(warningIconImage);
        popupTableRow.appendChild(errorMessageCell);
        popupTableRow.appendChild(closeCell);
        closeCell.appendChild(closeImage);
        //
        // initialize callout arrow
        //
        var div = document.createElement("div");
        div.style.fontSize = "1px";
        div.style.position = "relative";
        div.style.left = "1px";
        div.style.borderTop = "1px solid black";
        div.style.width = "15px";
        calloutArrowCell.appendChild(div);        
        for(var i = 14; i > 0; i--)
        {
            var line = document.createElement("div");
            line.style.width = i.toString() + "px";
            line.style.height = "1px";
            line.style.overflow = "hidden";
            line.style.backgroundColor = "LemonChiffon";
            line.style.borderLeft = "1px solid black";
            div.appendChild(line);
        }
}
ObjectMapper_MessagePanel.prototype.Hidden = function()
{
            this._popupTable.style.display = "none" ;    
            this._iframe. style.top="-100";
            this._iframe.style.left ="-100";      
}
ObjectMapper_MessagePanel.prototype.Show =  function( obj , msg ){
         //显示消息框 ; obj 基准对象（消息框在其旁边显示）      
       this._errorMessageCell.innerHTML = msg ;       
            var t = obj.offsetTop,  h = obj.clientHeight, l = obj.offsetLeft;    	
	        var w= obj.clientWidth;    	
	        
	        while (obj = obj.offsetParent){t += obj.offsetTop; l += obj.offsetLeft;}
	        
            this._popupTable.style.display = "block" ;
    	
	        this._popupTable.style.top = t;
	        this._popupTable.style.left =l+w; 	//(p=="image")?l+w+25: 
	        //this._popupTable.focus();	        	        
	       //
	         this._iframe.style.top =t;
             this._iframe.style.left =l+w+15; 
             this._iframe.style.width = this._popupTable.clientWidth-15 ;
             this._iframe.style.height = this._popupTable.clientHeight ;
            //	        
	        }
