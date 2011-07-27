using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint.CamlQuery
{

    /// <summary>
    /// ²éÑ¯×Ö¶Î
    /// query field 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldRef<T> : BaseFieldRef<T> where T : class //, new()
    {
 
        public FieldRef(string name) : base(name)  
        {
        }
       
        #region operator methods  

        public CAMLExpression<T> Equal(object value)
        {
            return CreateExpression(ComparisonOperators.Equal, value);
        }

        public CAMLExpression<T> NotEqual(object value)
        {
            return CreateExpression( ComparisonOperators.NotEqual , value);
        }

        public CAMLExpression<T> BeginsWith(object value)
        {
            return CreateExpression(ComparisonOperators.BeginsWith, value);

        }

        public CAMLExpression<T> Contains(object value)
        {
            return CreateExpression(ComparisonOperators.Contains, value);
           
        }
                
        public CAMLExpression<T> LessThan(object value)
        {
            return CreateExpression(ComparisonOperators.LessThan, value);
        }

        public CAMLExpression<T> MoreThan(object value)
        {
            return CreateExpression(ComparisonOperators.MoreThan, value);
        }

        public CAMLExpression<T> LessEqual(object value)
        {
            return CreateExpression(ComparisonOperators.LessThan, value);
        }

        public CAMLExpression<T> MoreEqual(object value)
        {
            return CreateExpression(ComparisonOperators.MoreEqual, value);
        }

        #endregion         


        #region operator overload

        public static CAMLExpression<T> operator ==(FieldRef<T> q, object oValue)
        {
            return q.Equal(oValue); ;
        }

        public static CAMLExpression<T> operator !=(FieldRef<T> q, object oValue)
        {
            return q.NotEqual(oValue); ;
        }

        public static CAMLExpression<T> operator >(FieldRef<T> q, object oValue)
        {
            return q.MoreThan(oValue); ;
        }

        public static CAMLExpression<T> operator <(FieldRef<T> q, object oValue)
        {
            return q.LessThan(oValue); ;
        }

        public static CAMLExpression<T> operator >=(FieldRef<T> q, object oValue)
        {
            return q.MoreEqual(oValue); ;
        }

        public static CAMLExpression<T> operator <=(FieldRef<T> q, object oValue)
        {
            return q.LessEqual(oValue); ;
        }

        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
       
    }

     
}
