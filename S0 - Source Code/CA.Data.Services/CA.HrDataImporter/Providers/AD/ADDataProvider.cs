namespace CA.HrDataImporter.Providers.AD
{
    using System.Data;
    using System.DirectoryServices;
    using System.Linq;
    using Exceptions;
    using Extensions;
    using Mapping;

    public class ADDataProvider : IDataProviders
    {
        private const string UserSchemaClassName = "user";

        private readonly IADDataReader reader;
        private readonly DataMapping mapping;

        public ADDataProvider(IADDataReader reader, DataMapping mapping)
        {
            reader.AssertNotNull("reader");
            mapping.AssertNotNull("mapping");

            this.reader = reader;
            this.mapping = mapping;
        }

        #region IDataProvider Members

        public DataTable ReadAsDataTable()
        {
            DataTable tmpTable = this.ReadADRawData();

            return this.SyncDataTables(tmpTable);
        }

        #endregion

        private DataTable SyncDataTables(DataTable source)
        {
            DataTable result = this.mapping.BuildDataTableSchema();

            result.BeginLoadData();

            var rows = source.Rows;

            foreach (DataRow srcRow in rows)
            {
                DataRow dr = result.NewRow();

                var columns = source.Columns;

                foreach (DataColumn col in columns)
                {
                    var cm = this.mapping[col.ColumnName];

                    if (cm == null)
                    {
                        continue;
                    }

                    string dtColName = cm.DataTableColumnName;

                    if (srcRow.IsNull(col))
                    {
                        if (!cm.CanBeNull && cm.DefaultValue == null)
                        {
                            throw new NullRequiredFiledException(dtColName, srcRow[0].ToString());
                        }
                    }
                    else
                    {
                        dr[dtColName] = srcRow[col];
                    }
                }

                result.Rows.Add(dr);
            }

            result.EndLoadData();

            return result;
        }

        private DataTable ReadADRawData()
        {
            var tmpTable = this.BuildTempDataTableSchema();

            tmpTable.BeginLoadData();

            var results = this.reader.Read();

            if (results != null)
            {
                foreach (SearchResult result in results)
                {
                    DirectoryEntry user = result.GetDirectoryEntry();

                    if (!user.SchemaClassName.Equals(UserSchemaClassName))
                    {
                        continue;
                    }

                    DataRow row = tmpTable.NewRow();

                    var columns = tmpTable.Columns;

                    foreach (string colName in from DataColumn col in columns select col.ColumnName)
                    {
                        row[colName] = user.GetProperty(colName);
                    }

                    tmpTable.Rows.Add(row);
                }
            }

            tmpTable.EndLoadData();

            return tmpTable;
        }

        private DataTable BuildTempDataTableSchema()
        {
            var dt = new DataTable();

            foreach (string col in this.mapping.ColumnMappings.Select(item => item.Key))
            {
                dt.Columns.Add(col);
            }

            return dt;
        }
    }
}