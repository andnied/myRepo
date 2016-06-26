using System;
using System.ServiceModel;

namespace Core.Common.Utils
{
    public static class Guard
    {
        public static void ThrowIf<T>(bool test, string msg, params string[] args) where T : Exception
        {
            if (!test)
                return;

            var ex = (T)Activator.CreateInstance(typeof(T), string.Format(msg, args));
            throw new FaultException<T>(ex, ex.Message);
        }
    }
}
