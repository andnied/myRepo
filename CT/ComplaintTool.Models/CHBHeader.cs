using System;
using System.Globalization;

namespace ComplaintTool.Models
{
    public class CHBHeader
    {
        //Record Type
        public string RecordType { get; set; }
        //Name File Format
        public string NameFileFormat { get; set; }
        //Version File Format
        public string VersionFileFormat { get; set; }
        //File Name: Name
        public string FileName { get; set; }
        //File Name: Atos Processing Date
        public string FileNameProcessingDate { get; set; }
        //File Name: Sequence Number in Current Year (First File in the year starts with 0001)
        public string FileNameSequenceNumberInCurrentYear { get; set; }

        private int _numberInCurrentYear = 1;
        public int NumberInCurrentYear
        {
            get { return _numberInCurrentYear; }
            set { _numberInCurrentYear = value; }
        }
        //File Name: extension
        public string FileNameExtension { get; set; }
        //Creation System Date Time 
        public string CreationSystemDateTime { get; set; }
        //Number of detail records (without header record)
        public string NumberOfDetailRecords { get; set; }

        public int NumberOfRecords { get; set; }

        public void ImplementValue()
        {
            RecordType = "FH";
            NameFileFormat = "CBR";
            VersionFileFormat = "01.003";
            FileName = "ESERV_CHARGEBACK-";
            FileNameProcessingDate = ProcesingDateFormat(DateTime.UtcNow);

            FileNameSequenceNumberInCurrentYear = FileNameSequenceNumberInCurrentYearFormat(NumberInCurrentYear);

            FileNameExtension = ".CBR";

            CreationSystemDateTime = CreationSystemDateTimeFormat(DateTime.UtcNow);

            NumberOfDetailRecords = NumberOfDetailRecordsFormat(NumberOfRecords);
        }

        public string GetHeaderLine()
        {
            return RecordType 
                + NameFileFormat 
                + VersionFileFormat 
                + FileName 
                + FileNameProcessingDate 
                + "_" 
                + FileNameSequenceNumberInCurrentYear 
                + FileNameExtension 
                + CreationSystemDateTime 
                + NumberOfDetailRecords;
        }

        private static string ProcesingDateFormat(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        private static string NumberOfDetailRecordsFormat(int recordsCount)
        {
            return recordsCount.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0');
        }

        private static string FileNameSequenceNumberInCurrentYearFormat(int numberOfRecords)
        {
            return numberOfRecords.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
        }

        private static string CreationSystemDateTimeFormat(DateTime dateTime)
        {
            return dateTime.ToString("yyMMddHHmmss");
        }
    }
}
