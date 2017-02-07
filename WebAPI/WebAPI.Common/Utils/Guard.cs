using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Exceptions;

namespace WebAPI.Common.Utils
{
    public static class Guard
    {
        private static string WrapperExceptionMessage = "Internal exception occured. Please see inner exception for details.";

        public static void ThrowIf<T>(bool assertion, string msg) 
            where T : Exception
        {
            if (!assertion)
                return;

            var innerException = (T)Activator.CreateInstance(typeof(T), new object[] { msg });
            var exception = (ApiException)Activator.CreateInstance(typeof(ApiException), new object[] { WrapperExceptionMessage, innerException });

            throw exception;
        }
    }
}
