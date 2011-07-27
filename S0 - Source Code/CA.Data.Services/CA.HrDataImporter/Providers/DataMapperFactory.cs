namespace CA.HrDataImporter.Providers
{
    using System.Configuration;
    using Mapping;

    public class DataMapperFactory : IDataMapperFactory
    {
        public static readonly DataMapperFactory Instance = new DataMapperFactory();

        public IDataMapper GetDataMapper(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.AD:
                    return new DataMapper(ConfigurationManager.AppSettings["ADDataMappingFilePath"]);
                case ProviderType.CSV:
                    return new DataMapper(ConfigurationManager.AppSettings["CsvDataMappingFilePath"]);
            }

            return null;
        }
    }
}