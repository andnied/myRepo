using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class CarNotRentedException : ApplicationException
    {
        public CarNotRentedException()
            : base()
        { }

        public CarNotRentedException(string msg)
            : base(msg)
        { }

        public CarNotRentedException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }
}
