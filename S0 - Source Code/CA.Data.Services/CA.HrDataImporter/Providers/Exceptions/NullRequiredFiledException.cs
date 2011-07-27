namespace CA.HrDataImporter.Providers.Exceptions
{
    /// <summary>
    /// Null required field exception.
    /// </summary>
    public class NullRequiredFiledException: BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullRequiredFiledException"/> class.
        /// </summary>
        /// <param name="dtColName">Name of the dt col.</param>
        /// <param name="lineNumber">The line number.</param>
        public NullRequiredFiledException(string dtColName, string lineNumber)
            : base(string.Format("Value for the required filed \"{0}\" is missing in the CSV file at line: {1}.", dtColName, lineNumber))
        {
        }
    }
}