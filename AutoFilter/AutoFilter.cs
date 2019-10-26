using AutoFilter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    public class AutoFilter<TSubject>
    {
        #region Filter

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

        private static ICollection<Expression<Func<TItem, bool>>> GetPropertiesExpressions<TItem, TFilter>(TFilter filter, bool inMemory)
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

        #endregion

        #region OrderBy

        public static IOrderedQueryable<TSubject> OrderByDescending(IQueryable<TSubject> query, string propertyName)
        {
            return Sort(query, propertyName, false);
        }

        public static IOrderedQueryable<TSubject> OrderBy(IQueryable<TSubject> query, string propertyName)
        {
            return Sort(query, propertyName, true);
        }

        public static IOrderedEnumerable<TSubject> OrderByDescending(IEnumerable<TSubject> query, string propertyName)
        {
            return Sort(query, propertyName, false);
        }

        public static IOrderedEnumerable<TSubject> OrderBy(IEnumerable<TSubject> query, string propertyName)
        {
            return Sort(query, propertyName, true);
        }

        private static MethodInfo ExpressionLambdaMethodInfo = typeof(Expression).GetMethods().First(x => x.Name == "Lambda");
        
        private static MethodInfo QueryableOrderByMethodInfo = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
        private static MethodInfo QueryableOrderByDescendingMethodInfo = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
        private static MethodInfo EnumerableOrderByMethodInfo = typeof(Enumerable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
        private static MethodInfo EnumerableOrderByDescendingMethodInfo = typeof(Enumerable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);

        private static IOrderedEnumerable<TSubject> Sort(IEnumerable<TSubject> query, string propertyName, bool orderByAsc)
        {
            (var expression, var property) = GetSortExpression(propertyName);

            //compileMethodInfo can not be cached in static field because Compile is method of generic Expression<> type
            var compileMethodInfo = TypeInfoCache.GetPublicMethods(expression.GetType())
                .First(x => x.Name == "Compile" && x.GetParameters().Length == 0);
            var func = compileMethodInfo.Invoke(expression, null);

            var methodInfo = orderByAsc ? EnumerableOrderByMethodInfo : EnumerableOrderByDescendingMethodInfo;
            var orderBy = methodInfo.MakeGenericMethod(typeof(TSubject), property.PropertyType);
            
            return (IOrderedEnumerable<TSubject>)orderBy.Invoke(query, new object[] { query, func });
        }

        private static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName, bool orderByAsc)
        {
            (var expression, var property) = GetSortExpression(propertyName);

            var methodInfo = orderByAsc ? QueryableOrderByMethodInfo : QueryableOrderByDescendingMethodInfo;
            var orderBy = methodInfo.MakeGenericMethod(typeof(TSubject), property.PropertyType);

            return (IOrderedQueryable<TSubject>)orderBy.Invoke(query, new object[] { query, expression });
        }
        
        private static (object expression, PropertyInfo property) GetSortExpression(string propertyName)
        {
            var property = TypeInfoCache
                .GetPublicProperties(typeof(TSubject))
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                    $"in type \"{typeof(TSubject)}\"");

            var parameter = Expression.Parameter(typeof(TSubject));
            var body = Expression.Property(parameter, propertyName);

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSubject), property.PropertyType);
            var lambda = ExpressionLambdaMethodInfo.MakeGenericMethod(delegateType);
            var expression = lambda.Invoke(null, new object[] { body, new[] { parameter } });
            return (expression, property);
        }

        #endregion        
    }
}
