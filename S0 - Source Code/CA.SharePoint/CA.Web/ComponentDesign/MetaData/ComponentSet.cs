//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：ComponentSet.cs
// 文件功能描述：组件元数据抽象 
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
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentSet
    {
        /// <summary>
        /// 子对象元数据 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="property"></param>
        private ComponentSet(ComponentSet parent, PropertyInfo property)
        {
            this.Property = property ;

            Type t = property.PropertyType;
     
            this.ParentSet = parent;
            this.Depth = parent.Depth + 1;            

            EditorAttribute disAtt = ComponentSet.GetEditorAttribute(property);

            if (disAtt != null)
            {
                this.DisplayName = disAtt.DisplayName;
              
                this.Ignore = disAtt.Ignore;
            }

            if (String.IsNullOrEmpty(this.DisplayName))
                this.DisplayName = this.Name;            

            PropertyInfo[] ps = t.GetProperties();

            this.Name = t.Name;

            foreach (PropertyInfo p in ps)
            {
                if (p.PropertyType.IsValueType || p.PropertyType == typeof(String))
                {
                    FieldSet f = new FieldSet(p, this);

                    if (f.Ignore == false)
                        this.Fields.Add(f);

                }
                else if (p.PropertyType.IsClass && p.PropertyType != parent.Type ) //防止递归问题
                {
                    ComponentSet set = new ComponentSet(this, p);

                    if (set.Ignore == false)
                        this.SubSet.Add(set);
                }
            }
            IList<FieldSet> sortedFields = FieldsSort(this.Fields);
            Fields = sortedFields;
        }

        /// <summary>
        /// 类型元数据
        /// </summary>
        /// <param name="t"></param>
        internal ComponentSet( Type t)
        {
            Type = t;

            PropertyInfo[] ps = t.GetProperties();

            this.Name = t.Name;

            //this.DisplayName = ComponentSet.GetDisplayName(t);

            foreach (PropertyInfo p in ps)
            {
                if (p.PropertyType.IsValueType || p.PropertyType == typeof(String))
                {
                    FieldSet f = new FieldSet(p, this);

                    if( f.Ignore ==false )
                        this.Fields.Add(f);

                }
                else if (p.PropertyType.IsClass)
                {
                    ComponentSet set = new ComponentSet(this, p );

                    if( set.Ignore == false )
                        this.SubSet.Add(set);
                }
            }
            IList<FieldSet> sortedFields = FieldsSort(this.Fields);
            Fields = sortedFields;
        }

        #region 对FieldSet集合重新排序
        private IList<FieldSet> FieldsSort(IList<FieldSet> fields)
        {
            IList<FieldSet> sortedFields = new List<FieldSet>();
            IList<FieldSet> tmpFields = new List<FieldSet>();
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].Sequence > 0)
                    tmpFields.Add(fields[i]);
            }
            while (tmpFields.Count > 0)
            {
                int index = int.MaxValue;
                int row = 0;
                for (int j = 0; j < tmpFields.Count; j++)
                {
                    if (tmpFields[j].Sequence < index)
                    {
                        index = tmpFields[j].Sequence;
                        row = j;
                    }
                }
                sortedFields.Add(tmpFields[row]);
                tmpFields.Remove(tmpFields[row]);

            }
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].Sequence == 0)
                    sortedFields.Add(fields[i]);
            }
            return sortedFields;
        }
        #endregion

        public readonly Type Type;

        /// <summary>
        /// 名称
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 显示名
        /// </summary>
        public readonly string DisplayName;

        public readonly bool Ignore = false;

        /// <summary>
        /// 父对象元数据
        /// </summary>
        public readonly ComponentSet ParentSet = null;

        /// <summary>
        /// 对应的属性信息
        /// </summary>
        public readonly PropertyInfo Property;

        /// <summary>
        /// 对象深度
        /// </summary>
        public readonly int Depth = 0;

        /// <summary>
        /// 唯一名
        /// </summary>
        public string UniqueName
        {
            get
            {
                if (this.ParentSet == null)
                    return this.Name;
                else
                    return this.ParentSet.UniqueName + "." + this.Name;
            }
        }

        /// <summary>
        /// 包含的字段
        /// </summary>
        public readonly IList<FieldSet> Fields = new List<FieldSet>();

        /// <summary>
        /// 通过属性名获取字段信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FieldSet GetField(string name)
        {
            foreach (FieldSet f in Fields)
            {
                if (String.Compare(f.Name, name, true) == 0)
                    return f;
            }

            return null;
        }

        /// <summary>
        /// 包含的字对象元数据
        /// </summary>
        public readonly IList<ComponentSet> SubSet = new List<ComponentSet>();

        public void UpdateValue(IDictionary<string, object> dic,object obj)
        {
            foreach (FieldSet f in this.Fields)
            {
                f.UpdateValue(dic,obj);
            }

            foreach (ComponentSet set in this.SubSet )
            {
                object subObj = set.Property.GetValue(obj, null);

                if (subObj == null)
                    continue;

                set.UpdateValue(dic,subObj);
            }
        }

        /// <summary>
        /// 获取对象值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetValue( object obj )
        {
            if (obj != null)
                return Property.GetValue(obj, null);
            else
                return null;
        }

        /// <summary>
        /// 获取类型的显示名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDisplayName(Type t)
        {
            object[] arr = t.GetCustomAttributes(typeof(EditorAttribute), true);

            string disName = "";

            if (arr != null && arr.Length > 0)
            {
                EditorAttribute disAtt = (EditorAttribute)arr[0];
                disName = disAtt.DisplayName;
            }

            if (String.IsNullOrEmpty(disName))
                disName = t.Name;

            return disName;
        }

        /// <summary>
        /// 获取字段的显示名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDisplayName(FieldInfo t)
        {
            object[] arr = t.GetCustomAttributes(typeof(EditorAttribute), true);

            string disName = "";

            if (arr != null && arr.Length > 0)
            {
                EditorAttribute disAtt = (EditorAttribute)arr[0];
                disName = disAtt.DisplayName;
            }

            if (String.IsNullOrEmpty(disName))
                disName = t.Name;

            return disName;
        }

        public static EditorAttribute GetEditorAttribute(FieldInfo t)
        {
            object[] arr = t.GetCustomAttributes(typeof(EditorAttribute), true);

            string disName = "";

            if (arr != null && arr.Length > 0)
            {
                return (EditorAttribute)arr[0];
                
            }

            return null;
        }

        public static EditorAttribute GetEditorAttribute(PropertyInfo t)
        {
            object[] arr = t.GetCustomAttributes(typeof(EditorAttribute), true);

            string disName = "";

            if (arr != null && arr.Length > 0)
            {
                return (EditorAttribute)arr[0];

            }

            return null;
        }
    }

    /// <summary>
    /// 字段元数据
    /// </summary>
    public class FieldSet
    {
        internal FieldSet(PropertyInfo p, ComponentSet ownerObjSet)
        {
            this.Name = p.Name;
            this.Property = p;
            this.Type = p.PropertyType;
            this.ParentSet = ownerObjSet;

            EditorAttribute disAtt = ComponentSet.GetEditorAttribute(p);

            if (disAtt != null)
            {
                this.DisplayName = disAtt.DisplayName;
                this.MaxLength = disAtt.MaxLength;
                this.EditorType = disAtt.EditorType;
                this.EditorArgs = disAtt.EditorArgs;
                this.CheckValues = disAtt.CheckValues;
                this.ValidationType = disAtt.ValidationType;
                this.ValidationExpression = disAtt.ValidationExpression;
                this.Sequence = disAtt.Sequence;

                this.CheckValuesProvider = this.GetCheckValuesProvider(disAtt);

                this.Ignore = disAtt.Ignore;
                //
            }

            if (String.IsNullOrEmpty(this.DisplayName))
                this.DisplayName = this.Name;
        }

        /// <summary>
        /// 修改对象字段值
        /// </summary>
        /// <param name="dic">值列表</param>
        /// <param name="obj"></param>
        public void UpdateValue(IDictionary<string, object> dic , object obj )
        {
            if (dic.ContainsKey(this.UniqueName))
            {
                object s = dic[this.UniqueName];

                setPropertyValue(this.Property, s, obj);
            }
        }

        /// <summary>
        /// 获取对象字段值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetValue( object obj )
        {
            if (obj != null)
                return Property.GetValue(obj, null);
            else
                return null;
        }

        private ICheckValuesProvider GetCheckValuesProvider( EditorAttribute att )
        {
            //if (att.CheckValues != null && att.CheckValues.Length > 0)
            //{
            //    return new SimpleCheckValuesProvider(CheckValues);
            //}
            //else 
                
            if (att.CheckValuesProvider != null)
            {
                ICheckValuesProvider p = (ICheckValuesProvider)System.Activator.CreateInstance(att.CheckValuesProvider);

                return p;
            }
            else
            {
                return null;
            }
        }

        private static void setPropertyValue(PropertyInfo p, object value, object obj)
        {
            if (value == null)
            {
                p.SetValue(obj, null, null);
                return;
            }

            if (p.PropertyType == typeof(String))
            {
                p.SetValue(obj, value, null);
            }
            else if (p.PropertyType.IsEnum)
            {
                p.SetValue(obj, Enum.Parse(p.PropertyType, value.ToString(), false), null);
            }
            else if (!p.PropertyType.IsClass && !p.PropertyType.IsInterface)
            {
                object oV = Convert.ChangeType(value, p.PropertyType);

                p.SetValue(obj, oV, null);
            }
        }

        public string UniqueName
        {
            get
            {
                return this.ParentSet.UniqueName + "." + this.Name;
            }
        }

        public readonly ComponentSet ParentSet;

        public readonly PropertyInfo Property;

        public readonly string Name;

        public readonly Type Type;

        public readonly string DisplayName;

        public readonly int MaxLength = -1;

        public readonly bool IsNullable = true;

        public readonly ValidationType ValidationType = ValidationType.Auto;

        public readonly string ValidationExpression = "";

        public readonly object[] CheckValues;

        public readonly EditorType EditorType = EditorType.Auto;

        public readonly object EditorArgs = null;

        public readonly ICheckValuesProvider CheckValuesProvider;

        public readonly bool Ignore = false;

        public readonly int Sequence;
    }
}
