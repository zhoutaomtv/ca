// --------------------------------------------------------------------
// - 版权所有  beyondbit.com
// - 作者：    张建义        Email:jianyi0115@163.com
// - 创建：    2005.10.18
// - 更改：
// - 备注：    
// --------------------------------------------------------------------

using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
 
namespace CA.Web
{
	
	///@ zhang jianyi 2005-10-18
	/// <summary>
	/// 分页控件----只提供显示逻辑，不涉及实际的数据分页操作
	/// 页面第一次加载时设置记录数，设置第一页显示，
	/// oninit中绑定PagerChanged事件，事件处理时按事件参数设置相应页。
	/// 提供多样式和模版设置功能
	///
	/// </summary>
	/// <example>
	/// 可以采用摸板控制控件的外观及选择相应显示功能，示例如下：
	/// <code>
	/// <![CDATA[
	/// 
	/// <bbit:tpager id="TPager1" runat="server" NumericButtonCount="3" RecordCount="100" EnableCookie="True"
	/// PageSizeOptions="10,20,30,40,50" BackColor="YellowGreen">
	/// <Template>
	///  记录总数：<%# Container.RecordCount %> 
	/// 总页数：<%# Container.PageCount %> 
	/// 每页记录数：<%# Container.PageSize %> 
	/// 当前第<%# Container.CurrentPageNumber %>页 |			
	/// <%# Container.PrePageLink %>
	/// <%# Container.NextPageLink %>  |
	/// <%# Container.NumericLinks %> |
	/// 转到第<%# Container.PageNumSelect %>页 | 
	/// 第<%# Container.PageNumInput %> | 
	/// 每页记录数：<%# Container.PageSizeChange %> | 
	/// </Template>
	/// </bbit:tpager>
	/// ]]>
	/// </code>
	/// </example>
	[DefaultProperty("PageSize"),
	DefaultEvent("PageIndexChanged"),
		ToolboxData("<{0}:TPager runat=server></{0}:TPager>")]
	[ParseChildren(true)]
	[Designer( typeof(TPagerDesigner) )]
	public class Pager : System.Web.UI.WebControls.WebControl , System.Web.UI.IPostBackEventHandler , System.Web.UI.IPostBackDataHandler
		, System.Web.UI.INamingContainer
	{

		#region 公共属性

		private bool enableCookie = true ;
		/// <summary>
		/// 是否启用cookie,若启用，则在整个站点保持PageSize同步
		/// </summary>
		[Description("是否启用cookie,若启用，则在整个站点保持PageSize同步"),
		Category("Behavior")]
		public bool EnableCookie
		{
			get {return enableCookie ; }
			set {enableCookie= value;}
		}

		private DisplayMode displayMode = DisplayMode.Always ;

		/// <summary>
		/// 显示模式:Always总是显示;AutoHidden当记录数为0时自动隐藏;AutoHiddenBeforePost当记录数为0且第一次加载时自动隐藏 
		/// </summary>
		[Description("显示模式:Always总是显示;AutoHidden当记录数为0时自动隐藏;AutoHiddenBeforePost当记录数为0且第一次加载时自动隐藏 "),
		Category("Behavior")]
		public DisplayMode DisplayMode
		{
			get {return displayMode ; }
			set {displayMode= value;}
		} 

		 
		private string cookieName = "Pager-PageSize";
		/// <summary>
		/// 保存每页记录数的cookie名
		/// </summary>
		[Description("保存每页记录数的cookie名"),
		Category("Behavior")]
		public string CookieName
		{
			get {return cookieName ; }
			set {cookieName= value;}
		}

		private HorizontalAlign horizontalAlign = HorizontalAlign.Center;
		/// <summary>
		/// 设置水平对齐方式
		/// </summary>
		[Description("设置水平对齐方式"),
			Category("Appearance"),
			DefaultValue(HorizontalAlign.Center)]
		public HorizontalAlign HorizontalAlign 
		{
			set
			{
				horizontalAlign = value ;
			}
			get
			{
				return horizontalAlign ;
			}
		}

		private string pageSizeOptions = "10,15,20,30";

		/// <summary>
		/// 可选每页记录数
		/// </summary>
		[Description("可选每页记录数"),Category("Behavior")]
		public string PageSizeOptions
		{
			get {return pageSizeOptions ; }
			set {pageSizeOptions= value;}
		}

		private int pageSize = 10 ;
		/// <summary>
		/// 设置或获取每页记录数
		/// </summary>
		[Description("设置或获取每页记录数"),Category("Behavior")]
		public int PageSize
		{
			set
			{ 
				this.pageSize = value;

				setPageCount() ;

				if( false == this.EnableCookie ) return ;

				HttpCookie cookie = new HttpCookie( CookieName );
				cookie.Name = CookieName ;
				cookie.Value = value.ToString() ;

				cookie.Expires = DateTime.Now.AddYears(1);

				Page.Response.Cookies.Add( cookie);
			}
			get
			{ 
				return this.pageSize; 
			}
		}

		private int pageCount = 0 ;
		/// <summary>
		/// 获取页数
		/// </summary>
		[Description("获取页数"),Category("Behavior")]
		public int PageCount
		{
			get
			{ 
				return this.pageCount; 
			}
		}
		
		private int currentPageIndex = 0 ;
		/// <summary>
		/// 设置或获取当前页索引（0开始）
		/// </summary>
		[Description("设置或获取当前页索引（0开始）"),Category("Behavior")]
		public int CurrentPageIndex
		{
			get {return currentPageIndex ; }
			set {currentPageIndex= value;}
		}

		private int recordCount = 0;
		/// <summary>
		/// 设置或获取记录数
		/// </summary>
		[Description("设置或获取记录数"),Category("Behavior")]
		public int RecordCount
		{
			get { return recordCount ;}
			set 
			{ 
				recordCount = value ;
				setPageCount() ;
			}
		}
		/// <summary>
		/// 重新计算页数
		/// </summary>
		private void setPageCount()
		{
			this.pageCount = this.recordCount / this.pageSize ;
			if( this.pageCount * this.pageSize < this.recordCount ) this.pageCount ++ ;
		}

		private TPagerMode mode = TPagerMode.Default ;
		/// <summary>
		/// 置或获取分页控件的样式，若有内部模版，则此属性失效
		/// </summary>
		[Description("设置或获取分页控件的样式，若有内部模版，则此属性失效"),Category("Behavior")]
		public TPagerMode Mode
		{
			get { return mode ;}
			set 
			{ 
				mode = value ;
			}
		}

		private string nextPageText = "下一页" ;
		/// <summary>
		/// 要用在"下一页"按钮上的文字
		/// </summary>
		[Description( @"要用在""下一页""按钮上的文字"),Category("Behavior")]
		public string NextPageText
		{
			get {return nextPageText ; }
			set {nextPageText= value;}
		}

		private string prePageText = "上一页" ;
		/// <summary>
		/// 要用在"上一页"按钮上的文字
		/// </summary>
		[Description( @"要用在""上一页""按钮上的文字"),Category("Behavior")]
		public string PrePageText
		{
			get {return prePageText ; }
			set {prePageText= value;}
		}


		private string nextNumericText = "..." ;
		/// <summary>
		/// 数字页码下一组文字
		/// </summary>
		[Description( @"数字页码下一组文字"),Category("Behavior")]
		public string NextNumericText
		{
			get {return nextNumericText ; }
			set {nextNumericText= value;}
		}

		private string preNumericText = "..." ;
		/// <summary>
		/// 数字页码上一组文字
		/// </summary>
		[Description( @"数字页码上一组文字"),Category("Behavior")]
		public string PreNumericText
		{
			get {return preNumericText ; }
			set {preNumericText= value;}
		}

		private string firstPageText = "..." ;
		/// <summary>
		/// 首页链接文字
		/// </summary>
		[Description( "首页"),Category("Behavior")]
		public string FirstPageText
		{
			get { return firstPageText ; }
			set { firstPageText= value;  } 
		}

		private string lastPageText = "..." ;
		/// <summary>
		/// 尾页链接文字
		/// </summary>
		[Description( "尾页"),Category("Behavior")]
		public string LastPageText
		{
			get { return lastPageText ; }
			set { lastPageText= value;}
		}

		private int pageButtonCount = 3 ;
		/// <summary>
		/// 分页用户界面中要显示的数字页码个数
		/// </summary>
		[Description( @"分页用户界面中要显示的数字页码个数"),Category("Behavior")]
		public int NumericButtonCount
		{
			get {return pageButtonCount ; }
			set {pageButtonCount= value;}
		}

		private string numericButtonFormat = "[{0}]" ;
		/// <summary>
		/// 数字页码格式
		/// </summary>
		[Description( @"数字页码格式"),Category("Behavior")]
		public string  NumericButtonFormat
		{
			get {return numericButtonFormat ; }
			set {numericButtonFormat= value;}
		}

//		private string numericButtonFormat = "[{0}]" ;
//		/// <summary>
//		/// 数字页码格式
//		/// </summary>
//		[Description( @"数字页码格式"),Category("Behavior")]
//		public string  PrePageLinkCssClass
//		{
//			get {return numericButtonFormat ; }
//			set {numericButtonFormat= value;}
//		}

		

		 
		#endregion 

		/// <summary>
		/// 分页事件代理
		/// </summary>
		public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

		/// <summary>
		/// 分页事件，页码改变或记录数将引发此事件
		/// </summary>
		public event PageChangedEventHandler PagerChanged;


		/// <summary>
		/// 处理分页事件
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPagerChanged(PageChangedEventArgs e)
		{
			if (PagerChanged != null)
				PagerChanged(this, e);
		}


		/// <summary>
		/// 控件初始化
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			//base.OnInit (e);
			Page.RegisterRequiresPostBack( this );

			if( this.EnableCookie && Page.Request.Cookies[ CookieName ] != null )
			{
				//Page.Response.Write( Page.Request.Cookies[CookieName].Value );

				this.pageSize = Convert.ToInt32( Page.Request.Cookies[CookieName].Value );

				this.setPageCount();
			}

		}

