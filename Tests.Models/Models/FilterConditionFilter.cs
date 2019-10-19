using AutoFilter;
using System;

namespace Tests.Models
{
    public class FilterConditionFilter
    {
        [FilterProperty(FilterCondition = FilterCondition.Equal)]
        public bool? BoolEqual { get; set; }

        [FilterProperty(FilterCondition = FilterCondition.GreaterOrEqual)]
        public int? IntGreaterOrEqual { get; set; }

        [FilterProperty(FilterCondition = FilterCondition.LessOrEqual)]
        public DateTime? DateTimeLessOrEqual { get; set; }

        [FilterProperty(FilterCondition = FilterCondition.Less)]
        public decimal? DecimalLess { get; set; }

        [FilterProperty(FilterCondition = FilterCondition.Greater)]
        public double? DoubleGreater { get; set; }
    }
}
