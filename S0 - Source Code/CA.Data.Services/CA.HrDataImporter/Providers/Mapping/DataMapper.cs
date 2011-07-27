namespace CA.HrDataImporter.Providers.Mapping
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;
    using Exceptions;
    using Extensions;

    /// <summary>
    ///   Implementation of IDataMapper interface.
    /// </summary>
    public class DataMapper : IDataMapper
    {
        private const string NewDateTimeDefaultValue = "now";
        private const string NewGuidDefaultValue = "new";
        private const string MappingXmlSchmaName = "CA.HrDataImporter.Providers.Schema.DataMapping.xsd";

        /// <summary>
        ///   Save the mapping XML file object.
        /// </summary>
        private readonly XmlDocument mappingFile;

        /// <summary>
        ///   Save the mapping XML file name.
        /// </summary>
        private readonly string mappingFileName;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DataMapper" /> class.
        /// </summary>
        /// <param name = "mappingFileName">Name of the mapping XML file.</param>
        public DataMapper(string mappingFileName)
        {
            if (!File.Exists(mappingFileName))
            {
                throw new FileNotFoundException("Cannot find the data mapping file:" + mappingFileName);
            }

            this.mappingFileName = mappingFileName;

            this.mappingFile = new XmlDocument();

            this.mappingFile.Load(mappingFileName);

            this.mappingFile.Schemas = GetMappingSchema();
        }

        #region IDataMapper Members

        /// <summary>
        /// Parses the XML mapping file.
        /// </summary>
        /// <returns>An external data source (CSV / AD) to DataTable mapping object</returns>
        public DataMapping GetMapping()
        {
            this.mappingFile.Validate(null);

            XmlElement root = this.mappingFile.DocumentElement;

            var columns = root.GetElementsByTagName("Column");

            var dataTableMapping = new DataMapping();

            foreach (XmlElement column in columns)
            {
                // Get DataTable column name. Cannot be empty.
                string dtColName = column.GetAttribute("Name");

                if (dtColName.IsNullOrWhitespace())
                {
                    throw new MissingDataTableColumnNameException(this.mappingFileName);
                }

                if (dataTableMapping.ColumnMappings.Values.Any(m => m.DataTableColumnName.Equals(dtColName)))
                {
                    throw new DuplicateDataTableColumnNameException(dtColName, this.mappingFileName);
                }

                bool placeholder = column.GetAttribute("PlaceHolder").To(false);

                // Get corresponding data source column name.
                string dataFieldName = placeholder ? "PH_" + dtColName : column.GetAttribute("DataFieldName");

                if (!placeholder) 
                {
                    if (dataFieldName.IsNullOrWhitespace())
                    {
                        throw new MissingDataFieldNameException(this.mappingFileName);
                    }

                    if (dataTableMapping.ColumnMappings.ContainsKey(dataFieldName))
                    {
                        throw new DuplicateDataFieldNameException(dataFieldName, this.mappingFileName);
                    }
                }                

                // Get DataTable column DbNullAllowed value. Default is true.
                bool nullable = placeholder ? true : column.GetAttribute("CanBeNull").To(true);

                // Get DataTable column DataType.
                Type colType = Type.GetType(column.GetAttribute("Type"), false);

                var columnMapping = new ColumnMapping
                {
                    DataTableColumnName = dtColName,
                    CanBeNull = nullable,
                    DataFieldName = dataFieldName,
                    DataTableColumnType = colType,
                    IsPlaceHolder = placeholder
                };

                // Parse default value
                string defaultValue = column.GetAttribute("DefaultValue");

                if (defaultValue.Length != 0)
                {
                    if (colType == typeof(DateTime) && defaultValue.Equals(NewDateTimeDefaultValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        columnMapping.DefaultValue = DateTime.Now;
                    }
                    else if (colType == typeof(Guid) && defaultValue.Equals(NewGuidDefaultValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        columnMapping.DefaultValue = new Guid();
                    }
                    else
                    {
                        // Simply assign without data type validation.
                        columnMapping.DefaultValue = defaultValue;
                    }
                }

                dataTableMapping.ColumnMappings.Add(dataFieldName, columnMapping);
            }

            return dataTableMapping;
        }

        #endregion

        /// <summary>
        ///   Gets the mapping schema.
        /// </summary>
        /// <returns>XmlSchemaSet object.</returns>
        private static XmlSchemaSet GetMappingSchema()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(MappingXmlSchmaName))
            {
                var schemas = new XmlSchemaSet();

                schemas.Add(XmlSchema.Read(stream, null));

                return schemas;
            }
        }
    }
}