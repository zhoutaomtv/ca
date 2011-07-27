using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// ²âÊÔ×é¼þ
    /// </summary>
    class TestModel
    {
        private string _UserName = "sys";

        [Editor(DisplayName = "User Name", MaxLength = 50,
            
            CheckValues = new object[] { 1,2,3  },
            
            EditorType=EditorType.Resource ,

            EditorArgs = new object[] { "sex"  } 

            )
        ]
        public string UserName
        { 
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
}
