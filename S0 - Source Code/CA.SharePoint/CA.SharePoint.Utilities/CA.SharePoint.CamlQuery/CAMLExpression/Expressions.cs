using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace CA.SharePoint.CamlQuery
{

    public interface IFieldInternalNameProvider
    {
        string GetInternalName( string displayName );
    }

    public interface ICAMLExpression
    {
        void ToCAML(IFieldInternalNameProvider provider, XmlNode pNode);

        void ToCAML(XmlNode pNode);
    }

    public abstract class CAMLExpression<T> : ICAMLExpression 
    {
        private CAMLExpression<T> And(CAMLExpression<T> q)
        {
            return new JoinCAMLExpression<T>(LogicalJoin.And, this, q);
        }

        private CAMLExpression<T> Or(CAMLExpression<T> q)
        {
            return new JoinCAMLExpression<T>(LogicalJoin.Or, this, q);
        }

        public static CAMLExpression<T> operator &(CAMLExpression<T> q1, CAMLExpression<T> q2)
        {
            return q1.And(q2);
        }

        public static CAMLExpression<T> operator |(CAMLExpression<T> q1, CAMLExpression<T> q2)
        {
            return q1.Or(q2);
        }

        /// <summary>
        /// Operator trues the specified right.
        /// </summary>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator true(CAMLExpression<T> right)
        {
            return false;
        }

        /// <summary>
        /// Operator falses the specified right.
        /// </summary>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator false(CAMLExpression<T> right)
        {
            return false;
        }

        #region ICAMLExpression Members

        public virtual void ToCAML(XmlNode pNode)
        {
            ToCAML(null, pNode);
        }

        public abstract void ToCAML(IFieldInternalNameProvider provider , XmlNode parentNode );

        public override string ToString()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml( "<Where/>" );

            this.ToCAML( null , doc.DocumentElement);

            return doc.InnerXml;
        }
 
        #endregion
    }


    public class SingleCAMLExpression<T> : CAMLExpression<T>
    {
        public SingleCAMLExpression(string field, string op, object value)
        {
            FieldName = field;

            Operator = op;
            Value = value;
        }

        public SingleCAMLExpression(string field, string op, object value, Type valueType) : this( field , op , value )
        {
            ValueType = valueType;
        }

        public readonly string FieldName;

        public readonly string Operator;

        public readonly object Value;

        private Type _ValueType = typeof(Object);
        private SPFieldType _SPFieldType = SPFieldType.Text;

        public Type ValueType
        {
            set
            {
                if (value == typeof(String) || value == typeof(Object))
                    _SPFieldType = SPFieldType.Text;
                else if (value == typeof(Int32) || value == typeof(Int16) || value == typeof(Int64))
                    _SPFieldType = SPFieldType.Integer;
                else if (value == typeof(Decimal) || value == typeof(float) || value == typeof(Single))
                    _SPFieldType = SPFieldType.Number;
                else if (value == typeof(Boolean))
                    _SPFieldType = SPFieldType.Boolean;
                else if (value == typeof(DateTime))
                    _SPFieldType = SPFieldType.DateTime;
            }
        }

        #region ICAMLExpression Members

        public override void ToCAML( IFieldInternalNameProvider provider , XmlNode parentNode )
        {
            XmlElement node = parentNode.OwnerDocument.CreateElement(Operator);
            parentNode.AppendChild( node );

            XmlNode fieldNode = parentNode.OwnerDocument.CreateElement( "FieldRef" );
            node.AppendChild(fieldNode);

            XmlAttribute att = parentNode.OwnerDocument.CreateAttribute( "Name" );

            if (provider == null)
                att.Value = FieldName;
            else
            {
                att.Value = provider.GetInternalName(FieldName);
            }

            fieldNode.Attributes.Append( att );

            XmlNode valueNode = parentNode.OwnerDocument.CreateElement( "Value" );          
            node.AppendChild( valueNode );

            if (_SPFieldType == SPFieldType.DateTime)
            {
                valueNode.InnerText = SPUtility.CreateISO8601DateTimeFromSystemDateTime( DateTime.Parse(Value.ToString()) );
            }
            else
            {
                valueNode.InnerText = "" + Value;
            }

            XmlAttribute att2 = parentNode.OwnerDocument.CreateAttribute( "Type" );
            att2.Value = _SPFieldType.ToString() ;
            valueNode.Attributes.Append( att2 );    
            //throw new Exception("The method or operation is not implemented.");
        }

        

        #endregion
    }

    public class JoinCAMLExpression<T> : CAMLExpression<T>
    {
        public readonly LogicalJoin LogicalJoin;

        public readonly CAMLExpression<T> Left;
        public readonly CAMLExpression<T> Right;

        public JoinCAMLExpression( LogicalJoin join , CAMLExpression<T> left ,CAMLExpression<T> right )
        {
            LogicalJoin = join;
            Left = left;
            Right = right;
        }

        public override void ToCAML(IFieldInternalNameProvider provider , XmlNode parentNode)
        {
            XmlElement node = parentNode.OwnerDocument.CreateElement( LogicalJoin.ToString() );
           
            parentNode.AppendChild( node );

            Left.ToCAML(provider, node);
            Right.ToCAML(provider, node);
        }
    }

    public enum LogicalJoin
    {
        And,
        Or
    }



    public static class  ComparisonOperators
    {
        
       public const string BeginsWith = "BeginsWith" ;
       
       public const string Contains = "Contains"; 

       public const string Equal =  "Eq" ;
             
       public const string MoreEqual = "Geq" ;                    

       public const string MoreThan =  "Gt";
 
       public const string LessEqual =  "Leq" ;
 
       public const string LessThan =  "Lt";             

       public const string NotEqual =  "Neq";

       public const string DateRangesOverlap = "DateRangesOverlap";

       public const string IsNotNull = "IsNotNull";

       public const string IsNull = "IsNull";               


    }

}



