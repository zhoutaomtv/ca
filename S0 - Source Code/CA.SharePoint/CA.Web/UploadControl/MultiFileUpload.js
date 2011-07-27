//author: jianyi0115@163.com
function MultiUploader_AddAttach( objId , uId , size , css  , maxCount , img ){
	
	
	//return ;
	
	var listObj = document.getElementById( objId + '_Ids');
	
	obj =  document.getElementById(  objId );
	
	arr = listObj.value.split( "," );
	
	if( maxCount > 0 ){
		if( arr.length > maxCount ){
			alert( "最多允许上传" + maxCount + "个附件！" );
			return ;
		}
	}
	
	//alert( uId );
	
	lastId = arr[ arr.length - 1 ];
	thisId = parseInt( lastId ) + 1 ;
	
	var div_obj = document.createElement('div');
	div_obj.id = objId +"_div_" + thisId ;
	div_obj.innerHTML = "附件名称：<input type=file name='" + uId  + "$_file_" + thisId + "' size="+size+" class="+css+" onchange='validate(this)'>";
	//div_obj.innerHTML += "<input type=button onclick=\"MultiUploader_Remove( '"+objId+"', '"+thisId+"' )\" value='删 除'>";
	
	div_obj.innerHTML += "<br>附件标题：<input name='" + objId + "_name_" + thisId + "' type=text size="+size+" class="+css+" >";
	div_obj.innerHTML += "<img border=0 style='cursor:hand' src='"+img+"' onclick=\"MultiUploader_Remove( '"+objId+"', '"+thisId+"' )\">";
	
	//alert( obj.innerHTML );
	obj.appendChild (div_obj);
	
	listObj.value += "," + thisId ;
}

function MultiUploader_Remove( objId ,id  ){
	var div_obj = document.getElementById( objId + '_div_' + id);
	//alert( div_obj ) ;
	div_obj.innerHTML = "" ;
	
	div_obj.removeNode( false );
	
	var listObj = document.getElementById( objId + '_Ids');
	
	arr = listObj.value.split( "," );
	list = "";
	
	for( i = 0 ; i < arr.length ; i ++ ){
		if( arr[i] == id ) continue ;
		
		if( list != "" ) list += ",";
		list += arr[i] ;
	}
	listObj.value = list  ;
}

function validate( obj ){

}



