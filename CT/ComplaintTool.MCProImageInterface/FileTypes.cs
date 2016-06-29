namespace ComplaintTool.MCProImageInterface
{
    public class OutgoingFileType
    {
        public const string FICN = "FICN";
        public const string FACU = "FACU";
        public const string FICU = "FICU";
        public const string SCU = "SCU";
        public const string ACU = "ACU";

        public static bool CheckExportCode(string exportCode)
        {
            return exportCode.ToUpper() == FICN
                || exportCode.ToUpper() == FACU
                || exportCode.ToUpper() == FICU;
        }
    }

    public class IncomingFileType
    {
        public const string FIMN = "FIMN";
        public const string FIMFA = "FIMFA";

        public static bool CheckImportCode(string importCode)
        {
            return importCode.ToUpper() == FIMN
               || importCode.ToUpper() == FIMFA;
        }
    }
}
