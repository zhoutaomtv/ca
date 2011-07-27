// --------------------------------------------------------------------
// - 版权所有  beyondbit.com
// - 作者：    张建义        Email:jianyi0115@163.com
// - 创建：    2005.11.18
// - 更改：
// - 备注：    
// --------------------------------------------------------------------

using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.IO;

namespace CA.Web
{
	/// <summary>
	/// TPagerDesigner 的摘要说明。
	/// </summary>
	public class TPagerDesigner : System.Web.UI.Design.ControlDesigner
	{
		public TPagerDesigner()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		private Pager _pager ;

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component) 
		{
			_pager = (Pager)component;
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

			_pager.DisplayMode = DisplayMode.Always ; //确保设计模式下控件始终显示

			_pager.RenderControl( htw );
			return sw.ToString() ;
			 
		}
	}
}
