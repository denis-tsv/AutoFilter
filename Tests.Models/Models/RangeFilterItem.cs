using System;

namespace Tests.Models.Models
{
    public class RangeFilterItem
    {
        public int Id { get; set; }
        public int? IntValue { get; set; }

        public RangeFilterNestedItem NestedItem { get; set; }
    }

    public class RangeFilterNestedItem
    {
        public int Id { get; set; }
        public DateTime? DateTimeValue { get; set; }
    }
}