		private  ITemplate _template ;
		/// <summary>
		/// 控件模版
		/// </summary>
		[PersistenceMode( PersistenceMode.InnerProperty ),TemplateContainer( typeof(PagerTemplate) )]
		public ITemplate Template
		{
			set{ this._template = value ; }
			get{ return this._template ; }
		}
		
		/// <summary>
		/// 模版处理
		/// </summary>
		protected override void CreateChildControls()
		{
			//base.CreateChildControls ();

			if( this._template != null )
			{
				PagerTemplate temp = new PagerTemplate( this );

				this._template.InstantiateIn( temp );

				this.Controls.Add( temp ) ;
			}

			base.ChildControlsCreated = true ;

		}

		private bool _isBinded = false ;
		/// <summary>
		/// 绑定
		/// </summary>
		public override void DataBind()
		{
			base.EnsureChildControls() ;
			base.DataBind ();
			_isBinded = true ;
		}
	
		/// <summary>
		/// 控件呈现之前注册js
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			if( _isBinded == false )
				DataBind();

			base.OnPreRender (e);	

			string jsKey = "Pager-js";

			if( Page.IsClientScriptBlockRegistered( jsKey ) ) return ;

			string js = "<script language='javascript'>function Event(args){ __doPostBack('"+ this.ID +"' , args ) ; }</script>";

