using ComplaintTool.DataAccess;
using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Model.Oraganizations
{
    public interface IReportOrganization
    {
        ChbRecord FillRecord(ChbRecord chbRecord, long? incomingId);
    }
}
