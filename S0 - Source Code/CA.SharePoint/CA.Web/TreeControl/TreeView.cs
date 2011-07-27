// --------------------------------------------------------------------
// - 版权所有  beyondbit.com
// - 作者：    张建义        Email:jianyi0115@163.com
// - 创建：    2005.11.18
// - 更改：
// - 备注：    
// --------------------------------------------------------------------

using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.TreeControl
{
	

	/// <summary>
	/// 实现异步加载的TreeView所在的Page必须实现的接口，定义子节点数据如何获取
	/// </summary>
	/// <example>
	/// 异步代码示例:
	/// <code>
	/// public class TreeTest : System.Web.UI.Page , ITreeViewAsyncCallback 
	/// {
	///		public string TreeViewGetChildHtml( TreeNode node ) 
	///		{
	///			return "" ;
	///		}
	///
	///		public TreeNodeCollection TreeViewGetChildNodes( TreeNode parentNode )
	///		{
	///			TreeNodeCollection nodes = new TreeNodeCollection(parentNode);
	///			TreeNode node = new TreeNode();
	///			node.Text = "NodeText" ;
	///			node.Value = "NodeValue" ;
	///			node.ImageUrl = "images/tree/icon-user.gif";
	///			node.Checked = false ;
	///			node.Expand = true ;
	///			nodes.Add( node ) ;
	///			return nodes ;
	///		}
	/// }
	/// </code>
	/// </example>
	public interface ITreeViewAsyncCallback
	{
		/// <summary>
		/// 获取子节点
		/// </summary>
		/// <param name="node">父节点;提供Value,Text属性</param>
		/// <returns>子节点集合</returns>
		TreeNodeCollection TreeViewGetChildNodes( TreeNode node ) ;

		/// <summary>
		/// 获取节点内容(html)
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		string TreeViewGetChildHtml( TreeNode node ) ;

	}

	/// <summary>
	/// 异步加载方式
	/// </summary>
	public enum AsyncCallbackType
	{
		/// <summary>
		/// 加载子节点
		/// </summary>
		GetChildNodes = 0 ,

		/// <summary>
		/// 加载任意的html
		/// </summary>
		GetChildHtml = 1 
	}

	/// <summary>
	/// 节点创建事件参数
	/// </summary>
	public class TreeNodeCreatedEventArgs : EventArgs 
	{
		internal TreeNodeCreatedEventArgs(  TreeNode node )
		{
			Node = node;
		}
		/// <summary>
		/// 被创建的节点
		/// </summary>
 		public readonly TreeNode Node ;
	}

	/// <summary>
	/// 节点创建事件代理
	/// </summary>
	public delegate void TreeNodeCreatedEventHandler( object sender , TreeNodeCreatedEventArgs args );

	/// <summary>
	/// 
	///  web树控件　1.0
	///  
	///  @author jianyi015@163.com
	///  
	///  2005-12-10 完成
	///  12-13 加入 TreeNodeCreated 事件
	///  
	///  具有如下功能:
	///  1.单选，复选，继承选择的客户端支持
	///  2.异步加载子节点功能
	///  3.可以异步加载html
	///  
	///  
	///  为提高性能，控件无ViewState功能，若回发页面需要保持TreeView的状态，需要代码控制，
	///  此控件十分适合页面无回发的情况，如弹出的选择树窗口，动态加载的导航树目录等
	///  
	///  注：　一个页面只支持一个TreeView 
	///  
	///  考虑到大多数情况下，一个页面只有一个树控件，
	///  要支持一个页面多个控件的需要对相应元素增加命名容器，增大了数据传输量，
	///  并给异步加载造成复杂性
	///  
	/// </summary>
	[ToolboxData("<{0}:TreeView runat=server></{0}:TreeView>")]
	[Designer( typeof(TreeViewDesigner) )]
	[ParseChildren( true , "ChildNodes" )]
	[DefaultEvent( "TreeNodeCreated" )]
	public class TreeView : System.Web.UI.WebControls.WebControl , System.Web.UI.IPostBackDataHandler
	{
		/// <summary>
		/// 
		/// </summary>
		public TreeView()
		{
			this._ChildNodes = new TreeNodeCollection( this , null ) ;
		}

		/// <summary>
		/// 节点创建事件
		/// </summary>
		public event TreeNodeCreatedEventHandler TreeNodeCreated ;

		/// <summary>
		/// 处理TreeNodeCreated事件
		/// </summary>
		/// <param name="node"></param>
		internal void OnTreeNodeCreated( TreeNode node )
		{
			if( TreeNodeCreated != null )
				RaiseTreeNodeCreatedEvent(  node ) ;
		}

		/// <summary>
		/// 处理TreeNodeCreated事件
		/// </summary>
		/// <param name="nodes"></param>
		internal void OnTreeNodeCreated( TreeNodeCollection nodes )
		{
			if( TreeNodeCreated != null )
			{
				foreach( TreeNode n in nodes )
					RaiseTreeNodeCreatedEvent(  n ) ;
			}
		}
		
		/// <summary>
		/// 处理TreeNodeCreated事件,第归引发事件
		/// </summary>
		/// <param name="node"></param>
		private void RaiseTreeNodeCreatedEvent( TreeNode node )
		{
			node.SetOwner( this );
			TreeNodeCreated( this , new TreeNodeCreatedEventArgs( node ) ) ;

			foreach( TreeNode n in node.ChildNodes )
			{
				n.SetOwner( this );
				RaiseTreeNodeCreatedEvent( n ) ;
			}
		}

		private string _ImageFolderUrl = "~/images/tree/" ; 
		/// <summary>
		/// 设置图片文件路径
		/// </summary>
		[Editor( typeof( System.Web.UI.Design.UrlEditor ) , typeof(System.Drawing.Design.UITypeEditor) ),
		Category("Settings"),
		Description("设置图片文件路径")]
		public string ImageFolderUrl
		{
			set
			{ 
				if( value.EndsWith("/") )
					_ImageFolderUrl = value ;
				else
					_ImageFolderUrl = value + "/" ; 
			}
			get
			{ 
				return _ImageFolderUrl ;
			}
		}
		
		//private Hashtable _ValueList = new Hashtable();
		private string _CheckedNodeValueList = "" ; 
		/// <summary>
		/// 获取选种的节点值列表
		/// </summary>
		[Category("Data"),
		Description("获取选中的节点值列表")]
		public string CheckedNodeValueList
		{
//			set
//			{ 
//				_CheckedNodeValueList = value ;
//			}
			get
			{ 
				return _CheckedNodeValueList ;  
			}
		}

		private AsyncCallbackType _AsyncCallbackType = AsyncCallbackType.GetChildNodes ;

		/// <summary>
		/// 异步加载方式
		/// </summary>
		[Category("Behavior"),
		Description("异步加载方式")]
		public AsyncCallbackType AsyncCallbackType
		{
			set{ _AsyncCallbackType = value ; }
			get{ return _AsyncCallbackType; }
		}

		private bool _IsCallback = false ;
		/// <summary>
		/// 控件是否处于异步回传状态
		/// </summary> 
		[Bindable(false),
		Description("是否处于异步回传状态")]
		public bool IsCallback
		{
			get{ return _IsCallback ;}
		}  

		private bool _IsCallTarget = false ;
		/// <summary>
		/// 控件是否处于异步回传状态
		/// </summary> 
		[Bindable(false),
		Description("控件是否处于异步回传状态")]
		public bool IsCallTarget
		{
			get{ return _IsCallTarget ;}
		}  

		private bool _ShowLines = false ; 
		/// <summary>
		/// 是否显示连接上下节点的线
		/// </summary>
		[Category("Appearance"),
		Description("是否显示连接上下节点的线")]
		public bool ShowLines
		{
			set{ _ShowLines = value ; }
			get{ return _ShowLines ;}
		}  

		private bool _EnableAsyncLoad = false ; 
		/// <summary>
		/// 是否启用动态加载，若启用，则Page必须实现ITreeViewAsyncCallback接口
		/// </summary>
		[Category("Behavior"),
		Description("否启用动态加载，若启用，则Page必须实现ITreeViewAsyncCallback接口")]
		public bool EnableAsyncLoad
		{
			set{ _EnableAsyncLoad = value ; }
			get{ return _EnableAsyncLoad ;}
		}  

		private bool _AutoLoadRootNodes = true ;
		[Category("Behavior"),
		Description("是否自动加载根节点")]
		public bool AutoLoadRootNodes
		{
			set{ _AutoLoadRootNodes = value ; }
			get{ return _AutoLoadRootNodes ;}
		}  

		private string _CheckboxClickHtml = "" ;
		private string _CheckboxClicked = "" ;
		private bool _MultiSelect = true ; 
		/// <summary>
		/// 是否允许多选
		/// </summary>
		[Category("Behavior"),
		Description("是否允许多选")]
		public bool MultiSelect
		{
			set
			{ 
				_MultiSelect = value ; 
			}
			get{ return _MultiSelect ;}
		}  

		private string _AsyncLoadMessage = " 正在加载...";
		/// <summary>
		/// 异步加载时信息
		/// </summary>
		[Category("Settings"),
		Description("异步加载时信息")]
		public string AsyncLoadMessage
		{
			set{ _AsyncLoadMessage = value ; }
			get{ return _AsyncLoadMessage ;}
		}  

		private string _AsyncLoadErrMessage = "";
		/// <summary>
		/// 异步加载错误时信息,若未设置则显示详细信息
		/// </summary>
		[Category("Settings"),
		Description("异步加载错误时信息,若未设置则显示错误详细信息")]
		public string AsyncLoadErrMessage
		{
			set{ _AsyncLoadErrMessage = value ; }
			get{ return _AsyncLoadErrMessage ;}
		}  


		private bool _AutoSelectChildNodes = false ; 
		/// <summary>
		/// 多选状态下，是否自动选中子节点
		/// </summary>
		[Category("Behavior"),
		Description("多选状态下，是否自动选中子节点")]
		public bool AutoSelectChildNodes
		{
			set{ _AutoSelectChildNodes = value ; }
			get{  return _AutoSelectChildNodes ; }
		}  

		private bool _AutoSelectParentNodes = false ;
		/// <summary>
		/// 是否自动选中父节点
		/// </summary>
		[Category("Behavior"),
		Description("多选状态下，是否自动选中父节点")]
		public bool AutoSelectParentNodes
		{
			set{ _AutoSelectParentNodes = value ; }
			get{ return _AutoSelectParentNodes ; }
		}

		private string _OnAsyncLoadComplete;
		/// <summary>
		/// 节点异步加载子节点后触发的客户端js函数 OnAsyncLoadComplete
		/// </summary>
		[Category("Behavior")]
		[Description("异步加载子节点结束后触发的js函数")]
		public string OnAsyncLoadComplete 
		{
			set{ _OnAsyncLoadComplete = value ; }
			get{ return _OnAsyncLoadComplete ;}
		}

		private string _OnAsyncLoad;
		/// <summary>
		/// 节点异步加载子节点后触发的客户端js函数 OnAsyncLoadComplete
		/// </summary>
		[Category("Behavior")]
		[Description("异步加载子节点开始时触发的js函数")]
		public string OnAsyncLoad 
		{
			set{ _OnAsyncLoad = value ; }
			get{ return _OnAsyncLoad ;}
		}

		private TreeNodeCollection _ChildNodes   ;
		/// <summary>
		/// 获取子节点
		/// </summary>
		[Bindable(false)]
		public TreeNodeCollection ChildNodes
		{
			//set{ _ChildNodes = value ; }
			get{ return _ChildNodes ;}
		}

		private void InitNodeJs()
		{
			_CheckboxClicked = this.ClientID + "_NodeClick(this)";
			_CheckboxClickHtml = "onclick=\""+_CheckboxClicked+"\"";
		}

		/// <summary>
		/// 选择必要的脚本
		/// </summary>
		private void InitModeJs()
		{			
			string nodeClickJs = "<script language='javascript'>\n";

			nodeClickJs += "function " + this.ClientID + "_NodeClick(obj){\n";

			if( _MultiSelect == false )
			{
				nodeClickJs += "TreeView_SingleSelect('"+this.ClientID+"',obj);\n";
			}
			else
			{
				if( _AutoSelectChildNodes )
				{
					nodeClickJs += "TreeView_SelectChildNodes(obj);\n";
				}

				if( this._AutoSelectParentNodes )
				{
					nodeClickJs += "TreeView_SelectParentNodes('"+this.ClientID+"',obj);\n";
				}
			}

			nodeClickJs += "\n}</script>";

			Page.RegisterClientScriptBlock( this.ClientID , nodeClickJs ) ;


//			if( _MultiSelect == false )
//			{
//				_CheckboxClicked = "TreeView_SingleSelect('"+this.ClientID+"',this)";
//				_CheckboxClickHtml = "onclick=\"TreeView_SingleSelect('"+this.ClientID+"',this);\"";
//			}
//			else
//			{
//				if( _AutoSelectChildNodes )
//				{
//					_CheckboxClicked = "TreeView_SelectChildNodes(this);";
//					_CheckboxClickHtml = "onclick=\"TreeView_SelectChildNodes(this);\"";
//				}
//			}
		}

		/// <summary>
		/// 初始化，若控件异步回传，则调用Page ITreeViewAsyncCallback 接口的GetChildNode方法返回自节点数据
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			if( Page.Request.QueryString["TreeView_ClientID"] != null )
			{
				this._IsCallback = true ;
			}

			if( Page.Request.QueryString["TreeView_ClientID"] == this.ClientID )
			{
				this._IsCallTarget = true ;

				this.ImageFolderUrl = base.ResolveUrl( this.ImageFolderUrl );

				this.EnableAsyncLoad = true ;

				if( Page.Request.QueryString["TreeView_ShowLines"] == "true" )
					this._ShowLines = true ;
				else
					this._ShowLines = false ;

				if( Page.Request.QueryString["TreeView_MultiSelect"] == "true" )
					this._MultiSelect= true ;
				else
					this._MultiSelect = false ;

				if( Page.Request.QueryString["TreeView_AutoSelectChildNodes"] == "true" )
					this._AutoSelectChildNodes = true ;
				else
					this._AutoSelectChildNodes = false ;

				ITreeViewAsyncCallback c ;

				if( this is ITreeViewAsyncCallback )
					c = this as ITreeViewAsyncCallback ;
				else if( Page  is ITreeViewAsyncCallback   )
					c = Page as ITreeViewAsyncCallback ;
				else
					throw new Exception( "Page 没有实现 ITreeViewAsyncCallback 接口，不能异步加载数据" );

				//TreeNodeCollection nodes = c.GetChildNode( new TreeNode( Page.Request.QueryString["TreeViewLoadValue"] , "" ) ) ;

				TreeNode parentNode = null ;

				if( Page.Request.QueryString["TreeView_LoadRoot"] != "true" )
				{
					parentNode = new TreeNode( Page.Request.QueryString["TreeView_NodeValue"] , Page.Request.QueryString["TreeView_NodeText"] );
				
					parentNode.SetOwner( this ) ;

					if( Page.Request.QueryString["TreeView_NodeChecked"] == "true" )
						parentNode.Checked = true ;

					//parentNode.Id = Page.Request.QueryString["TreeView_NodeID"] ;
				}

				Page.Response.Clear() ;

				//Page.Response.Write( Page.Request.QueryString.ToString() );

				if( _AsyncCallbackType == AsyncCallbackType.GetChildNodes )
				{
					this._ChildNodes = c.TreeViewGetChildNodes( parentNode ) ;

					OnTreeNodeCreated( _ChildNodes ) ;

					HtmlTextWriter writer = new HtmlTextWriter( Page.Response.Output ) ;

					RenderNodes( writer ) ;
				}
				else
				{
					Page.Response.Write( c.TreeViewGetChildHtml( parentNode ) ) ;
				}

				//Page.Response.Flush();
		 
				Page.Response.End();
			}
		}

		private const string ClientJsKey = "TreeView-ClientJs";

		/// <summary>
		/// 获取参数设置js--通用js
		/// </summary>
		/// <returns></returns>
		private string GetSettingJs()
		{
			string settingJs = "<SCRIPT lang='javascript'>" ;

			settingJs += "\n var TreeView_EnableAsyncLoad = " + this.EnableAsyncLoad.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_MultiSelect = " + this.MultiSelect.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_AutoSelectChildNodes = " + this.AutoSelectChildNodes.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_ShowLines = " + this.ShowLines.ToString().ToLower() + " ;" ;
			settingJs += "\n var TreeView_NodeImgUrl = '"+this.ImageFolderUrl+"node.gif';" ;
			settingJs += "\n var TreeView_EndNodeImgUrl = '"+this.ImageFolderUrl+"end.gif';" ;

			settingJs += "\n var TreeView_AsyncLoadMessage = \"" + this.AsyncLoadMessage.ToString() + "\" ;" ;
			settingJs += "\n var TreeView_AsyncLoadErrMessage = \"" + this.AsyncLoadErrMessage.ToString() + "\" ;" ;

			if( this.ShowLines )
			{
				settingJs += "\n var TreeView_LineNodeClickImgUrl = '"+this.ImageFolderUrl +"centerminus.gif';" +
					"\n var TreeView_LineNodeClickExpandImgUrl = '"+this.ImageFolderUrl+"centerplus.gif';" +								
					"\n var TreeView_LastNodeClickImgUrl = '"+this.ImageFolderUrl+"endminus.gif';" + 
					"\n var TreeView_LastNodeClickExpandImgUrl = '" + this.ImageFolderUrl + "endplus.gif';";
			}
			else
			{
				settingJs += "\n var TreeView_LineNodeClickImgUrl = '"+this.ImageFolderUrl +"minus.gif';" +
					"\n var TreeView_LineNodeClickExpandImgUrl = '"+this.ImageFolderUrl+"plus.gif';" +								
					"\n var TreeView_LastNodeClickImgUrl = '"+this.ImageFolderUrl+"endminus.gif';" + 
					"\n var TreeView_LastNodeClickExpandImgUrl = '" + this.ImageFolderUrl + "endplus.gif';";
			}
								
			settingJs += "\n</SCRIPT>" ;

			return settingJs ;
		}

		//树特殊的js
		//js函数以ClientID标示区分
		private string GetInstanceJs()
		{
			string instanceJs = "<SCRIPT lang='javascript'>" ;

			//_OnAsyncLoad

			instanceJs += "\n function "+this.ClientID+"_OnAsyncLoad(){ \n" ;

			
			if( this.OnAsyncLoad != null )
			{
				instanceJs += "\n try{\n";
				instanceJs += OnAsyncLoad + ";" ;
				instanceJs += "\n }catch(e){alert('OnAsyncLoad函数错误：'+ e )}";
			}
						
			instanceJs += "\n }" ;

			//_OnAsyncLoad end

			// OnAsyncLoadComplete function
			instanceJs += "\n function "+this.ClientID+"_OnAsyncLoadComplete(){ \n" ;
			
			if( this.OnAsyncLoadComplete != null )
			{
				instanceJs += "\n try{\n";
				instanceJs += OnAsyncLoadComplete + ";" ;
				instanceJs += "\n }catch(e){alert('OnAsyncLoadComplete函数错误：'+ e )}";
			}			
			
			instanceJs += "\n }" ;

			//OnAsyncLoadComplete end	

			//_NodeClick function
			instanceJs += "\n function " + this.ClientID + "_NodeClick(obj){\n";

			if( _MultiSelect == false )
			{
				instanceJs += "TreeView_SingleSelect('"+this.ClientID+"',obj);\n"; //单选js
			}
			else
			{
				if( _AutoSelectChildNodes )
				{
					instanceJs += "TreeView_SelectChildNodes(obj);\n"; //子继承选择js
				}

				if( this._AutoSelectParentNodes )
				{
					instanceJs += "TreeView_SelectParentNodes('"+this.ClientID+"',obj);\n"; //父选择js
				}
			}

			instanceJs += "\n }" ;
			////_NodeClick function end
								
			instanceJs += "\n</SCRIPT>" ;

			return instanceJs ;
		}


		/// <summary>
		/// 获取样式html
		/// </summary>
		/// <returns></returns>
		private string GetStyle()
		{			
			string style = "<style> .Tree_Line { BACKGROUND-POSITION-X: center; BACKGROUND-IMAGE: url("+this.ImageFolderUrl+"back.gif); WIDTH: 10px; BACKGROUND-REPEAT: repeat-y } </style>";
			return style ;
		}

		/// <summary>
		/// 注册客户端脚本
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			this.ImageFolderUrl = base.ResolveUrl( this.ImageFolderUrl );

			Page.RegisterRequiresPostBack( this ) ; //

//			if( Page.IsClientScriptBlockRegistered( ClientJsKey ) )
//				throw new Exception( "TreeView只支持每个页面一个控件" );

			Page.RegisterClientScriptBlock( ClientJsKey , ResourceHelper.GetJsResourceString( typeof(TreeView) ) ) ; //注册通用js

			Page.RegisterClientScriptBlock( ClientJsKey + "-Setting" , GetSettingJs() ) ; //注册参数设置js

			Page.RegisterClientScriptBlock( ClientJsKey + "-Style" ,  GetStyle() ) ; //注册style

			//InitModeJs();

			//自动加载第一级
			if( this.EnableAsyncLoad && this.ChildNodes.Count== 0 && AutoLoadRootNodes )
			{
				Page.RegisterStartupScript( this.ClientID + "_LoadRoot" , "<SCRIPT lang=\"javascript\">TreeView_AsyncLoadRoot('"+this.ClientID+"');</SCRIPT> " ) ;
			}

			//注册特殊js函数
			Page.RegisterClientScriptBlock( this.ClientID + "_Instance" ,  GetInstanceJs() ) ;

			//注册checked

			//string js = "<SCRIPT lang='javascript'>" + GetResourceString( t , "js" ) + "</SCRIPT> " ;
			//js += "function TreeView_NodeChecked(this)";

		}

		/// <summary>
		///获取所有的树Html，包括脚本.
		/// 若考虑到将树生成的所有html做缓存，可以调用此方法获取能使树运行的完整html
		/// </summary>
		/// <param name="withJs">是否包含脚本</param>
		/// <returns></returns>
		public string GetTreeHtml( bool withJs )
		{
			StringWriter tw = new StringWriter(); 
			HtmlTextWriter hw = new HtmlTextWriter(tw);

			if( withJs )
			{
				hw.WriteLine( ResourceHelper.GetJsResourceString( this.GetType() ) ) ;
				hw.WriteLine(  GetSettingJs() ) ;
				hw.WriteLine( GetStyle() ) ;
			}

			this.RenderControl(hw); 
			
			return tw.ToString( )  ;
		}

		/// <summary>
		/// 输出html
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderBeginTag( writer ) ;

			RenderNodes( writer ) ;

			this.RenderEndTag( writer ) ;

		}
		/// <summary>
		/// 输出子节点html
		/// </summary>
		/// <param name="writer"></param>
		private void RenderNodes( HtmlTextWriter writer )
		{
			if( ChildNodes == null || ChildNodes.Count == 0 ) return ;

			//InitModeJs() ;

			InitNodeJs();

			writer.WriteLine( "<table border='0' cellspacing=0 cellpadding=0>" );// border='1' bordercolor='#0030b0' cellspacing='0'

			if( _ShowLines )
			{
				for( int i = 0 ; i < this.ChildNodes.Count - 1 ; i ++ )
				{
					RenderLineNode( this.ChildNodes[i] , writer ) ;
				}

				RenderLastNode( this.ChildNodes[this.ChildNodes.Count - 1] , writer ) ;
			}
			else
			{
				for( int i = 0 ; i < this.ChildNodes.Count ; i ++ )
				{
					RenderNode( this.ChildNodes[i] , writer ) ;
				}

			}

			writer.WriteLine( "</table>" );
		}
		
		/// <summary>
		/// 输出节点的复选框html
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderCheckbox( TreeNode node , HtmlTextWriter writer )
		{
			if( node.EnableChecked )
			{
				writer.Write( "<input name='" + this.ClientID + "_Node' type='checkbox' value=\"" + node.Value + "\" " );

				if( node.OnCheckBoxClick != null && node.OnCheckBoxClick != "" )
				{
					//if( _CheckboxClickHtml != "" )
					writer.Write( "onclick=\"" );
					writer.Write( _CheckboxClicked );
					writer.Write( ";" );
					writer.Write( node.OnCheckBoxClick );
					writer.Write( ";\"" );
				}
				else
				{
					writer.Write( _CheckboxClickHtml );
				}

				if( node.ToolTip != null )
				{
					writer.Write( " title=\"" );
					writer.Write( node.ToolTip );
					writer.Write( "\"" );
				}

				if( node.Checked )
					writer.Write( " checked " );
				
				if( node.Disabled )
					writer.Write( " disabled " );

				//writer.Write( " ID='"+ node.Id +"'" );

				writer.Write( ">" );
			}
		}

		private void RenderNodeText( TreeNode node , HtmlTextWriter writer )
		{
			if( node.NavigateUrl != null )
			{
				writer.Write( "<a " );
				writer.Write( node.TargetHtml );
				writer.Write( " href=\"" + node.NavigateUrl + "\">" );
				writer.Write( node.Text );
				writer.Write( "</a>");
			}
			else
				writer.Write( node.Text );			 
		}

		
		private void RenderClickImage( TreeNode node , HtmlTextWriter writer )
		{
			if( node.Expand )
				writer.Write( "<image src='" + this.ImageFolderUrl + "minus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
			else
				writer.Write( "<image src='" + this.ImageFolderUrl + "plus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
		}
		
		/// <summary>
		/// 递归输出节点html,不显示下拉线
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderNode( TreeNode node , HtmlTextWriter writer )
		{
			writer.WriteLine( "<tr>" );
			writer.Write( "<td>" );
			//输出加号
			if( node.ChildNodes.Count > 0 || node.HasChildNodes )
			{
				RenderClickImage( node , writer ) ;

//				if( node.Expand )
//					writer.Write( "<image src='" + this.ImageFolderUrl + "minus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
//				else
//					writer.Write( "<image src='" + this.ImageFolderUrl + "plus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
			}
			else
			{
				writer.Write( " &nbsp; " ) ;
			}
			

			writer.WriteLine( "</td>" );
			writer.Write( "<td nowrap>" );

			RenderCheckbox( node , writer ) ;

			if( node.ImageUrl != null )
				writer.Write( "<image src=" + node.ImageUrl + ">" );
								
//			if( node.NavigateUrl != null )
//			{
//				writer.Write( "<a " );
//				writer.Write( node.TargetHtml );
//				writer.Write( " href=\"" + node.NavigateUrl + "\">" );
//				writer.Write( node.Text );
//				writer.Write( "</a>");
//
//			}
//			else
//				writer.Write( node.Text );

			RenderNodeText( node , writer ) ;

			writer.WriteLine( "</td>" );
			writer.WriteLine( "</tr>" );
				
			//子节点
			writer.WriteLine( "<tr " );
			writer.Write( node.ExpandHtml );
			writer.WriteLine( ">" );

			writer.Write( "<td width='5'>&nbsp; " );

			//writer.Write( " --" );

			writer.Write( "</td>" );
			writer.WriteLine( "<td>" );

			if( node.ChildNodes.Count > 0 )
			{
				writer.WriteLine( "<table border='0' cellspacing=0 cellpadding=0 >" );//border='1' bordercolor='#0030b0' cellspacing='0'

				foreach( TreeNode n in node.ChildNodes ) 
					RenderNode( n , writer ) ;

				writer.WriteLine( "</table>" );
			}

			writer.WriteLine( "</td>" );

			writer.WriteLine( "</tr>" );
				
		} 

		/// <summary>
		/// 递归输出节点html,显示下拉线
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderLineNode( TreeNode node , HtmlTextWriter writer   )
		{
			writer.WriteLine( "<tr>" );
			writer.Write( "<td>" );
			//输出加号
	 
			if( node.ChildNodes.Count > 0 || node.HasChildNodes )
			{
				if( node.Expand )
					writer.Write( "<image src='" + this.ImageFolderUrl + "centerminus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
				else
					writer.Write( "<image src='" + this.ImageFolderUrl + "centerplus.gif'  onclick=\"TreeView_NodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );

			}
			else
			{
				writer.Write( "<image src='" + this.ImageFolderUrl + "node.gif'>" );
			}
			
			writer.WriteLine( "</td>" );
			writer.Write( "<td nowrap>" );
	
			RenderCheckbox( node , writer ) ;

			if( node.ImageUrl != null )
				writer.Write( "<image src=" + node.ImageUrl + ">" );


			RenderNodeText( node , writer ) ;

			writer.WriteLine( "</td>" );
			writer.WriteLine( "</tr>" );
				
			//子节点
			writer.WriteLine( "<tr " );
			writer.Write( node.ExpandHtml );
			writer.WriteLine( ">" );

			writer.Write( "<td class='Tree_Line'> " ); // style='background-repeat:repeat-y;BACKGROUND-POSITION-X: center;'  background='"+ImageFolderUrl+"back.gif'

			//writer.Write( " --" );

			writer.Write( "</td>" );
			writer.WriteLine( "<td>" );

			if( node.ChildNodes.Count > 0 )
			{
				writer.WriteLine( "<table  border=0 cellspacing=0 cellpadding=0>" );//border='1' bordercolor='#0030b0' cellspacing='0'

				for( int i = 0 ; i < node.ChildNodes.Count - 1 ; i ++ )
				{
					RenderLineNode( node.ChildNodes[i] , writer ) ;
				}

				RenderLastNode( node.ChildNodes[node.ChildNodes.Count - 1] , writer ) ;

				writer.WriteLine( "</table>" );
			}

			writer.WriteLine( "</td>" );

			writer.WriteLine( "</tr>" );
				
		} 

		/// <summary>
		/// 递归输出同一级的最后一个节点html,显示下拉线
		/// </summary>
		/// <param name="node"></param>
		/// <param name="writer"></param>
		private void RenderLastNode( TreeNode node , HtmlTextWriter writer   )
		{
			writer.WriteLine( "<tr>" );
			writer.Write( "<td>" );
			//输出加号

			if( node.ChildNodes.Count > 0 || node.HasChildNodes  )
			{
				if( node.Expand )
					writer.Write( "<image src='" + this.ImageFolderUrl + "endminus.gif'  onclick=\"TreeView_LastNodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
				else
					writer.Write( "<image src='" + this.ImageFolderUrl + "endplus.gif'  onclick=\"TreeView_LastNodeClick('"+this.ClientID+"',this,'"+node.Value+"')\">" );
				//writer.Write( "<image src='" + this.ImageFolderUrl + "endplus.gif'   onclick=\"TreeView_LastNodeClick(this,'"+node.Value+"')\">" );
			}
			else
				writer.Write( "<image src='" + this.ImageFolderUrl + "end.gif'>" );
					
			
			writer.WriteLine( "</td>" );
			writer.Write( "<td nowrap>" );
	
			RenderCheckbox( node , writer ) ;

			if( node.ImageUrl != null )
				writer.Write( "<image src=" + node.ImageUrl + ">" );

					
//			if( node.NavigateUrl != null )
//			{
//				writer.Write( "<a " );
//				writer.Write( node.TargetHtml );
//				writer.Write( " href=\"" + node.NavigateUrl + "\">" );
//				writer.Write( node.Text );
//				writer.Write( "</a>");
//			}
//			else
//				writer.Write( node.Text );

			RenderNodeText( node , writer ) ;

			writer.WriteLine( "</td>" );
			writer.WriteLine( "</tr>" );
				
			//子节点
			writer.WriteLine( "<tr " );
			writer.Write( node.ExpandHtml );
			writer.WriteLine( ">" );

			writer.Write( "<td width='10'> " );

			writer.Write( "</td>" );
			writer.WriteLine( "<td>" );

			if( node.ChildNodes.Count > 0 )
			{
				writer.WriteLine( "<table  border=0 cellspacing=0 cellpadding=0>" );//border='1' bordercolor='#0030b0' cellspacing='0'

				for( int i = 0 ; i < node.ChildNodes.Count - 1 ; i ++ )
				{
					RenderLineNode( node.ChildNodes[i] , writer ) ;
				}

				RenderLastNode( node.ChildNodes[node.ChildNodes.Count - 1] , writer ) ;

				writer.WriteLine( "</table>" );
			}

			writer.WriteLine( "</td>" );

			writer.WriteLine( "</tr>" );
				
		} 

		
		#region IPostBackDataHandler 成员
		/// <summary>
		/// 
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
			// TODO:  添加 TreeView.RaisePostDataChangedEvent 实现
		}

		/// <summary>
		/// 处理　CheckedNodeValueList
		/// </summary>
		/// <param name="postDataKey"></param>
		/// <param name="postCollection"></param>
		/// <returns></returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			this._CheckedNodeValueList = postCollection[ this.ClientID + "_Node" ];
			return false;
		}

		#endregion


		private string _NodeNavigateUrlFormatString = "#";
		[Category("Settings")]
		[Description("节点链接路径格式化字符串，内部生成节点时使用，如javascript:alert('{0}','{1}')")]
		public string NodeNavigateUrlFormatString 
		{
			get{return _NodeNavigateUrlFormatString ;	}
			set{ _NodeNavigateUrlFormatString = value ; }
		}

		private bool _EnableNodeChecked = false ;
		[Category("Settings")]
		[Description("是否允许节点选择")]
		public bool EnableNodeChecked 
		{
			get{return _EnableNodeChecked ;	}
			set{ _EnableNodeChecked = value ; }
		}
        

		public string NodeValeField = "Value";
		public string NodeTextField= "Text";

		public object RootId ;
		public string KeyField ;
		public string ParentKeyField;

		private object _DataSource ;
		public object DataSource
		{
			set
			{
				_DataSource = value ;
			}
		}

		public override void DataBind()
		{
			if( this._DataSource != null )
			{
				if( this.RootId != null )
					this.BuildNodes( _DataSource as DataTable , this.ChildNodes , this.RootId  ) ;
				else
				{
					DataTable t = _DataSource as DataTable ;

					ArrayList rootList = new ArrayList();

					foreach( DataRow r in t.Rows ) //若未指定RootId，自动判断
					{
						DataRow[] rows = t.Select( this.KeyField + "='"+ r[this.ParentKeyField] +"'" );
						
						if( rows.Length == 0 )
						{
							if( ! rootList.Contains( r[this.ParentKeyField].ToString() ) )
								rootList.Add( r[this.ParentKeyField].ToString() );  
						}
					}

					foreach( string rootId in rootList )
					{
						this.BuildNodes( t , this.ChildNodes , rootId  ) ;
					}
				}
			}
		}


		private void BuildNodes( DataTable t , TreeNodeCollection toNodes , object parentId )
		{
			DataRow[] rows = t.Select( this.ParentKeyField + "='"+parentId+"'" );

			//Page.Response.Write( keyColumn + "='"+parentId+"'" + rows.Length.ToString() );

			foreach( DataRow r in rows )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = r ;
			
				n.Text = r[ this.NodeTextField ].ToString() ;
				 
				n.Value = r[ this.NodeValeField ].ToString() ;

				n.EnableChecked = this.EnableNodeChecked ;

				n.NavigateUrl = string.Format( this.NodeNavigateUrlFormatString , n.Value , n.Text );

				toNodes.Add( n ) ;

				BuildNodes( t , n.ChildNodes , r[ this.KeyField ] ) ;
			}

		}


		#region BuildFromDataTable 

		/// <summary>
		/// 从DataTable中创建树,  支持如下列：
		/// Text,Value,ImageUrl,NavigateUrl,Target,Checked,Expand,HasChildNodes
		/// </summary>
		/// <param name="t">数据源</param>
		/// <param name="parentId">根节点父ID</param>
		/// <param name="keyColumn">主键 列名</param>
		/// <param name="parentKeyColumn">父节点ID 列名</param>
		public void BuildFromDataTable( DataTable t , string parentId , string keyColumn , string parentKeyColumn )
		{
			this._ChildNodes.Clear() ;

			BuildNodesFromDataTable( t , this._ChildNodes , parentId , keyColumn , parentKeyColumn ) ;			
		}

		
		/// <summary>
		/// 递归产生节点
		/// </summary>
		/// <param name="t"></param>
		/// <param name="toNodes"></param>
		/// <param name="parentId"></param>
		/// <param name="keyColumn"></param>
		/// <param name="parentKeyColumn"></param>
		private void BuildNodesFromDataTable( DataTable t , TreeNodeCollection toNodes , string parentId , string keyColumn , string parentKeyColumn )
		{
			DataRow[] rows = t.Select( parentKeyColumn + "='"+parentId+"'" );

			//Page.Response.Write( keyColumn + "='"+parentId+"'" + rows.Length.ToString() );

			foreach( DataRow r in rows )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = r ;

				if( t.Columns.Contains( "Text" ) )
					n.Text = r["Text"].ToString() ;

				if( t.Columns.Contains( "Value" ) )
					n.Value = r["Value"].ToString() ;

				if( t.Columns.Contains( "ImageUrl" ) )
					n.ImageUrl = r["ImageUrl"].ToString() ;

				if( t.Columns.Contains( "Target" ) )
					n.Target = r["Target"].ToString() ;

				if( t.Columns.Contains( "NavigateUrl" ) )
					n.NavigateUrl = r["NavigateUrl"].ToString() ;

				if( t.Columns.Contains( "Checked" ) )
					n.Checked = Convert.ToBoolean( r["Checked"].ToString() ) ;

				if( t.Columns.Contains( "Disabled" ) )
					n.Disabled = Convert.ToBoolean( r["Disabled"].ToString() ) ;

				if( t.Columns.Contains( "Expand" ) )
					n.Expand = Convert.ToBoolean( r["Expand"].ToString() ) ; //

				if( t.Columns.Contains( "HasChildNodes" ) )
					n.HasChildNodes = Convert.ToBoolean( r["HasChildNodes"].ToString() ) ;

				toNodes.Add( n ) ;

				BuildNodesFromDataTable( t , n.ChildNodes , r[keyColumn].ToString() , keyColumn , parentKeyColumn ) ;
				
			}

		}

		#endregion

		#region BuildFromXml

		public void BuildFromXmlList( XmlDocument source  , string parentId , string keyAtt , string parentKeyAtt , string nameAtt , string valueAtt )
		{
			BuildNodesFromXmlList( source , this._ChildNodes , parentId , keyAtt , parentKeyAtt , nameAtt ,valueAtt  ) ;			
		}

		private void BuildNodesFromXmlList( XmlDocument source , TreeNodeCollection toNodes , 
			string parentId , string keyAtt , string parentKeyAtt , string nameAtt ,  string valueAtt )
		{

			XmlNodeList nodeList = source.SelectNodes( "*[@"+parentKeyAtt+"='" + parentId + "']" );

			foreach( XmlNode xn in nodeList )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = xn ;
				 
				n.Text = xn.Attributes[nameAtt].Value;
				n.Value = xn.Attributes[valueAtt].Value;

				if( xn.Attributes[ "ImageUrl" ] != null )
					n.ImageUrl = xn.Attributes["ImageUrl"].Value ;

				if( xn.Attributes[ "Target" ] != null )
					n.Target = xn.Attributes["Target"].Value ;

				if( xn.Attributes[ "NavigateUrl" ] != null )
					n.NavigateUrl = xn.Attributes["NavigateUrl"].Value ;

				if( xn.Attributes[ "EnableChecked" ] != null )
					n.EnableChecked = Convert.ToBoolean( xn.Attributes["EnableChecked"].Value );

				if( xn.Attributes[ "Checked" ] != null )
					n.Checked = Convert.ToBoolean( xn.Attributes["Checked"].Value );

				if( xn.Attributes[ "Disabled" ] != null )
					n.Disabled = Convert.ToBoolean( xn.Attributes["Disabled"].Value );

				if( xn.Attributes[ "Expand" ] != null )
					n.Expand = Convert.ToBoolean( xn.Attributes["Expand"].Value ) ;

				if( xn.Attributes[ "HasChildNodes" ] != null )  //
					n.HasChildNodes = Convert.ToBoolean( xn.Attributes["HasChildNodes"].Value );

				BuildNodesFromXmlList( source , n.ChildNodes , xn.Attributes[keyAtt].Value , keyAtt , parentKeyAtt , nameAtt , valueAtt ) ;

				toNodes.Add( n ) ;
			}
		}

		
