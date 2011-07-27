// --------------------------------------------------------------------
// - 版权所有  beyondbit.com
// - 作者：    张建义        Email:jianyi0115@163.com
// - 创建：    2005.11.18
// - 更改：
// - 备注：    
//2009-7-31 LastPageLink bug
//2009-8-24 select 必须有value标签，否则跟UpdatePanel不兼容
// --------------------------------------------------------------------

using System;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web
{
	/// <summary>
	/// 分页控件模版，此控件所有属性都可以在TPager的模版中绑定
	/// </summary>
	public class PagerTemplate : Control , INamingContainer
	{
		private Pager _pager ;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pager"></param>
		public PagerTemplate( Pager pager )
		{	
			_pager = pager ;
		}

		/// <summary>
		/// 每页记录数
		/// </summary>
		public int PageSize
		{
			get
			{ 
				return _pager.PageSize; 
			}
		}
		/// <summary>
		/// 总页数
		/// </summary>
		public int PageCount
		{
			get
			{ 
				return _pager.PageCount; 
			}
		}
		/// <summary>
		/// 当前页码
		/// </summary>
		public int CurrentPageNumber
		{
			get {return _pager.CurrentPageIndex + 1 ; ; }
		}

		/// <summary>
		/// 总记录数
		/// </summary>
		public int RecordCount
		{
				get {return _pager.RecordCount ; }
		}
		
		/// <summary>
		/// 上一页链接
		/// </summary>
		public string PrePageLink
		{
			get
			{ 
				if( this.PageCount <= 1 )//只有一页
					return( "<a disabled>" + _pager.PrePageText + "</a>" );
				else if( _pager.CurrentPageIndex == 0 ) //多页时 第一页
					return( "<a disabled>" + _pager.PrePageText + "</a>" );
				else if( _pager.CurrentPageIndex == ( _pager.PageCount -1 ) ) //多页时 最后一页
					return( "<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager , "pre" ) + "\">" + _pager.PrePageText + "</a>" );
				else
					return( "<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager , "pre" ) + "\">" + _pager.PrePageText + "</a>"  ) ;

			}
		}

 
		/// <summary>
		/// 下一页链接
		/// </summary>
		public string NextPageLink
		{
			get
			{ 
				if( _pager.PageCount <= 1 )//只有一页
					return( "<a disabled>" + _pager.NextPageText + "</a>" );
				else if( _pager.CurrentPageIndex == 0 ) //多页时 第一页
					return( "<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager , "next" ) + "\">" + _pager.NextPageText + "</a>" );
				else if( _pager.CurrentPageIndex == ( _pager.PageCount -1 ) ) //多页时 最后一页
					return( "<a disabled>" + _pager.NextPageText + "</a>" );
				else
					return( "<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager , "next" ) + "\">" + _pager.NextPageText + "</a>" );
			}
		}


		/// <summary>
		/// 首页链接文字
		/// </summary>
		public string FirstPageLink
		{
			get 
			{ 
				if( _pager.PageCount <= 1  )//只有一页
					return( "<a>" + _pager.FirstPageText + "</a>" );
				else if( _pager.CurrentPageIndex == 0 ) //多页时 第一页
					return( "<a>" + _pager.FirstPageText + "</a>" );
				else
					return( "<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager , "0" ) + "\">" + _pager.FirstPageText + "</a>" );
			
			}
 		}

		/// <summary>
		/// 尾页链接文字
		/// </summary>
		public string LastPageLink
		{
			get
			{ 
				if( _pager.PageCount <= 1  )//只有一页
					return( "<a>" + _pager.LastPageText + "</a>" );
				else if( _pager.CurrentPageIndex == ( _pager.PageCount - 1 ) ) 
					return( "<a>" + _pager.LastPageText + "</a>" );
				else
					return( "<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager , (_pager.PageCount-1).ToString() ) + "\">" + _pager.LastPageText + "</a>" );
			
			}
		}
 
		/// <summary>
		/// 页码选择
		/// </summary>
		public string PageNumSelect
		{
			get
			{ 
				int num = this.CurrentPageNumber ;
				StringBuilder sb = new StringBuilder() ;
				sb.Append( "<select name='"+_pager.UniqueID+"_PageIndexFromSelect' onchange=\"" +  _pager.Page.GetPostBackEventReference( _pager  , "go-select" ) + "\">" );

//
//				if( _pager.CurrentPageIndex < _pager.PageSize ) //第一页
//				{
//
//				}
//				else if( _pager.CurrentPageIndex < _pager.PageSize ) //最后页
//				{
//
//				}

								
				for( int i = 1 ; i <= this.PageCount ; i ++ )
				{
					if( i == num )
						sb.Append( "<option value='"+i+"' selected>"+i+"</option>" );
					else
                        sb.Append("<option  value='" + i + "'>" + i + "</option>");
				}


				sb.Append( "</select>" );
				return sb.ToString() ;
				
 			}
		}
		
		/// <summary>
		/// 页码输入
		/// </summary>
		public string PageNumInput
		{
			get
			{ 
				
				return"<input onkeypress=\"return ( '0123456789'.indexOf(String.fromCharCode(event.keyCode))!=-1);\" type='text' style='width:20px;TEXT-ALIGN:center' width='20px' name='" + _pager.ClientID + 
					"_PageIndexFromInput'/><input type=button value='GO' onclick=\"" +  _pager.Page.GetPostBackEventReference( _pager  , "go-input" ) + "\"/>" ;
//
//				return"<input onkeypress='return onlyInt()' type='text' style='width:20;TEXT-ALIGN:center' name='" + _pager.ClientID + 
//					"_PageIndex'>页<input type=button value='GO' onclick=\"Event('go')\">" ;
				
			}
		}

		 
		/// <summary>
		/// 每页记录数选择
		/// </summary>
		public string PageSizeChange
		{
			get
			{ 
				string enabledPageSizeList = _pager.PageSizeOptions ; // "10,15,20,30";

				string[] arr = enabledPageSizeList.Split( ',' );

				string sPageSize = "" + _pager.PageSize ;

				StringBuilder sb = new StringBuilder();
				//sb.Append( "<select onchange=\"Event( 'PageSize='+this.selectedValue)\">" );
				sb.Append( "<select name='"+_pager.ClientID+"_PageSize' onchange=\"" +  _pager.Page.GetPostBackEventReference( _pager  , "ps" ) + "\">" );

				foreach( string s in arr )
				{
					if( sPageSize == s )
					{
						sb.Append( "<option selected>" + s + "</option>" );
					}
					else
						sb.Append( "<option>" + s + "</option>" );
				}

				sb.Append( "</select>" );
				return sb.ToString()  ; 
			}
		}

		/// <summary>
		/// 数字页码
		/// </summary>
		public string NumericLinks
		{
			get
			{
				if( _pager.PageCount == 1 ) return "1";

				int pageNum = this.CurrentPageNumber ;

				StringBuilder sb = new StringBuilder();

				if( _pager.PageCount <= _pager.NumericButtonCount )
				{
					for( int i = 0 ; i < _pager.PageCount  ; i ++ )
					{
						if( i == _pager.CurrentPageIndex )
							sb.Append(  "&nbsp;" + string.Format( _pager.NumericButtonFormat , i+1 ) );
						else
							sb.Append( "&nbsp;<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager  , i.ToString() ) + "\">" + string.Format( _pager.NumericButtonFormat , 1+i ) + "</a>" );
					}
				}
				else
				{
					if( this.CurrentPageNumber > _pager.NumericButtonCount  )
					{
						sb.Append( "&nbsp;<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager  , "pregroup" ) + "\">" + _pager.PreNumericText+ "</a>" );
					}

					int start = ( _pager.CurrentPageIndex / _pager.NumericButtonCount ) * _pager.NumericButtonCount + 1 ;
					int end = start + _pager.NumericButtonCount - 1 ;
					if( end > _pager.PageCount ) end = _pager.PageCount ;


					for( int i = start ; i <= end ; i ++ )
					{
						if( i == pageNum )
							sb.Append( "&nbsp;" + string.Format( _pager.NumericButtonFormat , i ) );
						else
							sb.Append( "&nbsp;<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager  , (i-1).ToString() ) + "\">" + string.Format( _pager.NumericButtonFormat , i ) + "</a>" );
					}

					if( end < this.PageCount )
					{
						sb.Append( "&nbsp;<a href=\"javascript:" +  _pager.Page.GetPostBackEventReference( _pager  , "nextgroup" ) + "\">" + _pager.NextNumericText+ "</a>" );
					}

				}

				return sb.ToString() ;

			}
		}



	}
}
