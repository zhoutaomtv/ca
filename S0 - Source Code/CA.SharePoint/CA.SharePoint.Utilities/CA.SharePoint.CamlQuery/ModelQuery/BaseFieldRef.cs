using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint.CamlQuery
{

    /// <summary>
    ///  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseFieldRef<T> : IFieldRef where T : class //, new()
    {
        private string _fieldName;
        public BaseFieldRef(string name)  
        {
            _fieldName = name;
        }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }        

        protected virtual CAMLExpression<T> CreateExpression( string op , object value ) //where T : class
        {
            SingleCAMLExpression<T> q = new SingleCAMLExpression<T>( _fieldName , op , value );
            return q;
        }
      
        #region operator methods  
  
        public CAMLExpression<T> IsNull
        {
            get
            {
                return CreateExpression(ComparisonOperators.IsNull, null );
            }
        }

        public CAMLExpression<T> IsNotNull
        {
            get
            {
                return CreateExpression(ComparisonOperators.IsNotNull, null );
            }
        }
       
        #endregion         

        public override int GetHashCode()
        {
            return base.GetHashCode() ;
        }
      
        
    }
}
