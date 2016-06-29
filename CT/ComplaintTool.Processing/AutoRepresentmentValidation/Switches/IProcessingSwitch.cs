using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Processing.AutoRepresentmentValidation.Switches
{
    public interface IProcessingSwitch
    {
        void Switch(string caseId);
    }
}
