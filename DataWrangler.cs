using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NccDataTransformerUpgrade
{
    class DataWrangler
    {
        DataCleaner dataCleanerObj = new DataCleaner();
        DataFileIO dataFileIOObj = new DataFileIO();
        WriteToFileSystem writeToFileSystemObj = new WriteToFileSystem();

        private List<string> operatorFileList;
        private List<string> subscriberCountFileList;

        public void CreateOperatorDataForR(List<string> nccDataInString,
                       string ncc2015OperatorDataR_Input)
        {

            List<String> operatorDataOutput = new List<String>();
            foreach (String inputRecord in nccDataInString)
            {
                //replace all "|" with "," to create CSV file
                string ouputString = inputRecord.Replace('|', ',');
                operatorDataOutput.Add(inputRecord.Replace('|', ','));
                //string ouputString = inputRecord;
            }
            writeToFileSystemObj.WriteToFile(operatorDataOutput, "ncc2015OperatorDataR_Input", false);



        }

        public void ProcessOperatorAndCounts(List<string> nccDataInString,
            string[] reportQuaters, string operatorFile, string subscriberCountFile)
        {
            //skip past the header record and process the rest as
            //Operator and subscriberCount records

            operatorFileList = new List<string>();
            subscriberCountFileList = new List<string>();




            for (int loopper = 1; loopper < nccDataInString.Count; loopper++)
            {
                String operatorRecordAndCount = nccDataInString[loopper];
                String[] operatorAndCounts = dataCleanerObj.SplitString(operatorRecordAndCount, '|');
                //Create the operatorFile as defined by operatorFile parameter
                String SearchName = CreateOperatorFile(operatorAndCounts[0],
                                operatorFile);
                //now innerloop to write quater by quarter subcriber count
                //for operator contained in SearchName
                for (int innerLoop = 1; innerLoop < operatorAndCounts.Length;
                                    innerLoop++)
                {
                    CreateSubscriberCountFile(
                                   operatorAndCounts[innerLoop],
                                   subscriberCountFile,
                                   reportQuaters[innerLoop],
                                   SearchName);
                }
            }

            int operatorFileRecords = writeToFileSystemObj.WriteToFile(operatorFileList, operatorFile, false);
            if (operatorFileRecords != operatorFileList.Count)
            {
                String message = "Code:1988" + "String message = Cannot create " + operatorFile;
                throw (new Exception(message));
            }
            int subscriberCountRecords = writeToFileSystemObj.WriteToFile(subscriberCountFileList, subscriberCountFile, false);
            if (subscriberCountRecords != subscriberCountFileList.Count)
            {
                String message = "Code:1988" + "String message = Cannot create " + subscriberCountFile;
                throw (new Exception(message));
            }
            return;
        }

        public void CreateSubscriberCountFile(string subscriberCount,
                string subscriberCountFile,
                string quarterName, String OperatorName)
        {
            String outputRecord = OperatorName + "," + quarterName + "," +
                                subscriberCount;
            try
            {
                // subscriberCountFileWriter.WriteLine(outputRecord);
                subscriberCountFileList.Add(outputRecord);
            }
            catch (Exception error)
            {
                String message = "Cannot create " + subscriberCountFile;
                throw new Exception(message, error);
            }
            return;
        }

        public string CreateOperatorFile(string operatorName, string operatorFile)
        {
            String outputRecord = string.Empty;
            String searchName = string.Empty;
            //Convert operatorName to all upper case and search

            if (operatorName.ToUpper().Contains("MTN"))
            {
                searchName = "MTN";
                outputRecord = searchName + "," + operatorName;
            }
            if (operatorName.ToUpper().Contains("GLO"))
            {
                searchName = "GLO";
                outputRecord = searchName + "," + operatorName;
            }
            if (operatorName.ToUpper().Contains("AIRTEL"))
            {
                searchName = "AIRTEL";
                outputRecord = searchName + "," + operatorName;
            }
            if (operatorName.ToUpper().Contains("ETISALAT"))
            {
                searchName = "ETISALAT";
                outputRecord = searchName + "," + operatorName;
            }
            if ((searchName != string.Empty) && (outputRecord != string.Empty))
            {
                operatorFileList.Add(outputRecord);
            }
            else
            {
                String message = "Operator Name match is missing";
                throw new Exception(message);
            }
            return searchName;

        }

        public String[] ProcessHearder(string headerRecord,
                                String reportQuaterFileName)
        {
            //Split the headers into an array
            //By pass array[0] because it contains "Operartor" 
            //Header
            string[] quaters = dataCleanerObj.SplitString(headerRecord, '|');
            //Open streamwriter objecet for use in writing the quarters
            //file. Open with Append = false
            try
            {
                StreamWriter localFileWriter = new
                                StreamWriter(reportQuaterFileName, false);
                int loopCounter = 0;
                foreach (String quarterName in quaters)
                {
                    //jump array [0] because it contains the "operator"
                    if (loopCounter > 0)
                    {
                        localFileWriter.WriteLine(quarterName);
                    }
                    loopCounter++;
                }
                localFileWriter.Close();
            }
            catch (Exception error)
            {
                String message = "Cannot create " + reportQuaterFileName;
                throw new Exception(message, error);
            }
            return quaters;
        }

        public List<string> readFileIntoList(List<String> rawNccInputDataFile)
        {

            //each record in file is read into fileContentAsString
            List<String> fileContentAsString = new List<String>();
            //Use try-catch to attempt to open and read the file
            foreach (String textLine in rawNccInputDataFile)
            {
                String dataForCleansing = textLine.Replace("\"", "");

                fileContentAsString.Add(dataCleanerObj.CleanInputRecord(dataForCleansing));
            };

            return fileContentAsString;

        }

    }
}
