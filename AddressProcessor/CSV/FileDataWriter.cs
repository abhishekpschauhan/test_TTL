using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /// <summary>
    /// Class used write data to a file on disk or network location
    /// </summary>
    public class FileDataWriter : IDataWriter, IDisposable
    {
        private StreamWriter streamWriter = null;

        /// <inheritdoc />
        public void Open(string filePath)
        {
            if (streamWriter != null)
            {
                throw new InvalidOperationException("FileDataWriter is already initialized");
            }

            FileInfo fileInfo = new FileInfo(filePath);
            streamWriter = fileInfo.CreateText();
        }

        /// <inheritdoc />
        public void Close()
        {
            if (streamWriter != null)
            {
                // Close also disposes stream writer
                streamWriter.Close();
                streamWriter = null;
            }
        }

        /// <inheritdoc />
        public void WriteLine(string data)
        {
            if (streamWriter == null)
            {
                throw new InvalidOperationException("FileDataWriter is not initialized");
            }

            streamWriter.WriteLine(data);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (streamWriter != null)
            {
                // As stream writer is a managed resource, there is no need to
                // implement finalizer because effectively, the code
                // reduces to calling Dispose on the managed resource.
                // As dispose must be safe to be called multiple times, 
                // it is ok to call dispose on streamWriter multiple without any checks.
                // The complete implementation of IDisposable pattern can be found
                // in CSVFileReader implementation
                streamWriter.Dispose();
            }
        }
    }
}