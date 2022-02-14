using System;
using System.Collections.Generic;
using Tests.Models.Models;

namespace Tests.Data
{
    public class RangeData
    {
        public static List<RangeFilterItem> Items = new List<RangeFilterItem>
        {
            new RangeFilterItem { IntValue = 10 },
            new RangeFilterItem { IntValue = 20},
            new RangeFilterItem { IntValue = 100},

            new RangeFilterItem
            {
                NestedItem = new RangeFilterNestedItem
                {
                    DateTimeValue = new DateTime(2020, 01, 01)
                }
            },
            new RangeFilterItem
            {
                NestedItem = new RangeFilterNestedItem
                {
                    DateTimeValue = new DateTime(2021, 01, 01)
                }
            },
            new RangeFilterItem
            {
                NestedItem = new RangeFilterNestedItem
                {
                    DateTimeValue = new DateTime(2022, 01, 01)
                }
            },
        };
    }
}
