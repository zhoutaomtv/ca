using System;
using System.Collections ;


namespace CA.Web.UploadControl
{
	/// <summary>
	/// 上传附件集合
	/// </summary>
	public class PostedFileInfoCollection : CollectionBase
	{

		/// <summary>
		/// 
		/// </summary>
		public  PostedFileInfo this[ int index ]  
		{
			get  { return( ( PostedFileInfo) List[index] );}
			set  { List[index] = value;}
		}

		/**********************************
		* 集合方法 
		*
		**********************************/
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(  PostedFileInfo value )  
		{
			return( List.Add( value ) );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(  PostedFileInfo value )  
		{
			return( List.IndexOf( value ) );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert( int index,  PostedFileInfo value )  
		{
			//if( value.ID <= 0 ) throw new NullReferenceException( "ID属性必须大于0" );
			List.Insert( index, value );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove(  PostedFileInfo value )  
		{
			List.Remove( value );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(  PostedFileInfo value )  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != typeof(PostedFileInfo) )
				throw new ArgumentException( "value must be of type  PostedFileInfo.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != typeof(PostedFileInfo) )
				throw new ArgumentException( "value must be of type  PostedFileInfo.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != typeof(PostedFileInfo) )
				throw new ArgumentException( "newValue must be of type  PostedFileInfo.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != typeof(PostedFileInfo) )
				throw new ArgumentException( "value must be of type  PostedFileInfo." );
		}

	}
}
