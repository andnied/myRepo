using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.TemporaryValidation.Organizations
{
    public interface IOrganization
    {
        List<string> GetFilesNames(int processingStatus,int destintationStatus);
    }
}
