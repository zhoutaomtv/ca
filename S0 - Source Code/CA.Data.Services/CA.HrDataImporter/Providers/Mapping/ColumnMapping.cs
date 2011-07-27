namespace CA.HrDataImporter.Providers.Mapping
{
    using System;

    /// <summary>
    ///   External data source to DataTable columns relationship mapping class.
    /// </summary>
    public sealed class ColumnMapping
    {
        /// <summary>
        ///   Default DataTable Column Type : System.String.
        /// </summary>
        private static readonly Type DefaultDataTableColumnType = typeof (string);

        /// <summary>
        ///   DataTable column type variable.
        /// </summary>
        private Type type;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ColumnMapping" /> class.
        /// </summary>
        public ColumnMapping()
        {
            this.CanBeNull = true;
            this.type = DefaultDataTableColumnType;
        }

        /// <summary>
        ///   Gets or sets the name of the DataTable column.
        /// </summary>
        /// <value>The name of the data table column.</value>
        public string DataTableColumnName { get; set; }

        /// <summary>
        ///   Gets or sets the name of the data source column corresponding to the DataTable column.
        /// </summary>
        /// <value>The name of the data source column.</value>
        public string DataFieldName { get; set; }

        /// <summary>
        /// Gets or sets the type of the DataTable column.
        /// </summary>
        /// <value>The type of the data table column.</value>
        public Type DataTableColumnType
        {
            get
            {
                return this.type;
            }
            set
            {
                if (value != null)
                {
                    this.type = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the DBNULL is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed; otherwise, <c>false</c>.
        /// </value>
        public bool CanBeNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is placeholder.
        /// A placeholder column will never be assigned any value in the CsvDataTableReader.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this column is placeholder; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaceHolder { get; set; }

        /// <summary>
        ///   Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public object DefaultValue { get; set; }
    }
}