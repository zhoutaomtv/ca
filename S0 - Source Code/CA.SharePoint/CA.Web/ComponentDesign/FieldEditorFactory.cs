//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：FieldEditorFactory.cs
// 文件功能描述：编辑器工厂，返回配制的编辑器实例 
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
using CA.Web;
using System.Web.UI.WebControls;
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    internal static class FieldEditorFactory
    {
        static public System.Web.UI.WebControls.WebControl GetFieldEditor(FieldSet f)
        {
            //if( f.CheckValues != null && f.CheckValues.Length > 0 )
            //    return 

            if (f.EditorType == EditorType.Auto)
            {
                if (f.Type == typeof(DateTime))
                    return new DateTimeFiledEditor();

                if (f.Type == typeof(Boolean))
                    return new BooleanFiledEditor();

                if (f.Type.IsEnum)
                    return new EnumFiledEditor(f);

                return new StringFiledEditor();
            }
            else
            {
                if (f.EditorType == EditorType.DateTime)
                    return new DateTimeFiledEditor();

                if (f.EditorType == EditorType.Radio)
                {
                    if (f.Type.IsEnum)
                    {
                        return new RadioEnumEditor(f);
                    }
                    else if (f.CheckValues != null && f.CheckValues.Length > 0)
                    {
                        return new RadioCheckValuesEditor(f);
                    }
                    else
                    {
                        throw new NotSupportedException("[" + f.UniqueName + "]：只有是枚举类型或指定了约束才可以用Radio编辑器");
                    }
                }

                if (f.EditorType == EditorType.Checkbox)
                {
                    //if (f.CheckValues == null || f.CheckValues.Length == 0)
                    //{
                    //    throw new NotSupportedException("[" + f.UniqueName + "]：只有指定了约束才可以用Checkbox编辑器");
                    //}

                    return new CheckboxEditor(f);
                }

                if (f.EditorType == EditorType.DropDownList)
                {
                    if (f.Type.IsEnum)
                    {
                        return new EnumFiledEditor(f);
                    }
                    else if (f.CheckValues != null && f.CheckValues.Length > 0)
                    {
                        return new DropDownCheckValuesEditor(f);
                    }
                    else
                    {
                        throw new NotSupportedException("[" + f.UniqueName + "]：只有是枚举类型或指定了约束才可以用DropDownList编辑器");
                    }
                }

                //if (f.EditorType == EditorType.Resource)
                //{
                //    if (f.EditorArgs == null)
                //    {
                //        throw new NotSupportedException("[" + f.UniqueName + "]：Resource编辑器需要资源标示信息");
                //    }

                //    return new ResourceEditor(f);
                //}


                return new StringFiledEditor();
            }
        }
    }

}
