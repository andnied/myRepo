using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Utils
{
    public class Helper
    {
        public static bool AreFieldsValid<T>(string fields, Func<string, string> selector = null)
        {
            var fieldCollection = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(f => 
                {
                    f = f.Trim();

                    if (selector != null)
                        f = selector(f);

                    return f;
                });
            var objectFieldCollection = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Select(f => f.Name);

            return fieldCollection.Intersect(objectFieldCollection, StringComparer.OrdinalIgnoreCase).SequenceEqual(fieldCollection);
        }
    }
}
