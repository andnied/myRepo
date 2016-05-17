using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Utils
{
    public class IFException : Exception
    {
        public IFException(string msg)
            : base(msg)
        {
        }

        public IFException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
