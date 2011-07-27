using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace CA.Web
{
    /// <summary>
    /// 参数基类
    /// </summary>
    public abstract class Parameter // : WebControl
    {
        private string _Name;
        /// <summary>
        /// 参数名（对象属性）
        /// </summary> 
        [Description("参数名，对应到数据对象的字段名")]
        public string Name
        {
            get
            {
                return _Name ;
            }
            set
            {
                _Name = value;
            }
        }

        //private TypeCode _Type = TypeCode.String;
        ///// <summary>
        ///// 参数类型
        ///// </summary>
        //public TypeCode Type
        //{
        //    get
        //    {
        //        return _Type ;
        //    }
        //    set
        //    {
        //        _Type = value;
        //    }
        //}

        private ParameterDirection _ParameterDirection = ParameterDirection.InputOutput;
        /// <summary>
        /// 参数方向
        /// </summary>
        /// 
        [Description("参数值方向，是输入，输出还是双向")]
        public ParameterDirection ParameterDirection
        {
            get
            {
                return _ParameterDirection;
            }
            set
            {
                _ParameterDirection = value;
            }
        }

        private string _DefaultValue;
        /// <summary>
        /// 默认值（用户输入为空时的值）
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return _DefaultValue;
            }
            set
            {
                _DefaultValue = value;
            }
        }

        //public string PropertyName;

        /// <summary>
        /// 获取参数值 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public virtual object Evaluate(HttpContext context, Control control)
        {          
            return null;
        }

        /// <summary>
        /// 更新参数对应的UI对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="control"></param>
        public virtual void UpdateUI( object data , Control control)
        {
             
        }     
    }



    public enum ParameterDirection
    {
        // 摘要:
        //     参数是输入参数。
        Input = 1,
        //
        // 摘要:
        //     参数是输出参数。
        Output = 2,
        //
        // 摘要:
        //     参数既能输入，也能输出。
        InputOutput = 3,
        //
        // 摘要:
        //     参数表示诸如存储过程、内置函数或用户定义函数之类的操作的返回值。
        None = 6,
    }


}

//public class ParameterCollection : System.Collections.CollectionBase
//{
//    //private Control _owner;
//    //public ParameterCollection( Control owner )
//    //{
//    //    _owner = owner;
//    //}

//    public int Add(Parameter parameter)
//    {


//        return base.List.Add(parameter);
//    }
//    public int IndexOf(Parameter parameter)
//    {
//        return List.IndexOf(parameter);
//    }
//    public void Insert(int index, Parameter parameter)
//    {
//        List.Insert(index, parameter);
//    }

//    public void Remove(Parameter parameter)
//    {
//        List.Remove(parameter);
//    }

//    public void RemoveAt(int index)
//    {
//        List.RemoveAt(index);
//    }


//    //public void UpdateValues(HttpContext context, Control control)
//    //{
//    //    foreach (Parameter parameter1 in this )
//    //    {
//    //       // parameter1.UpdateValue(context, control);
//    //    }
//    //}

//    private int GetParameterIndex(string name)
//    {
//        for (int num1 = 0; num1 < base.Count; num1++)
//        {
//            if (string.Equals(this[num1].Name, name, StringComparison.OrdinalIgnoreCase))
//            {
//                return num1;
//            }
//        }
//        return -1;
//    }

//    public Parameter this[string name]
//    {
//        get
//        {
//            List[name];

//            int num1 = this.GetParameterIndex(name);
//            if (num1 == -1)
//            {
//                return null;
//            }
//            return this[num1];
//        }
//        set
//        {
//            int num1 = this.GetParameterIndex(name);
//            if (num1 == -1)
//            {
//                this.Add(value);
//            }
//            else
//            {
//                this[num1] = value;
//            }
//        }
//    }
//    public Parameter this[int index]
//    {
//        get
//        {
//            return (Parameter)this[index];
//        }
//        set
//        {
//            this[index] = value;
//        }
//    }

//}
