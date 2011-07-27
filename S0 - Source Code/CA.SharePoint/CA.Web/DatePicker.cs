using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace CA.Web
{
	///@2006-12-31 jianyi
	/// <summary>
	/// 时间选择控件 
	/// </summary>/ 
	[DefaultProperty("Value"),
	ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
	public class DatePicker : System.Web.UI.WebControls.TextBox  //,  System.Web.UI.IPostBackDataHandler
	{
		public DatePicker()
		{
			//this.ReadOnly = false ;
            this.CssClass = "DatePicker";
		}

		private DateTime _value   ;
		private string _imageUrl ;

		/// <summary>
		/// 图片路径
		/// </summary>
        //[Browsable(true),
        //Editor( typeof( System.Web.UI.Design.UrlEditor ) , typeof(System.Drawing.Design.UITypeEditor) ) , 
        //Category("Behavior")]
        //public string SelectImageUrl
        //{
        //    set
        //    {
        //        _imageUrl = value ;
        //    }
        //    get
        //    {
        //        return _imageUrl ;
        //    }
        //}

        //private string _clearImageUrl = "~/Images/TimePicker/";
        //[Browsable(true),
        //Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
        //Category("Behavior")]
        //public string ClearImageUrl
        //{
        //    set
        //    {
        //        _imageUrl = value;
        //    }
        //    get
        //    {
        //        return _imageUrl;
        //    }
        //}

		private string _function = null ;
		/// <summary>
		/// 自定义客户端Onclick函数
		/// </summary>
        //[Browsable(true),Description("")]
        //public string ClientFunction
        //{
        //    set
        //    {
        //        _function = value ;
        //    }
        //}

        /// <summary>
        /// 时间值
        /// </summary>
        [Bindable(true),
        Category("Data")]
        public DateTime Value
        {
            get
            {
                if (_value == DateTime.MinValue && !String.IsNullOrEmpty(Text))
                    _value = Convert.ToDateTime(Text);

                return _value;
            }
            set
            {
                _value = value;
                if (_value != DateTime.MinValue)
                    Text = value.ToShortDateString();
                else
                    Text = "";
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    _value = Convert.ToDateTime(value);
                else
                    _value = DateTime.MinValue; 

                base.Text = value;
            }
        }

        private bool _enableClear = true ;
        /// <summary>
        /// 是否允许时间清除
        /// </summary>
        public bool EnableClear
        {
            set { _enableClear = true; }
            get { return _enableClear; }
        }

        private string _ImageFolder = "~/Images/DatePicker/" ;
        /// <summary>
        /// 图片文件夹路径
        /// </summary>
        public string ImageFolder
        {
            set { _ImageFolder = value; }
            get { return _ImageFolder; }
        }

        private string _SelectImageCssClass = "DatePicker-SelectImage";
        /// <summary>
        /// 时间选择图片cssclass
        /// </summary>
        public string SelectImageCssClass
        {
            set { _SelectImageCssClass = value; }
            get { return _SelectImageCssClass; }
        }

        private string _SelectClearCssClass = "DatePicker-ClearImage";
        /// <summary>
        /// 时间清除图片 css calss
        /// </summary>
        public string SelectClearCssClass
        {
            set { _SelectClearCssClass = value; }
            get { return _SelectClearCssClass; }
        }

        private string _SelectTextboxCssClass = "DatePicker-Textbox";
        /// <summary>
        /// 文本框css class
        /// </summary>
        public string SelectTextboxCssClass
        {
            set { _SelectTextboxCssClass = value; }
            get { return _SelectTextboxCssClass; }
        }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

            ImageFolder = ResolveUrl( ImageFolder  );

            Page.ClientScript.RegisterClientScriptBlock( typeof( DatePicker) ,
                "DatePicker_ImageFolder" , "DatePicker_ImageFolder='"+ImageFolder+"';" , true ) ;

            ResourceHelper.RegisterClientScript(typeof(DatePicker), this , true );
		}

		/// <summary>
		/// 将此控件呈现给指定的输出参数。
		/// </summary>
		/// <param name="output">  </param>
		protected override void Render(HtmlTextWriter output)
		{
            output.Write("<span id='");
            output.Write( this.ClientID );
            output.Write( "_Container' " );

            output.Write( "class='" );
            output.Write( this.CssClass );
            output.Write( "' >" );

            if (ReadOnly || Enabled == false)
            {
                output.Write("<input class='" + this.SelectTextboxCssClass + "' style=\"cursor:hand;vertical-align:bottom;width:80px;border:none 0px black;\" name=\"" + this.UniqueID + "\" id='" + this.ClientID + "' type=\"text\" value=\"" + this.Text + "\" readonly />");
                output.Write("<img id'=" + this.ClientID + "_SelectImage' class='" + this.SelectImageCssClass + "'  style='margin:1px;vertical-align:middle;cursor:hand;width:16px;height:15px' border='0' src='" + ImageFolder + "select.gif'/>");
                
                if (_enableClear)
                    output.Write("<img id'=" + this.ClientID + "_ClearImage' class='" + this.SelectClearCssClass + "'  style='margin:1px;vertical-align:middle;cursor:hand;width:16px;height:15px' border='0' src='" + ImageFolder + "clear.gif' />");
            }
            else
            {
                output.Write("<input class='" + this.SelectTextboxCssClass + "'  style=\"cursor:hand;vertical-align:bottom;width:80px;border:none 0px black;\" onclick=\"TimePicker_Select('" + this.ClientID + "')\" name=\"" + this.UniqueID + "\" id='" + this.ClientID + "' type=\"text\" value=\"" + this.Text + "\" readonly />");
                output.Write("<img title='Select Date' id'=" + this.ClientID + "_SelectImage' class='" + this.SelectImageCssClass + "'  style='margin:1px;vertical-align:middle;cursor:hand;width:16px;height:15px' border='0' src='" + ImageFolder + "select.gif' onclick=\"TimePicker_Select('" + this.ClientID + "')\" />");

                if (_enableClear)
                    output.Write("<img title='Reset Date' id'=" + this.ClientID + "_ClearImage' class='" + this.SelectClearCssClass + "'  style='margin:1px;vertical-align:middle;cursor:hand;width:16px;height:15px' border='0' src='" + ImageFolder + "clear.gif' onclick=\"TimePicker_Clear('" + this.ClientID + "')\" />");
            }

            output.Write( "</span>" );
			
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
