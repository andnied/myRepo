using System.Management.Automation;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Common
{
    public abstract class ComplaintCmdletBase : Cmdlet
    {
        public virtual bool IsWriteMode { get; set; }

        protected ComplaintCmdletBase()
        {
            IsWriteMode = true;
            //Logging.LoadConfiguration();
        }

        public abstract void Process();

        public virtual void WriteComplaintObject(object obj)
        {
            if (IsWriteMode)
                WriteObject(obj);
        }

        protected sealed override void ProcessRecord()
        {
            Process();
            if (IsWriteMode)
            {
                foreach (var error in LogManager.GetErrors())
                {
                    var errorInformant = new ErrorInformant(this);
                    errorInformant.WriteError(error);
                }
            }
            base.ProcessRecord();
        }
    }
}
