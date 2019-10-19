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
            => AutoFilter<TSubject>.Sort(query, propertyName);
    }
    
}
