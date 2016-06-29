namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    static class AuditUtil
    {
        public static string BuildMessage(string[] columns, string[] row)
        {
            string message = "";
            for (var i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(row[i]))
                    message += columns[i] + ": " + row[i] + "; ";
            }
            return message;
        }
    }
}
