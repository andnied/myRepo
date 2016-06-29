using ComplaintTool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Documents.OrganizationServices
{
    public class McDocImportService : DocImportService
    {
        public McDocImportService() : base() { }

        public override string OrganizationId
        {
            get { return ComplaintTool.Common.Enum.Organization.MC.ToString(); }
        }

        public override string ProcessName
        {
            get { return Globals.McDocImportProcessName;}
        }

        public override Common.Enum.FileSource FileSource
        {
            get { return Common.Enum.FileSource.MC; }
        }
    }
}
