using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    public class SortPropertyCache<TFilter>
    {
        public static readonly IReadOnlyDictionary<string, ISortProperty<TFilter>> SortProperties;

        static SortPropertyCache()
        {
            var createSortPropertyMethodInfo = typeof(SortPropertyCache<>).MakeGenericType(typeof(TFilter))
                .GetMethod(nameof(CreateSortProperty), BindingFlags.Static | BindingFlags.NonPublic);
            
            SortProperties = typeof(TFilter)
                .GetProperties()
                .ToDictionary(
                    propertyInfo => propertyInfo.Name,
                    propertyInfo =>
                    {
                        var createSortProperty = createSortPropertyMethodInfo!.MakeGenericMethod(propertyInfo.PropertyType);
                        var res = createSortProperty.Invoke(null, new object[] { propertyInfo });
                        return (ISortProperty<TFilter>)res;
                    }
                );
        }

        private static ISortProperty<TFilter> CreateSortProperty<TProperty>(PropertyInfo propertyInfo)
        {
            var parameter = Expression.Parameter(typeof(TFilter), "x");
            var body = Expression.Property(parameter, propertyInfo);

            var lambda = Expression.Lambda<Func<TFilter, TProperty>>(body, parameter);
            var result = new SortProperty<TFilter, TProperty>(lambda, lambda.Compile());

            return result;
        }
    }
}
