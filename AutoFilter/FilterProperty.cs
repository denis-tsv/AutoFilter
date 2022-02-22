#nullable enable

using System;
using System.Reflection;

namespace AutoFilter
{
    public class FilterProperty<TFilter>
    {
        public FilterProperty(PropertyInfo propertyInfo, FilterPropertyAttribute filterPropertyAttribute, Func<TFilter, object?> propertyValueGetter, bool hasAttribute)
        {
            HasAttribute = hasAttribute;
            PropertyInfo = propertyInfo;
            FilterPropertyAttribute = filterPropertyAttribute;
            PropertyValueGetter = propertyValueGetter;
        }

        public bool HasAttribute { get; }
        public Func<TFilter, object?> PropertyValueGetter { get; }
        public PropertyInfo PropertyInfo { get; }
        public FilterPropertyAttribute FilterPropertyAttribute { get; }
    }
}
