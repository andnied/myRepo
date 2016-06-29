using System;

namespace ComplaintTool.Common.Utils
{
    public static class Guard
    {
        public static void ThrowIf<TException>(bool condition, string message) where TException : Exception
        {
            if (!condition) return;

            var exception = (TException)Activator.CreateInstance(
                typeof(TException), new object[] { message });
            throw exception;
        }
    }
}
