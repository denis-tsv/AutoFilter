using System.Collections.Generic;

namespace Tests.Models
{
    public class NavigationPropertyItem
    {
        public int Id { get; set; }


        public NestedItem NestedItem { get; set; }
    }

    public class NestedItem
    {
        public int Id { get; set; }
        public string String { get; set; }
        public int Int { get; set; }
        public int? NullableInt { get; set; }

        public ICollection<NavigationPropertyItem> NavigationPropertyItems { get; set; }
    }
}
