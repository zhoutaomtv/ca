using System;

namespace CA.Web.UploadControl
{
	/// <summary>
	/// PostedFileException 的摘要说明。
	/// </summary>
	public class PostedFileException : Exception 
	{
		public PostedFileException( string message ) : base ( message ) 
		{
 
		}


	}


	public enum	PostedFileExceptionType
	{
		ContentType ,
		Size ,
	}
}
