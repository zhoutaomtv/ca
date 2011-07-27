using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls ;
using System.ComponentModel;


namespace CA.Web.UploadControl
{
	/// <summary>
	/// 多文件上传控件 2005 10-14
	/// 
	/// 2005 10 17 加入数量限制 
	/// </summary>
	public class MultiFileUpload : WebControl , System.Web.UI.IPostBackDataHandler , INamingContainer 
	{

		private string _uploaderIds = "0";

		private int _maxContentLength = -1 ;
		private int _maxSize = -1 ;

		private string _contentType = "" ;
		private string _contentTypeList = "";

		private int _uploaderCount = 0 ;

		/// <summary>
		/// 附件允许大小（单位：M）
		/// </summary>
		[Category("Behavior"),Description("附件允许大小（单位：M）")]
		public int MaxSize
		{
			set
			{
				_maxContentLength = value * 1024 * 1024 ;
				_maxSize = value ;
			}
			get
			{
				return _maxSize;
			}
		}

		private int _maxCount = -1 ;

		/// <summary>
		/// 附件允许数量（小于0则无限制）
		/// </summary>
		[Category("Behavior"),Description("附件允许数量（<0则无限制）")]
		public int MaxCount
		{
			set
			{
				_maxCount = value ;
			}
			get
			{
				return _maxCount;
			}
		}

		/// <summary>
		/// 附件允许类型（多个以,间隔,可以是:image/gif,text/html,application/octet-stream,text/xml,text/plain,application/x-zip-compressed）
		/// </summary>
		[Category("Behavior"),
		Description("附件允许类型（多个以,间隔,可以是:image/gif,text/html,application/octet-stream,text/xml,text/plain,application/x-zip-compressed）")]
		public string ContentType
		{
			set
			{
				_contentType = value ;
				_contentTypeList = "," + value + ",";
			}
			get
			{
				return _contentType ;
			}
		}

		public string ErrorMessage = "" ;


		/// <summary>
		/// 初始附件控件数量
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("初始附件控件数量")]
		public int UploaderCount
		{
			set
			{
				this._uploaderCount = value ;
				 _uploaderIds = "0" ;
				for( int i = 1 ; i <= this._uploaderCount ; i ++ )
				{
					_uploaderIds += "," + i ;
				}

				//this.CreateChildControls();
			}
		}

		private bool _enableCountChanged = true ;
		/// <summary>
		/// 是否允许客户端改变上传控件数量
		/// </summary>
		[Category("Behavior"),Description("是否允许客户端改变上传控件数量")]
		public bool EnableCountChanged
		{
			set{_enableCountChanged=value;}
			get{return _enableCountChanged ;}
		}

//		private string _buttonFormat ;
//
//		/// <summary>
//		/// 
//		/// </summary>
//		[Category("Behavior"),Description("")]
//		public string ButtonFormat
//		{
//			set{ _buttonFormat=value;}
//			get{ return  _buttonFormat ;}
//		}

		private string _controlsCssClass ;
		/// <summary>
		/// 子控件样式class
		/// </summary>
		[Category("Behavior"),Description("子控件class")]
		public string ControlsCssClass
		{
			set{_controlsCssClass=value;}
			get{return _controlsCssClass ;}
		}

		private int _controlsSize = 30 ;
		/// <summary>
		/// 子控件大小
		/// </summary>
		[Category("Behavior"),Description("子控件大小")]
		public int ControlsSize 
		{
			set{_controlsSize=value;}
			get{return _controlsSize ;}
		}

		private string _imageUrl = "images/htmltextbox/delete.gif" ;
		/// <summary>
		/// 删除按钮图片路径
		/// </summary>
		[Category("Behavior"), Description("删除图片路径"),
		Editor( typeof( System.Web.UI.Design.UrlEditor ) , typeof(System.Drawing.Design.UITypeEditor) ) ]
		public string ImageUrl
		{
			set { _imageUrl = value; }
			get { return _imageUrl; }
		}


		private bool _throwable = false ;
		/// <summary>
		/// 附件错误时是否允许抛出异常
		/// </summary>
		[Category("Behavior"),Description("附件错误时是否允许抛出异常")]
		public bool Throwable 
		{
			set{_throwable=value;}
			get{return _throwable ;}
		}


		private PostedFileInfoCollection _files = new PostedFileInfoCollection() ;

		/// <summary>
		/// 获取附件集合－－若Throwable为true ,可能会抛出异常
		/// </summary>
		[Browsable(false),
		Description("获取附件－－可能会抛出异常")]
		public PostedFileInfoCollection Files
		{
			get
			{
				base.EnsureChildControls() ;

				PostedFileInfoCollection files = new PostedFileInfoCollection() ;

				int i = 0 ;
				foreach( PostedFileInfo pf in _files ) //过滤空附件
				{
					i ++ ;
					if( pf.PostedFile == null || pf.PostedFile.ContentLength == 0 ) continue ;
					
					if( this.ContentType != null && this.ContentType != "" 
						&& _contentTypeList.IndexOf( "," + pf.PostedFile.ContentType + "," ) == -1  )
					{
						if( Throwable ) throw new Exception( "第"+i+"个附件类型不正确" )   ;
						else continue ;
					}

					if( this._maxContentLength > 0 && pf.PostedFile.ContentLength > this._maxContentLength  )
					{
						if( Throwable ) throw new Exception( "第"+i+"个附件超过允许大小("+_maxSize+"M)" )   ;
						else continue ;
					}

					files.Add( pf ) ;
				}

				return files ;
			}
		}

//		public PostedFileInfoCollection GetFiles( string type )
//		{
//			PostedFileInfoCollection files = new PostedFileInfoCollection() ;
//			
//			string typeList = "," + type + "," ;
//			
//			foreach( PostedFileInfo pf in _files )
//			{
//				if( typeList.IndexOf( "," + pf.PostedFile.ContentType + "'" ) > -1 )
//				{
//					files.Add( pf ) ;
//				}
//			}
//
//			return files ;
//		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			Page.RegisterRequiresPostBack( this ) ;

