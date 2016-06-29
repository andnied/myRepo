using ComplaintTool.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa.Outgoing
{
    interface IVisaOutgoingService
    {
        ICollection GetRecords(string orgId);
        void ProcessRecord(object record, ComplaintUnitOfWork unitOfWork, string path);
        void Notify(Exception ex);
    }
}
