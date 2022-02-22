#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutoFilter
{
    public class AutoFilter
    {
        #region Filter

        public static IQueryable<TItem> Filter<TItem, TFilter>(IQueryable<TItem> query,
            TFilter filter,
            ComposeKind composeKind = ComposeKind.And)
        {
            var expression = CreateExpression<TItem, TFilter>(filter, composeKind, false);
            if (expression == null) return query;

            return query.Where(expression);
        }

        public static IEnumerable<TItem> Filter<TItem, TFilter>(IEnumerable<TItem> query,
            TFilter filter,
            ComposeKind composeKind = ComposeKind.And)
        {
            var expression = CreateExpression<TItem, TFilter>(filter, composeKind, true);
            if (expression == null) return query;

            return query.Where(expression.Compile());
        }

        public static Expression<Func<TItem, bool>>? CreateExpression<TItem, TFilter>(TFilter filter, ComposeKind composeKind = ComposeKind.And, bool addNullChecks = false)
        {
            var parameter = Expression.Parameter(typeof(TItem), "x");
            var itemPropertyNames = ItemPropertyCache<TItem>.PropertyNames;
            var propertyExpressions = GetPropertiesExpressions(parameter, filter, addNullChecks, itemPropertyNames);

            if (!propertyExpressions.Any())
            {
                return null;
            }

            var body = composeKind == ComposeKind.And
                ? propertyExpressions.Aggregate(Expression.AndAlso)
                : propertyExpressions.Aggregate(Expression.OrElse);

            var result = Expression.Lambda<Func<TItem, bool>>(body, parameter);

            return result;
        }

        private static ICollection<Expression> GetPropertiesExpressions<TFilter>(ParameterExpression parameter, TFilter filter, bool inMemory, HashSet<string> itemPropertyNames)
        {
            var filterProps = FilterPropertyCache<TFilter>.FilterProperties
                .Where(x => x.HasAttribute || itemPropertyNames.Contains(x.PropertyInfo.Name))
                .Where(x => x.PropertyValueGetter.Invoke(filter) != null)
                .ToList();

            var result = new List<Expression>(capacity: filterProps.Count);
            foreach (var filterProperty in filterProps)
            {
                var propertyExpression = filterProperty.FilterPropertyAttribute.GetExpression(parameter, inMemory, filterProperty.PropertyInfo, filterProperty.PropertyValueGetter(filter)!, filter!);
                result.Add(propertyExpression);
            }
            return result;
        }

        #endregion

        #region OrderBy

        public static IOrderedQueryable<TItem> OrderByDescending<TItem>(IQueryable<TItem> query, string propertyName)
        {
            return SortQueryable(query, propertyName, false);
        }

        public static IOrderedQueryable<TItem> OrderBy<TItem>(IQueryable<TItem> query, string propertyName)
        {
            return SortQueryable(query, propertyName, true);
        }

        public static IOrderedEnumerable<TItem> OrderByDescending<TItem>(IEnumerable<TItem> query, string propertyName)
        {
            return SortEnumerable(query, propertyName, false);
        }

        public static IOrderedEnumerable<TItem> OrderBy<TItem>(IEnumerable<TItem> query, string propertyName)
        {
            return SortEnumerable(query, propertyName, true);
        }

        private static IOrderedEnumerable<TItem> SortEnumerable<TItem>(IEnumerable<TItem> query, string propertyName, bool orderByAsc)
        {
            var sortProperty = GetSortProperty<TItem>(propertyName);

            var result = orderByAsc ? 
                sortProperty.SortEnumerable(query) : 
                sortProperty.SortEnumerableDesc(query);

            return result;
        }

        private static IOrderedQueryable<TItem> SortQueryable<TItem>(IQueryable<TItem> query, string propertyName, bool orderByAsc)
        {
            var sortProperty = GetSortProperty<TItem>(propertyName);

            var result = orderByAsc ? 
                sortProperty.SortQueryable(query) : 
                sortProperty.SortQueryableDesc(query);
            
            return result;
        }
        
        private static ISortProperty<TItem> GetSortProperty<TItem>(string propertyName)
        {
            var propertyExists = SortPropertyCache<TItem>.SortProperties.TryGetValue(propertyName, out var result);
            if (!propertyExists)
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" in type \"{typeof(TItem)}\"");

            return result!;
        }

        #endregion        
    }
}
