using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using System.Resources;
using System.Collections;
using System.IO ;
using System.Reflection ;

namespace CA.Web
{
	/// <summary>
	/// 资源管理
	/// </summary>
	public class ResourceHelper
	{
		private ResourceHelper()
		{

		} 

		private static object _lockedObject = new object();

		private static Hashtable _resourceList = new Hashtable() ; 


//		private static ResourceManager _resourceManager;
//		/// <summary>
//		/// 获取字符资源
//		/// </summary>
//		/// <param name="key"></param>
//		/// <returns></returns>
//		public static string GetResourceString(string key) 
//		{
//			if (_resourceManager == null) 
//			{
//				lock ( _lockedObject )
//				{
//
//					if ( _resourceManager == null)
//						_resourceManager = new ResourceManager( "CA.Web.Resource", typeof(ResourceHelper).Module.Assembly);
//				}
//
//			}
//
//			return _resourceManager.GetString(key, null);
//		}

		/// <summary>
		/// 获取了控件类型对应的js资源
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static string GetJsResourceString( Type t  ) 
		{
			return "\n<SCRIPT lang=\"javascript\">\n" + GetResourceString( t , "js" ) + "\n</SCRIPT> " ;
		}

        /// <summary>
        /// 注册资源脚本
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ctl"></param>
        public static void RegisterClientScript( Type t , Control ctl )
        {
            RegisterClientScript(t, ctl, t.FullName + ".js");
        }

        public static void RegisterClientScript(Type t, Control ctl , string js )
        {
            ctl.Page.ClientScript.RegisterClientScriptResource( t , js );

           //ctl.Page.ClientScript.RegisterClientScriptBlock(t, t.FullName, GetJsResourceString(t));

        }

        public static void RegisterClientScript(Type t, Control ctl , bool includeInPage )
        {
            if (includeInPage)
            {
                ctl.Page.ClientScript.RegisterClientScriptBlock(t, t.FullName, GetJsResourceString(t));
            }
            else
            {
                ctl.Page.ClientScript.RegisterClientScriptResource(t, t.FullName + ".js");
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static string GetResourceString( Type t , string suffix ) 
		{
			object os = _resourceList[ t ];

			if( os != null )
				return os.ToString() ;

			Assembly assembly = Assembly.GetExecutingAssembly()  ;

			Stream s = assembly.GetManifestResourceStream( t.FullName + "." + suffix );

			if( s == null )
				throw new Exception(  t.FullName + "." + suffix + " resource file doesn't exist." );

//			string[] ss = assembly.GetManifestResourceNames() ;
//
//			string list = "";
//			foreach( string l in ss )
//			{
//				list += ";" + l ;
//			}

			StreamReader	r = new StreamReader( s , System.Text.Encoding.GetEncoding( "GB2312" ) ) ; 
			
			string rs = r.ReadToEnd() ;

			r.Close() ;

			if( _resourceList[ t ] == null )
				_resourceList.Add( t , rs ) ;

			return rs  ;
		}
	}
}
