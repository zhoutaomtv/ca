namespace CA.HrDataImporter.Extensions
{
    using System;
    using System.ComponentModel;
    using System.DirectoryServices;
    using System.Linq;

    public static class TypeExtensions
    {
        public static bool IsNullOrWhitespace(this string input)
        {
            return input == null || input.All(char.IsWhiteSpace);
        }

        public static T To<T>(this string s, T defaultValue) where T: struct
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                return (T)converter.ConvertFromString(s);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void AssertNotNull<T>(this T argument, string argumentName) where T: class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static object GetProperty(this DirectoryEntry searchResult, string propertyName)
        {
            searchResult.AssertNotNull("searchResult");
            propertyName.AssertNotNull("propertyName");

            return searchResult.Properties.Contains(propertyName) ? searchResult.Properties[propertyName].Value : null;
        }
    }
}