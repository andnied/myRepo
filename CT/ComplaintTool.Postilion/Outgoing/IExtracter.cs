using ComplaintTool.DataAccess.Model;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Postilion.Outgoing
{
    interface IExtracter
    {
        ICollection GetExtract();
        object WriteExtractToFileAndCreatePostFile(ICollection colForExtract, string folderPath);
        string UpdateExtracts(ICollection colForExtract, object postilionFile);
        void Notify(ILogger logger, string fileName);
    }
}
