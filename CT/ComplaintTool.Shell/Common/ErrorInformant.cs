using System;
using System.Management.Automation;

namespace ComplaintTool.Shell.Common
{
    public class ErrorInformant
    {
        private Cmdlet _cmdlet;

        public ErrorInformant(Cmdlet cmdlet)
        {
            _cmdlet = cmdlet;
        }

        public void WriteError(Exception ex, ErrorCategory? errorCategory = null, string errorCode = null)
        {
            errorCategory = errorCategory ?? GetErrorCategory(ex);
            string errorId = errorCode ?? GetErrorId(ex);
            var errorRecord = new ErrorRecord(ex, errorId, errorCategory.Value, this);
            _cmdlet.WriteError(errorRecord);
        }

        private ErrorCategory GetErrorCategory(Exception ex)
        {
            if (ex is ArgumentException)
                return ErrorCategory.InvalidArgument;

            return ErrorCategory.InvalidOperation;
        }

        private string GetErrorId(Exception ex)
        {
            if (ex is ArgumentException)
                return "5001";

            return "5000";
        }
    }
}
