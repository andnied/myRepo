using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EvalService
{
    [DataContract]
    public class Eval
    {
        [DataMember]
        public string Submitter { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime TimeSent { get; set; }
    }
}
