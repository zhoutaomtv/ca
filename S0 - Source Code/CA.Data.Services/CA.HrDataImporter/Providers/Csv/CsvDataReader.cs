namespace CA.HrDataImporter.Providers.Csv
{
    using System;
    using System.IO;
    using System.Text;
    using Extensions;

    public sealed class CsvDataReader : ICsvDataReader, IDisposable
    {
        #region Local variables

        /// <summary>
        ///   Data buffer
        /// </summary>
        private readonly char[] buffer = new char[4096];

        /// <summary>
        ///   Stream reader
        /// </summary>
        private readonly TextReader reader;

        /// <summary>
        ///   CSV column separator
        /// </summary>
        private readonly char separatorChar;

        /// <summary>
        ///   Char index in buffer
        /// </summary>
        private int charIndex;

        /// <summary>
        ///   EndOfLine flag
        /// </summary>
        private bool eol;

        /// <summary>
        ///   EndOfStream flag
        /// </summary>
        private bool eos;

        /// <summary>
        ///   Data length
        /// </summary>
        private int length;

        /// <summary>
        ///   CSV line number
        /// </summary>
        private int lineNumber = 1;

        /// <summary>
        ///   Flag when the previous char was 0xD
        /// </summary>
        private bool previousWasCr;

        #endregion Local variables

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CsvDataReader" /> class.
        /// </summary>
        /// <param name = "csvFileName">Name of the CSV file.</param>
        /// <param name = "separatorChar">The separator char.</param>
        public CsvDataReader(string csvFileName, char separatorChar) : this(csvFileName, Encoding.Default, separatorChar)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CsvDataReader" /> class.
        /// </summary>
        /// <param name = "csvFileName">Name of the CSV file.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <param name = "separatorChar">The separator char.</param>
        public CsvDataReader(string csvFileName, Encoding encoding, char separatorChar)
        {
            encoding.AssertNotNull("encoding");

            if (!File.Exists(csvFileName))
            {
                throw new FileNotFoundException("Cannot find the CSV file: " + csvFileName);
            }

            this.reader = new StreamReader(csvFileName, encoding);

            this.separatorChar = separatorChar;
        }

        #region ICsvDataReader Members

        /// <summary>
        ///   Reads the CSV row.
        /// </summary>
        /// <param name = "row">The row.</param>
        /// <returns>Returns <c>true</c> if next row exists; else <c>false</c></returns>
        public bool ReadRow(out CsvRow row)
        {
            row = new CsvRow
            {
                LineNumber = this.lineNumber
            };

            while (true)
            {
                string col;

                if (!this.GetNextColumn(out col))
                {
                    return (row.TotalColumns > 0);
                }

                row.Columns.Add(col);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.reader != null)
            {
                this.reader.Dispose();
            }
        }

        #endregion

        /// <summary>
        ///   Gets the next CSV column.
        /// </summary>
        /// <param name = "col">The column.</param>
        /// <returns>eturns <c>true</c> if next column exists; else <c>false</c></returns>
        private bool GetNextColumn(out string col)
        {
            col = null;

            if (this.eol)
            {
                // Reset EndOfLine flag
                this.eol = false;
                return false;
            }

            bool lastCol = false;
            bool quoted = false;

            bool predata = true, postdata = false;

            var sb = new StringBuilder();

            while (true)
            {
                char c = this.GetNextChar(true);

                if (this.eos)
                {
                    if (lastCol)
                    {
                        col = sb.ToString();
                    }

                    return lastCol;
                }

                // Don't count CRLF as two line breaks.
                if (!this.previousWasCr && (c == '\x0A'))
                {
                    this.lineNumber++;
                }

                if (c == '\x0D')
                {
                    this.lineNumber++;
                    this.previousWasCr = true;
                }
                else
                {
                    this.previousWasCr = false;
                }

                if ((postdata || !quoted) && c == this.separatorChar)
                {
                    if (lastCol)
                    {
                        col = sb.ToString();
                    }

                    return true;
                }

                if ((predata || postdata || !quoted) && (c == '\x0A' || c == '\x0D'))
                {
                    this.eol = true;

                    if (c == '\x0D' && this.GetNextChar(false) == '\x0A')
                    {
                        this.GetNextChar(true);
                    }

                    if (lastCol)
                    {
                        col = sb.ToString();
                    }

                    return true;
                }

                if (predata && c == ' ')
                {
                    // Ignore preceeding whitespace.
                    continue;
                }

                if (predata && c == '"')
                {
                    // Quoted column data is starting.
                    quoted = true;
                    predata = false;
                    lastCol = true;
                    continue;
                }

                if (predata)
                {
                    // Data is starting without quotes.
                    predata = false;
                    sb.Append(c);
                    lastCol = true;
                    continue;
                }

                if (c == '"' && quoted)
                {
                    if (this.GetNextChar(false) == '"')
                    {
                        // Double quotes within quoted string means add a quote.
                        sb.Append(this.GetNextChar(true));
                    }
                    else
                    {
                        // Matching end-quote reached.
                        postdata = true;
                    }

                    continue;
                }

                sb.Append(c);
            }
        }

        /// <summary>
        ///   Gets the next char.
        /// </summary>
        /// <param name = "moveon">if set to <c>true</c> then continue next reading.</param>
        /// <returns>Next char.</returns>
        private char GetNextChar(bool moveon)
        {
            if (this.charIndex >= this.length)
            {
                this.length = this.reader.ReadBlock(this.buffer, 0, this.buffer.Length);

                if (this.length == 0)
                {
                    this.eos = true;
                    return '\0';
                }

                this.charIndex = 0;
            }

            return moveon ? this.buffer[this.charIndex++] : this.buffer[this.charIndex];
        }
    }
}