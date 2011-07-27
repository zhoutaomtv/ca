using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace CA.Web.TimeControl
{
	///@2005-5-27 jianyi
	/// <summary>
	/// 时间选择控件 
	/// </summary>
	/// 
	[DefaultProperty("Value"),
	ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
	public class DatePicker : System.Web.UI.WebControls.TextBox  //,  System.Web.UI.IPostBackDataHandler
	{
//		public DatePicker()
//		{
//			this.Text = _value.ToShortDateString() ;		
//		}

		private DateTime _value = DateTime.Now ;

//		private string _dateStyle = null ;
//		private string _hourStyle = null ;
//		private string _minuteStyle = null ;
//
//		private string _dateCssClass = null ;
//		private string _hourCssClass = null ;
//		private string _minuteCssClass = null ;

		private string _imageUrl ;


		/// <summary>
		/// 图片路径
		/// </summary>
		[Browsable(true),
		Editor( typeof( System.Web.UI.Design.UrlEditor ) , typeof(System.Drawing.Design.UITypeEditor) ) , 
		Category("Behavior")]
		public string ImageUrl
		{
			set
			{
				_imageUrl = value ;
			}
			get
			{
				return _imageUrl ;
			}
		}

		private string _function = null ;
		/// <summary>
		/// 自定义客户端Onclick函数
		/// </summary>
		[Browsable(true),Description("")]
		public string ClientFunction
		{
			set
			{
				_function = value ;
			}
		}

//		[Bindable(true),
//		Category("Data")]
//		public DateTime Value
//		{
//			get
//			{
//				return _value;
//			}
//
//			set
//			{
//				_value = value;
//			}
//		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			string clientJsKey = "DateTimePicker-ClientJs";

			if( this._function != null && this._function != "" )
			{

			}
			else
			{
				//_function = "DateTimePicker_setday(this)" ;
				_function = "calendar()" ;

			}

			this.ReadOnly = true ;

			this.Attributes.Add( "onclick" , _function );
 
			if( false == Page.IsClientScriptBlockRegistered( clientJsKey ) )
			{
				//Page.RegisterClientScriptBlock( clientJsKey , ResourceHelper.GetJsResourceString( typeof( DatePicker ) ) ) ;
			}
		}


		/// <summary>
		/// 将此控件呈现给指定的输出参数。
		/// </summary>
		/// <param name="output">  </param>
		protected override void Render(HtmlTextWriter output)
		{
	
			output.Write( "<input onclick=\""+ _function +"\" name=\"" + this.UniqueID + "\" id='"+this.ClientID+"' type=\"text\" value=\"" + this.Text + "\" readonly />" );

			if( this._imageUrl != null &&  this._imageUrl != "" )
			{
				output.Write( "<img style='cursor:hand' border='0' src='"+this._imageUrl+"' onclick=\"calendar( document.all."+ this.ClientID +" )\" />" ) ; 
			}
			
		}

//		#region IPostBackDataHandler 成员
//
//		public void RaisePostDataChangedEvent()
//		{
//			// TODO:  添加 TimeBox.RaisePostDataChangedEvent 实现
//		}
//
//		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
//		{
//			
//			string sDate = postCollection[ _clientId ];
//			if( sDate == null || sDate.Trim() == "" ) sDate = DateTime.Now.ToShortDateString();
//
//			string sHour =  postCollection[ _clientId + "_Hour" ] ;
//			string sMinute =  postCollection[ _clientId + "_Minute" ] ;
//
//			if( sHour == null ) sHour = "00";
//			if( sMinute == null ) sHour = "sMinute";
//
//			Value = DateTime.Parse( sDate + " " + sHour + ":" + sMinute ) ;
//
//			return false;
//		}
//
//		#endregion
	}
}
