using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutoFilter.Extensions
{
    public static class QuerableExtensions
    {
        public static IQueryable<TItem> Where<TItem, TNavigationProperty>(this IQueryable<TItem> queryable,
            Expression<Func<TItem, TNavigationProperty>> first, Expression<Func<TNavigationProperty, bool>> second)
        {
            return queryable.Where(first.Combine(second));
        }

        public static IQueryable<TItem> WhereAny<TItem, TCollectionNavigationProperty>(this IQueryable<TItem> queryable,
            Expression<Func<TItem, IEnumerable<TCollectionNavigationProperty>>> first, Expression<Func<TCollectionNavigationProperty, bool>> second)
        {
            return queryable.Where(first.Combine(second));
        }
    }
}
