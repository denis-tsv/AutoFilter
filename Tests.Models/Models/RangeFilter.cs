#nullable enable

using System;
using AutoFilter;
using AutoFilter.Filters;

namespace Tests.Models
{
    public class RangeFilter
    {
        public Range<int>? IntValue { get; set; }

        [NavigationProperty("NestedItem")]
        public Range<DateTime>? DateTimeValue { get; set; }
    }
}
