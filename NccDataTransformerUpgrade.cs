using System;
using System.Collections.Generic;
using System.IO;

namespace NccDataTransformerUpgrade
{
    class NccDataTransformerUpgrade
    {
        //The input file is copied into the root of the app
        // "..\\" simply means the file system path in which the containing 
        // folder of the coded file is located.
        const String inputFileName = ".\\bin\\Debug\\NCC-Operator data(July 2017)-FileInput.txt";
        const String reportQuaterFile = ".\\bin\\Debug\\QuarterName.txt";
        const String operatorFile = ".\\bin\\Debug\\Operator.txt";
        const String subscriberCountFile = ".\\bin\\Debug\\SubscriberCount.txt";
        const String Ncc2017OperatorDataR_Input = ".\\bin\\Debug\\NCC-Operator data(July 2017-R_Edited).txt";
        //2017q2
        //
        static String[] ReportQuaters;
        static List<String> executionTimeControl = new List<string>();

        static void Main(string[] args)
        {
            DataFileIO dataFileIOObj = new DataFileIO();
            DataWrangler DataWranglerObj = new DataWrangler();
            List<String> rawNccInputDataFile = dataFileIOObj.ReturnFileContentAsStringList(inputFileName);
            // if (rawNccInputDataFile == null)
            // {
            //     throw new Exception("No Data in rawNccInputDataFile");
            // }
            //*** Leave as is (Start) ***//
            String timerControl = DateTime.Now + " <Start> " + "eu.kvac E012257";
            Console.WriteLine(timerControl);
            //
            executionTimeControl.Add(timerControl);
            //*** Leave as is (End) ***//
            //1. Use the readFileIntoList() method to read file           
            List<String> NccDataInString = DataWranglerObj.readFileIntoList(rawNccInputDataFile);
            //2. header record is in NccDataInString[0]
            Console.WriteLine("NccDataInSting [0] " + NccDataInString[0]);
            String headerRecord = NccDataInString[0];
            if (headerRecord.Length == 0 || headerRecord == string.Empty)
            {
                throw new Exception("No header Record");
            }

            ReportQuaters = DataWranglerObj.ProcessHearder(headerRecord, reportQuaterFile);

            DataWranglerObj.ProcessOperatorAndCounts(NccDataInString, ReportQuaters,
                            operatorFile, subscriberCountFile);

            DataWranglerObj.CreateOperatorDataForR(NccDataInString, Ncc2017OperatorDataR_Input);
            //
            //*** Leave as is (Start) ***//
            timerControl = DateTime.Now + " <Finish> " + "eu.kvac E012257";
            Console.WriteLine(timerControl);
            executionTimeControl.Add(timerControl);
            WriteToFileSystem writeToFileSystemObj = new WriteToFileSystem();
            int countOfOperatorRecords =
                    writeToFileSystemObj.WriteToFile(executionTimeControl,
                            "ExecutionTimeControl.txt", false);
            //*** Leave as is (End) ***//        
            //
        }
        //***********************************       
        //***************************************************

    }
}
