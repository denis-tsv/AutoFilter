using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    public static class FilterPropertyCache<TFilter>
    {
        public static readonly FilterProperty<TFilter>[] FilterProperties;

        static FilterPropertyCache()
        {
            FilterProperties = typeof(TFilter)
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .Where(x => x.GetCustomAttribute<NotAutoFilteredAttribute>() == null)
                .Select(x => new FilterProperty<TFilter>
                {
                    PropertyInfo = x,
                    FilterPropertyAttribute = x.GetCustomAttribute<FilterPropertyAttribute>(),
                    PropertyValueGetter = CreateValueGetter(x)
                })
                .ToArray();

            //each property must have an attribute to generate an expression
            foreach (var x in FilterProperties)
            {
                if (x.FilterPropertyAttribute == null)
                    x.FilterPropertyAttribute = new FilterPropertyAttribute();
                else
                    x.HasAttribute = true;
            }
        }

        private static Func<TFilter, object> CreateValueGetter(PropertyInfo propertyInfo)
        {
            var parameter = Expression.Parameter(typeof(TFilter), "x");
            Expression body = Expression.Property(parameter, propertyInfo);
            if (propertyInfo.PropertyType.IsValueType)
            {
                body = Expression.Convert(body, typeof(object));
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TFilter), typeof(object));

            var lambdaMethodInfo = typeof(Expression).GetMethods().First(x => x.Name == nameof(Expression.Lambda)).MakeGenericMethod(delegateType);
            var lambdaExpression = lambdaMethodInfo.Invoke(null, new object[] { body, new[] { parameter } });
            var lambda = (Expression<Func<TFilter, object>>) lambdaExpression;
            return lambda.Compile();
        }
    }
}
