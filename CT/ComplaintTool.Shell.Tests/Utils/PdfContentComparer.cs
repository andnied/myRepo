using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComplaintTool.Shell.Tests.Utils
{
    public class PdfContentComparer
    {
        private string _basePath;
        private string _contentBasePdf;
        private const string _dataPattern = @"\d\d[.]\d\d[.]\d\d\d\d";

        public PdfContentComparer(string path)
        {
            _basePath = path;
            _contentBasePdf = null;
        }

        public bool Compare(byte [] bytes,bool ignoreData)
        {
            SetBaseContent();
            var content = GetContent(bytes);
            if (ignoreData)
            {
                content=IgnoreDate(content);
                _contentBasePdf = IgnoreDate(_contentBasePdf);
            }
            if (_contentBasePdf.Equals(content))
                return true;

            return false;
        }

        /// <summary>
        /// Funkcja ignoruje daty, gdyż może to być data wygenerowania
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string IgnoreDate(string content)
        {
            Regex regex = new Regex(_dataPattern);
            string result = regex.Replace(content, "");
            return result;
        }

        private void SetBaseContent()
        {
            if(!string.IsNullOrEmpty(_contentBasePdf))
                return;

            using (var reader = new PdfReader(_basePath))
            {
                _contentBasePdf= GetContent(reader);
            }
        }

        private string GetContent(byte[] bytes)
        {
            using (var reader = new PdfReader(bytes))
            {
                return GetContent(reader);
            }
        }

        private string GetContent(PdfReader pdfReader)
        {
            var sb = new StringBuilder();

            for (int i = 1; i <= pdfReader.NumberOfPages; i++)
            {
                sb.Append(PdfTextExtractor.GetTextFromPage(pdfReader, i));
            }

            return sb.ToString();
        }
    }
}
