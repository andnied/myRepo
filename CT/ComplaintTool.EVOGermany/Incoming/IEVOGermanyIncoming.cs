using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Incoming
{
    interface IEVOGermanyIncoming
    {
        bool Validate(string filePath);
        int ProcessFile(string filePath);
        void NotifyMainException(Exception ex);
    }
}
