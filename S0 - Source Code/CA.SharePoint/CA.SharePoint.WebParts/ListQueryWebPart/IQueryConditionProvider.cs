using System;
using System.Collections.Generic;
using System.Text;

namespace CA.SharePoint
{

    /// <summary>
    /// 查询条件提供接口 
    /// </summary>
    public interface IQueryConditionProvider
    {
        QueryCondition QueryCondition { get; }
    }

    /// <summary>
    /// 查询条件对象
    /// </summary>
    [Serializable]
    public class QueryCondition
    {
        private int[] _ItemIDs;
        /// <summary>
        /// 选择的项目ID
        /// </summary>
        public int[] ItemIDs
        {
            get { return _ItemIDs; }
            set { _ItemIDs = value; }
        }

        private string _Query;
        /// <summary>
        /// 包含Where和OrderBy
        /// </summary>
        public string Query
        {
            get { return _Query; }
            set { _Query = value; }
        }

        private string _Where;
        /// <summary>
        /// 只有Where
        /// </summary>
        public string Where
        {
            get { return _Where; }
            set { _Where = value; }
        }

        private string _OrderBy;
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy
        {
            get { return _OrderBy; }
            set { _OrderBy = value; }
        }

        private Guid _ViewId;
        /// <summary>
        /// 
        /// </summary>
        public Guid ViewId
        {
            get { return _ViewId; }
            set { _ViewId = value; }
        }

        private List<String> _ExportFields;
        /// <summary>
        /// 导出字段
        /// </summary>
        public List<String> ExportFields
        {
            get { return _ExportFields; }
            set { _ExportFields = value; }
        }

        public string GetExportFieldsXml()
        {
            if( this.ExportFields == null || this.ExportFields.Count == 0 ) return "";

            StringBuilder sb = new StringBuilder();

            foreach( String s in ExportFields )
            {
                sb.Append("<FieldRef Name='");
                sb.Append(s);
                sb.Append("'/>");
            }

            return sb.ToString();
        }
    }

    class QueryConditionProvider : IQueryConditionProvider
    {
        QueryCondition _q;
        public QueryConditionProvider(string q,List<String> exportFields)
        {
            _q = new QueryCondition();
            _q.Query = q;
            _q.ExportFields = exportFields;
        }

        #region IQueryProvider 成员

        public QueryCondition QueryCondition
        {
            get { return _q; }
        }

        #endregion
    }
}
