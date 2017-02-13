using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public static Expression<Func<T, T>> CreateDynamicSelectExpression<T>(string fields)
        {
            var parameterName = Expression.Parameter(typeof(T), "o"); // input parameter "o"
            var parameterType = Expression.New(typeof(T)); // new statement "new Data()"

            var bindings = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => {

                    var property = typeof(T).GetProperty(o.Trim(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase); // property "Field1"
                    var parametersProperty = Expression.Property(parameterName, property); // original value "o.Field1"

                    return Expression.Bind(property, parametersProperty); // set value "Field1 = o.Field1"
                }
            );

            var initialization = Expression.MemberInit(parameterType, bindings); // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(initialization, parameterName); // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"

            return lambda;
        }
    }
}
