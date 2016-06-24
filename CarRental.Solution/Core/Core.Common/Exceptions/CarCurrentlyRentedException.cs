using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class CarCurrentlyRentedException : ApplicationException
    {
        public CarCurrentlyRentedException()
            : base()
        { }

        public CarCurrentlyRentedException(string msg)
            : base(msg)
        { }

        public CarCurrentlyRentedException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }
}
