using System;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    class MatchAuditFile : AuditFileBase
    {
        private const string BeginMatchExpression = "ARN";

        public MatchAuditFile(string filePath)
            : base(filePath)
        {

        }

        public override bool Parse()
        {
            string[] columns = null;
            for (var l = 0; l < _lines.Length; l++)
            {
                if (columns == null && !_lines[l].Contains(BeginMatchExpression))
                    continue;

                if (columns != null)
                {
                    var message = string.Empty;
                    var row = _lines[l].Split(',');
                    var indexOfArn = Array.IndexOf(columns, "ARN");
                    var arn = row.GetValue(indexOfArn);

                    for (var i = 0; i < columns.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(row[i]))
                            message = message + columns[i] + ": " + row[i] + ". ";
                    }

                    _messages.Add(arn.ToString(), message);

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
