using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EvalServiceLibrary
{
    public class EvalService : IEvalService
    {
        private static List<Eval> evals = new List<Eval>();

        public void SubmitEval(Eval eval)
        {
            evals.Add(eval);
        }

        public IEnumerable<Eval> GetEvals()
        {
            Thread.Sleep(5000);
            return evals;
        }

        public Eval GetEval(int id)
        {
            return evals.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Eval> GetEvalsBySubmitter(string submitter)
        {
            return evals.Where(e => e.Submitter == submitter);
        }

        public void RemoveEval(int id)
        {
            if (evals.Any(e => e.Id == id))
                evals.Remove(evals.FirstOrDefault(e => e.Id == id));
        }
    }
}
