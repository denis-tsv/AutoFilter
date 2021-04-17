using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    public interface ISortProperty<TItem>
    {
        IOrderedQueryable<TItem> SortQueryable(IQueryable<TItem> items);
        IOrderedQueryable<TItem> SortQueryableDesc(IQueryable<TItem> items);

        IOrderedEnumerable<TItem> SortEnumerable(IEnumerable<TItem> items);
        IOrderedEnumerable<TItem> SortEnumerableDesc(IEnumerable<TItem> items);
    }

    public class SortProperty<TItem, TProperty> : ISortProperty<TItem>
    {
        public PropertyInfo PropertyInfo { get; set; }
        public Expression<Func<TItem, TProperty>> PropertyExpression { get; set; }
        public Func<TItem, TProperty> PropertyDelegate { get; set; }


        public IOrderedQueryable<TItem> SortQueryable(IQueryable<TItem> items)
        {
            return items.OrderBy(PropertyExpression);
        }

        public IOrderedQueryable<TItem> SortQueryableDesc(IQueryable<TItem> items)
        {
            return items.OrderByDescending(PropertyExpression);
        }

        public IOrderedEnumerable<TItem> SortEnumerable(IEnumerable<TItem> items)
        {
            return items.OrderBy(PropertyDelegate);
        }

        public IOrderedEnumerable<TItem> SortEnumerableDesc(IEnumerable<TItem> items)
        {
            return items.OrderByDescending(PropertyDelegate);
        }
    }
}
