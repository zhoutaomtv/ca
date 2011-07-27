namespace CA.HrDataImporter.Providers.Exceptions
{
    /// <summary>
    /// Duplicate DataTable Column Name Exception
    /// </summary>
    public class DuplicateDataTableColumnNameException: BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDataTableColumnNameException"/> class.
        /// </summary>
        /// <param name="colName">Name of the column.</param>
        /// <param name="fileName">Name of the file.</param>
        public DuplicateDataTableColumnNameException(string colName, string fileName)
            : base(string.Format("Duplicate value \"{0}\" defined for attribute \"Name\". ", colName) + FileNameMessage(fileName))
        {
        }
    }
}