//		public void BuildFromXmlTree( XmlDocument source , string nameAtt , string valueAtt )
//		{
//			BuildFromXmlTree( source.DocumentElement , this._ChildNodes , nameAtt ,valueAtt  ) ;			
//		}

		public void BuildFromXmlTree( XmlNode source , string nameAtt , string valueAtt )
		{
			BuildFromXmlTree( source , this._ChildNodes , nameAtt ,valueAtt  ) ;			
		}
		
		public void BuildFromXmlTree( XmlNodeList source , string nameAtt , string valueAtt )
		{
			foreach( XmlNode xn in source )
			{
				BuildFromXmlTree( xn , this._ChildNodes , nameAtt ,valueAtt  ) ;		
			}
		}

		public void BuildFromXmlTree( XmlNode source , TreeNodeCollection toNodes , string nameAtt ,  string valueAtt )
		{
			foreach( XmlNode xn in source.ChildNodes )
			{
				TreeNode n = new TreeNode(  );

				n.DataItem = xn ;
				 
				n.Text = xn.Attributes[nameAtt].Value;
				n.Value = xn.Attributes[valueAtt].Value;

				if( xn.Attributes[ "ImageUrl" ] != null )
					n.ImageUrl = xn.Attributes["ImageUrl"].Value ;

				if( xn.Attributes[ "Target" ] != null )
					n.Target = xn.Attributes["Target"].Value ;

				if( xn.Attributes[ "NavigateUrl" ] != null )
					n.NavigateUrl = xn.Attributes["NavigateUrl"].Value ;

				if( xn.Attributes[ "EnableChecked" ] != null )
					n.EnableChecked = Convert.ToBoolean( xn.Attributes["EnableChecked"].Value );

				if( xn.Attributes[ "Checked" ] != null )
					n.Checked = Convert.ToBoolean( xn.Attributes["Checked"].Value );

				if( xn.Attributes[ "Disabled" ] != null )
					n.Disabled = Convert.ToBoolean( xn.Attributes["Disabled"].Value );

				if( xn.Attributes[ "Expand" ] != null )
					n.Expand = Convert.ToBoolean( xn.Attributes["Expand"].Value ) ;

				if( xn.Attributes[ "HasChildNodes" ] != null )
					n.HasChildNodes = Convert.ToBoolean( xn.Attributes["HasChildNodes"].Value );

				BuildFromXmlTree( xn , n.ChildNodes , nameAtt , valueAtt ) ;

				toNodes.Add( n ) ;
			}
		}


		 


		#endregion
	}
}
