using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Utils
{
    public class MyLogger
    {
        private ILogger _logger;

        public MyLogger()
        {
            _logger = LogManager.GetCurrentClassLogger(new StackFrame(1).GetMethod().DeclaringType);
        }

        public MyLogger(Type type)
        {
            _logger = LogManager.GetCurrentClassLogger(type);
        }

        public void LogError(Exception ex)
        {
            _logger.LogError(ex);
        }
    }
}
