using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Documents
{
    public interface IExporter
    {
        string ProcessName { get; }
        void Export();
    }
}
