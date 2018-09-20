using AddressProcessing.CSV;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVFileWriterTests
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
        public void CSVFileWriter_DoubleInitialization()
        {
            FileDataWriter writer = new FileDataWriter();
            string filePath = GetTempFilePath();
            writer.Open(filePath);
            Assert.Throws<InvalidOperationException>(() => writer.Open(GetTempFilePath()));
            writer.Close();
        }

        [Test]
        public void CSVFileWriter_WriteWithoutInitialization()
        {
            FileDataWriter writer = new FileDataWriter();
            string filePath = GetTempFilePath();
            Assert.Throws<InvalidOperationException>(() => writer.WriteLine(filePath));
        }

        [Test]
        public void CSVFileWriter_SuccessfulWrite()
        {
            FileDataWriter writer = new FileDataWriter();
            string filePath = GetTempFilePath();
            writer.Open(filePath);
            Assert.DoesNotThrow(() => writer.WriteLine("somedata"));
            writer.Close();
        }

        [Test]
        public void CSVFileWriter_SuccessfulMultipleWrites()
        {
            FileDataWriter writer = new FileDataWriter();
            string filePath = GetTempFilePath();
            writer.Open(filePath);
            Assert.DoesNotThrow(() => writer.WriteLine("somedata"));
            Assert.DoesNotThrow(() => writer.WriteLine("somemoredata"));
            writer.Close();
        }

        [Test]
        public void CSVFileWriter_CleanupWithoutInitialization()
        {
            FileDataWriter writer = new FileDataWriter();
            Assert.DoesNotThrow(() => writer.Close());
        }

        [Test]
        public void CSVFileWriter_MultipleCleanups()
        {
            FileDataWriter writer = new FileDataWriter();
            string filePath = GetTempFilePath();
            writer.Open(filePath);

            Assert.DoesNotThrow(() => writer.Close());
            Assert.DoesNotThrow(() => writer.Close());
        }

        private static string GetTempFilePath()
        {
            string fileName = string.Format("{0}.txt", random.Next());
            string tempFile = Path.Combine(tempDirectory, fileName);
            return tempFile;
        }
    }
}
