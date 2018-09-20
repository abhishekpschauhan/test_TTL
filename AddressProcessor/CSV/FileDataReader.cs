using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /// <summary>
    /// Class used to read data to a file on disk or network location
    /// </summary>
    public class FileDataReader : IDataReader, IDisposable
    {
        StreamReader streamReader = null;
        private bool disposed = false;

        /// <inheritdoc />
        public void Open(string filePath)
        {
            if (streamReader != null)
            {
                throw new InvalidOperationException("FileDataReader is already opened");
            }

            streamReader = File.OpenText(filePath);
        }

        /// <inheritdoc />
        public void Close()
        {
            if (streamReader != null)
            {
                streamReader.Close();
                streamReader = null;
            }
        }

        /// <inheritdoc />
        public string ReadLine()
        {
            if (streamReader == null)
            {
                throw new InvalidOperationException("FileDataReader is not initialized");
            }

            return streamReader.ReadLine();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FileDataReader()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (streamReader != null)
                    {
                        streamReader.Dispose();
                    }
                }

                disposed = true;
            }
        }
    }
}