namespace CA.HrDataImporter.Providers.Exceptions
{
    /// <summary>
    /// Duplicate Csv Column Name Exception.
    /// </summary>
    public class DuplicateDataFieldNameException: BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDataFieldNameException"/> class.
        /// </summary>
        /// <param name="dataFieldName">Name of the data field.</param>
        /// <param name="fileName">Name of the file.</param>
        public DuplicateDataFieldNameException(string dataFieldName, string fileName)
            : base(string.Format("Duplicate value \"{0}\" defined for attribute \"DataFieldName\". ", dataFieldName) + FileNameMessage(fileName))
        {
        }
    }
}