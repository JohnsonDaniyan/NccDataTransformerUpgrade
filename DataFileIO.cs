using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NccDataTransformerUpgrade
{
    class DataFileIO
    {
        public DataFileIO()
        {

        }

        /// <summary>
        /// This method reads file using given file and path name and
        /// returns the record is a List<String> data container
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        /// <returns>List<String></returns>
        internal List<string> ReturnFileContentAsStringList(string fileNameAndPath)
        {
            List<String> listOfRecords = new List<String>();
            String studentRecord;
            //
            try
            {
                StreamReader reader = new StreamReader(fileNameAndPath);
                while ((studentRecord = reader.ReadLine()) != null)
                {
                    //process(read record);
                    listOfRecords.Add(studentRecord.Trim());
                }
                //Close input file
                reader.Close();
            }
            catch (Exception genericFileAccessError)
            {
                String errorMessage = "Problem reading file" + fileNameAndPath +
                    Environment.NewLine + genericFileAccessError;
                throw (new Exception(errorMessage));
            }
            finally
            {
            }

            return listOfRecords;
        }
    }
}
