using System;
using System.Collections.Concurrent;

namespace AutoFilter.Filters.Convert
{
    public class FilterValueConvertersCache
    {
        public static bool IsEnabled { get; set; } = true;

        private static readonly ConcurrentDictionary<Type, IFilterValueConverter> Cache = new();

        public static IFilterValueConverter GetConverter(Type type)
        {
            if (!IsEnabled) return (IFilterValueConverter)Activator.CreateInstance(type);

            return Cache.GetOrAdd(type, (IFilterValueConverter)Activator.CreateInstance(type));
        }
    }
}
