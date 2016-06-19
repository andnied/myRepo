using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Utils
{
    public static class SimpleMapper
    {
        public static void MapProperties<T>(T source, T destination) where T : class
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                prop.SetValue(destination, prop.GetValue(source));
            }
        }
    }
}
