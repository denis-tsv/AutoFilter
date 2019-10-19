using System;

namespace Tests.Models
{
    public class FilterConditionItem
    {
        public int Id { get; set; }

        public bool? BoolEqual { get; set; }

        public int IntGreaterOrEqual { get; set; }

        public DateTime? DateTimeLessOrEqual { get; set; }

        public decimal DecimalLess { get; set; }

        public double? DoubleGreater { get; set; }
    }
}
