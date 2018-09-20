namespace AddressProcessing.CSV
{
    /// <summary>
    /// Interface implemented by objects used for writing data to a location, path or connection
    /// </summary>
    public interface IDataWriter
    {
        /// <summary>
        /// Opens the data writer. Also used to initialize any objects for data write
        /// </summary>
        /// <param name="dataPath">Path, location or connection to the data</param>
        void Open(string dataPath);

        /// <summary>
        /// Writes data to the path, location or connection
        /// </summary>
        /// <param name="data">Data to be written</param>
        void WriteLine(string data);

        /// <summary>
        /// Closes the data writer. Also used to cleanup any objects used for data write
        /// </summary>
        void Close();
    }
}