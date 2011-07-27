using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

[assembly: WebResource("DateTimePicker.js", "application/x-javascript")]
namespace CA.Web.TimeControl
{
	///@2005-5-27 jianyi
	/// <summary>
	/// 
	/// 时间选择控件 
	/// 
	/// </summary>
	[DefaultProperty("Value"),
	ToolboxData("<{0}:DateTimePicker runat=server></{0}:DateTimePicker>")]
	public class DateTimePicker : System.Web.UI.WebControls.WebControl ,  System.Web.UI.IPostBackDataHandler , INamingContainer
	{
		public DateTimePicker(){}

		private bool _IsContainer = false ;
		public DateTimePicker( bool isContainer ){ _IsContainer = isContainer ; }
	
		private DateTime _value  ;

		private string _dateStyle = null ;
		private string _hourStyle = null ;
		private string _minuteStyle = null ;

		private string _dateCssClass = null ;
		private string _hourCssClass = null ;
		private string _minuteCssClass = null ;

		/// <summary>
		/// 日期框样式
		/// </summary>
		[Browsable(true),Category("Appearance")]
		[Description("日期框样式")]
		public string DateStyle
		{
			set
			{
				_dateStyle = value ;
			}
		}


		/// <summary>
		/// 小时下拉列表样式
		/// </summary>
		[Browsable(true),Category("Appearance")]
		[Description("小时下拉列表样式")]
		public string HourStyle
		{
			set
			{
				_hourStyle = value ;
			}
		}
		/// <summary>
		/// 分钟下拉列表样式
		/// </summary>
		[Browsable(true),Category("Appearance")]
		[Description("分钟下拉列表样式")]
		public string MinuteStyle
		{
			set
			{
				_minuteStyle = value ;
			}
		}

		/// <summary>
		/// 日期框样式类
		/// </summary>
		[Browsable(true),Category("Appearance")]
		[Description("")]
		public string DateCssClass
		{
			set
			{
				_dateCssClass = value ;
			}
		}
		/// <summary>
		/// 小时下拉列表样式类
		/// </summary>
		[Browsable(true),Category("Appearance")]
		[Description("")]
		public string HourCssClass
		{
			set
			{
				_hourCssClass = value ;
			}
		}
		/// <summary>
		/// 分钟下拉列表样式类
		/// </summary>
		[Browsable(true),Category("Appearance")]
		[Description("")]
		public string MinuteCssClass
		{
			set
			{
				_minuteCssClass = value ;
			}
		}

		private bool _timeVisible = true ; 
		/// <summary>
		/// 是否显示时间部分
		/// </summary>
		[Browsable(true)]
		[Description("是否显示时间部分")]
		public bool TimeVisible
		{
			set
			{
				_timeVisible = value ;
			}
			get
			{
				return _timeVisible ;
			}
		}

		/// <summary>
		/// 自动将值设为当前时间
		/// </summary>
		[Description("自动将值设为当前时间")]
		public bool AutoInitValue
		{
			set
			{
				_value = DateTime.Now ;
			}
		}



        private string _function = "ShowCal(this)"; //DateTimePicker_setday
		private bool _ClientFunctionIsSet = false ;
		/// <summary>
		/// 自定义客户端Onclick函数
		/// </summary>
		[Browsable(true),Description("自定义客户端Onclick函数")]
		public string ClientFunction
		{
			set
			{
				_function = value ;
				_ClientFunctionIsSet = true ;
			}
		}

		private bool _isValueSeted = false ; //是否设置值
		/// <summary>
		/// 获取或设置日期值
		/// </summary>
		[Bindable(true),
		Category("Data")]
		public DateTime Value
		{
			get
			{
				return _value;
			}

			set
			{
				_value = value;
				_isValueSeted = true ;
			}
		}

		private string _clientId
		{
			get
			{
				return this.ClientID ;
			}
		}

		/// <summary>
		/// 注册js
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			string clientJsKey = "DateTimePicker-ClientJs";

			if( _ClientFunctionIsSet )
			{

			}
			else  //若为设置子定义函数,则输出默认函数
			{
				//_function = "DateTimePicker_setday(this)" ;
				//_function = "OpenDate("+this.ClientID+")" ;

                Page.ClientScript.RegisterClientScriptResource(this.GetType(), "DateTimePicker.js");

				if( false == Page.IsClientScriptBlockRegistered( clientJsKey ) )
				{
					//Page.RegisterClientScriptBlock( clientJsKey , ResourceHelper.GetJsResourceString( this.GetType() ) ) ;
				}
			}

			DataBind();
 
			
		}

