using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint.CamlQuery
{
    /// <summary>
    ///  类型化查询字段
    /// typed query field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeFieldRef<TEntity, TField> : BaseFieldRef<TEntity>
        where TEntity : class, new()
    //where TEnum : struct
    {

        public TypeFieldRef(string name)
            : base(name)
        {
        }

        protected override CAMLExpression<TEntity> CreateExpression(string op, object value) 
        {
            SingleCAMLExpression<TEntity> q = new SingleCAMLExpression<TEntity>(this.FieldName, op, value, typeof(TField));
            return q;
        }      

        #region operator methods

        public CAMLExpression<TEntity> Equal(TField value)
        {
            return CreateExpression(ComparisonOperators.Equal, value);
        }

        public CAMLExpression<TEntity> NotEqual(TField value)
        {
            return CreateExpression(ComparisonOperators.NotEqual, value);
        }

        public CAMLExpression<TEntity> BeginsWith(TField value)
        {
            return CreateExpression(ComparisonOperators.BeginsWith, value);

        }

        public CAMLExpression<TEntity> Contains(TField value)
        {
            return CreateExpression(ComparisonOperators.Contains, value);

        } 

        public CAMLExpression<TEntity> LessThan(TField value)
        {
            return CreateExpression(ComparisonOperators.LessEqual, value);
        }

        public CAMLExpression<TEntity> MoreThan(TField value)
        {
            return CreateExpression(ComparisonOperators.MoreThan, value);
        }

        public CAMLExpression<TEntity> LessEqual(TField value)
        {
            return CreateExpression(ComparisonOperators.LessThan, value);
        }

        public CAMLExpression<TEntity> MoreEqual(TField value)
        {
            return CreateExpression(ComparisonOperators.MoreEqual, value);
        }

        #endregion


        #region operator overload

        public static CAMLExpression<TEntity> operator ==(TypeFieldRef<TEntity, TField> q, TField oValue)
        {
            return q.Equal(oValue); ;
        }

        public static CAMLExpression<TEntity> operator !=(TypeFieldRef<TEntity, TField> q, TField oValue)
        {
            return q.NotEqual(oValue);
        }

        public static CAMLExpression<TEntity> operator >(TypeFieldRef<TEntity, TField> q, TField oValue)
        {
            return q.MoreThan(oValue);
        }

        public static CAMLExpression<TEntity> operator <(TypeFieldRef<TEntity, TField> q, TField oValue)
        {
            return q.LessThan(oValue);
        }

        public static CAMLExpression<TEntity> operator >=(TypeFieldRef<TEntity, TField> q, TField oValue)
        {
            return q.MoreEqual(oValue); ;
        }

        public static CAMLExpression<TEntity> operator <=(TypeFieldRef<TEntity, TField> q, TField oValue)
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
