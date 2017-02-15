using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using WebAPI.Common.Utils;
using System.Data.Entity;

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

            Func<string, string> func = (f) =>
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

        public static IQueryable<T> DynamicSelect<T>(this IQueryable<T> source, string fields)
        {
            return source.Select(Helper.CreateDynamicSelectExpression<T>(fields));
        }

        public static async Task<TSource> FirstAsync<TSource, TException>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, string exceptionMessage)
           where TException : Exception
        {
            var result = await source.FirstOrDefaultAsync(predicate);

            if (result == null)
            {
                var exception = (TException)Activator.CreateInstance(typeof(TException), new object[] { exceptionMessage });

                throw exception;
            }

            return result;
        }

        public static async Task<TSource> FindAsync<TSource, TException>(this DbSet<TSource> source, string exceptionMessage, params object[] keyValues)
           where TException : Exception
           where TSource : class
        {
            var result = await source.FindAsync(keyValues);

            if (result == null)
            {
                var exception = (TException)Activator.CreateInstance(typeof(TException), new object[] { exceptionMessage });

                throw exception;
            }

            return result;
        }

        public static async Task<IQueryable> SelectAsync<TSource>(this IQueryable<TSource> source, string selector)
            where TSource : class
        {
            return await Task.Factory.StartNew(() =>
            {
                var result = source.Select(selector);

                return result;
            });
        }
    }
}
