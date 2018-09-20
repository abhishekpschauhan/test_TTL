using AddressProcessing.CSV;
using NUnit.Framework;
using System;
using System.IO;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVFileReaderTests
    {
        private static string tempDirectory = null;
        private static Random random = null;

        [TestFixtureSetUp]
        public void Init()
        {
            random = new Random();
            tempDirectory = Path.Combine(Path.GetTempPath(), random.Next().ToString());
            Directory.CreateDirectory(tempDirectory);
        }

        [TestFixtureTearDown]
        public void TearDownTests()
        {
            Directory.Delete(tempDirectory, true);
        }

        [Test]
        public void CSVFileReader_DoubleInitialization()
        {
            FileDataReader reader = new FileDataReader();
            string filePath = GetTempFilePath();
            reader.Open(filePath);
            Assert.Throws<InvalidOperationException>(() => reader.Open(GetTempFilePath()));
            reader.Close();
        }

        [Test]
        public void CSVFileReader_ReadWithoutInitialization()
        {
            FileDataReader reader = new FileDataReader();
            Assert.Throws<InvalidOperationException>(() => reader.ReadLine());
        }

        [Test]
        public void CSVFileReader_SuccessfulRead()
        {
            string filePath = GetTempFilePath();
            WriteData(filePath, "somedata");

            FileDataReader reader = new FileDataReader();
            reader.Open(filePath);

            string dataRead = null;
            Assert.DoesNotThrow(() => dataRead = reader.ReadLine());
            Assert.AreEqual("somedata", dataRead);

            reader.Close();
        }

        [Test]
        public void CSVFileReader_SuccessfulMultipleReads()
        {
            string filePath = GetTempFilePath();
            WriteData(filePath, "data1", "data2", "data3");

            FileDataReader reader = new FileDataReader();
            reader.Open(filePath);

            Assert.DoesNotThrow(() => reader.ReadLine());
            Assert.DoesNotThrow(() => reader.ReadLine());
            reader.Close();
        }

        [Test]
        public void CSVFileReader_SuccessfulMultipleReads_LessData()
        {
            string filePath = GetTempFilePath();
            WriteData(filePath, "data1");

            FileDataReader reader = new FileDataReader();
            reader.Open(filePath);

            Assert.DoesNotThrow(() => reader.ReadLine());
            Assert.DoesNotThrow(() => reader.ReadLine());
            reader.Close();
        }

        [Test]
        public void CSVFileReader_CleanupWithoutInitialization()
        {
            FileDataReader reader = new FileDataReader();
            Assert.DoesNotThrow(() => reader.Close());
        }

        [Test]
        public void CSVFileReader_MultipleCleanups()
        {
            FileDataReader reader = new FileDataReader();
            string filePath = GetTempFilePath();
            reader.Open(filePath);

            Assert.DoesNotThrow(() => reader.Close());
            Assert.DoesNotThrow(() => reader.Close());
        }

        private static string GetTempFilePath()
        {
            string fileName = string.Format("{0}.txt", random.Next());
            string tempFile = Path.Combine(tempDirectory, fileName);
            using (FileStream stream = File.Create(tempFile))
            {

            }

            return tempFile;
        }

        private static void WriteData(string filePath, params string[] data)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var dataLine in data)
                {
                    writer.WriteLine(dataLine);
                }
            }
        }
    }
}
