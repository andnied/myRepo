using System;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class CarCurrentlyRentedException : Exception
    {
        public CarCurrentlyRentedException() { }
        public CarCurrentlyRentedException(string message) : base(message) { }
        public CarCurrentlyRentedException(string message, Exception inner) : base(message, inner) { }
        protected CarCurrentlyRentedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
