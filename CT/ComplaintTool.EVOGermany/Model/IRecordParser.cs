using ComplaintTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.EVOGermany.Model
{
    public interface IRecordParser
    {
        LinkedList<string> ExportedColumns { get; set; }
        string GetLine<T>(T record);
    }
}
