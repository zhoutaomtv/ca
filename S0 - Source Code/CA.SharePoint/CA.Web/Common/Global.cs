using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web
{
    public abstract class Global
    {
        public static string ApplicationPath
        {
            get
            {
                string root = System.Web.HttpContext.Current.Request.ApplicationPath;
                if (root == "/") return root;
                else return root + "/";
            }
        }
    }
}
