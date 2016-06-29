using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Postilion.Incoming
{
    interface IPostilionIncoming
    {
        void ProcessMessage(string sourceFileNameWithoutExtension, string tempCaseId, string postilionStatus, string sourceFileName);
        bool AddToCRBReportItem(string caseId, long id);
    }
}
