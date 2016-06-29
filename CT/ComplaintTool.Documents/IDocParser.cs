using ComplaintTool.Models;
using System.Collections.Generic;

namespace ComplaintTool.Documents
{
    public interface IDocParser
    {
        string ExtractText();
        string GetParamValue(List<ParserKey> keyParsers);
    }
}
