using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        { }

        public NotFoundException(string msg)
            : base(msg)
        { }

        public NotFoundException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }
}
