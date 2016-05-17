using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaIF.Logic
{
    public interface IParser
    {
        IEnumerable<string[]> GetMessages();
        string[] RetrieveHeader(string[] msg);
        InputBase InitializeParent(string[] header);
        IEnumerable<InputBase> GenerateRecords(InputBase parentRecord, string[] msg);
        void AddRecords(IEnumerable<InputBase> records);
    }
}
