using ContactsBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.SqlRepository
{
    public class Class1
    {
        public static void Test()
        {
            var type = typeof(Contact);
            var prop = type.GetProperty("FirstName");
            var prop2 = type.GetProperty("LastName");

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
            var test = Expression.Or(propertyAccess, Expression.MakeMemberAccess(parameter, prop2));
            var whereExp = Expression.Lambda(test, parameter);
            
        }
    }
}
