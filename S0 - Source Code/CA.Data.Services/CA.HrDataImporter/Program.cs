namespace CA.HrDataImporter
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Extensions;
    using Microsoft.Office.Server;
    using Microsoft.Office.Server.UserProfiles;
    using Microsoft.SharePoint;
    using Providers;
    using Providers.AD;
    using Providers.Csv;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("HR data import job.");
            Console.WriteLine("Please do not close this window.");

            int totalImported = 0;
            int totalAdded = 0;

            try
            {
                string targetWebUrl = ConfigurationManager.AppSettings["CAWebSiteUrl"];

                Logger.Log("Start Job: Import HR data to " + targetWebUrl);

                Logger.Log("Data Mapping: Read CSV data mapping relations from " + ConfigurationManager.AppSettings["CsvDataMappingFilePath"]);

                var csvMapping = DataMapperFactory.Instance.GetDataMapper(ProviderType.CSV).GetMapping();

                DataTable csvTable;

                string csvFilePath = ConfigurationManager.AppSettings["CsvFilePath"];

                string encodingString = ConfigurationManager.AppSettings["CsvFileEncoding"];

                Encoding encoding = encodingString.IsNullOrWhitespace() ? Encoding.Default : Encoding.GetEncoding(encodingString);

                using (var csvReader = new CsvDataReader(csvFilePath, encoding, ConfigurationManager.AppSettings["CsvDataDelimiter"][0]))
                {
                    csvTable = new CsvDataProvider(csvReader, csvMapping).ReadAsDataTable();
                }

                var csvRows = csvTable.Rows;

                Logger.Log("Data Import: Read CSV data [" + csvRows.Count + " rows] from " + csvFilePath);

                Logger.Log("Data Mapping: Read AD data mapping relations from " + ConfigurationManager.AppSettings["ADDataMappingFilePath"]);

                var adMapping = DataMapperFactory.Instance.GetDataMapper(ProviderType.AD).GetMapping();

                DataTable adTable;

                string ldap = ConfigurationManager.AppSettings["LDAPString"];

                Logger.Log("Data Import: Read AD data from " + ldap);

                using (var adReader = new ADDataReader(ldap, adMapping.ColumnMappings.Keys.ToArray()))
                {
                    adTable = new ADDataProvider(adReader, adMapping).ReadAsDataTable();
                }

                string accountNameColumn = adMapping.ColumnMappings["sAMAccountName"].DataTableColumnName;

                // Update Manager Information for each employee.
                foreach (DataRow csvRow in csvRows)
                {
                    csvRow.BeginEdit();

                    string name = FindManagerAccountFromADTable(adTable, csvRow.Field<string>("ManagerId"), accountNameColumn);

                    csvRow["Manager"] = name;

                    csvRow.EndEdit();
                }

                bool logUpdatedUsers;

                bool.TryParse(ConfigurationManager.AppSettings["LogUpdatedUsers"], out logUpdatedUsers);

                using (var site = new SPSite(targetWebUrl))
                {
                    Logger.Log("Data Import: Read data import gray list.");

                    var exceptions = SPHelper.GetImportExceptionsFromSPList(site.RootWeb, ConfigurationManager.AppSettings["HRDataImportExceptionsListName"]);

                    var context = ServerContext.GetContext(site);

                    var upm = new UserProfileManager(context);

                    var columns = adTable.Columns;

                    foreach (DataRow r in adTable.Rows)
                    {
                        // User account and Employee ID should not be empty.
                        string account = r.Field<string>(accountNameColumn);
                        string employeeId = r.Field<string>("EmployeeId");

                        if (account.IsNullOrWhitespace() || employeeId.IsNullOrWhitespace())
                        {
                            continue;
                        }

                        // If specific account was listed in the import exceptions, ignore it.
                        if (exceptions.Any(e => e.Equals(account.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                        {
                            Logger.Log("Data Import: User [" + account + "] data will not update for the account is in the gray list.");
                            continue;
                        }

                        // Find in the CSV DataTable if any row has same employee ID.
                        foreach (DataRow row in from DataRow row in csvRows let id = row.Field<string>("EmployeeId") where employeeId.Trim().Equals(id) select row)
                        {
                            r.BeginEdit();

                            foreach (string colName in (from DataColumn col in columns select col.ColumnName).Where(colName => !row.IsNull((string) colName)))
                            {
                                r[colName] = row[colName];
                            }

                            r.EndEdit();

                            break;
                        }

                        try
                        {
                            var status = SPHelper.UpdateUserProfileByAccount(upm, account, r, columns);

                            if (status == UserProfileUpdateStatus.Updated)
                            {
                                if (logUpdatedUsers)
                                { 
                                    Logger.Log(string.Format("Data Import: User [{0}, {1}]  was updated.", account, employeeId)); 
                                }
                                totalImported++;
                            }
                            else
                            {
                                totalAdded++;
                            }
                        }
                        catch (SPException spex)
                        {
                            Logger.Log("Error: User [" + account + "] data was not updated. Error Message = " + spex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error: " + ex.Message);
            }

            string stat = string.Format(CultureInfo.InvariantCulture, "{0} employees were created, {1} employees were updated.", totalAdded, totalImported);

            Logger.Log("End Job: Data Import Completed. " + stat + " \r\n=======================================================");
        }

        private static string FindManagerAccountFromADTable(DataTable adTable, string managerId, string accountNameColName)
        {
            foreach (DataRow row in from DataRow row in adTable.Rows let id = row.Field<string>("EmployeeId") where !id.IsNullOrWhitespace() && id.Equals(managerId) select row)
            {
                return row.Field<string>(accountNameColName);
            }

            return string.Empty;
        }
    }
}