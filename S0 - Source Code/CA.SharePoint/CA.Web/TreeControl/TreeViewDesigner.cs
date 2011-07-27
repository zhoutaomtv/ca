using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.IO;

namespace CA.Web.TreeControl
{
	/// <summary>
	/// TreeView设计支持类
	/// </summary>
	public class TreeViewDesigner : System.Web.UI.Design.ControlDesigner
	{

		private TreeView _Tree = null ;

		public TreeViewDesigner()
		{
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component) 
		{
			_Tree = (TreeView)component;
			base.Initialize(component);
		}


		/// <summary>
		/// 获取设计时html
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml()
		{
			StringWriter sw = new StringWriter();

			HtmlTextWriter htw = new HtmlTextWriter(sw);

			if( _Tree.ChildNodes.Count != 0 )
			{
				_Tree.RenderControl( htw );
				return sw.ToString() ;
			}

			_Tree.RenderBeginTag( htw );

			htw.Write( "<b>"+_Tree.ID+"</b><br>  &nbsp; TreeNode1<br>  &nbsp; &nbsp; TreeNode2<br>   &nbsp; &nbsp; &nbsp; TreeNode3<br>" );

			_Tree.RenderEndTag( htw );

			return sw.ToString() ;
		}


		
	}


}
