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
	/// Ê÷Í¼³ÊÏÖÂß¼­»ùÀà
	/// </summary>
	internal abstract class TreeGraphRender
	{
		public TreeGraphRender( TreeGraph tree )
		{
			Control = tree ;
			//LineColorHtml = tree.LineColorHtml ;
			LineColorHtml = System.Drawing.ColorTranslator.ToHtml( tree.LineColor ) ;
		}

		protected TreeGraph Control ;

		protected string LineColorHtml = "blue";

		public abstract void Render(  HtmlTextWriter writer ) ;
	}
}
