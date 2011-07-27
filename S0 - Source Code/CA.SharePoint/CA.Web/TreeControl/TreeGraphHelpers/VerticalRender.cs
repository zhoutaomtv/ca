using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.TreeControl.TreeGraphHelpers
{
	/// <summary>
	/// 树图竖直呈现
	/// </summary>
	internal class VerticalRender : TreeGraphRender
	{

		private string NodeExtendTag = "" ;
		public VerticalRender( TreeGraph tree ) : base( tree )
		{
			if( tree.NodeRegionCssClass == null )
			{
				NodeExtendTag = " style='BORDER-RIGHT: "+this.LineColorHtml+" 1px solid; BORDER-TOP: "+this.LineColorHtml+
					" 1px solid; BORDER-LEFT: "+this.LineColorHtml+" 1px solid; WIDTH: 100px; BORDER-BOTTOM: "+this.LineColorHtml+" 1px solid; HEIGHT: 20px;'";
			}
			else
			{
				NodeExtendTag = "class='" + tree.NodeRegionCssClass + "'";
			}

			LineHtml = string.Format( LineHtml , tree.LineLength.ToString() , LineColorHtml ) ;
		}

		public override void Render(HtmlTextWriter writer)
		{
			if( Control.ChildNodes.Count == 0 ) //单根时
			{
				return ;
			}
			else if( Control.ChildNodes.Count == 1 ) //单根时
			{
				this.RenderRootNode( this.Control.ChildNodes[0] , writer ) ;
			}
			else //多根
			{
				RenderRootNodes( this.Control.ChildNodes , writer ) ; 
			}	
		}
	
		private void RenderRootNode( TreeNode n ,  HtmlTextWriter writer )
		{
			writer.WriteLine( "<table border=0 cellspacing=0 cellpadding=0 >" );// border='1' bordercolor='#0030b0' cellspacing='0'

			writer.Write( "<tr>" );
			writer.Write( "<td align='center'>" );

			RenderNode( n , writer ) ; //先输出根节点

			writer.Write( "</td>" );
			writer.Write( "</tr>" );

			//children
			writer.Write( "<tr>" );
			writer.Write( "<td align='center'>" );
		
			if( n.ChildNodes.Count > 0 )
				RenderNodesTop( n.ChildNodes , writer ) ; //输出子节点

			writer.Write( "</td>" );
			writer.Write( "</tr>" );
			writer.WriteLine( "</table>" ) ;
		}


		//输出同级节点前面连接线
		private void RenderNodesTop( TreeNodeCollection nodes ,  HtmlTextWriter writer )
		{
			writer.WriteLine( "<table border=0 cellspacing=0 cellpadding=0 width='100%'>" );// border='1' bordercolor='#0030b0' cellspacing='0'

			writer.Write( "<tr>" );

			RendeLine( writer ) ;

			writer.Write( "</tr>" );

			writer.Write( "<tr>" );
			
			writer.Write( "<td>" );

			//children
			RenderNodes( nodes , writer ) ;

			writer.Write( "</td>" );

			writer.Write( "</tr>" );
			
			writer.WriteLine( "</table>" ) ;
		}

		//输出节点内容
		protected void RenderNode( TreeNode n ,    HtmlTextWriter writer )
		{
			writer.WriteLine( "<table border=0 cellspacing=3 cellpadding=0 height='100%' >" );
			writer.Write( "<td " );
			writer.Write( NodeExtendTag );
			writer.Write( ">" );

            if (String.IsNullOrEmpty(n.NavigateUrl))
            {
                writer.Write(n.Text);
            }
            else
            {
                writer.Write("<a href='");
                writer.Write(n.NavigateUrl);
                writer.Write("' target='");
                writer.Write(n.Target);
                writer.Write("' title='");
                writer.Write(n.ToolTip);
                writer.Write("' >");
                writer.Write(n.Text);
                writer.Write("</a>");
            }

			writer.Write( "</td>" );
			writer.WriteLine( "</table>" );
		}

		private string LineHtml = "<td align='center' valign='middle' height='{0}'><table valign='bottom' bgcolor='{1}' border='0' cellpadding='0' cellspacing='0' height='100%' width='1'><tbody><tr><td></td></tr></tbody></table></td>";

		private void RendeLine(  HtmlTextWriter writer )
		{
//			writer.Write( "<td align='left' valign='middle' width='20'>" );
//
//			//line
//			writer.Write( "<table valign='bottom' bgcolor='"+ LineColorHtml +"' border='0' cellpadding='0' cellspacing='0' height='1' width='100%'><tbody><tr><td></td></tr></tbody></table>" ) ;	
//
//			writer.Write( "</td>" );

			writer.Write( this.LineHtml );
		}
		
		//输出同级节点
		private void RenderNodes( TreeNodeCollection nodes ,  HtmlTextWriter writer )
		{
			if( nodes.Count == 0 )
			{
				return ;
			}
			if( nodes.Count == 1 )
			{
				RenderSingleNodes( nodes , writer ) ;
			}
			else
			{
				RenderMultiNodes( nodes , writer ) ; 
			}
			
		}

		private void RenderRootNodes( TreeNodeCollection nodes ,  HtmlTextWriter writer )
		{
			writer.WriteLine( "<table border=0 cellspacing=0 cellpadding=0 >" ); 

			//输出线
			writer.Write( "<tr height=1>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				if( i == 0 )
				{
					writer.Write( "<td height='1' align='right'>" );
					writer.Write( "<table border='0' cellpadding='0' cellspacing='0' width='100%' height='1'><tr><td width='50%'></td><td width='50%' bgcolor='"+ LineColorHtml +"'></td></tr></table>" );
				}
				else if( i == nodes.Count - 1 )
				{
					writer.Write( "<td height='1' valign='left'>" );
					writer.Write( "<table bgcolor='"+ LineColorHtml +"' border='0' cellpadding='0' cellspacing='0' width='50%' width='1'><tbody><tr><td></td></tr></tbody></table>" );
				}
				else
				{
					writer.Write( "<td height='1'>" );
					writer.Write( "<table bgcolor='"+ LineColorHtml +"' border='0' cellpadding='0' cellspacing='0' width='100%' width='1'><tbody><tr><td></td></tr></tbody></table>" );
				}

				writer.Write( "</td>" );	
			}

			writer.Write( "</tr>" );

			//连接线
			writer.Write( "<tr height=20>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				RendeLine( writer ) ;
			}
			writer.Write( "</tr>" );

			//nodes
			writer.Write( "<tr>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				writer.Write( "<td align='center'>" );
				
				//writer.Write( nodes[i].Text );
				RenderNode( nodes[i] , writer ) ;
				
				writer.Write( "</td>" );
			}
			writer.Write( "</tr>" );

			//children
			writer.Write( "<tr>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				writer.Write( "<td>" );
				//
				if( nodes[i].ChildNodes.Count > 0 )
					RenderNodesTop( nodes[i].ChildNodes , writer ) ;
				
				writer.Write( "</td>" );
				
			}
			writer.Write( "</tr>" );

			writer.WriteLine( "</table>" ) ; 

		}


		private void RenderMultiNodes( TreeNodeCollection nodes ,  HtmlTextWriter writer )
		{
			writer.WriteLine( "<table border=0 cellspacing=0 cellpadding=0 width='100%'>" ); 

			//输出线
			writer.Write( "<tr height=1>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				if( i == 0 )
				{
					writer.Write( "<td height='1' align='right'>" );
					writer.Write( "<table border='0' cellpadding='0' cellspacing='0' width='100%' height='1'><tr><td width='50%'></td><td width='50%' bgcolor='"+ LineColorHtml +"'></td></tr></table>" );
				}
				else if( i == nodes.Count - 1 )
				{
					writer.Write( "<td height='1' align='left'>" );
					writer.Write( "<table bgcolor='"+ LineColorHtml +"' border='0' cellpadding='0' cellspacing='0' width='50%' width='1'><tbody><tr><td></td></tr></tbody></table>" );
				}
				else
				{
                    writer.Write("<td height='1' align='left'>");
					writer.Write( "<table bgcolor='"+ LineColorHtml +"' border='0' cellpadding='0' cellspacing='0' width='100%' width='1'><tbody><tr><td></td></tr></tbody></table>" );
				}

				writer.Write( "</td>" );	
			}

			writer.Write( "</tr>" );

			writer.Write( "<tr height=20>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				RendeLine( writer ) ;
			}
			writer.Write( "</tr>" );

			//nodes
			writer.Write( "<tr>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				writer.Write( "<td align='center'>" );
				
				//writer.Write( nodes[i].Text );
				RenderNode( nodes[i] , writer ) ;
				
				writer.Write( "</td>" );
			}
			writer.Write( "</tr>" );

			//children
			writer.Write( "<tr>" );
			for( int i = 0 ; i < nodes.Count ; i ++ )
			{
				writer.Write( "<td align='center'>" );
				//
				if( nodes[i].ChildNodes.Count > 0 )
					RenderNodesTop( nodes[i].ChildNodes , writer ) ;
				
				writer.Write( "</td>" );
				
			}
			writer.Write( "</tr>" );

			writer.WriteLine( "</table>" ) ; 
		}


		private void RenderSingleNodes( TreeNodeCollection nodes ,  HtmlTextWriter writer )
		{
			writer.WriteLine( "<table border=0 cellspacing=0 cellpadding=0 width='100%'>" ); 

			//输出线
			
			writer.Write( "<tr height=20>" );
		
				RendeLine( writer ) ;
			
			writer.Write( "</tr>" );

			//nodes
			writer.Write( "<tr>" );
		
				writer.Write( "<td align='center'>" );
				
				//writer.Write( nodes[i].Text );
				RenderNode( nodes[0] , writer ) ;
				
				writer.Write( "</td>" );
			
			writer.Write( "</tr>" );

			//children
			writer.Write( "<tr>" );
		
				writer.Write( "<td align='center'>" );
				//
				if( nodes[0].ChildNodes.Count > 0 )
					RenderNodesTop( nodes[0].ChildNodes , writer ) ;
				
				writer.Write( "</td>" );
				
			
			writer.Write( "</tr>" );

			writer.WriteLine( "</table>" ) ; 
		}
	
		

		 
	}
}
