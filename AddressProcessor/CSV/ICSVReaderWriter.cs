namespace AddressProcessing.CSV
{
    public interface ICSVReaderWriter
    {
        void OpenRead(string csvPath);

        void OpenWrite(string csvPath);

        void Write(params string[] columns);

        bool Read(out string column1, out string column2);

        void Close();
    }
}