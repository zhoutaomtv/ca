using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    public interface IComponentDesigner
    {
        void EditComponent(object obj);

        object GetComponent();        
    }
}
