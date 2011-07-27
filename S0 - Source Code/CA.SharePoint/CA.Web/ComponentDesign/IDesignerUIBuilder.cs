//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.
// 
// 创建标识： 张建义 2007-7-3
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using CA.Web;
using System.Web.UI.WebControls;
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 设计器UI创建逻辑
    /// </summary>
    interface IDesignerUIBuilder
    {
        /// <summary>
        /// 创建编辑UI
        /// </summary>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, IFiledEditor> BuildEditUI(ComponentDesignControl designer, ComponentSet set, object obj);

        /// <summary>
        /// 创建回发还原UI,只需要创建控件，不需要填充控件值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, IFiledEditor> BuildRetrieveUI(ComponentDesignControl designer, ComponentSet set);

        /// <summary>
        /// 创建只读查看UI
        /// </summary>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        void BuildViewUI(ComponentDesignControl designer, ComponentSet set, object obj);
    }
}