			Page.RegisterClientScriptBlock( jsKey , js ) ;
		}


		/// <summary>
		/// 将此控件呈现给指定的输出参数。
		/// </summary>
		/// <param name="output"> 要写出到的 HTML 编写器 </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.WriteLine( "<input type='hidden'name='" + this.UniqueID + "_PageArgs' value='" + 
				this.recordCount + "," + this.pageCount+ "," + this.currentPageIndex + ","+this.pageSize+"'>" );

			if( this.recordCount <= 0 ) //自动隐藏
			{
				if( displayMode == DisplayMode.AutoHidden )
					return ;
				else if( displayMode == DisplayMode.AutoHiddenBeforePost && false == Page.IsPostBack )
					return ;
			}

			if( this.Controls.Count != 0 ) //若存在模版
				base.Render( output );
			else
			{
				PagerTemplate temp = new PagerTemplate( this );

				base.RenderBeginTag( output ) ;
				output.Write( "<table align='"+this.horizontalAlign.ToString()+"'>" );
				output.Write( "<tr>" );
				if( this.Mode == TPagerMode.NextPrev )
				{
					output.Write( "<td valign=middle>" );
					output.Write( temp.PrePageLink + "&nbsp;" + temp.NextPageLink );
					output.Write( "</td>" );
				}
				else if( this.Mode == TPagerMode.NumericPages )
				{
					output.Write( temp.NumericLinks );
				}
				else if( this.Mode == TPagerMode.Standard )
				{
					output.Write( "<td valign=center align='left' width='40%' valign=middle nowrap >" );

					output.Write( string.Format( "当前页码：{0} 总页数：{1} 每页：{2} 总数：{3} " , this.CurrentPageIndex + 1 , this.pageCount ,this.pageSize ,this.recordCount  ) );

					output.WriteLine( " &nbsp; "  );

					output.WriteLine( "</td><td valign=center align='center' nowrap valign=middle>"  );
					output.Write( "【" + temp.PrePageLink + "】&nbsp;【" + temp.NextPageLink + "】" );

					output.WriteLine( "</td><td valign=center align='right' width='30%' nowrap valign=middle'>"  );
					output.WriteLine( temp.PageNumInput  );
					output.WriteLine( "</td>"  );
				}
				else if( this.Mode == TPagerMode.Default )
				{
					
					output.Write( "<td valign=center align='center' width='40%' valign=middle nowrap >" );

					output.Write( string.Format( "当前页码：{0} 总页数：{1} 每页：{2} 总数：{3} " , this.CurrentPageIndex + 1 , this.pageCount ,this.pageSize ,this.recordCount  ) );

					output.Write( "【" + temp.PrePageLink + "】&nbsp;【" + temp.NextPageLink + "】" );
					output.Write( "转到" );			
					output.Write( temp.PageNumSelect  );

					output.WriteLine( " &nbsp; "  );

					output.Write( "每页" );	
					output.Write( temp.PageSizeChange  );
				}

				output.Write( "</tr>" );
				output.Write( "</table>" );

				base.RenderEndTag( output ) ;

			}

		}

		#region IPostBackEventHandler 成员

		/// <summary>
		/// 处理回发事件
		/// </summary>
		/// <param name="eventArgument"></param>
		public void RaisePostBackEvent(string eventArgument)
		{
			// TODO:  添加 XPager.RaisePostBackEvent 实现

			//Page.Response.Write( eventArgument + currentPageIndex ) ;

			PageChangedEventArgs args = new PageChangedEventArgs();
			args.OldPageIndex = this.currentPageIndex ;

			args.RecordCount = this.recordCount;
			args.PageSize = this.PageSize ;

			if( eventArgument == "pre" )
			{
				args.NewPageIndex = args.OldPageIndex -1 ;
				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "next" )
			{
				args.NewPageIndex = args.OldPageIndex + 1 ;
				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "go-input" )
			{
				try
				{
					//args.NewPageIndex = Convert.ToInt32( this._sPageIndexFromInput ) - 1  ;

                    if (Int32.TryParse(this._sPageIndexFromInput, out args.NewPageIndex))
                        args.NewPageIndex = args.NewPageIndex - 1;
					
					if( args.NewPageIndex >= this.pageCount  )
						args.NewPageIndex = this.pageCount - 1;

					if( args.NewPageIndex < 0 ) args.NewPageIndex = 0 ;
				}
				catch
				{
					args.NewPageIndex = 0 ;
				}

				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "go-select" )
			{
				args.NewPageIndex = Convert.ToInt32( this._sPageIndexFromSelect ) - 1 ;
				args.EventType = PagerEventType.PageIndexChanged ;
			}
			else if( eventArgument == "ps" ) //页数该变
			{
				args.NewPageIndex = 0  ;
				//args.PageSize =  Convert.ToInt32( this._sPageSize ) ;
				//this.currentPageSize = args.PageSize ;
				args.EventType = PagerEventType.PageSizeChanged ;




			}
			else if( eventArgument == "pregroup" )
			{
				int start = ( this.CurrentPageIndex / this.NumericButtonCount ) * this.NumericButtonCount  ;
				args.NewPageIndex = start - this.NumericButtonCount  ;
 
				args.EventType = PagerEventType.PageSizeChanged ;
			}
			else if( eventArgument == "nextgroup" )
			{
				int start = ( this.CurrentPageIndex / this.NumericButtonCount ) * this.NumericButtonCount  ;
				args.NewPageIndex = start + this.NumericButtonCount  ;
 
				args.EventType = PagerEventType.PageSizeChanged ;
			}
			else
			{
				args.NewPageIndex =  Convert.ToInt32( eventArgument ) ;  ;
				args.EventType = PagerEventType.PageIndexChanged;
			}


			currentPageIndex = args.NewPageIndex ;

			//Page.Response.Write( eventArgument );

			this.OnPagerChanged( args ) ;
		}

		#endregion

		#region IPostBackDataHandler 成员
		
		/// <summary>
		/// 处理数据改变事件，空方法
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
		}

		private string _sPageSize ;
		private string _sPageIndexFromInput ;
		private string _sPageIndexFromSelect ;
 	
		/// <summary>
		/// 处理回发数据
		/// </summary>
		/// <param name="postDataKey"></param>
		/// <param name="postCollection"></param>
		/// <returns></returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			string args = postCollection[ this.UniqueID + "_PageArgs" ];
			if( args == null || args == "" ) return false ;

			string[] arr = args.Split( ',' );
			this.recordCount = int.Parse(arr[0]);
			this.pageCount = int.Parse(arr[1]);
			this.currentPageIndex = int.Parse(arr[2]);

            

			//this.pageSize = int.Parse(arr[3]);
			//Page.Response.Write( " LoadPostData" + currentPageIndex ) ;

			_sPageSize = postCollection[ this.ClientID + "_PageSize" ];
			_sPageIndexFromInput = postCollection[ this.ClientID + "_PageIndexFromInput" ];
            _sPageIndexFromSelect = postCollection[this.UniqueID + "_PageIndexFromSelect"];

			if( _sPageSize != null )   
				this.PageSize = Convert.ToInt32( _sPageSize );

			return false;
		}

		#endregion

		#region FilterByPager 帮助方法
		
		/// <summary>
		/// 取出当前页数据
		/// </summary>
		/// <param name="oldData"></param>
		/// <returns></returns>
		public DataTable FilterByPager( DataTable oldData )
		{
			return FilterByPager( oldData, this.pageSize , this.currentPageIndex ) ;
		}

		/// <summary>
		/// 取出当前页数据
		/// </summary>
		/// <param name="oldData">数据源</param>
		/// <param name="pSize">每页记录数</param>
		/// <param name="pIndex">开始索引</param>
		/// <returns></returns>
		public DataTable FilterByPager( DataTable oldData , int pSize , int pIndex )
		{
			if( oldData.Rows.Count <= pSize ) return oldData ;

			DataTable newData = new DataTable();
			foreach( DataColumn col in oldData.Columns )
			{
				newData.Columns.Add( new DataColumn( col.ColumnName , col.DataType ) );
			}

			int startIndex = 0 ;
			startIndex = pSize * pIndex ;
			if( startIndex > oldData.Rows.Count - 1 ) return newData ;

			for( ; startIndex < oldData.Rows.Count ; startIndex ++  )
			{
				if( newData.Rows.Count == pSize ) break;
				DataRow newRow = newData.NewRow();
				newData.Rows.Add( newRow );
				for( int i = 0 ; i < newData.Columns.Count ; i ++ )
				{
					newRow[i] = oldData.Rows[startIndex][i] ;
				}
			}
			return newData;
		}

		#endregion


	}
}
