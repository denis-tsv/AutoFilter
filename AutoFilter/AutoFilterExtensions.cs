using System.Collections.Generic;
using System.Linq;
using AutoFilterClass = AutoFilter.AutoFilter;

namespace AutoFilter
{    
    public static class AutoFilterExtensions
    {
        public static IQueryable<TItem> AutoFilter<TItem, TFilter>(
            this IQueryable<TItem> query, TFilter filter, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = AutoFilterClass.Filter(query, filter, composeKind);
            return filtered;            
        }

        public static IEnumerable<TItem> AutoFilter<TItem, TFilter>(
            this IEnumerable<TItem> query, TFilter filter, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = AutoFilterClass.Filter(query, filter, composeKind);
            return filtered;
        }

        public static IOrderedQueryable<TItem> OrderBy<TItem>(this IQueryable<TItem> query, string propertyName)
        {
            var result = AutoFilterClass.OrderBy(query, propertyName);
            return result;
        }

        public static IOrderedQueryable<TItem> OrderByDescending<TItem>(this IQueryable<TItem> query, string propertyName)
        {
            var result = AutoFilterClass.OrderByDescending(query, propertyName);
            return result;
        }

        public static IOrderedEnumerable<TItem> OrderBy<TItem>(this IEnumerable<TItem> query, string propertyName)
        {
            var result = AutoFilterClass.OrderBy(query, propertyName);
            return result;
        }

        public static IOrderedEnumerable<TItem> OrderByDescending<TItem>(this IEnumerable<TItem> query, string propertyName)
        {
            var result = AutoFilterClass.OrderByDescending(query, propertyName);
            return result;
        }
    }
    
}
