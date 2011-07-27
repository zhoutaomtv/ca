using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint.CamlQuery
{
    class Eq<T> : SingleCAMLExpression<T>
    {
        public Eq( string name , object value ) 
            : base( name , "Eq" ,  value )
        {
        }

    }
}
