using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Model
{
    [Serializable]
    public abstract class InputBase
    {
        public Guid Id { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Fid { get; set; }

        public virtual InputFile InputFile { get; set; }
        public virtual long InputFileId { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var prop in this.GetType().GetProperties())
            {
                builder.Append(prop.GetValue(this)).Append("\t");
            }

            return builder.ToString();
        }
    }
}