			Control c = this.Parent ;
			while( c != null && false ==(  c is System.Web.UI.HtmlControls.HtmlForm ) )
			{
				c = c.Parent ;
			}
			
			if( c == null ) throw new Exception( "控件必须放在HtmlForm中" );
			
			HtmlForm form = c as HtmlForm ;

			form.Enctype = "multipart/form-data";

			//enctype="multipart/form-data"
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void CreateChildControls()
		{
			this.Controls.Clear();

			if( this._uploaderIds == "0" ) return ;

			if( Page.IsPostBack == false ) return ;
 
			string[] arrIds = this._uploaderIds.Split( ',' );

			for( int i = 1 ; i < arrIds.Length ; i ++ )
			{
				System.Web.UI.HtmlControls.HtmlInputFile uploader = new System.Web.UI.HtmlControls.HtmlInputFile() ;	
				uploader.ID =  "_file_" + arrIds[i] ;
				uploader.Size = this.ControlsSize ;
				uploader.Attributes.Add( "class" , this.ControlsCssClass );
				
				this.Controls.Add( uploader ) ; //让控件自己匹配 附件

				this._files[i-1].SetPostedFile( uploader.PostedFile ) ;

				//if( uploader.PostedFile != null ) Page.Response.Write( uploader.PostedFile.FileName ) ;

			}

			
			base.ChildControlsCreated = true ;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			base.EnsureChildControls() ;

			registerJs() ;

		}
		
		//注册js
		private void registerJs()
		{
			string jsKey = "MultiUploader-js";

			if( Page.IsClientScriptBlockRegistered( jsKey ) ) return ;

			Page.RegisterClientScriptBlock( jsKey , ResourceHelper.GetJsResourceString( typeof(MultiFileUpload) ) );
		}

		private  void test()
		{
			foreach( Control c in this.Controls )
			{
				System.Web.UI.HtmlControls.HtmlInputFile f = c as System.Web.UI.HtmlControls.HtmlInputFile ;
				Page.Response.Write( f.Name );
				if( f.PostedFile != null ) 
					Page.Response.Write( f.PostedFile.FileName );

				Page.Response.Write( "<hr>" );
		
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="output"></param>
		protected override void Render(HtmlTextWriter output)
		{
			if( this.EnableViewState == false )  this._uploaderIds = "0" ;

			base.RenderBeginTag( output ) ;

			if( this.EnableCountChanged )
			{
				string link = "<div align=left>§<a href=\"javascript:MultiUploader_AddAttach('" 
					+ this.ClientID + "', '"+this.UniqueID+"' , '"+this.ControlsSize+"','"+this.ControlsCssClass+"',"+this.MaxCount+",'"+this.ImageUrl+"')\">新增附件</a>§"+
					"&nbsp;&nbsp;（提示：点击新增附件多次，可以增加多个附件！）"+
					"<hr style=BORDER-TOP: 1px dotted; BORDER-BOTTOM: 1px dotted; dotted:  color=#000000 noShade SIZE=0></div>" ;
			
				output.WriteLine( link );
			}	

			output.Write("<input type='hidden' name='"+ this.ClientID +"_Ids' id='"+ this.ClientID +"_Ids' value='"+_uploaderIds+"'>");

			//base.Render( output ) ;
			//return ;

			if( this._uploaderIds != "0" )
			{
				string[] arrIds = this._uploaderIds.Split( ',' );

				for( int i = 1 ; i < arrIds.Length ; i ++ )
				{
					output.WriteLine("<div id='"+ this.ClientID + "_div_" + arrIds[i] +"'>"); 

					//	this.Controls[i-1].RenderControl( output ) ;

					output.WriteLine( "附件名称：<input onchange=\"validate(this)\" type=file name='" + this.UniqueID + "$_file_" + arrIds[i] + "' size="+ControlsSize+" class="+ControlsCssClass+">" );

					// output.WriteLine("<input type=button onclick=\"MultiUploader_Remove( '"+this.ClientID+"', '"+arrIds[i]+"' )\" value='删 除'>");
						
					output.Write( "<br>附件标题：<input  name='" + this.ClientID + "_name_" + arrIds[i] + "' type=text size="+this.ControlsSize+" class="+this.ControlsCssClass+" >" );
					output.WriteLine( "<img border=0 style='cursor:hand' src='"+ this.ImageUrl +"' onclick=\"MultiUploader_Remove( '"+this.ClientID+"', '"+ arrIds[i] +"' )\">" );
					
					output.WriteLine("</div>");
				}
			}

			base.RenderEndTag( output ) ;

		}

		#region IPostBackDataHandler 成员

		public void RaisePostDataChangedEvent()
		{
 		}

		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			this._uploaderIds = postCollection[ this.ClientID +"_Ids" ];

			string[] arrIds = this._uploaderIds.Split( ',' );

			PostedFileInfo f ;
			//填充文件名
			for( int i = 1 ; i < arrIds.Length ; i ++ )
			{
				f = new PostedFileInfo(); 
				 
				f.SetCustomName( postCollection[ this.ClientID + "_name_" + i ] );
				_files.Add( f ) ;
			}

			return false;
		}

		#endregion
	}
}
