namespace AddressProcessing.CSV
{
    /// <summary>
    /// Interface implemented by objects used for reading data from a location, path or connection
    /// </summary>
    public interface IDataReader
    {
        /// <summary>
        /// Opens the data reader. Also used to initialize any objects for data read
        /// </summary>
        /// <param name="dataPath">Path, location or connection to the data</param>
        void Open(string dataPath);

        /// <summary>
        /// Reads one line from the data
        /// </summary>
        /// <returns>One line from the data</returns>
        string ReadLine();

        /// <summary>
        /// Closes the data reader. Also used to cleanup any objects used for data read
        /// </summary>
        void Close();
    }
}