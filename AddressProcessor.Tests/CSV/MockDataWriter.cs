using AddressProcessing.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressProcessing.Tests.CSV
{
    public class MockDataWriter : IDataWriter
    {
        public Action<string> OpenDelegate { get; set; }

        public Action<string> WriteLineDelegate { get; set; }

        public Action CloseDelegate { get; set; }

        public MockDataWriter()
        {
            OpenDelegate = (s) => { };
            WriteLineDelegate = (s) => { };
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

        public void WriteLine(string data)
        {
            WriteLineDelegate(data);
        }
    }
}
