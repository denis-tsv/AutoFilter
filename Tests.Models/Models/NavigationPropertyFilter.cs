using AutoFilter;
using AutoFilter.Filters;

namespace Tests.Models
{
    public class NavigationPropertyFilter
    {
        [NavigationProperty("NestedItem", TargetPropertyName = "String")]
        public string StringFilter { get; set; }

        [NavigationProperty("NestedItem", TargetPropertyName = "Int")]
        public int? Int { get; set; }

        [NavigationProperty("NestedItem", TargetPropertyName = "NullableInt", FilterCondition = FilterCondition.GreaterOrEqual)]
        public int? NullableIntFilter { get; set; }        
    }
}
