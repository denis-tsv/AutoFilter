using AutoFilter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutoFilter
{
    public class AutoFilter<TSubject>
    {
        public static IQueryable<TSubject> Filter<TFilter>(IQueryable<TSubject> query,
            TFilter filter,
            ComposeKind composeKind = ComposeKind.And)
        {
            var expression = GetExpression<TSubject, TFilter>(filter, composeKind, false);
            if (expression == null) return query;

            return query.Where(expression);

        }

        public static IEnumerable<TSubject> Filter<TFilter>(IEnumerable<TSubject> query,
            TFilter filter,
            ComposeKind composeKind = ComposeKind.And)
        {
            var expression = GetExpression<TSubject, TFilter>(filter, composeKind, true);
            if (expression == null) return query;

            return query.Where(expression.AsFunc());
        }

        public static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName)
        {
            (string, bool) GetSorting()
            {
                var arr = propertyName.Split('.');
                if (arr.Length == 1)
                    return (arr[0], false);
                var sort = arr[1];
                if (string.Equals(sort, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], false);
                if (string.Equals(sort, "DESC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], true);
                return (arr[0], false);
            }

            var (name, isDesc) = GetSorting();
            propertyName = name;

            var property = TypeInfoCache
                .GetPublicProperties(typeof(TSubject))
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                    $"in type \"{typeof(TSubject)}\"");

            var parameter = Expression.Parameter(typeof(TSubject));
            var body = Expression.Property(parameter, propertyName);

            var lambda = TypeInfoCache
                .GetPublicMethods(typeof(Expression))
                .First(x => x.Name == "Lambda");

            lambda = lambda.MakeGenericMethod(typeof(Func<,>)
                .MakeGenericType(typeof(TSubject), property.PropertyType));

            var expression = lambda.Invoke(null, new object[] { body, new[] { parameter } });

            var methodName = isDesc ? "OrderByDescending" : "OrderBy";

            var orderBy = typeof(Queryable)
                .GetMethods()
                .First(x => x.Name == methodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TSubject), property.PropertyType);

            return (IOrderedQueryable<TSubject>)orderBy.Invoke(query, new object[] { query, expression });
        }

        protected static Expression<Func<TItem, bool>> GetExpression<TItem, TFilter>(TFilter filter, ComposeKind composeKind, bool inMemory)
        {
            var propertyExpressions = GetPropertiesExpressions<TItem, TFilter>(filter, inMemory);

            if (!propertyExpressions.Any())
            {
                return null;
            }

            var result = composeKind == ComposeKind.And
                ? propertyExpressions.Aggregate((c, n) => c.And(n))
                : propertyExpressions.Aggregate((c, n) => c.Or(n));

            return result;
        }

        private static ICollection< Expression<Func<TItem, bool>> > GetPropertiesExpressions<TItem, TFilter>(TFilter filter, bool inMemory)
        {
            var filterProps = FilterPropertyCache.GetFilterProperties(filter.GetType())
                    .Where(x => x.PropertyInfo.GetValue(filter) != null)
                    .ToList();

            var res = new List<Expression<Func<TItem, bool>>>(capacity: filterProps.Count);
            foreach (var filterProperty in filterProps)
            {
                var expr = filterProperty.FilterPropertyAttribute.GetExpression<TItem>(inMemory, filterProperty.PropertyInfo, filter);
                res.Add(expr); 
            }
            return res;            
        }        
    }
}
