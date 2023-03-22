using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AutoFilter;

public class EnumerablePropertyCache
{
    private static readonly ConcurrentDictionary<PropertyInfo, (MethodInfo methodInfo, Type elementType)> Cache = new ();

    public static (MethodInfo ContainsMethodInfo, Type ElementType) GetInfo(PropertyInfo propertyInfo)
    {
        return Cache.GetOrAdd(propertyInfo, pi =>
        {
            var containsEnumerable = typeof(Enumerable)
                .GetMethods()
                .First(x => x.Name == nameof(Enumerable.Contains) && x.GetParameters().Length == 2);

            var itemType = !pi.PropertyType.IsArray
                ? pi.PropertyType.GenericTypeArguments.First()
                : pi.PropertyType.GetElementType();

            var genericMethodInfo = containsEnumerable.MakeGenericMethod(itemType);

            return new(genericMethodInfo, itemType);
        });
    }
}