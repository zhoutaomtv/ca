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
	/// 下拉列表树　
	/// 2006-3-20 
	/// </summary>
	public class TreeDropDownList : System.Web.UI.WebControls.DropDownList
	{
		public TreeDropDownList()
		{

		}

		public object RootId ;
		public string KeyField = "ID" ;
		public string ParentKeyField = "ParentID";


		private bool _ShowLines = true ; 
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

//		private TreeNodeCollection _ChildNodes ;  
//		/// <summary>
//		/// 子节点集合
//		/// </summary>
//		public TreeNodeCollection ChildNodes
//		{
//			{
//				return _ChildNodes ;
//			}
//		}



		private delegate void BuildItemsHandler( DataTable t , string parentId , string prefix );


		private BuildItemsHandler BuildItemsMethod ;


		public override void DataBind()
		{
			if( this.DataSource == null ) return ;

			if( this.ShowLines )
				BuildItemsMethod = new BuildItemsHandler( BuildLineItems );
			else
				BuildItemsMethod = new BuildItemsHandler( BuildItems );
			 
				if( this.RootId != null )
					BuildItemsMethod( DataSource as DataTable , this.RootId.ToString() , ""  ) ;
				else
				{
					DataTable t = DataSource as DataTable ;

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
						BuildItemsMethod( t , rootId , ""  ) ;
					}
				}
		}

		private void BuildLineItems( DataTable t , string parentId , string prefix )
		{
			DataRow[] rows = t.Select( this.ParentKeyField + "='"+parentId+"'" );

			if( rows.Length <= 0 ) return ;

			int lastButOneIndex = rows.Length - 1 ;

			for( int i = 0 ; i < lastButOneIndex ; i ++ )
			{
				ListItem item = new ListItem();
			
				item.Text = prefix + "|-" + rows[i][ this.DataTextField ].ToString() ;
				 
				item.Value = rows[i][ this.DataValueField ].ToString() ;

				Items.Add( item ) ;

				BuildLineItems( t , rows[i][ this.KeyField ].ToString() , prefix + "|　" ) ;
			}

			//生成最后一个
			ListItem lastItem = new ListItem();
			
			lastItem.Text = prefix + "|_" + rows[lastButOneIndex][ this.DataTextField ].ToString() ;
				 
			lastItem.Value = rows[lastButOneIndex][ this.DataValueField ].ToString() ;

			Items.Add( lastItem ) ;

			BuildLineItems( t , rows[lastButOneIndex][ this.KeyField ].ToString() , prefix + "　 " ) ;
		}

		private void BuildItems( DataTable t , string parentId , string prefix )
		{
			DataRow[] rows = t.Select( this.ParentKeyField + "='"+parentId+"'" );

			if( rows.Length <= 0 ) return ;

			int lastIndex = rows.Length ;

			for( int i = 0 ; i < lastIndex ; i ++ )
			{
				ListItem item = new ListItem();
			
				item.Text = prefix  + rows[i][ this.DataTextField ].ToString() ;
				 
				item.Value = rows[i][ this.DataValueField ].ToString() ;

				Items.Add( item ) ;

				BuildItems( t , rows[i][ this.KeyField ].ToString() , prefix + "　" ) ;
			}

		}


//		private void BuildNodes( DataTable t , TreeNodeCollection toNodes , object parentId )
//		{
//			DataRow[] rows = t.Select( this.ParentKeyField + "='"+parentId+"'" );
//
//			//Page.Response.Write( keyColumn + "='"+parentId+"'" + rows.Length.ToString() );
//
//			foreach( DataRow r in rows )
//			{
//				TreeNode n = new TreeNode(  );
//
//				n.DataItem = r ;
//			
//				n.Text = r[ this.DataTextField ].ToString() ;
//				 
//				n.Value = r[ this.DataValueField ].ToString() ;
//
//				toNodes.Add( n ) ;
//
//				BuildNodes( t , n.ChildNodes , r[ this.KeyField ] ) ;
//			}
//
//		}

	}
}
