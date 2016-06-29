using ComplaintTool.Models;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComplaintTool.Documents.DocParsers
{
    public class PdfDocParser:IDocParser
    {
        private readonly string _path;
        private string _extractedText;
        private string _organizationId;

        public PdfDocParser(string path,string organizationId)
        {
            _path = path;
            _organizationId = organizationId;
            _extractedText = null;
        }

        public string ExtractText()
        {
            var sb = new StringBuilder();

            using (var reader = new PdfReader(_path))
            {
                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    sb.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }

            _extractedText = sb.ToString();
            return _extractedText;
        }

        public string GetParamValue(List<ParserKey> parserKeys)
        {
            if (_extractedText == null)
                ExtractText();

            return string.IsNullOrEmpty(_extractedText) ? null : GetValueFromText(parserKeys);
        }

        private string GetValueFromText(IEnumerable<ParserKey> parserKeys)
        {
            foreach(var parserKey in parserKeys)
            {
                if (!_extractedText.Contains(parserKey.KeyValue)) continue;
                var startIndex = _extractedText.LastIndexOf(parserKey.KeyValue, StringComparison.Ordinal);
                var endIndex = parserKey.KeyValue.Length + parserKey.AddLenght;
                var text = _extractedText.Substring(startIndex,endIndex);
                return text.Replace(parserKey.KeyValue,"").Replace('\n', ' ').Replace(" ", "").Replace("-", "");
            }
            return null;
        }
    }
}
