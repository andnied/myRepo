using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EvalService
{
    [ServiceContract]
    public interface IEvalService
    {
        [OperationContract]
        IEnumerable<Eval> GetAll();
        [OperationContract]
        void Add(Eval eval);
    }
}
