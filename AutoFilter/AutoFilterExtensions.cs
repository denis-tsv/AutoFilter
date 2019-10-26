using System.Collections.Generic;
using System.Linq;

namespace AutoFilter
{    
    public static class AutoFilterExtensions
    {
        public static IQueryable<TSubject> AutoFilter<TSubject, TFilter>(
            this IQueryable<TSubject> query, TFilter filter, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = AutoFilter<TSubject>.Filter(query, filter, composeKind);
            return filtered;            
        }

        public static IEnumerable<TSubject> AutoFilter<TSubject, TFilter>(
            this IEnumerable<TSubject> query, TFilter filter, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = AutoFilter<TSubject>.Filter(query, filter, composeKind);
            return filtered;
        }

        public static IOrderedQueryable<TSubject> OrderBy<TSubject>(this IQueryable<TSubject> query, string propertyName)
        {
            var result = AutoFilter<TSubject>.OrderBy(query, propertyName);
            return result;
        }

        public static IOrderedQueryable<TSubject> OrderByDescending<TSubject>(this IQueryable<TSubject> query, string propertyName)
        {
            var result = AutoFilter<TSubject>.OrderByDescending(query, propertyName);
            return result;
        }

        public static IOrderedEnumerable<TSubject> OrderBy<TSubject>(this IEnumerable<TSubject> query, string propertyName)
        {
            var result = AutoFilter<TSubject>.OrderBy(query, propertyName);
            return result;
        }

        public static IOrderedEnumerable<TSubject> OrderByDescending<TSubject>(this IEnumerable<TSubject> query, string propertyName)
        {
            var result = AutoFilter<TSubject>.OrderByDescending(query, propertyName);
            return result;
        }
    }
    
}
