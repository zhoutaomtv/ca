//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：Attributes.cs
// 文件功能描述：组件设计元数据 
// 
// 
// 创建标识： 张建义 2007-7-3
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using System;
using System.Collections;
using System.Text;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 属性设计时特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EditorAttribute : System.Attribute
    {         
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName ;

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength = -1;

        //public bool IsNullable = true ;

        /// <summary>
        /// 校验类型
        /// </summary>
        public ValidationType ValidationType = ValidationType.Auto ;

        /// <summary>
        /// 校验正则表达式
        /// </summary>
        public string ValidationExpression = "" ;

        /// <summary>
        /// 约束值
        /// </summary>
        public object[] CheckValues ;         

        /// <summary>
        /// 约束值提供程序
        /// </summary>
        public Type CheckValuesProvider;

        /// <summary>
        /// 字段编辑器类型
        /// </summary>
        public EditorType EditorType = EditorType.Auto ;

        /// <summary>
        /// 字段编辑器参数
        /// </summary>
        public object[] EditorArgs = null ;   
     
        /// <summary>
        /// 设计时忽略
        /// </summary>
        public bool Ignore = false ;

        public int Sequence = 0;

    }

    /// <summary>
    /// 约束值提供程序接口
    /// </summary> 
    public interface ICheckValuesProvider
    {
        System.Collections.IDictionary GetCheckValues();         
    }

    internal class SimpleCheckValuesProvider : ICheckValuesProvider
    {
        private object[] _values;

        public SimpleCheckValuesProvider(object[] values)
        {
            _values = values;
        }

        #region ICheckValuesProvider 成员

        public System.Collections.IDictionary GetCheckValues()
        {
            IDictionary dic = new System.Collections.Specialized.ListDictionary();

            foreach (object o in _values)
            {
                dic.Add( o , o );
            }

            return dic;                 
        }

        #endregion
    }

    /// <summary>
    /// 验证类型
    /// </summary>
    //public enum ValidationType
    //{
    //    Auto = 20 ,

    //    Integer = 2,

    //    Double = 3,

    //    Currency= 4,

    //    //Date ,

    //    //Time ,

    //    DateTime= 5,

    //    Email= 6,

    //    Phone= 7,

    //    Mobile= 8,

    //    //Url,

    //    IdCard= 9,

    //    Number= 10,

    //    //Zip,

    //    //QQ,

    //    English= 11,

    //    Chinese= 12,

    //    // UnSafe ,

    //}

}
