using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using WebAPI.Common.Utils;

namespace WebAPI.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> DynamicSort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(sort))
            {
                return source;
            }

            Func<string, string> func = (string f) =>
            {
                if (f.StartsWith("-"))
                    return f.Remove(0, 1);
                else
                    return f;
            };

            if (!(Helper.AreFieldsValid<T>(sort, func)))
            {
                return source;
            }

            var fields = sort.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim());
            var sortExpression = string.Empty;

            foreach (var field in fields)
            {
                if (field.StartsWith("-"))
                {
                    sortExpression += field.Remove(0, 1) + " descending,";
                }
                else
                {
                    sortExpression += field + ",";
                }
            }

            if (!(string.IsNullOrWhiteSpace(sortExpression)))
            {
                sortExpression = sortExpression.Remove(sortExpression.Length - 1);
                source = source.OrderBy(sortExpression);
            }

            return source;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, int page, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (page == 0 || count == 0)
            {
                return source;
            }

            return source.Skip((page - 1) * count).Take(count);
        }

        public static IQueryable<T> Select<T>(this IQueryable<T> source, string fields)
        {
            return source.Select(CreateNewStatement<T>(fields)).AsQueryable();
        }

        private static Func<T, T> CreateNewStatement<T>(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(T));

            // create initializers
            var bindings = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => {
                    // property "Field1"
                    var mi = typeof(T).GetProperty(o.Trim(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = o.Field1"
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }
    }
}
