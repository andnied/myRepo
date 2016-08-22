using System;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class AuthorizationValidationException : Exception
    {
        public AuthorizationValidationException() { }
        public AuthorizationValidationException(string message) : base(message) { }
        public AuthorizationValidationException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizationValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
