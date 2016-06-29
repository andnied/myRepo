using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.CTLogger
{
    public interface ILogger
    {
        void LogComplaintEvent(int msgEventNumber, params object[] args);
        void LogComplaintException(Exception ex, int errNumber = 500);
    }
}
