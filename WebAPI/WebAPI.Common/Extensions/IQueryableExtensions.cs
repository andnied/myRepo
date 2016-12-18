using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

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

            var fields = sort.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim());

            if (!(AreFieldsValid<T>(fields)))
            {
                return source;
            }

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

        private static bool AreFieldsValid<T>(IEnumerable<string> fields)
        {
            return fields
                .Select(f =>
                {
                    if (f.StartsWith("-"))
                        return f.Remove(0, 1);
                    else
                        return f;
                })
                .All(f => typeof(T).GetProperty(f, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null);
        }
    }
}
