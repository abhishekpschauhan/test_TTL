using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        1) List three to five key concerns with this implementation that you would discuss with the junior developer. 

        Please leave the rest of this file as it is so we can discuss your concerns during the next stage of the interview process.

        *) Single Responsibility: 
        *               The class is responsible for 1. Converting data in correct format, 2. Writing the data to destination and 3. Reading data from a source.
        *               Changes in any of the three - format, type of destination or source, will force changes in the class. Hence, the class should be refacrtored to have only one responsibility
        *) Use of disposable resources:
        *               The class uses StreamReader and StreamWriter which implement IDisposable interface. 
        *               Hence, the class should also implement IDisposable interface and dispose the reader and writer.
        *) Replacability (Liskov Substitution): 
        *               The class does not have any interface. Though it maybe not be always required, here, no interface means it cannot be substited easily later with any other class. 
        *               For ex, if it is required to replace CSVReaderWriter with EncryptedReaderWriter, in case it is needed to store data encrypted, all references to this class will need to be modified.
        *) Multiple Open calls: 
        *               It is possible that the user of this class can call Open multiple times with different file names witout calling Close. In that case, streams opened in every call will remain open.
        *               Therefore, in the Open call it must be checked if the streams are already created or not.
        *) Inefficient string construction: 
        *               The Write method creates the string to be written in a loop which is inefficient. In C# strings are immutable. Constructing this way will create many intermediate strings. 
        *               Use string.Join instead.
        *) Possible Null Reference Exception: 
        *               It is possible that the user of this class might call ReadLine or Write before calling Open. This can cause null reference exception.
        *               Therefore, Before calling Read and Write on stream reader and writer, there must be checks to see if they are null or not.
        *) Possible IndexOutOfBounds Exception: 
        *               It is possible that file to read has just one line with no tabs. In that case, string.Split will have only one element. In this case column2 = columns[SECOND_COLUMN]; will cause index out of bounds exception
        *               Therefore, assignment to column1 and column2 must be done only if there are more than two elements in columns      
        *) Code duplication: 
        *               The two Read methods perform (almost) identical operations. The objective of the first Read method is not clear. Is it to just skip the next line in the file?
        *               If yes, then the second method can be used for this purpose. It adds overhead to maintain backward compatibility.     
        *) Redendent Arguments: 
        *               The two input arguments in first Read method are not required. 
        *               Even two local variables can work.
        *) Single Line Methods: 
        *               ReadLine and WriteLine have just one line in the body. 
        *               Though one line body methods are not always bad, but in this case, the one line can be used directly rather than declaring new methods.
    */

    public class CSVReaderWriterForAnnotation
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
