using System.Collections.Generic;

namespace Tests.Models
{
    public class EnumerableFilter
    {
        public int[] Int { get; set; }
        public int[] NullableInt { get; set; }
        
        public IEnumerable<TargetEnum> Enum { get; set; }
        public IEnumerable<TargetEnum> NullableEnum { get; set; }

        public List<string> String { get; set; }
    }
}
