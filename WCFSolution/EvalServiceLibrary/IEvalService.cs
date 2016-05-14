using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace EvalServiceLibrary
{
    [ServiceContract]
    public interface IEvalService
    {
        [WebGet(UriTemplate = "evals")]
        [OperationContract]
        IEnumerable<Eval> GetEvals();

        [WebInvoke(Method = "POST", UriTemplate = "evals")]
        [OperationContract]
        void SubmitEval(Eval eval);

        [WebGet(UriTemplate = "eval?id={id}")]
        [OperationContract]
        Eval GetEval(int id);

        [WebGet(UriTemplate = "evals/{submitter}")]
        [OperationContract]
        IEnumerable<Eval> GetEvalsBySubmitter(string submitter);

        [WebInvoke(Method = "DELETE", UriTemplate = "eval?id={id}")]
        [OperationContract]
        void RemoveEval(int id);
    }
}
