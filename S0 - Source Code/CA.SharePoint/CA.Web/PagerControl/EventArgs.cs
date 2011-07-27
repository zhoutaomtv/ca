// --------------------------------------------------------------------
// - 版权所有  beyondbit.com
// - 作者：    张建义        Email:jianyi0115@163.com
// - 创建：    2005.11.18
// - 更改：
// - 备注：    
// --------------------------------------------------------------------
using System;

namespace CA.Web
{

	/// <summary>
	/// 分页事件参数
	/// </summary>
	public class PageChangedEventArgs : EventArgs 
	{
		/// <summary>
		/// 上一页索引
		/// </summary>
		public int OldPageIndex;

		/// <summary>
		/// 新页索引
		/// </summary>
		public int NewPageIndex;
		
		/// <summary>
		/// 记录数
		/// </summary>
		public int RecordCount ;

		/// <summary>
		/// 每页记录数
		/// </summary>
		public int PageSize ;
		
		/// <summary>
		/// 分页事件类型
		/// </summary>
		public PagerEventType EventType = PagerEventType.PageIndexChanged ;
	}


	/// <summary>
	/// 分页事件类型
	/// </summary>
	public enum PagerEventType
	{
		/// <summary>
		/// 页码改变事件
		/// </summary>
		PageIndexChanged = 0 ,

		/// <summary>
		/// 每页记录数改变事件
		/// </summary>
		PageSizeChanged = 1 
	}
}
