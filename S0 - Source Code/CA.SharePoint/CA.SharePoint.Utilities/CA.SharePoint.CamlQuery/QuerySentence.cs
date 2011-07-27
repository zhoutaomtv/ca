using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;
 
namespace CA.SharePoint.CamlQuery
{
 
    /// <summary>
    /// 查询入口对象
    /// query gateway
    /// </summary>
    public static class ListQuery 
    {
        public static Select Select(params IFieldRef[] fields)
        {
            //IList<QueryExpression> sqlList = new List<QueryExpression>();
            QueryContext context = new QueryContext();

            Select s = new Select(context, fields);

            return s;
        }

        public static Select Select( uint top , params IFieldRef[] fields)
        {
            QueryContext context = new QueryContext();

            context.RowLimit = top;

            Select s = new Select(context, fields);

            return s;
        }

        public static Select Select(uint top)
        {
            QueryContext context = new QueryContext();

            context.RowLimit = top;

            Select s = new Select(context, null);

            return s;
        }

        public static Select Select()
        {
            QueryContext context = new QueryContext();

            Select s = new Select(context, null);

            return s;
        }
    }

    public class QueryContext
    {
        internal QueryContext() { }

        public  SPList List ;

        public string ListName;

        public IFieldRef[] ViewFields;

        public uint RowLimit;

        public ICAMLExpression Query ;

        public IDictionary<IFieldRef, bool> OrderByFields = new Dictionary<IFieldRef, bool>();

        public IFieldRef GroupByField ;

    }

    /// <summary>
    ///  查询语句
    /// </summary>
    public abstract class QuerySentence
    {
        internal QuerySentence(QueryContext context)
        {
            Context = context;
        }

        protected readonly QueryContext Context ;

        public string ToCAMLString()
        {
            return CAMLBuilder.View(this.Context);
        }
       
    }

    public abstract class ReturnableQuerySentence : QuerySentence
    {

        public ReturnableQuerySentence(QueryContext c)
            : base(c)
        {
        }

        public SPListItemCollection GetItems()
        {
            SPQuery query = new SPQuery();

            //if (Context.RowLimit > 0)
            //    query.RowLimit = Context.RowLimit;

            //if (null != Context.ViewFields && Context.ViewFields.Length > 0)
            //{
            //    query.ViewFields = CAMLBuilder.ViewFields(Context.ViewFields);
            //}

            //query.Query = CAMLBuilder.Where(Context.Query);
            query.ViewXml = CAMLBuilder.View(this.Context);

            SPListItemCollection items = Context.List.GetItems(query);

            return items;
        }

        
    }
 

    /// <summary>
    /// 选择
    /// select
    /// </summary>
    public class Select : QuerySentence
    {
        internal Select(QueryContext context , params IFieldRef[] fields) 
            : base( context )
        {
            context.ViewFields = fields ;
        }

        //internal readonly IFieldRef[] Fields;

        public From From( SPList list )
        {
            From from = new From(this.Context, list );
            return from;
        }
 
    }

    /// <summary>
    /// 
    /// </summary>
    public class From : QuerySentence
    {
        internal From(QueryContext context , SPList list ) 
            : base(context)
        {
            context.List = list;
        }        

        public Where Where( ICAMLExpression q) //where T : class
        {
            Where where = new Where(this.Context, q);

            return where;
        }

        public Order OrderBy(IFieldRef field)
        {
            return new Order(this.Context, field, true);
        }

        public Order OrderBy(IFieldRef field, bool desc)
        {
            return new Order(this.Context, field, desc);
        }         



    }
    
    public class Where : ReturnableQuerySentence
    {
        internal Where( QueryContext context , ICAMLExpression expr)
            : base(context)
        {
            context.Query = expr;
        }

        public Order OrderBy(IFieldRef field)
        {
            return new Order(this.Context, field, true);
        }

        public Order OrderBy(IFieldRef field, bool desc)
        {
            return new Order(this.Context, field, desc);
        }         

    }

    /// <summary>
    /// 排序
    /// </summary>
    public class Order : ReturnableQuerySentence
    {
        internal Order(QueryContext context, IFieldRef field, bool desc)
            : base(context)
        {
            context.OrderByFields.Add(field, desc);
        }

        public Order OrderBy( IFieldRef field , bool  desc )
        {
            this.Context.OrderByFields.Add(field, desc);
            return this;
        }  
       
    }

    /// <summary>
    /// 分组
    /// </summary>
    public class Group : ReturnableQuerySentence
    {
        internal Group(QueryContext context, IFieldRef field)
            : base(context)
        {
            context.GroupByField = field;
        }              

    }

//<GroupBy
//  Collapse = "TRUE" | "FALSE">
//  <FieldRef Name = "Field_Name"/>
//</GroupBy>


 
 
}

 