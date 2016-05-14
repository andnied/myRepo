using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EvalServiceLibrary
{
    [DataContract]
    public class Eval
    {
        public Eval()
        {

        }

        public Eval(string submitter, string msg)
        {
            Submitter = submitter;
            Message = msg;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Submitter { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime TimeSent { get; set; }
    }
}