		private ITemplate _Template;
		[PersistenceMode( PersistenceMode.InnerProperty ),TemplateContainer( typeof(DateTimePicker) )]
		public ITemplate Template
		{
			set{ _Template = value ; }
			get{ return _Template; } 
		}


		protected override void CreateChildControls()
		{
			//base.CreateChildControls ();

			this.Controls.Clear();

			if( this._Template != null )
			{
				//DateTimePicker p = new DateTimePicker(  );
				_Template.InstantiateIn( this );
				//this.Controls.Add(p);
			}

			base.ChildControlsCreated = true ;
		}

		static string[] _minuteSetting = new string[]{ "00" , "05" , "10" ,"15" ,"20" ,"25" ,"30" ,  "35" , "40" , "45" , "50" , "55" } ;

		public string Year
		{
			get
			{
				string html ;
				if( _value == DateTime.MinValue )
					html = ( "<input onclick=\""+ _function +"\" name=\"" + _clientId + "\" type=\"text\" value=\"\" readonly " );
				else
					html = ( "<input onclick=\""+ _function +"\" name=\"" + _clientId + "\" type=\"text\" value=\"" + _value.ToShortDateString() + "\" readonly " );
			
				if( this._dateCssClass != null )
					html+= " class='"+ _dateCssClass +"' " ;

				if( this._dateStyle != null )
					html+=( " style='"+ _dateStyle +"' " );
				else
					html+=( " style='cursor:hand;width:80' " );

				html += ( "/>" );

				return html ;			
			}
		}

		public string Month
		{
			get
			{
				if( false == _timeVisible ) return "" ;

				StringBuilder sb = new StringBuilder(); 
				sb.Append( "<select name=\"" + _clientId + "_Hour\" " );

				if( this._hourCssClass != null )
					sb.Append( " class='"+ _hourCssClass +"' " );

				if( this._hourStyle != null )
					sb.Append( " style='"+ _hourStyle +"' " );

				sb.Append( ">" );


				int i ;
				int hour = this.Value.Hour ;
				for( i = 0 ; i < 24 ; i ++ )
				{
					sb.Append( "<option" );
					if( i == hour ) sb.Append( " selected " );
					sb.Append( ">" );

					//if( i < 10 )
					//	sb.Append( "0" + i ) ;
					//else
						sb.Append( "" + i ) ;

					sb.Append( "</option>" );
				}

				sb.Append( "</select> 时" ) ;

				return sb.ToString() ;
			}
		}

		public string Day
		{
			get
			{
				if( false == _timeVisible ) return "" ;

				StringBuilder sb = new StringBuilder(); 

				sb.Append( "<select name=\"" + _clientId + "_Minute\" " );

				if( this._minuteCssClass != null )
					sb.Append( " class='"+ _minuteCssClass +"' " );

				if( this._minuteStyle != null )
					sb.Append( " style='"+ _minuteStyle +"' " );

				sb.Append( ">" );

				int minute = this.Value.Minute ;

				if( minute > 0 && ( _isValueSeted || Page.IsPostBack ) )
				{
					sb.Append( "<option selected>" );
				 
					if( minute < 10 )
						sb.Append( "0" ) ;

					sb.Append( "" + minute ) ;

					sb.Append( "</option>" );
				}

				for( int i = 0 ; i < _minuteSetting.Length ; i ++ )
				{
					sb.Append( "<option>" );
				 
					sb.Append( _minuteSetting[i] ) ;

					sb.Append( "</option>" );
				}

				sb.Append( "</select> 分" ) ;

				return sb.ToString();

			}

		}
		

		private bool _isBinded = false ;
		/// <summary>
		/// 绑定
		/// </summary>
		public override void DataBind()
		{
			if( _isBinded ) return ;
			base.EnsureChildControls() ;
			base.DataBind ();
			_isBinded = true ;
		}

		


		protected override void Render(HtmlTextWriter output)
		{
			if( _IsContainer )
			{
				base.Render( output );
				return ;
			}

			if( this.Controls.Count > 0 ) 
				base.Render( output );
			else
			{
				base.RenderBeginTag( output ) ;

				output.WriteLine( this.Year );
				output.WriteLine( this.Month );
				output.WriteLine( this.Day );

				base.RenderEndTag( output ) ;
			}
		}


