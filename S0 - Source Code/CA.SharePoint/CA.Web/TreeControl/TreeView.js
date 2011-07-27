///TreeView 
//author: jianyi0115@163.com
		/*var TreeView_EnableAsyncLoad = true ;
		
		var TreeView_LineNodeClickImgUrl = "images/tree/minus.gif"; //折叠加号图片
		var TreeView_LineNodeClickExpandImgUrl = "images/tree/plus.gif";　//展开加号图片
		
		var TreeView_LastNodeClickImgUrl = "images/tree/endminus.gif"; //折叠加号图片
		var TreeView_LastNodeClickExpandImgUrl = "images/tree/endplus.gif";　//展开加号图片*/
		
		//节点展开函数
		function TreeView_NodeClick( treeId , obj , nodeValue ){
			
			var tr = obj.parentElement.parentElement ;
			var table = tr.parentElement.parentElement;

			if(table.rows.length-2<tr.rowIndex ) return ;
			
			var nextRow =  table.rows( tr.rowIndex + 1) ;
			if( nextRow.style.display == "" ){ 
				nextRow.style.display = "none";
				obj.src = TreeView_LineNodeClickExpandImgUrl ;
			}
			else{
				nextRow.style.display = "";
				obj.src = TreeView_LineNodeClickImgUrl;
			}
			
			if( TreeView_EnableAsyncLoad && nextRow.cells[1].innerHTML == ""  )
				TreeView_AsyncLoad( treeId , obj , nodeValue , tr.cells[1].innerText ) ;		
		}
		
		//最后节点展开函数
		function TreeView_LastNodeClick( treeId , obj , nodeValue ){
			
			var tr = obj.parentElement.parentElement ;
			var table = tr.parentElement.parentElement;

			if(table.rows.length-2<tr.rowIndex ) return ;
			
			var nextRow =  table.rows( tr.rowIndex + 1) ;
			if( nextRow.style.display == "" ){ 
				nextRow.style.display = "none";
				obj.src = TreeView_LastNodeClickExpandImgUrl ;
			}
			else{
				nextRow.style.display = "";
				obj.src =  TreeView_LastNodeClickImgUrl;
			}
			
			if( TreeView_EnableAsyncLoad && nextRow.cells[1].innerHTML == ""  )
				TreeView_AsyncLoad( treeId , obj , nodeValue , tr.cells[1].innerText ) ;		
				
		}
		
		//获取xmlhttp对象，对不同的浏览器对象不同
		function TreeView_GetRequestObj(){
			var req = new ActiveXObject("microsoft.xmlhttp");
			//var req = new ActiveXObject("Msxml2.XMLHTTP.3.0");
			return req ;
		}
		//请求数据
		function TreeView_AsyncLoad( treeId , obj , nodeValue , nodeText ){
			var tr = obj.parentElement.parentElement ;
			var table = tr.parentElement.parentElement;
			
			var cks = tr.getElementsByTagName( "input" ); //tvID ,
			var checkedSet = "" ;

			if( cks.length > 0 )
			{
				checkedSet = "&TreeView_NodeChecked=" + cks[0].checked + "&TreeView_NodeID=" + cks[0].id ;
			}
			
			var nextRow =  table.rows( tr.rowIndex + 1 ) ;
			
			var request = TreeView_GetRequestObj() ;
			
			//var url =  "" + window.location.pathname ;
			var url =  "" + window.location.href ;
			url = document.forms[0].action;
			if( url.indexOf("?") == -1 ) url += "?";
			
			url = url + "&TreeView_NodeValue=" + escape(nodeValue) + "&TreeView_NodeText=" + escape(nodeText) +
							checkedSet + 
							"&TreeView_MultiSelect=" + TreeView_MultiSelect + 
							"&TreeView_AutoSelectChildNodes=" + TreeView_AutoSelectChildNodes +
							"&TreeView_ShowLines=" + TreeView_ShowLines + "&TreeView_ClientID=" + treeId ;
							
			nextRow.cells[1].innerHTML = TreeView_AsyncLoadMessage ;
				
			request.onreadystatechange = function()
			{
				if(request.readyState == 4){
					var html = request.responseText ;
					if( html != "" )
					{		
						nextRow.cells[1].innerHTML = html ;
						
						if( TreeView_MultiSelect && TreeView_AutoSelectChildNodes && cks.length > 0 )
							TreeView_SelectChildNodes( cks[0] );
					}
					else
					{
						nextRow.cells[1].innerHTML = "&nbsp;" ;
						nextRow.style.display = "none";	
						if( TreeView_ShowLines ){ 
							if( table.rows.length == ( nextRow.rowIndex + 1 ) ) //last node
								tr.cells[0].innerHTML = "<img src='" + TreeView_EndNodeImgUrl + "'>";		
							else
								tr.cells[0].innerHTML = "<img src='" + TreeView_NodeImgUrl + "'>";
						}else
						{
							tr.cells[0].innerHTML = " &nbsp;" ;
						}				
					}
					
					eval( treeId + "_OnAsyncLoadComplete();" );
				}
			}
			
			eval( treeId + "_OnAsyncLoad();" ); 
			
			request.open( "GET", url , true );
			request.send();
		}
		
		//请求数据-加载根节点
		function TreeView_AsyncLoadRoot(  treeId ){		
			var treeContainer = document.getElementById( treeId );
							
			var request = TreeView_GetRequestObj() ;
			
			var url =  "" + window.location.href ;
			url = document.forms[0].action;
			if( url.indexOf("?") == -1 ) url += "?";
			
			url = url + "&TreeView_LoadRoot=true&TreeView_NodeValue=&TreeView_NodeText=&TreeView_MultiSelect=" + TreeView_MultiSelect + 
							"&TreeView_AutoSelectChildNodes=" + TreeView_AutoSelectChildNodes +
							"&TreeView_ShowLines=" + TreeView_ShowLines + "&TreeView_ClientID=" + treeId
							
			treeContainer.innerHTML = TreeView_AsyncLoadMessage ;
				
			request.onreadystatechange = function()
			{
				if(request.readyState == 4){
					var html = request.responseText ;					
					treeContainer.innerHTML = html ;
					eval( treeId + "_OnAsyncLoadComplete();" );						
				}
			}
			
			eval( treeId + "_OnAsyncLoad();" );
			
			request.open( "GET", url , true );
			request.send();
		}
	
		
		//单选效果
		var TreeView_CurCheckbox = null ;
		function TreeView_SingleSelect( tvId , checkbox ){
			
			checked = checkbox.checked ;
			
			if( TreeView_CurCheckbox != null )
				TreeView_CurCheckbox.checked = false ;
			else
				TreeView_SetNodesChecked( tvId , false ); //第一次清初所有
			
			TreeView_CurCheckbox = checkbox ;	
				
			checkbox.checked = checked ;
		}
		
		//选中子节点
		function TreeView_SelectChildNodes( checkbox ){

			var tr = checkbox.parentElement.parentElement ;
			var table = tr.parentElement.parentElement;
			
			var nextRow =  table.rows( tr.rowIndex + 1) ;
			
			objs = nextRow.getElementsByTagName( "input" ) ;
			
			for( i = 0 ; i < objs.length ; i ++ ){
				if( objs[0].type != "checkbox" ) continue ;
				objs[i].checked = checkbox.checked ;
			}
		}
		
		//选父节点
		function TreeView_SetParentNodeChecked( treeId , tableObj , checked )
		{
			if( tableObj.parentElement.id == treeId ) return ;
			
			parentTable = tableObj.parentElement.parentElement.parentElement.parentElement;
			
			parentRow = tableObj.parentElement.parentElement;
			
			if( false == checked ) //同一级若有其他节点被选中
			{
				var others = tableObj.getElementsByTagName( "input" )
				if( TreeView_IsOneChecked( others ) ) return ;
			}	
			
			var objs =  parentTable.rows( parentRow.rowIndex-1 ).getElementsByTagName( "input" )
		
			if( objs.length > 0 )
				objs[0].checked = checked ;

			TreeView_SetParentNodeChecked( treeId , parentTable , checked ) ;
		}
		
		function TreeView_IsOneChecked( checkboxs )
		{
			for( i = 0 ; i < checkboxs.length ; i ++ )
			{
				if( checkboxs[i].checked ) return true ;		
			}
			return false ;
		}
		
		//选父节点
		function TreeView_SelectParentNodes( treeId , checkbox )
		{
			var tr = checkbox.parentElement.parentElement ;
			var table = tr.parentElement.parentElement;
			
			TreeView_SetParentNodeChecked( treeId , table , checkbox.checked )
		}
		
		
		
		function TreeView( id ){
			this.ID = id ;
		
			this.GetSelectedNodes = function(){
				return TreeView_GetSelectedNodes( this.ID );
			}
			
			this.SetSelectedNodes = function( valueList ){
				TreeView_SelectNodes( this.ID , valueList );
			}
		}
		
		//选中节点
		//id : TreeView ClientID  ； valueList 选中节点值列表
		function TreeView_SelectNodes( id , valueList ){
			var objs = document.getElementsByName( id + "_Node" );
			var list = "," + valueList + ",";
				
			for( i = 0 ; i < objs.length ; i ++ ){
				if( objs[0].type != "checkbox" ) continue ;
				
				if( list.indexOf( "," + objs[i].value + "," ) != -1 )
					objs[i].checked = true ;
				else
					objs[i].checked = false ;
			}
		}
	
		 
		function TreeView_GetSelectedNodes( id ){
			var nodes = new Array();
			var node ;
			var objs = document.getElementsByName( id + "_Node" );
			
			var count = 0 ;
			for( i = 0 ; i < objs.length ; i ++ ){
				if( objs[0].type != "checkbox" || false == objs[i].checked ) continue ;
				 
				node = new Object();
				node.Value = objs[i].value ;
				
				var tr = objs[i].parentElement.parentElement ;
				node.Text = tr.cells[1].innerText ;
				
				nodes[ count ] = node ;
				count ++ ;
			}
			return nodes ;
		} 
		
		function TreeView_GetCheckedNodeValueList( id ){
			 
			var objs = document.getElementsByName( id + "_Node" );
			
			var list = "" ;
			for( i = 0 ; i < objs.length ; i ++ ){
				if( objs[0].type != "checkbox" || false == objs[i].checked ) continue ;
				 
				if( list != "" )
					list += "," ;
				list += objs[i].value ;	
			}
			
			return list ;
		} 
		
		//设置所有节点选中状态
		//id : TreeView ClientID ; checked : 是否选中
		function TreeView_SetNodesChecked( id , checked ){
			var objs = document.getElementsByName( id + "_Node" );
			for( i = 0 ; i < objs.length ; i ++ ){
				if( objs[0].type != "checkbox"  ) continue ;
				objs[i].checked = checked ;
			}
		} 
		
		//获取选择节点对象 节点值列表object.NodeValueList , 节点文本列表object.NodeTextList
		function TreeView_GetSelectedObject( id ){
			var obj = new Object();
			var node ;
			var objs = document.getElementsByName( id + "_Node" );
			
			var valueList = "";
			var textList = "";
			
			var count = 0 ;
			for( i = 0 ; i < objs.length ; i ++ ){
				if( objs[0].type != "checkbox" || false == objs[i].checked ) continue ;
				 
				if( valueList != "" )
				{
					valueList += ",";
					textList += ",";
				} 
				
				valueList += objs[i].value ;
				
				var tr = objs[i].parentElement.parentElement ;
				textList += tr.cells[1].innerText ;
			}
			
			obj.NodeValueList = valueList ;
			obj.NodeTextList = textList ;
					
			return obj ;
		} 

