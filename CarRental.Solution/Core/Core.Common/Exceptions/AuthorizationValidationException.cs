using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class AuthorizationValidationException : ApplicationException
    {
        public AuthorizationValidationException()
            : base()
        { }

        public AuthorizationValidationException(string msg)
            : base(msg)
        { }

        public AuthorizationValidationException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }
}
