using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 布局对象
    /// </summary>
    public class LayoutField
    {
        private string _FieldKey;
        /// <summary>
        /// 字段关键名
        /// </summary>
        public string FieldKey
        {
            get
            {
                return _FieldKey;
            }
            set
            {
                _FieldKey = value;
            }
        }
        private int _OrderPosition;
        /// <summary>
        /// 字段所处的位置
        /// </summary>
        public int OrderPosition
        {
            get
            {
                return _OrderPosition;
            }
            set
            {
                _OrderPosition = value;
            }
        }
        private string _ChsDisplayName;
        /// <summary>
        /// 中文显示名
        /// </summary>
        public string ChsDisplayName
        {
            get
            {
                return _ChsDisplayName;
            }
            set
            {
                _ChsDisplayName = value;
            }
        }
    }
}
