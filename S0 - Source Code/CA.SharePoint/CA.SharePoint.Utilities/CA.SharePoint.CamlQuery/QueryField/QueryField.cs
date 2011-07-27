using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint.CamlQuery
{
    public class QueryField : CA.SharePoint.CamlQuery.FieldRef<object>
    {

        public QueryField(string name)
            : base(name)
        {
        }

    }
}
