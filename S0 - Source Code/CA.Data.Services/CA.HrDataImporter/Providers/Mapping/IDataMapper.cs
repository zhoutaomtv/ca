namespace CA.HrDataImporter.Providers.Mapping
{
    /// <summary>
    /// Interface for DataMapper.
    /// </summary>
    public interface IDataMapper
    {
        /// <summary>
        /// Parses the XML mapping file.
        /// </summary>
        /// <returns>An external data source (CSV / AD) to DataTable mapping object</returns>
        DataMapping GetMapping();
    }
}