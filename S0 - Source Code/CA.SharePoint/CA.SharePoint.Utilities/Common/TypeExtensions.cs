namespace CA.SharePoint.Utilities.Common
{
    using System;

    public static class TypeExtensions
    {
        public static bool IsNotNullOrWhitespace(this string input)
        {
            return input != null && input.Trim().Length != 0;
        }

        public static bool IsNullOrWhitespace(this string input)
        {
            return input == null || input.Trim().Length == 0;
        }

        public static void AssertNotNull<T>(T input, string paramName) where T : class
        {
            if (input == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static string AsString<T>(this T input) where T :class
        {
            return input == null ? string.Empty : input.ToString();
        }
    }
}