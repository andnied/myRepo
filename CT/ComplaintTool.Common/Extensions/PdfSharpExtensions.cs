using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using System.Collections.Generic;
using PdfSharp.Pdf.Content.Objects;

namespace ComplaintTool.Common.Extensions
{
    public static class PdfSharpExtensions
    {
        public static IEnumerable<string> ExtractText(this PdfPage page)
        {
            var content = ContentReader.ReadContent(page);
            var text = content.ExtractText();
            return text;
        }

        public static IEnumerable<string> ExtractText(this CObject cObject)
        {
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name != OpCodeName.Tj.ToString() &&
                    cOperator.OpCode.Name != OpCodeName.TJ.ToString()) yield break;
                foreach (var cOperand in cOperator.Operands)
                    foreach (var txt in ExtractText(cOperand))
                        yield return txt;
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                    foreach (var txt in ExtractText(element))
                        yield return txt;
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                yield return cString.Value;
            }
        }
    }
}
