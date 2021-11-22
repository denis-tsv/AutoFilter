using AutoFilter.Filters.Convert;
using System;

namespace Tests.Models
{
    public class StringToEnumConverter : IFilterValueConverter
    {
        public object Convert(object value)
        {
            return Enum.Parse(typeof(TargetEnum), (string)value);
        }
    }

    public class InvalidCaseFilter
    {
        [ConvertFilter(typeof(StringToEnumConverter), TargetPropertyName = "TargetEnum")]
        public string EnumTargetType { get; set; }

        [ConvertFilter(typeof(StringToEnumConverter), TargetPropertyName = "TargetEnum")]
        public string NotExistsValue { get; set; }

        public string NotExistsProperty { get; set; }
    }
}
