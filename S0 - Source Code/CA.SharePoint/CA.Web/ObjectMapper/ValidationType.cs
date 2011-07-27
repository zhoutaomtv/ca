using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web
{
    /// <summary>
    /// 验证类型
    /// </summary>
    public enum ValidationType
    {
        None = 20 ,

        Integer = 2,

        Double = 3,

        Currency= 4,

        //Date ,

        //Time ,

        DateTime= 5,

        Email= 6,

        Phone= 7,

        Mobile= 8,

        //Url,

        IdCard= 9,

        Number= 10,

        //Zip,

        //QQ,

        English= 11,

        Chinese= 12,

        Auto = 13

        // UnSafe ,

    }
}
