using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static TSource First<TSource, TException>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string exceptionMessage)
            where TException : Exception
        {
            var result = source.FirstOrDefault(predicate);

            if (result == null)
            {
                var exception = (TException)Activator.CreateInstance(typeof(TException), new object[] { exceptionMessage });

                throw exception;
            }

            return result;
        }
    }
}
