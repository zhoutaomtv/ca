namespace CA.HrDataImporter.Providers.Csv
{
    public interface ICsvDataReader
    {
        /// <summary>
        /// Reads the CSV row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>Returns <c>true</c> if next row exists; else <c>false</c></returns>
        bool ReadRow(out CsvRow row);
    }
}