namespace CA.HrDataImporter.Providers.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    ///   External data source to DataTable relationship mapping class
    /// </summary>
    public sealed class DataMapping
    {
        /// <summary>
        ///   Collection for storing mapping relationship. Use "DataFieldName" as keyword.
        /// </summary>
        private readonly Dictionary<string, ColumnMapping> mappings = new Dictionary<string, ColumnMapping>();

        /// <summary>
        ///   Gets the data mappings.
        /// </summary>
        /// <value>The data mappings.</value>
        public Dictionary<string, ColumnMapping> ColumnMappings
        {
            get
            {
                return this.mappings;
            }
        }

        /// <summary>
        ///   Gets the <see cref = "ColumnMapping" /> with the specified data source column name.
        /// </summary>
        /// <value>The object. Returns NULL if not found.</value>
        public ColumnMapping this[string dataFieldName]
        {
            get
            {
                return this.mappings.ContainsKey(dataFieldName) ? this.mappings[dataFieldName] : null;
            }
        }

        /// <summary>
        ///   Creates an empty data table.
        /// </summary>
        /// <returns>An empty DataTable.</returns>
        public DataTable BuildDataTableSchema()
        {
            var dt = new DataTable();

            foreach (ColumnMapping m in this.mappings.Select(item => item.Value))
            {
                var col = new DataColumn(m.DataTableColumnName, m.DataTableColumnType)
                {
                    AllowDBNull = m.CanBeNull
                };

                if (m.DefaultValue != null)
                {
                    col.DefaultValue = m.DefaultValue;
                }

                dt.Columns.Add(col);
            }

            return dt;
        }
    }
}