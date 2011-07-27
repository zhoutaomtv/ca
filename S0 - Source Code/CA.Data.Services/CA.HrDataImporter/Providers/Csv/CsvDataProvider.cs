namespace CA.HrDataImporter.Providers.Csv
{
    using System.Data;
    using Exceptions;
    using Extensions;
    using Mapping;

    /// <summary>
    ///   CSV to DataTable converter
    /// </summary>
    public class CsvDataProvider : IDataProviders
    {
        private const string LineNumberColumnName = "CSV_LINE_NUMBER";

        /// <summary>
        ///   Stores the mapping object.
        /// </summary>
        private readonly DataMapping mapping;

        /// <summary>
        ///   The CSV stream reader.
        /// </summary>
        private readonly ICsvDataReader reader;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CsvDataProvider" /> class.
        /// </summary>
        /// <param name = "reader">The reader.</param>
        /// <param name = "mapping">The mapping.</param>
        public CsvDataProvider(ICsvDataReader reader, DataMapping mapping)
        {
            reader.AssertNotNull("reader");
            mapping.AssertNotNull("mapping");

            this.mapping = mapping;
            this.reader = reader;
        }

        #region IDataProvider Members

        /// <summary>
        ///   Reads as data table.
        /// </summary>
        /// <returns>The DataTable object.</returns>
        public DataTable ReadAsDataTable()
        {
            DataTable tmpTable = this.ReadCsvRawData();

            return this.SyncDataTables(tmpTable);
        }

        #endregion

        /// <summary>
        ///   Syncs the data tables.
        /// </summary>
        /// <param name = "source">The source.</param>
        /// <returns>The DataTable object.</returns>
        private DataTable SyncDataTables(DataTable source)
        {
            DataTable result = this.mapping.BuildDataTableSchema();

            result.BeginLoadData();

            foreach (DataRow srcRow in source.Rows)
            {
                DataRow dr = result.NewRow();

                foreach (DataColumn col in source.Columns)
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

        /// <summary>
        ///   Reads the CSV raw data.
        /// </summary>
        /// <returns>A temp DataTable object.</returns>
        private DataTable ReadCsvRawData()
        {
            // Create a temp DataTable to hold data from CSV.
            var tmpTable = new DataTable();

            tmpTable.Columns.Add(LineNumberColumnName);

            tmpTable.BeginLoadData();

            CsvRow row;

            bool firstRow = true;

            while (this.reader.ReadRow(out row))
            {
                // Ignore empty lines
                if (row.TotalColumns == 1 && row[0].IsNullOrWhitespace())
                {
                    continue;
                }

                // In case the first line
                if (firstRow)
                {
                    foreach (string colName in row.Columns)
                    {
                        tmpTable.Columns.Add(colName);
                    }

                    firstRow = false;
                }
                else
                {
                    int i = 1;

                    DataRow dr = tmpTable.NewRow();

                    dr[LineNumberColumnName] = row.LineNumber;

                    var columns = row.Columns;

                    foreach (string col in columns)
                    {
                        if (!col.IsNullOrWhitespace())
                        {
                            dr[i] = col;
                        }

                        i++;
                    }

                    tmpTable.Rows.Add(dr);
                }
            }

            tmpTable.EndLoadData();

            return tmpTable;
        }
    }
}