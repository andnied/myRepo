using System;
using System.Globalization;
using ComplaintTool.Common.Utils;
using ComplaintTool.Models;
using ComplaintTool.MCProImageInterface.Model;

namespace ComplaintTool.MCProImageInterface.Outgoing.Case
{
    public class CaseFilingRecordToFACUGenerator : CaseFilingRecordToXmlFileGenerator
    {
        public CaseFilingRecordToFACUGenerator(CaseFilingRecord record, ComplaintStage lastStage, string tempFolderPath)
            : base(record, lastStage, tempFolderPath)
        {
        }

        public override string FileType
        {
            get
            {
                return OutgoingFileType.FACU;
            }
        }

        public override string Generate()
        {
            var facu = new Facu();
            // TODO implementacja mapowania danych
            return XmlUtil.SerializeToFile<Facu>(facu, this.ProcessFilePath).ToImageProEncoding();
        }
    }
}
