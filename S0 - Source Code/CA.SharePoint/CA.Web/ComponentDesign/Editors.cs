//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：Editors.cs
// 文件功能描述：编辑器实现 
// 
// 
// 创建标识： 张建义 2007-7-3
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CA.Web;
using System.Web.UI.WebControls;
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 字符串编辑器
    /// </summary>
    internal class StringFiledEditor : TextBox, IFiledEditor
    {
        public StringFiledEditor()
        {
            this.Width = new Unit( "100%" );
        }    

        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value == null)
                    this.Text = "";
                else
                    this.Text = value.ToString();
            }
        }

        #endregion
    }

    internal class BooleanFiledEditor : CheckBox, IFiledEditor
    {
        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                return this.Checked;
            }
            set
            {
                if (value == null)
                    this.Checked = false;
                else
                    this.Checked = Convert.ToBoolean(value);
            }
        }

        #endregion
    }
    /// <summary>
    /// 时间字段编辑器
    /// </summary>
    internal class DateTimeFiledEditor : DatePicker, IFiledEditor
    {
        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                if( value != null )
                    this.Value = Convert.ToDateTime(value);
            }
        }

        #endregion
    }

    /// <summary>
    /// 枚举字段编辑器，采用下拉列表实现
    /// </summary>
    internal class EnumFiledEditor : DropDownList, IFiledEditor
    {
        private FieldSet _EditedField;
        public EnumFiledEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                if (String.IsNullOrEmpty(this.SelectedValue))
                    return null;
                else
                    return Enum.Parse(_EditedField.Type, this.SelectedValue);
            }
            set
            {
                if( value != null )
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //解析枚举字段
            FieldInfo[] fields = _EditedField.Type.GetFields();

            for (int i = 1; i < fields.Length; i++)//需要从第2个开始，第一个为类型信息
            {
                ListItem item = new ListItem();
                item.Value = fields[i].Name;
                item.Text = ComponentSet.GetDisplayName(fields[i]);
                this.Items.Add(item);
            }
        }

        #endregion
    }

    /// <summary>
    /// 枚举字段编辑器，采用单选框实现
    /// </summary>
    internal class RadioEnumEditor : RadioButtonList, IFiledEditor
    {
        private FieldSet _EditedField;
        public RadioEnumEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                return Enum.Parse(_EditedField.Type, this.SelectedValue);
            }
            set
            {
                if( value != null )
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            FieldInfo[] fields = _EditedField.Type.GetFields();

            for (int i = 1; i < fields.Length; i++)
            {
                ListItem item = new ListItem();
                item.Value = fields[i].Name;
                item.Text = ComponentSet.GetDisplayName(fields[i]);
                this.Items.Add(item);
            }
        }

        #endregion
    }

    /// <summary>
    /// 约束字段编辑器，采用单选框实现
    /// </summary>
    internal class RadioCheckValuesEditor : RadioButtonList, IFiledEditor
    {
        private FieldSet _EditedField;
        public RadioCheckValuesEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                if (value != null)
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EditorHelper.BuildListItem(_EditedField, this);
        }

        #endregion
    }

    internal class CheckboxEditor : CheckBoxList, IFiledEditor
    {
        private FieldSet _EditedField;
        public CheckboxEditor(FieldSet f)
        {
            _EditedField = f;
        }

        public object FieldValue
        {
            get
            {
                return EditorHelper.GetSelectedValue(this);
                 
            }
            set
            {
                EditorHelper.SetSelectedValue(this,value);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.RepeatDirection = RepeatDirection.Horizontal;

            EditorHelper.BuildListItem(_EditedField, this);
            

            //for (int i = 0; i < _EditedField.CheckValues.Length; i++)
            //{
            //    ListItem item = new ListItem();
            //    item.Text = _EditedField.CheckValues[i].ToString();
            //    this.Items.Add(item);
            //}
        }
    }

    //internal class ResourceEditor : DropDownList, IFiledEditor
    //{
    //    private FieldSet _EditedField;
    //    public ResourceEditor(FieldSet f)
    //    {
    //        _EditedField = f;
    //    }

    //    public object FieldValue
    //    {
    //        get
    //        {
    //            return this.SelectedValue;
    //        }
    //        set
    //        {
    //            if (value != null)
    //                this.SelectedValue = value.ToString();
    //        }
    //    }

    //    protected override void OnInit(EventArgs e)
    //    {
    //        base.OnInit(e);

    //        object[] args = (object[])_EditedField.EditorArgs;

    //        if (args.Length == 1) //若参数数组只指定了资源表识
    //        {
    //            this.ResourceType = args[0].ToString();

    //        }
    //        else if (args.Length == 2)
    //        {
    //            this.ResourceName = args[0].ToString();
    //            this.ResourceType = args[1].ToString();
    //        }
    //    }
    //}

    internal class DropDownCheckValuesEditor : DropDownList, IFiledEditor
    {
        private FieldSet _EditedField;
        public DropDownCheckValuesEditor(FieldSet f)
        {
            _EditedField = f;
        }

        #region IFiledEditor 成员

        public object FieldValue
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                if (value != null)
                    this.SelectedValue = value.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EditorHelper.BuildListItem(_EditedField, this);

            //for (int i = 1; i < _EditedField.CheckValues.Length; i++)
            //{
            //    ListItem item = new ListItem();
            //    item.Text = _EditedField.CheckValues[i].ToString();
            //    this.Items.Add(item);
            //}
        }


        #endregion
    }

    /// <summary>
    /// 编辑器帮助类
    /// </summary>
    internal static class EditorHelper
    {
        /// <summary>
        /// 创建列表项
        /// 
        /// 若存在约束值，则从约束值创建，
        /// 若存在约束值提供程序，则调用之创建，
        /// 否则抛出异常
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static void BuildListItem( FieldSet f , ListControl ctl )
        {
            if (f.CheckValues != null && f.CheckValues.Length > 0)
            {
                for (int i = 0; i < f.CheckValues.Length; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = f.CheckValues[i].ToString();
                    ctl.Items.Add( item );
                }
                 
            }
            else if (f.CheckValuesProvider != null)
            {
                IDictionary dic = f.CheckValuesProvider.GetCheckValues();

                foreach( DictionaryEntry d in dic )
                {
                    ListItem item = new ListItem() ;

                    item.Value = d.Key.ToString();
                    item.Text = d.Value.ToString();

                    ctl.Items.Add(item);
                }               

           }
            else
            {
                throw new NotSupportedException("[" + f.UniqueName + "]没有指定约束值或约束值提供程序，无法创建列表项");
            }
        }

        public static string GetSelectedValue(ListControl ctl )
        {
            string v = "";

            foreach (ListItem i in ctl.Items )
            {
                if (i.Selected)
                {
                    if (v != "") v += ",";

                    v += i.Value;
                }
            }

            return v;
        }

        public static void SetSelectedValue(ListControl ctl,object value )
        {
            if (value == null)
                return;

            string v = "," + value + ",";

            if (v == ",,") return;

            foreach (ListItem i in ctl.Items)
            {
                if (v.IndexOf("," + i.Value + ",") != -1)
                    i.Selected = true;                
            }
        }
    }


}
