using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Common.Logging
{
    public class LogManager : ILogger
    {
        public LogManager(Type type)
        {

        }

        public void LogComplaintEvent(int msgEventNumber, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogComplaintException(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
