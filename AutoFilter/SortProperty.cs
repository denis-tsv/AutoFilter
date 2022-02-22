using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        private readonly Expression<Func<TItem, TProperty>> _propertyExpression;
        private readonly Func<TItem, TProperty> _propertyDelegate;

        public SortProperty(Expression<Func<TItem, TProperty>> propertyExpression, Func<TItem, TProperty> propertyDelegate)
        {
            _propertyExpression = propertyExpression;
            _propertyDelegate = propertyDelegate;
        }

        public IOrderedQueryable<TItem> SortQueryable(IQueryable<TItem> items)
        {
            return items.OrderBy(_propertyExpression);
        }

        public IOrderedQueryable<TItem> SortQueryableDesc(IQueryable<TItem> items)
        {
            return items.OrderByDescending(_propertyExpression);
        }

        public IOrderedEnumerable<TItem> SortEnumerable(IEnumerable<TItem> items)
        {
            return items.OrderBy(_propertyDelegate);
        }

        public IOrderedEnumerable<TItem> SortEnumerableDesc(IEnumerable<TItem> items)
        {
            return items.OrderByDescending(_propertyDelegate);
        }
    }
}
