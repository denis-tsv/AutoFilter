using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AutoFilter
{
    public static class FilterPropertyCache
    {
        private static readonly ConcurrentDictionary<Type, FilterProperty[]> Cache = new ConcurrentDictionary<Type, FilterProperty[]>();

        public static bool IsEnabled { get; set; } = true;

        public static FilterProperty[] GetFilterProperties(Type type)
        {
            if (!IsEnabled) return CalcFilterProperties(type);

            return Cache.GetOrAdd(type, CalcFilterProperties(type));
        }

        private static FilterProperty[] CalcFilterProperties(Type type)
        {
            var props = TypeInfoCache
                .GetPublicProperties(type)
                .Select(x => new FilterProperty
                {
                    PropertyInfo = x,
                    FilterPropertyAttribute = x.GetCustomAttribute<FilterPropertyAttribute>()                        
                })
                .ToArray();

            //each property must have an attribute to generate an expression
            foreach(var x in props)
            {
                if (x.FilterPropertyAttribute == null)
                    x.FilterPropertyAttribute = new FilterPropertyAttribute();                
            }

            return props;
        }
    }
}
