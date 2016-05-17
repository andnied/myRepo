using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ManualContainer
{
    public class Resolver
    {
        private static IDictionary<Type, Type> DependencyMap = new Dictionary<Type, Type>();
        
        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private static object Resolve(Type type)
        {
            Type resolvedType = null;

            if (DependencyMap.Values.Any(m => m == (resolvedType = DependencyMap[type])))
            {
                ParameterInfo[] constructorParameters = null;
                var firstConstructor = resolvedType.GetConstructors().First();

                if ((constructorParameters = firstConstructor.GetParameters()).Count() == 0)
                    return Activator.CreateInstance(resolvedType);

                var parameters = new List<object>();
                foreach (var par in constructorParameters)
                {
                    parameters.Add(Resolve(par.ParameterType));
                }

                return firstConstructor.Invoke(parameters.ToArray());
            }
            else
                return null;
        }

        public static void RegisterDependency<TFrom, TTo>()
        {
            DependencyMap.Add(typeof(TFrom), typeof(TTo));
        }
    }
}
