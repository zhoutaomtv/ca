using System;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.TreeControl
{
	/// <summary>
	///树节点 
	/// </summary>
	[ParseChildren( true , "ChildNodes" )]
	public class TreeNode // : System.Web.UI.WebControls.WebControl
	{

		/// <summary>
		/// 
		/// </summary>
		public TreeNode()
		{
			_ChildNodes = new TreeNodeCollection( _Owner , this )   ;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">节点值</param>
		/// <param name="text">节点显示文本</param>
		public TreeNode( string value , string text ) : this()
		{
			this._Value = value ;
			this._Text = text ;		
		}

		private TreeView  _Owner = null ;
		/// <summary>
		/// 获取父节点
		/// </summary>
		public TreeView  Owner
		{
			get{ return _Owner ;}
		}

		/// <summary>
		/// 设置节点所属的TreeView
		/// </summary>
		/// <param name="owner"></param>
		internal void SetOwner( TreeView  owner )
		{
			_Owner = owner ;
			_ChildNodes.SetOwner( owner ) ;
		}


		private TreeNode  _ParentNode = null ;
		/// <summary>
		/// 获取父节点
		/// </summary>
		public TreeNode  ParentNode
		{
			//set{ _ParentNode = value ; }
			get{ return _ParentNode ;}
		}

		/// <summary>
		/// 设置父节点
		/// </summary>
		/// <param name="parentNode"></param>
		internal void SetParentNode( TreeNode  parentNode )
		{
			_ParentNode = parentNode ;
		}


		//public string Id ;


 		private string _ImageUrl ; // = "images/tree/top.gif" ;
		/// <summary>
		/// 节点图片路径
		/// </summary>
		public string ImageUrl
		{
			set{ _ImageUrl = value ; }
			get{ return _ImageUrl ;}
		}

		private string _Text ;
		/// <summary>
		/// 节点显示文本
		/// </summary>
		public string Text
		{
			set{ _Text = value ; }
			get{ return _Text ;}
		}

		private string _ToolTip ;
		/// <summary>
		/// 节点显示文本
		/// </summary>
		public string ToolTip
		{
			set{ _ToolTip = value ; }
			get{ return _ToolTip ;}
		}

//		private string _OnAsyncLoaded;
//		/// <summary>
//		/// 节点异步加载子节点后触发的客户端js函数 OnAsyncLoadComplete
//		/// </summary>
//		public string OnAsyncLoadComplete 
//		{
//			set{ _OnAsyncLoaded = value ; }
//			get{ return _OnAsyncLoaded ;}
//		}
//
//		public string OnAsyncLoad ;


		private string _Value = "" ;
		/// <summary>
		/// 节点的值
		/// </summary>
		public string Value
		{
			set{ _Value = value ; }
			get{ return _Value ;}
		}

		private bool _EnableChecked = false ;
		/// <summary>
		/// 是否允许选择
		/// </summary>
		public bool EnableChecked
		{
			set{ _EnableChecked = value ; }
			get{ return _EnableChecked ;}
		}

		private bool _Disabled = false ;
		//internal string DisabledHtml = "" ;
		/// <summary>
		/// 是否允许选择
		/// </summary>
		public bool Disabled
		{
			set{ _Disabled = value ; }
			get{ return _Disabled ;}
		}

		//internal string CheckedHtml = "" ;
		private bool _Checked = false ;
		/// <summary>
		/// 是否选中,若设置此属性，则自动关联EnableChecked
		/// </summary>
		public bool Checked
		{
			set{ 
				
				EnableChecked = true ;
				_Checked = value ;

//				if( value )
//					CheckedHtml = "checked";
//				else
//					CheckedHtml = "";
			
			}
			get{ 
				
				return _Checked ;
			}
		}

		private string _CssClass ;
		/// <summary>
		/// 节点样式
		/// </summary>
		public string CssClass
		{
			set{ _CssClass = value ; }
			get{ return _CssClass ;}
		}


		private string _NavigateUrl ;
		/// <summary>
		/// 节点链接
		/// </summary>
		public string NavigateUrl
		{
			set{ _NavigateUrl = value ; }
			get{ return _NavigateUrl ;}
		}


		internal string TargetHtml = "" ;
		private string _Target ;
		/// <summary>
		/// 节点链接目标框架
		/// </summary>
		public string Target
		{
			set
			{ 
				_Target = value ; 
				TargetHtml = "target='"+value+"'";
			}
			get{ return _Target ;}
		}

		internal string ExpandHtml = "style='display:none'" ;
		private bool _Expand = false ;

		/// <summary>
		/// 节点是否展开
		/// </summary>
		public bool Expand
		{
			set
			{ 
				_Expand = value ; 
				if( value )
				{
					ExpandHtml = "style=''" ;

				//	if( this._ParentNode != null )    //确保节点一定会展开
				//		this._ParentNode.Expland = true ;
				}
				else
					ExpandHtml = "style='display:none'" ;		
			}
			get
			{ 
				return _Expand ;
			}
		}

//		private bool _IsLastNode ;
//		public bool IsLastNode
//		{
//			set{ _IsLastNode = value ; }
//			get{ return _IsLastNode ;}
//		}


		private bool _HasChildNodes = false ;  

		/// <summary>
		/// 是否存在自节点,异步加载时必须设置此属性为true才允许节点加载数据,
		/// </summary>
		public bool HasChildNodes
		{
			set
			{ 
				_HasChildNodes = value ;
			}
			get
			{ 
				return _HasChildNodes ;
			}
		}

		private string _OnCheckBoxClick ;
		public string OnCheckBoxClick
		{
			set
			{ 
				_OnCheckBoxClick = value ;
			}
			get
			{ 
				return _OnCheckBoxClick ;
			}
		}

		/// <summary>
		/// 数据绑定时节点对应的数据行 DataRow 或 XmlNode
		/// </summary>
		public object DataItem ;

		public string Id;
		public string ParentId ;

		private TreeNodeCollection _ChildNodes ;  
		/// <summary>
		/// 子节点集合
		/// </summary>
		public TreeNodeCollection ChildNodes
		{
//			set
//			{ 
//				_ChildNodes = value ; 
//			}
			get
			{
				return _ChildNodes ;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ownerDocument"></param>
		/// <returns></returns>
		public XmlNode ToXml( XmlDocument ownerDocument )
		{
			XmlNode node = ownerDocument.CreateElement( "Node" ); 

			XmlAttribute att ;

			att = ownerDocument.CreateAttribute( "Value" );
			att.Value = this._Value ;
			node.Attributes.Append( att ) ;

			att = ownerDocument.CreateAttribute( "Text" );
			att.Value = this._Text ;
			node.Attributes.Append( att ) ;

			att = ownerDocument.CreateAttribute( "NavigateUrl" );
			att.Value = this._NavigateUrl ;
			node.Attributes.Append( att ) ;

			att = ownerDocument.CreateAttribute( "Target" );
			att.Value = this._Target ;
			node.Attributes.Append( att ) ;			

			return node ;
		}

		
	}
}
