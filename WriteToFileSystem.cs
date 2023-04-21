using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NccDataTransformerUpgrade
{
    class WriteToFileSystem
    {
        //Add default Constructor
        public WriteToFileSystem()
        {

        }
        /// <summary>
        /// Writes content of List<String> to the File System 
        /// using given file and path name.
        /// If appendToFile is true it appends to existing
        /// file or creats a the file if it does not exist.
        /// </summary>
        /// <param name="outputRecords"></param>
        /// <param name="filePathAndName"></param>
        /// <param name="appendToFile"></param>
        /// <returns>int  as number of records written to output file</returns>
        public int WriteToFile(List<String> outputRecords,
                            String filePathAndName, Boolean appendToFile)
        {
            try
            {
                using (StreamWriter localFileWriter = new StreamWriter(filePathAndName, appendToFile))
                {
                    //
                    int loopLimit = outputRecords.Count;
                    int loopCounter = 0;
                    while (loopCounter < loopLimit)
                    {
                        //Write record from List to output file
                        localFileWriter.WriteLine(outputRecords[loopCounter]);
                        //prevent infinite loop
                        loopCounter++;
                    }
                    //Close file
                    localFileWriter.Flush();
                    localFileWriter.Close();
                    return loopCounter;
                }
            }
            catch (IOException genericFileAccessError)
            {
                
                String errorMessage = "Problem writing to file" + filePathAndName;
                throw (new IOException(genericFileAccessError.Message));
            }
            finally
            {

            }                       
        }
    }
}
