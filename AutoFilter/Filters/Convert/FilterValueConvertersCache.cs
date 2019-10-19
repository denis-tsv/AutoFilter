using System;
using System.Collections.Concurrent;

namespace AutoFilter.Filters.Convert
{
    public class FilterValueConvertersCache
    {
        public static bool IsEnabled { get; set; } = true;

        private static readonly ConcurrentDictionary<Type, IFilverValueConverter> Cache =
            new ConcurrentDictionary<Type, IFilverValueConverter>();

        public static IFilverValueConverter GetConverter(Type type)
        {
            if (!IsEnabled) return (IFilverValueConverter)Activator.CreateInstance(type);

            return Cache.GetOrAdd(type, (IFilverValueConverter)Activator.CreateInstance(type));
        }
    }
}
