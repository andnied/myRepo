using ComplaintTool.Common.Enum;
using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.CLFValidation
{
    public class ClfProcessor
    {
        public void UpdateClfReportStatus()
        {
            using (var unitOfWork = ComplaintUnitOfWork.Create())
            {
                var clfItems = unitOfWork.Repo<CLFRepo>().GetIncomingAndOutgoingItems((int)ClfReportStatus.New);
                foreach (var clf in clfItems)
                {
                    if (clf.OutgoingItem.CLFReportId != null)
                    {
                        clf.OutgoingItem.CLFReportStatus = (int)ClfReportStatus.Processed;
                        unitOfWork.Repo<CLFRepo>().Update(clf.OutgoingItem);
                    }

                    if (clf.IncomingItem.CLFReportId != null)
                    {
                        clf.IncomingItem.CLFReportStatus = (int)ClfReportStatus.Processed;
                        unitOfWork.Repo<CLFRepo>().Update(clf.IncomingItem);
                    }
                }
                unitOfWork.Commit();
            }
        }

        public void ValidateClf()
        {
            var clfItems = new ClfItemsValidator();
            //clfItems.ValidateItems();
        }
    }
}
