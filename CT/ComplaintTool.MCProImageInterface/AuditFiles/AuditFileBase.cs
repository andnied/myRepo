using System.Collections.Generic;
using System.IO;

namespace ComplaintTool.MCProImageInterface.AuditFiles
{
    public abstract class AuditFileBase
    {
        protected readonly string[] _lines;
        protected readonly Dictionary<string, string> _messages = new Dictionary<string, string>();
        public Dictionary<string, string> Messages { get { return _messages; } }
        public string FilePath { get; private set; }

        public AuditFileBase(string filePath)
        {
            this.FilePath = filePath;
            _lines = File.ReadAllLines(filePath);
        }

        public abstract bool Parse();
    }
}
