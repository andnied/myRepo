using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class UnableToRentForDateException : ApplicationException
    {
        public UnableToRentForDateException()
            : base()
        { }

        public UnableToRentForDateException(string msg)
            : base(msg)
        { }

        public UnableToRentForDateException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }
}
