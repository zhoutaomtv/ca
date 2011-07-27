namespace CA.HrDataImporter.Providers
{
    using Mapping;

    public enum ProviderType
    {
        AD,
        CSV
    }

    /// <summary>
    /// IDataProvider interface.
    /// </summary>
    interface IDataMapperFactory
    {
        /// <summary>
        /// Reads external data into a DataTable.
        /// </summary>
        /// <returns></returns>
        IDataMapper GetDataMapper(ProviderType type);
    }
}