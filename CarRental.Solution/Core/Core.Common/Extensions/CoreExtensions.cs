using Core.Common.Core;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Extensions
{
    public static class CoreExtensions
    {
        public static bool IsNavigable(this PropertyInfo source)
        {
            return !((source.GetCustomAttributes(typeof(NotNavigableAttribute), true)).Any());
        }

        public static bool IsNavigable(this ObjectBase source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName).IsNavigable();
        }

        public static bool IsNavigable(this ObjectBase source, Expression<Func<ObjectBase>> propertyExpression)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            return source.IsNavigable(propertyName);
        }
    }
}
