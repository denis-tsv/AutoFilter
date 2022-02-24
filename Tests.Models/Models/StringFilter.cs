using AutoFilter;

namespace Tests.Models
{
    public class StringFilter
    {
        public string NoAttribute { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.Contains, IgnoreCase = false)]
        public string ContainsCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.Contains, IgnoreCase = true)]
        public string ContainsIgnoreCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.StartsWith, IgnoreCase = false)]
        public string StartsWithCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.StartsWith, IgnoreCase = true)]
        public string StartsWithIgnoreCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.EndsWith, IgnoreCase = false)]
        public string EndsWithCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.EndsWith, IgnoreCase = true)]
        public string EndsWithIgnoreCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.Equals, IgnoreCase = false)]
        public string EqualsCase { get; set; }

        [FilterProperty(StringFilter = StringFilterCondition.Equals, IgnoreCase = true)]
        public string EqualsIgnoreCase { get; set; }

        [FilterProperty(TargetPropertyName = "PropertyName")]
        public string TargetStringProperty { get; set; }

        [FilterProperty(TargetPropertyName = "PropertyName", StringFilter = StringFilterCondition.Contains, IgnoreCase = true)]
        public string TargetStringPropertyContainsIgnoreCase { get; set; }        
    }
}
