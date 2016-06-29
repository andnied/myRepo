using ComplaintTool.Shell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ComplaintTool.Common;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;
using ComplaintTool.Common.CTLogger;

namespace ComplaintTool.Shell.Utils
{
    [Cmdlet(VerbsCommon.Show, "ProcessingKeys")]
    public class ShowProcessingKeys:ComplaintCmdletBase
    {
        ILogger Logger = LogManager.GetLogger();

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "The number of the last ProcessKeys.")]
        [Alias("ProcessKeysNumber")]
        public int? Number { get; set; }

        public override void Process()
        {
            try
            {
                using(var unitOfWork=ComplaintUnitOfWork.Create())
                {
                    var processingKeys=unitOfWork.Repo<ProcessRepo>().GetProcessingKeys(Number ?? 10);
                    processingKeys.ForEach(x =>
                    {
                        var msg = string.Format("{0}{4}{1}{4}{2}{4}{3}", x.ProcessKey1.ToString().ToUpper(), x.ProcessDate,
                                x.FileName, x.Source,'\t');

                        if (IsWriteMode)
                            WriteObject(msg);
                    });
                }
            }catch(Exception ex)
            {
                Logger.LogComplaintException(ex);
            }
        }
    }
}
