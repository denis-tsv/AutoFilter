using System;
using System.Reflection;

namespace AutoFilter
{
    public class FilterProperty<TFilter>
    {
        public Func<TFilter, object> PropertyValueGetter { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public FilterPropertyAttribute FilterPropertyAttribute { get; set; }        
    }
}
