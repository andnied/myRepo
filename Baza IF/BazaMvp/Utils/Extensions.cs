using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Utils
{
    public static class Extensions
    {
        public static void LogError(this ILogger logger, Exception ex)
        {
            //TODO
            //throw new NotImplementedException();
        }

        public static T Clone<T>(this T source)
        {
            if (!(typeof(T).IsSerializable))
                throw new ArgumentException("Object is not serializable.");

            if (object.ReferenceEquals(source, null))
                return default(T);

            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static IEnumerable<object> CastDynamically<T>(this IEnumerable<T> resultToBeCasted, Type type)
        {
            var method = typeof(Enumerable).GetMethod("OfType");
            var methodGeneric = method.MakeGenericMethod(new Type[] { type });
            return (IEnumerable<object>)methodGeneric.Invoke(null, new object[] { resultToBeCasted });
        }
    }
}
