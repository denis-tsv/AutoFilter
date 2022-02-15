using System;
using System.Reflection;

namespace AutoFilter
{
    public class FilterProperty<TFilter>
    {
        public bool HasAttribute { get; set; }
        public Func<TFilter, object> PropertyValueGetter { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public FilterPropertyAttribute FilterPropertyAttribute { get; set; }        
    }
}
