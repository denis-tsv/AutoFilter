using AutoFilter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    public class AutoFilter
    {
        #region Filter

        public static IQueryable<TItem> Filter<TItem, TFilter>(IQueryable<TItem> query,
            TFilter filter,
            ComposeKind composeKind = ComposeKind.And)
        {
            var expression = GetExpression<TItem, TFilter>(filter, composeKind, false);
            if (expression == null) return query;

            return query.Where(expression);

        }

        public static IEnumerable<TItem> Filter<TItem, TFilter>(IEnumerable<TItem> query,
            TFilter filter,
            ComposeKind composeKind = ComposeKind.And)
        {
            var expression = GetExpression<TItem, TFilter>(filter, composeKind, true);
            if (expression == null) return query;

            return query.Where(expression.AsFunc());
        }

        protected static Expression<Func<TItem, bool>> GetExpression<TItem, TFilter>(TFilter filter, ComposeKind composeKind, bool inMemory)
        {
            var parameter = Expression.Parameter(typeof(TItem));
            var propertyExpressions = GetPropertiesExpressions(parameter, filter, inMemory);

            if (!propertyExpressions.Any())
            {
                return null;
            }

            var body = composeKind == ComposeKind.And
                ? propertyExpressions.Aggregate((c, n) => Expression.AndAlso(c, n))
                : propertyExpressions.Aggregate((c, n) => Expression.OrElse(c, n));

            var result = Expression.Lambda<Func<TItem, bool>>(body, parameter);

            return result;
        }

        private static ICollection<Expression> GetPropertiesExpressions<TFilter>(ParameterExpression parameter, TFilter filter, bool inMemory)
        {
            var filterProps = FilterPropertyCache.GetFilterProperties(filter.GetType())
                    .Where(x => x.PropertyInfo.GetValue(filter) != null)
                    .ToList();

            var res = new List<Expression>(capacity: filterProps.Count);
            foreach (var filterProperty in filterProps)
            {
                var expr = filterProperty.FilterPropertyAttribute.GetExpression(parameter, inMemory, filterProperty.PropertyInfo, filter);
                res.Add(expr);
            }
            return res;
        }

        #endregion

        #region OrderBy

        public static IOrderedQueryable<TItem> OrderByDescending<TItem>(IQueryable<TItem> query, string propertyName)
        {
            return Sort(query, propertyName, false);
        }

        public static IOrderedQueryable<TItem> OrderBy<TItem>(IQueryable<TItem> query, string propertyName)
        {
            return Sort(query, propertyName, true);
        }

        public static IOrderedEnumerable<TItem> OrderByDescending<TItem>(IEnumerable<TItem> query, string propertyName)
        {
            return Sort(query, propertyName, false);
        }

        public static IOrderedEnumerable<TItem> OrderBy<TItem>(IEnumerable<TItem> query, string propertyName)
        {
            return Sort(query, propertyName, true);
        }

        private static MethodInfo ExpressionLambdaMethodInfo = typeof(Expression).GetMethods().First(x => x.Name == "Lambda");
        
        private static MethodInfo QueryableOrderByMethodInfo = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
        private static MethodInfo QueryableOrderByDescendingMethodInfo = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
        private static MethodInfo EnumerableOrderByMethodInfo = typeof(Enumerable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
        private static MethodInfo EnumerableOrderByDescendingMethodInfo = typeof(Enumerable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);

        private static IOrderedEnumerable<TItem> Sort<TItem>(IEnumerable<TItem> query, string propertyName, bool orderByAsc)
        {
            (var expression, var property) = GetSortExpression<TItem>(propertyName);

            //compileMethodInfo can not be cached in static field because Compile is method of generic Expression<> type
            var compileMethodInfo = TypeInfoCache.GetPublicMethods(expression.GetType())
                .First(x => x.Name == "Compile" && x.GetParameters().Length == 0);
            var func = compileMethodInfo.Invoke(expression, null);

            var methodInfo = orderByAsc ? EnumerableOrderByMethodInfo : EnumerableOrderByDescendingMethodInfo;
            var orderBy = methodInfo.MakeGenericMethod(typeof(TItem), property.PropertyType);
            
            return (IOrderedEnumerable<TItem>)orderBy.Invoke(query, new object[] { query, func });
        }

        private static IOrderedQueryable<TItem> Sort<TItem>(IQueryable<TItem> query, string propertyName, bool orderByAsc)
        {
            (var expression, var property) = GetSortExpression<TItem>(propertyName);

            var methodInfo = orderByAsc ? QueryableOrderByMethodInfo : QueryableOrderByDescendingMethodInfo;
            var orderBy = methodInfo.MakeGenericMethod(typeof(TItem), property.PropertyType);

            return (IOrderedQueryable<TItem>)orderBy.Invoke(query, new object[] { query, expression });
        }
        
        private static (object expression, PropertyInfo property) GetSortExpression<TItem>(string propertyName)
        {
            var property = TypeInfoCache
                .GetPublicProperties(typeof(TItem))
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                    $"in type \"{typeof(TItem)}\"");

            var parameter = Expression.Parameter(typeof(TItem));
            var body = Expression.Property(parameter, propertyName);

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TItem), property.PropertyType);
            var lambda = ExpressionLambdaMethodInfo.MakeGenericMethod(delegateType);
            var expression = lambda.Invoke(null, new object[] { body, new[] { parameter } });
            return (expression, property);
        }

        #endregion        
    }
}
