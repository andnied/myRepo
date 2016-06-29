using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;

namespace ComplaintTool.Postilion.Outgoing.Model.Representment
{
    public abstract class Organization4103Extracter
    {
        protected ComplaintUnitOfWork UnitOfWork;
        protected Models.Representment Representment;

        protected Organization4103Extracter(ComplaintUnitOfWork unitOfWork, Models.Representment representment)
        {
            UnitOfWork = unitOfWork;
            Representment = representment;
        }

        public bool ExtractRepresentment()
        {
            try
            {
                if (Representment == null)
                    throw new Exception("ExtractRepresentment Error. FeeCollection is null");

                var complaint = UnitOfWork.Repo<ComplaintRepo>().FindByCaseId(Representment.CaseId);
                var complaintValue = UnitOfWork.Repo<ComplaintRepo>().FindComplaintValueByValueId(Representment.ValueId);
                var complaintStage = UnitOfWork.Repo<ComplaintRepo>().GetStageById(Representment.StageId);
                var complaintRecord = UnitOfWork.Repo<ComplaintRepo>().FindRecordByRecordId(Representment.RecordsId);

                if (complaintValue == null)
                    throw new Exception(
                        string.Format("ExtractRepresentment Error. CaseId = {0} ComplaintValues is null. ValueId = {1}",
                            Representment.CaseId, Representment.ValueId));
                if (complaintStage == null)
                    throw new Exception(
                        string.Format("ExtractRepresentment Error. CaseId = {0} ComplaintStage is null. StageId = {1}",
                            Representment.CaseId, Representment.StageId));
                if (complaintRecord == null)
                    throw new Exception(
                        string.Format(
                            "ExtractRepresentment Error. CaseId = {0} ComplaintRecord is null. RecordId = {1}",
                            Representment.CaseId, Representment.RecordsId));

                Set4103Extract(complaint, complaintValue, complaintStage, complaintRecord);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public void Set4103Extract(Complaint complaint, ComplaintValue complaintValue, ComplaintStage complaintStage, ComplaintRecord complaintRecord)
        {
            var i4103 = Set4103(complaint, complaintValue, complaintStage, complaintRecord);
            AddRepresentmentExtract(i4103);
        }

        private void AddRepresentmentExtract(I4103 i4103)
        {
            var lineWhiteBase64String = i4103.GetRecordWhiteBase64String();
            var line = i4103.GetRecord();
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var representmentExtract = new RepresentmentExtract
            {
                CaseId = Representment.CaseId,
                RepresentmentId = Representment.RepresentmentId,
                PostilionExtractClearString = line,
                PostilionExtractWithBase64String = lineWhiteBase64String,
                ErrorFlag = false,
                Status = 0,
                InsertDate = DateTime.Now,
                InsertUser = windowsIdentity != null ? windowsIdentity.Name : "WindowsIdentity error."
            };
            //_unitOfWork.Repo<RepresentmentRepo>().Add(representmentExtract);
            UnitOfWork.Repo<RepresentmentRepo>().AddRepresentmentExtract(representmentExtract);
            Representment.Status = 2;
        }

        protected abstract I4103 Set4103(Complaint complaint, ComplaintValue complaintValue, ComplaintStage complaintStage, ComplaintRecord complaintRecord);
    }
}
