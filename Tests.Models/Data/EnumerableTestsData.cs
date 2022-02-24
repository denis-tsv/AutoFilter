using System.Collections.Generic;
using Tests.Models;

namespace Tests.Data
{
    public class EnumerableTestsData
    {
        public static List<EnumerableFilterItem> Items = new List<EnumerableFilterItem>
        {
            new EnumerableFilterItem { Int = 0 },
            new EnumerableFilterItem { Int = 1 },
            new EnumerableFilterItem { Int = 10 },

            new EnumerableFilterItem { NullableInt = 0 },
            new EnumerableFilterItem { NullableInt = 1 },
            new EnumerableFilterItem { NullableInt = 10 },

            new EnumerableFilterItem { Enum = TargetEnum.First},
            new EnumerableFilterItem { Enum = TargetEnum.Default},

            new EnumerableFilterItem { NullableEnum = TargetEnum.First},
            new EnumerableFilterItem { NullableEnum = TargetEnum.Default},

            new EnumerableFilterItem { String = "S1"},
            new EnumerableFilterItem { String = "S2"},
            new EnumerableFilterItem { String = "S3"}

        };
    }
}
