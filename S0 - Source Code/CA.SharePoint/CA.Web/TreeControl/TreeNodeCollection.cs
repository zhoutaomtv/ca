using System;
using System.Xml;
using System.Collections ;


namespace CA.Web.TreeControl
{
	/// <summary>
	/// 树节点集合
	/// </summary>
	public class TreeNodeCollection : CollectionBase
	{
		private TreeView _Owner ;
		private TreeNode _ParentNode = null ;

		public TreeNodeCollection()
		{

		}

		//private int _DepthCount ;
		public int DepthCount
		{
			get
			{
				//return _DepthCount;

				int count = this.Count ;
				foreach( TreeNode n in this )
				{
					count += n.ChildNodes.DepthCount ; 
				}
				return count ;
			}
            
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="parent"></param>
		public TreeNodeCollection( TreeView owner , TreeNode parent )
		{
			_Owner = owner ;
			_ParentNode = parent;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		public TreeNodeCollection( TreeNode parent )
		{
			_ParentNode = parent;
		}

		/// <summary>
		/// 按索引获取节点
		/// </summary>
		public  TreeNode this[ int index ]  
		{
			get  
			{
				return( ( TreeNode) List[index] );
			}

			set  { List[index] = value;}
		}

//		private void RaiseEvent( TreeNode node )
//		{
//			node.SetOwner( _Owner );
//			_Owner.OnTreeNodeCreated( node ) ;
//
//			foreach( TreeNode n in node.ChildNodes )
//			{
//				n.SetOwner( _Owner );
//				RaiseEvent( n ) ;
//			}
//		}

		/// <summary>
		/// 设置节点集合所属的TreeView
		/// </summary>
		/// <param name="owner"></param>
		internal void SetOwner( TreeView  owner )
		{
			_Owner = owner ;
		}

		/// <summary>
		/// 添加节点
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(  TreeNode value )  
		{
 			value.SetParentNode( this._ParentNode ) ;

			if( _Owner != null )
			{
				_Owner.OnTreeNodeCreated( value ) ;
			}



			return( List.Add( value ) );
		}

		/// <summary>
		/// 获取节点索引
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(  TreeNode value )  
		{
			return( List.IndexOf( value ) );
		}

		/// <summary>
		/// 插入节点
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert( int index,  TreeNode value )  
		{
			value.SetParentNode( this._ParentNode ) ;

			if( _Owner != null )
			{
				_Owner.OnTreeNodeCreated( value ) ;
			}

			List.Insert( index, value );
		}

		/// <summary>
		/// 删除节点
		/// </summary>
		/// <param name="value"></param>
		public void Remove(  TreeNode value )  
		{
			List.Remove( value );
		}

		public TreeNodeCollection SearchByParentId( string parentId )
		{
			TreeNodeCollection nodes = new TreeNodeCollection ();

			foreach( TreeNode n in this )
			{
				if( n.ParentId == parentId )
					nodes.Add( n ) ;
			}

			return nodes ;
		}

		public bool ExistById( string id )
		{
			TreeNodeCollection nodes = new TreeNodeCollection();

			foreach( TreeNode n in this )
			{
				if( n.Id == id )
					return true ;
			}

			return false ;
		}

		public bool ExistByParentId( string pid )
		{
			TreeNodeCollection nodes = new TreeNodeCollection ();

			foreach( TreeNode n in this )
			{
				if( n.ParentId == pid )
					return true ;
			}

			return false ;
		}

		/// <summary>
		/// 是否包含节点
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(  TreeNode value )  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != typeof(TreeNode) )
				throw new ArgumentException( "value must be of type  TreeNode.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != typeof(TreeNode) )
				throw new ArgumentException( "value must be of type  TreeNode.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != typeof(TreeNode) )
				throw new ArgumentException( "newValue must be of type  TreeNode.", "newValue" );

		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != typeof(TreeNode) )
				throw new ArgumentException( "value must be of type  TreeNode." );
		}



	}
}
