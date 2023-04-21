using System;
using System.Collections.Generic;
using System.Text;

namespace NccDataTransformerUpgrade
{
    class DataCleaner
    {

        public String CleanInputRecord(String textLine)
        {
            String tempLine = textLine.Replace('\t', '|');
            tempLine = tempLine.Replace("|||", "|");
            tempLine = tempLine.Replace("||", "|");
            tempLine = tempLine.Replace(",", "");
            int locationOfLastTab = tempLine.IndexOf("|");
            if (locationOfLastTab < tempLine.Length)
            {

            }
            else
            {
                tempLine = tempLine.Substring(0, locationOfLastTab);
            }
            return tempLine;
        }
        public String[] SplitString(String text, Char ch)
        {
            String[] quaters = text.Split('|');
            return quaters;
        }
    }
}
