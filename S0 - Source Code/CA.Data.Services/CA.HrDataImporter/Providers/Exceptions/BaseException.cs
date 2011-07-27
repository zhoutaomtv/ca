namespace CA.HrDataImporter.Providers.Exceptions
{
    using System;
    using System.IO;

    /// <summary>
    /// Base exception class.
    /// </summary>
    public class BaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BaseException(string message) : base(message)
        {
        }

        /// <summary>
        ///
        /// Get the the file name message.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Message string.</returns>
        public static string FileNameMessage(string fileName)
        {
            return fileName == null ? string.Empty : " Reading file \"" + Path.GetFullPath(fileName) + "\".";
        }
    }
}