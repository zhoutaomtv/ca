namespace CA.HrDataImporter.Providers
{
    using System.Data;

    /// <summary>
    /// IDataProvider interface.
    /// </summary>
    interface IDataProviders
    {
        /// <summary>
        /// Reads external data into a DataTable.
        /// </summary>
        /// <returns></returns>
        DataTable ReadAsDataTable();
    }
}