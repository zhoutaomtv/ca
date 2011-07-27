using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint.CamlQuery
{
    public class TypeQueryField<TField> : CA.SharePoint.CamlQuery.TypeFieldRef<object, TField>
    {

        public TypeQueryField(string name)
            : base(name)
        {
        }

    }
}
