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
	/// 分页控件模式
	/// </summary>
	public enum TPagerMode
	{
		 
		/// <summary>
		/// 默认
		/// </summary>
		Default = 0,
 
		/// <summary>
		/// 最小
		/// </summary>
		NextPrev = 1,

		/// <summary>
		/// 数据链接模式
		/// </summary>
		NumericPages = 2 ,

	    //启用所有功能
		//Advanced = 3 ,
		
		/// <summary>
		/// 标准控件
		/// </summary>
		Standard = 3 ,

	}

	public enum DisplayMode
	{
		Always = 0 ,

		AutoHidden = 1 ,

		AutoHiddenBeforePost = 2




	}
}
