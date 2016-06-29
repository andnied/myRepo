using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Documents
{
    public interface IDocCreator
    {
        string Extension{get;}
        void LoadConfiguration();
        byte[] CreateDocument(string caseId, long stageId);
    }
}
