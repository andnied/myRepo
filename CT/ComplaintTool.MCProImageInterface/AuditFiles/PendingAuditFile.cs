using System;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class PendingAuditFile : AuditFileBase
    {
        private const string SearchExpression = "Data File Name";

        public PendingAuditFile(string filePath) 
            : base(filePath)
        {
        }

        public override bool Parse()
        {
            string[] columns = null;
            var indexOfFileName = -1;
            foreach (var currentLine in _lines)
            {
                if (string.IsNullOrEmpty(currentLine) || currentLine.Trim() == "")
                {
                    columns = null;
                    continue;
                }

                if (currentLine.Contains(SearchExpression))
                {
                    columns = currentLine.Split(',');
                    indexOfFileName = Array.IndexOf(columns, SearchExpression);
                    continue;
                }

                if (columns == null) continue;

                var row = currentLine.Split(',');
                var fileName = row[indexOfFileName];
                _messages.Add(fileName, AuditUtil.BuildMessage(columns, row));
            }
            return true;
        }
    }
}
