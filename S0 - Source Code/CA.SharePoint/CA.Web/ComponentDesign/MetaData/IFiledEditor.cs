//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：IFiledEditor.cs
// 文件功能描述：组件属性编辑器接口
// 
// 
// 创建标识： 张建义 2007-7-3
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 编辑器类型
    /// </summary>
    public enum EditorType
    {
        Auto ,

        DateTime ,

        Resource ,

        Radio ,

        List ,

        DropDownList ,

        Checkbox 
    }

    /// <summary>
    /// 字段编辑器,有控件项目实现
    /// </summary>
    public interface IFiledEditor
    {
        object FieldValue
        {
            set;
            get;
        }
    }
}
