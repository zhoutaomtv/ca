namespace CA.HrDataImporter.Extensions
{
    using System;
    using System.IO;
    using System.Configuration;

    public static class Logger
    {
        public static void Log(string msg)
        {
            msg = string.Format("{0:G} ==> \t{1}\r\n", DateTime.Now, msg);

            File.AppendAllText(GetLogFilePath(), msg);
        }

        private static string GetLogFilePath() 
        {
            string dir = ConfigurationManager.AppSettings["LogFilePath"];


            if (dir.IsNullOrWhitespace()) 
            {
                dir = ".";
            }

            Directory.CreateDirectory(dir);

            return Path.Combine(dir, "hr_data_import_log.log");
            
        }
    }
}
