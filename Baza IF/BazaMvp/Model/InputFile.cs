using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    [Serializable]
    public class InputFile
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<InputBase> Records { get; set; }

        public IEnumerable<VisaRecord> VisaRecords { get { return Records.OfType<VisaRecord>(); } }
        public IEnumerable<MasterCardRecord> McRecords { get { return Records.OfType<MasterCardRecord>(); } }
    }
}
