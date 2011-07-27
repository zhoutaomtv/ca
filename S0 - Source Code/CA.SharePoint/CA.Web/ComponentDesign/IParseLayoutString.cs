using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    interface IParseLayoutString
    {
        /// <summary>
        /// 根据布局字符串解析为对象
        /// </summary>
        /// <param name="strlayout"></param>
        void Parselayout(string strlayout);
    }
}
