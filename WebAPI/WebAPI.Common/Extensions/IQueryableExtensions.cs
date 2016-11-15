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

            var fields = sort.Split(',');

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

        private static bool AreFieldsValid<T>(string[] fields)
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
