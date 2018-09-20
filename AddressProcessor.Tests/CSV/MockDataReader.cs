using AddressProcessing.CSV;
using System;

namespace Csv.Tests
{
    internal class MockDataReader : IDataReader
    {
        public Action<string> OpenDelegate { get; set; }

        public Func<string> ReadLineDelegate { get; set; }

        public Action CloseDelegate { get; set; }

        public MockDataReader()
        {
            OpenDelegate = (s) => { };
            ReadLineDelegate = () => string.Empty;
            CloseDelegate = () => { };
        }

        public void Close()
        {
            CloseDelegate();
        }

        public void Open(string csvPath)
        {
            OpenDelegate(csvPath);
        }

        public string ReadLine()
        {
            return ReadLineDelegate();
        }
    }
}