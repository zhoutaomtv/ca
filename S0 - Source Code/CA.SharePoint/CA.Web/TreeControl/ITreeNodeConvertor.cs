using System;
using System.Collections;

namespace CA.Web.TreeControl
{
	/// <summary>
	/// ITreeNodeConvertor 的摘要说明。
	/// </summary>
	public interface ITreeNodeConvertor
	{
		TreeNode ToTreeNode( object dateItem );		 
	}

	public class TreeNodeBuilder
	{
		private ITreeNodeConvertor _c;
		public TreeNodeBuilder( ITreeNodeConvertor c ) 
		{
			_c = c ;
		}

		public void BuildNodes( TreeView tree , System.Collections.IEnumerable list )
		{
			BuildNodes( tree.ChildNodes , list ) ;
		}

		public void BuildNodes( TreeNodeCollection nodes  , System.Collections.IEnumerable list )
		{
			TreeNodeCollection tempnodes = new TreeNodeCollection ();

			System.Collections.IEnumerator lt = list.GetEnumerator();

			while( lt.MoveNext() )
			{
				object o = lt.Current ;

				tempnodes.Add( _c.ToTreeNode( o ) );
			}

			TreeNodeCollection roots = this.GetRootNodes( tempnodes ) ;

			foreach( TreeNode n in roots )
			{
				nodes.Add( n );
				this.build( tempnodes , n.ChildNodes , n ) ;
			}
		}

		private TreeNodeCollection GetRootNodes( TreeNodeCollection nodes )
		{
			TreeNodeCollection tempnodes = new TreeNodeCollection ();

			foreach( TreeNode n in nodes )
			{
				if( nodes.ExistByParentId( n.Id ) )
					tempnodes.Add(n);
			}

			return tempnodes ;
		}

		private void build( TreeNodeCollection from , TreeNodeCollection to , TreeNode parent )
		{
			TreeNodeCollection childNodes = from.SearchByParentId( parent.Id ) ;

			foreach( TreeNode n in childNodes )
			{
				to.Add(n);
				this.build( from , n.ChildNodes , n ) ;
			}
		}
	
	}
}
