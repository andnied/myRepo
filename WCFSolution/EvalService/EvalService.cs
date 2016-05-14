using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EvalService
{
    public class EvalService : IEvalService
    {
        private static List<Eval> evals = new List<Eval>();

        public void Add(Eval eval)
        {
            evals.Add(eval);
        }

        public IEnumerable<Eval> GetAll()
        {
            return evals;
        }
    }
}
