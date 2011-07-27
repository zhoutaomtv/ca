using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.UploadControl
{
	/// <summary>
	/// 上传附件信息
	/// </summary>
	public class PostedFileInfo
	{
		public PostedFileInfo()
		{
 
		}

		private System.Web.HttpPostedFile _PostedFile ;
		/// <summary>
		/// 上传附件
		/// </summary>
		public System.Web.HttpPostedFile PostedFile
		{
			get{
				return _PostedFile ;
			}
		}

		private string _CustomName ;
		/// <summary>
		/// 附件自定义名
		/// </summary>
		public string CustomName
		{
			get
			{
				return _CustomName ;
			}
		}
		
		/// <summary>
		/// 设置附件名
		/// </summary>
		/// <param name="customName"></param>
		internal void SetCustomName( string customName )
		{
			_CustomName = customName ;		
		}
		
		/// <summary>
		/// 设置附件
		/// </summary>
		/// <param name="postedFile"></param>
		internal void SetPostedFile( System.Web.HttpPostedFile postedFile )
		{
			_PostedFile = postedFile ;

			if( _PostedFile != null )
			{
				_FileName = _PostedFile.FileName ;
				_ContentLength = _PostedFile.ContentLength ;
				_ContentType = _PostedFile.ContentType ;
			}
		}


		private string _FileName ;
		/// <summary>
		/// 附件全名
		/// </summary>
		public string FileName
		{
			get
			{
				return _FileName ;
			}
		}

		private string _ContentType ;
		/// <summary>
		/// 附件类型
		/// </summary>
		public string ContentType
		{
			get
			{
				return _ContentType ;
			}
		}

		private int _ContentLength ;
		/// <summary>
		/// 附件大小（字节）
		/// </summary>
		public int ContentLength
		{
			get
			{
				return _ContentLength ;
			}
		}

 

	}
}
