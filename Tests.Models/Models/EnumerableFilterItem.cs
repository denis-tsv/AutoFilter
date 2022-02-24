namespace Tests.Models
{
    public class EnumerableFilterItem
    {
        public int Id { get; set; }

        public int Int { get; set; }
        public int? NullableInt { get; set; }

        public TargetEnum Enum { get; set; }
        public TargetEnum? NullableEnum { get; set; }

        public string String { get; set; }
    }
}
