using System.Reflection;

namespace AutoFilter
{
    public class FilterProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public FilterPropertyAttribute FilterPropertyAttribute { get; set; }        
    }
}
