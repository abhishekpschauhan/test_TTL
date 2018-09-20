using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */
    /// <summary>
    /// Class responsible for reading and writing data in delimiter separated format. Default delimiter is '\t'.
    /// 
    /// It is desirable to have separate classes handling responsibility of writing, 
    /// reading and data formatting. As the code is in production and
    /// backward compatibility needs to be maintained, the responsibility for reading and writing
    /// data is extracted. Interfaces of reader / writer are used instead to build the CSVReaderWriter.
    /// This makes the class testable and reusable even if there are changes in the backing store of the data.
    /// </summary>
    public class CSVReaderWriter : ICSVReaderWriter
    {
        private IDataReader csvReader = null;
        private IDataWriter csvWriter = null;
        private char delimiter = '\t';
        private char[] delimiterArr = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public CSVReaderWriter()
        {
            csvReader = new FileDataReader();
            csvWriter = new FileDataWriter();
            delimiterArr = new char[] { delimiter };
        }

        public CSVReaderWriter(IDataReader reader, IDataWriter writer, char delimiter)
        {
            csvReader = reader;
            csvWriter = writer;
            this.delimiter = delimiter;
            delimiterArr = new char[] { delimiter };
        }

        /// <summary>
        /// Opens the <see cref="CSVReaderWriter"/> for reading or writing the data
        /// </summary>
        /// <param name="csvPath">Path or location of the CSV</param>
        /// <param name="mode">Whether to open for reading or writing</param>
        public void Open(string csvPath, Mode mode)
        {
            if (mode == Mode.Read)
            {
                if (csvReader == null)
                {
                    throw new ApplicationException("CSV reader is null");
                }

                // In older code, CSVReaderWriter can be opened for read multiple times
                // To maintain backward compatibility, close existing opened
                // reader, so that it can be reopened.
                csvReader.Close();
                csvReader.Open(csvPath);
            }
            else if (mode == Mode.Write)
            {
                if (csvWriter == null)
                {
                    throw new ApplicationException("CSV writer is null");
                }

                // In older code, CSVReaderWriter can be opened for write multiple times
                // To maintain backward compatibility, close existing opened
                // writer, so that it can be reopened.
                csvWriter.Close();
                csvWriter.Open(csvPath);
            }
            else
            {
                throw new Exception("Unknown file mode for " + csvPath);
            }
        }

        /// <summary>
        /// Writes column data
        /// </summary>
        /// <param name="columns">Columns to be written</param>
        public void Write(params string[] columns)
        {
            if (csvWriter == null)
            {
                throw new ApplicationException("CSV writer is null");
            }

            // In case input is null, store empty string
            string outPut = columns == null ? string.Empty : string.Join(delimiter.ToString(), columns);
            csvWriter.WriteLine(outPut);
        }
        
        public bool Read(string column1, string column2)
        {
            // Keeping the method for backward compatibility
            // Its ok to use column1 and column2 as out parameters as they are not used
            // later. It they were used elsewhere, new variables should be defined
            return Read(out column1, out column2);
        }

        /// <summary>
        /// Read the CSV data
        /// </summary>
        /// <param name="column1">Data in first column</param>
        /// <param name="column2">Data in second column</param>
        /// <returns>True, if the data read was successful. False, otherwise</returns>
        public bool Read(out string column1, out string column2)
        {
            if (csvReader == null)
            {
                throw new ApplicationException("CSV reader is null");
            }

            column1 = null;
            column2 = null;

            string line = csvReader.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                string[] columns = line.Split(delimiterArr, StringSplitOptions.None);
                if (columns.Length >= 2)
                {
                    column1 = columns[0];
                    column2 = columns[1];
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Close the reader writer
        /// </summary>
        public void Close()
        {
            // Close should be able to be called multiple times
            if (csvWriter != null)
            {
                csvWriter.Close();
            }

            if (csvReader != null)
            {
                csvReader.Close();
            }
        }

        public void OpenRead(string csvPath)
        {
            this.Open(csvPath, Mode.Read);
        }

        public void OpenWrite(string csvPath)
        {
            this.Open(csvPath, Mode.Write);
        }
    }
}
