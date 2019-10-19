using System;

namespace AutoFilter.Filters
{
    public class TargetPropertyNameAttribute : FilterPropertyAttribute
    {
        public TargetPropertyNameAttribute(string propertyName)
        {
            TargetPropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }        
    }
}
