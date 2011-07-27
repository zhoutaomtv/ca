using System;

namespace CA.SharePoint.CamlQuery
{
    /// <summary>
    /// 查询字段接口
    /// query field 
    /// </summary>
    public interface IFieldRef
    {
        string FieldName { get; }   
    }

    /// <summary>
    /// 查询字段抽象类
    /// </summary>
    //public abstract class QueryField : IQueryField
    //{
    //    public abstract string FieldName { get; }      
    //}

}
