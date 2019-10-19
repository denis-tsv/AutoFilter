using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AutoFilter
{
    public static class TypeInfoCache
    {
        public static bool IsEnabled { get; set; } = true;

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        private static readonly ConcurrentDictionary<Type, MethodInfo[]> MethodsCache =
            new ConcurrentDictionary<Type, MethodInfo[]>();


        public static PropertyInfo[] GetPublicProperties(Type type)
        {
            if (!IsEnabled) return CalcPublicProperties(type);

            return PropertiesCache.GetOrAdd(type, CalcPublicProperties(type));
        }

        private static PropertyInfo[] CalcPublicProperties(Type type)
        {
            return type
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();
        }

        public static MethodInfo[] GetPublicMethods(Type type)
        {
            if (!IsEnabled) return CalcPublicMethods(type);

            return MethodsCache.GetOrAdd(type, CalcPublicMethods(type));
        }

        private static MethodInfo[] CalcPublicMethods(Type type)
        {
            return type
                .GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToArray();
        }
    }
}
