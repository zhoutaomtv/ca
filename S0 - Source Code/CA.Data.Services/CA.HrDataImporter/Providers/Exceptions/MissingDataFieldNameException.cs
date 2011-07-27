namespace CA.HrDataImporter.Providers.Exceptions
{
    /// <summary>
    ///   Thrown when the CSV column name was empty in the mapping XML file.
    /// </summary>
    public class MissingDataFieldNameException :BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingDataFieldNameException"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public MissingDataFieldNameException(string fileName)
            : base("No value provided for attribute \"DataFieldName\" when the \"CsvFirstLineHasColumnNames\" is set to true." + FileNameMessage(fileName))
        {
        }
    }
}