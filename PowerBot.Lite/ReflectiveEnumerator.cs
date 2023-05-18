using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerBot.Lite
{
    internal static class ReflectiveEnumerator
    {
        public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
        {
            var classes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x =>
                    x.GetTypes()
                        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
                        .ToList()
                ).ToList();

            return classes;
        }
    }
}
