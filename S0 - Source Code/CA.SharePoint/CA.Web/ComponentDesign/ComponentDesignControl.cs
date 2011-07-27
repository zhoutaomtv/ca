//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：FieldEditorFactory.cs
// 文件功能描述：组件设计器，对一个组件的属性进行编辑 ,支持对象关联，暂不支持列表属性
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
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using CA.Web.ComponentDesign;

namespace CA.Web
{
    /// <summary>
    /// 动态创建控件事件参数
    /// </summary>
    public class ControlCreatedEventArgs : EventArgs
    {
        internal ControlCreatedEventArgs(System.Web.UI.WebControls.WebControl ctl, FieldSet f)
        {
            Control = ctl;
            FieldSet = f;
        }
        /// <summary>
        /// 被创建的控件 
        /// </summary>
        public readonly System.Web.UI.WebControls.WebControl Control;

        /// <summary>
        /// 控件对应的字段信息 
        /// </summary>
        public readonly FieldSet FieldSet;
    }

    /// <summary>
    /// 动态创建控件事件代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ControlCreatedEventHandler(object sender, ControlCreatedEventArgs e);
    
    /// <summary>
    /// 组件的UI创建类别
    /// </summary>
    public enum BuilderType
    {
        /// <summary>
        /// 表格布局
        /// </summary>
        Layoutable,

        /// <summary>
        /// 层次布局
        /// </summary>
        Hierarchial
    }

    /// <summary>
    /// 组件设计器
    /// </summary>
    [ParseChildren(true)]
    public class ComponentDesignControl : Table, System.Web.UI.INamingContainer, IComponentDesigner
    {
        const string DESIENED_OBJECT = "DESIENED_OBJECT";

        private bool _ReadOnly = false;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
            }
        }

        private HierarchicalUIBuilder _HierarchicalUIBuilder = new HierarchicalUIBuilder ();
        /// <summary>
        /// 
        /// </summary>
        public HierarchicalUIBuilder HierarchicalUIBuilder
        {
            get { return _HierarchicalUIBuilder; }
        }

        private LayoutableUIBuilder _LayoutableUIBuilder = new LayoutableUIBuilder();
        public LayoutableUIBuilder LayoutableUIBuilder
        {
            get
            {
                return _LayoutableUIBuilder;
            }
        }

        private string _NameCellCssClass;
        /// <summary>
        /// 字段名单元格样式
        /// </summary>
        public string NameCellCssClass
        {
            get { return _NameCellCssClass; }
            set { _NameCellCssClass = value; }
        }

        private string _ValueCellCssClass;
        /// <summary>
        /// 字段值单元格样式
        /// </summary>
        public string ValueCellCssClass
        {
            get { return _ValueCellCssClass; }
            set { _ValueCellCssClass = value; }
        }

        private string _FieldNameFormatString;
        /// <summary>
        /// 字段名格式化字符串
        /// </summary>
        public string FieldNameFormatString
        {
            get { return _FieldNameFormatString; }
            set { _FieldNameFormatString = value; }
        }

        private ComponentSet _ComponentSet;
        private object _DesignedObject;

        private IDesignerUIBuilder _UIBuilder;
        /// <summary>
        /// 返回当前UIBuilder
        /// </summary>
        internal IDesignerUIBuilder UIBuilder 
        {
            get
            {
                if (_UIBuilder == null)
                {
                    if (_BuilderType == BuilderType.Hierarchial)
                        _UIBuilder = _HierarchicalUIBuilder;
                    else
                        _UIBuilder = _LayoutableUIBuilder;
                }

                return _UIBuilder;
            }
        }

        private BuilderType _BuilderType=BuilderType.Hierarchial;
        /// <summary>
        /// 组件的布局类型
        /// </summary>
        [Browsable(true)]
        public BuilderType BuilderType
        {
            get
            {
                return _BuilderType;
            }
            set 
            {
                _BuilderType = value;
                if (_BuilderType == BuilderType.Hierarchial)
                    _UIBuilder = _HierarchicalUIBuilder;
                else 
                    _UIBuilder = _LayoutableUIBuilder;
            }
        }

        //记录所有编辑控件
        private IDictionary<string, IFiledEditor> FieldSetEditors = new Dictionary<string, IFiledEditor>();

        //private string _LayoutString;
        ///// <summary>
        ///// 布局字符串,采用','间隔字段，|换行 Name,Sex,Birthday|UserType 
        ///// </summary>
        //public string LayoutString
        //{
        //    get { return _LayoutString; }
        //    set { _LayoutString = value; }
        //}

        //private List<LayoutField> _LayoutFields=new List<LayoutField>();
        ///// <summary>
        ///// 对象的字段的布局对象的集合
        ///// </summary>        
        //public List<LayoutField> LayoutFields
        //{
        //    get
        //    {
        //        return _LayoutFields;
        //    }
        //    set
        //    {
        //        _LayoutFields = value;
        //    }
        //}

        private int _RepeatColumns = 1;
        /// <summary>
        /// 列重复数，2列，4列，6列，必须为偶数
        /// </summary>
        public int RepeatColumns
        { 
            get { return _RepeatColumns; }
            set 
            {
                 _RepeatColumns = value; 
            }
        }

        /// <summary>
        /// 动态创建控件事件
        /// </summary>
        public event ControlCreatedEventHandler ControlCreated;

        internal void RaiseControlCreatedEvent(System.Web.UI.WebControls.WebControl ctl, FieldSet f)
        {
            if (ControlCreated != null)
                ControlCreated(this, new ControlCreatedEventArgs(ctl, f));
        }

        public void ShowComponent(object obj)
        {
            _ComponentSet = ComponentMetaDataFactctory.GetMetaData(obj.GetType());
            //BuildReadonlyUI(_ComponentSet, obj);
            UIBuilder.BuildViewUI(this, _ComponentSet, obj);
        }

        /// <summary>
        /// 初始化组件设计UI
        /// </summary>
        /// <param name="obj"></param>
        public void EditComponent(object obj)
        {
            this.Rows.Clear();

            Type t = obj.GetType();
            _DesignedObject = obj;

            _ComponentSet = ComponentMetaDataFactctory.GetMetaData(t);

            this.ViewState[DESIENED_OBJECT] = obj;

            if( ReadOnly )
                UIBuilder.BuildViewUI(this, _ComponentSet, _DesignedObject);
            else
                this.FieldSetEditors = UIBuilder.BuildEditUI(this, _ComponentSet, _DesignedObject);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page.IsPostBack)
            {
                InitControls();
            }
        }

        private void InitControls()
        {
            if (this.ViewState[DESIENED_OBJECT] != null)
            {
                this.Rows.Clear();

                _DesignedObject = this.ViewState[DESIENED_OBJECT];

                _ComponentSet = ComponentMetaDataFactctory.GetMetaData(_DesignedObject.GetType());

                if (ReadOnly)
                    UIBuilder.BuildViewUI(this, _ComponentSet, _DesignedObject);
                else
                    this.FieldSetEditors = UIBuilder.BuildRetrieveUI(this, _ComponentSet);
            }
        }

        /// <summary>
        /// 填充组件属性值
        /// </summary>
        /// <param name="obj"></param>
        public object GetComponent()
        {
            if (_ComponentSet == null)
                throw new Exception("not define edit control.");

            IDictionary<string, object> dic = new Dictionary<string, object>();

            foreach (KeyValuePair<string, IFiledEditor> kv in this.FieldSetEditors)
            {
                dic.Add(kv.Key, kv.Value.FieldValue);
            }

            _ComponentSet.UpdateValue(dic, _DesignedObject);

            return _DesignedObject;
        }

    }

   
}