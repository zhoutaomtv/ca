using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web
{
    /// <summary>
    /// UI控件-对象映射异常
    /// </summary>
    public class ObjectMapException : Exception
    {
        public ObjectMapException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="p">发生异常的参数</param>
        public ObjectMapException(string message, Parameter p)
            : base(message)
        {
            _Parameter = p;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">发生异常的参数</param>
        /// <param name="initValue">UI值</param>
        /// <param name="toType">对象属性类型</param>
        /// <param name="innerException"></param>
        public ObjectMapException(Parameter p, object initValue, Type toType,
            Exception innerException)

            : base("不能将[ " + initValue + " ]转换为类型[ " + toType + " ] ", innerException)
        {
            _Parameter = p;
        }

        private Parameter _Parameter;
        /// <summary>
        /// 发生异常的参数 
        /// </summary>
        public Parameter Parameter
        {
            get
            {
                return _Parameter;
            }
        }
    }
    
}
