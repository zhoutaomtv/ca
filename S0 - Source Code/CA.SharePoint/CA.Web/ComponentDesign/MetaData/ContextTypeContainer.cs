using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;


namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 运行环境dll容器
    /// </summary>
    class ContextTypeContainer
    {
        public Assembly[] GetAssemblies()
        {
            return null;
        }

        public Type[] GetTypes()
        {
            return null;
        }

        public string[] GetNamespaces()
        {
            return null;
        }

        public Type[] FindTypes( string name )
        {
            return null;
        }

        public Type[] GetInstanceTypes( Assembly ass )
        {
            return null;
        }

        public Type[] GetImplent( Type interfaceType )
        {
            return null;
        }


    }
}
