namespace CA.HrDataImporter.Providers.Exceptions
{
    /// <summary>
    ///   Thrown when the column name was empty in the mapping XML file.
    /// </summary>
    public class MissingDataTableColumnNameException :BaseException
    {
        public MissingDataTableColumnNameException(string fileName)
            : base("No value provided for required attribute \"Name\". " + FileNameMessage(fileName))
        {
        }
    }
}