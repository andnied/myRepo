using ComplaintTool.Common;
using ComplaintTool.Visa.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Visa
{
    public abstract class VisaIncomingBase : IComplaintProcess
    {
        public string OrganizationId
        {
            get
            {
                return Common.Enum.Organization.VISA.ToString();
            }
        }

        public string ProcessName
        {
            get
            {
                return Globals.VisaIncomingInterfaceProcessName;
            }
        }

        public static VisaIncomingBase GetService(string type, string filePath = null, int fileId = 0, string arn = null)
        {
            if (type == "registration")
                return new VisaIncomingRegistration(filePath);
            if (type == "processing")
                return new VisaIncomingProcessing(fileId, arn);

            return null;
        }

        public abstract string ProcessFilePath { get; }

        public abstract string FilePath { get; }

        public abstract int Process();
    }
}