		/// <summary>
		/// 将此控件呈现给指定的输出参数。
		/// </summary>
		/// <param name="output">  </param>
//		protected void Render2(HtmlTextWriter output)
//		{
//			//base.Render( output );showCal
//
//			//base.RenderBeginTag( output ) ;
//
//			output.Write( "<table id='"+this.ClientID+"' ><tr><td id='"+this.ClientID+"_DateCell'>" );
//			
//
//			if( _value == DateTime.MinValue )
//				output.Write( "<input onclick=\""+ _function +"\" name=\"" + _clientId + "\" type=\"text\" value=\"\" readonly " );
//			else
//				output.Write( "<input onclick=\""+ _function +"\" name=\"" + _clientId + "\" type=\"text\" value=\"" + _value.ToShortDateString() + "\" readonly " );
//
//			
//			if( this._dateCssClass != null )
//				output.Write( " class='"+ _dateCssClass +"' " );
//
//			if( this._dateStyle != null )
//				output.Write( " style='"+ _dateStyle +"' " );
//			else
//				output.Write( " style='cursor:hand;width:80' " );
//
//			output.WriteLine( "/>" );
//
//
//			output.Write( "</td><td id='"+this.ClientID+"_HourCell'>" );
//
//
//			if( false == _timeVisible ){
//				//base.RenderEndTag( output ) ;
//				output.Write( "</td></tr></table>" );
//
//				return ;
//			}
//			  
//
//			//输出小时
//
//			output.Write( "<select name=\"" + _clientId + "_Hour\" " );
//
//			if( this._hourCssClass != null )
//				output.Write( " class='"+ _hourCssClass +"' " );
//
//			if( this._hourStyle != null )
//				output.Write( " style='"+ _hourStyle +"' " );
//
//			output.WriteLine( ">" );
//
//
//			int i ;
//			int hour = this.Value.Hour ;
//			for( i = 0 ; i < 24 ; i ++ )
//			{
//				output.Write( "<option" );
//				if( i == hour ) output.Write( " selected " );
//				output.Write( ">" );
//				if( i < 10 )
//					output.Write( "0" + i ) ;
//				else
//					output.Write( "" + i ) ;
//				output.WriteLine( "</option>" );
//			}
//
//			output.WriteLine( "</select> 时" ) ;
//
//
//			output.Write( "</td><td id='"+this.ClientID+"_MinuteCell'>" );
//
//
//
//			//输出分钟
//
//			output.Write( "<select name=\"" + _clientId + "_Minute\" " );
//
//			if( this._minuteCssClass != null )
//				output.Write( " class='"+ _minuteCssClass +"' " );
//
//			if( this._minuteStyle != null )
//				output.Write( " style='"+ _minuteStyle +"' " );
//
//			output.WriteLine( ">" );
//
//			int minute = this.Value.Minute ;
////			for( i = 0 ; i < 60 ; i +=15 )
////			{
////				output.Write( "<option" );
////				if( i == minute ) output.Write( " selected " );
////				output.Write( ">" );
////				if( i < 10 )
////					output.Write( "0" + i ) ;
////				else
////					output.Write( "" + i ) ;
////				output.WriteLine( "</option>" );
////			}
//
//			if( minute > 0 && ( _isValueSeted || Page.IsPostBack ) )
//			{
//				output.Write( "<option selected>" );
//				 
//				if( minute < 10 )
//					output.Write( "0" ) ;
//
//				output.Write( "" + minute ) ;
//
//				output.WriteLine( "</option>" );
//			}
//
//			for( i = 0 ; i < _minuteSetting.Length ; i ++ )
//			{
//				output.Write( "<option>" );
//				 
//				output.Write( _minuteSetting[i] ) ;
//
//				output.WriteLine( "</option>" );
//			}
//
//			output.WriteLine( "</select> 分" ) ;
//
//			output.Write( "</td></tr></table>" );
//
//			//base.RenderEndTag( output ) ;
//			
//		}


		#region IPostBackDataHandler 成员

		/// <summary>
		/// 
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
			// TODO:  添加 TimeBox.RaisePostDataChangedEvent 实现
		}

		/// <summary>
		/// 处理回发数据
		/// </summary>
		/// <param name="postDataKey"></param>
		/// <param name="postCollection"></param>
		/// <returns></returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			
			string sDate = postCollection[ _clientId ];
			if( sDate == null || sDate.Trim() == "" ) //sDate = DateTime.Now.ToShortDateString();
			{
				Value = DateTime.MinValue ;
				return false;
			}

			string sHour =  postCollection[ _clientId + "_Hour" ] ;
			string sMinute =  postCollection[ _clientId + "_Minute" ] ;

			if( sHour == null ) sHour = "00";
			if( sMinute == null ) sMinute = "00";

			Value = DateTime.Parse( sDate + " " + sHour + ":" + sMinute ) ;

			return false;
		}

		#endregion
	}
}
