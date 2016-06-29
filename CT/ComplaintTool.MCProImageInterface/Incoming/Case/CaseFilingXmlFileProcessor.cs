using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ComplaintTool.Common.Utils;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;

namespace ComplaintTool.MCProImageInterface.Incoming.Case
{
    abstract class CaseFilingXmlFileProcessor<T> : XmlFileProcessor<T>
    {
        protected Complaint _complaint;
        protected ComplaintStage _complaintStage;
        protected CaseFilingRecord _caseFilingRecord;
        protected CaseFilingIncomingFile _caseFilingIncomingFile;

        public CaseFilingXmlFileProcessor(ComplaintUnitOfWork unitOfWork, XmlFileInfo xmlFile) 
            : base(unitOfWork, xmlFile)
        {
        }

        protected override bool ProcessData(T data)
        {
            if (ValidateData(data) && MatchCase(data))
            {
                return ProcessCase(data);
            }
            return false;
        }

        protected virtual bool ValidateData(T data)
        {
            var context = new ValidationContext(data, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(data, context, results, true);
        }

        protected abstract bool MatchCase(T data);
        protected abstract bool ProcessCase(T data);
    }
}
