using BazaMvp.DataAccess;
using BazaMvp.DataAccess.Repos;
using BazaMvp.Model;
using BazaMvp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace BazaIF.Logic
{
    public abstract class ParserBase : IDisposable
    {
        #region Public

        public void Dispose()
        {
            try
            {
                _unitOfWork.Dispose();
            }
            catch { }
        }

        #endregion

        #region Protected

        protected readonly string _filePath;
        protected readonly UnitOfWork _unitOfWork;

        protected abstract string ValueRegex { get; }

        protected ParserBase(string filePath)
        {
            _filePath = filePath;
            _unitOfWork = UnitOfWork.Create();
        }

        protected IEnumerable<string[]> GetAllMessages(string startPoint, string endPoint)
        {
            var lines = System.IO.File.ReadAllLines(_filePath).Where(l => l.Replace(" ", "").Length > 0).ToArray();
            int startLine = -1;
            string[] reportArray = null;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(startPoint) && startLine == -1)
                {
                    startLine = i;
                    i += 4;
                }

                if (lines[i].Contains(endPoint) && startLine >= 0)
                {
                    reportArray = new string[i - startLine];
                    Array.Copy(lines, startLine, reportArray, 0, (i--) - startLine);

                    if (!(reportArray.Any(l => l.Contains("NO DATA FOR THIS REPORT")) || reportArray.Any(l => l.Contains("NO DATA TO REPORT"))))
                    {
                        yield return reportArray;
                        reportArray = null;
                    }

                    startLine = -1;
                }
            }
        }

        protected decimal GetDecimalValue(string value)
        {
            var valueStr = value.Trim().Replace(",", "").Replace('.', ',').Replace(" ", "");
            var valueFactor = valueStr.Contains("CR") ? 1 : -1;
            var valueResult = decimal.Parse(valueStr.Replace("CR", "").Replace("DR", "").Replace("DB", ""));

            return valueResult * valueFactor;
        }

        protected virtual void AddRecords(IEnumerable<InputBase> records)
        {
            if (_unitOfWork.Repo<FilesRepo>().FidExists(records.FirstOrDefault().Fid))
                throw new IFException("Plik juz zostal wczytany.");

            var file = _unitOfWork.Repo<FilesRepo>().AddFile(new BazaMvp.Model.InputFile
            {
                FileName = Path.GetFileName(_filePath),
                UploadDate = DateTime.Now,
                UserName = WindowsIdentity.GetCurrent().Name
            });

            records.ToList().ForEach(r => r.InputFile = file);
        }

        #endregion
    }
}
