namespace CA.HrDataImporter.Providers.AD
{
    using System.DirectoryServices;

    public interface IADDataReader
    {
        SearchResultCollection Read();
    }
}