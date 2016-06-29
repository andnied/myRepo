using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintTool.Common.Extensions;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.MCProImageInterface.Outgoing
{
    public class TiffGenerator
    {
        private readonly ComplaintUnitOfWork _unitOfWork;
        private readonly string _tempFolderPath;

        public TiffGenerator(ComplaintUnitOfWork unitOfWork, string tempFolderPath)
        {
            Guard.ThrowIf<ArgumentNullException>(unitOfWork == null, "unitOfWork");
            Guard.ThrowIf<ArgumentNullException>(tempFolderPath.IsEmpty(), "tempFolderPath");
            _unitOfWork = unitOfWork;
            _tempFolderPath = tempFolderPath;
        }

        public string GenerateTiffFileForRepresentment(Representment representment)
        {
            _unitOfWork.Repo<DocumentRepo>().GenerateRepresentmentDocument(representment, _tempFolderPath);
            var fileList = Directory.GetFiles(_tempFolderPath, "*.pdf").Union(Directory.GetFiles(_tempFolderPath, "*.PDF")).ToArray();
            return this.GenerateTifFile(fileList, _tempFolderPath, representment.CaseId);
        }

        public string GenerateTiffFileForComplaint(Complaint complaint)
        {
            var dirPath = _unitOfWork.Repo<DocumentRepo>().GenerateDocumentExport(complaint, _tempFolderPath);
            var fileList = Directory.GetFiles(dirPath, "*.pdf").Union(Directory.GetFiles(dirPath, "*.PDF")).ToArray();
            return this.GenerateTifFile(fileList, dirPath, complaint.CaseId);
        }

        public string GenerateTiffFileForDocuments(IEnumerable<DocumentExport> documents, string fileId)
        {
            var dirPath = _unitOfWork.Repo<DocumentRepo>().GenerateDocumentExport(documents, _tempFolderPath);
            var fileList = Directory.GetFiles(dirPath, "*.pdf").Union(Directory.GetFiles(dirPath, "*.PDF")).ToArray();
            return this.GenerateTifFile(fileList, dirPath, fileId);
        }

        private string GenerateTifFile(string[] fileList, string dirPath, string fileId)
        {
            var retTiffPath = string.Empty;
            var count = 0;
            foreach (var pdfDoc in fileList)
            {
                count++;
                var tmpPath = dirPath + @"\" + "dok0" + count + "_%03d.png";
                PdfToTiff.ExportPdfToTiff(pdfDoc, tmpPath);
            }

            var tmpList = Directory.GetFiles(dirPath, "*.png");
            var tiffFileName = dirPath + @"\" + fileId + ".tif";
            PdfToTiff.SaveMultipage(tmpList, tiffFileName, "TIFF");

            if (File.Exists(tiffFileName))
                retTiffPath = tiffFileName;
            return retTiffPath;
        }
    }
}
