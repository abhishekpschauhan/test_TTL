using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AddressProcessing.CSV;
using AddressProcessing.Tests.CSV;
using static AddressProcessing.CSV.CSVReaderWriter;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private char delimiter = '\t';

        /// <summary>
        /// Test that Open is not called on a null csv writer
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_NullWriterOpen()
        {
            var csvReaderWriter = new CSVReaderWriter(null, null, delimiter);
            Assert.Throws<ApplicationException>(() => csvReaderWriter.Open("somedata", Mode.Write));
            Assert.Throws<ApplicationException>(() => csvReaderWriter.OpenWrite("somedata"));
        }

        /// <summary>
        /// Test that Open is not called on a null csv writer
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_NullWriterOpen()
        {
            var csvReaderWriter = new CSVReaderWriter(null, null, delimiter);
            Assert.Throws<ApplicationException>(() => csvReaderWriter.Open("somedata", Mode.Read));
            Assert.Throws<ApplicationException>(() => csvReaderWriter.OpenRead("somedata"));
        }

        /// <summary>
        /// Test that write is not called on a null csv writer
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_NullWriter()
        {
            var csvReaderWriter = new CSVReaderWriter(null, null, delimiter);
            Assert.Throws<ApplicationException>(() => csvReaderWriter.Write("somedata"));
        }

        /// <summary>
        /// Test that write is not called on a null csv writer
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_NullReader()
        {
            string column1 = null;
            string column2 = null;
            var csvReaderWriter = new CSVReaderWriter(null, null, delimiter);
            Assert.Throws<ApplicationException>(() => csvReaderWriter.Read(out column1, out column2));
        }

        /// <summary>
        /// Test that csv writer is opened when CSVReaderWriter is opened for write
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_Opened()
        {
            int openCalled = 0;
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.OpenDelegate =
                        (s) =>
                        {
                            Assert.AreEqual("somefilepath", s);
                            openCalled++;
                        };

            var csvReaderWriter = new CSVReaderWriter(null, mockWriter, delimiter);
            csvReaderWriter.Open("somefilepath", Mode.Write);
            csvReaderWriter.OpenWrite("somefilepath");
            Assert.AreEqual(2, openCalled);
        }

        /// <summary>
        /// Test that csv reader is opened when CSVReaderWriter is opened for write
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_Opened()
        {
            int openCalled = 0;
            MockDataReader mockReader = new MockDataReader();
            mockReader.OpenDelegate =
                        (s) =>
                        {
                            Assert.AreEqual("somefilepath", s);
                            openCalled++;
                        };

            var csvReaderWriter = new CSVReaderWriter(mockReader, null, delimiter);
            csvReaderWriter.Open("somefilepath", Mode.Read);
            csvReaderWriter.OpenRead("somefilepath");
            Assert.AreEqual(2, openCalled);
        }

        /// <summary>
        /// Test the csv writer is not opened when CSVReaderWriter is opened for read
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_NotOpenedOnReadOpen()
        {
            MockDataReader mockReader = new MockDataReader();
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.OpenDelegate =
                        (s) =>
                        {
                            Assert.Fail("Open called for writer");
                        };

            var csvReaderWriter = new CSVReaderWriter(mockReader, mockWriter, delimiter);
            csvReaderWriter.Open("somefilepath", Mode.Read);
            csvReaderWriter.OpenRead("somefilepath");
        }

        /// <summary>
        /// Test the csv reader is not opened when CSVReaderWriter is opened for read
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_NotOpenedOnWriteOpen()
        {
            MockDataReader mockReader = new MockDataReader();
            MockDataWriter mockWriter = new MockDataWriter();
            mockReader.OpenDelegate =
                        (s) =>
                        {
                            Assert.Fail("Open called for reader");
                        };

            var csvReaderWriter = new CSVReaderWriter(mockReader, mockWriter, delimiter);
            csvReaderWriter.Open("somefilepath", Mode.Write);
            csvReaderWriter.OpenWrite("somefilepath");
        }

        /// <summary>
        /// Test case where no data is entered to write
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_NullData()
        {
            bool writeLineCalled = false;
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.WriteLineDelegate =
                    (s) =>
                    {
                        Assert.AreEqual(string.Empty, s);
                        writeLineCalled = true;
                    };

            var csvReaderWriter = new CSVReaderWriter(null, mockWriter, delimiter);
            csvReaderWriter.Write();
            Assert.True(writeLineCalled);
        }

        /// <summary>
        /// Test case where the result of read is null
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_NullData()
        {
            string column1 = null;
            string column2 = null;
            bool readLineCalled = false;
            MockDataReader mockReader = new MockDataReader();
            mockReader.ReadLineDelegate =
                () =>
                {
                    readLineCalled = true;
                    return null;
                };

            var csvReaderWriter = new CSVReaderWriter(mockReader, null, delimiter);
            Assert.IsFalse(csvReaderWriter.Read(out column1, out column2));
            Assert.IsTrue(readLineCalled);
        }

        /// <summary>
        /// Test case where the value of data to be written is null
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_ValidNullData()
        {
            bool writeLineCalled = false;
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.WriteLineDelegate =
                    (s) =>
                    {
                        Assert.AreEqual(string.Empty, s);
                        writeLineCalled = true;
                    };
            var csvReaderWriter = new CSVReaderWriter(null, mockWriter, delimiter);
            csvReaderWriter.Write(null);
            Assert.True(writeLineCalled);
        }

        /// <summary>
        /// Test case where the result of read is empty string
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_EmptyData()
        {
            string column1 = null;
            string column2 = null;
            bool readLineCalled = false;
            MockDataReader mockReader = new MockDataReader();
            mockReader.ReadLineDelegate =
                () =>
                {
                    readLineCalled = true;
                    return string.Empty;
                };

            var csvReaderWriter = new CSVReaderWriter(mockReader, null, delimiter);
            Assert.IsFalse(csvReaderWriter.Read(out column1, out column2));
            Assert.IsTrue(readLineCalled);
        }

        /// <summary>
        /// Test case where only one value is written as CSV
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_ValidSingleValueData()
        {
            bool writeLineCalled = false;
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.WriteLineDelegate =
                    (s) =>
                    {
                        Assert.AreEqual("somedata", s);
                        writeLineCalled = true;
                    };
            var csvReaderWriter = new CSVReaderWriter(null, mockWriter, delimiter);
            csvReaderWriter.Write("somedata");
            Assert.True(writeLineCalled);
        }

        /// <summary>
        /// Test case where the result of read is untabbed data
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_NoTabbedData()
        {
            string column1 = null;
            string column2 = null;
            bool readLineCalled = false;
            MockDataReader mockReader = new MockDataReader();
            mockReader.ReadLineDelegate =
                () =>
                {
                    readLineCalled = true;
                    return "somedata";
                };

            var csvReaderWriter = new CSVReaderWriter(mockReader, null, delimiter);
            Assert.IsFalse(csvReaderWriter.Read(out column1, out column2));
            Assert.IsTrue(readLineCalled);
        }

        /// <summary>
        /// Test case where multiple values are written as CSV
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_ValidMultiValueData()
        {
            bool writeLineCalled = false;
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.WriteLineDelegate =
                    (s) =>
                    {
                        Assert.AreEqual(string.Join(delimiter.ToString(), new string[] { "data1", "data2", "data3" }), s);
                        writeLineCalled = true;
                    };

            var csvReaderWriter = new CSVReaderWriter(null, mockWriter, delimiter);
            csvReaderWriter.Write("data1", "data2", "data3");
            Assert.True(writeLineCalled);
        }

        /// <summary>
        /// Test case where the result of read is multiple values separated by tabs
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_MultiValueTabbedData()
        {
            string column1 = null;
            string column2 = null;
            MockDataReader mockReader = new MockDataReader();
            mockReader.ReadLineDelegate = () => string.Join(delimiter.ToString(), new string[]{ "data1", "data2", "data3"});

            var csvReaderWriter = new CSVReaderWriter(mockReader, null, delimiter);

            Assert.IsTrue(csvReaderWriter.Read(out column1, out column2));
            Assert.AreEqual("data1", column1);
            Assert.AreEqual("data2", column2);
        }

        /// <summary>
        /// Test case where multiple values are written as CSV
        /// </summary>
        [Test]
        public void CSVReaderWriter_Write_CleanupCalledOnClose()
        {
            bool cleanupCalled = false;
            MockDataWriter mockWriter = new MockDataWriter();
            mockWriter.CloseDelegate =
                    () =>
                    {
                        cleanupCalled = true;
                    };

            var csvReaderWriter = new CSVReaderWriter(null, mockWriter, delimiter);
            csvReaderWriter.Close();
            Assert.IsTrue(cleanupCalled, "Cleanup was not called for data writer");
        }

        /// <summary>
        /// Test case where multiple values are written as CSV
        /// </summary>
        [Test]
        public void CSVReaderWriter_Read_CleanupCalledOnClose()
        {
            bool cleanupCalled = false;
            MockDataReader mockReader = new MockDataReader();
            mockReader.CloseDelegate =
                    () =>
                    {
                        cleanupCalled = true;
                    };

            var csvReaderWriter = new CSVReaderWriter(mockReader, null, delimiter);
            csvReaderWriter.Close();
            Assert.IsTrue(cleanupCalled, "Cleanup was not called for data reader");
        }
    }
}
