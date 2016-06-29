namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class ErrorAuditFile : AuditFileBase
    {
        private const string BeginErrorExpression = "Data File Name";

        public ErrorAuditFile(string filePath)
            : base(filePath)
        {

        }

        public override bool Parse()
        {
            string[] columns = null;
            for (var l = 0; l < _lines.Length; l++)
            {
                if (columns == null && !_lines[l].Contains(BeginErrorExpression))
                    continue;

                if (columns != null)
                {
                    var row = _lines[l].Split(',');

                    var message = string.Empty;
                    var fileName = string.Empty;

                    for (var i = 0; i < columns.Length; i++)
                    {
                        if (i == 0) fileName = row[i];

                        var isTheLastColumn = i == columns.Length - 1;

                        if (!string.IsNullOrWhiteSpace(row[i]) && !isTheLastColumn)
                            message = message + columns[i] + ": " + row[i] + ". ";

                        if (!isTheLastColumn) continue;
                        
                        message = message + columns[i] + ": ";

                        for (var j = i; j < row.Length; j++)
                            message = message + row[j] + ", ";

                        message = message.Remove(message.Length - 2, 2) + ".";
                        _messages.Add(fileName, message);
                    }

                    var isTheLastLine = (l + 1) >= _lines.Length;
                    if (!isTheLastLine)
                    {
                        if (!string.IsNullOrWhiteSpace(_lines[l + 1]))
                            continue;
                    }

                    columns = null;
                    continue;
                }

                columns = _lines[l].Split(',');
            }
            return true;
        }
    }
}
