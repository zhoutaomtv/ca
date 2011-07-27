namespace CA.HrDataImporter.Providers.Csv
{
    using System.Collections.Generic;

    /// <summary>
    /// Csv Row class.
    /// </summary>
    public class CsvRow
    {
        /// <summary>
        /// Csv columns data collection.
        /// </summary>
        private readonly List<string> columns = new List<string>();

        /// <summary>
        /// Gets the column data with the specified column index.
        /// </summary>
        /// <value>Column data.</value>
        public string this[int columnIndex]
        {
            get
            {
                return this.columns[columnIndex];
            }
        }

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>The line number.</value>
        public int LineNumber { get; set; }

        /// <summary>
        /// Gets the columns collection.
        /// </summary>
        /// <value>The columns.</value>
        public IList<string> Columns
        {
            get
            {
                return this.columns;
            }
        }

        /// <summary>
        /// Gets the total columns.
        /// </summary>
        /// <value>The total columns.</value>
        public int TotalColumns
        {
            get
            {
                return this.columns.Count;
            }
        }
    }
}